using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class UserPanel : Form
    {
        private User currentUser = null!;
        private Panel sidebarPanel = null!;
        private Panel contentPanel = null!;
        private Panel headerPanel = null!;
        private Panel specialtiesPanel = null!;
        private Panel applicationsPanel = null!;
        private FlowLayoutPanel cardsFlowPanel = null!;
        private DataGridView dgvSpecialties = null!;
        private Button btnSpecialtiesNav = null!;
        private Button btnApplicationsNav = null!;
        private Label lblPageTitle = null!;
        private Button btnRefreshCards = null!;

        public UserPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowSpecialtiesPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1400, 850);
            this.Text = "КНИЖНЫЙ ФОНД — Личный кабинет";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.DoubleBuffered = true;

            // Боковая панель (черная, узкая)
            sidebarPanel = new Panel
            {
                Size = new Size(240, 850),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(18, 18, 18),
                Dock = DockStyle.Left
            };

            // Логотип
            Label lblLogo = new Label
            {
                Text = "КНИЖНЫЙ\nФОНД",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(220, 70),
                Location = new Point(10, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Информация о пользователе
            Panel userInfoPanel = new Panel
            {
                Size = new Size(220, 80),
                Location = new Point(10, 110),
                BackColor = Color.FromArgb(30, 30, 30)
            };

            Label lblUserIcon = new Label
            {
                Text = "[ ]",
                Font = new Font("Consolas", 20),
                ForeColor = Color.White,
                Size = new Size(50, 50),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblUserName = new Label
            {
                Text = currentUser.FullName.Length > 18
                    ? currentUser.FullName.Substring(0, 15) + "..."
                    : currentUser.FullName,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(150, 25),
                Location = new Point(65, 18),
                BackColor = Color.Transparent
            };

            Label lblStudentId = new Label
            {
                Text = $"ID: {currentUser.StudentNumber ?? "—"}",
                Font = new Font("Consolas", 9),
                ForeColor = Color.FromArgb(150, 150, 150),
                Size = new Size(150, 20),
                Location = new Point(65, 43),
                BackColor = Color.Transparent
            };

            userInfoPanel.Controls.Add(lblUserIcon);
            userInfoPanel.Controls.Add(lblUserName);
            userInfoPanel.Controls.Add(lblStudentId);

            // Разделитель
            Panel divider1 = new Panel
            {
                Size = new Size(200, 1),
                Location = new Point(20, 210),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            // Меню навигации
            Label lblMenu = new Label
            {
                Text = "МЕНЮ",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100),
                Size = new Size(200, 20),
                Location = new Point(20, 230),
                BackColor = Color.Transparent
            };

            btnSpecialtiesNav = CreateNavButton("КАТАЛОГ", 260, true);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnApplicationsNav = CreateNavButton("МОИ ЗАЯВКИ", 315, false);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            // Кнопка выхода
            Button btnLogout = new Button
            {
                Text = "[ ВЫХОД ]",
                Location = new Point(0, 760),
                Size = new Size(240, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 11),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
            btnLogout.Click += (s, e) =>
            {
                this.Hide();
                var loginForm = new LoginForm();
                loginForm.FormClosed += (s2, args) => this.Close();
                loginForm.Show();
            };

            sidebarPanel.Controls.Add(lblLogo);
            sidebarPanel.Controls.Add(userInfoPanel);
            sidebarPanel.Controls.Add(divider1);
            sidebarPanel.Controls.Add(lblMenu);
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnApplicationsNav);
            sidebarPanel.Controls.Add(btnLogout);

            // Панель контента
            contentPanel = new Panel
            {
                Location = new Point(240, 0),
                Size = new Size(1160, 850),
                BackColor = Color.White
            };

            // Заголовок страницы (верхняя панель)
            headerPanel = new Panel
            {
                Size = new Size(1160, 80),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            headerPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 230), 1))
                    e.Graphics.DrawLine(pen, 0, 79, 1160, 79);
            };

            lblPageTitle = new Label
            {
                Text = "КАТАЛОГ КНИГ",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(800, 40),
                Location = new Point(40, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblPageTitle);

            contentPanel.Controls.Add(headerPanel);

            // Создаем панели для разных разделов
            CreateSpecialtiesPanel();
            CreateApplicationsPanel();

            this.Controls.Add(sidebarPanel);
            this.Controls.Add(contentPanel);
        }

        private Button CreateNavButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = isActive ? $"> {text}" : $"  {text}",
                Location = new Point(0, y),
                Size = new Size(240, 45),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, isActive ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = isActive ? Color.White : Color.FromArgb(180, 180, 180),
                BackColor = isActive ? Color.FromArgb(45, 45, 45) : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Padding = new Padding(15, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 35, 35);
            return btn;
        }

        private void UpdateNavButtons(bool catalogActive)
        {
            btnSpecialtiesNav.Text = catalogActive ? "> КАТАЛОГ" : "  КАТАЛОГ";
            btnSpecialtiesNav.Font = new Font("Segoe UI", 11, catalogActive ? FontStyle.Bold : FontStyle.Regular);
            btnSpecialtiesNav.ForeColor = catalogActive ? Color.White : Color.FromArgb(180, 180, 180);
            btnSpecialtiesNav.BackColor = catalogActive ? Color.FromArgb(45, 45, 45) : Color.Transparent;

            btnApplicationsNav.Text = !catalogActive ? "> МОИ ЗАЯВКИ" : "  МОИ ЗАЯВКИ";
            btnApplicationsNav.Font = new Font("Segoe UI", 11, !catalogActive ? FontStyle.Bold : FontStyle.Regular);
            btnApplicationsNav.ForeColor = !catalogActive ? Color.White : Color.FromArgb(180, 180, 180);
            btnApplicationsNav.BackColor = !catalogActive ? Color.FromArgb(45, 45, 45) : Color.Transparent;
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(40, 100),
                Size = new Size(1080, 720),
                BackColor = Color.White,
                Visible = true
            };

            // Подзаголовок
            Label lblSubtitle = new Label
            {
                Text = "Выберите категорию для подачи заявки на книгу",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(120, 120, 120),
                Size = new Size(600, 25),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            // DataGridView
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 40),
                Size = new Size(1080, 560),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками (горизонтально)
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 620),
                Size = new Size(1080, 60),
                BackColor = Color.Transparent
            };

            Button btnSubmit = new Button
            {
                Text = "ПОДАТЬ ЗАЯВКУ",
                Location = new Point(0, 0),
                Size = new Size(200, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += (s, e) => SubmitApplication();
            btnSubmit.MouseEnter += (s, e) => btnSubmit.BackColor = Color.FromArgb(40, 40, 40);
            btnSubmit.MouseLeave += (s, e) => btnSubmit.BackColor = Color.Black;

            Button btnRefresh = new Button
            {
                Text = "ОБНОВИТЬ",
                Location = new Point(220, 0),
                Size = new Size(150, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 2;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.Click += (s, e) => LoadSpecialties();

            buttonPanel.Controls.Add(btnSubmit);
            buttonPanel.Controls.Add(btnRefresh);

            specialtiesPanel.Controls.Add(lblSubtitle);
            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(40, 100),
                Size = new Size(1080, 720),
                BackColor = Color.White,
                Visible = false
            };

            // Подзаголовок
            Label lblSubtitle = new Label
            {
                Text = "Ваши поданные заявки на книги",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(120, 120, 120),
                Size = new Size(600, 25),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            // Контейнер для карточек
            Panel scrollPanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(1080, 560),
                BackColor = Color.FromArgb(250, 250, 250),
                AutoScroll = true
            };

            cardsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1060, 560),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(5)
            };

            scrollPanel.Controls.Add(cardsFlowPanel);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 620),
                Size = new Size(1080, 60),
                BackColor = Color.Transparent
            };

            Button btnDelete = new Button
            {
                Text = "УДАЛИТЬ ЗАЯВКУ",
                Location = new Point(0, 0),
                Size = new Size(180, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 50),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteApplication();

            btnRefreshCards = new Button
            {
                Text = "ОБНОВИТЬ",
                Location = new Point(200, 0),
                Size = new Size(150, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRefreshCards.FlatAppearance.BorderSize = 2;
            btnRefreshCards.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefreshCards.Click += (s, e) => LoadApplicationsCards();

            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefreshCards);

            applicationsPanel.Controls.Add(lblSubtitle);
            applicationsPanel.Controls.Add(scrollPanel);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void ShowSpecialtiesPanel()
        {
            specialtiesPanel.Visible = true;
            applicationsPanel.Visible = false;
            UpdateNavButtons(true);
            lblPageTitle.Text = "КАТАЛОГ КНИГ";
        }

        private void ShowApplicationsPanel()
        {
            specialtiesPanel.Visible = false;
            applicationsPanel.Visible = true;
            UpdateNavButtons(false);
            lblPageTitle.Text = "МОИ ЗАЯВКИ";
            LoadApplicationsCards();
        }

        private void LoadData()
        {
            try
            {
                LoadSpecialties();
            }
            catch (Exception)
            {
                // Silent
            }
        }

        private void LoadSpecialties()
        {
            try
            {
                List<BookCategory> specialties = DatabaseHelper.GetAllBookCategories();

                if (dgvSpecialties == null) return;

                dgvSpecialties.DataSource = null;
                dgvSpecialties.DataSource = specialties;

                if (dgvSpecialties.Columns.Count > 0)
                {
                    if (dgvSpecialties.Columns.Contains("Id"))
                        dgvSpecialties.Columns["Id"]!.HeaderText = "ID";
                    if (dgvSpecialties.Columns.Contains("Name"))
                        dgvSpecialties.Columns["Name"]!.HeaderText = "Название";
                    if (dgvSpecialties.Columns.Contains("Code"))
                        dgvSpecialties.Columns["Code"]!.HeaderText = "Код";
                    if (dgvSpecialties.Columns.Contains("PlacesCount"))
                        dgvSpecialties.Columns["PlacesCount"]!.HeaderText = "Кол-во";
                    if (dgvSpecialties.Columns.Contains("MinScore"))
                        dgvSpecialties.Columns["MinScore"]!.HeaderText = "Мин. балл";
                    if (dgvSpecialties.Columns.Contains("Description"))
                        dgvSpecialties.Columns["Description"]!.HeaderText = "Описание";
                }
            }
            catch (Exception)
            {
                if (dgvSpecialties != null)
                    dgvSpecialties.DataSource = null;
            }
        }

        private void LoadApplicationsCards()
        {
            cardsFlowPanel.Controls.Clear();

            try
            {
                List<BookRequest> applications = DatabaseHelper.GetUserBookRequests(currentUser.Id);

                if (applications.Count == 0)
                {
                    Label lblNoApps = new Label
                    {
                        Text = "Заявок пока нет.\nПерейдите в раздел «Каталог» для подачи заявки.",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = Color.FromArgb(120, 120, 120),
                        Size = new Size(1000, 80),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };
                    cardsFlowPanel.Controls.Add(lblNoApps);
                    return;
                }

                foreach (var app in applications)
                {
                    var card = ModernUIHelper.CreateApplicationCard(app, (s, e) =>
                    {
                        ApplicationDetailsForm detailsForm = new ApplicationDetailsForm(app, false);
                        detailsForm.ShowDialog();

                        if (detailsForm.DialogResult == DialogResult.OK)
                            LoadApplicationsCards();
                    });

                    cardsFlowPanel.Controls.Add(card);
                }
            }
            catch (Exception)
            {
                Label lblError = new Label
                {
                    Text = "Ошибка загрузки заявок",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.FromArgb(120, 120, 120),
                    Size = new Size(1000, 80),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                cardsFlowPanel.Controls.Add(lblError);
            }
        }

        private void SubmitApplication()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var specialty = (BookCategory)dgvSpecialties.SelectedRows[0].DataBoundItem;
                BookRequestForm form = new BookRequestForm(currentUser, specialty);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadApplicationsCards();
                    ShowApplicationsPanel();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка открытия формы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteApplication()
        {
            Panel? selectedCard = null;
            foreach (Control control in cardsFlowPanel.Controls)
            {
                if (control is Panel panel && panel.BackColor == ModernUIHelper.CardHoverBackground)
                {
                    selectedCard = panel;
                    break;
                }
            }

            if (selectedCard == null)
            {
                MessageBox.Show("Выберите заявку (кликните на карточку)!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (BookRequest)selectedCard.Tag;

            if (MessageBox.Show("Удалить заявку?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteBookRequest(application.Id);
                    LoadApplicationsCards();
                    MessageBox.Show("Заявка удалена!", "Готово",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка удаления", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
