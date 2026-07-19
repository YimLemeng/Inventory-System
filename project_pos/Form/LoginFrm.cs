using project_pos.BLL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_pos
{
    public partial class LoginFrm : Form
    {
        private readonly UserBLL _userBLL = new UserBLL();
        public LoginFrm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // ១. ហៅ BLL ផ្ទៀងផ្ទាត់គណនី
                User loggedInUser = _userBLL.AuthenticateUser(txtUsername.Text, txtPassword.Text);
                if (loggedInUser != null)
                {
                    MessageBox.Show($"Welcome, {loggedInUser.FullName}!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ២. បង្កើត Form មេ (MainFrm)
                    MainFrm main = new MainFrm(txtUsername.Text);

                    // លាក់ Form Login នេះ រួចបង្ហាញ Form មេ
                    this.Hide();
                    main.ShowDialog();

                    // បិទកម្មវិធីទាំងស្រុង ពេលគេបិទ MainFrm ចោល
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
