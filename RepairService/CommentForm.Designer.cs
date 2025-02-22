namespace RepairService
{
    partial class CommentForm
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
            this.lstComments = new System.Windows.Forms.ListView();
            this.colDateTime = new System.Windows.Forms.ColumnHeader();
            this.colUser = new System.Windows.Forms.ColumnHeader();
            this.colComment = new System.Windows.Forms.ColumnHeader();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnAddComment = new System.Windows.Forms.Button();
            this.chkRequestManager = new System.Windows.Forms.CheckBox();

            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Комментарии";

            // ListView
            this.lstComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstComments.Location = new System.Drawing.Point(12, 12);
            this.lstComments.Size = new System.Drawing.Size(560, 300);
            this.lstComments.View = System.Windows.Forms.View.Details;
            this.lstComments.FullRowSelect = true;
            this.lstComments.GridLines = true;

            // Columns
            this.colDateTime.Text = "Дата и время";
            this.colDateTime.Width = 150;
            this.colUser.Text = "Пользователь";
            this.colUser.Width = 150;
            this.colComment.Text = "Комментарий";
            this.colComment.Width = 260;

            this.lstComments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colDateTime,
                this.colUser,
                this.colComment
            });

            // TextBox
            this.txtComment.Location = new System.Drawing.Point(12, 324);
            this.txtComment.Size = new System.Drawing.Size(560, 60);
            this.txtComment.Multiline = true;

            // CheckBox
            this.chkRequestManager.Location = new System.Drawing.Point(12, 390);
            this.chkRequestManager.Size = new System.Drawing.Size(200, 24);
            this.chkRequestManager.Text = "Требуется внимание менеджера";

            // Button
            this.btnAddComment.Location = new System.Drawing.Point(452, 390);
            this.btnAddComment.Size = new System.Drawing.Size(120, 30);
            this.btnAddComment.Text = "Добавить";
            this.btnAddComment.Click += new System.EventHandler(this.btnAddComment_Click);

            // Add controls
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lstComments,
                this.txtComment,
                this.chkRequestManager,
                this.btnAddComment
            });

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListView lstComments;
        private System.Windows.Forms.ColumnHeader colDateTime;
        private System.Windows.Forms.ColumnHeader colUser;
        private System.Windows.Forms.ColumnHeader colComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnAddComment;
        private System.Windows.Forms.CheckBox chkRequestManager;
    }
}