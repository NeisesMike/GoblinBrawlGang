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
    public partial class Form1 : Form
    {
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
        private GroupBox myEncounters = null;

        private Queue<Player> players = new Queue<Player>();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMenu();
        }
        private void LoadMenu()
        {
            CreateButtonPlease(0, "Add Player", AddPlayer);
            CreateButtonPlease(1, "Remove Player", RemovePlayer);
            CreateButtonPlease(2, "Medium Encounter", CreateMediumEncounters);
        }
        private void CreateButtonPlease(int yValue, string name, EventHandler func)
        {
            Button up = new Button();
            up.Text = name;
            up.AutoSize = true;
            up.Location = new Point(680, 45 * yValue);
            up.Click += func;
            this.Controls.Add(up);
        }

        private void GraphicsUpdate()
        {
            Debug.WriteLine("graphics update");
            this.Controls.Clear();

            int thisthPlayer = 0;
            foreach (Player plyr in players)
            {
                GroupBox thisPlayerGB = plyr.DrawSelf(thisthPlayer);
                thisthPlayer++;
                Controls.Add(thisPlayerGB);
            }

            LoadMenu();
        }

        private void AddPlayer(object sender, EventArgs e)
        {
            players.Enqueue(new Player(unique_id, 1));
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
        private void CreateMediumEncounters(object sender, EventArgs e)
        {
            Controls.Remove(myEncounters);
            myEncounters = XPManager.CreateEncounters(players, Difficulty.Medium);
            Controls.Add(myEncounters);
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
