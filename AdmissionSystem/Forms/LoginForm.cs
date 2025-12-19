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
            try
            {
                DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
            }

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 650);
            this.Text = "Библиотека БППК - Вход";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.LightBackground;
            this.DoubleBuffered = true;

            // Левая панель с формой входа (поля ввода СЛЕВА)
            panelLeft = new Panel
            {
                Size = new Size(600, 650),
                Location = new Point(0, 0),
                BackColor = ModernUIHelper.LightBackground
            };

            // Заголовок формы
            Label lblFormTitle = ModernUIHelper.CreateModernLabel(
                "ВХОД В СИСТЕМУ",
                new Point(80, 120),
                24,
                FontStyle.Bold,
                ModernUIHelper.TextPrimary
            );

            Label lblFormSubtitle = ModernUIHelper.CreateModernLabel(
                "Введите ваши учётные данные для продолжения",
                new Point(80, 165),
                10,
                FontStyle.Regular,
                ModernUIHelper.TextMuted
            );

            // Логин
            Label lblLogin = ModernUIHelper.CreateModernLabel(
                "ЛОГИН",
                new Point(80, 230),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelLoginBox = new Panel
            {
                Location = new Point(80, 255),
                Size = new Size(440, 50),
                BackColor = ModernUIHelper.LightBackground
            };
            panelLoginBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(ModernUIHelper.PrimaryAccent, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            txtLogin = new TextBox
            {
                Location = new Point(15, 13),
                Size = new Size(410, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelLoginBox.Controls.Add(txtLogin);

            // Пароль
            Label lblPassword = ModernUIHelper.CreateModernLabel(
                "ПАРОЛЬ",
                new Point(80, 325),
                9,
                FontStyle.Bold,
                ModernUIHelper.TextSecondary
            );

            Panel panelPasswordBox = new Panel
            {
                Location = new Point(80, 350),
                Size = new Size(440, 50),
                BackColor = ModernUIHelper.LightBackground
            };
            panelPasswordBox.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(ModernUIHelper.PrimaryAccent, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            txtPassword = new TextBox
            {
                Location = new Point(15, 13),
                Size = new Size(410, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary,
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
                Location = new Point(80, 410),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопка входа
            btnLogin = ModernUIHelper.CreateGradientButton(
                "ВОЙТИ В СИСТЕМУ",
                new Point(80, 460),
                new Size(440, 55),
                ModernUIHelper.PrimaryAccent,
                ModernUIHelper.SecondaryAccent
            );
            btnLogin.Click += BtnLogin_Click;

            // Кнопка регистрации
            btnRegister = ModernUIHelper.CreateGradientButton(
                "РЕГИСТРАЦИЯ",
                new Point(80, 525),
                new Size(440, 55),
                ModernUIHelper.SecondaryAccent,
                ModernUIHelper.PrimaryAccent
            );
            btnRegister.Click += BtnRegister_Click;

            // Добавление элементов на левую панель
            panelLeft.Controls.Add(lblFormTitle);
            panelLeft.Controls.Add(lblFormSubtitle);
            panelLeft.Controls.Add(lblLogin);
            panelLeft.Controls.Add(panelLoginBox);
            panelLeft.Controls.Add(lblPassword);
            panelLeft.Controls.Add(panelPasswordBox);
            panelLeft.Controls.Add(chkShowPassword);
            panelLeft.Controls.Add(btnLogin);
            panelLeft.Controls.Add(btnRegister);

            // Правая декоративная панель с градиентом (декорация СПРАВА)
            panelRight = new Panel
            {
                Size = new Size(400, 650),
                Location = new Point(600, 0),
                BackColor = ModernUIHelper.SidebarBackground
            };
            panelRight.Paint += PanelRight_Paint;

            // Логотип и текст на правой панели
            Label lblLogo = new Label
            {
                Text = "B",
                Font = new Font("Segoe UI", 96, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextLight,
                Size = new Size(350, 140),
                Location = new Point(25, 140),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblAppName = new Label
            {
                Text = "БИБЛИОТЕКА\nБППК",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextLight,
                Size = new Size(350, 120),
                Location = new Point(25, 290),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblAppSubtitle = new Label
            {
                Text = "Система управления библиотекой",
                Font = new Font("Segoe UI", 11),
                ForeColor = ModernUIHelper.NeutralDark,
                Size = new Size(350, 60),
                Location = new Point(25, 420),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Ссылка на инструкцию
            linkInstruction = new LinkLabel
            {
                Text = "Инструкция пользователя",
                Font = new Font("Segoe UI", 10, FontStyle.Underline),
                Size = new Size(350, 30),
                Location = new Point(25, 520),
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = ModernUIHelper.NeutralDark,
                ActiveLinkColor = ModernUIHelper.TextLight,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            linkInstruction.LinkClicked += LinkInstruction_LinkClicked;

            panelRight.Controls.Add(lblLogo);
            panelRight.Controls.Add(lblAppName);
            panelRight.Controls.Add(lblAppSubtitle);
            panelRight.Controls.Add(linkInstruction);

            this.Controls.Add(panelLeft);
            this.Controls.Add(panelRight);

            // Enter для входа
            this.AcceptButton = btnLogin;
        }

        private void PanelRight_Paint(object sender, PaintEventArgs e)
        {
            // Рисуем градиент на правой панели (черно-белый)
            using (var brush = new LinearGradientBrush(
                panelRight.ClientRectangle,
                ModernUIHelper.SidebarGradientStart,
                ModernUIHelper.SidebarGradientEnd,
                45F))
            {
                e.Graphics.FillRectangle(brush, panelRight.ClientRectangle);
            }

            // Добавляем декоративные круги (полупрозрачные белые)
            using (var circleBrush = new SolidBrush(Color.FromArgb(15, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(circleBrush, -50, -50, 200, 200);
                e.Graphics.FillEllipse(circleBrush, 250, 450, 250, 250);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка",
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
                MessageBox.Show("Ошибка подключения к базе данных. Пожалуйста, попробуйте позже.", "Ошибка",
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
