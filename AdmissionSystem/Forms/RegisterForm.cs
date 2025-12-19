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
            this.BackColor = ModernUIHelper.LightBackground;
            this.DoubleBuffered = true;

            // Главная карточка
            Panel cardPanel = new Panel
            {
                Location = new Point(100, 40),
                Size = new Size(700, 680),
                BackColor = ModernUIHelper.LightBackground
            };
            cardPanel.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(ModernUIHelper.BorderColor, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            // Заголовок с иконкой
            Label lblIcon = new Label
            {
                Text = "REG",
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(660, 70),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblTitle = ModernUIHelper.CreateModernLabel(
                "СОЗДАТЬ НОВЫЙ АККАУНТ",
                new Point(20, 95),
                18,
                FontStyle.Bold,
                ModernUIHelper.TextPrimary
            );
            lblTitle.Size = new Size(660, 35);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            Label lblSubtitle = ModernUIHelper.CreateModernLabel(
                "Заполните форму для регистрации в системе",
                new Point(20, 130),
                10,
                FontStyle.Regular,
                ModernUIHelper.TextMuted
            );
            lblSubtitle.Size = new Size(660, 25);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // Разделитель
            Panel divider1 = new Panel
            {
                Location = new Point(20, 165),
                Size = new Size(660, 2),
                BackColor = ModernUIHelper.BorderColor
            };

            // ФИО
            Label lblFullName = ModernUIHelper.CreateModernLabel(
                "ПОЛНОЕ ИМЯ",
                new Point(50, 185),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelFullNameBox = CreateInputPanel(new Point(50, 210), new Size(600, 45));
            txtFullName = CreateInput(panelFullNameBox);

            // Почта
            Label lblEmail = ModernUIHelper.CreateModernLabel(
                "ПОЧТА",
                new Point(50, 265),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelEmailBox = CreateInputPanel(new Point(50, 290), new Size(600, 45));
            txtEmail = CreateInput(panelEmailBox);

            // Телефон
            Label lblPhone = ModernUIHelper.CreateModernLabel(
                "ТЕЛЕФОН",
                new Point(50, 345),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelPhoneBox = CreateInputPanel(new Point(50, 370), new Size(600, 45));
            txtPhoneNumber = CreateInput(panelPhoneBox);

            // Логин
            Label lblLogin = ModernUIHelper.CreateModernLabel(
                "ЛОГИН",
                new Point(50, 425),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelLoginBox = CreateInputPanel(new Point(50, 450), new Size(600, 45));
            txtLogin = CreateInput(panelLoginBox);

            // Пароль
            Label lblPassword = ModernUIHelper.CreateModernLabel(
                "ПАРОЛЬ",
                new Point(50, 505),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelPasswordBox = CreateInputPanel(new Point(50, 530), new Size(290, 45));
            txtPassword = CreateInput(panelPasswordBox);
            txtPassword.UseSystemPasswordChar = true;

            // Подтверждение пароля
            Label lblConfirmPassword = ModernUIHelper.CreateModernLabel(
                "ПОВТОРИТЕ ПАРОЛЬ",
                new Point(360, 505),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelConfirmPasswordBox = CreateInputPanel(new Point(360, 530), new Size(290, 45));
            txtConfirmPassword = CreateInput(panelConfirmPasswordBox);
            txtConfirmPassword.UseSystemPasswordChar = true;

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароли",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(50, 585),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопки регистрации
            btnRegister = ModernUIHelper.CreateGradientButton("ЗАРЕГИСТРИРОВАТЬСЯ", new Point(50, 620), new Size(600, 50), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
            btnRegister.Click += BtnRegister_Click;

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
            cardPanel.Controls.Add(btnRegister);

            this.Controls.Add(cardPanel);

            // Enter для регистрации
            this.AcceptButton = btnRegister;
        }

        private Panel CreateInputPanel(Point location, Size size)
        {
            Panel panel = new Panel
            {
                Location = location,
                Size = size,
                BackColor = ModernUIHelper.LightBackground
            };
            panel.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(ModernUIHelper.PrimaryAccent, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };
            return panel;
        }

        private TextBox CreateInput(Panel parent)
        {
            TextBox txt = new TextBox
            {
                Location = new Point(15, 11),
                Size = new Size(parent.Size.Width - 30, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            parent.Controls.Add(txt);
            return txt;
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
