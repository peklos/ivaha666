using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class AdminPanel : Form
    {
        private User currentUser = null!;
        private Panel sidebarPanel = null!;
        private Panel contentPanel = null!;
        private Panel applicationsPanel = null!;
        private Panel specialtiesPanel = null!;
        private Panel usersPanel = null!;
        private FlowLayoutPanel appsCardsPanel = null!;
        private DataGridView dgvUsers = null!;
        private DataGridView dgvSpecialties = null!;
        private Button btnApplicationsNav = null!;
        private Button btnSpecialtiesNav = null!;
        private Button btnUsersNav = null!;
        private Label lblPageTitle = null!;
        private Panel selectedApplicationCard = null!;

        public AdminPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowApplicationsPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1500, 900);
            this.Text = "Панель администратора";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ModernUIHelper.LightBackground;
            this.DoubleBuffered = true;

            // Панель контента СЛЕВА
            contentPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1280, 900),
                BackColor = ModernUIHelper.LightBackground
            };

            // Заголовок страницы
            lblPageTitle = new Label
            {
                Text = "УПРАВЛЕНИЕ ЗАЯВКАМИ НА КНИГИ",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1200, 70),
                Location = new Point(40, 30),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Боковая панель навигации СПРАВА
            sidebarPanel = new Panel
            {
                Location = new Point(1280, 0),
                Size = new Size(220, 900),
                BackColor = ModernUIHelper.SidebarBackground
            };
            sidebarPanel.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    sidebarPanel.ClientRectangle,
                    ModernUIHelper.SidebarGradientStart,
                    ModernUIHelper.SidebarGradientEnd,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, sidebarPanel.ClientRectangle);
                }
            };

            // Логотип и приветствие
            Label lblLogo = new Label
            {
                Text = "ADM",
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextLight,
                Size = new Size(220, 72),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblWelcome = new Label
            {
                Text = "ПАНЕЛЬ\nАДМИНИСТРАТОРА",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextLight,
                Size = new Size(220, 66),
                Location = new Point(0, 95),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblUserName = new Label
            {
                Text = currentUser.FullName,
                Font = new Font("Segoe UI", 10),
                ForeColor = ModernUIHelper.NeutralDark,
                Size = new Size(200, 36),
                Location = new Point(10, 165),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Panel divider = new Panel
            {
                Location = new Point(10, 210),
                Size = new Size(200, 1),
                BackColor = ModernUIHelper.SecondaryAccent
            };

            // Кнопки навигации
            btnApplicationsNav = CreateSidebarButtonRight("Заявки на книги", 250, true);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            btnSpecialtiesNav = CreateSidebarButtonRight("Книги", 320, false);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnUsersNav = CreateSidebarButtonRight("Пользователи", 390, false);
            btnUsersNav.Click += (s, e) => ShowUsersPanel();

            // Кнопка выхода внизу
            Button btnLogout = new Button
            {
                Text = "  ВЫХОД",
                Location = new Point(0, 800),
                Size = new Size(220, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = ModernUIHelper.NeutralDark,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = ModernUIHelper.SecondaryAccent;
            btnLogout.Click += (s, e) =>
            {
                this.Hide();
                var loginForm = new LoginForm();
                loginForm.FormClosed += (s2, args) => this.Close();
                loginForm.Show();
            };

            sidebarPanel.Controls.Add(lblLogo);
            sidebarPanel.Controls.Add(lblWelcome);
            sidebarPanel.Controls.Add(lblUserName);
            sidebarPanel.Controls.Add(divider);
            sidebarPanel.Controls.Add(btnApplicationsNav);
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnUsersNav);
            sidebarPanel.Controls.Add(btnLogout);

            // Создаем панели для разных разделов
            CreateApplicationsPanel();
            CreateSpecialtiesPanel();
            CreateUsersPanel();

            this.Controls.Add(contentPanel);
            this.Controls.Add(sidebarPanel);
        }

        private Button CreateSidebarButtonRight(string text, int yPosition, bool isActive)
        {
            var button = new Button
            {
                Text = "  " + text,
                Location = new Point(0, yPosition),
                Size = new Size(220, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = isActive ? ModernUIHelper.TextPrimary : ModernUIHelper.TextLight,
                BackColor = isActive ? ModernUIHelper.LightBackground : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ModernUIHelper.SecondaryAccent;
            return button;
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1220, 750),
                BackColor = Color.Transparent,
                Visible = true
            };

            // Заголовок панели заявлений
            Label lblAppsTitle = new Label
            {
                Text = "ВСЕ ЗАЯВКИ НА КНИГИ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextSecondary,
                Location = new Point(0, 10),
                Size = new Size(1160, 40),
                BackColor = Color.Transparent
            };

            // Контейнер для карточек с прокруткой
            Panel scrollPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(1200, 500),
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // FlowLayoutPanel для автоматического расположения карточек
            appsCardsPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1180, 500),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            scrollPanel.Controls.Add(appsCardsPanel);

            // Панель с кнопками действий (изменен порядок)
            var buttonPanel = new TableLayoutPanel
            {
                Location = new Point(0, 580),
                Size = new Size(1200, 70),
                BackColor = Color.Transparent,
                ColumnCount = 5,
                RowCount = 1
            };
            for (int i = 0; i < 5; i++) buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            ComboBox cmbFilter = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            cmbFilter.Items.AddRange(new object[] { "Все", "На рассмотрении", "Одобрено", "Отклонено" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) => LoadApplicationsCards();

            Button btnRefresh = ModernUIHelper.CreateGradientButton("ОБНОВИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnRefresh.Click += (s, e) => LoadApplicationsCards();

            Button btnApprove = ModernUIHelper.CreateGradientButton("ОДОБРИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
            btnApprove.Click += (s, e) => ChangeApplicationStatus("Одобрено");

            Button btnReject = ModernUIHelper.CreateGradientButton("ОТКЛОНИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnReject.Click += (s, e) => ChangeApplicationStatus("Отклонено");

            Button btnDelete = ModernUIHelper.CreateGradientButton("УДАЛИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.NeutralDarker, ModernUIHelper.NeutralDark);
            btnDelete.Click += (s, e) => DeleteApplication();

            cmbFilter.Dock = DockStyle.Fill;
            btnRefresh.Dock = DockStyle.Fill;
            btnApprove.Dock = DockStyle.Fill;
            btnReject.Dock = DockStyle.Fill;
            btnDelete.Dock = DockStyle.Fill;

            buttonPanel.Controls.Add(cmbFilter, 0, 0);
            buttonPanel.Controls.Add(btnRefresh, 1, 0);
            buttonPanel.Controls.Add(btnApprove, 2, 0);
            buttonPanel.Controls.Add(btnReject, 3, 0);
            buttonPanel.Controls.Add(btnDelete, 4, 0);

            applicationsPanel.Controls.Add(lblAppsTitle);
            applicationsPanel.Controls.Add(scrollPanel);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1220, 750),
                BackColor = Color.Transparent,
                Visible = false
            };

            // DataGridView для категорий
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 10),
                Size = new Size(1220, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками (изменен порядок)
            var buttonPanel = new TableLayoutPanel
            {
                Location = new Point(0, 580),
                Size = new Size(1220, 70),
                BackColor = Color.Transparent,
                ColumnCount = 4,
                RowCount = 1
            };
            for (int i = 0; i < 4; i++) buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            Button btnRefresh = ModernUIHelper.CreateGradientButton("ОБНОВИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnRefresh.Click += (s, e) => LoadSpecialties();

            Button btnAdd = ModernUIHelper.CreateGradientButton("ДОБАВИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
            btnAdd.Click += (s, e) => AddSpecialty();

            Button btnEdit = ModernUIHelper.CreateGradientButton("ИЗМЕНИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnEdit.Click += (s, e) => EditSpecialty();

            Button btnDelete = ModernUIHelper.CreateGradientButton("УДАЛИТЬ", Point.Empty, new Size(1, 1), ModernUIHelper.NeutralDarker, ModernUIHelper.NeutralDark);
            btnDelete.Click += (s, e) => DeleteSpecialty();

            btnRefresh.Dock = DockStyle.Fill;
            btnAdd.Dock = DockStyle.Fill;
            btnEdit.Dock = DockStyle.Fill;
            btnDelete.Dock = DockStyle.Fill;

            buttonPanel.Controls.Add(btnRefresh, 0, 0);
            buttonPanel.Controls.Add(btnAdd, 1, 0);
            buttonPanel.Controls.Add(btnEdit, 2, 0);
            buttonPanel.Controls.Add(btnDelete, 3, 0);

            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateUsersPanel()
        {
            usersPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1220, 750),
                BackColor = Color.Transparent,
                Visible = false
            };

            // DataGridView для пользователей
            dgvUsers = new DataGridView
            {
                Location = new Point(0, 10),
                Size = new Size(1220, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvUsers);

            // Панель с кнопками (изменен порядок)
            var buttonPanel = new TableLayoutPanel
            {
                Location = new Point(0, 580),
                Size = new Size(1220, 70),
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            Button btnRefresh = ModernUIHelper.CreateGradientButton("ОБНОВИТЬ СПИСОК", Point.Empty, new Size(1, 1), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnRefresh.Click += (s, e) => LoadUsers();

            Button btnDelete = ModernUIHelper.CreateGradientButton("УДАЛИТЬ ПОЛЬЗОВАТЕЛЯ", Point.Empty, new Size(1, 1), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
            btnDelete.Click += (s, e) => DeleteUser();

            btnRefresh.Dock = DockStyle.Fill;
            btnDelete.Dock = DockStyle.Fill;

            buttonPanel.Controls.Add(btnRefresh, 0, 0);
            buttonPanel.Controls.Add(btnDelete, 1, 0);

            usersPanel.Controls.Add(dgvUsers);
            usersPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(usersPanel);
        }

        private void ShowApplicationsPanel()
        {
            applicationsPanel.Visible = true;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = false;

            btnApplicationsNav.BackColor = ModernUIHelper.LightBackground;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextPrimary;
            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextLight;
            btnUsersNav.BackColor = Color.Transparent;
            btnUsersNav.ForeColor = ModernUIHelper.TextLight;

            lblPageTitle.Text = "УПРАВЛЕНИЕ ЗАЯВКАМИ НА КНИГИ";

            LoadApplicationsCards();
        }

        private void ShowSpecialtiesPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = true;
            usersPanel.Visible = false;

            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextLight;
            btnSpecialtiesNav.BackColor = ModernUIHelper.LightBackground;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextPrimary;
            btnUsersNav.BackColor = Color.Transparent;
            btnUsersNav.ForeColor = ModernUIHelper.TextLight;

            lblPageTitle.Text = "УПРАВЛЕНИЕ КАТЕГОРИЯМИ КНИГ";
        }

        private void ShowUsersPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = true;

            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextLight;
            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextLight;
            btnUsersNav.BackColor = ModernUIHelper.LightBackground;
            btnUsersNav.ForeColor = ModernUIHelper.TextPrimary;

            lblPageTitle.Text = "УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ";
        }

        private void LoadData()
        {
            try
            {
                LoadSpecialties();
                LoadUsers();
            }
            catch (Exception)
            {
            }
        }

        private void LoadApplicationsCards()
        {
            appsCardsPanel.Controls.Clear();
            selectedApplicationCard = null;

            try
            {
                List<BookRequest> bookRequests = DatabaseHelper.GetAllBookRequests();

                if (bookRequests.Count == 0)
                {
                    Label lblNoApps = new Label
                    {
                        Text = "Заявлений пока нет.",
                        Font = new Font("Segoe UI", 14),
                        ForeColor = ModernUIHelper.TextMuted,
                        Size = new Size(1100, 100),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };
                    appsCardsPanel.Controls.Add(lblNoApps);
                    return;
                }

                foreach (var app in bookRequests)
                {
                    var currentApp = app;

                    Panel card = ModernUIHelper.CreateApplicationCard(currentApp, (s, e) =>
                    {
                        var clickedCard = (Panel)s;

                        if (selectedApplicationCard != null && selectedApplicationCard != clickedCard)
                        {
                            selectedApplicationCard.BackColor = ModernUIHelper.LightBackground;
                            selectedApplicationCard.Refresh();
                        }

                        clickedCard.BackColor = ModernUIHelper.CardHoverBackground;
                        clickedCard.Refresh();
                        selectedApplicationCard = clickedCard;

                        ApplicationDetailsForm detailsForm = new ApplicationDetailsForm(currentApp, true);
                        detailsForm.ShowDialog();

                        if (detailsForm.DialogResult == DialogResult.OK)
                        {
                            LoadApplicationsCards();
                        }
                    });

                    appsCardsPanel.Controls.Add(card);
                }
            }
            catch (Exception)
            {
                Label lblError = new Label
                {
                    Text = "Не удалось загрузить заявления",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = ModernUIHelper.TextMuted,
                    Size = new Size(1100, 100),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                appsCardsPanel.Controls.Add(lblError);
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
                    {
                        var idColumn = dgvSpecialties.Columns["Id"];
                        if (idColumn != null)
                        {
                            idColumn.HeaderText = "ID";
                        }
                    }

                    if (dgvSpecialties.Columns.Contains("Name"))
                    {
                        var nameColumn = dgvSpecialties.Columns["Name"];
                        if (nameColumn != null)
                            nameColumn.HeaderText = "Название";
                    }

                    if (dgvSpecialties.Columns.Contains("Code"))
                    {
                        var codeColumn = dgvSpecialties.Columns["Code"];
                        if (codeColumn != null)
                            codeColumn.HeaderText = "Код";
                    }

                    if (dgvSpecialties.Columns.Contains("PlacesCount"))
                    {
                        var placesColumn = dgvSpecialties.Columns["PlacesCount"];
                        if (placesColumn != null)
                            placesColumn.HeaderText = "Мест";
                    }

                    if (dgvSpecialties.Columns.Contains("MinScore"))
                    {
                        var scoreColumn = dgvSpecialties.Columns["MinScore"];
                        if (scoreColumn != null)
                            scoreColumn.HeaderText = "Мин. балл";
                    }

                    if (dgvSpecialties.Columns.Contains("Description"))
                    {
                        var descColumn = dgvSpecialties.Columns["Description"];
                        if (descColumn != null)
                            descColumn.HeaderText = "Описание";
                    }
                }
            }
            catch (Exception)
            {
                if (dgvSpecialties != null)
                {
                    dgvSpecialties.DataSource = null;
                }
            }
        }

        private void LoadUsers()
        {
            try
            {
                List<User> users = DatabaseHelper.GetAllUsers();

                if (dgvUsers == null) return;

                dgvUsers.DataSource = null;
                dgvUsers.DataSource = users;

                if (dgvUsers.Columns.Count > 0)
                {
                    if (dgvUsers.Columns.Contains("Id"))
                    {
                        var idColumn = dgvUsers.Columns["Id"];
                        if (idColumn != null)
                        {
                            idColumn.HeaderText = "ID";
                        }
                    }

                    if (dgvUsers.Columns.Contains("Login"))
                    {
                        var loginColumn = dgvUsers.Columns["Login"];
                        if (loginColumn != null)
                            loginColumn.HeaderText = "Логин";
                    }

                    if (dgvUsers.Columns.Contains("Password"))
                    {
                        var passColumn = dgvUsers.Columns["Password"];
                        if (passColumn != null)
                            passColumn.Visible = false;
                    }

                    if (dgvUsers.Columns.Contains("FullName"))
                    {
                        var nameColumn = dgvUsers.Columns["FullName"];
                        if (nameColumn != null)
                            nameColumn.HeaderText = "ФИО";
                    }

                    if (dgvUsers.Columns.Contains("Role"))
                    {
                        var roleColumn = dgvUsers.Columns["Role"];
                        if (roleColumn != null)
                            roleColumn.HeaderText = "Роль";
                    }
                }
            }
            catch (Exception)
            {
                if (dgvUsers != null)
                {
                    dgvUsers.DataSource = null;
                }
            }
        }

        private void ChangeApplicationStatus(string status)
        {
            if (selectedApplicationCard == null)
            {
                MessageBox.Show("Выберите заявление (кликните на карточку)!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (BookRequest)selectedApplicationCard.Tag;

            if (MessageBox.Show($"Изменить статус заявления на '{status}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateBookRequestStatus(application.Id, status);
                    LoadApplicationsCards();
                    MessageBox.Show("Статус успешно изменен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось изменить статус заявления", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteApplication()
        {
            if (selectedApplicationCard == null)
            {
                MessageBox.Show("Выберите заявление для удаления (кликните на карточку)!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (BookRequest)selectedApplicationCard.Tag;

            if (MessageBox.Show("Удалить выбранное заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteBookRequest(application.Id);
                    LoadApplicationsCards();
                    MessageBox.Show("Заявление успешно удалено!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось удалить заявление", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddSpecialty()
        {
            try
            {
                SpecialtyEditForm form = new SpecialtyEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSpecialties();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму добавления категории", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для редактирования!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var specialty = (BookCategory)dgvSpecialties.SelectedRows[0].DataBoundItem;
                SpecialtyEditForm form = new SpecialtyEditForm(specialty);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSpecialties();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть форму редактирования категории", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var specialty = (BookCategory)dgvSpecialties.SelectedRows[0].DataBoundItem;

                if (MessageBox.Show($"Удалить категорию '{specialty.Name}'?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseHelper.DeleteBookCategory(specialty.Id);
                    LoadSpecialties();
                    MessageBox.Show("Книга успешно удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось удалить категорию", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = (User)dgvUsers.SelectedRows[0].DataBoundItem;

                if (user.Role == "Admin")
                {
                    MessageBox.Show("Невозможно удалить администратора!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"Удалить пользователя '{user.FullName}'?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseHelper.DeleteUser(user.Id);
                    LoadUsers();
                    MessageBox.Show("Пользователь успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось удалить пользователя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
