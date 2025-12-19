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
            this.Text = "КНИЖНЫЙ ФОНД — Детали заявки";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Size = new Size(550, 500);

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ДЕТАЛИ ЗАЯВКИ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(500, 40),
                Location = new Point(25, 20),
                BackColor = Color.Transparent
            };

            // Разделитель
            Panel divider = new Panel
            {
                Size = new Size(500, 1),
                Location = new Point(25, 70),
                BackColor = Color.FromArgb(220, 220, 220)
            };

            // Панель с информацией
            Panel infoPanel = new Panel
            {
                Location = new Point(25, 85),
                Size = new Size(500, 280),
                BackColor = Color.FromArgb(250, 250, 250)
            };
            infoPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, infoPanel.Width - 1, infoPanel.Height - 1);
            };

            int yPos = 15;
            int labelWidth = 130;
            int valueWidth = 350;

            AddInfoRow(infoPanel, "Название:", application.BookTitle, ref yPos, labelWidth, valueWidth);
            AddInfoRow(infoPanel, "Автор:", application.Author, ref yPos, labelWidth, valueWidth);
            AddInfoRow(infoPanel, "ISBN:", application.ISBN ?? "—", ref yPos, labelWidth, valueWidth);
            AddInfoRow(infoPanel, "Категория:", application.CategoryName ?? "—", ref yPos, labelWidth, valueWidth);
            AddInfoRow(infoPanel, "Дата заявки:", application.SubmittedAt ?? "—", ref yPos, labelWidth, valueWidth);

            // Статус
            string statusText = application.Status ?? "На рассмотрении";
            Color statusColor = statusText == "Одобрено" ? Color.FromArgb(40, 40, 40) :
                               statusText == "Отклонено" ? Color.FromArgb(100, 100, 100) : Color.FromArgb(70, 70, 70);
            AddInfoRow(infoPanel, "Статус:", $"[{statusText.ToUpper()}]", ref yPos, labelWidth, valueWidth, statusColor, true);

            if (!string.IsNullOrEmpty(application.Notes) && isAdminMode)
            {
                AddInfoRow(infoPanel, "Заметки:", application.Notes, ref yPos, labelWidth, valueWidth);
            }

            // Кнопки
            int btnY = 385;

            Button btnClose = new Button
            {
                Text = "ЗАКРЫТЬ",
                Location = new Point(25, btnY),
                Size = new Size(160, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 2;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnClose.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblTitle);
            this.Controls.Add(divider);
            this.Controls.Add(infoPanel);
            this.Controls.Add(btnClose);

            if (isAdminMode && application.Status == "На рассмотрении")
            {
                Button btnApprove = new Button
                {
                    Text = "ОДОБРИТЬ",
                    Location = new Point(200, btnY),
                    Size = new Size(155, 48),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(40, 40, 40),
                    Cursor = Cursors.Hand
                };
                btnApprove.FlatAppearance.BorderSize = 0;
                btnApprove.Click += (s, e) => ApproveApplication();

                Button btnReject = new Button
                {
                    Text = "ОТКЛОНИТЬ",
                    Location = new Point(365, btnY),
                    Size = new Size(160, 48),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(80, 80, 80),
                    Cursor = Cursors.Hand
                };
                btnReject.FlatAppearance.BorderSize = 0;
                btnReject.Click += (s, e) => RejectApplication();

                this.Controls.Add(btnApprove);
                this.Controls.Add(btnReject);
            }
        }

        private void AddInfoRow(Panel panel, string label, string? value, ref int yPos, int labelWidth, int valueWidth, Color? valueColor = null, bool bold = false)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(15, yPos),
                Size = new Size(labelWidth, 28),
                BackColor = Color.Transparent
            };

            Label lblValue = new Label
            {
                Text = value ?? "—",
                Font = new Font(bold ? "Consolas" : "Segoe UI", 10, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = valueColor ?? Color.Black,
                Location = new Point(labelWidth + 15, yPos),
                Size = new Size(valueWidth, 28),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(lblLabel);
            panel.Controls.Add(lblValue);
            yPos += 35;
        }

        private void ApproveApplication()
        {
            if (MessageBox.Show("Одобрить заявку?", "Подтверждение",
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
                    MessageBox.Show("Ошибка одобрения", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RejectApplication()
        {
            if (MessageBox.Show("Отклонить заявку?", "Подтверждение",
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
                    MessageBox.Show("Ошибка отклонения", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
