using System;
using Microsoft.Data.SqlClient;

namespace AdoNetLibraryApp
{
    class Program
    {
        private const string ServerName = "(localdb)\\MSSQLLocalDB";
        private const string DatabaseName = "Library2";
        private readonly string connectionString;

        public Program()
        {
            connectionString = $@"Data Source={ServerName};
                Initial Catalog={DatabaseName};
                Integrated Security=SSPI;
                Pooling=false";

            InitializeDatabase();
        }

        static void Main(string[] args)
        {
            Program app = new Program();
            app.RunMainMenu();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (var conn = new SqlConnection($@"Data Source={ServerName};Initial Catalog=master;Integrated Security=SSPI"))
                {
                    conn.Open();

                    // Создаем базу данных, если не существует
                    var cmdCheckDb = new SqlCommand(
                        $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{DatabaseName}') " +
                        $"CREATE DATABASE [{DatabaseName}] COLLATE Cyrillic_General_CI_AS", conn);
                    cmdCheckDb.ExecuteNonQuery();
                }

                // Создаем таблицу, если не существует
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmdCreateTable = new SqlCommand(
                        @"IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='Authors' AND xtype='U')
                          CREATE TABLE Authors (
                              Id INT IDENTITY(1,1) PRIMARY KEY,
                              FirstName NVARCHAR(50) NOT NULL,
                              LastName NVARCHAR(50) NOT NULL
                          )", conn);
                    cmdCreateTable.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации БД: {ex.Message}");
            }
        }

        private void RunMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("БИБЛИОТЕКА АВТОРОВ");
                Console.WriteLine("1. Просмотр всех авторов");
                Console.WriteLine("2. Добавить нового автора");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите действие: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowAllAuthors();
                        break;
                    case "2":
                        AddNewAuthor();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void ShowAllAuthors()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК АВТОРОВ:\n");

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT Id, FirstName, LastName FROM Authors ORDER BY LastName, FirstName", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Авторы не найдены.");
                            return;
                        }

                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Id"]}. {reader["LastName"]} {reader["FirstName"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении данных: {ex.Message}");
            }
        }

        private void AddNewAuthor()
        {
            Console.Clear();
            Console.WriteLine("ДОБАВЛЕНИЕ НОВОГО АВТОРА\n");

            Console.Write("Введите имя автора: ");
            var firstName = Console.ReadLine();

            Console.Write("Введите фамилию автора: ");
            var lastName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Имя и фамилия не могут быть пустыми!");
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(
                        "INSERT INTO Authors (FirstName, LastName) VALUES (@FirstName, @LastName)",
                        conn);

                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nАвтор успешно добавлен!");
                    }
                    else
                    {
                        Console.WriteLine("\nНе удалось добавить автора.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении автора: {ex.Message}");
            }
        }
    }
}