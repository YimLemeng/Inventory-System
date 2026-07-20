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
    public partial class StockInFrm : Form
    {
        private readonly StockInBLL _stockInBLL = new StockInBLL();
        public StockInFrm()
        {
            InitializeComponent();
            LoadStockInRecords();
        }

        public void LoadStockInRecords()
        {
            try
            {
                dgvStockIn.DataSource = null;
                dgvStockIn.DataSource = _stockInBLL.GetAllStockIns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock in records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }
        private void PerformSearch()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    LoadStockInRecords(); 
                }
                else
                {
                    dgvStockIn.DataSource = null;
                    dgvStockIn.DataSource = _stockInBLL.SearchStockIn(keyword);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }
    }
}
