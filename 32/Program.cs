using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;

// Общий интерфейс для работы с разными СУБД
public interface IDatabaseService
{
    bool TestConnection();
    void CreateStudentsTable();
    int AddStudent(string name, int age, string groupName);
    List<Student> GetAllStudents();
    Student GetStudentById(int id);
    bool UpdateStudent(int id, string name, int age, string groupName);
    bool DeleteStudent(int id);
    List<Student> GetStudentsByGroup(string groupName);
}

// Модель студента
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string GroupName { get; set; }

    public override string ToString()
    {
        return $"ID: {Id} | {Name} | {Age} лет | Группа: {GroupName}";
    }
}

// Реализация для SQLite
public class SQLiteService : IDatabaseService
{
    private readonly string _connectionString;

    public SQLiteService(string databasePath = "students.db")
    {
        _connectionString = $"Data Source={databasePath};Version=3;";
    }

    public bool TestConnection()
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка подключения к SQLite: {ex.Message}");
            return false;
        }
    }

    public void CreateStudentsTable()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Age INTEGER NOT NULL,
                    GroupName TEXT NOT NULL
                )";

            using (var command = new SQLiteCommand(createTableSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public int AddStudent(string name, int age, string groupName)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string insertSql = @"
                INSERT INTO Students (Name, Age, GroupName)
                VALUES (@name, @age, @groupName);
                SELECT last_insert_rowid();";

            using (var command = new SQLiteCommand(insertSql, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@groupName", groupName);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }

    public List<Student> GetAllStudents()
    {
        var students = new List<Student>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectSql = "SELECT * FROM Students";

            using (var command = new SQLiteCommand(selectSql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Age = reader.GetInt32("Age"),
                        GroupName = reader.GetString("GroupName")
                    });
                }
            }
        }

        return students;
    }

    public Student GetStudentById(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectSql = "SELECT * FROM Students WHERE Id = @id";

            using (var command = new SQLiteCommand(selectSql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Age = reader.GetInt32("Age"),
                            GroupName = reader.GetString("GroupName")
                        };
                    }
                }
            }
        }

        return null;
    }

    public bool UpdateStudent(int id, string name, int age, string groupName)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string updateSql = @"
                UPDATE Students 
                SET Name = @name, Age = @age, GroupName = @groupName
                WHERE Id = @id";

            using (var command = new SQLiteCommand(updateSql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@groupName", groupName);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    public bool DeleteStudent(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string deleteSql = "DELETE FROM Students WHERE Id = @id";

            using (var command = new SQLiteCommand(deleteSql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    public List<Student> GetStudentsByGroup(string groupName)
    {
        var students = new List<Student>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectSql = "SELECT * FROM Students WHERE GroupName = @groupName";

            using (var command = new SQLiteCommand(selectSql, connection))
            {
                command.Parameters.AddWithValue("@groupName", groupName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Age = reader.GetInt32("Age"),
                            GroupName = reader.GetString("GroupName")
                        });
                    }
                }
            }
        }

        return students;
    }
}

// Реализация для SQL Server
public class SqlServerService : IDatabaseService
{
    private readonly string _connectionString;

    public SqlServerService(string server = "localhost", string database = "CollegeDB")
    {
        _connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security=True;";
    }

