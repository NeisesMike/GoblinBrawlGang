using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace GoblinBrawlGang
{
    public partial class GoblinBrawlGang : Form
    {
        private static int defaultPlayerLevel = 1;
        private int _num_buttons = 0;
        private int num_buttons
        {
            get
            {
                int ret = _num_buttons;
                _num_buttons++;
                return ret;
            }
            set
            {
                _num_buttons = 0;
            }
        }
        private int _unique_id = 0;
        private int unique_id
        {
            get
            {
                int ret = _unique_id;
                _unique_id++;
                return ret;
            }
        }
        public static List<GroupBox> myEncounters = new List<GroupBox>();

        public static Queue<Player> players = new Queue<Player>();

        public static void InitDebugPlayerList()
        {
            for(int i=0; i<5; i++)
            {
                players.Enqueue(new Player(i, 5));
            }
        }
        public GoblinBrawlGang()
        {
            this.AutoSize = true;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MonsterManual.InitManual();
            InitDebugPlayerList();
            this.GraphicsUpdate();
        }
        private void LoadMenu()
        {
            CreateButtonPlease("Reset Players", ResetPlayers);
            CreateButtonPlease("Add Player", AddPlayer);
            CreateButtonPlease("Remove Player", RemovePlayer);
            CreateButtonPlease("Easy Encounter", CreateEncounters(Difficulty.Easy));
            CreateButtonPlease("Medium Encounter", CreateEncounters(Difficulty.Medium));
            CreateButtonPlease("Hard Encounter", CreateEncounters(Difficulty.Hard));
            CreateButtonPlease("Deadly Encounter", CreateEncounters(Difficulty.Deadly));
            CreateDropdownPlease("Default Player Level");
        }
        private void CreateButtonPlease(string name, EventHandler func)
        {
            Button up = new Button();
            up.Text = name;
            up.AutoSize = true;
            up.Location = new Point(650, 25 * num_buttons);
            up.Click += func;
            this.Controls.Add(up);
        }
        private void CreateDropdownPlease(string name)
        {
            GroupBox thisGB = new GroupBox();
            thisGB.Text = name;
            thisGB.Height = 125;
            thisGB.Width = 125;
            thisGB.Location = new Point(125, 12);

            ListBox thisBox = new ListBox();
            thisBox.Parent = thisGB;
            thisBox.Text = "Default Player Level";
            thisBox.Size = new System.Drawing.Size(100, 100);
            thisBox.Location = new System.Drawing.Point(12, 24);

            // Shutdown the painting of the ListBox as items are added.
            thisBox.BeginUpdate();
            // Loop through and add 50 items to the ListBox.
            for (int i = 1; i <= 20; i++)
            {
                thisBox.Items.Add(i);
            }
            // Allow the ListBox to repaint and display the new items.
            thisBox.EndUpdate();

            void DropdownSelect(object sender, System.EventArgs e)
            {
                // Add event handler code here.
                defaultPlayerLevel = (int)thisBox.SelectedItem;
            }
            thisBox.Click += new EventHandler(DropdownSelect);



            this.Controls.Add(thisGB);
        }

        private void WipeScreen()
        {
            this.Controls.Clear();
            num_buttons = 0;
        }
        private void GraphicsUpdate()
        {
            WipeScreen();

            int thisthPlayer = 0;
            foreach (Player plyr in players)
            {
                GroupBox thisPlayerGB = plyr.DrawSelf(thisthPlayer);
                thisthPlayer++;
                Controls.Add(thisPlayerGB);
            }

            LoadMenu();
        }
        private void ResetPlayers(object sender, EventArgs e)
        {
            players.Clear();
            GraphicsUpdate();
        }
        private void AddPlayer(object sender, EventArgs e)
        {
            players.Enqueue(new Player(unique_id, defaultPlayerLevel));
            GraphicsUpdate();
        }
        private void RemovePlayer(object sender, EventArgs e)
        {
            if (players.Count > 0)
            {
                players.Dequeue();
                GraphicsUpdate();
            }
        }
        private System.EventHandler CreateEncounters(Difficulty diff)
        {
            void UpdateEncounters(object sender, EventArgs e)
            {
                foreach(GroupBox gb in myEncounters)
                {
                    Controls.Remove(gb);
                }
                myEncounters = XPManager.CreateEncounters(players, diff);

                int currentRow = 12;
                for (int i = 0; i < myEncounters.Count; i++)
                {
                    GroupBox gb = myEncounters[i];
                    gb.Location = new Point(275, currentRow);
                    currentRow += gb.Height + 75;
                    Controls.Add(gb);
                }
            }
            return UpdateEncounters;
        }




        public void temp()
        {
            Label namelabel = new Label();
            namelabel.Location = new Point(13, 13);
            namelabel.Text = "greeter";
            namelabel.AutoSize = true;
            this.Controls.Add(namelabel);
        }
    }
}
