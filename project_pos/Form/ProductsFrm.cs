using project_pos.BLL;
using project_pos.DAL;
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
    public partial class ProductsFrm : Form
    {
        Product p = new Product();
        private readonly CategoryBLL _categoryBLL = new CategoryBLL();
        public ProductsFrm()
        {
            InitializeComponent();
            LoadTheme();
            LoadRecord();
            LoadCategory();
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            txtId.ReadOnly = true;
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btns.BackColor = Color.FromArgb(51, 51, 76);
                    btns.ForeColor = Color.Gainsboro;
                    btns.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void LoadRecord()
        {
            try
            {
                ProductDAL pro = new ProductDAL();
                dgvStockProduct.DataSource = pro.GetAllProduct().Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void LoadCategory()
        {
            try
            {
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryId";
                cboCategory.DataSource = _categoryBLL.GetAllCategories();
                cboCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            p.ProductName = txtName.Text;
            p.UnitPrice = decimal.Parse(txtUnitPrice.Text);
            p.StockQty = 0;
            p.Barcode = txtBarCode.Text;
            p.CategoryId = cboCategory.SelectedIndex == -1 ? (int?)null : Convert.ToInt32(cboCategory.SelectedValue);
            ProductBLL probll = new ProductBLL();

            if (probll.Save(p))
            {
                MessageBox.Show("Save successfully");
                LoadRecord();
                ClearForm();
            }
            else MessageBox.Show("Not successfully");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int productId))
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUnitPrice.Text) ||
                string.IsNullOrWhiteSpace(txtStockQty.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) ||
                !int.TryParse(txtStockQty.Text, out int stockQty))
            {
                MessageBox.Show("Invalid number format.");
                return;
            }

            try
            {
                p.ProductID = productId;
                p.ProductName = txtName.Text.Trim();
                p.UnitPrice = unitPrice;
                p.StockQty = stockQty;
                p.Barcode = txtBarCode.Text.Trim();
                p.CategoryId = cboCategory.SelectedIndex == -1 ? (int?)null : Convert.ToInt32(cboCategory.SelectedValue);

                ProductBLL probll = new ProductBLL();
                if (probll.Update(p))
                {
                    MessageBox.Show("Update successfully!");
                    LoadRecord();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Update failed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                p.ProductID = int.Parse(txtId.Text);
                ProductBLL probll = new ProductBLL();
                if (probll.Delete(p.ProductID))
                {
                    MessageBox.Show("Delete successfully");
                    LoadRecord();
                    ClearForm();
                }
                else MessageBox.Show("Not successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvStockProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStockProduct.Rows[e.RowIndex];
                txtId.Text = row.Cells["ProductID"].Value.ToString();
                txtName.Text = row.Cells["ProductName"].Value.ToString();
                txtUnitPrice.Text = row.Cells["UnitPrice"].Value.ToString();
                txtStockQty.Text = row.Cells["StockQty"].Value.ToString();
                txtBarCode.Text = row.Cells["Barcode"].Value.ToString();
                if (row.Cells["CategoryId"].Value != DBNull.Value)
                {
                    cboCategory.SelectedValue = Convert.ToInt32(row.Cells["CategoryId"].Value);
                }
                else
                {
                    cboCategory.SelectedIndex = -1;
                }
            }
        }

        private void txtStockQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtName.Clear();
            txtUnitPrice.Clear();
            txtStockQty.Clear();
            txtBarCode.Clear();
            cboCategory.SelectedIndex = -1;
        }
    }
}
