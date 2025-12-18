namespace LibrarySystem.Models
{
    public class BookCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int BooksCount { get; set; }
        public string Description { get; set; } = null!;
    }
}
