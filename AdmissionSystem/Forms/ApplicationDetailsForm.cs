using System;
using System.Drawing;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class ApplicationDetailsForm : Form
    {
        private BookRequest application;
        private bool isAdminMode;

        public ApplicationDetailsForm(BookRequest app, bool isAdmin = false)
        {
            application = app;
            isAdminMode = isAdmin;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Детали заявки на книгу";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.LightBackground;
            this.Padding = new Padding(20);
            this.Size = new Size(620, 550);

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ПОДРОБНАЯ ИНФОРМАЦИЯ О ЗАЯВКЕ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(580, 45),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Основная панель с информацией
            Panel infoPanel = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(580, 350),
                BackColor = ModernUIHelper.CardBackground,
                Padding = new Padding(18)
            };
            infoPanel.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(ModernUIHelper.BorderColor, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            int yPos = 20;
            int labelWidth = 150;
            int valueWidth = 400;

            // Название книги
            AddInfoRow(infoPanel, "Название книги:", application.BookTitle, ref yPos, labelWidth, valueWidth);

            // Автор
            AddInfoRow(infoPanel, "Автор:", application.Author, ref yPos, labelWidth, valueWidth);

            // ISBN
            AddInfoRow(infoPanel, "ISBN:", application.ISBN, ref yPos, labelWidth, valueWidth);

            // Категория
            AddInfoRow(infoPanel, "Категория:", application.CategoryName, ref yPos, labelWidth, valueWidth);

            // Дата заявки
            AddInfoRow(infoPanel, "Дата заявки:", application.SubmittedAt, ref yPos, labelWidth, valueWidth);

            // Статус
            Color statusColor = application.Status == "Одобрено" ? ModernUIHelper.PrimaryAccent :
                               application.Status == "Отклонено" ? ModernUIHelper.NeutralDark : ModernUIHelper.SecondaryAccent;
            AddInfoRow(infoPanel, "Статус:", application.Status, ref yPos, labelWidth, valueWidth, statusColor);

            // Заметки (только для админа)
            if (!string.IsNullOrEmpty(application.Notes) && isAdminMode)
            {
                AddInfoRow(infoPanel, "Заметки:", application.Notes, ref yPos, labelWidth, valueWidth);
            }

            // Кнопки
            int btnY = 430;

            Button btnClose = ModernUIHelper.CreateGradientButton("ЗАКРЫТЬ", new Point(200, btnY), new Size(200, 50), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnClose.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblTitle);
            this.Controls.Add(infoPanel);
            this.Controls.Add(btnClose);

            if (isAdminMode && application.Status == "На рассмотрении")
            {
                Button btnApprove = ModernUIHelper.CreateGradientButton("ОДОБРИТЬ", new Point(10, btnY), new Size(180, 50), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
                btnApprove.Click += (s, e) => ApproveApplication();

                Button btnReject = ModernUIHelper.CreateGradientButton("ОТКЛОНИТЬ", new Point(410, btnY), new Size(180, 50), ModernUIHelper.NeutralDarker, ModernUIHelper.NeutralDark);
                btnReject.Click += (s, e) => RejectApplication();

                btnClose.Location = new Point(200, btnY);
                btnClose.Size = new Size(200, 50);

                this.Controls.Add(btnApprove);
                this.Controls.Add(btnReject);
            }
        }

        private void AddInfoRow(Panel panel, string label, string value, ref int yPos, int labelWidth, int valueWidth, Color? valueColor = null)
        {
            // Метка
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Location = new Point(15, yPos),
                Size = new Size(labelWidth, 30),
                BackColor = Color.Transparent
            };

            // Значение
            Label lblValue = new Label
            {
                Text = value ?? "—",
                Font = new Font("Segoe UI", 11),
                ForeColor = valueColor ?? ModernUIHelper.TextPrimary,
                Location = new Point(labelWidth + 15, yPos),
                Size = new Size(valueWidth, 30),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(lblLabel);
            panel.Controls.Add(lblValue);
            yPos += 40;
        }

        private void ApproveApplication()
        {
            if (MessageBox.Show("Одобрить это заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateBookRequestStatus(application.Id, "Одобрено");
                    application.Status = "Одобрено";
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось одобрить заявку", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RejectApplication()
        {
            if (MessageBox.Show("Отклонить это заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateBookRequestStatus(application.Id, "Отклонено");
                    application.Status = "Отклонено";
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось отклонить заявление", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
