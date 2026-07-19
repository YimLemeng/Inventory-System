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
    public partial class CategoryFrm : Form
    {
        private readonly CategoryBLL _categoryBLL = new CategoryBLL();
        public CategoryFrm()
        {
            InitializeComponent();
            LoadData();
            txtId.ReadOnly = true;
        }
        private void LoadData()
        {
            try
            {
                dgvCategory.DataSource = _categoryBLL.GetAllCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Category cat = new Category
                {
                    CategoryName = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim()
                };
                if (_categoryBLL.Save(cat))
                {
                    MessageBox.Show("Saved successfully");
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text)) return;
                Category cat = new Category
                {
                    Id = Convert.ToInt32(txtId.Text),
                    CategoryName = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim()
                };
                if (_categoryBLL.Update(cat))
                {
                    MessageBox.Show("Updated successfully");
                    LoadData();
                    ClearForm();
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
                if (string.IsNullOrEmpty(txtId.Text)) return;
                int id = Convert.ToInt32(txtId.Text);
                if (_categoryBLL.Delete(id))
                {
                    MessageBox.Show("Deleted successfully");
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtName.Clear();
            txtDescription.Clear();
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCategory.Rows[e.RowIndex];
                txtId.Text = row.Cells["CategoryId"].Value.ToString();
                txtName.Text = row.Cells["CategoryName"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value != DBNull.Value ? row.Cells["Description"].Value.ToString() : "";
            }
        }
    }
}
