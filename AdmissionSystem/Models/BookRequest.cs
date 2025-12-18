using System;

namespace LibrarySystem.Models
{
    public class BookRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookCategoryId { get; set; }
        public string BookTitle { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = null!; // "На рассмотрении", "Одобрено", "Отклонено"
        public DateTime SubmissionDate { get; set; }
        public string Notes { get; set; } = null!;

        // Дополнительные свойства для отображения
        public string FullRequest => $"{BookTitle} - {Author}";
        public string CategoryName { get; set; } = null!;
        public string SubmittedAt => SubmissionDate.ToString("dd.MM.yyyy HH:mm");

        public BookRequest()
        {
            SubmissionDate = DateTime.Now;
            Status = "На рассмотрении";
        }
    }
}
