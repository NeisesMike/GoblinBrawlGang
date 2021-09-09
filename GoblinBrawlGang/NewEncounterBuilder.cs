using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoblinBrawlGang
{
    public partial class NewEncounterBuilder : Form
    {
        private List<CombatRating.CR> CRMobList = new List<CombatRating.CR>();
        private GroupBox CRMobListGB = new GroupBox();
        private List<MonsterType> MTList = new List<MonsterType>();
        private GroupBox encounterGB = new GroupBox();
        private GroupBox CRButtonsGB = new GroupBox();
        private GroupBox ControlButtonsGB = new GroupBox();
        private GroupBox AXPGB = new GroupBox();
        private GroupBox DifficultyGB = new GroupBox();
        private GroupBox FiltersGB = new GroupBox();

        private int numControlButtons = 0;

        public NewEncounterBuilder()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponent();
            InitStaticComponents();
        }
        public NewEncounterBuilder(List<Monster> mobs)
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponent();
            InitStaticComponents();
            foreach (Monster mob in mobs)
            {
                AddToCRMobList(mob.cr);
            }
        }

        public void UpdateGraphics()
        {
            UpdateCRMobList();
            UpdateMTList();
            UpdateEncounterDifficulty();
        }
        public void InitStaticComponents()
        {
            AddCRButtons();
            AddControlButtons();
            AddXPMultiplierNotes();
            BuildDropDowns();
            UpdateGraphics();
        }
        public void AddControlButtons()
        {
            ControlButtonsGB = new GroupBox();
            ControlButtonsGB.Text = "Controls";
            ControlButtonsGB.Width = CRButtonsGB.Width;
            ControlButtonsGB.Height = 100;
            ControlButtonsGB.Location = new Point(12, CRButtonsGB.Location.Y + CRButtonsGB.Height);

            NewControlButtonPlease("Reset Encounter", ResetEncounter);
            NewControlButtonPlease("Toggle Filters", ToggleFilters);
            NewControlButtonPlease("Fill Encounter", RandomizeEncounter);
            NewControlButtonPlease("Unfill", ReduceEncounter);

            Controls.Add(ControlButtonsGB);
        }
        private void ToggleFilters(object sender, EventArgs e)
        {
            if (Controls.Contains(FiltersGB))
            {
                Controls.Remove(FiltersGB);
            }
            else
            {
                Controls.Add(FiltersGB);
            }
        }
        private void ResetEncounter(object sender, EventArgs e)
        {
            CRMobList.Clear();
            MTList.Clear();
            UpdateGraphics();
        }
        private void NewControlButtonPlease(string name, System.EventHandler handler)
        {
            Button thisButton = new Button();
            thisButton.Parent = ControlButtonsGB;
            thisButton.Text = name;
            thisButton.Height = 25;
            thisButton.Width = 100;
            thisButton.Location = new Point(12 + thisButton.Width * (numControlButtons % 2), 24 + thisButton.Height * (numControlButtons / 2));
            thisButton.Click += handler;
            numControlButtons++;
        }
        public void AddCRButtons()
        {
            CRButtonsGB = new GroupBox();
            CRButtonsGB.Text = "Add Creature of CR";
            CRButtonsGB.AutoSize = true;
            CRButtonsGB.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CRButtonsGB.Location = new Point(12, 12);
            const int buttonsHeight = 25;
            const int buttonsWidth = 100;
            const int numRows = 10;
            const int numCols = 3;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    int thisButtonCRNum = i * numCols + j;
                    if (thisButtonCRNum <= (int)CombatRating.CR.thirty)
                    {
                        CombatRating.CR thisButtonCR = (CombatRating.CR)thisButtonCRNum;
                        Button thisButton = new Button();
                        thisButton.Name = thisButtonCR.ToString();
                        thisButton.Text = thisButtonCR.ToString();
                        thisButton.Height = buttonsHeight;
                        thisButton.Width = buttonsWidth;
                        thisButton.Parent = CRButtonsGB;
                        thisButton.Location = new Point(12 + buttonsWidth * j, 24 + buttonsHeight * i);
                        thisButton.Click += AddCreatureOfCR(thisButtonCR);
                    }
                }
            }
            Controls.Add(CRButtonsGB);
        }
        public System.EventHandler AddCreatureOfCR(CombatRating.CR cr)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                AddToCRMobList(cr);
            }
            return ThisButton_Click;
        }
        public void AddToCRMobList(CombatRating.CR cr)
        {
            CRMobList.Add(cr);
            UpdateGraphics();
        }
        public void RemoveFromCRMobList(CombatRating.CR cr)
        {
            CRMobList.Remove(cr);
            UpdateGraphics();
        }
        public void RemoveFromCRMobList(int index)
        {
            CRMobList.RemoveAt(index);
            UpdateGraphics();
        }
        public void UpdateCRMobList()
        {
            Controls.Remove(CRMobListGB);
            CRMobListGB = new GroupBox();
            CRMobListGB.Location = new Point(CRButtonsGB.Location.X + CRButtonsGB.Width + 12, 12);
            CRMobListGB.Text = "CR Mob List";
            CRMobListGB.AutoSize = true;
            CRMobListGB.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            MonsterManual.IntStringComparer ISComp = new MonsterManual.IntStringComparer();
            CRMobList.Sort(delegate (CombatRating.CR x, CombatRating.CR y)
            {
                return (ISComp.Compare(CombatRating.CREnumToString(x), CombatRating.CREnumToString(y)));
            });

            for (int i = 0; i < CRMobList.Count; i++)
            {
                Label thisLabel = new Label();
                thisLabel.Text = CRMobList[i].ToString();
                thisLabel.Height = 25;
                thisLabel.Width = 75;
                thisLabel.Parent = CRMobListGB;
                thisLabel.Location = new Point(4, 24 + i * thisLabel.Height);

                Button findButton = new Button();
                findButton.Text = "Find";
                findButton.Height = 25;
                findButton.Width = 50;
                findButton.Parent = CRMobListGB;
                findButton.Location = new Point(4 + 75, 24 + i * thisLabel.Height);
                findButton.Click += OpenConnectedMobCF(CRMobList[i]);

                Button delButton = new Button();
                delButton.Text = "Delete";
                delButton.Height = 25;
                delButton.Width = 50;
                delButton.Parent = CRMobListGB;
                delButton.Location = new Point(4 + 75 + 50, 24 + i * thisLabel.Height);
                delButton.Click += RemoveThisCRMob(i);
            }
            if (CRMobList.Count > 0)
            {
                Controls.Add(CRMobListGB);
            }
        }
        public System.EventHandler RemoveThisCRMob(int index)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                RemoveFromCRMobList(index);
            }
            return ThisButton_Click;
        }
        public void OpenConnectedCF()
        {

        }
        public System.EventHandler OpenConnectedMobCF(CombatRating.CR cr)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                CreatureFinder thisMobCF = new CreatureFinder(cr, this);
                thisMobCF.Show();
            }
            return ThisButton_Click;
        }
        public void SwapCRMobForMob(CombatRating.CR cr, MonsterType mt)
        {
            RemoveFromCRMobList(cr);
            AddToMTList(mt);
        }
        public void SwapMobForCRMob(MonsterType mt, CombatRating.CR cr)
        {
            AddToCRMobList(cr);
            RemoveFromMTList(mt);
        }
        public void AddToMTList(MonsterType mt)
        {
            MTList.Add(mt);
            UpdateGraphics();
        }
        public void RemoveFromMTList(MonsterType mt)
        {
            MTList.Remove(mt);
            UpdateGraphics();
        }
        private void UpdateMTList()
        {
            this.Controls.Remove(encounterGB);

            encounterGB = new GroupBox();
            encounterGB.Text = "My Encounter";
            encounterGB.AutoSize = true;
            encounterGB.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            if (Controls.Contains(CRMobListGB))
            {
                encounterGB.Location = new Point(CRMobListGB.Location.X + CRMobListGB.Width + 12, 12);
            }
            else
            {
                encounterGB.Location = new Point(CRButtonsGB.Location.X + CRButtonsGB.Width + 12, 12);
            }

            // print in rows of four
            for (int i = 0; i < MTList.Count; i++)
            {
                GroupBox thisGB = new GroupBox();
                thisGB.Parent = encounterGB;
                thisGB.Height = 240;
                thisGB.Width = 300;
                thisGB.Location = new Point(4 + 12 + thisGB.Width * (i % 4), 24 + thisGB.Height * (i / 4));

                Label thisLabel = new Label();
                thisLabel.Parent = thisGB;
                thisLabel.Location = new Point(12, 12);
                thisLabel.AutoSize = true;
                thisLabel.Text =
                    MTList[i].name + Environment.NewLine +
                    "CR: " + MTList[i].cr + Environment.NewLine +
                    "Size: " + MTList[i].size + Environment.NewLine +
                    "Type: " + MTList[i].type + Environment.NewLine +
                    "Tags: " + MTList[i].tags + Environment.NewLine +
                    "Section: " + MTList[i].section + Environment.NewLine +
                    "Align: " + MTList[i].alignment + Environment.NewLine +
                    "Env: " + MTList[i].environment + Environment.NewLine +
                    "AC: " + MTList[i].ac + Environment.NewLine +
                    "HP: " + MTList[i].hp;


                Button delButton = new Button();
                delButton.Text = "Delete";
                delButton.Height = 25;
                delButton.Width = 100;
                delButton.Parent = thisGB;
                delButton.Location = new Point(12, 170);
                delButton.Click += RemoveCreatureFromEncounter(MTList[i]);

                Button findButton = new Button();
                findButton.Text = "Replace";
                findButton.Height = 25;
                findButton.Width = 100;
                findButton.Parent = thisGB;
                findButton.Location = new Point(12, 170 + 25);
                findButton.Click += ReplaceCreatureUsingFinder(MTList[i]);
            }
            this.Controls.Add(encounterGB);
        }
        public System.EventHandler RemoveCreatureFromEncounter(MonsterType mt)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                RemoveFromMTList(mt);
            }
            return ThisButton_Click;
        }
        public System.EventHandler ReplaceCreatureUsingFinder(MonsterType mt)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                RemoveFromMTList(mt);
                CreatureFinder thisMobCF = new CreatureFinder(CombatRating.CRStringToEnum(mt.cr), this);
                thisMobCF.Show();
            }
            return ThisButton_Click;
        }
        public double CalculateAXPForThisEncounter()
        {
            double base_xp = 0;
            int mobCount = 0;
            foreach (CombatRating.CR cr in CRMobList)
            {
                base_xp += XPManager.monsters[cr].Item1;
                mobCount++;
            }
            foreach (MonsterType mt in MTList)
            {
                base_xp += XPManager.monsters[CombatRating.CRStringToEnum(mt.cr)].Item1;
                mobCount++;
            }
            return base_xp * XPManager.GetEncounterMultiplier(GoblinBrawlGang.players.Count, mobCount);
        }
        public void UpdateEncounterDifficulty()
        {
            Controls.Remove(DifficultyGB);
            DifficultyGB = new GroupBox();
            DifficultyGB.Height = AXPGB.Height;
            DifficultyGB.Width = AXPGB.Width;
            DifficultyGB.Location = new Point((12 + CRButtonsGB.Width) - DifficultyGB.Width, ControlButtonsGB.Location.Y + ControlButtonsGB.Height);
            DifficultyGB.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            double AXP = CalculateAXPForThisEncounter();
            string report = "AXP: " + AXP.ToString() + Environment.NewLine + "Difficulty: ";
            double easySupremum = XPManager.GetEncounterAXPThreshold(Difficulty.Easy);
            double mediumSupremum = XPManager.GetEncounterAXPThreshold(Difficulty.Medium);
            double hardSupremum = XPManager.GetEncounterAXPThreshold(Difficulty.Hard);
            double deadlySupremum = XPManager.GetEncounterAXPThreshold(Difficulty.Deadly);
            if (hardSupremum < AXP)
            {
                report += "Deadly";
            }
            else if (mediumSupremum < AXP)
            {
                report += "Hard";
            }
            else if (easySupremum < AXP)
            {
                report += "Medium";
            }
            else
            {
                report += "Easy";
            }

            Label thisLab = new Label();
            thisLab.Parent = DifficultyGB;
            thisLab.Location = new Point(12, 12);
            thisLab.AutoSize = true;
            thisLab.Text =
                report + Environment.NewLine
              + "Creature Count: " + (CRMobList.Count + MTList.Count).ToString() + Environment.NewLine
              + "Easy:   [0, " + easySupremum.ToString() + "]" + Environment.NewLine
              + "Medium: (" + easySupremum.ToString() + ", " + mediumSupremum.ToString() + "]" + Environment.NewLine
              + "Hard:   (" + mediumSupremum.ToString() + ", " + hardSupremum.ToString() + "]" + Environment.NewLine
              + "Deadly: (" + hardSupremum.ToString() + ", " + deadlySupremum.ToString() + "]";

            Controls.Add(DifficultyGB);
        }
        public void AddXPMultiplierNotes()
        {
            AXPGB = new GroupBox();
            AXPGB.Location = new Point(12, ControlButtonsGB.Location.Y + ControlButtonsGB.Height);
            AXPGB.AutoSize = true;
            AXPGB.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            double[] multiplierList;
            if (GoblinBrawlGang.players.Count < 3)
            {
                multiplierList = new double[] { 1.5, 2.0, 2.5, 3.0, 4.0, 5.0 };
            }
            else if (5 < GoblinBrawlGang.players.Count)
            {
                multiplierList = new double[] { 0.5, 1.0, 1.5, 2.0, 2.5, 3.0 };
            }
            else
            {
                multiplierList = new double[] { 1.0, 1.5, 2.0, 2.5, 3.0, 4.0 };
            }

            Label thisLab = new Label();
            thisLab.Parent = AXPGB;
            thisLab.Location = new Point(12, 12);
            thisLab.AutoSize = true;
            thisLab.Font = new Font(FontFamily.GenericMonospace.Name, 8);
            thisLab.Text =
                "Mobs  : Multiplier" + Environment.NewLine
              + "1     : " + multiplierList[0].ToString() + Environment.NewLine
              + "2     : " + multiplierList[1].ToString() + Environment.NewLine
              + "3-6   : " + multiplierList[2].ToString() + Environment.NewLine
              + "7-10  : " + multiplierList[3].ToString() + Environment.NewLine
              + "11-14 : " + multiplierList[4].ToString() + Environment.NewLine
              + "15+   : " + multiplierList[5].ToString() + Environment.NewLine;

            Controls.Add(AXPGB);
        }
        private void BuildDropDowns()
        {

            FiltersGB = new GroupBox();
            FiltersGB.Width = CRButtonsGB.Width;
            FiltersGB.Height = 1080 - (AXPGB.Location.Y + AXPGB.Height + 12);
            FiltersGB.Location = new Point(12, AXPGB.Location.Y + AXPGB.Height + 12);

            int currentVertStart = 24;
            CheckedListBox GetCLB(string name)
            {
                CheckedListBox thisCLB = new CheckedListBox();
                thisCLB.Parent = FiltersGB;
                thisCLB.Name = name;
                thisCLB.Text = name;
                thisCLB.Height = 64;
                thisCLB.Width = CRButtonsGB.Width - 24;
                thisCLB.Location = new Point(12, currentVertStart);
                currentVertStart += thisCLB.Height + 12;
                return thisCLB;
            }

            CheckedListBox sizeCLB = GetCLB("size");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.sizeDict)
            {
                sizeCLB.Items.Add(pair.Key);
            }

            CheckedListBox typeCLB = GetCLB("type");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.typeDict)
            {
                typeCLB.Items.Add(pair.Key);
            }

            CheckedListBox tagsCLB = GetCLB("tags");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.tagsDict)
            {
                if (pair.Key != "" && !pair.Key.Contains("any") && !pair.Key.Contains("Any"))
                {
                    tagsCLB.Items.Add(pair.Key);
                }
            }

            CheckedListBox sectionCLB = GetCLB("section");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.sectionDict)
            {
                if (pair.Key != "")
                {
                    sectionCLB.Items.Add(pair.Key);
                }
            }

            CheckedListBox alignmentCLB = GetCLB("alignment");
            alignmentCLB.Items.Add("Good");
            alignmentCLB.Items.Add("GvE: Neutral");
            alignmentCLB.Items.Add("Evil");
            alignmentCLB.Items.Add("Lawful");
            alignmentCLB.Items.Add("LvC: Neutral");
            alignmentCLB.Items.Add("Chaotic");
            alignmentCLB.Items.Add("Non-Good");
            alignmentCLB.Items.Add("Non-Evil");
            alignmentCLB.Items.Add("Non-Lawful");
            alignmentCLB.Items.Add("Non-Chaotic");
            alignmentCLB.Items.Add("Unaligned");

            CheckedListBox environmentCLB = GetCLB("environment");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.environmentDict)
            {
                if (pair.Key.Contains(",") || pair.Key == "")
                {
                    continue;
                }
                environmentCLB.Items.Add(pair.Key);
            }
        }
        private void RandomizeEncounter(object sender, EventArgs e)
        {
            System.Random rand = new System.Random();
            List<Tuple<CombatRating.CR, MonsterType>> toBeSwapped = new List<Tuple<CombatRating.CR, MonsterType>>();
            foreach (CombatRating.CR cr in CRMobList)
            {
                List<MonsterType> crMobs = GenerateFilteredCreatureList(cr);
                if (crMobs == null)
                {
                    continue;
                }
                MonsterType chosenMob = crMobs[rand.Next(crMobs.Count)];
                toBeSwapped.Add(new Tuple<CombatRating.CR, MonsterType>(cr, chosenMob));
            }
            foreach (var swap in toBeSwapped)
            {
                SwapCRMobForMob(swap.Item1, swap.Item2);
            }
        }
        private List<MonsterType> GenerateFilteredCreatureList(CombatRating.CR cr)
        {
            // declare filters
            List<string> crFilters = new List<string>();
            List<string> sizeFilters = new List<string>();
            List<string> typeFilters = new List<string>();
            List<string> tagsFilters = new List<string>();
            List<string> sectionFilters = new List<string>();
            List<string> alignmentFilters = new List<string>();
            List<string> environmentFilters = new List<string>();


            // generate filters
            crFilters.Add(CombatRating.CREnumToString(cr));

            foreach (CheckedListBox clb in FiltersGB.Controls.OfType<CheckedListBox>())
            {
                foreach (string cItem in clb.CheckedItems)
                {
                    switch (clb.Name)
                    {
                        case ("size"):
                            sizeFilters.Add(cItem);
                            break;
                        case ("type"):
                            typeFilters.Add(cItem);
                            break;
                        case ("tags"):
                            tagsFilters.Add(cItem);
                            break;
                        case ("section"):
                            sectionFilters.Add(cItem);
                            break;
                        case ("alignment"):
                            alignmentFilters.Add(cItem);
                            break;
                        case ("environment"):
                            environmentFilters.Add(cItem);
                            break;

                    }
                }
            }

            List<MonsterType> filteredList = new List<MonsterType>();
            foreach (MonsterType mt in MonsterManual.MyManual)
            {
                if
                    (
                        (crFilters.Contains(mt.cr) || crFilters.Count == 0)
                     && (sizeFilters.Contains(mt.size) || sizeFilters.Count == 0)
                     && (typeFilters.Contains(mt.type) || typeFilters.Count == 0)
                     && (MonsterManual.IsProperTag(mt, tagsFilters) || tagsFilters.Count == 0)
                     && (MonsterManual.IsProperSection(mt, sectionFilters) || sectionFilters.Count == 0)
                     && (MonsterManual.IsProperEnv(mt, environmentFilters) || environmentFilters.Count == 0)
                     && (MonsterManual.IsProperAlignment(mt, alignmentFilters) || alignmentFilters.Count == 0)
                    )
                {
                    filteredList.Add(mt);
                }
            }

            if (filteredList.Count == 0)
            {
                return null;
            }
            return filteredList;
        }
        private void ReduceEncounter(object sender, EventArgs e)
        {
            List<Tuple<MonsterType, CombatRating.CR>> toBeSwapped = new List<Tuple<MonsterType, CombatRating.CR>>();
            foreach (MonsterType mt in MTList)
            {
                toBeSwapped.Add(new Tuple<MonsterType, CombatRating.CR>(mt, CombatRating.CRStringToEnum(mt.cr)));
            }
            foreach (var swap in toBeSwapped)
            {
                SwapMobForCRMob(swap.Item1, swap.Item2);
            }
        }
    }
}
