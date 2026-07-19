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
    public partial class SupplierFrm : Form
    {
        private readonly SupplierBLL _supplierBLL = new SupplierBLL();

        public SupplierFrm()
        {
            InitializeComponent();
            LoadData();
            txtId.ReadOnly = true;
        }
        private void LoadData()
        {
            try
            {
                dgvSupplier.DataSource = null;
                dgvSupplier.DataSource = _supplierBLL.GetAllSuppliers().Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
                {
                    MessageBox.Show("Please enter Supplier Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Supplier sup = new Supplier
                {
                    SupplierName = txtSupplierName.Text.Trim(),
                    ContactName = txtContactName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };
                if (_supplierBLL.Save(sup))
                {
                    MessageBox.Show("Supplier saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Please select a supplier from the list to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Supplier sup = new Supplier
                {
                    Id = Convert.ToInt32(txtId.Text),
                    SupplierName = txtSupplierName.Text.Trim(),
                    ContactName = txtContactName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };
                if (_supplierBLL.Update(sup))
                {
                    MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Please select a supplier from the list to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(txtId.Text);
                    if (_supplierBLL.Delete(id))
                    {
                        MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvSupplier.Rows[e.RowIndex];
                    txtId.Text = row.Cells["SupplierId"].Value.ToString();
                    txtSupplierName.Text = row.Cells["SupplierName"].Value.ToString();
                    txtContactName.Text = row.Cells["ContactName"].Value != DBNull.Value ? row.Cells["ContactName"].Value.ToString() : "";
                    txtPhone.Text = row.Cells["Phone"].Value != DBNull.Value ? row.Cells["Phone"].Value.ToString() : "";
                    txtEmail.Text = row.Cells["Email"].Value != DBNull.Value ? row.Cells["Email"].Value.ToString() : "";
                    txtAddress.Text = row.Cells["Address"].Value != DBNull.Value ? row.Cells["Address"].Value.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting row: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtSupplierName.Clear();
            txtContactName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
        }
    }
}
