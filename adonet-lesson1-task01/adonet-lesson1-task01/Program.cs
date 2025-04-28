using System;
using Microsoft.Data.SqlClient;

namespace AdoNetSample1
{
    class Program
    {
        SqlConnection conn = null;
        const string ServerName = "(localdb)\\MSSQLLocalDB";
        const string DatabaseName = "Library2";

        public Program()
        {
            conn = new SqlConnection();
            conn.ConnectionString = $@"Data Source={ServerName};
                Initial Catalog={DatabaseName};
                Integrated Security=SSPI;
                Pooling=false"; // Отключаем пуллинг для создания БД
        }

        static void Main(string[] args)
        {
            Program pr = new Program();
            pr.CreateDatabaseIfNotExists();
            pr.CreateTable();
            pr.InsertQuery();
        }

        public void CreateDatabaseIfNotExists()
        {
            try
            {
                string masterConnectionString = $@"Data Source={ServerName};
                    Initial Catalog=master;
                    Integrated Security=SSPI;
                    Pooling=false";

                using (var masterConn = new SqlConnection(masterConnectionString))
                {
                    masterConn.Open();

                    // Проверяем существование базы данных
                    var cmdCheckDb = new SqlCommand(
                        $"SELECT database_id FROM sys.databases WHERE Name = '{DatabaseName}'",
                        masterConn);
                    var result = cmdCheckDb.ExecuteScalar();

                    if (result == null)
                    {
                        // Создаем базу с правильной кодировкой для кириллицы
                        var cmdCreateDb = new SqlCommand(
                            $"CREATE DATABASE [{DatabaseName}] COLLATE Cyrillic_General_CI_AS",
                            masterConn);
                        cmdCreateDb.ExecuteNonQuery();
                        Console.WriteLine("База данных создана с поддержкой кириллицы.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании БД: {ex.Message}");
            }
        }

        public void CreateTable()
        {
            try
            {
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

                SqlCommand cmd = new SqlCommand(createTableQuery, conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Таблица Authors создана или уже существует.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании таблицы: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertQuery()
        {
            try
            {
                conn.Open();
                // Используем параметризованный запрос для безопасности
                string insertString = @"
                    INSERT INTO Authors (FirstName, LastName)
                    VALUES (@FirstName, @LastName)";

                SqlCommand cmd = new SqlCommand(insertString, conn);

                // Добавляем параметры
                cmd.Parameters.AddWithValue("@FirstName", "Клиффорд");
                cmd.Parameters.AddWithValue("@LastName", "Саймак");

                cmd.ExecuteNonQuery();
                Console.WriteLine("Данные успешно добавлены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вставке данных: {ex.Message}");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}