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
        private Panel panelTop = null!;
        private Panel panelMain = null!;
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
            this.Size = new Size(1100, 700);
            this.Text = "Библиотека ГБПОУ БППК - Вход в систему";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.LightBackground;
            this.DoubleBuffered = true;

            // Верхняя декоративная панель (бордовая полоса с названием)
            panelTop = new Panel
            {
                Size = new Size(1100, 180),
                Location = new Point(0, 0),
                BackColor = ModernUIHelper.PrimaryAccent
            };
            panelTop.Paint += PanelTop_Paint;

            // Логотип библиотеки
            Label lblLogo = new Label
            {
                Text = "БИБЛИОТЕКА",
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(600, 60),
                Location = new Point(250, 35),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblCollege = new Label
            {
                Text = "ГБПОУ БППК",
                Font = new Font("Segoe UI", 18, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 220, 220),
                Size = new Size(600, 35),
                Location = new Point(250, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblBookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI", 48),
                Size = new Size(100, 80),
                Location = new Point(150, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblBookIcon2 = new Label
            {
                Text = "📖",
                Font = new Font("Segoe UI", 48),
                Size = new Size(100, 80),
                Location = new Point(850, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            panelTop.Controls.Add(lblLogo);
            panelTop.Controls.Add(lblCollege);
            panelTop.Controls.Add(lblBookIcon);
            panelTop.Controls.Add(lblBookIcon2);

            // Основная панель с формой входа
            panelMain = new Panel
            {
                Size = new Size(500, 420),
                Location = new Point(300, 220),
                BackColor = ModernUIHelper.CardBackground
            };
            panelMain.Paint += PanelMain_Paint;

            // Заголовок формы
            Label lblFormTitle = new Label
            {
                Text = "АВТОРИЗАЦИЯ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(420, 40),
                Location = new Point(40, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblFormSubtitle = new Label
            {
                Text = "Введите данные для входа в систему",
                Font = new Font("Segoe UI", 10),
                ForeColor = ModernUIHelper.TextMuted,
                Size = new Size(420, 25),
                Location = new Point(40, 70),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Разделитель
            Panel divider = ModernUIHelper.CreateDivider(new Point(40, 105), 420);

            // Логин
            Label lblLogin = new Label
            {
                Text = "Логин",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(420, 25),
                Location = new Point(40, 125),
                BackColor = Color.Transparent
            };

            Panel panelLoginBox = new Panel
            {
                Location = new Point(40, 150),
                Size = new Size(420, 45),
                BackColor = ModernUIHelper.SidebarBackground
            };
            panelLoginBox.Paint += (s, e) =>
            {
                using (var pen = new Pen(ModernUIHelper.SecondaryAccent, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, panelLoginBox.Width - 1, panelLoginBox.Height - 1);
            };

            txtLogin = new TextBox
            {
                Location = new Point(15, 10),
                Size = new Size(390, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelLoginBox.Controls.Add(txtLogin);

            // Пароль
            Label lblPassword = new Label
            {
                Text = "Пароль",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(420, 25),
                Location = new Point(40, 205),
                BackColor = Color.Transparent
            };

            Panel panelPasswordBox = new Panel
            {
                Location = new Point(40, 230),
                Size = new Size(420, 45),
                BackColor = ModernUIHelper.SidebarBackground
            };
            panelPasswordBox.Paint += (s, e) =>
            {
                using (var pen = new Pen(ModernUIHelper.SecondaryAccent, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, panelPasswordBox.Width - 1, panelPasswordBox.Height - 1);
            };

            txtPassword = new TextBox
            {
                Location = new Point(15, 10),
                Size = new Size(390, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = ModernUIHelper.SidebarBackground,
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
                Location = new Point(40, 280),
                ForeColor = ModernUIHelper.TextMuted,
                BackColor = Color.Transparent
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопка входа (по центру, большая)
            btnLogin = ModernUIHelper.CreateGradientButton(
                "ВОЙТИ",
                new Point(40, 315),
                new Size(200, 45),
                ModernUIHelper.PrimaryAccent,
                ModernUIHelper.PrimaryAccent
            );
            btnLogin.Click += BtnLogin_Click;

            // Кнопка регистрации (справа)
            btnRegister = ModernUIHelper.CreateGradientButton(
                "РЕГИСТРАЦИЯ",
                new Point(260, 315),
                new Size(200, 45),
                ModernUIHelper.SecondaryAccent,
                ModernUIHelper.SecondaryAccent
            );
            btnRegister.Click += BtnRegister_Click;

            // Ссылка на инструкцию
            linkInstruction = new LinkLabel
            {
                Text = "Инструкция пользователя",
                Font = new Font("Segoe UI", 9),
                Size = new Size(420, 25),
                Location = new Point(40, 375),
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = ModernUIHelper.SecondaryAccent,
                ActiveLinkColor = ModernUIHelper.PrimaryAccent,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            linkInstruction.LinkClicked += LinkInstruction_LinkClicked;

            // Добавление элементов на главную панель
            panelMain.Controls.Add(lblFormTitle);
            panelMain.Controls.Add(lblFormSubtitle);
            panelMain.Controls.Add(divider);
            panelMain.Controls.Add(lblLogin);
            panelMain.Controls.Add(panelLoginBox);
            panelMain.Controls.Add(lblPassword);
            panelMain.Controls.Add(panelPasswordBox);
            panelMain.Controls.Add(chkShowPassword);
            panelMain.Controls.Add(btnLogin);
            panelMain.Controls.Add(btnRegister);
            panelMain.Controls.Add(linkInstruction);

            // Тестовые данные - подсказка
            Label lblTestData = new Label
            {
                Text = "Тестовые данные: admin / admin123  или  student1 / pass123",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernUIHelper.TextMuted,
                Size = new Size(600, 25),
                Location = new Point(250, 650),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            this.Controls.Add(panelTop);
            this.Controls.Add(panelMain);
            this.Controls.Add(lblTestData);

            // Enter для входа
            this.AcceptButton = btnLogin;
        }

        private void PanelTop_Paint(object sender, PaintEventArgs e)
        {
            // Градиент на верхней панели
            using (var brush = new LinearGradientBrush(
                panelTop.ClientRectangle,
                ModernUIHelper.PrimaryAccent,
                ModernUIHelper.SecondaryAccent,
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panelTop.ClientRectangle);
            }

            // Декоративные элементы
            using (var brush = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(brush, -50, 100, 150, 150);
                e.Graphics.FillEllipse(brush, 1000, -50, 150, 150);
            }
        }

        private void PanelMain_Paint(object sender, PaintEventArgs e)
        {
            // Тень и рамка
            using (var pen = new Pen(ModernUIHelper.SidebarGradientEnd, 2))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panelMain.Width - 1, panelMain.Height - 1);
            }

            // Верхняя декоративная линия
            using (var brush = new SolidBrush(ModernUIHelper.AccentGold))
            {
                e.Graphics.FillRectangle(brush, 0, 0, panelMain.Width, 4);
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
