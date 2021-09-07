using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;



namespace GoblinBrawlGang
{
    public struct MonsterType
    {
        public string guid;
        public string fid;
        public string name;
        public string cr;
        public string size;
        public string type;
        public string tags;
        public string section;
        public string alignment;
        public string environment;
        public string ac;
        public string hp;
        public string init;
        public string lair;
        public string legendary;
        public string unique;
        public string sources;
    }
    public static class MonsterManual
    {
        public static List<MonsterType> MyManual = new List<MonsterType>();

        public class IntStringComparer : StringComparer
        {
            public override int Compare(string x, string y)
            {
                if (x == null && y != null) 
                {
                    return -1;
                }
                if (x != null && y == null)
                {
                    return 1;
                }
                if (x == null && y == null) 
                {
                    return 0;
                }

                switch(x)
                {
                    case "1/2":
                        x = "0.5";
                        break;
                    case "1/4":
                        x = "0.25";
                        break;
                    case "1/8":
                        x = "0.125";
                        break;
                }
                switch (y)
                {
                    case "1/2":
                        y = "0.5";
                        break;
                    case "1/4":
                        y = "0.25";
                        break;
                    case "1/8":
                        y = "0.125";
                        break;
                }

                if (double.Parse(x) < double.Parse(y))
                {
                    return -1;
                }
                else if (double.Parse(x) > double.Parse(y))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            public override bool Equals(string x, string y)
            {
                return int.Parse(x) == int.Parse(y);
            }

            public override int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
        public static IntStringComparer ISComp = new IntStringComparer();

        public static SortedDictionary<string, List<MonsterType>> crDict = new SortedDictionary<string, List<MonsterType>>(ISComp);
        public static SortedDictionary<string, List<MonsterType>> sizeDict = new SortedDictionary<string, List<MonsterType>>(StringComparer.Ordinal);
        public static SortedDictionary<string, List<MonsterType>> typeDict = new SortedDictionary<string, List<MonsterType>>(StringComparer.Ordinal);
        public static SortedDictionary<string, List<MonsterType>> tagsDict = new SortedDictionary<string, List<MonsterType>>(StringComparer.Ordinal);
        public static SortedDictionary<string, List<MonsterType>> sectionDict = new SortedDictionary<string, List<MonsterType>>(StringComparer.Ordinal);
        public static SortedDictionary<string, List<MonsterType>> alignmentDict = new SortedDictionary<string, List<MonsterType>>(StringComparer.Ordinal);
        public static SortedDictionary<string, List<MonsterType>> environmentDict = new SortedDictionary<string, List<MonsterType>>(StringComparer.Ordinal);

        public static void InitManual()
        {
            var path = "C:/Users/Michael/source/GoblinBrawlGang/GoblinBrawlGang/DBs/OfficialKFC.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    MonsterType thisMonster = new MonsterType();
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    thisMonster.guid = fields[0];
                    thisMonster.fid = fields[1];
                    thisMonster.name = fields[2];
                    thisMonster.cr = fields[3];
                    thisMonster.size = fields[4];
                    thisMonster.type = fields[5];
                    thisMonster.tags = fields[6];
                    thisMonster.section = fields[7];
                    thisMonster.alignment = fields[8];
                    thisMonster.environment = fields[9];
                    thisMonster.ac = fields[10];
                    thisMonster.hp = fields[11];
                    thisMonster.init = fields[12];
                    thisMonster.lair = fields[13];
                    thisMonster.legendary = fields[14];
                    thisMonster.unique = fields[15];
                    thisMonster.sources = fields[16];
                    MyManual.Add(thisMonster);
                    SortIntoDictionaries(thisMonster);
                }
            }
        }
        
        public static void SortIntoDictionaries(MonsterType thisMonster)
        {
            // 1 cr
            if(crDict.ContainsKey(thisMonster.cr))
            {
                crDict[thisMonster.cr].Add(thisMonster);
            }
            else
            {
                crDict.Add(thisMonster.cr, new List<MonsterType>() { thisMonster });
            }

            // 2 size
            if (sizeDict.ContainsKey(thisMonster.size))
            {
                sizeDict[thisMonster.size].Add(thisMonster);
            }
            else
            {
                sizeDict.Add(thisMonster.size, new List<MonsterType>() { thisMonster });
            }

            // 3 type
            if (typeDict.ContainsKey(thisMonster.type))
            {
                typeDict[thisMonster.type].Add(thisMonster);
            }
            else
            {
                typeDict.Add(thisMonster.type, new List<MonsterType>() { thisMonster });
            }

            // tags
            if (tagsDict.ContainsKey(thisMonster.tags))
            {
                tagsDict[thisMonster.tags].Add(thisMonster);
            }
            else
            {
                tagsDict.Add(thisMonster.tags, new List<MonsterType>() { thisMonster });
            }

            // section
            if (sectionDict.ContainsKey(thisMonster.section))
            {
                sectionDict[thisMonster.section].Add(thisMonster);
            }
            else
            {
                sectionDict.Add(thisMonster.section, new List<MonsterType>() { thisMonster });
            }

            // alignment
            if (alignmentDict.ContainsKey(thisMonster.alignment))
            {
                alignmentDict[thisMonster.alignment].Add(thisMonster);
            }
            else
            {
                alignmentDict.Add(thisMonster.alignment, new List<MonsterType>() { thisMonster });
            }

            // environment
            if (environmentDict.ContainsKey(thisMonster.environment))
            {
                environmentDict[thisMonster.environment].Add(thisMonster);
            }
            else
            {
                environmentDict.Add(thisMonster.environment, new List<MonsterType>() { thisMonster });
            }
        }


        // verify alignment filters
        public static bool IsProperAlignment(MonsterType mt, List<string> filters)
        {
            // if this creature is unaligned, and we've selected unaligned as a filter,
            // allow it regardless of the other filters.
            if (mt.alignment == "unaligned" && filters.Contains("Unaligned"))
            {
                return true;
            }

            foreach (string align in filters)
            {
                // return false when the monster has an alignment type that we disallow,
                // or when it fails to have an alignment type we require
                switch (align)
                {
                    case ("Good"):
                        if (!mt.alignment.Contains("good"))
                        {
                            return false;
                        }
                        break;
                    case ("GvE: Neutral"):
                        if (mt.alignment.Contains("good") || mt.alignment.Contains("evil"))
                        {
                            return false;
                        }
                        break;
                    case ("Evil"):
                        if (!mt.alignment.Contains("evil"))
                        {
                            return false;
                        }
                        break;
                    case ("Lawful"):
                        if (!mt.alignment.Contains("lawful"))
                        {
                            return false;
                        }
                        break;
                    case ("LvC: Neutral"):
                        if (mt.alignment.Contains("lawful") || mt.alignment.Contains("chaotic"))
                        {
                            return false;
                        }
                        break;
                    case ("Chaotic"):
                        if (!mt.alignment.Contains("chaotic"))
                        {
                            return false;
                        }
                        break;
                    case ("Non-Good"):
                        if (mt.alignment.Contains("good"))
                        {
                            return false;
                        }
                        break;
                    case ("Non-Evil"):
                        if (!mt.alignment.Contains("evil"))
                        {
                            return false;
                        }
                        break;
                    case ("Non-Lawful"):
                        if (mt.alignment.Contains("lawful"))
                        {
                            return false;
                        }
                        break;
                    case ("Non-Chaotic"):
                        if (mt.alignment.Contains("chaotic"))
                        {
                            return false;
                        }
                        break;
                    case ("Unaligned"):
                        if (!mt.alignment.Contains("unaligned"))
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
