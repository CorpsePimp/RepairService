namespace RepairService
{
    partial class MainForm
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

        private void InitializeComponent()
        {
            this.dgvRequests = new System.Windows.Forms.DataGridView();
            this.btnNewRequest = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();

            // Фильтры
            this.txtSearchEquipment = new System.Windows.Forms.TextBox();
            this.txtSearchMalfunction = new System.Windows.Forms.TextBox();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.lblEquipmentSearch = new System.Windows.Forms.Label();
            this.lblMalfunctionSearch = new System.Windows.Forms.Label();
            this.lblStatusFilter = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Система учета ремонтных заявок";

            // Labels
            this.lblEquipmentSearch.AutoSize = true;
            this.lblEquipmentSearch.Location = new System.Drawing.Point(12, 15);
            this.lblEquipmentSearch.Size = new System.Drawing.Size(85, 15);
            this.lblEquipmentSearch.Text = "Оборудование:";

            this.lblMalfunctionSearch.AutoSize = true;
            this.lblMalfunctionSearch.Location = new System.Drawing.Point(260, 15);
            this.lblMalfunctionSearch.Size = new System.Drawing.Size(105, 15);
            this.lblMalfunctionSearch.Text = "Тип неисправности:";

            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Location = new System.Drawing.Point(530, 15);
            this.lblStatusFilter.Size = new System.Drawing.Size(45, 15);
            this.lblStatusFilter.Text = "Статус:";

            // TextBoxes
            this.txtSearchEquipment.Location = new System.Drawing.Point(100, 12);
            this.txtSearchEquipment.Size = new System.Drawing.Size(150, 23);

            this.txtSearchMalfunction.Location = new System.Drawing.Point(370, 12);
            this.txtSearchMalfunction.Size = new System.Drawing.Size(150, 23);

            // ComboBox
            this.cmbStatusFilter.Location = new System.Drawing.Point(580, 12);
            this.cmbStatusFilter.Size = new System.Drawing.Size(150, 23);
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // DataGridView
            this.dgvRequests.Location = new System.Drawing.Point(12, 41);
            this.dgvRequests.Size = new System.Drawing.Size(960, 508);
            this.dgvRequests.AllowUserToAddRows = false;
            this.dgvRequests.AllowUserToDeleteRows = false;
            this.dgvRequests.ReadOnly = true;
            this.dgvRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRequests.MultiSelect = false;
            this.dgvRequests.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequests_CellDoubleClick);

            // Buttons
            this.btnNewRequest.Location = new System.Drawing.Point(752, 12);
            this.btnNewRequest.Size = new System.Drawing.Size(100, 23);
            this.btnNewRequest.Text = "Новая заявка";
            this.btnNewRequest.Click += new System.EventHandler(this.btnNewRequest_Click);

            this.btnStatistics.Location = new System.Drawing.Point(872, 12);
            this.btnStatistics.Size = new System.Drawing.Size(100, 23);
            this.btnStatistics.Text = "Статистика";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);

            // Add controls
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblEquipmentSearch,
                this.txtSearchEquipment,
                this.lblMalfunctionSearch,
                this.txtSearchMalfunction,
                this.lblStatusFilter,
                this.cmbStatusFilter,
                this.dgvRequests,
                this.btnNewRequest,
                this.btnStatistics
            });

            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvRequests;
        private System.Windows.Forms.Button btnNewRequest;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.TextBox txtSearchEquipment;
        private System.Windows.Forms.TextBox txtSearchMalfunction;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.Label lblEquipmentSearch;
        private System.Windows.Forms.Label lblMalfunctionSearch;
        private System.Windows.Forms.Label lblStatusFilter;
    }
}