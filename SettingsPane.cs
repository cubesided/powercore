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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Powercore
{
    public partial class SettingsPane : Form
    {
        Label currentlySelected;

        // Panels
        panels.defaultPane defaults = null;
        panels.browserPane browser = null;

        public SettingsPane()
        {
            InitializeComponent();
        }

        private void CurrentlySelected_GotFocus(object sender, EventArgs e)
        {
            var labelsOnForm = this.Controls.OfType<Label>().ToList();
            foreach (Label label in labelsOnForm)
            {

                if (label.Name == currentlySelected.Name)
                {
                    // nop
                    currentlySelected.Font = new Font("Segoe UI Light", 12, FontStyle.Underline);
                    //Console.WriteLine("print1");
                }
                else
                {
                    label.Font = new Font("Segoe UI Light", 12, FontStyle.Regular);
                    //Console.WriteLine("prin2t");
                }
            }
            currentlySelected = null;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            currentlySelected = tab0;
            roll();
            defaults = new panels.defaultPane();
            defaults.TopLevel = false;
            defaults.Parent = panel1;
            defaults.StartPosition = FormStartPosition.CenterParent;
            defaults.Show();
            panel1.Controls.Add(defaults);
            this.Focus();
        }

        private void SettingsPane_Load(object sender, EventArgs e)
        {
            label6_Click(tab0, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void tab1_Click(object sender, EventArgs e)
        {

            currentlySelected = tab1;
            roll();
            browser = new panels.browserPane();
            browser.TopLevel = false;
            browser.Parent = panel1;
            browser.StartPosition = FormStartPosition.CenterParent;
            browser.Show();
            panel1.Controls.Add(browser);
            this.Focus();
        }

        private void roll()
        {
            var labelsOnForm = this.Controls.OfType<Label>().ToList();
            foreach (Label label in labelsOnForm)
            {

                if (label.Name == currentlySelected.Name)
                {
                    // nop
                    currentlySelected.Font = new Font("Segoe UI Light", 12, FontStyle.Underline);
                    //Console.WriteLine("print1");
                }
                else
                {
                    label.Font = new Font("Segoe UI Light", 12, FontStyle.Regular);
                    //Console.WriteLine("prin2t");
                    if (label.Text == "Browser")
                    {
                        if (browser != null)
                            browser.Close();
                    }
                    else if (label.Text == "Defaults")
                    {
                        if (defaults != null)
                            defaults.Close();
                    }
                }
            }
        }
    }
}
