using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GoblinBrawlGang
{
    public partial class EncounterBuilder : Form
    {
        public List<MonsterType> myEncounter = new List<MonsterType>();
        public List<GroupBox> myEncounterGBs = new List<GroupBox>();

        public CheckedListBox crCLB;
        public CheckedListBox sizeCLB;
        public CheckedListBox typeCLB;
        public CheckedListBox tagsCLB;
        public CheckedListBox sectionCLB;
        public CheckedListBox alignmentCLB;
        public CheckedListBox environmentCLB;

        private List<Monster> mobList;
        private int leftBarWidth = 260;

        public EncounterBuilder(List<Monster> inMobList, SortedDictionary<int, Tuple<string, int>> printDict)
        {
            InitializeComponent();
            mobList = inMobList;
            this.AutoSize = true;
            Label mobs = GetMobListLabel(printDict, mobList.Count);
            this.Controls.Add(mobs);
            Button rando = GetRandomizerButton(12, 12 + mobs.Height + 12);
            this.Controls.Add(rando);
            Button resetti = GetResetFiltersButton(rando.Width + 12, 12 + mobs.Height + 12);
            this.Controls.Add(resetti);
            AddDropDowns(12 + mobs.Height + 12 + rando.Height + 12);
        }

        private void EncounterBuilder_Load(object sender, EventArgs e)
        {
        }

        public Label GetMobListLabel(SortedDictionary<int, Tuple<string, int>> printDict, int numMobs)
        {
            Label monsterSeqeuence = new Label();
            monsterSeqeuence.AutoSize = true;
            monsterSeqeuence.Location = new Point(12, 18);
            monsterSeqeuence.Text = "Creature Count: " + numMobs.ToString() + Environment.NewLine;
            foreach (KeyValuePair<int, Tuple<string, int>> mobGroup in printDict)
            {
                monsterSeqeuence.Text += mobGroup.Value.Item1 + " : " + mobGroup.Value.Item2.ToString() + Environment.NewLine;
            }
            return monsterSeqeuence;
        }

        private void WipeEncounters()
        {
            foreach (GroupBox gb in myEncounterGBs)
            {
                Controls.Remove(gb);
            }
        }
        private void ResetFilters()
        {
            sizeCLB.ClearSelected();
            for (int i = 0; i < sizeCLB.Items.Count; i++)
            {
                sizeCLB.SetItemChecked(i, false);
            }
            typeCLB.ClearSelected();
            for (int i = 0; i < typeCLB.Items.Count; i++)
            {
                typeCLB.SetItemChecked(i, false);
            }
            tagsCLB.ClearSelected();
            for (int i = 0; i < tagsCLB.Items.Count; i++)
            {
                tagsCLB.SetItemChecked(i, false);
            }
            sectionCLB.ClearSelected();
            for (int i = 0; i < sectionCLB.Items.Count; i++)
            {
                sectionCLB.SetItemChecked(i, false);
            }
            alignmentCLB.ClearSelected();
            for (int i = 0; i < alignmentCLB.Items.Count; i++)
            {
                alignmentCLB.SetItemChecked(i, false);
            }
            environmentCLB.ClearSelected();
            for (int i = 0; i < environmentCLB.Items.Count; i++)
            {
                environmentCLB.SetItemChecked(i, false);
            }
        }
        private Button GetRandomizerButton(int x, int y)
        {
            void RandomGo(object sender, EventArgs e)
            {
                WipeEncounters();
                GenerateRandomEncounter();
                PrintEncounter();
            }
            Button rando = new Button();
            rando.Text = "Randomize";
            rando.Height = 50;
            rando.Width = 100;
            rando.Location = new Point(x, y);
            rando.Click += RandomGo;
            return rando;
        }
        private Button GetResetFiltersButton(int x, int y)
        {
            void RandomGo(object sender, EventArgs e)
            {
                ResetFilters();
            }
            Button res = new Button();
            res.Text = "Reset Filters";
            res.Height = 50;
            res.Width = 100;
            res.Location = new Point(x, y);
            res.Click += RandomGo;
            return res;
        }
        private void AddDropDowns(int vertStart)
        {
            int currentVertStart = vertStart;

            CheckedListBox GetCLB(string name)
            {
                CheckedListBox thisCLB = new CheckedListBox();
                thisCLB.Text = name;
                thisCLB.Height = 100;
                thisCLB.Width = leftBarWidth;
                thisCLB.Location = new Point(12, currentVertStart);
                currentVertStart += thisCLB.Height + 12;
                return thisCLB;
            }

            /*
            crCLB = GetCLB("cr");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.crDict)
            {
                crCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(crCLB);
            */

            sizeCLB = GetCLB("size");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.sizeDict)
            {
                sizeCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(sizeCLB);

            typeCLB = GetCLB("type");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.typeDict)
            {
                typeCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(typeCLB);

            tagsCLB = GetCLB("tags");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.tagsDict)
            {
                if (pair.Key != "" && !pair.Key.Contains("any") && !pair.Key.Contains("Any"))
                {
                    tagsCLB.Items.Add(pair.Key);
                }
            }
            this.Controls.Add(tagsCLB);

            sectionCLB = GetCLB("section");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.sectionDict)
            {
                if (pair.Key != "")
                {
                    sectionCLB.Items.Add(pair.Key);
                }
            }
            this.Controls.Add(sectionCLB);

            alignmentCLB = GetCLB("alignment");
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
            this.Controls.Add(alignmentCLB);

            environmentCLB = GetCLB("environment");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.environmentDict)
            {
                if (pair.Key.Contains(",") || pair.Key == "")
                {
                    continue;
                }
                environmentCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(environmentCLB);
        }

        private void PrintEncounter()
        {
            // print in rows of three
            int position = 0;
            foreach (MonsterType mt in myEncounter)
            {
                GroupBox thisGB = new GroupBox();
                thisGB.Text = mt.name;
                thisGB.Height = 200;
                thisGB.Width = 300;
                thisGB.Location = new Point(leftBarWidth + 12 + 12 + thisGB.Width * (position % 3), 12 + thisGB.Height * (position / 3));
                position++;

                Label thisLabel = new Label();
                thisLabel.Parent = thisGB;
                thisLabel.Location = new Point(12, 24);
                thisLabel.AutoSize = true;
                thisLabel.Text =
                    "CR: " + mt.cr + Environment.NewLine +
                    "Size: " + mt.size + Environment.NewLine +
                    "Type: " + mt.type + Environment.NewLine +
                    "Tags: " + mt.tags + Environment.NewLine +
                    "Section: " + mt.section + Environment.NewLine +
                    "Align: " + mt.alignment + Environment.NewLine +
                    "Env: " + mt.environment + Environment.NewLine +
                    "AC: " + mt.ac + Environment.NewLine +
                    "HP: " + mt.hp;

                this.Controls.Add(thisGB);
                myEncounterGBs.Add(thisGB);
            }
        }

        private void GenerateRandomEncounter()
        {
            myEncounter.Clear();
            Random random = new Random();

            // generate filters
            /*
            List<string> crFilters = new List<string>();
            foreach (string cItem in crCLB.CheckedItems)
            {
                crFilters.Add(cItem);
            }
            */

            List<string> sizeFilters = new List<string>();
            foreach (string cItem in sizeCLB.CheckedItems)
            {
                sizeFilters.Add(cItem);
            }

            List<string> typeFilters = new List<string>();
            foreach (string cItem in typeCLB.CheckedItems)
            {
                typeFilters.Add(cItem);
            }

            List<string> tagsFilters = new List<string>();
            foreach (string cItem in tagsCLB.CheckedItems)
            {
                tagsFilters.Add(cItem);
            }

            List<string> sectionFilters = new List<string>();
            foreach (string cItem in sectionCLB.CheckedItems)
            {
                sectionFilters.Add(cItem);
            }

            List<string> alignmentFilters = new List<string>();
            foreach (string cItem in alignmentCLB.CheckedItems)
            {
                alignmentFilters.Add(cItem);
            }

            List<string> environmentFilters = new List<string>();
            foreach (string cItem in environmentCLB.CheckedItems)
            {
                environmentFilters.Add(cItem);
            }

            List<MonsterType> filteredList = new List<MonsterType>();
            foreach (MonsterType mt in MonsterManual.MyManual)
            {
                if
                    (
                        (sizeFilters.Contains(mt.size) || sizeFilters.Count == 0)
                     && (typeFilters.Contains(mt.type) || typeFilters.Count == 0)
                     && (tagsFilters.Contains(mt.tags) || tagsFilters.Count == 0)
                     && (sectionFilters.Contains(mt.section) || sectionFilters.Count == 0)
                     && (environmentFilters.Contains(mt.environment) || environmentFilters.Count == 0)
                     && (MonsterManual.IsProperAlignment(mt, alignmentFilters) || alignmentFilters.Count == 0)
                    )
                {
                    filteredList.Add(mt);
                }
            }

            if (filteredList.Count == 0)
            {
                // TODO throw a helpful error message
                // "Those filters were incompatible."
                return;
            }

            // build the new monstertype list: the new encounter
            foreach (Monster mob in mobList)
            {
                List<MonsterType> thisCRList = filteredList.FindAll(e => CombatRating.CRStringToEnum(e.cr) == mob.cr);
                if (thisCRList.Count == 0)
                {
                    myEncounter.Clear();
                    // TODO throw a helpful error message
                    // "There are no such creatures at this CR level: " + mob.cr.ToString()
                    return;
                }
                MonsterType chosenMob = thisCRList[random.Next(thisCRList.Count)];
                myEncounter.Add(chosenMob);
            }


        }


    }
}
