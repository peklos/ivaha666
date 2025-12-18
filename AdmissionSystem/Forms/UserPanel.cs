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
            this.Size = new Size(1500, 900);
            this.Text = "Библиотека БППК - Личный кабинет студента";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ModernUIHelper.LightBackground;
            this.DoubleBuffered = true;

            // Боковая панель навигации (компактная)
            sidebarPanel = ModernUIHelper.CreateSidebar(new Size(220, 900));

            // Логотип библиотеки
            Label lblLogo = new Label
            {
                Text = "📖",
                Font = new Font("Segoe UI", 48),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(220, 72),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblWelcome = new Label
            {
                Text = "ЛИЧНЫЙ\nКАБИНЕТ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(220, 66),
                Location = new Point(0, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblUserName = new Label
            {
                Text = currentUser.FullName,
                Font = new Font("Segoe UI", 10),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(200, 30),
                Location = new Point(10, 170),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Номер студенческого билета
            Label lblStudentNumber = new Label
            {
                Text = $"№ {currentUser.StudentNumber ?? "—"}",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernUIHelper.TextMuted,
                Size = new Size(200, 25),
                Location = new Point(10, 195),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Panel divider = ModernUIHelper.CreateDivider(new Point(10, 230), 200);

            // Кнопки навигации
            btnSpecialtiesNav = ModernUIHelper.CreateSidebarButton("  Каталог книг", 260, true);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnApplicationsNav = ModernUIHelper.CreateSidebarButton("  Мои заявки", 330);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            sidebarPanel.Controls.Add(lblStudentNumber);

            // Кнопка выхода
            Button btnLogout = new Button
            {
                Text = "Выход",
                Location = new Point(0, 800),
                Size = new Size(220, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12),
                ForeColor = ModernUIHelper.DangerColor,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = ModernUIHelper.CardBackground;
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
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnApplicationsNav);
            sidebarPanel.Controls.Add(btnLogout);

            // Панель контента
            contentPanel = new Panel
            {
                Location = new Point(220, 0),
                Size = new Size(1280, 900),
                BackColor = ModernUIHelper.CardBackground
            };

            // Заголовок страницы
            lblPageTitle = new Label
            {
                Text = "ДОСТУПНЫЕ КАТЕГОРИИ КНИГ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1240, 60),
                Location = new Point(40, 30),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Создаем панели для разных разделов
            CreateSpecialtiesPanel();
            CreateApplicationsPanel();

            this.Controls.Add(sidebarPanel);
            this.Controls.Add(contentPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1220, 750),
                BackColor = Color.Transparent,
                Visible = true
            };

            // DataGridView для категорий
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 70),
                Size = new Size(1220, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками
            var buttonPanel = new TableLayoutPanel
            {
                Location = new Point(0, 640),
                Size = new Size(1220, 80),
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));

            Button btnSubmit = ModernUIHelper.CreateGradientButton("ПОДАТЬ ЗАЯВЛЕНИЕ", Point.Empty, new Size(1,1), ModernUIHelper.PrimaryAccent, ModernUIHelper.PrimaryAccent);
            btnSubmit.Click += (s, e) => SubmitApplication();

            Button btnRefresh = ModernUIHelper.CreateGradientButton("ОБНОВИТЬ", Point.Empty, new Size(1,1), ModernUIHelper.SecondaryAccent, ModernUIHelper.SecondaryAccent);
            btnRefresh.Click += (s, e) => LoadSpecialties();

            btnSubmit.Dock = DockStyle.Fill; btnRefresh.Dock = DockStyle.Fill;

            buttonPanel.Controls.Add(btnSubmit, 0, 0);
            buttonPanel.Controls.Add(btnRefresh, 1, 0);

            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = false
            };

            // Заголовок панели заявлений
            Label lblAppsTitle = new Label
            {
                Text = "МОИ ЗАЯВЛЕНИЯ",
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
                Size = new Size(1140, 550),
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // FlowLayoutPanel для автоматического расположения карточек
            cardsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(1120, 550),
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            scrollPanel.Controls.Add(cardsFlowPanel);

            var buttonPanel = new TableLayoutPanel
            {
                Location = new Point(0, 640),
                Size = new Size(1160, 80),
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            Button btnDelete = ModernUIHelper.CreateGradientButton("УДАЛИТЬ", Point.Empty, new Size(1,1), ModernUIHelper.DangerColor, ModernUIHelper.DangerColor);
            btnDelete.Click += (s, e) => DeleteApplication();

            btnRefreshCards = ModernUIHelper.CreateGradientButton("ОБНОВИТЬ", Point.Empty, new Size(1,1), ModernUIHelper.SecondaryAccent, ModernUIHelper.SecondaryAccent);
            btnRefreshCards.Click += (s, e) => LoadApplicationsCards();

            btnDelete.Dock = DockStyle.Fill; btnRefreshCards.Dock = DockStyle.Fill;

            buttonPanel.Controls.Add(btnDelete, 0, 0);
            buttonPanel.Controls.Add(btnRefreshCards, 1, 0);

            applicationsPanel.Controls.Add(lblAppsTitle);
            applicationsPanel.Controls.Add(scrollPanel);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void ShowSpecialtiesPanel()
        {
            specialtiesPanel.Visible = true;
            applicationsPanel.Visible = false;

            btnSpecialtiesNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextPrimary;
            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "ДОСТУПНЫЕ КАТЕГОРИИ КНИГ";
        }

        private void ShowApplicationsPanel()
        {
            specialtiesPanel.Visible = false;
            applicationsPanel.Visible = true;

            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnApplicationsNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextPrimary;

            lblPageTitle.Text = "МОИ ЗАЯВЛЕНИЯ";
            
            // Обновляем карточки при переходе на вкладку
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
                // Не показываем ошибку пользователю
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
                // Не показываем ошибку пользователю
                if (dgvSpecialties != null)
                {
                    dgvSpecialties.DataSource = null;
                }
            }
        }

        private void LoadApplicationsCards()
        {
            // Очищаем старые карточки
            cardsFlowPanel.Controls.Clear();

            try
            {
                // Получаем заявления пользователя
                List<BookRequest> applications = DatabaseHelper.GetUserBookRequests(currentUser.Id);

                if (applications.Count == 0)
                {
                    // Сообщение если нет заявлений
                    Label lblNoApps = new Label
                    {
                        Text = "У вас пока нет заявлений.\nПерейдите в раздел 'Категории' чтобы подать заявку.",
                        Font = new Font("Segoe UI", 12),
                        ForeColor = ModernUIHelper.TextSecondary,
                        Size = new Size(1100, 100),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };
                    cardsFlowPanel.Controls.Add(lblNoApps);
                    return;
                }

                // Создаем карточки для каждого заявления
                foreach (var app in applications)
                {
                    var card = ModernUIHelper.CreateApplicationCard(app, (s, e) =>
                    {
                        // При клике на карточку открываем детали
                        ApplicationDetailsForm detailsForm = new ApplicationDetailsForm(app, false);
                        detailsForm.ShowDialog();
                        
                        // Обновляем карточки после закрытия формы (если статус изменился)
                        if (detailsForm.DialogResult == DialogResult.OK)
                        {
                            LoadApplicationsCards();
                        }
                    });
                    
                    cardsFlowPanel.Controls.Add(card);
                }
            }
            catch (Exception)
            {
                // Не показываем ошибку пользователю
                Label lblError = new Label
                {
                    Text = "Не удалось загрузить заявления",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = ModernUIHelper.TextSecondary,
                    Size = new Size(1100, 100),
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
                MessageBox.Show("Выберите категорию!", "Предупреждение",
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
                MessageBox.Show("Не удалось открыть форму подачи заявления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteApplication()
        {
            // Находим выбранную карточку
            Panel selectedCard = null;
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
                MessageBox.Show("Выберите заявление для удаления (кликните на карточку)!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (BookRequest)selectedCard.Tag;

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
    }
}
