using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace RepairService
{
    public partial class MainForm : Form
    {
        private readonly int _userId;
        private readonly string _userRole;
        private DataTable _requestsTable;

        public MainForm(int userId, string userRole)
        {
            InitializeComponent();
            _userId = userId;
            _userRole = userRole;
            SetupDataGridView();
            LoadRequests();
            SetupFilters();
            LoadRequests();
            btnNewRequest.Visible = _userRole != "executor";
            btnStatistics.Visible = _userRole == "admin" || _userRole == "manager";

            this.Text = $"Система учета ремонтных заявок - {_userRole}";
        }

        private void SetupDataGridView()
        {
            dgvRequests.AutoGenerateColumns = false;

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RequestId",
                HeaderText = "№",
                DataPropertyName = "request_id",
                Width = 50
            });

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Дата создания",
                DataPropertyName = "created_at",
                Width = 120
            });

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Equipment",
                HeaderText = "Оборудование",
                DataPropertyName = "equipment_name",
                Width = 150
            });

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Problem",
                HeaderText = "Проблема",
                DataPropertyName = "problem_description",
                Width = 200
            });

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Статус",
                DataPropertyName = "status",
                Width = 100
            });

            dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Executor",
                HeaderText = "Исполнитель",
                DataPropertyName = "executor_name",
                Width = 150
            });
        }

        private void SetupFilters()
        {
            // Инициализируем комбобокс статусов
            cmbStatusFilter.Items.Clear();
            cmbStatusFilter.Items.AddRange(new[] { "Все", "waiting", "in_progress", "completed" });
            cmbStatusFilter.SelectedIndex = 0;

            // Привязываем обработчики событий
            txtSearchEquipment.TextChanged += (s, e) => ApplyFilters();
            txtSearchMalfunction.TextChanged += (s, e) => ApplyFilters();
            cmbStatusFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_requestsTable == null) return;

            try
            {
                string equipmentSearch = txtSearchEquipment.Text.ToLower().Trim();
                string malfunctionSearch = txtSearchMalfunction.Text.ToLower().Trim();
                string statusFilter = cmbStatusFilter.SelectedIndex == 0 ? "" : cmbStatusFilter.SelectedItem.ToString();

                DataView dv = _requestsTable.DefaultView;
                var filterParts = new List<string>();

                // Фильтр по оборудованию
                if (!string.IsNullOrEmpty(equipmentSearch))
                {
                    filterParts.Add($"LOWER(Оборудование) LIKE '%{equipmentSearch}%'");
                }

                // Фильтр по типу неисправности
                if (!string.IsNullOrEmpty(malfunctionSearch))
                {
                    filterParts.Add($"LOWER([Тип неисправности]) LIKE '%{malfunctionSearch}%'");
                }

                // Фильтр по статусу
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    filterParts.Add($"Статус = '{statusFilter}'");
                }

                dv.RowFilter = string.Join(" AND ", filterParts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при применении фильтров: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadRequests()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT 
                        r.request_id as ""№"",
                        r.created_at as ""Дата создания"",
                        r.equipment_name as ""Оборудование"",
                        r.malfunction_type as ""Тип неисправности"",
                        r.problem_description as ""Описание"",
                        r.status as ""Статус"",
                        c.full_name as ""Клиент"",
                        e.full_name as ""Исполнитель"",
                        r.estimated_completion_date as ""Плановая дата"",
                        r.actual_completion_date as ""Фактическая дата""
                    FROM repair_requests r
                    LEFT JOIN users c ON r.client_id = c.user_id
                    LEFT JOIN users e ON r.executor_id = e.user_id
                    WHERE 1=1 ";

                    if (_userRole == "executor")
                        query += " AND r.executor_id = @userId";
                    else if (_userRole == "client")
                        query += " AND r.client_id = @userId";

                    query += " ORDER BY r.created_at DESC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        if (_userRole != "admin" && _userRole != "manager")
                            cmd.Parameters.AddWithValue("userId", _userId);

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            _requestsTable = new DataTable();
                            adapter.Fill(_requestsTable);
                            dgvRequests.DataSource = _requestsTable;
                        }
                    }

                    // Настройка отображения датагрида
                    dgvRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    foreach (DataGridViewColumn col in dgvRequests.Columns)
                    {
                        if (col.Name.Contains("дата"))
                        {
                            col.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewRequest_Click(object sender, EventArgs e)
        {
            var form = new RequestForm(_userId, _userRole);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadRequests();
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            var form = new StatisticsForm(_userId, _userRole);
            form.ShowDialog();
        }

        private void dgvRequests_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int requestId = Convert.ToInt32(dgvRequests.Rows[e.RowIndex].Cells["№"].Value);
                var form = new RequestForm(_userId, _userRole, requestId);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRequests();
                }
            }
        }
    }
}