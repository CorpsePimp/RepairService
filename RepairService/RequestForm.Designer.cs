namespace RepairService
{
    partial class RequestForm
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
            this.txtEquipment = new System.Windows.Forms.TextBox();
            this.txtMalfunctionType = new System.Windows.Forms.TextBox();
            this.cmbExecutor = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.dtpEstimatedDate = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEquipment = new System.Windows.Forms.Label();
            this.lblMalfunctionType = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblExecutor = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblEstimatedDate = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Labels
            this.lblEquipment.AutoSize = true;
            this.lblEquipment.Location = new System.Drawing.Point(12, 15);
            this.lblEquipment.Text = "Оборудование:";

            this.lblMalfunctionType.AutoSize = true;
            this.lblMalfunctionType.Location = new System.Drawing.Point(12, 44);
            this.lblMalfunctionType.Text = "Тип неисправности:";

            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 73);
            this.lblDescription.Text = "Описание:";

            this.lblExecutor.AutoSize = true;
            this.lblExecutor.Location = new System.Drawing.Point(12, 183);
            this.lblExecutor.Text = "Исполнитель:";

            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 212);
            this.lblStatus.Text = "Статус:";

            this.lblEstimatedDate.AutoSize = true;
            this.lblEstimatedDate.Location = new System.Drawing.Point(12, 241);
            this.lblEstimatedDate.Text = "Планируемая дата:";

            // Controls
            this.txtEquipment.Location = new System.Drawing.Point(150, 12);
            this.txtEquipment.Size = new System.Drawing.Size(300, 23);

            this.txtMalfunctionType.Location = new System.Drawing.Point(150, 41);
            this.txtMalfunctionType.Size = new System.Drawing.Size(300, 23);

            this.txtDescription.Location = new System.Drawing.Point(150, 70);
            this.txtDescription.Size = new System.Drawing.Size(300, 100);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.cmbExecutor.Location = new System.Drawing.Point(150, 180);
            this.cmbExecutor.Size = new System.Drawing.Size(300, 23);
            this.cmbExecutor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbStatus.Location = new System.Drawing.Point(150, 209);
            this.cmbStatus.Size = new System.Drawing.Size(300, 23);
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.dtpEstimatedDate.Location = new System.Drawing.Point(150, 238);
            this.dtpEstimatedDate.Size = new System.Drawing.Size(300, 23);
            this.dtpEstimatedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            this.btnSave.Location = new System.Drawing.Point(150, 280);
            this.btnSave.Size = new System.Drawing.Size(120, 30);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Location = new System.Drawing.Point(330, 280);
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // Add controls to form
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblEquipment,
                this.txtEquipment,
                this.lblMalfunctionType,
                this.txtMalfunctionType,
                this.lblDescription,
                this.txtDescription,
                this.lblExecutor,
                this.cmbExecutor,
                this.lblStatus,
                this.cmbStatus,
                this.lblEstimatedDate,
                this.dtpEstimatedDate,
                this.btnSave,
                this.btnCancel
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtEquipment;
        private System.Windows.Forms.TextBox txtMalfunctionType;
        private System.Windows.Forms.ComboBox cmbExecutor;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.DateTimePicker dtpEstimatedDate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblEquipment;
        private System.Windows.Forms.Label lblMalfunctionType;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblExecutor;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblEstimatedDate;
    }
}