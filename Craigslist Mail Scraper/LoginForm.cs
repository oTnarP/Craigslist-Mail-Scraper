using System;
using System.Drawing;
using System.Windows.Forms;
using VerifyUsers;

namespace Craigslist_Mail_Scraper
{
    public partial class LoginForm : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnExit;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        

        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text == "USERNAME")
            {
                txtUser.Text = "";
            }
        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                txtUser.Text = "USERNAME";
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "PASSWORD")
            {
                txtPass.Text = "";
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (txtPass.Text == "")
            {
                txtPass.Text = "PASSWORD";
            }
        }

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void LoginForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void LoginForm_Click(object sender, EventArgs e)
        {
            this.ActiveControl = btnExit;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "USERNAME")
            {
                MessageBox.Show("Please, Enter your Username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            else if (txtPass.Text == "PASSWORD")
            {
                MessageBox.Show("Please, Enter your Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            else
            {
                try
                {
                    Registration rg = new Registration();
                    rg.CheckInfo(txtUser, txtPass, "CMScraper");

                    if (rg.Report == "Active")
                    {
                        var mf = new Main();
                        mf.Show();
                        this.Hide();
                    }

                    else if (rg.Report == "Deactive")
                    {
                        MessageBox.Show("Your license is expired!!", "Important Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    else
                    {
                        MessageBox.Show("Username or Password is incorrect.", "Important Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    MessageBox.Show("Please check your connection!", "Important Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}
