using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Powercore
{
    public partial class SettingsPane : Form
    {
        public SettingsPane()
        {
            InitializeComponent();
        }

        private void SettingsPane_Load(object sender, EventArgs e)
        {
            tabBox.Text = Powercore.Properties.Settings.Default.newTabLocation;
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
            } else
            {
                Powercore.Properties.Settings.Default.background = bgBox.Text;
                Properties.Settings.Default.Save();
            }
            MessageBox.Show("Restart your browser for changes to take effect.");
            Application.Restart();
        }
    }
}
