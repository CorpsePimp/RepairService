using System;
using System.Windows.Forms;
using Npgsql;

namespace RepairService
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите логин и пароль", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    // Изменил password_hash на password в запросе
                    using (var cmd = new NpgsqlCommand(
                        @"SELECT user_id, role FROM users 
                          WHERE login = @login AND password = @password", conn))
                    {
                        cmd.Parameters.AddWithValue("login", txtLogin.Text);
                        cmd.Parameters.AddWithValue("password", txtPassword.Text);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string userRole = reader.GetString(1);

                                var mainForm = new MainForm(userId, userRole);
                                this.Hide();
                                mainForm.FormClosed += (s, args) => this.Close();
                                mainForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}