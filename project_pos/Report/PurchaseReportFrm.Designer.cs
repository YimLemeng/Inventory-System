namespace project_pos
{
    partial class PurchaseReportFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnReportPurchase = new System.Windows.Forms.Button();
            this.btnClearReport = new System.Windows.Forms.Button();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.Location = new System.Drawing.Point(45, 164);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1455, 824);
            this.reportViewer1.TabIndex = 0;
            // 
            // btnReportPurchase
            // 
            this.btnReportPurchase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReportPurchase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportPurchase.ForeColor = System.Drawing.Color.Yellow;
            this.btnReportPurchase.Location = new System.Drawing.Point(859, 49);
            this.btnReportPurchase.Name = "btnReportPurchase";
            this.btnReportPurchase.Size = new System.Drawing.Size(271, 91);
            this.btnReportPurchase.TabIndex = 9;
            this.btnReportPurchase.Text = "Report Purchases";
            this.btnReportPurchase.UseVisualStyleBackColor = false;
            this.btnReportPurchase.Click += new System.EventHandler(this.btnReportPurchase_Click);
            // 
            // btnClearReport
            // 
            this.btnClearReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearReport.ForeColor = System.Drawing.Color.Yellow;
            this.btnClearReport.Location = new System.Drawing.Point(1230, 49);
            this.btnClearReport.Name = "btnClearReport";
            this.btnClearReport.Size = new System.Drawing.Size(271, 91);
            this.btnClearReport.TabIndex = 10;
            this.btnClearReport.Text = "Clear";
            this.btnClearReport.UseVisualStyleBackColor = false;
            this.btnClearReport.Click += new System.EventHandler(this.btnClearReport_Click);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(47, 72);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(291, 40);
            this.dtpFromDate.TabIndex = 11;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(485, 72);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(291, 40);
            this.dtpToDate.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 31);
            this.label3.TabIndex = 13;
            this.label3.Text = "From Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(480, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 31);
            this.label1.TabIndex = 14;
            this.label1.Text = "To Date:";
            // 
            // PurchaseReportFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(1551, 1158);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.btnClearReport);
            this.Controls.Add(this.btnReportPurchase);
            this.Controls.Add(this.reportViewer1);
            this.Name = "PurchaseReportFrm";
            this.Text = "PurchaseReportFrm";
            this.Load += new System.EventHandler(this.PurchaseReportFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Button btnReportPurchase;
        private System.Windows.Forms.Button btnClearReport;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}