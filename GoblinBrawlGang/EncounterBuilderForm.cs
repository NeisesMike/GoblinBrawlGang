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
    public partial class EncounterBuilderForm : Form
    {
        public List<MonsterType> myEncounter = new List<MonsterType>();

        public CheckedListBox crCLB;
        public CheckedListBox sizeCLB;
        public CheckedListBox typeCLB;
        public CheckedListBox tagsCLB;
        public CheckedListBox sectionCLB;
        public CheckedListBox alignmentCLB;
        public CheckedListBox environmentCLB;


        public EncounterBuilderForm(List<Monster> mobList, SortedDictionary<int, Tuple<string, int>> printDict)
        {
            this.AutoSize = true;
            InitializeComponent();
            Label mobs = GetMobListLabel(printDict, mobList.Count);
            this.Controls.Add(mobs);
            Button rando = GetRandomizerButton(12 + mobs.Height + 12);
            this.Controls.Add(rando);
            AddDropDowns(12 + mobs.Height + 12 + rando.Height + 12);
        }

        private void EncounterBuilderForm_Load(object sender, EventArgs e)
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

        private Button GetRandomizerButton(int vertStart)
        {
            void RandomGo(object sender, EventArgs e)
            {
                GenerateRandomEncounter();
                PrintEncounter();
            }
            Button rando = new Button();
            rando.Text = "Randomize";
            rando.Height = 50;
            rando.Width = 100;
            rando.Location = new Point(12, vertStart);
            rando.Click += RandomGo;
            return rando;
        }
        private void AddDropDowns(int vertStart)
        {
            int currentVertStart = vertStart;

            CheckedListBox GetCLB(string name)
            {
                CheckedListBox thisCLB = new CheckedListBox();
                thisCLB.Text = name;
                thisCLB.Height = 100;
                thisCLB.Width = 200;
                thisCLB.Location = new Point(12, currentVertStart);
                currentVertStart += thisCLB.Height + 12;
                return thisCLB;
            }

            crCLB = GetCLB("cr");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.crDict)
            {
                crCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(crCLB);

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
                tagsCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(tagsCLB);

            sectionCLB = GetCLB("section");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.sectionDict)
            {
                sectionCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(sectionCLB);

            alignmentCLB = GetCLB("alignment");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.alignmentDict)
            {
                alignmentCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(alignmentCLB);

            environmentCLB = GetCLB("environment");
            foreach (KeyValuePair<string, List<MonsterType>> pair in MonsterManual.environmentDict)
            {
                environmentCLB.Items.Add(pair.Key);
            }
            this.Controls.Add(environmentCLB);
        }

        private void PrintEncounter()
        {
            // print in rows of three
            int position = 0;
            foreach(MonsterType mt in myEncounter)
            {
                GroupBox thisGB = new GroupBox();
                thisGB.Text = mt.name;
                thisGB.Height = 175;
                thisGB.Width = 150;
                thisGB.Location = new Point(200 + 12 + 12 + thisGB.Width * (position%3), 12 + thisGB.Height * (position/3));
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
            }
        }

        private void GenerateRandomEncounter()
        {
            // generate filters
            List<string> crFilters = new List<string>();
            foreach (string cItem in crCLB.CheckedItems)
            {
                crFilters.Add(cItem);
            }

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

            //TODO apply the filters to the master list, 
            // and randomize over the filtered list.



            // these are just for testing
            myEncounter.Add(MonsterManual.MyManual[0]);
            myEncounter.Add(MonsterManual.MyManual[0]);
            myEncounter.Add(MonsterManual.MyManual[0]);
            myEncounter.Add(MonsterManual.MyManual[10]);
            myEncounter.Add(MonsterManual.MyManual[10]);
            myEncounter.Add(MonsterManual.MyManual[10]);
            myEncounter.Add(MonsterManual.MyManual[100]);
            myEncounter.Add(MonsterManual.MyManual[100]);
            myEncounter.Add(MonsterManual.MyManual[100]);
        }
    }
}
