using System;
using System.Drawing;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;

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
            this.BackColor = UI.ModernUIHelper.LightBackground;
            this.Padding = new Padding(20);
            this.Size = new Size(620, 720);

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ПОДРОБНАЯ ИНФОРМАЦИЯ О ЗАЯВКЕ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = UI.ModernUIHelper.PrimaryAccent,
                Size = new Size(580, 40),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Основная панель с информацией
            Panel infoPanel = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(580, 520),
                BackColor = UI.ModernUIHelper.CardBackground,
                Padding = new Padding(18)
            };

            int yPos = 10;
            int labelWidth = 140;
            int valueWidth = 400;

            // Название книги
            AddInfoRow(infoPanel, "Название книги:", application.BookTitle, ref yPos, labelWidth, valueWidth);
            
            // Автор
            AddInfoRow(infoPanel, "Автор:", application.Author, ref yPos, labelWidth, valueWidth);
            
            // ISBN
            AddInfoRow(infoPanel, "ISBN:", application.ISBN, ref yPos, labelWidth, valueWidth);
            
            // Категория
            AddInfoRow(infoPanel, "Книга:", application.CategoryName, ref yPos, labelWidth, valueWidth);
            
            // Дата заявки
            AddInfoRow(infoPanel, "Дата заявки:", application.SubmittedAt, ref yPos, labelWidth, valueWidth);
            
            // Статус
            Color statusColor = application.Status == "Одобрено" ? UI.ModernUIHelper.SuccessColor :
                               application.Status == "Отклонено" ? UI.ModernUIHelper.DangerColor : UI.ModernUIHelper.WarningColor;
            AddInfoRow(infoPanel, "Статус:", application.Status, ref yPos, labelWidth, valueWidth, statusColor);
            
            // Заметки (только для админа)
            if (!string.IsNullOrEmpty(application.Notes) && isAdminMode)
            {
                AddInfoRow(infoPanel, "Заметки:", application.Notes, ref yPos, labelWidth, valueWidth);
            }

            // Кнопки — табличное расположение (до 3 кнопок)
            var buttonPanel = new TableLayoutPanel
            {
                Location = new Point(10, 600),
                Size = new Size(580, 80),
                BackColor = Color.Transparent,
                ColumnCount = 3,
                RowCount = 1
            };
            for (int i = 0; i < 3; i++) buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            Button btnClose = UI.ModernUIHelper.CreateGradientButton("ЗАКРЫТЬ", Point.Empty, new Size(1,1), UI.ModernUIHelper.SecondaryAccent, UI.ModernUIHelper.SecondaryAccent);
            btnClose.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            btnClose.Dock = DockStyle.Fill;
            buttonPanel.Controls.Add(btnClose, 1, 0);

            if (isAdminMode && application.Status == "На рассмотрении")
            {
                Button btnApprove = UI.ModernUIHelper.CreateGradientButton("ОДОБРИТЬ", Point.Empty, new Size(1,1), UI.ModernUIHelper.SuccessColor, UI.ModernUIHelper.SuccessColor);
                btnApprove.Click += (s, e) => ApproveApplication();

                Button btnReject = UI.ModernUIHelper.CreateGradientButton("ОТКЛОНИТЬ", Point.Empty, new Size(1,1), UI.ModernUIHelper.DangerColor, UI.ModernUIHelper.DangerColor);
                btnReject.Click += (s, e) => RejectApplication();

                btnApprove.Dock = DockStyle.Fill; btnReject.Dock = DockStyle.Fill;
                buttonPanel.Controls.Add(btnApprove, 0, 0);
                buttonPanel.Controls.Add(btnReject, 2, 0);
            }

            // Добавляем контролы
            this.Controls.Add(lblTitle);
            this.Controls.Add(infoPanel);
            this.Controls.Add(buttonPanel);
        }

        private void AddInfoRow(Panel panel, string label, string value, ref int yPos, int labelWidth, int valueWidth, Color? valueColor = null)
        {
            // Метка
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = UI.ModernUIHelper.TextSecondary,
                Location = new Point(0, yPos),
                Size = new Size(labelWidth, 25),
                BackColor = Color.Transparent
            };

            // Значение
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10),
                ForeColor = valueColor ?? UI.ModernUIHelper.TextPrimary,
                Location = new Point(labelWidth, yPos),
                Size = new Size(valueWidth, 25),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(lblLabel);
            panel.Controls.Add(lblValue);
            yPos += 30;
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
