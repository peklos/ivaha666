using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtLogin = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Button btnRegister = null!;
        private LinkLabel linkInstruction = null!;
        private Panel panelLeft = null!;
        private Panel panelRight = null!;
        private CheckBox chkShowPassword = null!;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 650);
            this.Text = "КНИЖНЫЙ ФОНД — Авторизация";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            // Левая панель (черная с названием)
            panelLeft = new Panel
            {
                Size = new Size(450, 650),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(20, 20, 20)
            };
            panelLeft.Paint += PanelLeft_Paint;

            // Название системы
            Label lblSystemName = new Label
            {
                Text = "КНИЖНЫЙ\nФОНД",
                Font = new Font("Segoe UI", 42, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(400, 140),
                Location = new Point(25, 180),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            Label lblTagline = new Label
            {
                Text = "Система управления\nкнижным фондом",
                Font = new Font("Segoe UI Light", 16),
                ForeColor = Color.FromArgb(180, 180, 180),
                Size = new Size(400, 60),
                Location = new Point(25, 340),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Декоративные линии
            Panel line1 = new Panel
            {
                Size = new Size(80, 3),
                Location = new Point(25, 160),
                BackColor = Color.White
            };

            Panel line2 = new Panel
            {
                Size = new Size(150, 1),
                Location = new Point(25, 420),
                BackColor = Color.FromArgb(60, 60, 60)
            };

            // Версия
            Label lblVersion = new Label
            {
                Text = "v2.0",
                Font = new Font("Consolas", 11),
                ForeColor = Color.FromArgb(100, 100, 100),
                Size = new Size(100, 25),
                Location = new Point(25, 580),
                BackColor = Color.Transparent
            };

            panelLeft.Controls.Add(lblSystemName);
            panelLeft.Controls.Add(lblTagline);
            panelLeft.Controls.Add(line1);
            panelLeft.Controls.Add(line2);
            panelLeft.Controls.Add(lblVersion);

            // Правая панель (белая с формой)
            panelRight = new Panel
            {
                Size = new Size(550, 650),
                Location = new Point(450, 0),
                BackColor = Color.White
            };

            // Заголовок формы
            Label lblFormTitle = new Label
            {
                Text = "ВХОД В СИСТЕМУ",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(450, 50),
                Location = new Point(50, 120),
                BackColor = Color.Transparent
            };

            Label lblFormSubtitle = new Label
            {
                Text = "Введите учетные данные для входа",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(120, 120, 120),
                Size = new Size(450, 30),
                Location = new Point(50, 175),
                BackColor = Color.Transparent
            };

            // Логин
            Label lblLogin = new Label
            {
                Text = "ЛОГИН",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Size = new Size(400, 20),
                Location = new Point(50, 240),
                BackColor = Color.Transparent
            };

            Panel panelLoginBox = new Panel
            {
                Location = new Point(50, 265),
                Size = new Size(400, 50),
                BackColor = Color.FromArgb(245, 245, 245)
            };
            panelLoginBox.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, panelLoginBox.Width - 1, panelLoginBox.Height - 1);
            };

            txtLogin = new TextBox
            {
                Location = new Point(15, 12),
                Size = new Size(370, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(245, 245, 245),
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None
            };
            panelLoginBox.Controls.Add(txtLogin);

            // Пароль
            Label lblPassword = new Label
            {
                Text = "ПАРОЛЬ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Size = new Size(400, 20),
                Location = new Point(50, 330),
                BackColor = Color.Transparent
            };

            Panel panelPasswordBox = new Panel
            {
                Location = new Point(50, 355),
                Size = new Size(400, 50),
                BackColor = Color.FromArgb(245, 245, 245)
            };
            panelPasswordBox.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, panelPasswordBox.Width - 1, panelPasswordBox.Height - 1);
            };

            txtPassword = new TextBox
            {
                Location = new Point(15, 12),
                Size = new Size(370, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(245, 245, 245),
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            panelPasswordBox.Controls.Add(txtPassword);

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароль",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(50, 415),
                ForeColor = Color.FromArgb(100, 100, 100),
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопка входа (черная)
            btnLogin = new Button
            {
                Text = "ВОЙТИ",
                Location = new Point(50, 460),
                Size = new Size(190, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(40, 40, 40);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.Black;

            // Кнопка регистрации (контурная)
            btnRegister = new Button
            {
                Text = "РЕГИСТРАЦИЯ",
                Location = new Point(260, 460),
                Size = new Size(190, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 2;
            btnRegister.FlatAppearance.BorderColor = Color.Black;
            btnRegister.Click += BtnRegister_Click;
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = Color.FromArgb(245, 245, 245);
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = Color.White;

            // Ссылка на инструкцию
            linkInstruction = new LinkLabel
            {
                Text = "Инструкция пользователя",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(50, 530),
                LinkColor = Color.FromArgb(80, 80, 80),
                ActiveLinkColor = Color.Black,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            linkInstruction.LinkClicked += LinkInstruction_LinkClicked;

            // Разделитель
            Panel divider = new Panel
            {
                Size = new Size(400, 1),
                Location = new Point(50, 570),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            // Тестовые данные
            Label lblTestData = new Label
            {
                Text = "Тест: admin / admin123  |  student1 / pass123",
                Font = new Font("Consolas", 9),
                ForeColor = Color.FromArgb(150, 150, 150),
                Size = new Size(400, 20),
                Location = new Point(50, 585),
                BackColor = Color.Transparent
            };

            // Добавляем элементы на правую панель
            panelRight.Controls.Add(lblFormTitle);
            panelRight.Controls.Add(lblFormSubtitle);
            panelRight.Controls.Add(lblLogin);
            panelRight.Controls.Add(panelLoginBox);
            panelRight.Controls.Add(lblPassword);
            panelRight.Controls.Add(panelPasswordBox);
            panelRight.Controls.Add(chkShowPassword);
            panelRight.Controls.Add(btnLogin);
            panelRight.Controls.Add(btnRegister);
            panelRight.Controls.Add(linkInstruction);
            panelRight.Controls.Add(divider);
            panelRight.Controls.Add(lblTestData);

            this.Controls.Add(panelLeft);
            this.Controls.Add(panelRight);

            this.AcceptButton = btnLogin;
        }

        private void PanelLeft_Paint(object sender, PaintEventArgs e)
        {
            // Градиент на левой панели
            using (var brush = new LinearGradientBrush(
                panelLeft.ClientRectangle,
                Color.FromArgb(15, 15, 15),
                Color.FromArgb(35, 35, 35),
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, panelLeft.ClientRectangle);
            }

            // Декоративные круги
            using (var brush = new SolidBrush(Color.FromArgb(10, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(brush, -100, 400, 250, 250);
                e.Graphics.FillEllipse(brush, 300, -80, 200, 200);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                User user = DatabaseHelper.GetUser(login, password);

                if (user != null)
                {
                    this.Hide();

                    if (user.Role == "Admin")
                    {
                        AdminPanel adminPanel = new AdminPanel(user);
                        adminPanel.FormClosed += (s, args) => this.Close();
                        adminPanel.Show();
                    }
                    else
                    {
                        UserPanel userPanel = new UserPanel(user);
                        userPanel.FormClosed += (s, args) => this.Close();
                        userPanel.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к базе данных.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                RegisterForm registerForm = new RegisterForm();
                registerForm.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму регистрации", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkInstruction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Инструкция_пользователя.docx");
            string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Инструкция_пользователя.docx");

            string instructionPath = File.Exists(rootPath) ? rootPath : resourcesPath;

            if (File.Exists(instructionPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.GetFullPath(instructionPath),
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть инструкцию: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл инструкции не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
