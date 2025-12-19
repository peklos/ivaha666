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

        public SpecialtyEditForm(BookCategory existingSpecialty)
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
            this.Size = new Size(520, 520);
            this.Text = isEditMode ? "Редактирование категории" : "Добавление категории";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.LightBackground;

            int yPosition = 20;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = isEditMode ? "РЕДАКТИРОВАНИЕ" : "НОВАЯ КАТЕГОРИЯ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(470, 45),
                Location = new Point(25, yPosition),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            yPosition += 55;

            // Название
            Label lblName = CreateLabel("НАЗВАНИЕ", yPosition);
            yPosition += 25;
            txtName = CreateTextBox(yPosition);
            txtName.Text = isEditMode ? specialty.Name : string.Empty;
            yPosition += 50;

            // Код
            Label lblCode = CreateLabel("КОД", yPosition);
            yPosition += 25;
            txtCode = CreateTextBox(yPosition);
            txtCode.Text = isEditMode ? specialty.Code : string.Empty;
            yPosition += 50;

            // Количество книг
            Label lblPlaces = CreateLabel("КОЛИЧЕСТВО КНИГ", yPosition);
            yPosition += 25;
            numPlaces = new NumericUpDown
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(450, 35),
                Minimum = 1,
                Maximum = 10000,
                Value = isEditMode ? specialty.BooksCount : 100,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 50;

            // Описание
            Label lblDescription = CreateLabel("ОПИСАНИЕ", yPosition);
            yPosition += 25;
            txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(450, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Text = isEditMode ? specialty.Description : string.Empty
            };
            yPosition += 115;

            // Кнопки
            Button btnSave = ModernUIHelper.CreateGradientButton("СОХРАНИТЬ", new Point(25, yPosition), new Size(220, 50), ModernUIHelper.PrimaryAccent, ModernUIHelper.SecondaryAccent);
            btnSave.Click += BtnSave_Click;

            Button btnCancel = ModernUIHelper.CreateGradientButton("ОТМЕНА", new Point(255, yPosition), new Size(220, 50), ModernUIHelper.SecondaryAccent, ModernUIHelper.PrimaryAccent);
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Добавление контролов
            this.Controls.Add(lblTitle);
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

        private Label CreateLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(25, yPosition),
                Size = new Size(450, 22),
                ForeColor = ModernUIHelper.TextSecondary,
                BackColor = Color.Transparent
            };
        }

        private TextBox CreateTextBox(int yPosition)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, yPosition),
                Size = new Size(450, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = ModernUIHelper.LightBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название категории!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Введите код категории!", "Ошибка",
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

                MessageBox.Show($"Категория успешно {(isEditMode ? "обновлена" : "добавлена")}!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