    public bool TestConnection()
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка подключения к SQL Server: {ex.Message}");
            return false;
        }
    }

    public void CreateStudentsTable()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string createTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Students' AND xtype='U')
                CREATE TABLE Students (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Age INT NOT NULL,
                    GroupName NVARCHAR(50) NOT NULL
                )";

            using (var command = new SQLiteCommand(createTableSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public int AddStudent(string name, int age, string groupName)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string insertSql = @"
                INSERT INTO Students (Name, Age, GroupName)
                VALUES (@name, @age, @groupName);
                SELECT SCOPE_IDENTITY();";

            using (var command = new SQLiteCommand(insertSql, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@groupName", groupName);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }

    public List<Student> GetAllStudents()
    {
        var students = new List<Student>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectSql = "SELECT * FROM Students";

            using (var command = new SQLiteCommand(selectSql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Age = reader.GetInt32("Age"),
                        GroupName = reader.GetString("GroupName")
                    });
                }
            }
        }

        return students;
    }

    public Student GetStudentById(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectSql = "SELECT * FROM Students WHERE Id = @id";

            using (var command = new SQLiteCommand(selectSql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Age = reader.GetInt32("Age"),
                            GroupName = reader.GetString("GroupName")
                        };
                    }
                }
            }
        }

        return null;
    }

    public bool UpdateStudent(int id, string name, int age, string groupName)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string updateSql = @"
                UPDATE Students 
                SET Name = @name, Age = @age, GroupName = @groupName
                WHERE Id = @id";

            using (var command = new SQLiteCommand(updateSql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@groupName", groupName);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    public bool DeleteStudent(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string deleteSql = "DELETE FROM Students WHERE Id = @id";

            using (var command = new SQLiteCommand(deleteSql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    public List<Student> GetStudentsByGroup(string groupName)
    {
        var students = new List<Student>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string selectSql = "SELECT * FROM Students WHERE GroupName = @groupName";

            using (var command = new SQLiteCommand(selectSql, connection))
            {
                command.Parameters.AddWithValue("@groupName", groupName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Age = reader.GetInt32("Age"),
                            GroupName = reader.GetString("GroupName")
                        });
                    }
                }
            }
        }

        return students;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== DATABASE SWITCHER - РАБОТА С РАЗНЫМИ СУБД ===\n");

        // Задание 1: Подключение к SQLite
        DemonstrateSQLite();

        // Задание 2: Подключение к SQL Server
        DemonstrateSQLServer();

        // Задание 3: Проверка подключения
        TestConnections();

        // Задание 6: Продвинутое - Database Switcher
        DatabaseSwitcher();

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }

    // Задание 1: Подключение к SQLite
    static void DemonstrateSQLite()
    {
        Console.WriteLine("1. ПОДКЛЮЧЕНИЕ К SQLite\n");

        try
        {
            var sqliteService = new SQLiteService();

            if (sqliteService.TestConnection())
            {
                Console.WriteLine("✅ Подключение к SQLite успешно установлено");

                // Создаем таблицу
                sqliteService.CreateStudentsTable();
                Console.WriteLine("✅ Таблица Students создана или уже существует");

                // Добавляем тестовые данные
                sqliteService.AddStudent("Иванов Иван", 18, "П-21");
                sqliteService.AddStudent("Петров Петр", 19, "П-22");
                sqliteService.AddStudent("Сидорова Анна", 18, "П-21");

                Console.WriteLine("✅ Тестовые данные добавлены");

                // Показываем данные
                DisplayStudents("SQLite", sqliteService.GetAllStudents());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при работе с SQLite: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 2: Подключение к SQL Server
    static void DemonstrateSQLServer()
    {
        Console.WriteLine("2. ПОДКЛЮЧЕНИЕ К SQL SERVER\n");

        try
        {
            var sqlServerService = new SqlServerService();

            if (sqlServerService.TestConnection())
            {
                Console.WriteLine("✅ Подключение к SQL Server успешно установлено");

                // Создаем таблицу
                sqlServerService.CreateStudentsTable();
                Console.WriteLine("✅ Таблица Students создана или уже существует");

                // Добавляем тестовые данные
                sqlServerService.AddStudent("Иванов Иван", 18, "П-21");
                sqlServerService.AddStudent("Петров Петр", 19, "П-22");
                sqlServerService.AddStudent("Сидорова Анна", 18, "П-21");

                Console.WriteLine("✅ Тестовые данные добавлены");

                // Показываем данные
                DisplayStudents("SQL Server", sqlServerService.GetAllStudents());
            }
            else
            {
                Console.WriteLine("ℹ️ SQL Server недоступен. Проверьте подключение к базе данных.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при работе с SQL Server: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 3: Проверка подключения
    static void TestConnections()
    {
        Console.WriteLine("3. ПРОВЕРКА ПОДКЛЮЧЕНИЙ К БАЗАМ ДАННЫХ\n");

        Console.WriteLine("=== ПРОВЕРКА SQLite ===");
        var sqliteService = new SQLiteService();
        if (sqliteService.TestConnection())
        {
            Console.WriteLine("✅ SQLite: Подключение успешно");
        }
        else
        {
            Console.WriteLine("❌ SQLite: Ошибка подключения");
        }

        Console.WriteLine("\n=== ПРОВЕРКА SQL SERVER ===");
        var sqlServerService = new SqlServerService();
        if (sqlServerService.TestConnection())
        {
            Console.WriteLine("✅ SQL Server: Подключение успешно");
        }
        else
        {
            Console.WriteLine("❌ SQL Server: Ошибка подключения");
        }

        Console.WriteLine();
    }

    // Задание 6: Продвинутое - Database Switcher
    static void DatabaseSwitcher()
    {
        Console.WriteLine("6. DATABASE SWITCHER - ВЫБОР СУБД\n");

        IDatabaseService databaseService = null;

        while (databaseService == null)
        {
            Console.WriteLine("Выберите СУБД для работы:");
            Console.WriteLine("1. SQLite (локальная база данных)");
            Console.WriteLine("2. SQL Server (серверная база данных)");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    databaseService = new SQLiteService();
                    Console.WriteLine("✅ Выбрана SQLite");
                    break;
                case "2":
                    databaseService = new SqlServerService();
                    Console.WriteLine("✅ Выбрана SQL Server");
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор. Попробуйте снова.\n");
                    break;
            }
        }

        // Проверяем подключение
        if (!databaseService.TestConnection())
        {
            Console.WriteLine("❌ Не удалось подключиться к выбранной СУБД");
            return;
        }

        // Создаем таблицу
        databaseService.CreateStudentsTable();
        Console.WriteLine("✅ Таблица Students готова к работе");

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== DATABASE SWITCHER - УПРАВЛЕНИЕ СТУДЕНТАМИ ===");
            Console.WriteLine($"СУБД: {(databaseService is SQLiteService ? "SQLite" : "SQL Server")}");
            Console.WriteLine(new string('=', 50));

            DisplayStudentsTable(databaseService.GetAllStudents());

            Console.WriteLine("\n=== МЕНЮ ОПЕРАЦИЙ ===");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Найти студента по ID");
            Console.WriteLine("3. Обновить данные студента");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Показать студентов по группе");
            Console.WriteLine("6. Показать всех студентов");
            Console.WriteLine("7. Сменить СУБД");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent(databaseService);
                    break;
                case "2":
                    FindStudentById(databaseService);
                    break;
                case "3":
                    UpdateStudent(databaseService);
                    break;
                case "4":
                    DeleteStudent(databaseService);
                    break;
                case "5":
                    ShowStudentsByGroup(databaseService);
                    break;
                case "6":
                    ShowAllStudents(databaseService);
                    break;
                case "7":
                    // Возвращаемся к выбору СУБД
                    return;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор! Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Методы для Database Switcher

    static void AddStudent(IDatabaseService service)
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ СТУДЕНТА ===");

        Console.Write("Имя: ");
        string name = Console.ReadLine();

        Console.Write("Возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("❌ Неверный формат возраста!");
            Console.ReadKey();
            return;
        }

        Console.Write("Группа: ");
        string group = Console.ReadLine();

        try
        {
            int id = service.AddStudent(name, age, group);
            Console.WriteLine($"✅ Студент добавлен с ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при добавлении: {ex.Message}");
        }

        Console.ReadKey();
    }

    static void FindStudentById(IDatabaseService service)
    {
        Console.WriteLine("\n=== ПОИСК СТУДЕНТА ПО ID ===");

        Console.Write("Введите ID студента: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ Неверный формат ID!");
            Console.ReadKey();
            return;
        }

        try
        {
            var student = service.GetStudentById(id);
            if (student != null)
            {
                Console.WriteLine($"\n📋 НАЙДЕН СТУДЕНТ:");
                Console.WriteLine(student.ToString());
            }
            else
            {
                Console.WriteLine("❌ Студент с указанным ID не найден");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при поиске: {ex.Message}");
        }

        Console.ReadKey();
    }

    static void UpdateStudent(IDatabaseService service)
    {
        Console.WriteLine("\n=== ОБНОВЛЕНИЕ ДАННЫХ СТУДЕНТА ===");

        Console.Write("Введите ID студента для обновления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ Неверный формат ID!");
            Console.ReadKey();
            return;
        }

        // Сначала находим студента
        var existingStudent = service.GetStudentById(id);
        if (existingStudent == null)
        {
            Console.WriteLine("❌ Студент с указанным ID не найден");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nТекущие данные: {existingStudent}");
        Console.WriteLine("\nВведите новые данные (оставьте пустым для сохранения текущего значения):");

        Console.Write($"Имя [{existingStudent.Name}]: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
            name = existingStudent.Name;

        Console.Write($"Возраст [{existingStudent.Age}]: ");
        string ageInput = Console.ReadLine();
        int age = string.IsNullOrWhiteSpace(ageInput) ? existingStudent.Age : int.Parse(ageInput);

        Console.Write($"Группа [{existingStudent.GroupName}]: ");
        string group = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(group))
            group = existingStudent.GroupName;

        try
        {
            bool success = service.UpdateStudent(id, name, age, group);
            if (success)
            {
                Console.WriteLine("✅ Данные студента обновлены");
            }
            else
            {
                Console.WriteLine("❌ Не удалось обновить данные студента");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при обновлении: {ex.Message}");
        }

        Console.ReadKey();
    }

    static void DeleteStudent(IDatabaseService service)
    {
        Console.WriteLine("\n=== УДАЛЕНИЕ СТУДЕНТА ===");

        Console.Write("Введите ID студента для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ Неверный формат ID!");
            Console.ReadKey();
            return;
        }

        // Сначала находим студента
        var existingStudent = service.GetStudentById(id);
        if (existingStudent == null)
        {
            Console.WriteLine("❌ Студент с указанным ID не найден");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nВы действительно хотите удалить студента: {existingStudent}");
        Console.Write("Подтвердите удаление (y/n): ");
        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "y")
        {
            try
            {
                bool success = service.DeleteStudent(id);
                if (success)
                {
                    Console.WriteLine("✅ Студент удален");
                }
                else
                {
                    Console.WriteLine("❌ Не удалось удалить студента");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при удалении: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("❌ Удаление отменено");
        }

        Console.ReadKey();
    }

    static void ShowStudentsByGroup(IDatabaseService service)
    {
        Console.WriteLine("\n=== СТУДЕНТЫ ПО ГРУППЕ ===");

        Console.Write("Введите название группы: ");
        string group = Console.ReadLine();

        try
        {
            var students = service.GetStudentsByGroup(group);
            if (students.Count > 0)
            {
                Console.WriteLine($"\n📋 СТУДЕНТЫ ГРУППЫ '{group}':");
                DisplayStudentsTable(students);
            }
            else
            {
                Console.WriteLine($"❌ В группе '{group}' нет студентов");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при поиске: {ex.Message}");
        }

        Console.ReadKey();
    }

    static void ShowAllStudents(IDatabaseService service)
    {
        Console.WriteLine("\n=== ВСЕ СТУДЕНТЫ ===");

        try
        {
            var students = service.GetAllStudents();
            DisplayStudentsTable(students);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при загрузке данных: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Вспомогательные методы для отображения

    static void DisplayStudents(string databaseType, List<Student> students)
    {
        Console.WriteLine($"📋 СТУДЕНТЫ В {databaseType}:");
        Console.WriteLine(new string('=', 50));

        if (students.Count > 0)
        {
            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }
        }
        else
        {
            Console.WriteLine("Список студентов пуст");
        }

        Console.WriteLine(new string('=', 50));
    }

    static void DisplayStudentsTable(List<Student> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст");
            return;
        }

        Console.WriteLine("ID  Имя                | Возраст | Группа");
        Console.WriteLine(new string('-', 45));

        foreach (var student in students)
        {
            Console.WriteLine($"{student.Id,-3} {student.Name,-18} | {student.Age,-7} | {student.GroupName}");
        }

        Console.WriteLine($"\nВсего студентов: {students.Count}");
    }
}