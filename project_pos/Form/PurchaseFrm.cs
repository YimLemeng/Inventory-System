using project_pos.BLL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            if (cboSupplier.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a supplier for this item.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
            // ទាញយក ID & Name របស់ Product និង Supplier
            int productId = Convert.ToInt32(cboProduct.SelectedValue);
            DataRowView selectedProduct = (DataRowView)cboProduct.SelectedItem;
            string productName = selectedProduct["ProductName"].ToString();
            int supplierId = Convert.ToInt32(cboSupplier.SelectedValue);
            DataRowView selectedSupplier = (DataRowView)cboSupplier.SelectedItem;
            string supplierName = selectedSupplier["SupplierName"].ToString();
            // បន្ថែមចូល Cart ដោយភ្ជាប់ SupplierId របស់ Item នោះស្រាប់
            _purchaseCart.Add(new StockIn
            {
                ProductId = productId,
                ProductName = productName,
                SupplierId = supplierId,
                SupplierName = supplierName, // ភ្ជាប់ឈ្មោះ Supplier ជាមួយ Item នេះ
                Qty = qty,
                UnitCost = unitCost,
                Note = txtNote.Text.Trim()
            });
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
                // 1. បង្កើត Purchase Header
                Purchase purchase = new Purchase
                {
                    SupplierId = _purchaseCart.First().SupplierId ?? 0,
                    PurchaseDate = DateTime.Now,
                    TotalAmount = _purchaseCart.Sum(x => x.Qty * x.UnitCost),
                    Note = _purchaseCart.FirstOrDefault()?.Note ?? ""
                };
                // 2. បំលែង Cart ទៅជា List<PurchaseDetail>
                List<PurchaseDetail> details = _purchaseCart.Select(item => new PurchaseDetail
                {
                    ProductId = item.ProductId,
                    SupplierId = item.SupplierId ?? 0,
                    Qty = item.Qty,
                    UnitCost = item.UnitCost,
                    LineTotal = item.Qty * item.UnitCost
                }).ToList();
                // 3. ហៅ PurchaseBLL រក្សាទុក (ចូល Purchases, PurchaseDetails, StockIn & Products Stock)
                PurchaseBLL purchaseBLL = new PurchaseBLL();
                int newPurchaseId = purchaseBLL.SavePurchase(purchase, details);
                if (newPurchaseId > 0)
                {
                    MessageBox.Show($"Purchase #{newPurchaseId} saved successfully!\nData saved to Purchases, PurchaseDetails & StockIn tables.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _purchaseCart.Clear();
                    RefreshCart();
                    cboSupplier.SelectedIndex = -1;
                    cboProduct.SelectedIndex = -1;
                    txtNote.Clear();
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
