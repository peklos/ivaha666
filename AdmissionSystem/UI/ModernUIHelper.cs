using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibrarySystem.Models;

namespace LibrarySystem.UI
{
    /// <summary>
    /// Библиотечный UI helper - тёплые коричневые и бордовые тона
    /// </summary>
    public static class ModernUIHelper
    {
        // Основные фоновые цвета (тёплые бежевые тона)
        public static readonly Color LightBackground = ColorFromHex("#FDF8F3");
        public static readonly Color CardBackground = ColorFromHex("#FFFBF7");
        public static readonly Color SidebarBackground = ColorFromHex("#F5EDE4");

        // Акцентные цвета (библиотечные - бордовый и коричневый)
        public static readonly Color PrimaryAccent = ColorFromHex("#8B2635");      // Бордовый
        public static readonly Color SecondaryAccent = ColorFromHex("#6B4423");    // Коричневый
        public static readonly Color SuccessColor = ColorFromHex("#4A7C59");       // Тёмно-зелёный
        public static readonly Color DangerColor = ColorFromHex("#C23B22");        // Красный
        public static readonly Color WarningColor = ColorFromHex("#D4A017");       // Золотистый

        // Текст
        public static readonly Color TextPrimary = ColorFromHex("#2C1810");        // Тёмно-коричневый
        public static readonly Color TextSecondary = ColorFromHex("#5D4E37");      // Коричневый
        public static readonly Color TextMuted = ColorFromHex("#8B7355");          // Светло-коричневый

        // Дополнительные оттенки
        public static readonly Color SidebarGradientStart = ColorFromHex("#F5EDE4");
        public static readonly Color SidebarGradientEnd = ColorFromHex("#E8DDD0");
        public static readonly Color CardHoverBackground = ColorFromHex("#F0E6D9");
        public static readonly Color NeutralDark = ColorFromHex("#A39585");
        public static readonly Color NeutralDarker = ColorFromHex("#7D6E5D");

        // Цвет для кнопок и акцентов
        public static readonly Color AccentGold = ColorFromHex("#C4A35A");         // Золотой акцент

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
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                BackColor = startColor
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = endColor;
            button.Padding = new Padding(12, 8, 12, 8);
            button.Margin = new Padding(6);

            // Эффект при наведении
            button.MouseEnter += (s, e) => {
                button.BackColor = Color.FromArgb(
                    Math.Min(255, startColor.R + 20),
                    Math.Min(255, startColor.G + 20),
                    Math.Min(255, startColor.B + 20)
                );
            };
            button.MouseLeave += (s, e) => button.BackColor = startColor;

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
                BackColor = SidebarBackground,
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
                if (s is not Panel p) return;
                // Тёплая тень
                using (var pen = new Pen(Color.FromArgb(40, SecondaryAccent), 1))
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

                // Декоративная полоса справа
                using (var pen = new Pen(PrimaryAccent, 3))
                {
                    e.Graphics.DrawLine(pen, sidebar.Width - 2, 0, sidebar.Width - 2, sidebar.Height);
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
                ForeColor = isActive ? Color.White : TextSecondary,
                BackColor = isActive ? PrimaryAccent : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = CardHoverBackground;

            return button;
        }

        /// <summary>
        /// Стилизует DataGridView в библиотечной теме
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

            // Стиль заголовков - бордовый акцент
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
            dgv.DefaultCellStyle.SelectionBackColor = CardHoverBackground;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            // Альтернативные строки
            dgv.AlternatingRowsDefaultCellStyle.BackColor = SidebarBackground;

            dgv.GridColor = SidebarGradientEnd;
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
        /// Создает разделитель
        /// </summary>
        public static Panel CreateDivider(Point location, int width)
        {
            return new Panel
            {
                Location = location,
                Size = new Size(width, 2),
                BackColor = AccentGold
            };
        }

        /// <summary>
        /// Создает карточку заявки на книгу
        /// </summary>
        public static Panel CreateApplicationCard(BookRequest app, EventHandler? onClick = null)
        {
            var card = new Panel
            {
                Size = new Size(350, 220),
                BackColor = CardBackground,
                Padding = new Padding(15),
                Cursor = Cursors.Hand,
                Tag = app
            };

            // Определяем цвет статуса
            Color statusColor;
            string status = app.Status ?? "На рассмотрении";
            switch (status)
            {
                case "Одобрено":
                    statusColor = SuccessColor;
                    break;
                case "Отклонено":
                    statusColor = DangerColor;
                    break;
                default:
                    statusColor = WarningColor;
                    break;
            }

            // Стиль карточки с закруглениями
            card.Paint += (s, e) =>
            {
                var panel = (Panel)s;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Заливка
                using (var brush = new SolidBrush(panel.BackColor))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }

                // Верхняя полоса статуса
                using (var brush = new SolidBrush(statusColor))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, panel.Width, 5);
                }

                // Рамка
                using (var pen = new Pen(SidebarGradientEnd, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };

            // Иконка книги
            Label lblIcon = new Label
            {
                Text = "📖",
                Font = new Font("Segoe UI", 24),
                Location = new Point(10, 15),
                Size = new Size(50, 40),
                BackColor = Color.Transparent
            };

            // Название книги (крупно)
            string fullName = app.FullRequest ?? "Не указано";
            Label lblName = new Label
            {
                Text = fullName.Length > 30 ? fullName.Substring(0, 27) + "..." : fullName,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(65, 15),
                Size = new Size(270, 30),
                BackColor = Color.Transparent
            };

            // Категория
            string specialtyName = app.CategoryName ?? "Не указана";
            Label lblSpecialty = new Label
            {
                Text = "Категория: " + (specialtyName.Length > 22 ? specialtyName.Substring(0, 19) + "..." : specialtyName),
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(10, 60),
                Size = new Size(320, 25),
                BackColor = Color.Transparent
            };

            // Автор
            Label lblScore = new Label
            {
                Text = $"Автор: {app.Author}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextSecondary,
                Location = new Point(10, 85),
                Size = new Size(320, 25),
                BackColor = Color.Transparent
            };

            // Дата
            string submittedAt = app.SubmittedAt ?? "Не указана";
            Label lblDate = new Label
            {
                Text = $"Дата заявки: {submittedAt}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(10, 115),
                Size = new Size(320, 25),
                BackColor = Color.Transparent
            };

            // ISBN
            string isbn = app.ISBN ?? "";
            Label lblPassport = new Label
            {
                Text = $"ISBN: {(string.IsNullOrEmpty(isbn) ? "—" : isbn)}",
                Font = new Font("Segoe UI", 9),
                ForeColor = TextMuted,
                Location = new Point(10, 140),
                Size = new Size(320, 25),
                BackColor = Color.Transparent
            };

            // Статус (в нижней части)
            Label lblStatus = new Label
            {
                Text = status.ToUpper(),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(10, 175),
                Size = new Size(320, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Добавляем все элементы на карточку
            card.Controls.Add(lblIcon);
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
