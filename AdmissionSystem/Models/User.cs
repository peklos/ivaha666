using System;

namespace LibrarySystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;  // Номер студенческого билета
        public string Address { get; set; } = null!;        // Адрес проживания
        public string Role { get; set; } = null!;           // "Admin" или "User"
        public DateTime RegistrationDate { get; set; }

        public User()
        {
            RegistrationDate = DateTime.Now;
        }
    }
}
