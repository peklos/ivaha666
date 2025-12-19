using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using LibrarySystem.Models;

namespace LibrarySystem.Database
{
    public static class DatabaseHelper
    {
        private static string DbPath
                            {
                                get
                                {
                                    var baseDir = AppContext.BaseDirectory;
                            
                                    if (string.IsNullOrWhiteSpace(baseDir))
                                        throw new InvalidOperationException("BaseDirectory is not initialized");
                            
                                    return Path.Combine(baseDir, "library.db");
                                }
                            }
                            
                            private static string ConnectionString =>
                                $"Data Source={DbPath};Version=3;";


        public static void InitializeDatabase()
        {
            bool isNewDatabase = !File.Exists(dbPath);

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Создание таблицы пользователей с полями для студентов
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Login TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        FullName TEXT NOT NULL,
                        Email TEXT,
                        PhoneNumber TEXT,
                        StudentNumber TEXT,
                        Address TEXT,
                        Role TEXT NOT NULL,
                        RegistrationDate TEXT NOT NULL
                    )");

                // Добавляем новые колонки если их нет (для совместимости)
                try { connection.Execute("ALTER TABLE Users ADD COLUMN Email TEXT;"); } catch { }
                try { connection.Execute("ALTER TABLE Users ADD COLUMN PhoneNumber TEXT;"); } catch { }
                try { connection.Execute("ALTER TABLE Users ADD COLUMN StudentNumber TEXT;"); } catch { }
                try { connection.Execute("ALTER TABLE Users ADD COLUMN Address TEXT;"); } catch { }

                // Создание таблицы категорий книг
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS BookCategories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Code TEXT NOT NULL UNIQUE,
                        BooksCount INTEGER NOT NULL,
                        Description TEXT
                    )");

