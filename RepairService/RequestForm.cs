using System;
using System.Windows.Forms;
using Npgsql;

namespace RepairService
{
    public partial class RequestForm : Form
    {
        private readonly int _userId;
        private readonly string _userRole;
        private readonly int? _requestId;

        public RequestForm(int userId, string userRole, int? requestId = null)
        {
            InitializeComponent();
            _userId = userId;
            _userRole = userRole;
            _requestId = requestId;

            LoadExecutors();
            SetupControls();

            if (_requestId.HasValue)
            {
                LoadRequestData();
            }

            // Добавляем обработчик события
            this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
        }

        private void LoadExecutors()
        {
            if (_userRole == "admin" || _userRole == "manager")
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(
                        "SELECT user_id, full_name FROM users WHERE role = 'executor'", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbExecutor.Items.Add(new ComboBoxItem(
                                    reader.GetInt32(0),
                                    reader.GetString(1)));
                            }
                        }
                    }
                }
            }

            cmbStatus.Items.AddRange(new[] { "waiting", "in_progress", "completed" });
        }

        private void LoadRequestData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT 
                            equipment_name,
                            malfunction_type,
                            problem_description,
                            executor_id,
                            status,
                            estimated_completion_date
                        FROM repair_requests 
                        WHERE request_id = @requestId", conn))
                    {
                        cmd.Parameters.AddWithValue("requestId", _requestId.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtEquipment.Text = reader.GetString(0);
                                txtMalfunctionType.Text = reader.GetString(1);
                                txtDescription.Text = reader.GetString(2);
                                if (!reader.IsDBNull(3))
                                {
                                    foreach (ComboBoxItem item in cmbExecutor.Items)
                                    {
                                        if (item.Id == reader.GetInt32(3))
                                        {
                                            cmbExecutor.SelectedItem = item;
                                            break;
                                        }
                                    }
                                }
                                cmbStatus.SelectedItem = reader.GetString(4);
                                dtpEstimatedDate.Value = reader.GetDateTime(5);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных заявки: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupControls()
        {
            bool isManagerOrAdmin = _userRole == "manager" || _userRole == "admin";
            bool isExecutor = _userRole == "executor";

            txtEquipment.ReadOnly = _requestId.HasValue;
            txtMalfunctionType.ReadOnly = _requestId.HasValue;
            cmbExecutor.Enabled = isManagerOrAdmin;
            cmbStatus.Enabled = isManagerOrAdmin || isExecutor;

            if (!_requestId.HasValue)
            {
                this.Text = "Новая заявка";
                cmbStatus.SelectedItem = "waiting";
                dtpEstimatedDate.Value = DateTime.Now.AddDays(1);
            }
            else
            {
                this.Text = $"Заявка №{_requestId}";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            if (!_requestId.HasValue)
                            {
                                using (var cmd = new NpgsqlCommand(@"
                                    INSERT INTO repair_requests (
                                        equipment_name,
                                        malfunction_type,
                                        problem_description,
                                        client_id,
                                        executor_id,
                                        status,
                                        estimated_completion_date
                                    ) VALUES (
                                        @equipmentName,
                                        @malfunctionType,
                                        @description,
                                        @clientId,
                                        @executorId,
                                        @status,
                                        @estimatedDate
                                    )", conn, transaction))
                                {
                                    AddRequestParameters(cmd);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (var cmd = new NpgsqlCommand(@"
                                    UPDATE repair_requests SET
                                        equipment_name = @equipmentName,
                                        malfunction_type = @malfunctionType,
                                        problem_description = @description,
                                        executor_id = @executorId,
                                        status = @status,
                                        estimated_completion_date = @estimatedDate
                                    WHERE request_id = @requestId", conn, transaction))
                                {
                                    AddRequestParameters(cmd);
                                    cmd.Parameters.AddWithValue("requestId", _requestId.Value);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заявки: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRequestParameters(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("equipmentName", txtEquipment.Text);
            cmd.Parameters.AddWithValue("malfunctionType", txtMalfunctionType.Text);
            cmd.Parameters.AddWithValue("description", txtDescription.Text);
            cmd.Parameters.AddWithValue("clientId", _userId);

            if (cmbExecutor.SelectedItem != null)
                cmd.Parameters.AddWithValue("executorId",
                    ((ComboBoxItem)cmbExecutor.SelectedItem).Id);
            else
                cmd.Parameters.AddWithValue("executorId", DBNull.Value);

            cmd.Parameters.AddWithValue("status", cmbStatus.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("estimatedDate", dtpEstimatedDate.Value);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtEquipment.Text))
            {
                MessageBox.Show("Введите название оборудования", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMalfunctionType.Text))
            {
                MessageBox.Show("Введите тип неисправности", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Введите описание проблемы", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус заявки", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpEstimatedDate.Value < DateTime.Now)
            {
                MessageBox.Show("Планируемая дата не может быть в прошлом", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_requestId.HasValue && cmbStatus.SelectedItem?.ToString() == "completed")
            {
                try
                {
                    using (var conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand(@"
                            UPDATE repair_requests 
                            SET actual_completion_date = CURRENT_TIMESTAMP
                            WHERE request_id = @requestId", conn))
                        {
                            cmd.Parameters.AddWithValue("requestId", _requestId.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении даты выполнения: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public class ComboBoxItem
    {
        public int Id { get; }
        public string Text { get; }

        public ComboBoxItem(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}