using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Models
{
    
    public class User
    {
        public static UserState ParseState(string state)
        {
            switch(state)
            {
                case "OK": return UserState.OK;
                case "BANNED": return UserState.BANNED;
                case "PENDING": return UserState.PENDING;
                case "DELETED": return UserState.DELETED;
            }
            return UserState.OK;
        }
        public int id;
        public string username;
        public string password;
        public int access;
        public string email;
        public UserState userState;
        // We must erase hash for enabled login
        public string hash;
        public DateTime registrationTime;
        public string registrationIP;
        public DateTime lastLoginTime;
        public string lastLoginIP;
        public string passwordHash;

        public enum UserState { OK, BANNED, PENDING, DELETED }
    }
}
