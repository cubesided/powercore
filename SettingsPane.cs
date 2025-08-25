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

        private void label6_Click(object sender, EventArgs e)
        {
            panels.defaultPane defaults = new panels.defaultPane();

            defaults.TopLevel = false;
            defaults.Parent = panel1;
            defaults.StartPosition = FormStartPosition.CenterParent;
            defaults.Show();
            panel1.Controls.Add(defaults);
            this.Focus();
        }

        private void SettingsPane_Load(object sender, EventArgs e)
        {
            label6_Click(label1, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
