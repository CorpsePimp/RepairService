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

            // Настройка DataGridView
            SetupDataGridView();

            // Настройка фильтров
            SetupFilters();

            // Загрузка данных
            LoadRequests();

            // Настройка прав доступа
            btnNewRequest.Visible = _userRole != "executor";
            btnStatistics.Visible = _userRole == "admin" || _userRole == "manager";
            btnReports.Visible = _userRole == "admin" || _userRole == "manager";


            this.Text = $"Система учета ремонтных заявок - {_userRole}";
        }

        private void SetupDataGridView()
        {
            // Настройка внешнего вида и поведения DataGridView
            dgvRequests.AutoGenerateColumns = false;
            dgvRequests.AllowUserToAddRows = false;
            dgvRequests.AllowUserToDeleteRows = false;
            dgvRequests.ReadOnly = true;
            dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRequests.MultiSelect = false;
            dgvRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Определение колонок
            dgvRequests.Columns.Clear();
            dgvRequests.Columns.AddRange(
                new DataGridViewTextBoxColumn { DataPropertyName = "№", HeaderText = "№", Width = 50 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Дата создания", HeaderText = "Дата создания", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Оборудование", HeaderText = "Оборудование", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Тип неисправности", HeaderText = "Тип неисправности", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Описание", HeaderText = "Описание", Width = 200 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Статус", HeaderText = "Статус", Width = 100 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Клиент", HeaderText = "Клиент", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Исполнитель", HeaderText = "Исполнитель", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Плановая дата", HeaderText = "Плановая дата", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Фактическая дата", HeaderText = "Фактическая дата", Width = 120 }
            );

            // Добавляем обработчик события форматирования ячеек
            dgvRequests.CellFormatting += DgvRequests_CellFormatting;
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

        private void DgvRequests_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                var column = dgvRequests.Columns[e.ColumnIndex];

                // Форматирование дат
                if (column.HeaderText.Contains("дата"))
                {
                    if (e.Value is DateTime dateValue)
                    {
                        e.Value = dateValue.ToString("dd.MM.yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
                }

                // Форматирование статусов
                if (column.HeaderText == "Статус")
                {
                    switch (e.Value.ToString().ToLower())
                    {
                        case "waiting":
                            e.Value = "Ожидание";
                            break;
                        case "in_progress":
                            e.Value = "В работе";
                            break;
                        case "completed":
                            e.Value = "Завершено";
                            break;
                    }
                    e.FormattingApplied = true;
                }
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
                        }
                    }

                    // Устанавливаем источник данных
                    dgvRequests.DataSource = _requestsTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            if (_requestsTable == null) return;

            try
            {
                string equipmentSearch = txtSearchEquipment.Text.ToLower().Trim();
                string malfunctionSearch = txtSearchMalfunction.Text.ToLower().Trim();
                string statusFilter = cmbStatusFilter.SelectedIndex == 0 ? "" : cmbStatusFilter.SelectedItem.ToString();

                var filterParts = new List<string>();

                if (!string.IsNullOrEmpty(equipmentSearch))
                    filterParts.Add($"Convert([Оборудование], 'System.String') LIKE '%{equipmentSearch}%'");

                if (!string.IsNullOrEmpty(malfunctionSearch))
                    filterParts.Add($"Convert([Тип неисправности], 'System.String') LIKE '%{malfunctionSearch}%'");

                if (!string.IsNullOrEmpty(statusFilter))
                    filterParts.Add($"[Статус] = '{statusFilter}'");

                _requestsTable.DefaultView.RowFilter = filterParts.Count > 0 ?
                    string.Join(" AND ", filterParts) : "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при применении фильтров: {ex.Message}",
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
        private void btnReports_Click(object sender, EventArgs e)
        {
            if (_userRole == "admin" || _userRole == "manager")
            {
                var form = new ReportForm(_userRole);
                form.ShowDialog();
            }
        }
        private void btnFeedback_Click(object sender, EventArgs e)
        {
            var form = new FeedbackForm();
            form.ShowDialog();
        }
    }
}