using Microsoft.Reporting.WinForms;
using project_pos.BLL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_pos
{
    public partial class PurchaseReportFrm : Form
    {
        private readonly ReportBLL _reportBLL = new ReportBLL();
        public PurchaseReportFrm()
        {
            InitializeComponent();
        }
        private void PurchaseReportFrm_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
        private void btnReportPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                // ១. ទាញយកថ្ងៃខែដែលអ្នកប្រើប្រាស់បានជ្រើសរើស
                DateTime fromDate = dtpFromDate.Value;
                DateTime toDate = dtpToDate.Value;
                // ២. ផ្ញើថ្ងៃខែទៅទាញយកទិន្នន័យពី BLL
                DataSet ds = _reportBLL.GetPurchaseReportData(fromDate, toDate);
                reportViewer1.LocalReport.ReportEmbeddedResource = "project_pos.Report.rptPurchaseInvoice.rdlc";
                reportViewer1.ProcessingMode = ProcessingMode.Local;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Purchase Report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearReport_Click(object sender, EventArgs e)
        {
            this.reportViewer1.Clear();
        }
    }
}
