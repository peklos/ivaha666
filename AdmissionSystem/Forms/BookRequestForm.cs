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
            this.Size = new Size(540, 450);
            this.Text = "Заявка на книгу";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.LightBackground;

            int yPosition = 20;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "ЗАЯВКА НА КНИГУ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(490, 45),
                Location = new Point(25, yPosition),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            yPosition += 55;

            // Книга
            Label lblCategory = new Label
            {
                Text = "КАТЕГОРИЯ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(25, yPosition),
                Size = new Size(200, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            yPosition += 25;

            cmbCategory = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(475, 35),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            var categories = DatabaseHelper.GetAllBookCategories();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            yPosition += 45;

            // Название книги
            Label lblBookTitle = new Label
            {
                Text = "НАЗВАНИЕ КНИГИ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(25, yPosition),
                Size = new Size(200, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            yPosition += 25;

            txtBookTitle = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(475, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 45;

            // Автор
            Label lblAuthor = new Label
            {
                Text = "АВТОР",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(25, yPosition),
                Size = new Size(200, 25),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
            yPosition += 25;

            txtAuthor = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(475, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 45;

            // ISBN
            Label lblISBN = new Label
            {
                Text = "ISBN (необязательно)",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(25, yPosition),
                Size = new Size(200, 25),
                ForeColor = ModernUIHelper.TextMuted,
                BackColor = Color.Transparent
            };
            yPosition += 25;

            txtISBN = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(475, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 50;

            // Кнопки
            btnSubmit = ModernUIHelper.CreateGradientButton("ПОДАТЬ ЗАЯВКУ", new Point(25, yPosition), new Size(230, 50), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
            btnSubmit.Click += BtnSubmit_Click;

            btnCancel = ModernUIHelper.CreateGradientButton("ОТМЕНА", new Point(270, yPosition), new Size(230, 50), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

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
            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnCancel);
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
