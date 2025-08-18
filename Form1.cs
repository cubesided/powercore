using CefSharp;
using CefSharp.WinForms;
using Microsoft.Win32;
using Powercore.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Configuration;


namespace Powercore
{
    public partial class mainWnd : Form
    {

        bool donttaptheglass = true;
        public mainWnd()
        {
            // Initialize Cache
            string bg = Powercore.Properties.Settings.Default.background;
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            if (bg == "")
            {
                GrabDesktopImage();
            } else
            {
                try
                {
                    // Get the background image path from the registry
                    string imagePath = Powercore.Properties.Settings.Default.background;

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        // Load the image
                        Image backgroundImage = Image.FromFile(imagePath);

                        // Set the form's background image
                        this.BackgroundImage = backgroundImage;
                        this.BackgroundImageLayout = ImageLayout.None; // Or another layout as needed
                    }
                    else
                    {
                        MessageBox.Show("Could not retrieve background image path.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred\nNot an image/valid type: {ex.Message}\n\nBrowser can continue.");
                }
            }
        }

        private void GrabDesktopImage()
        {
            try
            {
                // Get the background image path from the registry
                string imagePath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", null);

                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Load the image
                    Image backgroundImage = Image.FromFile(imagePath);

                    // Set the form's background image
                    this.BackgroundImage = backgroundImage;
                    this.BackgroundImageLayout = ImageLayout.None; // Or another layout as needed
                }
                else
                {
                    MessageBox.Show("Could not retrieve background image path.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void mainWnd_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            crhmvs.Text = "Chromium " + Cef.ChromiumVersion;

            TabPage newTabPage = new TabPage("New Tab");
            tabControl1.TabPages.Add(newTabPage);

            ChromiumWebBrowser webControl = new ChromiumWebBrowser();
            webControl.Dock = DockStyle.Fill;
            newTabPage.Controls.Add(webControl);
            webControl.Parent = newTabPage;


            search(Powercore.Properties.Settings.Default.newTabLocation);

            webControl.TitleChanged += webControl_TitleChanged;
            webControl.AddressChanged += webControl_AddressChanged;
            webControl.KeyDown += webControl_KeyDown;
            newTabPage.MouseDoubleClick += NewTabPage_MouseClick;
            webControl.StatusMessage += webControl_Status;
            webControl.LoadError += webControl_URLError;

            tabControl1.SelectTab(tabControl1.TabCount - 1);
        }

        private void btnQuick_Click(object sender, EventArgs e)
        { /*
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
                search("@/powercore/quickmenu");
            MessageBox.Show("Adding menu later, currently about box :P");
            about about = new about();
            about.StartPosition = FormStartPosition.CenterScreen;
            about.Show(); */
            if (donttaptheglass)
            {
                qckmen.Visible = true;
                donttaptheglass = false;
            } else
            {
                qckmen.Visible = false;
                donttaptheglass = true;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
                search(urlBox.Text);
            Console.WriteLine(urlBox.Text);
        }

        private void urlBox_KeyDown(object sender, KeyEventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    search(urlBox.Text);
                    Console.WriteLine(urlBox.Text);
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
        }

        private void search(string given)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null) {
                if (webControl.Address == "https://www.google.com/search?q=@/powercore/settings")
                {
                    webControl.Show();
                }

                if (given.StartsWith("http://") || given.StartsWith("https://") || given.Contains("."))
                {
                    webControl.LoadUrl(given);
                    Console.WriteLine(given);
                }
                else { webControl.LoadUrl("https://google.com/search?q=" + given); Console.WriteLine(given); }

                // @/ handler
                if (given.StartsWith("@/"))
                {
                    string handle = "@/";
                    string use = given.Replace(handle, "https://cubesided.github.io/data/");
                    use += ".html";

                    if (given == "@/powercore/settings") {
                        SettingsPane settingsPane = new SettingsPane();
                        settingsPane.TopLevel = false;
                        settingsPane.Show();
                        tabControl1.SelectedTab.Controls.Add(settingsPane);
                        webControl.Hide();
                        settingsPane.Parent = tabControl1.SelectedTab;
                        settingsPane.Size = tabControl1.SelectedTab.Size;
                    } else
                    {
                        webControl.LoadUrl(use);
                    }
                }
            } 
        }

        private void urlBox_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
            {
                if (urlBox.Text == webControl.Address || urlBox.Text.StartsWith("@/"))
                {
                    urlBox.Text = "";
                }
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            // Perform cleanup or save data here
            Cef.Shutdown();
        }

        private void btnRefersh_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
                webControl.Reload();
        }

