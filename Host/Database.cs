using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
//Add MySql Library
using MySql.Data.MySqlClient;

namespace Host
{
    class Database
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private object insertLock = new object();
        private static Database instance;
        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }
        //Constructor
        private Database()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = AppSettings.Default.server;
            database = AppSettings.Default.database;
            uid = AppSettings.Default.user;
            password = AppSettings.Default.password;
            var connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + "; convert zero datetime=True; CharSet=utf8;";

            connection = new MySqlConnection(connectionString);
        }


        //open connection to database
        private bool OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open) return true;
            try
            {
                connection.Open();
                
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                       Logger.Log("Cannot connect to database server. Ex: " +ex.Message, Logger.SEVERITY.FATAL);
                        break;

                    case 1045:
                        Logger.Log("Cannot connect to database server. Invalid Login.", Logger.SEVERITY.FATAL);
                        break;
                    default:
                        Logger.Log("Cannot connect to database server. Ex: " + ex.Message, Logger.SEVERITY.FATAL); break;

                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Logger.Log("Cannot close connection Ex: " + ex.Message, Logger.SEVERITY.FATAL);
                return false;
            }
        }

        //Insert statement
        public int InsertQuery(string table, string[] columns, string[] values)
        {
            
            if (connection.State != System.Data.ConnectionState.Open) this.OpenConnection();

            string commandText = "INSERT INTO ";
            commandText += table + " (";
            for (int i = 0; i < columns.Length; i++)
            {
                commandText += columns[i];
                if (i < columns.Length - 1) commandText += ", ";
            }
            commandText += ") VALUES (";
            for (int i = 0; i < values.Length; i++)
            {
                commandText += "'" + values[i] + "'";
                if (i < values.Length - 1) commandText += ", ";
            }
            commandText += ")";
            lock (insertLock)
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(commandText, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();
                    return (Int32)cmd.LastInsertedId;

                }
            }
            return -1;
        }

        //Update statement
        public int UpdateQuery(string table, string what, string where)
        {
            string query = "UPDATE " + table + " SET " + what + " WHERE " + where;

            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                return cmd.ExecuteNonQuery();

  
            }
            return -1;
        }

        //Delete statement
        public int DeleteQuery(string table, string where)
        {
            string query = "DELETE FROM " + table +" WHERE " + where;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                return cmd.ExecuteNonQuery();
            }
            return -1;
        }

        //Select statement
        public List<string> SelectQuery(string what, string table, string where = "", string limit = "")
        {
            string query;
            if (where != "")
            {
                if (limit != "")
                {
                    query = "SELECT " + what + " FROM " + table + " WHERE " + where + " LIMIT " + limit;
                }
                else
                {
                    query = "SELECT " + what + " FROM " + table + " WHERE " + where;

                }
            }
            else
            {
                query = "SELECT " + what + " FROM "  + table;
            }
            List<string> list = new List<string>();
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                var columns = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i));

                }
                
                while (reader.Read())
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        list.Add((reader[columns[i]].ToString()));
                    }
                }
                reader.Close();
            }
            return list;
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
            //    MessageBox.Show("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
             //   MessageBox.Show("Error , unable to Restore!");
            }
        }
    }
}
