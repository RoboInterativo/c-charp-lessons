using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        // Инициализация клиента MS SQL
        const string ServerName = "(localdb)\\MSSQLLocalDB";
        const string DatabaseName = "Library3";

        // Создаем БД
        string masterConnectionString = $@"Data Source={ServerName};
                 Initial Catalog=master;
                 Integrated Security=SSPI;
                 Pooling=false";

        using (var masterConn = new SqlConnection(masterConnectionString))
        {
            masterConn.Open();
            // Создаем базу с правильной кодировкой для кириллицы
            var cmdCreateDb = new SqlCommand(
                $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{DatabaseName}') CREATE DATABASE [{DatabaseName}] COLLATE Cyrillic_General_CI_AS",
                masterConn);
            cmdCreateDb.ExecuteNonQuery();
            Console.WriteLine("База данных создана с поддержкой кириллицы.");
        }

        // Создаем подключение к нашей БД
        string connectionString = $@"Data Source={ServerName};
            Initial Catalog={DatabaseName};
            Integrated Security=SSPI;
            Pooling=false";

        using (var conn = new SqlConnection(connectionString))
        {
            // Создание таблицы
            conn.Open();
            string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Authors' AND xtype='U')
                BEGIN
                    CREATE TABLE Authors (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FirstName NVARCHAR(50) NOT NULL,
                        LastName NVARCHAR(50) NOT NULL
                    )
                END";

            SqlCommand cmdTable = new SqlCommand(createTableQuery, conn);
            cmdTable.ExecuteNonQuery();
            Console.WriteLine("Таблица проверена/создана.");

            // Вставка данных
            // Используем параметризованный запрос для безопасности
            string insertString = @"
                INSERT INTO Authors (FirstName, LastName)
                VALUES (@FirstName, @LastName)";

            SqlCommand cmdInsert = new SqlCommand(insertString, conn);

            // Добавляем параметры
            cmdInsert.Parameters.AddWithValue("@FirstName", "Клиффорд");
            cmdInsert.Parameters.AddWithValue("@LastName", "Саймак");

            int rowsAffected = cmdInsert.ExecuteNonQuery();
            Console.WriteLine($"Данные успешно добавлены! Затронуто строк: {rowsAffected}");

            // SELECT - чтение данных из таблицы
            Console.WriteLine("\n--- Чтение данных из таблицы Authors ---");
            string selectQuery = "SELECT Id, FirstName, LastName FROM Authors";

            using (SqlCommand cmdSelect = new SqlCommand(selectQuery, conn))
            using (SqlDataReader reader = cmdSelect.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("Id\tFirstName\tLastName");
                    Console.WriteLine("--\t---------\t--------");

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);

                        Console.WriteLine($"{id}\t{firstName}\t\t{lastName}");
                    }
                }
                else
                {
                    Console.WriteLine("В таблице нет данных.");
                }
            }

            // Дополнительный пример: SELECT с WHERE
            Console.WriteLine("\n--- Поиск автора по фамилии ---");
            string searchQuery = "SELECT Id, FirstName, LastName FROM Authors WHERE LastName LIKE @SearchName";

            using (SqlCommand cmdSearch = new SqlCommand(searchQuery, conn))
            {
                cmdSearch.Parameters.AddWithValue("@SearchName", "%Саймак%");

                using (SqlDataReader reader = cmdSearch.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Найденные авторы:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Имя: {reader["FirstName"]}, Фамилия: {reader["LastName"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Авторы не найдены.");
                    }
                }
            }
        }

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }
}