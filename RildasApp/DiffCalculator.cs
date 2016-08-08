using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RildasApp
{

    public enum Action
    {
        ADD,
        DELETE,
        CHANGE,
        SAME
    };


    public struct Difference
    {
        public int? lineNumberFile1;
        public int? lineNumberFile2;
        public bool timingChanged;
        public Action action;

        public Difference(int? file1, int? file2, bool changed, Action act)
        {
            lineNumberFile1 = file1;
            lineNumberFile2 = file2;
            timingChanged = changed;
            action = act;
        }
    }


    public static class DiffCalculator
    {
        public struct timeInfo
        {
            public timeInfo(int Date, short Line, short MayChan)
            {
                date = Date;
                line = Line;
                MaybeChanged = MayChan;
            }

            public int date;
            public short line;
            public short MaybeChanged;

            internal void SetMaybeChanged(short v)
            {
                MaybeChanged = v;
            }
        }

        public static List<Difference> Find(MemoryStream MS1, MemoryStream MS2)
        {

            Dictionary<string, List<timeInfo>> addedInfo = new Dictionary<string, List<timeInfo>>();
            List<Difference> diffs = new List<Difference>();

            var sr1 = new StreamReader(MS1);
            string myStr1;
            short linenum = 1;
            while ((myStr1 = sr1.ReadLine()) != "[Events]")
            {
                linenum++;
            }
            linenum++;
            myStr1 = sr1.ReadLine();
            while ((myStr1 = sr1.ReadLine()) != null)
            {
                linenum++;

                string part1 = myStr1.Split(',')[1];
                string part2 = myStr1.Split(',')[2];

                string regex = @"^[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,";
                string output = Regex.Replace(myStr1, regex, String.Empty);
                regex = @"\{([^}]*)\}";
                output = Regex.Replace(output, regex, String.Empty);

                DateTime dt1 = DateTime.ParseExact(part1, "H:mm:ss.FFF", System.Globalization.CultureInfo.CurrentCulture);
                DateTime dt2 = DateTime.ParseExact(part2, "H:mm:ss.FFF", System.Globalization.CultureInfo.CurrentCulture);

                int date = ((dt1.Hour*60 + dt1.Minute)*60 + dt1.Second)*1000 + dt1.Millisecond;
                int date2 = ((dt2.Hour*60 + dt2.Minute)*60 + dt2.Second)*1000 + dt2.Millisecond;

                List<timeInfo> item;

                if (!addedInfo.TryGetValue(output, out item))
                {
                    List<timeInfo> tempList = new List<timeInfo> {new timeInfo(date + date2, linenum, -1)};
                    addedInfo.Add(output, tempList);
                }
                else
                {
                    item.Add(new timeInfo(date + date2, linenum, -1));
                }
            }
            linenum = 1;
            var sr2 = new StreamReader(MS2);

            while ((myStr1 = sr2.ReadLine()) != "[Events]")
            {
                linenum++;
            }
            linenum++;
            myStr1 = sr2.ReadLine();

            while ((myStr1 = sr2.ReadLine()) != null)
            {
                linenum++;

                string part1 = myStr1.Split(',')[1];
                string part2 = myStr1.Split(',')[2];

                string regex = @"^[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,";
                string output = Regex.Replace(myStr1, regex, String.Empty);
                regex = @"\{([^}]*)\}";
                output = Regex.Replace(output, regex, String.Empty);

                DateTime dt1 = DateTime.ParseExact(part1, "H:mm:ss.FFF", System.Globalization.CultureInfo.CurrentCulture);
                DateTime dt2 = DateTime.ParseExact(part2, "H:mm:ss.FFF", System.Globalization.CultureInfo.CurrentCulture);

                int date = ((dt1.Hour*60 + dt1.Minute)*60 + dt1.Second)*1000 + dt1.Millisecond;
                int date2 = ((dt2.Hour*60 + dt2.Minute)*60 + dt2.Second)*1000 + dt2.Millisecond;


                List<timeInfo> item;

                if (!addedInfo.TryGetValue(output, out item))
                {
                    diffs.Add(new Difference(null, linenum, false, Action.ADD));
                }
                else
                {
                    bool hasBeenSet = false;
                    for (int a = 0; a < item.Count; a++)
                    {


                        int d1 = item[a].date;



                        if (d1 == date + date2)
                        {
                            diffs.Add(new Difference(item[a].line, linenum, false, Action.SAME));
                            hasBeenSet = true;
                            if (item[a].MaybeChanged != -1)
                            {
                                hasBeenSet = false;
                                for (int b = 0; b < item.Count; b++)
                                {
                                    if (item[b].MaybeChanged == -1)
                                    {
                                        item[b].SetMaybeChanged(item[a].MaybeChanged);
                                        hasBeenSet = true;
                                    }
                                }
                                if (hasBeenSet == false)
                                {



                                    diffs.Add(new Difference(null, item[a].MaybeChanged, false, Action.ADD));
                                    hasBeenSet = true;
                                }
                            }


                            item.RemoveAt(a);
                            break;
                        }
                    }
                    if (hasBeenSet == false)
                    {
                        for (int a = 0; a < item.Count; a++)
                        {
                            if (item[a].MaybeChanged == -1)
                            {
                                item[a].SetMaybeChanged(linenum);
                                hasBeenSet = true;
                                break;
                            }
                        }
                    }
                    if (hasBeenSet == false)
                    {
                        diffs.Add(new Difference(null, linenum, false, Action.ADD));
                    }

                }

            }

            foreach (KeyValuePair<string, List<timeInfo>> entry in addedInfo)
            {
                for (int i = 0; i < entry.Value.Count; i++)
                {
                    if (entry.Value[i].MaybeChanged != -1)
                    {
                        diffs.Add(new Difference(entry.Value[i].line, entry.Value[i].MaybeChanged, true, Action.CHANGE));
                        continue;
                    }

                    diffs.Add(new Difference(entry.Value[i].line, null, false, Action.DELETE));
                }
                // do something with entry.Value or entry.Key
            }
            /*
                diffs.Add(new Difference(null,0, true,Action.ADD));
                diffs.Add(new Difference(2,null, true, Action.CHANGE));
                diffs.Add(new Difference(5,3, false, Action.DELETE));*/
            return diffs;
        }

    }
}


