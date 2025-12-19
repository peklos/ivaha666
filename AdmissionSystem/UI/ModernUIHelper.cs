using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibrarySystem.Models;

namespace LibrarySystem.UI
{
    /// <summary>
    /// Монохромный UI helper - черно-белая минималистичная тема
    /// </summary>
    public static class ModernUIHelper
    {
        // Основные фоновые цвета (оттенки серого)
        public static readonly Color LightBackground = ColorFromHex("#FAFAFA");
        public static readonly Color CardBackground = ColorFromHex("#FFFFFF");
        public static readonly Color SidebarBackground = ColorFromHex("#1A1A1A");

        // Акцентные цвета (черно-белые)
        public static readonly Color PrimaryAccent = ColorFromHex("#000000");      // Черный
        public static readonly Color SecondaryAccent = ColorFromHex("#404040");    // Темно-серый
        public static readonly Color SuccessColor = ColorFromHex("#2D2D2D");       // Почти черный
        public static readonly Color DangerColor = ColorFromHex("#1A1A1A");        // Черный для удаления
        public static readonly Color WarningColor = ColorFromHex("#666666");       // Серый

        // Текст
        public static readonly Color TextPrimary = ColorFromHex("#000000");        // Черный
        public static readonly Color TextSecondary = ColorFromHex("#333333");      // Темно-серый
        public static readonly Color TextMuted = ColorFromHex("#777777");          // Серый

        // Дополнительные оттенки
        public static readonly Color SidebarGradientStart = ColorFromHex("#1A1A1A");
        public static readonly Color SidebarGradientEnd = ColorFromHex("#2D2D2D");
        public static readonly Color CardHoverBackground = ColorFromHex("#F0F0F0");
        public static readonly Color NeutralDark = ColorFromHex("#555555");
        public static readonly Color NeutralDarker = ColorFromHex("#333333");

        // Цвет для разделителей и акцентов
        public static readonly Color AccentGold = ColorFromHex("#000000");         // Черный акцент

        // Белый текст для темных кнопок
        public static readonly Color TextLight = ColorFromHex("#FFFFFF");

        /// <summary>
        /// Создает стильную кнопку с монохромным дизайном
        /// </summary>
        public static Button CreateGradientButton(string text, Point location, Size size, Color startColor, Color endColor)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                BackColor = startColor
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = endColor;
            button.Padding = new Padding(10, 6, 10, 6);
            button.Margin = new Padding(4);

            // Эффект при наведении
            button.MouseEnter += (s, e) => {
                button.BackColor = Color.FromArgb(
                    Math.Min(255, startColor.R + 30),
                    Math.Min(255, startColor.G + 30),
                    Math.Min(255, startColor.B + 30)
                );
            };
            button.MouseLeave += (s, e) => button.BackColor = startColor;

            return button;
        }

