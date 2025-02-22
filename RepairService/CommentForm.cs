using System;
using System.Windows.Forms;
using Npgsql;

namespace RepairService
{
    public partial class CommentForm : Form
    {
        private readonly int _requestId;
        private readonly int _userId;

        public CommentForm(int requestId, int userId, string userRole)
        {
            InitializeComponent();
            _requestId = requestId;
            _userId = userId;
            LoadComments();
        }

        private void LoadComments()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT rc.created_at, u.full_name, rc.comment_text
                        FROM request_comments rc
                        JOIN users u ON rc.user_id = u.user_id
                        WHERE rc.request_id = @requestId
                        ORDER BY rc.created_at DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("requestId", _requestId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new ListViewItem(reader.GetDateTime(0).ToString("dd.MM.yyyy HH:mm"));
                                item.SubItems.Add(reader.GetString(1));
                                item.SubItems.Add(reader.GetString(2));
                                lstComments.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке комментариев: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtComment.Text))
            {
                MessageBox.Show("Введите текст комментария",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = new NpgsqlCommand(@"
                                INSERT INTO request_comments (
                                    request_id, user_id, comment_text
                                ) VALUES (
                                    @requestId, @userId, @commentText
                                )", conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("requestId", _requestId);
                                cmd.Parameters.AddWithValue("userId", _userId);
                                cmd.Parameters.AddWithValue("commentText", txtComment.Text);
                                cmd.ExecuteNonQuery();
                            }

                            if (chkRequestManager.Checked)
                            {
                                using (var cmd = new NpgsqlCommand(@"
                                    UPDATE repair_requests 
                                    SET manager_id = (
                                        SELECT user_id 
                                        FROM users 
                                        WHERE role = 'manager' 
                                        LIMIT 1
                                    )
                                    WHERE request_id = @requestId", conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("requestId", _requestId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            txtComment.Clear();
                            chkRequestManager.Checked = false;
                            LoadComments();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Ошибка при добавлении комментария: {ex.Message}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении комментария: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}