                // Создание таблицы заявок на книги
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS BookRequests (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        BookCategoryId INTEGER NOT NULL,
                        BookTitle TEXT NOT NULL,
                        Author TEXT NOT NULL,
                        ISBN TEXT,
                        RequestDate TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        SubmissionDate TEXT NOT NULL,
                        Notes TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id),
                        FOREIGN KEY (BookCategoryId) REFERENCES BookCategories(Id)
                    )");

                // Добавление тестовых данных
                if (isNewDatabase)
                {
                    // Администратор
                    connection.Execute(@"
                        INSERT INTO Users (Login, Password, FullName, Email, PhoneNumber, StudentNumber, Address, Role, RegistrationDate)
                        VALUES ('admin', 'admin123', 'Администратор Библиотеки', 'admin@bppk.ru', '+7 (999) 000-00-01', '', 'г. Белгород, ул. Библиотечная, д. 1', 'Admin', @date)",
                        new { date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                    // Библиотекарь
                    connection.Execute(@"
                        INSERT INTO Users (Login, Password, FullName, Email, PhoneNumber, StudentNumber, Address, Role, RegistrationDate)
                        VALUES ('librarian', 'lib123', 'Иванова Мария Петровна', 'ivanova@bppk.ru', '+7 (999) 000-00-02', '', 'г. Белгород, ул. Центральная, д. 15', 'Admin', @date)",
                        new { date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                    // Тестовые студенты
                    connection.Execute(@"
                        INSERT INTO Users (Login, Password, FullName, Email, PhoneNumber, StudentNumber, Address, Role, RegistrationDate)
                        VALUES
                        ('student1', 'pass123', 'Петров Иван Сергеевич', 'petrov@student.bppk.ru', '+7 (999) 111-11-11', 'СТ-2024-001', 'г. Белгород, ул. Студенческая, д. 5, кв. 12', 'User', @date),
                        ('student2', 'pass123', 'Сидорова Анна Владимировна', 'sidorova@student.bppk.ru', '+7 (999) 222-22-22', 'СТ-2024-002', 'г. Белгород, пр. Славы, д. 78, кв. 45', 'User', @date),
                        ('student3', 'pass123', 'Козлов Дмитрий Александрович', 'kozlov@student.bppk.ru', '+7 (999) 333-33-33', 'СТ-2024-003', 'г. Белгород, ул. Народный бульвар, д. 102', 'User', @date),
                        ('student4', 'pass123', 'Морозова Елена Игоревна', 'morozova@student.bppk.ru', '+7 (999) 444-44-44', 'СТ-2023-015', 'г. Белгород, ул. Губкина, д. 33, кв. 8', 'User', @date),
                        ('student5', 'pass123', 'Новиков Алексей Павлович', 'novikov@student.bppk.ru', '+7 (999) 555-55-55', 'СТ-2023-022', 'г. Белгород, ул. Щорса, д. 64, кв. 21', 'User', @date)",
                        new { date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                    // Категории книг для колледжа
                    connection.Execute(@"
                        INSERT INTO BookCategories (Name, Code, BooksCount, Description)
                        VALUES
                        ('Учебники по программированию', 'PROG', 150, 'Языки программирования, алгоритмы, структуры данных'),
                        ('Техническая литература', 'TECH', 200, 'Компьютерные сети, операционные системы, базы данных'),
                        ('Математика и физика', 'MATH', 120, 'Высшая математика, дискретная математика, физика'),
                        ('Художественная литература', 'FICTION', 300, 'Классика, современная проза, поэзия'),
                        ('Экономика и право', 'ECON', 80, 'Экономическая теория, правоведение, менеджмент'),
                        ('Иностранные языки', 'LANG', 100, 'Английский, немецкий, учебные пособия'),
                        ('История и философия', 'HIST', 90, 'История России, мировая история, философия'),
                        ('Периодические издания', 'PERIOD', 50, 'Журналы, газеты, научные статьи')");

                    // Тестовые заявки на книги
                    connection.Execute(@"
                        INSERT INTO BookRequests (UserId, BookCategoryId, BookTitle, Author, ISBN, RequestDate, Status, SubmissionDate, Notes)
                        VALUES
                        (3, 1, 'Чистый код', 'Роберт Мартин', '978-5-4461-0960-9', @date, 'На рассмотрении', @date, ''),
                        (3, 1, 'Грокаем алгоритмы', 'Адитья Бхаргава', '978-5-4461-0923-4', @date, 'Одобрено', @date, 'Выдана до 15.01.2025'),
                        (4, 2, 'Компьютерные сети', 'Эндрю Таненбаум', '978-5-4461-1248-7', @date, 'На рассмотрении', @date, ''),
                        (5, 4, 'Мастер и Маргарита', 'Михаил Булгаков', '978-5-17-090325-1', @date, 'Одобрено', @date, ''),
                        (6, 3, 'Высшая математика', 'Письменный Д.Т.', '978-5-8112-6421-4', @date, 'Отклонено', @date, 'Нет в наличии'),
                        (7, 6, 'English Grammar in Use', 'Raymond Murphy', '978-1-108-45768-0', @date, 'На рассмотрении', @date, '')",
                        new { date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                }
            }
        }

        // Методы для работы с пользователями
        public static User? GetUser(string login, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var trimmedLogin = login?.Trim();
                var user = connection.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Login = @Login COLLATE NOCASE",
                    new { Login = trimmedLogin });

                if (user == null)
                    return null;

                if (string.Equals(user.Password?.Trim(), password?.Trim(), StringComparison.Ordinal))
                    return user;

                return null;
            }
        }

        public static bool RegisterUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    user.Login = user.Login?.Trim() ?? "";
                    user.Password = user.Password?.Trim() ?? "";
                    user.Email = user.Email?.Trim() ?? "";
                    user.PhoneNumber = user.PhoneNumber?.Trim() ?? "";
                    user.StudentNumber = user.StudentNumber?.Trim() ?? "";
                    user.Address = user.Address?.Trim() ?? "";

                    connection.Execute(@"
                        INSERT INTO Users (Login, Password, FullName, Email, PhoneNumber, StudentNumber, Address, Role, RegistrationDate)
                        VALUES (@Login, @Password, @FullName, @Email, @PhoneNumber, @StudentNumber, @Address, @Role, @RegistrationDate)",
                        user);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<User> GetAllUsers()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        public static void UpdateUser(User user)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE Users
                    SET Login = @Login, Password = @Password, FullName = @FullName,
                        Email = @Email, PhoneNumber = @PhoneNumber, StudentNumber = @StudentNumber,
                        Address = @Address, Role = @Role
                    WHERE Id = @Id", user);
            }
        }

        public static void DeleteUser(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = userId });
            }
        }

        // Методы для работы с категориями книг
        public static List<BookCategory> GetAllBookCategories()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<BookCategory>("SELECT * FROM BookCategories").ToList();
            }
        }

        public static void AddBookCategory(BookCategory category)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    INSERT INTO BookCategories (Name, Code, BooksCount, Description)
                    VALUES (@Name, @Code, @BooksCount, @Description)",
                    category);
            }
        }

        public static void UpdateBookCategory(BookCategory category)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE BookCategories
                    SET Name = @Name, Code = @Code, BooksCount = @BooksCount, Description = @Description
                    WHERE Id = @Id", category);
            }
        }

        public static void DeleteBookCategory(int categoryId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM BookCategories WHERE Id = @Id", new { Id = categoryId });
            }
        }

        // Методы для работы с заявками на книги
        public static List<BookRequest> GetAllBookRequests()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<BookRequest>(@"
                    SELECT br.*, bc.Name as CategoryName
                    FROM BookRequests br
                    LEFT JOIN BookCategories bc ON br.BookCategoryId = bc.Id").ToList();
            }
        }

        public static List<BookRequest> GetUserBookRequests(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<BookRequest>(@"
                    SELECT br.*, bc.Name as CategoryName
                    FROM BookRequests br
                    LEFT JOIN BookCategories bc ON br.BookCategoryId = bc.Id
                    WHERE br.UserId = @UserId",
                    new { UserId = userId }).ToList();
            }
        }

        public static void AddBookRequest(BookRequest request)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    INSERT INTO BookRequests
                    (UserId, BookCategoryId, BookTitle, Author, ISBN, RequestDate,
                     Status, SubmissionDate, Notes)
                    VALUES
                    (@UserId, @BookCategoryId, @BookTitle, @Author, @ISBN, @RequestDate,
                     @Status, @SubmissionDate, @Notes)",
                    request);
            }
        }

        public static void UpdateBookRequest(BookRequest request)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE BookRequests
                    SET BookCategoryId = @BookCategoryId, BookTitle = @BookTitle, Author = @Author,
                        ISBN = @ISBN, RequestDate = @RequestDate, Status = @Status, Notes = @Notes
                    WHERE Id = @Id", request);
            }
        }

        public static void DeleteBookRequest(int requestId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM BookRequests WHERE Id = @Id", new { Id = requestId });
            }
        }

        public static BookCategory GetBookCategoryById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QueryFirstOrDefault<BookCategory>(
                    "SELECT * FROM BookCategories WHERE Id = @Id", new { Id = id });
            }
        }

        public static bool UserExists(string login)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var count = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Users WHERE Login = @Login",
                    new { Login = login });
                return count > 0;
            }
        }

        public static void UpdateBookRequestStatus(int requestId, string status)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute(@"
                    UPDATE BookRequests
                    SET Status = @Status
                    WHERE Id = @Id",
                    new { Id = requestId, Status = status });
            }
        }
    }
}
