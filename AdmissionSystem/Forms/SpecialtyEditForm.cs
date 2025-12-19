using System;
using System.Drawing;
using System.Windows.Forms;
using LibrarySystem.Database;
using LibrarySystem.Models;
using LibrarySystem.UI;

namespace LibrarySystem.Forms
{
    public partial class SpecialtyEditForm : Form
    {
        private BookCategory specialty = null!;
        private bool isEditMode;

        private TextBox txtName = null!;
        private TextBox txtCode = null!;
        private TextBox txtDescription = null!;
        private NumericUpDown numPlaces = null!;

        public SpecialtyEditForm() : this(null) { }

        public SpecialtyEditForm(BookCategory? existingSpecialty)
        {
            if (existingSpecialty != null)
            {
                specialty = existingSpecialty;
                isEditMode = true;
            }
            else
            {
                specialty = new BookCategory();
                isEditMode = false;
            }
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(480, 520);
            this.Text = isEditMode ? "КНИЖНЫЙ ФОНД — Редактирование" : "КНИЖНЫЙ ФОНД — Новая категория";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            int yPos = 25;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = isEditMode ? "РЕДАКТИРОВАНИЕ" : "НОВАЯ КАТЕГОРИЯ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(430, 40),
                Location = new Point(25, yPos),
                BackColor = Color.Transparent
            };
            yPos += 50;

            // Разделитель
            Panel divider = new Panel
            {
                Size = new Size(430, 1),
                Location = new Point(25, yPos),
                BackColor = Color.FromArgb(220, 220, 220)
            };
            yPos += 20;

            // Название
            Label lblName = CreateLabel("НАЗВАНИЕ", yPos);
            yPos += 22;
            txtName = CreateTextBox(yPos);
            txtName.Text = isEditMode ? specialty.Name : string.Empty;
            yPos += 50;

            // Код
            Label lblCode = CreateLabel("КОД", yPos);
            yPos += 22;
            txtCode = CreateTextBox(yPos);
            txtCode.Text = isEditMode ? specialty.Code : string.Empty;
            yPos += 50;

            // Количество книг
            Label lblPlaces = CreateLabel("КОЛИЧЕСТВО КНИГ", yPos);
            yPos += 22;
            numPlaces = new NumericUpDown
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(430, 38),
                Minimum = 1,
                Maximum = 10000,
                Value = isEditMode ? specialty.BooksCount : 100,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
            yPos += 50;

            // Описание
            Label lblDescription = CreateLabel("ОПИСАНИЕ", yPos);
            yPos += 22;
            txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(430, 90),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                Text = isEditMode ? specialty.Description : string.Empty
            };
            yPos += 110;

            // Кнопки
            Button btnSave = new Button
            {
                Text = "СОХРАНИТЬ",
                Location = new Point(25, yPos),
                Size = new Size(210, 48),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            btnSave.MouseEnter += (s, e) => btnSave.BackColor = Color.FromArgb(40, 40, 40);
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = Color.Black;

            Button btnCancel = new Button
            {
                Text = "ОТМЕНА",
                Location = new Point(245, yPos),
                Size = new Size(210, 48),
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
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblCode);
            this.Controls.Add(txtCode);
            this.Controls.Add(lblPlaces);
            this.Controls.Add(numPlaces);
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private Label CreateLabel(string text, int yPos)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(25, yPos),
                Size = new Size(430, 20),
                BackColor = Color.Transparent
            };
        }

        private TextBox CreateTextBox(int yPos)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPos),
                Size = new Size(430, 38),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 248, 248),
                ForeColor = Color.Black
            };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Введите код!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                specialty.Name = txtName.Text.Trim();
                specialty.Code = txtCode.Text.Trim();
                specialty.Description = txtDescription.Text.Trim();
                specialty.BooksCount = (int)numPlaces.Value;

                if (isEditMode)
                {
                    DatabaseHelper.UpdateBookCategory(specialty);
                }
                else
                {
                    DatabaseHelper.AddBookCategory(specialty);
                }

                MessageBox.Show($"Категория {(isEditMode ? "обновлена" : "добавлена")}!",
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
