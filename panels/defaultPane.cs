using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Powercore.panels
{
    public partial class defaultPane : Form
    {
        public defaultPane()
        {
            InitializeComponent();
        }

        private void defaultPane_Load(object sender, EventArgs e)
        {
            tabBox.Text = Powercore.Properties.Settings.Default.newTabLocation;
            if (Powercore.Properties.Settings.Default.background == "Off")
            {
                bgToggle.Checked = false;
                bgBox.ReadOnly = true;
                bgBox.BackColor = SystemColors.ControlDark;
            } else
            {
                bgToggle.Checked = true;
                bgBox.ReadOnly = false;
                bgBox.BackColor = SystemColors.Control;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Powercore.Properties.Settings.Default.newTabLocation = tabBox.Text;
            Properties.Settings.Default.Save();
            Console.WriteLine(Powercore.Properties.Settings.Default.newTabLocation);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bgBox.Text == null)
            {
                Powercore.Properties.Settings.Default.background = " ";
                Properties.Settings.Default.Save();
            }
            else
            {
                Powercore.Properties.Settings.Default.background = bgBox.Text;
                Properties.Settings.Default.Save();
            }
            MessageBox.Show("Restart your browser for changes to take effect.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bgToggle.Checked == false)
            {
                Powercore.Properties.Settings.Default.background = "Off";
                Properties.Settings.Default.Save();
                bgBox.ReadOnly = true;
                bgBox.BackColor = SystemColors.ControlDark;
            } else
            {
                Powercore.Properties.Settings.Default.background = "";
                Properties.Settings.Default.Save();
                bgBox.ReadOnly = false;
                bgBox.BackColor = SystemColors.Control;
            }
        }
    }
}
