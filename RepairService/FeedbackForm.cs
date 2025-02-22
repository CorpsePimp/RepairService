using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text.pdf.qrcode;
using QRCoder;

namespace RepairService
{
    public partial class FeedbackForm : Form
    {
        public FeedbackForm()
        {
            InitializeComponent();
            GenerateQRCode();
        }

        private void GenerateQRCode()
        {
            try
            {
                string QRvkUrl = "https://social.mtdv.me/httpsvkcomsingingwaiter136";
                string vkUrl = "https://vk.com/singingwaiter136";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRvkUrl, QRCodeGenerator.ECCLevel.Q);
                QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
                pictureBoxQR.Image = qrCode.GetGraphic(5);

                lblInfo.Text = "Отсканируйте QR-код для перехода в VK\nили перейдите по ссылке:\n" + vkUrl;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации QR-кода: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}