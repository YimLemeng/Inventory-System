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
    public partial class PosPaymentFrm : Form
    {
        private readonly CustomerBLL _customerBLL = new CustomerBLL();
        private readonly ProductBLL _productBLL = new ProductBLL();
        private readonly ReceiptBLL _receiptBLL = new ReceiptBLL();
        private readonly PaymentMethodBLL _paymentMethodBLL = new PaymentMethodBLL();
        private readonly List<ReceiptDetail> _cart = new List<ReceiptDetail>();
        public PosPaymentFrm()
        {
            InitializeComponent();
            LoadData();
            RefreshCart();
            LoadAllReceipts();
            cboCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void LoadData()
        {
            try
            {
                cboCustomer.DataSource = _customerBLL.GetAllCustomers();
                cboCustomer.DisplayMember = "Name";
                cboCustomer.ValueMember = "CustomerId";
                cboCustomer.SelectedIndex = -1;

                cboProduct.DataSource = _productBLL.GetAllProducts();
                cboProduct.DisplayMember = "ProductName";
                cboProduct.ValueMember = "ProductID";
                cboProduct.SelectedIndex = -1;

                cboPaymentType.DataSource = _paymentMethodBLL.GetAllPaymentMethod();
                cboPaymentType.DisplayMember = "MethodName";
                cboPaymentType.ValueMember = "PaymentMethodId";
                cboPaymentType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load data: " + ex.Message);
            }
        }
        private void LoadAllReceipts()
        {
            try
            {
                dgvReceipt.DataSource = null;
                dgvReceipt.DataSource = _receiptBLL.GetAllReceipts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load receipts: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product.");
                return;
            }
            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }
            DataRowView selectedRow = (DataRowView)cboProduct.SelectedItem;

            int productId = Convert.ToInt32(selectedRow["ProductID"]);
            string productName = selectedRow["ProductName"].ToString();
            decimal price = Convert.ToDecimal(selectedRow["UnitPrice"]);
            int stockQty = Convert.ToInt32(selectedRow["StockQty"]);
            if (qty > stockQty)
            {
                MessageBox.Show("Not enough stock.");
                return;
            }

            // Polymorphism: CalculateLineTotal() gives a different result
            // depending on whether the product IsPromo or not
            Product product = new Product
            {
                ProductName = productName,
                UnitPrice = price,
                StockQty = stockQty
            };
            decimal lineTotal = _productBLL.CalculateLineTotal(product, qty);

            _cart.Add(new ReceiptDetail
            {
                ProductId = productId,
                ProductName = productName,
                Price = price,
                Qty = qty,
                LineTotal = lineTotal
            });

            RefreshCart();
            txtQty.Clear();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e) => RefreshCart();
        private void RefreshCart()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cart.Select(d => new
            {
                d.ProductName,
                d.Qty,
                d.Price,
                d.LineTotal
            }).ToList();

            decimal.TryParse(txtDiscount.Text, out decimal discountPercent);
            decimal subTotal = _receiptBLL.CalculateSubTotal(_cart);
            decimal grandTotal = _receiptBLL.CalculateGrandTotal(_cart, discountPercent);
            lblSubtotal.Text = subTotal.ToString("N2");
            lblDiscount.Text = (subTotal - grandTotal).ToString("N2");
            lblGrandtotal.Text = grandTotal.ToString("N2");
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (!_cart.Any())
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            if (cboCustomer.SelectedIndex == -1 || cboCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            int customerId = Convert.ToInt32(cboCustomer.SelectedValue);
            if (cboPaymentType.SelectedIndex == -1 || cboPaymentType.SelectedValue == null)
            {
                MessageBox.Show("Please select a payment method.");
                return;
            }
            int paymentMethodId = Convert.ToInt32(cboPaymentType.SelectedValue);
            decimal.TryParse(txtDiscount.Text, out decimal discountPercent);
            decimal subTotal = _receiptBLL.CalculateSubTotal(_cart);
            decimal grandTotal = _receiptBLL.CalculateGrandTotal(_cart, discountPercent);

            var receipt = new Receipt
            {
                CustomerId = customerId,
                SubTotal = subTotal,
                DiscountPercent = discountPercent,
                GrandTotal = grandTotal,
                PaymentMethodId = paymentMethodId
            };

            try
            {
                var receiptId = _receiptBLL.SaveReceipt(receipt, _cart);
                MessageBox.Show($"Checkout completed.\r\nReceipt #{receiptId}\nGrand Total: {grandTotal:N2}");

                _cart.Clear();
                txtDiscount.Clear();
                RefreshCart();
                LoadData();
                LoadAllReceipts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Checkout failed: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow != null && dgvCart.CurrentRow.Index >= 0)
            {
                int selectedIndex = dgvCart.CurrentRow.Index;
                if (selectedIndex < _cart.Count)
                {
                    _cart.RemoveAt(selectedIndex);
                    RefreshCart();
                }
            }
            else
            {
                MessageBox.Show("Please select an item from the cart to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvCart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCart.Columns[e.ColumnIndex].Name == "colDelete")
            {
                _cart.RemoveAt(e.RowIndex);
                RefreshCart();
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
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

