﻿using System;
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
    public partial class CreatureFinder : Form
    {
        private enum CheckedData
        {
            None,
            Checked,
            Unchecked
        }
        public FlowLayoutPanel creatureListPanel;

        public TextBox nameFilter = new TextBox();

        public CheckedListBox crCLB;
        public CheckedListBox sizeCLB;
        public CheckedListBox typeCLB;
        public CheckedListBox tagsCLB;
        public CheckedListBox sectionCLB;
        public CheckedListBox alignmentCLB;
        public CheckedListBox environmentCLB;

        private int leftBarWidth = 260;
        public List<MonsterType> mobList = new List<MonsterType>();

        public CreatureFinder()
        {
            InitializeComponent();
            this.AutoSize = true;

            Button rando = GetRandomizerButton(12, 12, false, CombatRating.CR.zed, null);
            this.Controls.Add(rando);

            nameFilter = new TextBox();
            nameFilter.Location = new Point(12, rando.Location.Y + rando.Height + 12);
            nameFilter.TextChanged += MaybeUpdateListsAfterTyping();
            this.Controls.Add(nameFilter);

            AddDropDowns(12 + nameFilter.Location.Y + nameFilter.Height + 12);
            nameFilter.Width = crCLB.Width;

            Button resetti = GetResetFiltersButton((crCLB.Width + crCLB.Location.X) - rando.Width, 12, false);
            this.Controls.Add(resetti);
        }
        public CreatureFinder(CombatRating.CR cr, NewEncounterBuilder neb)
        {
            InitializeComponent();
            this.AutoSize = true;

            Button rando = GetRandomizerButton(12, 12, true, cr, neb);
            this.Controls.Add(rando);

            nameFilter = new TextBox();
            nameFilter.Location = new Point(12, rando.Location.Y + rando.Height + 12);
            nameFilter.TextChanged += MaybeUpdateListsAfterTypingMobCF(cr, neb);
            this.Controls.Add(nameFilter);

            AddDropDownsMobCF(12 + nameFilter.Location.Y + nameFilter.Height + 12, cr, neb);
            PrintFilteredManual(GenerateFilteredCreatureListGeneral(true, cr, CheckedData.None, null, null), true, neb);

            nameFilter.Width = sizeCLB.Width;
            Button resetti = GetResetFiltersButton((sizeCLB.Width + sizeCLB.Location.X) - rando.Width, 12, true);
            this.Controls.Add(resetti);
        }
        private System.EventHandler MaybeUpdateListsAfterTyping()
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                if (nameFilter.Text.Length > 1)
                {
                    WipeEncounters();
                    List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(false, CombatRating.CR.thirty, CheckedData.None, null, null);
                    PrintFilteredManual(creatures, false, null);
                }
            }
            return ThisButton_Click;
        }
        private System.EventHandler MaybeUpdateListsAfterTypingMobCF(CombatRating.CR cr, NewEncounterBuilder neb)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                if (nameFilter.Text.Length > 1)
                {
                    WipeEncounters();
                    List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(true, cr, CheckedData.None, null, null);
                    PrintFilteredManual(creatures, true, neb);
                }
            }
            return ThisButton_Click;
        }
        private ItemCheckEventHandler MaybeUpdateListsAfterBoxChecked(CheckedListBox clb)
        {
            void ThisButton_Click(object sender, ItemCheckEventArgs e)
            {
                if (e.CurrentValue == CheckState.Checked)
                {
                    if (GetTotalItemsChecked(false) > 2)
                    {
                        WipeEncounters();
                        List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(false, CombatRating.CR.thirty, CheckedData.Unchecked, clb, clb.Items[e.Index].ToString());
                        PrintFilteredManual(creatures, false, null);
                        return;
                    }
                }
                else
                {
                    if (GetTotalItemsChecked(false) > 0)
                    {
                        WipeEncounters();
                        List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(false, CombatRating.CR.thirty, CheckedData.Checked, clb, clb.Items[e.Index].ToString());
                        PrintFilteredManual(creatures, false, null);
                        return;
                    }
                }
                if (nameFilter.TextLength > 1)
                {
                    WipeEncounters();
                    List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(false, CombatRating.CR.thirty, e.CurrentValue == CheckState.Checked ? CheckedData.Unchecked : CheckedData.Checked, clb, clb.Items[e.Index].ToString());
                    PrintFilteredManual(creatures, false, null);
                    return;
                }
            }
            return ThisButton_Click;
        }
        private ItemCheckEventHandler MaybeUpdateListsAfterBoxCheckedMobCF(CheckedListBox clb, CombatRating.CR cr, NewEncounterBuilder neb)
        {
            void ThisButton_Click(object sender, ItemCheckEventArgs e)
            {
                if (e.CurrentValue == CheckState.Checked)
                {
                    if (GetTotalItemsChecked(true) > 2)
                    {
                        WipeEncounters();
                        List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(true, cr, CheckedData.Unchecked, clb, clb.Items[e.Index].ToString());
                        PrintFilteredManual(creatures, true, neb);
                        return;
                    }
                }
                else
                {
                    WipeEncounters();
                    List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(true, cr, CheckedData.Checked, clb, clb.Items[e.Index].ToString());
                    PrintFilteredManual(creatures, true, neb);
                    return;
                }
                if (nameFilter.TextLength > 1)
                {
                    WipeEncounters();
                    List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(true, cr, CheckedData.None, clb, clb.Items[e.Index].ToString());
                    PrintFilteredManual(creatures, true, neb);
                    return;
                }
            }
            return ThisButton_Click;
        }

        private int GetTotalItemsChecked(bool isMobCF)
        {
            int ret = 0;
            if (!isMobCF)
            {
                ret += crCLB.CheckedItems.Count;
            }
            ret += sizeCLB.CheckedItems.Count;
            ret += typeCLB.CheckedItems.Count;
            ret += tagsCLB.CheckedItems.Count;
            ret += sectionCLB.CheckedItems.Count;
            ret += alignmentCLB.CheckedItems.Count;
            ret += environmentCLB.CheckedItems.Count;
            return ret;
        }




        private void CreatureFinder_Load(object sender, EventArgs e)
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
            Controls.Remove(creatureListPanel);
        }
        private void ResetFilters(bool isMobCF)
        {
            nameFilter.Text = "";
            if(!isMobCF)
            {
                crCLB.ClearSelected();
                for (int i = 0; i < crCLB.Items.Count; i++)
                {
                    crCLB.SetItemChecked(i, false);
                }
            }
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
        private Button GetRandomizerButton(int x, int y, bool isMobCF, CombatRating.CR cr, NewEncounterBuilder neb)
        {
            void RandomGo(object sender, EventArgs e)
            {
                WipeEncounters();
                List<MonsterType> creatures = GenerateFilteredCreatureListGeneral(isMobCF, cr, CheckedData.None, null, null);
                PrintFilteredManual(creatures, isMobCF, neb);
            }
            Button rando = new Button();
            rando.Text = "Filter";
            rando.Height = 50;
            rando.Width = 125;
            rando.Location = new Point(x, y);
            rando.Click += RandomGo;
            return rando;
        }
        private Button GetResetFiltersButton(int x, int y, bool isMobCF)
        {
            void RandomGo(object sender, EventArgs e)
            {
                ResetFilters(isMobCF);
            }
            Button res = new Button();
            res.Text = "Reset Filters";
            res.Height = 50;
            res.Width = 125;
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
                thisCLB.ItemCheck += MaybeUpdateListsAfterBoxChecked(thisCLB);
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
        private void AddDropDownsMobCF(int vertStart, CombatRating.CR cr, NewEncounterBuilder neb)
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
                thisCLB.ItemCheck += MaybeUpdateListsAfterBoxCheckedMobCF(thisCLB, cr, neb);
                return thisCLB;
            }

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
        private void PrintFilteredManual(List<MonsterType> creatures, bool isMobCF, NewEncounterBuilder neb)
        {
            //  if isMobCF==true MUST pass neb!=null
            if (creatures==null)
            {
                return;
            }

            FlowLayoutPanel rootPanel = new FlowLayoutPanel();
            rootPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            rootPanel.WrapContents = false;
            rootPanel.AutoScroll = true;
            rootPanel.Text = "Creatures";
            rootPanel.Height = 800;
            rootPanel.Width = 1280;
            rootPanel.Location = new Point(leftBarWidth + 12, 12);

            int row_length = 6;
            int row_height = 225;

            for(int i=0; i<creatures.Count; i+= row_length)
            {
                GroupBox rootGB = new GroupBox();
                rootGB.Parent = rootPanel;
                rootGB.Height = row_height;
                rootGB.Width = 1200;
                rootGB.Location = new Point(0, (i/ row_length) * row_height);
                for (int j=0; j< row_length; j++)
                {
                    if(creatures.Count <= i+j)
                    {
                        break;
                    }
                    GroupBox thisGB = new GroupBox();
                    MonsterType mt = creatures[i + j];
                    thisGB.Text = mt.name;
                    thisGB.Height = row_height;
                    thisGB.Width = 200;
                    thisGB.Parent = rootGB;
                    thisGB.Location = new Point(thisGB.Width * (j % row_length), 0);

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

                    Button newEncButton = new Button();
                    newEncButton.Text = "New Encounter";
                    newEncButton.Height = 25;
                    newEncButton.Width = 125;
                    newEncButton.Parent = thisGB;
                    newEncButton.Location = new Point(12, 165);
                    newEncButton.Click += StartNewEncounterWithCreature(mt);

                    if (isMobCF)
                    {
                        Button chooseButton = new Button();
                        chooseButton.Text = "Choose";
                        chooseButton.Height = 25;
                        chooseButton.Width = 125;
                        chooseButton.Parent = thisGB;
                        chooseButton.Location = new Point(12, 165 + 25);
                        chooseButton.Click += ChooseThisCreatureForEncounter(CombatRating.CRStringToEnum(mt.cr), neb, mt);
                    }
                }
            }
            this.Controls.Add(rootPanel);
            creatureListPanel = rootPanel;
        }
        public System.EventHandler ChooseThisCreatureForEncounter(CombatRating.CR cr, NewEncounterBuilder neb, MonsterType thisMob)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                neb.SwapCRMobForMob(cr, thisMob);
                this.Close();
            }
            return ThisButton_Click;
        }
        private List<MonsterType> GenerateFilteredCreatureListGeneral(bool isMobCF, CombatRating.CR cr, CheckedData checkData, CheckedListBox clb, string itemName)
        {
            // cr input only considered when isMobCF

            // generate filters
            List<string> crFilters = new List<string>();
            if (isMobCF)
            {
                crFilters.Add(CombatRating.CREnumToString(cr));
            }
            else
            {
                foreach (string cItem in crCLB.CheckedItems)
                {
                    crFilters.Add(cItem);
                }
                if (checkData == CheckedData.Checked && clb == crCLB)
                {
                    crFilters.Add(itemName);
                }
                if (checkData == CheckedData.Unchecked && clb == crCLB)
                {
                    crFilters.Remove(itemName);
                }
            }

            List<string> sizeFilters = new List<string>();
            foreach (string cItem in sizeCLB.CheckedItems)
            {
                sizeFilters.Add(cItem);
            }
            if(checkData == CheckedData.Checked && clb == sizeCLB)
            {
                sizeFilters.Add(itemName);
            }
            if (checkData == CheckedData.Unchecked && clb == sizeCLB)
            {
                sizeFilters.Remove(itemName);
            }

            List<string> typeFilters = new List<string>();
            foreach (string cItem in typeCLB.CheckedItems)
            {
                typeFilters.Add(cItem);
            }
            if (checkData == CheckedData.Checked && clb == typeCLB)
            {
                typeFilters.Add(itemName);
            }
            if (checkData == CheckedData.Unchecked && clb == typeCLB)
            {
                typeFilters.Remove(itemName);
            }


            List<string> tagsFilters = new List<string>();
            foreach (string cItem in tagsCLB.CheckedItems)
            {
                tagsFilters.Add(cItem);
            }
            if (checkData == CheckedData.Checked && clb == tagsCLB)
            {
                tagsFilters.Add(itemName);
            }
            if (checkData == CheckedData.Unchecked && clb == tagsCLB)
            {
                tagsFilters.Remove(itemName);
            }

            List<string> sectionFilters = new List<string>();
            foreach (string cItem in sectionCLB.CheckedItems)
            {
                sectionFilters.Add(cItem);
            }
            if (checkData == CheckedData.Checked && clb == sectionCLB)
            {
                sectionFilters.Add(itemName);
            }
            if (checkData == CheckedData.Unchecked && clb == sectionCLB)
            {
                sectionFilters.Remove(itemName);
            }

            List<string> alignmentFilters = new List<string>();
            foreach (string cItem in alignmentCLB.CheckedItems)
            {
                alignmentFilters.Add(cItem);
            }
            if (checkData == CheckedData.Checked && clb == alignmentCLB)
            {
                alignmentFilters.Add(itemName);
            }
            if (checkData == CheckedData.Unchecked && clb == alignmentCLB)
            {
                alignmentFilters.Remove(itemName);
            }

            List<string> environmentFilters = new List<string>();
            foreach (string cItem in environmentCLB.CheckedItems)
            {
                environmentFilters.Add(cItem);
            }
            if (checkData == CheckedData.Checked && clb == environmentCLB)
            {
                environmentFilters.Add(itemName);
            }
            if (checkData == CheckedData.Unchecked && clb == environmentCLB)
            {
                environmentFilters.Remove(itemName);
            }


            List<MonsterType> filteredList = new List<MonsterType>();
            foreach (MonsterType mt in MonsterManual.MyManual)
            {
                if
                    (
                        (mt.name.ToLower().Contains(nameFilter.Text.ToLower()) || nameFilter.Text == "")
                     && (crFilters.Contains(mt.cr) || crFilters.Count == 0)
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
        public System.EventHandler StartNewEncounterWithCreature(MonsterType thisMob)
        {
            void ThisButton_Click(object sender, EventArgs e)
            {
                NewEncounterBuilder EBform = new NewEncounterBuilder(thisMob);
                EBform.Show();
            }
            return ThisButton_Click;
        }
    }
}
