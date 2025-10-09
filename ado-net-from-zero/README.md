# Работа с базой данных в C# с использованием Microsoft.Data.SqlClient

В этой статье мы рассмотрим, как создать приложение на C#, которое взаимодействует с базой данных SQL Server с помощью пакета Microsoft.Data.SqlClient.

## Установка необходимых пакетов

Для начала работы необходимо установить требуемые пакеты NuGet.

### Шаги установки:

1. Откройте меню `Проект → Управление пакетами NuGet`
2. Перейдите на вкладку "Обзор"
3. Найдите и установите следующие пакеты:
   - **ADO.Net.Client.Core**
   - **Microsoft.Data.SqlClient**

![Установка пакетов](images/ado01.PNG)
![Поиск пакетов](images/ado02.PNG)
![Процесс установки](images/ado03.PNG)

После успешной установки в обозревателе решений в разделе "Зависимости" появятся установленные пакеты:

![Установленные пакеты](images/ado04.PNG)

## Подключение пространств имен

Для работы с базой данных добавьте следующие пространства имен в начало файла:

```csharp
using System;
using Microsoft.Data.SqlClient;
```

## Инициализация подключения к SQL Server

Объявим константы и переменные для подключения:

```csharp
const string ServerName = "(localdb)\\MSSQLLocalDB";
const string DatabaseName = "Library3";
```

## Создание базы данных

Для создания базы данных необходимо подключиться к серверу с указанием `Initial Catalog=master`:

```csharp
string masterConnectionString = $@"Data Source={ServerName};
         Initial Catalog=master;
         Integrated Security=SSPI;
         Pooling=false";

using (var masterConn = new SqlConnection(masterConnectionString))
{
    masterConn.Open();
    // Создаем базу с правильной кодировкой для кириллицы
    var cmdCreateDb = new SqlCommand(
        $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{DatabaseName}') " +
        $"CREATE DATABASE [{DatabaseName}] COLLATE Cyrillic_General_CI_AS",
        masterConn);
    cmdCreateDb.ExecuteNonQuery();
    Console.WriteLine("База данных создана с поддержкой кириллицы.");
}
```

**Важно:** Мы добавили проверку `IF NOT EXISTS` чтобы избежать ошибки при повторном запуске программы.

## Создание таблицы

После создания базы данных создадим таблицу Authors:

```csharp
string connectionString = $@"Data Source={ServerName};
    Initial Catalog={DatabaseName};
    Integrated Security=SSPI;
    Pooling=false";

using (var conn = new SqlConnection(connectionString))
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

    SqlCommand cmdTable = new SqlCommand(createTableQuery, conn);
    cmdTable.ExecuteNonQuery();
    Console.WriteLine("Таблица проверена/создана.");
}
```

## Вставка данных

Используем параметризованные запросы для безопасной вставки данных:

```csharp
string insertString = @"
    INSERT INTO Authors (FirstName, LastName)
    VALUES (@FirstName, @LastName)";

SqlCommand cmdInsert = new SqlCommand(insertString, conn);

// Добавляем параметры
cmdInsert.Parameters.AddWithValue("@FirstName", "Клиффорд");
cmdInsert.Parameters.AddWithValue("@LastName", "Саймак");

int rowsAffected = cmdInsert.ExecuteNonQuery();
Console.WriteLine($"Данные успешно добавлены! Затронуто строк: {rowsAffected}");
```

## Чтение данных (SELECT)

### Простой SELECT всех записей:

```csharp
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
```

### SELECT с условием WHERE:

```csharp
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
```

## Полный код приложения

```csharp
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
```

## Заключение

В этой статье мы рассмотрели:
- Установку необходимых пакетов NuGet
- Создание базы данных с поддержкой кириллицы
- Создание таблиц
- Безопасную вставку данных с использованием параметризованных запросов
- Чтение данных с помощью SqlDataReader
- Использование блоков using для правильного управления ресурсами

Этот код обеспечивает надежное взаимодействие с базой данных и может служить основой для более сложных приложений.
