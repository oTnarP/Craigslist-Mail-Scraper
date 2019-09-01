using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text;

namespace Craigslist_Mail_Scraper
{
    public partial class Main : Form
    {
        Thread th;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            
            if (txtUrl.Text == "" && txtBrowse.Text == "No File Selected")
            {
                MessageBox.Show("Please select your destination first.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (txtBrowse.Text == "No File Selected")
                {
                    if (btnRun.Text == "Run")
                    {
                        SingleUrl su = new SingleUrl();
                        su.Url = txtUrl.Text;
                        su.listView = listView;
                        su.txtLog = txtLog;
                        th = new Thread(() => su.Scrape());
                        th.Start();
                        btnRun.Text = "Stop";
                    }

                    else
                    {
                        th.Abort();
                        btnRun.Text = "Run";
                    }
                }

                else
                {
                    if (btnRun.Text == "Run")
                    {
                        BulkUrl bu = new BulkUrl();
                        bu.listCategories = listCategories;
                        bu.listView = listView;
                        bu.txtLog = txtLog;
                        th = new Thread(() => bu.Scrape());
                        th.Start();
                        btnRun.Text = "Stop";
                    }

                    else
                    {
                        th.Abort();
                        btnRun.Text = "Run";
                    }
                }
                
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.FileName = "Craigslist Leads";
            sfd.Title = "Export to Excel";
            StringBuilder sb = new StringBuilder();
            foreach (ColumnHeader ch in listView.Columns)
            {
                sb.Append(ch.Text + ",");
            }
            sb.AppendLine();
            foreach (ListViewItem lvi in listView.Items)
            {
                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    if (lvs.Text.Trim() == string.Empty)
                        sb.Append(" ,");
                    else
                        sb.Append(lvs.Text + ",");
                }
                sb.AppendLine();
            }

            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                sw.Write(sb.ToString());
                sw.Close();
            }

            MessageBox.Show("The report has been exported.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();

            f.Title = "Browse Text Files";
            f.Filter = "txt files (*.txt)|*.txt";

            if (f.ShowDialog() == DialogResult.OK)
            {
                listCategories.Items.Clear();

                using (StreamReader r = new StreamReader(f.OpenFile()))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listCategories.Items.Add(line);
                        listCategories.SelectedIndex = 0;
                        txtBrowse.Text = f.SafeFileName;
                        txtLog.Text = "Total Categories: " + listCategories.Items.Count;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBrowse.Text = "No File Selected";
            txtUrl.Text = "";
            listCategories.Items.Clear();
            listView.Items.Clear();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
