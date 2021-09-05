using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace GoblinBrawlGang
{
    public class Player
    {
        public int id = -1;
        public int level = -1;

        public Player(int inputID, int lvl)
        {
            id = inputID;
            level = lvl;
        }
        public void IncrementLevel()
        {
            level++;
        }
        public void DecrementLevel()
        {
            level--;
        }
        public GroupBox DrawSelf(int position)
        {
            GroupBox playerBox = new GroupBox();
            playerBox.Text = "Player " + id;
            playerBox.Height = 105;
            playerBox.Width = 100;
            playerBox.Location = new Point(12, 12 + 105 * position);

            Label levelLabel = new Label();
            levelLabel.Text = level.ToString();
            levelLabel.AutoSize = true;
            levelLabel.Parent = playerBox;
            levelLabel.Location = new Point(playerBox.Width / 2, playerBox.Height / 2);

            void IncrementLevel(object sender, EventArgs e)
            {
                levelLabel.Text = level switch
                {
                    (>= 20) => "20",
                    _ => (level + 1).ToString()
                };
                level = int.Parse(levelLabel.Text);
            }
            void DecrementLevel(object sender, EventArgs e)
            {
                levelLabel.Text = level switch
                {
                    (<= 1) => "1",
                    _ => (level - 1).ToString()
                };
                level = int.Parse(levelLabel.Text);
            }

            Button up = new Button();
            up.Text = "+";
            up.Height = 25;
            up.Width = 25;
            up.Parent = playerBox;
            up.Location = new Point(12, playerBox.Height / 4);
            up.Click += IncrementLevel;

            Button down = new Button();
            down.Text = "-";
            down.Height = 25;
            down.Width = 25;
            down.Parent = playerBox;
            down.Location = new Point(12, playerBox.Height / 4 + 30);
            down.Click += DecrementLevel;

            return playerBox;
        }
    }
}
