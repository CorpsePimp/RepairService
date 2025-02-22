using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.IO;

namespace RepairService
{
    public partial class ReportForm : Form
    {
        private readonly string _userRole;
        private DataTable _reportData;

        public ReportForm(string userRole)
        {
            InitializeComponent();
            _userRole = userRole;

            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;

            cmbReportType.Items.AddRange(new[] {
                "Заявки по статусам",
                "Заявки по исполнителям",
                "Среднее время выполнения"
            });
            cmbReportType.SelectedIndex = 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            try
            {
                string query = "";
                switch (cmbReportType.SelectedIndex)
                {
                    case 0: // Заявки по статусам
                        query = @"
                            SELECT 
                                status as ""Статус"",
                                COUNT(*) as ""Количество"",
                                AVG(EXTRACT(EPOCH FROM (COALESCE(actual_completion_date, CURRENT_TIMESTAMP) - created_at))/86400.0)::numeric(10,1) as ""Среднее время (дней)""
                            FROM repair_requests
                            WHERE created_at BETWEEN @dateFrom AND @dateTo
                            GROUP BY status
                            ORDER BY COUNT(*) DESC";
                        break;

                    case 1: // Заявки по исполнителям
                        query = @"
                            SELECT 
                                COALESCE(u.full_name, 'Не назначен') as ""Исполнитель"",
                                COUNT(*) as ""Всего заявок"",
                                COUNT(CASE WHEN r.status = 'completed' THEN 1 END) as ""Выполнено"",
                                COUNT(CASE WHEN r.status = 'in_progress' THEN 1 END) as ""В работе"",
                                COUNT(CASE WHEN r.status = 'waiting' THEN 1 END) as ""Ожидает""
                            FROM repair_requests r
                            LEFT JOIN users u ON r.executor_id = u.user_id
                            WHERE r.created_at BETWEEN @dateFrom AND @dateTo
                            GROUP BY u.full_name
                            ORDER BY COUNT(*) DESC";
                        break;

                    case 2: // Среднее время выполнения
                        query = @"
                            SELECT 
                                malfunction_type as ""Тип неисправности"",
                                COUNT(*) as ""Количество заявок"",
                                AVG(EXTRACT(EPOCH FROM (actual_completion_date - created_at))/86400.0)::numeric(10,1) as ""Среднее время выполнения (дней)""
                            FROM repair_requests
                            WHERE status = 'completed'
                            AND created_at BETWEEN @dateFrom AND @dateTo
                            GROUP BY malfunction_type
                            ORDER BY COUNT(*) DESC";
                        break;
                }

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("dateFrom", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("dateTo", dtpTo.Value.Date.AddDays(1));

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            _reportData = new DataTable();
                            adapter.Fill(_reportData);
                            dgvReport.DataSource = _reportData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPdf()
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PDF файл (*.pdf)|*.pdf";
                    sfd.FileName = $"Отчет_{DateTime.Now:yyyy-MM-dd}.pdf";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sfd.FileName, FileMode.Create));
                        document.Open();

                        // Добавляем заголовок
                        document.Add(new Paragraph($"Отчет: {cmbReportType.Text}"));
                        document.Add(new Paragraph($"Период: с {dtpFrom.Value:dd.MM.yyyy} по {dtpTo.Value:dd.MM.yyyy}"));
                        document.Add(new Paragraph("\n"));

                        // Создаем таблицу
                        PdfPTable table = new PdfPTable(_reportData.Columns.Count);
                        table.WidthPercentage = 100;

                        // Добавляем заголовки
                        foreach (DataColumn column in _reportData.Columns)
                        {
                            table.AddCell(new PdfPCell(new Phrase(column.ColumnName)));
                        }

                        // Добавляем данные
                        foreach (DataRow row in _reportData.Rows)
                        {
                            foreach (object item in row.ItemArray)
                            {
                                table.AddCell(new PdfPCell(new Phrase(item.ToString())));
                            }
                        }

                        document.Add(table);
                        document.Close();
                        MessageBox.Show("Отчет успешно сохранен в PDF", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в PDF: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel()
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel файл (*.xlsx)|*.xlsx";
                    sfd.FileName = $"Отчет_{DateTime.Now:yyyy-MM-dd}.xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("Отчет");

                            // Добавляем заголовок
                            worksheet.Cells["A1"].Value = $"Отчет: {cmbReportType.Text}";
                            worksheet.Cells["A2"].Value = $"Период: с {dtpFrom.Value:dd.MM.yyyy} по {dtpTo.Value:dd.MM.yyyy}";

                            // Добавляем заголовки столбцов
                            for (int i = 0; i < _reportData.Columns.Count; i++)
                            {
                                worksheet.Cells[4, i + 1].Value = _reportData.Columns[i].ColumnName;
                            }

                            // Добавляем данные
                            for (int row = 0; row < _reportData.Rows.Count; row++)
                            {
                                for (int col = 0; col < _reportData.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 5, col + 1].Value = _reportData.Rows[row][col];
                                }
                            }

                            // Автоматическая ширина столбцов
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                            // Сохраняем файл
                            package.SaveAs(new FileInfo(sfd.FileName));
                            MessageBox.Show("Отчет успешно сохранен в Excel", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (_reportData != null && _reportData.Rows.Count > 0)
                ExportToPdf();
            else
                MessageBox.Show("Нет данных для экспорта", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_reportData != null && _reportData.Rows.Count > 0)
                ExportToExcel();
            else
                MessageBox.Show("Нет данных для экспорта", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}