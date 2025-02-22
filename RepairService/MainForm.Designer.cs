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
            this.btnReports = new System.Windows.Forms.Button();
            this.btnFeedback = new System.Windows.Forms.Button();
            this.txtSearchEquipment = new System.Windows.Forms.TextBox();
            this.txtSearchMalfunction = new System.Windows.Forms.TextBox();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.lblEquipmentSearch = new System.Windows.Forms.Label();
            this.lblMalfunctionSearch = new System.Windows.Forms.Label();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            this.SuspendLayout();

            // 
            // dgvRequests
            // 
            this.dgvRequests.AllowUserToAddRows = false;
            this.dgvRequests.AllowUserToDeleteRows = false;
            this.dgvRequests.ColumnHeadersHeight = 29;
            this.dgvRequests.Location = new System.Drawing.Point(14, 44);
            this.dgvRequests.MultiSelect = false;
            this.dgvRequests.Name = "dgvRequests";
            this.dgvRequests.ReadOnly = true;
            this.dgvRequests.RowHeadersWidth = 51;
            this.dgvRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRequests.Size = new System.Drawing.Size(1344, 545);
            this.dgvRequests.TabIndex = 6;
            this.dgvRequests.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequests_CellDoubleClick);

            // 
            // btnNewRequest
            // 
            this.btnNewRequest.Location = new System.Drawing.Point(1078, 13);
            this.btnNewRequest.Name = "btnNewRequest";
            this.btnNewRequest.Size = new System.Drawing.Size(137, 25);
            this.btnNewRequest.TabIndex = 7;
            this.btnNewRequest.Text = "Новая заявка";
            this.btnNewRequest.Click += new System.EventHandler(this.btnNewRequest_Click);

            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(1221, 13);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(137, 25);
            this.btnStatistics.TabIndex = 8;
            this.btnStatistics.Text = "Статистика";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);

            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(935, 13);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(137, 25);
            this.btnReports.TabIndex = 9;
            this.btnReports.Text = "Отчеты";
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);

            // 
            // btnFeedback
            // 
            this.btnFeedback.Location = new System.Drawing.Point(14, 595);
            this.btnFeedback.Name = "btnFeedback";
            this.btnFeedback.Size = new System.Drawing.Size(137, 25);
            this.btnFeedback.TabIndex = 10;
            this.btnFeedback.Text = "Обратная связь";
            this.btnFeedback.UseVisualStyleBackColor = true;
            this.btnFeedback.Click += new System.EventHandler(this.btnFeedback_Click);

            // 
            // txtSearchEquipment
            // 
            this.txtSearchEquipment.Location = new System.Drawing.Point(128, 13);
            this.txtSearchEquipment.Name = "txtSearchEquipment";
            this.txtSearchEquipment.Size = new System.Drawing.Size(171, 22);
            this.txtSearchEquipment.TabIndex = 1;

            // 
            // txtSearchMalfunction
            // 
            this.txtSearchMalfunction.Location = new System.Drawing.Point(450, 13);
            this.txtSearchMalfunction.Name = "txtSearchMalfunction";
            this.txtSearchMalfunction.Size = new System.Drawing.Size(171, 22);
            this.txtSearchMalfunction.TabIndex = 3;

            // 
            // cmbStatusFilter
            // 
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Location = new System.Drawing.Point(689, 13);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(171, 24);
            this.cmbStatusFilter.TabIndex = 5;

            // 
            // lblEquipmentSearch
            // 
            this.lblEquipmentSearch.AutoSize = true;
            this.lblEquipmentSearch.Location = new System.Drawing.Point(14, 16);
            this.lblEquipmentSearch.Name = "lblEquipmentSearch";
            this.lblEquipmentSearch.Size = new System.Drawing.Size(108, 16);
            this.lblEquipmentSearch.TabIndex = 0;
            this.lblEquipmentSearch.Text = "Оборудование:";

            // 
            // lblMalfunctionSearch
            // 
            this.lblMalfunctionSearch.AutoSize = true;
            this.lblMalfunctionSearch.Location = new System.Drawing.Point(305, 16);
            this.lblMalfunctionSearch.Name = "lblMalfunctionSearch";
            this.lblMalfunctionSearch.Size = new System.Drawing.Size(139, 16);
            this.lblMalfunctionSearch.TabIndex = 2;
            this.lblMalfunctionSearch.Text = "Тип неисправности:";

            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Location = new System.Drawing.Point(627, 16);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(56, 16);
            this.lblStatusFilter.TabIndex = 4;
            this.lblStatusFilter.Text = "Статус:";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1375, 640);
            this.Controls.Clear();
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblEquipmentSearch,
                this.txtSearchEquipment,
                this.lblMalfunctionSearch,
                this.txtSearchMalfunction,
                this.lblStatusFilter,
                this.cmbStatusFilter,
                this.btnNewRequest,
                this.btnStatistics,
                this.btnReports,
                this.btnFeedback,
                this.dgvRequests
            });
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Система учета ремонтных заявок";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvRequests;
        private System.Windows.Forms.Button btnNewRequest;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnFeedback;
        private System.Windows.Forms.TextBox txtSearchEquipment;
        private System.Windows.Forms.TextBox txtSearchMalfunction;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.Label lblEquipmentSearch;
        private System.Windows.Forms.Label lblMalfunctionSearch;
        private System.Windows.Forms.Label lblStatusFilter;
    }
}