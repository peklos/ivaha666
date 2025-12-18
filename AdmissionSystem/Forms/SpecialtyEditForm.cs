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
            this.BackColor = ModernUIHelper.CardBackground;

            int yPosition = 20;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = isEditMode ? "РЕДАКТИРОВАНИЕ КАТЕГОРИИ" : "ДОБАВЛЕНИЕ КАТЕГОРИИ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(470, 40),
                Location = new Point(25, yPosition),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            yPosition += 50;

            // Название
            Label lblName = CreateLabel("Название:", yPosition);
            lblName.ForeColor = ModernUIHelper.TextSecondary;
            txtName = CreateTextBox(yPosition + 25);
            txtName.BackColor = ModernUIHelper.SidebarBackground;
            txtName.ForeColor = ModernUIHelper.TextPrimary;
            txtName.Text = isEditMode ? specialty.Name : string.Empty;
            yPosition += 70;

            // Код
            Label lblCode = CreateLabel("Код:", yPosition);
            lblCode.ForeColor = ModernUIHelper.TextSecondary;
            txtCode = CreateTextBox(yPosition + 25);
            txtCode.BackColor = ModernUIHelper.SidebarBackground;
            txtCode.ForeColor = ModernUIHelper.TextPrimary;
            txtCode.Text = isEditMode ? specialty.Code : string.Empty;
            yPosition += 70;

            // Количество книг
            Label lblPlaces = CreateLabel("Количество книг:", yPosition);
            lblPlaces.ForeColor = ModernUIHelper.TextSecondary;
            numPlaces = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(450, 30),
                Minimum = 1,
                Maximum = 10000,
                Value = isEditMode ? specialty.BooksCount : 100,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary
            };
            yPosition += 70;

            // Описание
            Label lblDescription = CreateLabel("Описание:", yPosition);
            lblDescription.ForeColor = ModernUIHelper.TextSecondary;
            txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition + 25),
                Size = new Size(450, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                Text = isEditMode ? specialty.Description : string.Empty
            };
            yPosition += 150;

            // Кнопки — таблица (2 колонки)
            var actionsPanel = new TableLayoutPanel
            {
                Location = new Point(25, yPosition),
                Size = new Size(450, 50),
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            Button btnSave = ModernUIHelper.CreateGradientButton("СОХРАНИТЬ", Point.Empty, new Size(1,1), ModernUIHelper.PrimaryAccent, ModernUIHelper.PrimaryAccent);
            btnSave.Click += BtnSave_Click;

            Button btnCancel = ModernUIHelper.CreateGradientButton("ОТМЕНА", Point.Empty, new Size(1,1), ModernUIHelper.SecondaryAccent, ModernUIHelper.SecondaryAccent);
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            btnSave.Dock = DockStyle.Fill; btnCancel.Dock = DockStyle.Fill;

            actionsPanel.Controls.Add(btnSave, 0, 0);
            actionsPanel.Controls.Add(btnCancel, 1, 0);

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
            this.Controls.Add(actionsPanel);
        }

        private Label CreateLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(450, 20)
            };
        }

        private TextBox CreateTextBox(int yPosition)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, yPosition),
                Size = new Size(450, 30),
                BorderStyle = BorderStyle.FixedSingle
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

                MessageBox.Show($"Книга успешно {(isEditMode ? "обновлена" : "добавлена")}!",
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
