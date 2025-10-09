using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        // Строка подключения
        string connectionString = GetConnectionString();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение к SQL Server успешно установлено!");

                // Получаем информацию о сервере
                GetServerInfo(connection);

                // Проверяем доступные базы данных
                GetDatabases(connection);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL Server: {ex.Message}");
            Console.WriteLine($"Код ошибки: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }

        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    static string GetConnectionString()
    {
        // Выберите подходящий вариант:

        // 1. LocalDB
        //return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=true;TrustServerCertificate=true;";

        // 2. SQL Server Express
        return @"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=true;TrustServerCertificate=true;";

        // 3. Полный SQL Server с аутентификацией Windows
        //return @"Data Source=МойСервер;Initial Catalog=master;Integrated Security=true;";

        // 4. SQL Server с аутентификацией SQL Server
        // return @"Data Source=МойСервер;Initial Catalog=master;User Id=sa;Password=ВашПароль;";
    }

    static void GetServerInfo(SqlConnection connection)
    {
        string query = @"
            SELECT
                @@SERVERNAME as ServerName,
                @@VERSION as Version,
                DB_NAME() as CurrentDatabase";

        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                Console.WriteLine($"\nИнформация о сервере:");
                Console.WriteLine($"Имя сервера: {reader["ServerName"]}");
                Console.WriteLine($"Текущая БД: {reader["CurrentDatabase"]}");
                Console.WriteLine($"Версия: {reader["Version"]}");
            }
        }
    }

    static void GetDatabases(SqlConnection connection)
    {
        string query = @"
            SELECT name, state_desc, create_date
            FROM sys.databases
            WHERE state = 0  -- только онлайн базы данных
            ORDER BY name";

        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nДоступные базы данных:");
            Console.WriteLine("Имя базы данных".PadRight(30) + "Статус".PadRight(15) + "Дата создания");
            Console.WriteLine(new string('-', 70));

            while (reader.Read())
            {
                string dbName = reader["name"].ToString();
                string state = reader["state_desc"].ToString();
                DateTime createDate = (DateTime)reader["create_date"];

                Console.WriteLine($"{dbName.PadRight(30)}{state.PadRight(15)}{createDate:dd.MM.yyyy}");
            }
        }
    }
}