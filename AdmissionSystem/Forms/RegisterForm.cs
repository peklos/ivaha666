using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class RegisterForm : Form
    {
        private TextBox txtLogin = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private TextBox txtFullName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhoneNumber = null!;
        private Button btnRegister = null!;
        private Button btnCancel = null!;
        private CheckBox chkShowPassword = null!;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(900, 760);
            this.Text = "Регистрация нового пользователя";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            // Фоновый градиент
            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.White,
                    Color.White,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            // Главная карточка
            Panel cardPanel = ModernUIHelper.CreateCard(new Point(100, 40), new Size(700, 680));
            cardPanel.BackColor = Color.White;

            // Заголовок с иконкой
            Label lblIcon = new Label
            {
                Text = "✨",
                Font = new Font("Segoe UI", 48),
                ForeColor = ModernUIHelper.SecondaryAccent,
                Size = new Size(660, 80),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblTitle = ModernUIHelper.CreateModernLabel(
                "СОЗДАТЬ НОВЫЙ АККАУНТ",
                new Point(20, 110),
                18,
                FontStyle.Bold,
                ModernUIHelper.TextPrimary
            );
            lblTitle.Size = new Size(660, 35);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            Label lblSubtitle = ModernUIHelper.CreateModernLabel(
                "Заполните форму для регистрации в системе",
                new Point(20, 145),
                10,
                FontStyle.Regular,
                ModernUIHelper.TextSecondary
            );
            lblSubtitle.Size = new Size(660, 25);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // Разделитель
            Panel divider1 = ModernUIHelper.CreateDivider(new Point(20, 185), 660);
            divider1.BackColor = Color.White;

            // ФИО
            Label lblFullName = ModernUIHelper.CreateModernLabel(
                "ПОЛНОЕ ИМЯ",
                new Point(50, 210),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextMuted
            );

            Panel panelFullNameBox = new Panel
            {
                Location = new Point(50, 235),
                Size = new Size(600, 45),
                BackColor = Color.White
            };

            txtFullName = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(570, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            txtFullName.PlaceholderText = "ФИО";
            panelFullNameBox.Controls.Add(txtFullName);
            panelFullNameBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            Label lblEmail = ModernUIHelper.CreateModernLabel(
                "ПОЧТА",
                new Point(50, 265),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextMuted
            );

            Panel panelEmailBox = new Panel
            {
                Location = new Point(50, 290),
                Size = new Size(600, 45),
                BackColor = Color.White
            };

            txtEmail = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(570, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            txtEmail.PlaceholderText = "example@domain.ru";
            panelEmailBox.Controls.Add(txtEmail);
            panelEmailBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            // Телефон
            Label lblPhone = ModernUIHelper.CreateModernLabel(
                "ТЕЛЕФОН",
                new Point(50, 320),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextMuted
            );

            Panel panelPhoneBox = new Panel
            {
                Location = new Point(50, 345),
                Size = new Size(600, 45),
                BackColor = Color.White
            };

            txtPhoneNumber = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(570, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            txtPhoneNumber.PlaceholderText = "+7 (___) ___-__-__";
            panelPhoneBox.Controls.Add(txtPhoneNumber);
            panelPhoneBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            // Логин

            Label lblLogin = ModernUIHelper.CreateModernLabel(
                "ЛОГИН",
                new Point(50, 375),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextMuted
            );

            Panel panelLoginBox = new Panel
            {
                Location = new Point(50, 400),
                Size = new Size(600, 45),
                BackColor = Color.White
            };

            txtLogin = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(570, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            txtLogin.PlaceholderText = "Логин";
            panelLoginBox.Controls.Add(txtLogin);
            panelLoginBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            // Пароль
            Label lblPassword = ModernUIHelper.CreateModernLabel(
                "ПАРОЛЬ",
                new Point(50, 455),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextMuted
            );

            Panel panelPasswordBox = new Panel
            {
                Location = new Point(50, 480),
                Size = new Size(290, 45),
                BackColor = Color.White
            };

            txtPassword = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            txtPassword.PlaceholderText = "Пароль";
            panelPasswordBox.Controls.Add(txtPassword);
            panelPasswordBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            // Подтверждение пароля
            Label lblConfirmPassword = ModernUIHelper.CreateModernLabel(
                "ПОВТОРИТЕ ПАРОЛЬ",
                new Point(360, 455),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextMuted
            );

            Panel panelConfirmPasswordBox = new Panel
            {
                Location = new Point(360, 480),
                Size = new Size(290, 45),
                BackColor = Color.White
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            txtConfirmPassword.PlaceholderText = "Повторите пароль";
            panelConfirmPasswordBox.Controls.Add(txtConfirmPassword);
            panelConfirmPasswordBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароли",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(50, 535),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Разделитель
            Panel divider2 = ModernUIHelper.CreateDivider(new Point(20, 570), 660);
            divider2.BackColor = Color.White;

            // Кнопки
            // Кнопки регистрации — таблица 2 столбца
            var actionsPanel = new TableLayoutPanel
            {
                Location = new Point(50, 600),
                Size = new Size(600, 110),
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2
            };
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));

            btnRegister = ModernUIHelper.CreateGradientButton("ЗАРЕГИСТРИРОВАТЬСЯ", Point.Empty, new Size(1,1), ModernUIHelper.PrimaryAccent, ModernUIHelper.PrimaryAccent);
            btnRegister.Click += BtnRegister_Click;
            btnCancel = ModernUIHelper.CreateGradientButton("ОТМЕНА", Point.Empty, new Size(1,1), Color.FromArgb(240,240,240), Color.FromArgb(240,240,240));
            btnCancel.ForeColor = ModernUIHelper.TextPrimary;
            btnCancel.Click += (s, e) => this.Close();

            btnRegister.Dock = DockStyle.Fill; btnCancel.Dock = DockStyle.Fill;

            actionsPanel.Controls.Add(btnRegister, 0, 0);
            actionsPanel.Controls.Add(btnCancel, 0, 1);

            // Добавление элементов на карточку
            cardPanel.Controls.Add(lblIcon);
            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(lblSubtitle);
            cardPanel.Controls.Add(divider1);
            cardPanel.Controls.Add(lblFullName);
            cardPanel.Controls.Add(panelFullNameBox);
            cardPanel.Controls.Add(lblEmail);
            cardPanel.Controls.Add(panelEmailBox);
            cardPanel.Controls.Add(lblPhone);
            cardPanel.Controls.Add(panelPhoneBox);
            cardPanel.Controls.Add(lblLogin);
            cardPanel.Controls.Add(panelLoginBox);
            cardPanel.Controls.Add(lblPassword);
            cardPanel.Controls.Add(panelPasswordBox);
            cardPanel.Controls.Add(lblConfirmPassword);
            cardPanel.Controls.Add(panelConfirmPasswordBox);
            cardPanel.Controls.Add(chkShowPassword);
            cardPanel.Controls.Add(divider2);
            cardPanel.Controls.Add(actionsPanel);

            this.Controls.Add(cardPanel);

            // Enter для регистрации
            this.AcceptButton = btnRegister;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();

            // Валидация
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Введите ваше ФИО!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Введите корректную почту!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(phone) || phone.Length < 5)
            {
                MessageBox.Show("Введите корректный номер телефона!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должен содержать минимум 4 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка уникальности логина
            if (DatabaseHelper.UserExists(login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                User newUser = new User
                {
                    Login = login,
                    Password = password.Trim(),
                    FullName = fullName,
                    Email = email,
                    PhoneNumber = phone,
                    Role = "User"
                };

                DatabaseHelper.RegisterUser(newUser);

                MessageBox.Show("Регистрация прошла успешно!\nТеперь вы можете войти в систему.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
