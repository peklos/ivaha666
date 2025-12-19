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
    public partial class AdminPanel : Form
    {
        private User currentUser = null!;
        private Panel sidebarPanel = null!;
        private Panel contentPanel = null!;
        private Panel headerPanel = null!;
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
        private Panel? selectedApplicationCard = null;

        public AdminPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowApplicationsPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1450, 880);
            this.Text = "КНИЖНЫЙ ФОНД — Панель администратора";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.DoubleBuffered = true;

            // Боковая панель (черная)
            sidebarPanel = new Panel
            {
                Size = new Size(260, 880),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(15, 15, 15),
                Dock = DockStyle.Left
            };

            // Логотип
            Label lblLogo = new Label
            {
                Text = "КНИЖНЫЙ\nФОНД",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(240, 75),
                Location = new Point(10, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            Label lblRole = new Label
            {
                Text = "[АДМИНИСТРАТОР]",
                Font = new Font("Consolas", 10),
                ForeColor = Color.FromArgb(180, 180, 180),
                Size = new Size(240, 20),
                Location = new Point(10, 100),
                BackColor = Color.Transparent
            };

            // Информация о пользователе
            Panel userInfoPanel = new Panel
            {
                Size = new Size(240, 70),
                Location = new Point(10, 135),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            Label lblAdminIcon = new Label
            {
                Text = "[*]",
                Font = new Font("Consolas", 18),
                ForeColor = Color.White,
                Size = new Size(45, 45),
                Location = new Point(10, 12),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblAdminName = new Label
            {
                Text = currentUser.FullName.Length > 16
                    ? currentUser.FullName.Substring(0, 13) + "..."
                    : currentUser.FullName,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(175, 25),
                Location = new Point(60, 15),
                BackColor = Color.Transparent
            };

            Label lblAdminRole = new Label
            {
                Text = "Библиотекарь",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(130, 130, 130),
                Size = new Size(175, 20),
                Location = new Point(60, 38),
                BackColor = Color.Transparent
            };

            userInfoPanel.Controls.Add(lblAdminIcon);
            userInfoPanel.Controls.Add(lblAdminName);
            userInfoPanel.Controls.Add(lblAdminRole);

            // Разделитель
            Panel divider1 = new Panel
            {
                Size = new Size(220, 1),
                Location = new Point(20, 225),
                BackColor = Color.FromArgb(45, 45, 45)
            };

            // Меню
            Label lblMenu = new Label
            {
                Text = "УПРАВЛЕНИЕ",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(90, 90, 90),
                Size = new Size(200, 20),
                Location = new Point(20, 245),
                BackColor = Color.Transparent
            };

            btnApplicationsNav = CreateNavButton("ЗАЯВКИ", 275, true);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            btnSpecialtiesNav = CreateNavButton("КАТЕГОРИИ", 330, false);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnUsersNav = CreateNavButton("ПОЛЬЗОВАТЕЛИ", 385, false);
            btnUsersNav.Click += (s, e) => ShowUsersPanel();

            // Кнопка выхода
            Button btnLogout = new Button
            {
                Text = "[ ВЫХОД ]",
                Location = new Point(0, 790),
                Size = new Size(260, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 11),
                ForeColor = Color.FromArgb(130, 130, 130),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 35, 35);
            btnLogout.Click += (s, e) =>
            {
                this.Hide();
                var loginForm = new LoginForm();
                loginForm.FormClosed += (s2, args) => this.Close();
                loginForm.Show();
            };

            sidebarPanel.Controls.Add(lblLogo);
            sidebarPanel.Controls.Add(lblRole);
            sidebarPanel.Controls.Add(userInfoPanel);
            sidebarPanel.Controls.Add(divider1);
            sidebarPanel.Controls.Add(lblMenu);
            sidebarPanel.Controls.Add(btnApplicationsNav);
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnUsersNav);
            sidebarPanel.Controls.Add(btnLogout);

            // Панель контента
            contentPanel = new Panel
            {
                Location = new Point(260, 0),
                Size = new Size(1190, 880),
                BackColor = Color.White
            };

            // Заголовок
            headerPanel = new Panel
            {
                Size = new Size(1190, 85),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            headerPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(225, 225, 225), 1))
                    e.Graphics.DrawLine(pen, 0, 84, 1190, 84);
            };

            lblPageTitle = new Label
            {
                Text = "УПРАВЛЕНИЕ ЗАЯВКАМИ",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(800, 45),
                Location = new Point(35, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblPageTitle);

            contentPanel.Controls.Add(headerPanel);

            // Панели разделов
            CreateApplicationsPanel();
            CreateSpecialtiesPanel();
            CreateUsersPanel();

            this.Controls.Add(sidebarPanel);
            this.Controls.Add(contentPanel);
        }

        private Button CreateNavButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = isActive ? $"> {text}" : $"  {text}",
                Location = new Point(0, y),
                Size = new Size(260, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, isActive ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = isActive ? Color.White : Color.FromArgb(160, 160, 160),
                BackColor = isActive ? Color.FromArgb(40, 40, 40) : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Padding = new Padding(20, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            return btn;
        }

        private void UpdateNavButtons(int activeIndex)
        {
            Button[] buttons = { btnApplicationsNav, btnSpecialtiesNav, btnUsersNav };
            string[] names = { "ЗАЯВКИ", "КАТЕГОРИИ", "ПОЛЬЗОВАТЕЛИ" };

            for (int i = 0; i < buttons.Length; i++)
            {
                bool isActive = i == activeIndex;
                buttons[i].Text = isActive ? $"> {names[i]}" : $"  {names[i]}";
                buttons[i].Font = new Font("Segoe UI", 11, isActive ? FontStyle.Bold : FontStyle.Regular);
                buttons[i].ForeColor = isActive ? Color.White : Color.FromArgb(160, 160, 160);
                buttons[i].BackColor = isActive ? Color.FromArgb(40, 40, 40) : Color.Transparent;
            }
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(35, 105),
                Size = new Size(1120, 750),
                BackColor = Color.White,
                Visible = true
            };

            // Описание
            Label lblDesc = new Label
            {
                Text = "Просмотр и управление заявками на книги",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(110, 110, 110),
                Size = new Size(600, 25),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            // Контейнер для карточек
            Panel scrollPanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(1120, 520),
                BackColor = Color.FromArgb(248, 248, 248),
                AutoScroll = true
            };

            appsCardsPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1100, 520),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(8)
            };

            scrollPanel.Controls.Add(appsCardsPanel);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 580),
                Size = new Size(1120, 60),
                BackColor = Color.Transparent
            };

            Button btnApprove = CreateActionButton("ОДОБРИТЬ", 0, Color.FromArgb(30, 30, 30));
            btnApprove.Click += (s, e) => ChangeApplicationStatus("Одобрено");

            Button btnReject = CreateActionButton("ОТКЛОНИТЬ", 160, Color.FromArgb(60, 60, 60));
            btnReject.Click += (s, e) => ChangeApplicationStatus("Отклонено");

            Button btnDelete = CreateActionButton("УДАЛИТЬ", 320, Color.FromArgb(90, 90, 90));
            btnDelete.Click += (s, e) => DeleteApplication();

            Button btnRefresh = CreateOutlineButton("ОБНОВИТЬ", 480);
            btnRefresh.Click += (s, e) => LoadApplicationsCards();

            ComboBox cmbFilter = new ComboBox
            {
                Location = new Point(640, 5),
                Size = new Size(180, 45),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            cmbFilter.Items.AddRange(new object[] { "Все заявки", "На рассмотрении", "Одобрено", "Отклонено" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) => LoadApplicationsCards();

            buttonPanel.Controls.Add(btnApprove);
            buttonPanel.Controls.Add(btnReject);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Controls.Add(cmbFilter);

            applicationsPanel.Controls.Add(lblDesc);
            applicationsPanel.Controls.Add(scrollPanel);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private Button CreateActionButton(string text, int x, Color bgColor)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 5),
                Size = new Size(150, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = bgColor,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(
                Math.Min(255, bgColor.R + 20),
                Math.Min(255, bgColor.G + 20),
                Math.Min(255, bgColor.B + 20));
            btn.MouseLeave += (s, e) => btn.BackColor = bgColor;
            return btn;
        }

        private Button CreateOutlineButton(string text, int x)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 5),
                Size = new Size(140, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            return btn;
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(35, 105),
                Size = new Size(1120, 750),
                BackColor = Color.White,
                Visible = false
            };

            Label lblDesc = new Label
            {
                Text = "Управление категориями книг",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(110, 110, 110),
                Size = new Size(600, 25),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 40),
                Size = new Size(1120, 520),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 580),
                Size = new Size(1120, 60),
                BackColor = Color.Transparent
            };

            Button btnAdd = CreateActionButton("ДОБАВИТЬ", 0, Color.FromArgb(30, 30, 30));
            btnAdd.Click += (s, e) => AddSpecialty();

            Button btnEdit = CreateActionButton("ИЗМЕНИТЬ", 160, Color.FromArgb(60, 60, 60));
            btnEdit.Click += (s, e) => EditSpecialty();

            Button btnDelete = CreateActionButton("УДАЛИТЬ", 320, Color.FromArgb(90, 90, 90));
            btnDelete.Click += (s, e) => DeleteSpecialty();

            Button btnRefresh = CreateOutlineButton("ОБНОВИТЬ", 480);
            btnRefresh.Click += (s, e) => LoadSpecialties();

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            specialtiesPanel.Controls.Add(lblDesc);
            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateUsersPanel()
        {
            usersPanel = new Panel
            {
                Location = new Point(35, 105),
                Size = new Size(1120, 750),
                BackColor = Color.White,
                Visible = false
            };

            Label lblDesc = new Label
            {
                Text = "Управление пользователями системы",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(110, 110, 110),
                Size = new Size(600, 25),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            dgvUsers = new DataGridView
            {
                Location = new Point(0, 40),
                Size = new Size(1120, 520),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvUsers);

            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 580),
                Size = new Size(1120, 60),
                BackColor = Color.Transparent
            };

            Button btnDelete = CreateActionButton("УДАЛИТЬ", 0, Color.FromArgb(60, 60, 60));
            btnDelete.Click += (s, e) => DeleteUser();

            Button btnRefresh = CreateOutlineButton("ОБНОВИТЬ", 160);
            btnRefresh.Click += (s, e) => LoadUsers();

            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            usersPanel.Controls.Add(lblDesc);
            usersPanel.Controls.Add(dgvUsers);
            usersPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(usersPanel);
        }

        private void ShowApplicationsPanel()
        {
            applicationsPanel.Visible = true;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = false;
            UpdateNavButtons(0);
            lblPageTitle.Text = "УПРАВЛЕНИЕ ЗАЯВКАМИ";
            LoadApplicationsCards();
        }

        private void ShowSpecialtiesPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = true;
            usersPanel.Visible = false;
            UpdateNavButtons(1);
            lblPageTitle.Text = "КАТЕГОРИИ КНИГ";
        }

        private void ShowUsersPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = true;
            UpdateNavButtons(2);
            lblPageTitle.Text = "ПОЛЬЗОВАТЕЛИ";
        }

        private void LoadData()
        {
            try
            {
                LoadSpecialties();
                LoadUsers();
            }
            catch (Exception) { }
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
                        Text = "Заявок пока нет",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = Color.FromArgb(120, 120, 120),
                        Size = new Size(1000, 80),
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
                        var clickedCard = s as Panel;
                        if (clickedCard == null) return;

                        if (selectedApplicationCard != null && selectedApplicationCard != clickedCard)
                        {
                            selectedApplicationCard.BackColor = ModernUIHelper.CardBackground;
                            selectedApplicationCard.Refresh();
                        }

                        clickedCard.BackColor = ModernUIHelper.CardHoverBackground;
                        clickedCard.Refresh();
                        selectedApplicationCard = clickedCard;

                        ApplicationDetailsForm detailsForm = new ApplicationDetailsForm(currentApp, true);
                        detailsForm.ShowDialog();

                        if (detailsForm.DialogResult == DialogResult.OK)
                            LoadApplicationsCards();
                    });

                    appsCardsPanel.Controls.Add(card);
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
                        dgvUsers.Columns["Id"]!.HeaderText = "ID";
                    if (dgvUsers.Columns.Contains("Login"))
                        dgvUsers.Columns["Login"]!.HeaderText = "Логин";
                    if (dgvUsers.Columns.Contains("Password"))
                        dgvUsers.Columns["Password"]!.Visible = false;
                    if (dgvUsers.Columns.Contains("FullName"))
                        dgvUsers.Columns["FullName"]!.HeaderText = "ФИО";
                    if (dgvUsers.Columns.Contains("StudentNumber"))
                        dgvUsers.Columns["StudentNumber"]!.HeaderText = "Студ. билет";
                    if (dgvUsers.Columns.Contains("Address"))
                        dgvUsers.Columns["Address"]!.HeaderText = "Адрес";
                    if (dgvUsers.Columns.Contains("PhoneNumber"))
                        dgvUsers.Columns["PhoneNumber"]!.HeaderText = "Телефон";
                    if (dgvUsers.Columns.Contains("Email"))
                        dgvUsers.Columns["Email"]!.HeaderText = "Email";
                    if (dgvUsers.Columns.Contains("Role"))
                        dgvUsers.Columns["Role"]!.HeaderText = "Роль";
                    if (dgvUsers.Columns.Contains("RegistrationDate"))
                        dgvUsers.Columns["RegistrationDate"]!.Visible = false;
                }
            }
            catch (Exception)
            {
                if (dgvUsers != null)
                    dgvUsers.DataSource = null;
            }
        }

        private void ChangeApplicationStatus(string status)
        {
            if (selectedApplicationCard == null)
            {
                MessageBox.Show("Выберите заявку!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (BookRequest)selectedApplicationCard.Tag;

            if (MessageBox.Show($"Изменить статус на «{status}»?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.UpdateBookRequestStatus(application.Id, status);
                    LoadApplicationsCards();
                    MessageBox.Show("Статус изменен!", "Готово",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка изменения статуса", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteApplication()
        {
            if (selectedApplicationCard == null)
            {
                MessageBox.Show("Выберите заявку!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (BookRequest)selectedApplicationCard.Tag;

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

        private void AddSpecialty()
        {
            try
            {
                SpecialtyEditForm form = new SpecialtyEditForm();
                if (form.ShowDialog() == DialogResult.OK)
                    LoadSpecialties();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка открытия формы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditSpecialty()
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
                SpecialtyEditForm form = new SpecialtyEditForm(specialty);
                if (form.ShowDialog() == DialogResult.OK)
                    LoadSpecialties();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSpecialty()
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

                if (MessageBox.Show($"Удалить «{specialty.Name}»?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseHelper.DeleteBookCategory(specialty.Id);
                    LoadSpecialties();
                    MessageBox.Show("Категория удалена!", "Готово",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = (User)dgvUsers.SelectedRows[0].DataBoundItem;

                if (user.Role == "Admin")
                {
                    MessageBox.Show("Нельзя удалить администратора!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"Удалить «{user.FullName}»?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseHelper.DeleteUser(user.Id);
                    LoadUsers();
                    MessageBox.Show("Пользователь удален!", "Готово",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
