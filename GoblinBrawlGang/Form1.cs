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

        private struct PlayerType
        {
            public int id;
            public int level;
        }

        private List<PlayerType> players = new List<PlayerType>();

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
        }


        private void CreateButtonPlease(int yValue, string name, EventHandler func)
        {
            Button up = new Button();
            up.Text = name;
            up.AutoSize = true;
            up.Location = new Point(680, 105 * yValue);
            up.Click += func;
            this.Controls.Add(up);
        }

        private void GraphicsUpdate()
        {
            this.Controls.Clear();

            int thisthPlayer = 0;
            foreach (PlayerType plyr in players)
            {
                PlayerType thisPlayer = plyr;
                GroupBox newPlayer = new GroupBox();
                newPlayer.Text = "Player " + plyr.id;
                newPlayer.AutoSize = true;
                newPlayer.Location = new Point(12, 12 + 105 * thisthPlayer);

                Label level = new Label();
                level.Text = plyr.level.ToString();
                level.AutoSize = true;
                level.Parent = newPlayer;
                level.Location = new Point(newPlayer.Width/2, newPlayer.Height/2);

                void IncrementLevel(object sender, EventArgs e)
                {
                    int curLev = int.Parse(level.Text);
                    level.Text = curLev switch
                    {
                        (>= 20) => "20",
                        _ => (curLev + 1).ToString()
                    };
                    thisPlayer.level = int.Parse(level.Text);
                }
                void DecrementLevel(object sender, EventArgs e)
                { 
                    int curLev = int.Parse(level.Text);
                    level.Text = curLev switch
                    {
                        (<= 1) => "1",
                        _ => (curLev - 1).ToString()
                    };
                    thisPlayer.level = int.Parse(level.Text);
                }

                Button up = new Button();
                up.Text = "+";
                up.AutoSize = true;
                up.Parent = newPlayer;
                up.Location = new Point(12, newPlayer.Height / 4);
                up.Click += IncrementLevel;

                Button down = new Button();
                down.Text = "-";
                down.AutoSize = true;
                down.Parent = newPlayer;
                down.Location = new Point(12, newPlayer.Height / 4 + 30);
                down.Click += DecrementLevel;

                this.Controls.Add(newPlayer);

                thisthPlayer++;
            }
            LoadMenu();

        }

        private void AddPlayer(object sender, EventArgs e)
        {
            players.Add(new PlayerType { id = unique_id, level = 1 });
            GraphicsUpdate();
        }
        private void RemovePlayer(object sender, EventArgs e)
        {
            players.RemoveAt(0);
            GraphicsUpdate();
        }
        private void RemovePlayer()
        {

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