        /// <summary>
        /// Создает контурную кнопку (outline style)
        /// </summary>
        public static Button CreateOutlineButton(string text, Point location, Size size)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = PrimaryAccent,
                Cursor = Cursors.Hand,
                BackColor = CardBackground
            };

            button.FlatAppearance.BorderSize = 2;
            button.FlatAppearance.BorderColor = PrimaryAccent;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            button.Padding = new Padding(10, 6, 10, 6);

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
                BackColor = CardBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None,
                Tag = placeholder
            };

            var panel = new Panel
            {
                Location = location,
                Size = new Size(size.Width, size.Height + 10),
                BackColor = LightBackground,
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
        /// Создает панель-карточку с тенью
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
                if (s is not Panel p) return;
                // Черная рамка
                using (var pen = new Pen(Color.FromArgb(60, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            return panel;
        }

        /// <summary>
        /// Создает боковую панель навигации (черная)
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
                // Сплошная заливка черным
                using (var brush = new SolidBrush(SidebarBackground))
                {
                    e.Graphics.FillRectangle(brush, sidebar.ClientRectangle);
                }

                // Правая граница белая
                using (var pen = new Pen(Color.FromArgb(50, 255, 255, 255), 1))
                {
                    e.Graphics.DrawLine(pen, sidebar.Width - 1, 0, sidebar.Width - 1, sidebar.Height);
                }
            };

            return sidebar;
        }

        /// <summary>
        /// Создает кнопку для боковой панели (белый текст на черном)
        /// </summary>
        public static Button CreateSidebarButton(string text, int yPosition, bool isActive = false)
        {
            var button = new Button
            {
                Text = "  " + text,
                Location = new Point(0, yPosition),
                Size = new Size(220, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = isActive ? Color.Black : Color.White,
                BackColor = isActive ? Color.White : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);

            return button;
        }

        /// <summary>
        /// Стилизует DataGridView в монохромной теме
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = CardBackground;
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

            // Заголовки - черные
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryAccent;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = PrimaryAccent;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 50;

            // Стиль ячеек
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 230);
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            // Альтернативные строки
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            dgv.GridColor = Color.FromArgb(220, 220, 220);
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
                BackColor = CardBackground,
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
        /// Создает разделитель (черный)
        /// </summary>
        public static Panel CreateDivider(Point location, int width)
        {
            return new Panel
            {
                Location = location,
                Size = new Size(width, 1),
                BackColor = Color.FromArgb(200, 200, 200)
            };
        }

        /// <summary>
        /// Создает карточку заявки на книгу в монохромном стиле
        /// </summary>
        public static Panel CreateApplicationCard(BookRequest app, EventHandler? onClick = null)
        {
            var card = new Panel
            {
                Size = new Size(340, 200),
                BackColor = CardBackground,
                Padding = new Padding(15),
                Cursor = Cursors.Hand,
                Tag = app
            };

            // Определяем стиль статуса
            string statusSymbol;
            Color statusColor;
            string status = app.Status ?? "На рассмотрении";
            switch (status)
            {
                case "Одобрено":
                    statusSymbol = "[+]";
                    statusColor = Color.FromArgb(40, 40, 40);
                    break;
                case "Отклонено":
                    statusSymbol = "[x]";
                    statusColor = Color.FromArgb(100, 100, 100);
                    break;
                default:
                    statusSymbol = "[?]";
                    statusColor = Color.FromArgb(80, 80, 80);
                    break;
            }

            // Стиль карточки с рамкой
            card.Paint += (s, e) =>
            {
                var panel = (Panel)s;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Белая заливка
                using (var brush = new SolidBrush(panel.BackColor))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }

                // Левая полоса статуса
                using (var brush = new SolidBrush(statusColor))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, 4, panel.Height);
                }

                // Черная рамка
                using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };

            // Название книги
            string fullName = app.FullRequest ?? "Не указано";
            Label lblName = new Label
            {
                Text = fullName.Length > 28 ? fullName.Substring(0, 25) + "..." : fullName,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(15, 12),
                Size = new Size(310, 28),
                BackColor = Color.Transparent
            };

            // Категория
            string specialtyName = app.CategoryName ?? "Не указана";
            Label lblSpecialty = new Label
            {
                Text = "Раздел: " + (specialtyName.Length > 25 ? specialtyName.Substring(0, 22) + "..." : specialtyName),
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(15, 45),
                Size = new Size(310, 22),
                BackColor = Color.Transparent
            };

            // Автор
            Label lblAuthor = new Label
            {
                Text = $"Автор: {app.Author ?? "—"}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(15, 70),
                Size = new Size(310, 22),
                BackColor = Color.Transparent
            };

            // Дата
            string submittedAt = app.SubmittedAt ?? "Не указана";
            Label lblDate = new Label
            {
                Text = $"Дата: {submittedAt}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(15, 100),
                Size = new Size(310, 22),
                BackColor = Color.Transparent
            };

            // ISBN
            string isbn = app.ISBN ?? "";
            Label lblISBN = new Label
            {
                Text = $"ISBN: {(string.IsNullOrEmpty(isbn) ? "—" : isbn)}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(15, 125),
                Size = new Size(310, 22),
                BackColor = Color.Transparent
            };

            // Статус
            Label lblStatus = new Label
            {
                Text = $"{statusSymbol} {status.ToUpper()}",
                Font = new Font("Consolas", 11, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(15, 160),
                Size = new Size(310, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Добавляем элементы
            card.Controls.Add(lblName);
            card.Controls.Add(lblSpecialty);
            card.Controls.Add(lblAuthor);
            card.Controls.Add(lblDate);
            card.Controls.Add(lblISBN);
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
                card.BackColor = CardBackground;
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
