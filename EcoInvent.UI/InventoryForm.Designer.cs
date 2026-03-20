using System.Drawing;
using System.Windows.Forms;

namespace EcoInvent.UI
{
    partial class InventoryForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dgvItems = new DataGridView();
            lblName = new Label();
            txtName = new TextBox();
            lblCategory = new Label();
            txtCategory = new TextBox();
            lblStock = new Label();
            txtStock = new TextBox();
            lblReorder = new Label();
            txtReorder = new TextBox();

            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();

            // 
            // dgvItems
            // 
            dgvItems.Name = "dgvItems";
            dgvItems.TabIndex = 0;

            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Name = "lblName";
            lblName.Text = "Item Name";

            // 
            // txtName
            // 
            txtName.Name = "txtName";

            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Name = "lblCategory";
            lblCategory.Text = "Category";

            // 
            // txtCategory
            // 
            txtCategory.Name = "txtCategory";

            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Name = "lblStock";
            lblStock.Text = "Stock";

            // 
            // txtStock
            // 
            txtStock.Name = "txtStock";

            // 
            // lblReorder
            // 
            lblReorder.AutoSize = true;
            lblReorder.Name = "lblReorder";
            lblReorder.Text = "Reorder Level";

            // 
            // txtReorder
            // 
            txtReorder.Name = "txtReorder";

            // 
            // InventoryForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1500, 900);
            Name = "InventoryForm";
            Text = "EcoInvent Enterprise";

            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvItems;
        private Label lblName;
        private TextBox txtName;
        private Label lblCategory;
        private TextBox txtCategory;
        private Label lblStock;
        private TextBox txtStock;
        private Label lblReorder;
        private TextBox txtReorder;
    }
}