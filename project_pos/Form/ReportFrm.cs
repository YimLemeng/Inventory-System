using project_pos.BLL;
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
    public partial class ReportFrm : Form
    {
        private readonly ReportBLL _reportBLL = new ReportBLL();
        public ReportFrm()
        {
            InitializeComponent();
        }

        private void btnViewSales_Click(object sender, EventArgs e)
        {
            try
            {
                dgvReport.DataSource = null;
                dgvReport.DataSource = _reportBLL.GetSalesReport(dtpFrom.Value, dtpTo.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAlertStock_Click(object sender, EventArgs e)
        {
            try
            {
                dgvReport.DataSource = null;
                dgvReport.DataSource = _reportBLL.GetInventoryAlertReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