        public void AddNewTab(string url)
        {
            TabPage newTabPage = new TabPage("New Tab");
            tabControl1.TabPages.Add(newTabPage);

            newTabPage.MouseDoubleClick += NewTabPage_MouseClick;

            ChromiumWebBrowser webControl = new ChromiumWebBrowser();
            tabControl1.SelectTab(tabControl1.TabCount - 1);
            webControl.Dock = DockStyle.Fill;
            newTabPage.Controls.Add(webControl);
            webControl.Parent = newTabPage;
            webControl.Dock = DockStyle.Fill;
            webControl.TitleChanged += webControl_TitleChanged;
            webControl.AddressChanged += webControl_AddressChanged;
            webControl.KeyDown += webControl_KeyDown;
            webControl.StatusMessage += webControl_Status;
            webControl.LoadError += webControl_URLError;

            search(url);
            //search(url);
        }
        private async void webControl_URLError(object sender, LoadErrorEventArgs e)
        { 
            string use = null;
            string given = e.FailedUrl;
            bool exists = await UrlChecker.DoesUrlExist(given);
            if (!exists) {
                Console.WriteLine("URL could not be found,\nsearching instead...");
                use = given.ReplaceFirst("http://", "");
                use = use.ReplaceLast('/', ' ');

                e.Browser.MainFrame.LoadUrl("https://google.com/search?q=" + use);
            }
        }

        private void webControl_Status(object sender, StatusMessageEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                richTextBox1.Text = e.Value;
            }));
        }

        private void NewTabPage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                tabControl1.SelectedTab.Dispose();
            }
        }

        private void webControl_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (e.Address.StartsWith("https://cubesided.github.io/data/"))
                {
                    string given = e.Address;
                    string handle = "https://cubesided.github.io/data/";
                    string first = given.Replace(handle, "@/");
                    string use = first.Replace(".html", "");

                    urlBox.Text = use;
                }
                else { urlBox.Text = e.Address; }
            }));
        }

        private void webControl_KeyDown(object sender, KeyEventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
            {
                if (e.KeyCode == Keys.F5)
                {
                    webControl.Reload();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void webControl_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                tabControl1.SelectedTab.Text = e.Title;
                if (e.Title.EndsWith("quickmenu.html"))
                {
                    tabControl1.SelectedTab.Text = "New Tab";
                }
            }));
        }

        private void btnTab_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Powercore.Properties.Settings.Default.newTabLocation);
            AddNewTab(Powercore.Properties.Settings.Default.newTabLocation);
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            // Close tab
            if (e.Button == MouseButtons.Middle)
            {
                //Looping through the controls.
                for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
                {
                    Rectangle r = tabControl1.GetTabRect(i);
                    //Getting the position of the "x" mark.
                    Rectangle closeButton = r;
                    if (closeButton.Contains(e.Location))
                    {
                        if (i == 0)
                        { this.tabControl1.TabPages.RemoveAt(i); } 

                        else if (tabControl1.SelectedTab == tabControl1.TabPages[i])
                        { this.tabControl1.SelectTab(i - 1); this.tabControl1.TabPages.RemoveAt(i); }
                        else { this.tabControl1.TabPages.RemoveAt(i); }

                        if (this.tabControl1.TabPages.Count == 0) { Application.Exit(); }
                        break;
                    }
                }
            }
            else { /* void */}
        }

        // quickmenu

        private void button2_Click(object sender, EventArgs e)
        {
            mainWnd mainWnd = new mainWnd();
            mainWnd.Show();
            qckmen.Visible = false;
            donttaptheglass = true;
        }

        private void qckactins_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
                search("@/powercore/quickmenu");
            qckmen.Visible = false;
            donttaptheglass = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNewTab(Powercore.Properties.Settings.Default.newTabLocation);
            qckmen.Visible = false;
            donttaptheglass = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser webControl = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (webControl != null)
                webControl.ShowDevTools();

            qckmen.Visible = false;
            donttaptheglass = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            about about = new about();
            about.StartPosition = FormStartPosition.CenterScreen;
            about.Show();

            qckmen.Visible = false;
            donttaptheglass = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddNewTab("@/powercore/settings");
            qckmen.Visible = false;
            donttaptheglass = true;
        }
    }
}

public static class StringExtensions
{
    public static string ReplaceFirst(this string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text; // Search string not found
        }
        if (pos == 0) // Check if the search string is at the beginning
        {
            return text.Remove(pos, search.Length).Insert(pos, replace);
        }
        return text; // Search string found but not at the beginning, so no action
    }

    public static string ReplaceLast(this string input, char oldChar, char newChar)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        int lastSpaceIndex = input.LastIndexOf(' ');

        // If there's no space, the entire string is the last word.
        if (lastSpaceIndex == -1)
        {
            return input.Replace(oldChar, newChar);
        }
        else
        {
            // Extract the part of the string before the last word.
            string beginningOfString = input.Substring(0, lastSpaceIndex + 1);

            // Extract the last word itself.
            string lastWord = input.Substring(lastSpaceIndex + 1);

            // Replace the specified letter in the last word.
            string modifiedLastWord = lastWord.Replace(oldChar, newChar);

            // Combine the parts to form the new string.
            return beginningOfString + modifiedLastWord;
        }
    }
}