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
        private TextBox txtStudentNumber = null!;
        private TextBox txtAddress = null!;
        private Button btnRegister = null!;
        private Button btnCancel = null!;
        private CheckBox chkShowPassword = null!;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(550, 750);
            this.Text = "Регистрация студента - Библиотека БППК";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.LightBackground;
            this.DoubleBuffered = true;

            int yPos = 20;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "РЕГИСТРАЦИЯ СТУДЕНТА",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(500, 40),
                Location = new Point(25, yPos),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            yPos += 45;

            Label lblSubtitle = new Label
            {
                Text = "Заполните форму для получения доступа к библиотеке",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernUIHelper.TextMuted,
                Size = new Size(500, 20),
                Location = new Point(25, yPos),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            yPos += 30;

            // Разделитель
            Panel divider = ModernUIHelper.CreateDivider(new Point(25, yPos), 500);
            yPos += 15;

            // ФИО
            Label lblFullName = CreateLabel("ФИО студента *", yPos);
            yPos += 25;
            txtFullName = CreateTextBox(yPos, "Иванов Иван Иванович");
            yPos += 50;

            // Номер студенческого
            Label lblStudentNumber = CreateLabel("Номер студенческого билета *", yPos);
            yPos += 25;
            txtStudentNumber = CreateTextBox(yPos, "СТ-2024-XXX");
            yPos += 50;

            // Адрес
            Label lblAddress = CreateLabel("Адрес проживания *", yPos);
            yPos += 25;
            txtAddress = CreateTextBox(yPos, "г. Белгород, ул. ...");
            yPos += 50;

            // Email
            Label lblEmail = CreateLabel("Email *", yPos);
            yPos += 25;
            txtEmail = CreateTextBox(yPos, "student@bppk.ru");
            yPos += 50;

            // Телефон
            Label lblPhone = CreateLabel("Телефон *", yPos);
            yPos += 25;
            txtPhoneNumber = CreateTextBox(yPos, "+7 (___) ___-__-__");
            yPos += 50;

            // Логин
            Label lblLogin = CreateLabel("Логин для входа *", yPos);
            yPos += 25;
            txtLogin = CreateTextBox(yPos, "Минимум 3 символа");
            yPos += 50;

            // Пароли в одну строку
            Label lblPassword = CreateLabel("Пароль *", yPos);
            Label lblConfirmPassword = new Label
            {
                Text = "Подтверждение *",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(240, 20),
                Location = new Point(275, yPos),
                BackColor = Color.Transparent
            };
            yPos += 25;

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(235, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                UseSystemPasswordChar = true
            };

            txtConfirmPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(275, yPos),
                Size = new Size(235, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                UseSystemPasswordChar = true
            };
            yPos += 45;

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароли",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(25, yPos),
                ForeColor = ModernUIHelper.TextMuted,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };
            yPos += 35;

            // Разделитель
            Panel divider2 = ModernUIHelper.CreateDivider(new Point(25, yPos), 500);
            yPos += 15;

            // Кнопки
            btnRegister = ModernUIHelper.CreateGradientButton(
                "ЗАРЕГИСТРИРОВАТЬСЯ",
                new Point(25, yPos),
                new Size(240, 45),
                ModernUIHelper.PrimaryAccent,
                ModernUIHelper.PrimaryAccent
            );
            btnRegister.Click += BtnRegister_Click;

            btnCancel = ModernUIHelper.CreateGradientButton(
                "ОТМЕНА",
                new Point(275, yPos),
                new Size(235, 45),
                ModernUIHelper.NeutralDark,
                ModernUIHelper.NeutralDarker
            );
            btnCancel.Click += (s, e) => this.Close();

            // Добавление элементов
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(divider);
            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblStudentNumber);
            this.Controls.Add(txtStudentNumber);
            this.Controls.Add(lblAddress);
            this.Controls.Add(txtAddress);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhoneNumber);
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(chkShowPassword);
            this.Controls.Add(divider2);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnRegister;
        }

        private Label CreateLabel(string text, int yPos)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(500, 20),
                Location = new Point(25, yPos),
                BackColor = Color.Transparent
            };
        }

        private TextBox CreateTextBox(int yPos, string placeholder)
        {
            var textBox = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(485, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            textBox.PlaceholderText = placeholder;
            return textBox;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string studentNumber = txtStudentNumber.Text.Trim();
            string address = txtAddress.Text.Trim();
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

            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Введите номер студенческого билета!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Введите адрес проживания!", "Ошибка",
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

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка",
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
                    StudentNumber = studentNumber,
                    Address = address,
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
