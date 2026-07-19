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
    public partial class StockOutFrm : Form
    {
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly StockOutBLL _stockOutBLL = new StockOutBLL();
        public StockOutFrm()
        {
            InitializeComponent();
            LoadProducts();
            LoadStockOutRecords();
            cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadProducts()
        {
            cboProduct.DataSource = _productBLL.GetAllProducts();
            cboProduct.DisplayMember = "ProductName";
            cboProduct.ValueMember = "ProductID";
            cboProduct.SelectedIndex = -1;
        }
        private void LoadStockOutRecords()
        {
            dgvStockOut.DataSource = null;
            dgvStockOut.DataSource = _stockOutBLL.GetAllStockOuts();
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
                StockOut so = new StockOut
                {
                    ProductId = Convert.ToInt32(cboProduct.SelectedValue),
                    Qty = int.TryParse(txtQty.Text, out int q) ? q : 0,
                    Reason = txtReason.Text.Trim(),
                    Note = txtNote.Text.Trim(),
                    StockOutDate = DateTime.Now
                };
                if (_stockOutBLL.Save(so))
                {
                    MessageBox.Show("Stock Out saved successfully!");
                    LoadStockOutRecords(); // Refresh ឡើងវិញ
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
            txtReason.Clear();
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
