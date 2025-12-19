using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibrarySystem.Models;

namespace LibrarySystem.UI
{
    /// <summary>
    /// Современный UI helper с черно-белой темой
    /// </summary>
    public static class ModernUIHelper
    {
        // Черно-белая палитра
        public static readonly Color LightBackground = ColorFromHex("#FFFFFF");
        public static readonly Color CardBackground = ColorFromHex("#F5F5F5");
        public static readonly Color SidebarBackground = ColorFromHex("#1A1A1A");

        // Акцентные цвета (оттенки серого)
        public static readonly Color PrimaryAccent = ColorFromHex("#000000");
        public static readonly Color SecondaryAccent = ColorFromHex("#404040");
        public static readonly Color SuccessColor = ColorFromHex("#2D2D2D");
        public static readonly Color DangerColor = ColorFromHex("#4A4A4A");
        public static readonly Color WarningColor = ColorFromHex("#666666");

        // Текст
        public static readonly Color TextPrimary = ColorFromHex("#000000");
        public static readonly Color TextSecondary = ColorFromHex("#333333");
        public static readonly Color TextMuted = ColorFromHex("#777777");
        public static readonly Color TextLight = ColorFromHex("#FFFFFF");

        // Дополнительные оттенки
        public static readonly Color SidebarGradientStart = ColorFromHex("#2A2A2A");
        public static readonly Color SidebarGradientEnd = ColorFromHex("#0A0A0A");
        public static readonly Color CardHoverBackground = ColorFromHex("#E8E8E8");
        public static readonly Color NeutralDark = ColorFromHex("#888888");
        public static readonly Color NeutralDarker = ColorFromHex("#555555");
        public static readonly Color BorderColor = ColorFromHex("#CCCCCC");

        /// <summary>
        /// Создает стильную кнопку с градиентом
        /// </summary>
        public static Button CreateGradientButton(string text, Point location, Size size, Color startColor, Color endColor)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = TextLight,
                Cursor = Cursors.Hand,
                BackColor = startColor
            };

            button.FlatAppearance.BorderSize = 2;
            button.FlatAppearance.BorderColor = PrimaryAccent;
            button.FlatAppearance.MouseOverBackColor = endColor;
            button.Padding = new Padding(12, 8, 12, 8);
            button.Margin = new Padding(6);
            button.BackColor = startColor;
            button.ForeColor = TextLight;
            button.Cursor = Cursors.Hand;

            button.Font = new Font("Segoe UI", 11.5f, FontStyle.Bold);

