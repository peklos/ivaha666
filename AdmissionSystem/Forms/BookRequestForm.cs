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
            this.Size = new Size(500, 450);
            this.Text = "КНИЖНЫЙ ФОНД — Заявка на книгу";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            int yPos = 25;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "НОВАЯ ЗАЯВКА",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(450, 40),
                Location = new Point(25, yPos),
                BackColor = Color.Transparent
            };
            yPos += 50;

            // Разделитель
            Panel divider = new Panel
            {
                Size = new Size(450, 1),
                Location = new Point(25, yPos),
                BackColor = Color.FromArgb(220, 220, 220)
            };
            yPos += 20;

            // Категория
            Label lblCategory = new Label
            {
                Text = "КАТЕГОРИЯ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(25, yPos),
                Size = new Size(450, 20),
                BackColor = Color.Transparent
            };
            yPos += 22;

            cmbCategory = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(450, 35),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black
            };
            var categories = DatabaseHelper.GetAllBookCategories();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            yPos += 45;

            // Название книги
            Label lblBookTitle = new Label
            {
                Text = "НАЗВАНИЕ КНИГИ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(25, yPos),
                Size = new Size(450, 20),
                BackColor = Color.Transparent
            };
            yPos += 22;

            txtBookTitle = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(450, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black
            };
            yPos += 48;

            // Автор
            Label lblAuthor = new Label
            {
                Text = "АВТОР",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(25, yPos),
                Size = new Size(450, 20),
                BackColor = Color.Transparent
            };
            yPos += 22;

            txtAuthor = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(450, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black
            };
            yPos += 48;

            // ISBN
            Label lblISBN = new Label
            {
                Text = "ISBN (необязательно)",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(25, yPos),
                Size = new Size(450, 20),
                BackColor = Color.Transparent
            };
            yPos += 22;

            txtISBN = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(450, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black
            };
            yPos += 55;

            // Кнопки
            btnSubmit = new Button
            {
                Text = "ПОДАТЬ ЗАЯВКУ",
                Location = new Point(25, yPos),
                Size = new Size(220, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += BtnSubmit_Click;
            btnSubmit.MouseEnter += (s, e) => btnSubmit.BackColor = Color.FromArgb(40, 40, 40);
            btnSubmit.MouseLeave += (s, e) => btnSubmit.BackColor = Color.Black;

            btnCancel = new Button
            {
                Text = "ОТМЕНА",
                Location = new Point(255, yPos),
                Size = new Size(220, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Добавление контролов
            this.Controls.Add(lblTitle);
            this.Controls.Add(divider);
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
                MessageBox.Show("Заполните все обязательные поля!", "Внимание",
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

                MessageBox.Show("Заявка подана!\nОжидайте рассмотрения.",
                    "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
