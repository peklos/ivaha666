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
            this.Size = new Size(520, 720);
            this.Text = "КНИЖНЫЙ ФОНД — Регистрация";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            int yPos = 25;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "РЕГИСТРАЦИЯ",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(460, 45),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            yPos += 50;

            Label lblSubtitle = new Label
            {
                Text = "Заполните форму для создания аккаунта",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(110, 110, 110),
                Size = new Size(460, 25),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            yPos += 35;

            // Разделитель
            Panel divider = new Panel
            {
                Size = new Size(460, 1),
                Location = new Point(30, yPos),
                BackColor = Color.FromArgb(220, 220, 220)
            };
            yPos += 20;

            // ФИО
            Label lblFullName = CreateLabel("ФИО", yPos);
            yPos += 22;
            txtFullName = CreateTextBox(yPos, "Иванов Иван Иванович");
            yPos += 48;

            // Номер студенческого
            Label lblStudentNumber = CreateLabel("НОМЕР СТУДЕНЧЕСКОГО", yPos);
            yPos += 22;
            txtStudentNumber = CreateTextBox(yPos, "СТ-2024-XXX");
            yPos += 48;

            // Адрес
            Label lblAddress = CreateLabel("АДРЕС", yPos);
            yPos += 22;
            txtAddress = CreateTextBox(yPos, "г. Москва, ул. ...");
            yPos += 48;

            // Email
            Label lblEmail = CreateLabel("EMAIL", yPos);
            yPos += 22;
            txtEmail = CreateTextBox(yPos, "student@mail.ru");
            yPos += 48;

            // Телефон
            Label lblPhone = CreateLabel("ТЕЛЕФОН", yPos);
            yPos += 22;
            txtPhoneNumber = CreateTextBox(yPos, "+7 (___) ___-__-__");
            yPos += 48;

            // Логин
            Label lblLogin = CreateLabel("ЛОГИН", yPos);
            yPos += 22;
            txtLogin = CreateTextBox(yPos, "Минимум 3 символа");
            yPos += 48;

            // Пароли
            Label lblPassword = CreateLabel("ПАРОЛЬ", yPos);
            Label lblConfirmPassword = new Label
            {
                Text = "ПОДТВЕРЖДЕНИЕ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Size = new Size(220, 20),
                Location = new Point(255, yPos),
                BackColor = Color.Transparent
            };
            yPos += 22;

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, yPos),
                Size = new Size(215, 38),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black,
                UseSystemPasswordChar = true
            };

            txtConfirmPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(255, yPos),
                Size = new Size(215, 38),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black,
                UseSystemPasswordChar = true
            };
            yPos += 45;

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароли",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 22),
                Location = new Point(30, yPos),
                ForeColor = Color.FromArgb(100, 100, 100),
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };
            yPos += 35;

            // Кнопки
            btnRegister = new Button
            {
                Text = "СОЗДАТЬ АККАУНТ",
                Location = new Point(30, yPos),
                Size = new Size(220, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = Color.FromArgb(40, 40, 40);
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = Color.Black;

            btnCancel = new Button
            {
                Text = "ОТМЕНА",
                Location = new Point(260, yPos),
                Size = new Size(210, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
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
                ForeColor = Color.FromArgb(80, 80, 80),
                Size = new Size(460, 20),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
        }

        private TextBox CreateTextBox(int yPos, string placeholder)
        {
            var textBox = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, yPos),
                Size = new Size(440, 38),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black
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
                MessageBox.Show("Введите ФИО!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Введите номер студенческого!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Введите адрес!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Введите корректный email!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(phone) || phone.Length < 5)
            {
                MessageBox.Show("Введите корректный телефон!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(login) || login.Length < 3)
            {
                MessageBox.Show("Логин должен быть минимум 3 символа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password) || password.Length < 4)
            {
                MessageBox.Show("Пароль должен быть минимум 4 символа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DatabaseHelper.UserExists(login))
            {
                MessageBox.Show("Такой логин уже занят!", "Внимание",
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

                MessageBox.Show("Регистрация успешна!\nТеперь можете войти.",
                    "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