            return button;
        }

        /// <summary>
        /// Создает стильное текстовое поле
        /// </summary>
        public static TextBox CreateModernTextBox(Point location, Size size, string placeholder = "")
        {
            var textBox = new TextBox
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 11),
                BackColor = LightBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None,
                Tag = placeholder
            };

            var panel = new Panel
            {
                Location = location,
                Size = new Size(size.Width, size.Height + 10),
                BackColor = CardBackground,
                Padding = new Padding(2)
            };

            textBox.Location = new Point(10, 5);
            panel.Controls.Add(textBox);

            return textBox;
        }

        /// <summary>
        /// Создает современную метку
        /// </summary>
        public static Label CreateModernLabel(string text, Point location, int fontSize = 10,
            FontStyle style = FontStyle.Regular, Color? color = null)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = new Font("Segoe UI", fontSize, style),
                ForeColor = color ?? TextSecondary,
                AutoSize = true,
                BackColor = Color.Transparent
            };
        }

        /// <summary>
        /// Создает панель-карточку
        /// </summary>
        public static Panel CreateCard(Point location, Size size)
        {
            var panel = new Panel
            {
                Location = location,
                Size = size,
                BackColor = CardBackground,
                Padding = new Padding(20)
            };

            panel.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(BorderColor, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            return panel;
        }

        /// <summary>
        /// Создает боковую панель навигации
        /// </summary>
        public static Panel CreateSidebar(Size size)
        {
            var sidebar = new Panel
            {
                Location = new Point(0, 0),
                Size = size,
                BackColor = SidebarBackground,
                Dock = DockStyle.Left
            };

            sidebar.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    sidebar.ClientRectangle,
                    SidebarGradientStart,
                    SidebarGradientEnd,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, sidebar.ClientRectangle);
                }
            };

            return sidebar;
        }

        /// <summary>
        /// Создает кнопку для боковой панели
        /// </summary>
        public static Button CreateSidebarButton(string text, int yPosition, bool isActive = false)
        {
            var button = new Button
            {
                Text = "  " + text,
                Location = new Point(0, yPosition),
                Size = new Size(220, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = isActive ? TextPrimary : TextLight,
                BackColor = isActive ? LightBackground : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = SecondaryAccent;

            return button;
        }

        /// <summary>
        /// Стилизует DataGridView в черно-белой теме
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = LightBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 45;

            // Стиль заголовков
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryAccent;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextLight;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = PrimaryAccent;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 50;

            // Стиль ячеек
            dgv.DefaultCellStyle.BackColor = LightBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = CardHoverBackground;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            dgv.GridColor = BorderColor;
        }

        /// <summary>
        /// Создает стильный NumericUpDown
        /// </summary>
        public static NumericUpDown CreateModernNumericUpDown(Point location, Size size,
            decimal min, decimal max, decimal value, int decimalPlaces = 0)
        {
            var numericUpDown = new NumericUpDown
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 11),
                BackColor = LightBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = min,
                Maximum = max,
                Value = value,
                DecimalPlaces = decimalPlaces
            };

            return numericUpDown;
        }

        /// <summary>
        /// Создает разделитель
        /// </summary>
        public static Panel CreateDivider(Point location, int width)
        {
                return new Panel
                {
                    Location = location,
                    Size = new Size(width, 1),
                    BackColor = BorderColor
                };
        }

        /// <summary>
        /// Создает карточку заявки на книгу
        /// </summary>
        public static Panel CreateApplicationCard(BookRequest app, EventHandler onClick = null)
        {
            var card = new Panel
            {
                Size = new Size(350, 200),
                BackColor = LightBackground,
                Padding = new Padding(15),
                Cursor = Cursors.Hand,
                Tag = app
            };

            // Определяем цвет статуса (оттенки серого)
            Color statusColor;
            string status = app.Status ?? "На рассмотрении";
            switch (status)
            {
                case "Одобрено":
                    statusColor = PrimaryAccent;
                    break;
                case "Отклонено":
                    statusColor = NeutralDark;
                    break;
                default:
                    statusColor = SecondaryAccent;
                    break;
            }

            // Стиль карточки
            card.Paint += (s, e) =>
            {
                var panel = (Panel)s;
                using (var brush = new SolidBrush(panel.BackColor))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }

                // Рамка с цветом статуса сверху
                using (var pen = new Pen(statusColor, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }

                // Тень
                ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle,
                    Color.FromArgb(40, 0, 0, 0), 0, ButtonBorderStyle.None,
                    Color.FromArgb(40, 0, 0, 0), 5, ButtonBorderStyle.None,
                    Color.FromArgb(40, 0, 0, 0), 0, ButtonBorderStyle.None,
                    Color.FromArgb(40, 0, 0, 0), 5, ButtonBorderStyle.None);
            };

            // Название книги (крупно)
            string fullName = app.FullRequest ?? "Не указано";
            Label lblName = new Label
            {
                Text = fullName.Length > 25 ? fullName.Substring(0, 22) + "..." : fullName,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(10, 10),
                Size = new Size(310, 30),
                BackColor = Color.Transparent
            };

            // Книга
            string specialtyName = app.CategoryName ?? "Не указана";
            Label lblSpecialty = new Label
            {
                Text = "Книга: " + (specialtyName.Length > 25 ? specialtyName.Substring(0, 22) + "..." : specialtyName),
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(10, 45),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // Автор
            Label lblScore = new Label
            {
                Text = $"Автор: {app.Author}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(10, 70),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // Дата
            string submittedAt = app.SubmittedAt ?? "Не указана";
            Label lblDate = new Label
            {
                Text = $"Дата: {submittedAt}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(10, 95),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // ISBN
            string isbn = app.ISBN ?? "";
            Label lblPassport = new Label
            {
                Text = $"ISBN: {isbn}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(10, 120),
                Size = new Size(310, 25),
                BackColor = Color.Transparent
            };

            // Статус (в правом нижнем углу)
            Label lblStatus = new Label
            {
                Text = status.ToUpper(),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(170, 155),
                Size = new Size(150, 30),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            // Добавляем все элементы на карточку
            card.Controls.Add(lblName);
            card.Controls.Add(lblSpecialty);
            card.Controls.Add(lblScore);
            card.Controls.Add(lblDate);
            card.Controls.Add(lblPassport);
            card.Controls.Add(lblStatus);

            // Обработчик клика
            if (onClick != null)
            {
                card.Click += onClick;
                foreach (Control control in card.Controls)
                {
                    control.Click += onClick;
                    control.Cursor = Cursors.Hand;
                }
            }

            // Эффект при наведении
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = CardHoverBackground;
                card.Refresh();
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = LightBackground;
                card.Refresh();
            };

            return card;
        }

        /// <summary>
        /// Помощник: конвертирует HEX в Color
        /// </summary>
        public static Color ColorFromHex(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}
