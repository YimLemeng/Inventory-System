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
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly SupplierBLL _supplierBLL = new SupplierBLL();
        private readonly StockInBLL _stockInBLL = new StockInBLL();
        public StockInFrm()
        {
            InitializeComponent();
            cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadProducts();
            LoadStockInRecords();
            LoadSuppliers();
        }

        private void LoadProducts()
        {
            cboProduct.DataSource = _productBLL.GetAllProducts();
            cboProduct.DisplayMember = "ProductName";
            cboProduct.ValueMember = "ProductId";
            cboProduct.SelectedIndex = -1;
        }

        private void LoadSuppliers()
        {
            try
            {
                cboSupplier.DisplayMember = "SupplierName";
                cboSupplier.ValueMember = "SupplierId";
                cboSupplier.DataSource = _supplierBLL.GetAllSuppliers().Tables[0];
                cboSupplier.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadStockInRecords()
        {
            dgvStockIn.DataSource = null;
            dgvStockIn.DataSource = _stockInBLL.GetAllStockIns();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProduct.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a product.");
                    return;
                }
                StockIn si = new StockIn
                {
                    ProductId = Convert.ToInt32(cboProduct.SelectedValue),
                    Qty = int.TryParse(txtQty.Text, out int q) ? q : 0,
                    UnitCost = decimal.TryParse(txtUnit.Text, out decimal c) ? c : 0,
                    SupplierId = cboSupplier.SelectedIndex == -1 ? (int?)null : Convert.ToInt32(cboSupplier.SelectedValue),
                    Note = txtNote.Text.Trim(),
                    StockInDate = DateTime.Now
                };
                if (_stockInBLL.Save(si))
                {
                    MessageBox.Show("Stock In saved successfully!");
                    LoadStockInRecords();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();
        private void ClearForm()
        {
            cboProduct.SelectedIndex = -1;
            txtQty.Clear();
            txtUnit.Clear();
            cboSupplier.SelectedIndex = -1;
            txtNote.Clear();
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
