namespace RepairService
{
    partial class FeedbackForm
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
            this.pictureBoxQR = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).BeginInit();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Обратная связь";

            // PictureBox for QR code
            this.pictureBoxQR.Location = new System.Drawing.Point(50, 50);
            this.pictureBoxQR.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            // Label
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(50, 370);
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Add controls
            this.Controls.Add(this.pictureBoxQR);
            this.Controls.Add(this.lblInfo);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBoxQR;
        private System.Windows.Forms.Label lblInfo;
    }
}