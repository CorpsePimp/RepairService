namespace RepairService
{
    partial class StatisticsForm
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
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTotalRequests = new System.Windows.Forms.Label();
            this.lblCompletedRequests = new System.Windows.Forms.Label();
            this.lblAverageTime = new System.Windows.Forms.Label();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Статистика";

            // Date Range Controls
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Location = new System.Drawing.Point(12, 15);
            this.lblDateFrom.Text = "С:";

            this.dtpFrom.Location = new System.Drawing.Point(30, 12);
            this.dtpFrom.Size = new System.Drawing.Size(150, 23);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);

            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Location = new System.Drawing.Point(190, 15);
            this.lblDateTo.Text = "По:";

            this.dtpTo.Location = new System.Drawing.Point(215, 12);
            this.dtpTo.Size = new System.Drawing.Size(150, 23);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);

            // Refresh Button
            this.btnRefresh.Location = new System.Drawing.Point(380, 12);
            this.btnRefresh.Size = new System.Drawing.Size(100, 23);
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // Statistics Labels
            this.lblTotalRequests.AutoSize = true;
            this.lblTotalRequests.Location = new System.Drawing.Point(12, 50);
            this.lblTotalRequests.Size = new System.Drawing.Size(200, 15);

            this.lblCompletedRequests.AutoSize = true;
            this.lblCompletedRequests.Location = new System.Drawing.Point(222, 50);
            this.lblCompletedRequests.Size = new System.Drawing.Size(200, 15);

            this.lblAverageTime.AutoSize = true;
            this.lblAverageTime.Location = new System.Drawing.Point(432, 50);
            this.lblAverageTime.Size = new System.Drawing.Size(300, 15);

            // DataGridView
            this.dgvStatistics.Location = new System.Drawing.Point(12, 80);
            this.dgvStatistics.Size = new System.Drawing.Size(760, 369);
            this.dgvStatistics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStatistics.AllowUserToAddRows = false;
            this.dgvStatistics.AllowUserToDeleteRows = false;
            this.dgvStatistics.ReadOnly = true;

            // Add controls
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblDateFrom,
                this.dtpFrom,
                this.lblDateTo,
                this.dtpTo,
                this.btnRefresh,
                this.lblTotalRequests,
                this.lblCompletedRequests,
                this.lblAverageTime,
                this.dgvStatistics
            });

            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTotalRequests;
        private System.Windows.Forms.Label lblCompletedRequests;
        private System.Windows.Forms.Label lblAverageTime;
        private System.Windows.Forms.DataGridView dgvStatistics;
    }
}