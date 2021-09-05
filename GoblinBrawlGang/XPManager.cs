﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace GoblinBrawlGang
{
    public enum CR
    {
        zed,
        eighth,
        quarter,
        half,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        eleven,
        twelve,
        thirteen,
        fourteen,
        fifteen,
        sixteen,
        seventeen,
        eighteen,
        nineteen,
        twenty,
        twenty_one,
        twenty_two,
        twenty_three,
        twenty_four,
        thirty
    }
    public enum Difficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
        Deadly = 3
    }
    public struct Monster
    {
        public CR cr;
        public int baseXP;
        public string name;
        public Monster(CR inCR, int inXP, string inName)
        {
            cr = inCR;
            baseXP = inXP;
            name = inName;
        }
    }
    public static class XPManager
    {
        public static Monster ChooseRandomCRMonster()
        {
            System.Random rand = new System.Random();
            CR chosenCR = (CR)rand.Next((int)CR.zed, (int)CR.thirty);
            Monster mob = new Monster(chosenCR, monsters[chosenCR].Item1, monsters[chosenCR].Item2);
            return mob;
        }
        public static double CalculateExpense(List<Monster> mobList)
        {
            double totalBaseXP = 0;
            foreach (Monster mob in mobList)
            {
                totalBaseXP += mob.baseXP;
            }
            return totalBaseXP * GetEncounterMultiplier(Form1.players.Count, mobList.Count);
        }
        public static List<Monster> CreateRandomEncounter(int budget)
        {
            bool IsListUnderBudget(List<Monster> currentMobList, Monster candidate)
            {
                List<Monster> potentialMobList = new List<Monster>();
                potentialMobList.AddRange(currentMobList);
                potentialMobList.Add(candidate);
                return CalculateExpense(potentialMobList) <= budget;
            }

            // name maps to count
            List<Monster> mobList = new List<Monster>();
            int fuel = 100;
            while(0 < fuel)
            {
                Monster thisMonster = ChooseRandomCRMonster();
                if(IsListUnderBudget(mobList,thisMonster))
                {
                    mobList.Add(thisMonster);
                    fuel = 100;
                    continue;
                }
                fuel--;
            }
            return mobList;
        }
        public static GroupBox DeviseEncounter(int xpTotal)
        {
            GroupBox encountersBox = new GroupBox();
            encountersBox.AutoSize = true;

            Label monsterSeqeuence = new Label();
            monsterSeqeuence.AutoSize = true;
            monsterSeqeuence.Parent = encountersBox;
            monsterSeqeuence.Location = new Point(12,18);

            List<Monster> mobList = CreateRandomEncounter(xpTotal);

            SortedDictionary<int,Tuple<string, int>> printDict = new SortedDictionary<int, Tuple<string, int>>();
            foreach (Monster mob in mobList)
            {
                if (printDict.ContainsKey((int)mob.cr))
                {
                    printDict[(int)mob.cr] = new Tuple<string, int>(mob.name, 1 + printDict[(int)mob.cr].Item2);
                }
                else
                {
                    printDict.Add((int)mob.cr, new Tuple<string, int>(mob.name, 1));
                }
            }

            monsterSeqeuence.Text = "Creature Count: " + mobList.Count.ToString() + Environment.NewLine;
            foreach (KeyValuePair<int, Tuple<string, int>> mobGroup in printDict)
            {
                monsterSeqeuence.Text += mobGroup.Value.Item1 + " : " + mobGroup.Value.Item2.ToString() + Environment.NewLine;
            }

            encountersBox.Text = CalculateExpense(mobList) + "XP Encounter";
            encountersBox.Height = (printDict.Count+1) * 15;

            void LoadEncounterBuilder(object sender, EventArgs e)
            {
                EncounterBuilderForm EBform = new EncounterBuilderForm(mobList, printDict);
                EBform.Show();
            }

            // add button to "select" this encounter for further configuration
            Button select = new Button();
            select.Parent = encountersBox;
            select.Text = "select";
            select.Height = 40;
            select.Width = 100;
            select.Location = new Point(125, 24);
            select.Click += LoadEncounterBuilder;



            return encountersBox;
        }
        public static List<GroupBox> CreateEncounters(Queue<Player> players, Difficulty difficulty)
        {
            int totalXPThreshold = 0;
            foreach(Player plyr in players)
            {
                totalXPThreshold += XPThresholdsTable[plyr.level - 1, (int)difficulty];
            }
            List<GroupBox> encounters = new List<GroupBox>();
            for (int i = 0; i < 3; i++)
            {
                encounters.Add(DeviseEncounter(totalXPThreshold));
            }
            return encounters;
        }
        public static Dictionary<CR, Tuple<int, string>> monsters = new Dictionary<CR, Tuple<int, string>>()
        {
            { CR.zed, new Tuple<int,string>(10, "zed") },
            { CR.eighth, new Tuple<int,string>(25, "eighth") },
            { CR.quarter, new Tuple<int,string>(50, "quarter") },
            { CR.half, new Tuple<int,string>(100, "halve") },
            { CR.one, new Tuple<int,string>(200, "one") },
            { CR.two, new Tuple<int,string>(450, "two") },
            { CR.three, new Tuple<int,string>(700, "three") },
            { CR.four, new Tuple<int,string>(1100, "four") },
            { CR.five, new Tuple<int,string>(1800, "five") },
            { CR.six, new Tuple<int,string>(2300, "six") },
            { CR.seven, new Tuple<int,string>(2900, "seven") },
            { CR.eight, new Tuple<int,string>(3900, "eight") },
            { CR.nine, new Tuple<int,string>(5000, "nine") },
            { CR.ten, new Tuple<int,string>(5900, "ten") },
            { CR.eleven, new Tuple<int,string>(7200, "eleven") },
            { CR.twelve, new Tuple<int,string>(8400, "twelve") },
            { CR.thirteen, new Tuple<int,string>(10000, "thirteen") },
            { CR.fourteen, new Tuple<int,string>(11500, "fourteen") },
            { CR.fifteen, new Tuple<int,string>(13000, "fifteen") },
            { CR.sixteen, new Tuple<int,string>(15000, "sixteen") },
            { CR.seventeen, new Tuple<int,string>(18000, "seventeen") },
            { CR.eighteen, new Tuple<int,string>(20000, "eighteen") },
            { CR.nineteen, new Tuple<int,string>(22000, "nineteen") },
            { CR.twenty, new Tuple<int,string>(25000, "twenty") },
            { CR.twenty_one, new Tuple<int,string>(33000, "twenty-one") },
            { CR.twenty_two, new Tuple<int,string>(41000, "twenty-two") },
            { CR.twenty_three, new Tuple<int,string>(50000, "twentry-three") },
            { CR.twenty_four, new Tuple<int,string>(62000, "twentry-four") },
            { CR.thirty, new Tuple<int,string>(155000, "thirty") },
        };

        public static double GetEncounterMultiplier(int numPlayers, int numMonsters)
        {
            double[] multiplierList;
            if (Form1.players.Count < 3)
            {
                multiplierList = new double[] { 1.5, 2, 2.5, 3, 4, 5 };
            }
            else if (5 < Form1.players.Count)
            {
                multiplierList = new double[] { 0.5, 1, 1.5, 2, 2.5, 3 };
            }
            else
            {
                multiplierList =  new double[] { 1, 1.5, 2, 2.5, 3, 4 };
            }

            return numMonsters switch
            {
                1 => multiplierList[0],
                2 => multiplierList[1],
                (>=3) and (<=6) => multiplierList[2],
                (>= 7) and (<= 10) => multiplierList[3],
                (>= 11) and (<= 14) => multiplierList[4],
                _ => multiplierList[5],
            };
        }

        public static readonly int[,] XPThresholdsTable = new int[,]
        {
            {25,50,75,100},
            {50,100,150,200},
            {75,150,225,400},
            {125,250,375,500},
            {250,500,750,1100},
            {300,600,900,1400},
            {350,750,1100,1700},
            {450,900,1400,2100},
            {550,1100,1600,2400},
            {600,1200,1900,2800},
            {800,1600,2400,3600},
            {1000,2000,3000,4500},
            {1100,2200,3400,5100},
            {1250,2500,3800,5700},
            {1400,2800,4300,6400},
            {1600,3200,4800,7200},
            {2000,3900,5900,8800},
            {2100,4200,6300,9500},
            {2400,4900,7300,10900},
            {2800,5700,8500,12700},
        };
    }
}
