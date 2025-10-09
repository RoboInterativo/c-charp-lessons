# Подключение к MS SQL Server

Существует несколько способов подключения к MS SQL Server из приложения C#. Рассмотрим основные подходы.

## 1. Установка необходимых пакетов

Перед началом работы установите пакет через NuGet:

```
Microsoft.Data.SqlClient
```

Или через Package Manager Console:
```powershell
Install-Package Microsoft.Data.SqlClient
```

## 2. Строка подключения (Connection String)

### Основные параметры строки подключения:

```csharp
// Аутентификация Windows
string connectionString = "Data Source=сервер;Initial Catalog=база_данных;Integrated Security=true;";

// Аутентификация SQL Server
string connectionString = "Data Source=сервер;Initial Catalog=база_данных;User Id=логин;Password=пароль;";

// С дополнительными параметрами
string connectionString = "Data Source=сервер;Initial Catalog=база_данных;Integrated Security=true;TrustServerCertificate=true;Connection Timeout=30;";
```

### Параметры строки подключения:

| Параметр | Описание |
|----------|----------|
| `Data Source` | Имя сервера или IP-адрес |
| `Server` | Альтернатива Data Source |
| `Initial Catalog` | Имя базы данных |
| `Database` | Альтернатива Initial Catalog |
| `Integrated Security` | Аутентификация Windows (true/false) |
| `Trusted_Connection` | Альтернатива Integrated Security |
| `User ID` | Имя пользователя SQL Server |
| `Password` | Пароль пользователя |
| `TrustServerCertificate` | Доверять сертификату сервера |
| `Connection Timeout` | Таймаут подключения в секундах |

## 3. Примеры подключения к разным типам серверов

### Подключение к LocalDB:
```csharp
using Microsoft.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyDatabase;Integrated Security=true;TrustServerCertificate=true;";

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Подключение к LocalDB успешно!");
}
```

### Подключение к именованному экземпляру:
```csharp
string connectionString = @"Data Source=МойСервер\ИмяЭкземпляра;Initial Catalog=MyDatabase;Integrated Security=true;";
```

### Подключение к серверу по IP-адресу:
```csharp
string connectionString = @"Data Source=192.168.1.100;Initial Catalog=MyDatabase;User Id=myuser;Password=mypassword;TrustServerCertificate=true;";
```

### Подключение с указанием порта:
```csharp
string connectionString = @"Data Source=192.168.1.100,1433;Initial Catalog=MyDatabase;User Id=myuser;Password=mypassword;";
```

## 4. Полный пример приложения

```csharp
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
        return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=true;TrustServerCertificate=true;";

        // 2. SQL Server Express
        // return @"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=true;";

        // 3. Полный SQL Server с аутентификацией Windows
        // return @"Data Source=МойСервер;Initial Catalog=master;Integrated Security=true;";

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
```

## 5. Проверка подключения с обработкой ошибок

```csharp
using System;
using Microsoft.Data.SqlClient;

class DatabaseTester
{
    public static void TestConnection(string connectionString)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Устанавливаем таймаут подключения
                connection.Open();

                Console.WriteLine("✓ Подключение успешно!");
                Console.WriteLine($"Сервер: {connection.DataSource}");
                Console.WriteLine($"База данных: {connection.Database}");
                Console.WriteLine($"Версия сервера: {connection.ServerVersion}");
                Console.WriteLine($"Состояние: {connection.State}");
            }
        }
        catch (SqlException ex) when (ex.Number == 18456)
        {
            Console.WriteLine("✗ Ошибка аутентификации: неверный логин или пароль");
        }
        catch (SqlException ex) when (ex.Number == 4060)
        {
            Console.WriteLine("✗ База данных не найдена");
        }
        catch (SqlException ex) when (ex.Number == 53)
        {
            Console.WriteLine("✗ Сервер не найден или недоступен");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"✗ Ошибка SQL Server [{ex.Number}]: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Общая ошибка: {ex.Message}");
        }
    }
}
```

## 6. Построитель строк подключения

```csharp
public class ConnectionStringBuilder
{
    public static string BuildConnectionString(
        string server,
        string database,
        string username = null,
        string password = null,
        bool integratedSecurity = true,
        int timeout = 30)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = server,
            InitialCatalog = database,
            ConnectTimeout = timeout,
            TrustServerCertificate = true
        };

        if (integratedSecurity)
        {
            builder.IntegratedSecurity = true;
        }
        else
        {
            builder.UserID = username;
            builder.Password = password;
        }

        return builder.ConnectionString;
    }
}

// Использование:
string connString1 = ConnectionStringBuilder.BuildConnectionString(
    server: "(localdb)\\MSSQLLocalDB",
    database: "MyDatabase");

string connString2 = ConnectionStringBuilder.BuildConnectionString(
    server: "myserver",
    database: "MyDatabase",
    username: "sa",
    password: "password123",
    integratedSecurity: false);
```

## 7. Распространенные проблемы и решения

### Проблема: "Cannot open database requested by the login"
**Решение:** Проверьте имя базы данных в параметре `Initial Catalog`

### Проблема: "Login failed for user"
**Решение:**
- Проверьте логин/пароль
- Убедитесь, что включена аутентификация Mixed Mode
- Для Windows аутентификации используйте `Integrated Security=true`

### Проблема: "A network-related or instance-specific error"
**Решение:**
- Проверьте имя сервера
- Убедитесь, что SQL Server запущен
- Проверьте настройки брандмауэра

### Проблема: "Connection timeout"
**Решение:** Увеличьте `Connection Timeout` в строке подключения

## 8. Проверка доступности сервера

```csharp
public static bool IsServerAvailable(string serverName)
{
    try
    {
        using (var connection = new SqlConnection($"Data Source={serverName};Connection Timeout=5;"))
        {
            connection.Open();
            return true;
        }
    }
    catch
    {
        return false;
    }
}
```

Этот материал покрывает основные аспекты подключения к MS SQL Server из C# приложения. Начните с LocalDB для тестирования, затем переходите к полноценным серверам.
