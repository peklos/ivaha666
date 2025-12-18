using System;
using System.Drawing;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class BookRequestForm : Form
    {
        private User currentUser = null!;
        private BookCategory category = null!;

        private TextBox txtBookTitle = null!;
        private TextBox txtAuthor = null!;
        private TextBox txtISBN = null!;
        private ComboBox cmbCategory = null!;
        private Button btnSubmit = null!;
        private Button btnCancel = null!;

        public BookRequestForm(User user)
        {
            currentUser = user;
            InitializeComponent();
        }

        public BookRequestForm(User user, BookCategory cat)
        {
            currentUser = user;
            category = cat;
            InitializeComponent();

            if (cmbCategory != null && category != null)
            {
                cmbCategory.SelectedValue = category.Id;
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(540, 420);
            this.Text = "Заявка на книгу";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.CardBackground;

            int yPosition = 20;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ЗАЯВКА НА КНИГУ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(490, 40),
                Location = new Point(25, yPosition),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            yPosition += 50;

            // Книга
            Label lblCategory = new Label
            {
                Text = "Книга:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(100, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            cmbCategory = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(130, yPosition),
                Size = new Size(360, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            // Загрузка категорий
            var categories = DatabaseHelper.GetAllBookCategories();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            yPosition += 40;

            // Название книги
            Label lblBookTitle = new Label
            {
                Text = "Название книги:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(120, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            txtBookTitle = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(485, 30),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 70;

            // Автор
            Label lblAuthor = new Label
            {
                Text = "Автор:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(100, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            txtAuthor = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(485, 30),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 70;

            // ISBN
            Label lblISBN = new Label
            {
                Text = "ISBN (необязательно):",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(150, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            txtISBN = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(485, 30),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 70;

            // Кнопки — расположим как таблицу (2 колонки)
            var actionsPanel = new TableLayoutPanel
            {
                Location = new Point(25, yPosition),
                Size = new Size(485, 50),
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnSubmit = ModernUIHelper.CreateGradientButton("ПОДАТЬ ЗАЯВКУ", Point.Empty, new Size(1,1), ModernUIHelper.PrimaryAccent, ModernUIHelper.PrimaryAccent);
            btnSubmit.Click += BtnSubmit_Click;

            btnCancel = ModernUIHelper.CreateGradientButton("ОТМЕНА", Point.Empty, new Size(1,1), ModernUIHelper.SecondaryAccent, ModernUIHelper.SecondaryAccent);
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            btnSubmit.Dock = DockStyle.Fill; btnCancel.Dock = DockStyle.Fill;

            actionsPanel.Controls.Add(btnSubmit, 0, 0);
            actionsPanel.Controls.Add(btnCancel, 1, 0);

            // Добавление контролов
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cmbCategory);
            this.Controls.Add(lblBookTitle);
            this.Controls.Add(txtBookTitle);
            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAuthor);
            this.Controls.Add(lblISBN);
            this.Controls.Add(txtISBN);
            this.Controls.Add(actionsPanel);
        }

        private Label CreateLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(550, 20)
            };
        }

        private TextBox CreateTextBox(int yPosition)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(550, 30),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtBookTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BookRequest request = new BookRequest
                {
                    UserId = currentUser.Id,
                    BookCategoryId = ((BookCategory)cmbCategory.SelectedItem).Id,
                    BookTitle = txtBookTitle.Text.Trim(),
                    Author = txtAuthor.Text.Trim(),
                    ISBN = txtISBN.Text.Trim()
                };

                DatabaseHelper.AddBookRequest(request);

                MessageBox.Show("Заявка на книгу успешно подана!\nОжидайте рассмотрения администратором.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подаче заявки: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
