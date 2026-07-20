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
    public partial class PurchaseFrm : Form
    {
        private readonly SupplierBLL _supplierBLL = new SupplierBLL();
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly StockInBLL _stockInBLL = new StockInBLL();
        private readonly List<StockIn> _purchaseCart = new List<StockIn>();
        public PurchaseFrm()
        {
            InitializeComponent();
            LoadSuppliers();
            LoadProducts();
            RefreshCart();
            cboSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadSuppliers()
        {
            try
            {
                cboSupplier.DisplayMember = "SupplierName";
                cboSupplier.ValueMember = "SupplierId";
                cboSupplier.DataSource = _supplierBLL.GetAllSuppliers();
                cboSupplier.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadProducts()
        {
            try
            {
                cboProduct.DisplayMember = "ProductName";
                cboProduct.ValueMember = "ProductID";
                cboProduct.DataSource = _productBLL.GetAllProducts();
                cboProduct.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtUnit.Text, out decimal unitCost) || unitCost < 0)
            {
                MessageBox.Show("Please enter a valid unit cost.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int productId = Convert.ToInt32(cboProduct.SelectedValue);
            DataRowView selectedProduct = (DataRowView)cboProduct.SelectedItem;
            string productName = selectedProduct["ProductName"].ToString();
            // បើទំនិញមានក្នុង Cart រួចហើយ គ្រាន់តែបូកថែម Qty
            var existingItem = _purchaseCart.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Qty += qty;
                existingItem.UnitCost = unitCost; // អាប់ដេតតម្លៃដើមថ្មី
            }
            else
            {
                _purchaseCart.Add(new StockIn
                {
                    ProductId = productId,
                    ProductName = productName,
                    Qty = qty,
                    UnitCost = unitCost,
                    Note = txtNote.Text.Trim()
                });
            }
            RefreshCart();
            txtQty.Clear();
            txtUnit.Clear();
            txtNote.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (dgvPurchases.CurrentRow != null && dgvPurchases.CurrentRow.Index >= 0)
            {
                int selectedIndex = dgvPurchases.CurrentRow.Index;
                if (selectedIndex < _purchaseCart.Count)
                {
                    _purchaseCart.RemoveAt(selectedIndex);
                    RefreshCart();
                }
            }
            else
            {
                MessageBox.Show("Please select an item from the cart to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshCart()
        {
            dgvPurchases.DataSource = null;
            dgvPurchases.DataSource = _purchaseCart.Select(item => new
            {
                item.ProductName,
                item.Qty,
                item.UnitCost,
                TotalCost = item.Qty * item.UnitCost,
                item.Note
            }).ToList();
            decimal grandTotal = _purchaseCart.Sum(item => item.Qty * item.UnitCost);
            lblGrandTotal.Text = grandTotal.ToString("N2");
        }

        private void btnSavePurchase_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_purchaseCart.Any())
                {
                    MessageBox.Show("Purchase cart is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cboSupplier.SelectedIndex == -1 || cboSupplier.SelectedValue == null)
                {
                    MessageBox.Show("Please select a supplier.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int supplierId = Convert.ToInt32(cboSupplier.SelectedValue);
                int successCount = 0;
                // រក្សាទុកគ្រប់ Item ក្នុង Cart ចូលតារាង StockIn (StockInDAL នឹង Auto Update StockQty ក្នុង Products)
                foreach (var item in _purchaseCart)
                {
                    item.SupplierId = supplierId;
                    item.StockInDate = DateTime.Now;
                    if (_stockInBLL.Save(item))
                    {
                        successCount++;
                    }
                }
                if (successCount > 0)
                {
                    MessageBox.Show($"Purchase completed successfully!\n{successCount} item(s) added to StockIn & Product stock updated automatically.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _purchaseCart.Clear();
                    RefreshCart();
                    cboSupplier.SelectedIndex = -1;
                    cboProduct.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing purchase: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
