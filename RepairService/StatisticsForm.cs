using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace RepairService
{
    public partial class StatisticsForm : Form
    {
        private readonly int _userId;
        private readonly string _userRole;

        public StatisticsForm(int userId, string userRole)
        {
            InitializeComponent();
            _userId = userId;
            _userRole = userRole;

            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;

            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    LoadGeneralStatistics(conn);
                    LoadTypeStatistics(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGeneralStatistics(NpgsqlConnection conn)
        {
            using (var cmd = new NpgsqlCommand(@"
                SELECT 
                    COUNT(*) as total_requests,
                    COUNT(CASE WHEN status = 'completed' THEN 1 END) as completed_requests,
                    AVG(CASE 
                        WHEN status = 'completed' 
                        THEN EXTRACT(EPOCH FROM (actual_completion_date - created_at))/86400.0 
                    END) as avg_completion_time
                FROM repair_requests
                WHERE created_at BETWEEN @dateFrom AND @dateTo", conn))
            {
                cmd.Parameters.AddWithValue("dateFrom", dtpFrom.Value.Date);
                cmd.Parameters.AddWithValue("dateTo", dtpTo.Value.Date.AddDays(1));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int totalRequests = reader.GetInt32(0);
                        int completedRequests = reader.GetInt32(1);
                        double avgTime = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);

                        lblTotalRequests.Text = $"Всего заявок: {totalRequests}";
                        lblCompletedRequests.Text = $"Выполнено заявок: {completedRequests}";
                        lblAverageTime.Text = $"Среднее время выполнения: {avgTime:F1} дней";
                    }
                }
            }
        }

        private void LoadTypeStatistics(NpgsqlConnection conn)
        {
            using (var cmd = new NpgsqlCommand(@"
                SELECT 
                    malfunction_type,
                    COUNT(*) as count,
                    AVG(CASE 
                        WHEN status = 'completed' 
                        THEN EXTRACT(EPOCH FROM (actual_completion_date - created_at))/86400.0 
                    END) as average_time
                FROM repair_requests
                WHERE created_at BETWEEN @dateFrom AND @dateTo
                GROUP BY malfunction_type
                ORDER BY count DESC", conn))
            {
                cmd.Parameters.AddWithValue("dateFrom", dtpFrom.Value.Date);
                cmd.Parameters.AddWithValue("dateTo", dtpTo.Value.Date.AddDays(1));

                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    // Переименовываем колонки для отображения
                    dt.Columns["malfunction_type"].ColumnName = "Тип неисправности";
                    dt.Columns["count"].ColumnName = "Количество";
                    dt.Columns["average_time"].ColumnName = "Среднее время (дней)";

                    dgvStatistics.DataSource = dt;

                    // Форматируем колонку со средним временем
                    if (dgvStatistics.Columns["Среднее время (дней)"] is DataGridViewColumn column)
                    {
                        column.DefaultCellStyle.Format = "F1";
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                dtpFrom.Value = dtpTo.Value;
            }
            LoadStatistics();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value;
            }
            LoadStatistics();
        }
    }
}