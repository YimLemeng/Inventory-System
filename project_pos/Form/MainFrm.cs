using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_pos
{
    public partial class MainFrm : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        public MainFrm(string username)
        {
            InitializeComponent();
            //btnPayment.Visible = false;
            toolUser.Text = username;
            random = new Random();
            btnCloseChildForm.Visible = false;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private Color SelectThemColor()
        {
            int index = random.Next(ThemColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnCloseChildForm.Visible = true;
                }
            }
        } 
        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        public void OpenChildForm(Form childForm, object btnSender)
        {
            if(activeForm != null) activeForm.Close();
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktop.Controls.Add(childForm);
            this.panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitle.Text = childForm.Text;
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ProductsFrm(), sender);
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PurchaseFrm(), sender);
        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if (activeForm != null) activeForm.Close();
            Reset();
        }
        private void Reset()
        {
            DisableButton();
            lblTitle.Text = "Dashboard";
            panelTitleBar.BackColor = panelMenu.BackColor;
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            btnCloseChildForm.Visible = false;
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StockInFrm(), sender);
        }

        private void btnStockOut_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StockOutFrm(), sender);
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            OpenChildForm(new SupplierFrm(), sender);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CategoryFrm(), sender);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PurchaseReportFrm(), sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginFrm login = new LoginFrm();
            this.Hide();
            login.ShowDialog();
        }
    }
}
