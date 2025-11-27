using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

// Класс для представления студента
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string GroupName { get; set; }

    public override string ToString()
    {
        return $"{Name} | {Age} | {GroupName}";
    }
}

// Главный класс для работы с базой данных
public class DatabaseManager
{
    private string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    /// <summary>
    /// Инициализация базы данных и создание таблицы, если она не существует
    /// </summary>
    private void InitializeDatabase()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Age INTEGER NOT NULL,
                GroupName TEXT NOT NULL
            )";

        using var command = new SQLiteCommand(createTableQuery, connection);
        command.ExecuteNonQuery();

        // Добавляем тестовые данные, если таблица пуста
        var checkDataQuery = "SELECT COUNT(*) FROM Students";
        using var checkCommand = new SQLiteCommand(checkDataQuery, connection);
        var count = Convert.ToInt64(checkCommand.ExecuteScalar());

        if (count == 0)
        {
            var insertTestDataQuery = @"
                INSERT INTO Students (Name, Age, GroupName) VALUES 
                ('Иванов Иван', 18, 'П-21'),
                ('Петров Петр', 19, 'П-22'),
                ('Сидорова Мария', 20, 'П-21'),
                ('Козлов Алексей', 18, 'П-23')";

            using var insertCommand = new SQLiteCommand(insertTestDataQuery, connection);
            insertCommand.ExecuteNonQuery();
            Console.WriteLine("Добавлены тестовые данные");
        }
    }

    /// <summary>
    /// Получение всех студентов из базы данных
    /// </summary>
    public List<Student> GetAllStudents()
    {
        var students = new List<Student>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "SELECT Id, Name, Age, GroupName FROM Students";
        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

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

        return students;
    }

    /// <summary>
    /// Получение студентов по группе
    /// </summary>
    public List<Student> GetByGroup(string group)
    {
        var students = new List<Student>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "SELECT Id, Name, Age, GroupName FROM Students WHERE GroupName = @group";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@group", group);

        using var reader = command.ExecuteReader();

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

        return students;
    }

    /// <summary>
    /// Получение студентов, отсортированных по возрасту
    /// </summary>
    public List<Student> GetStudentsSortedByAge(bool ascending = true)
    {
        var students = new List<Student>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = "SELECT Id, Name, Age, GroupName FROM Students ORDER BY Age " +
                      (ascending ? "ASC" : "DESC");

        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

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

        return students;
    }

    /// <summary>
    /// Добавление нового студента в базу данных
    /// Используются параметризованные запросы для защиты от SQL-инъекций
    /// </summary>
    public void AddStudent(Student student)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "INSERT INTO Students (Name, Age, GroupName) VALUES (@name, @age, @group)";
        using var command = new SQLiteCommand(query, connection);

        // Параметризованный запрос для безопасности
        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@age", student.Age);
        command.Parameters.AddWithValue("@group", student.GroupName);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine("✅ Студент успешно добавлен");
        }
        else
        {
            Console.WriteLine("❌ Ошибка при добавлении студента");
        }
    }

    /// <summary>
    /// Обновление данных студента
    /// </summary>
    public void UpdateStudent(Student student)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "UPDATE Students SET Age = @age, GroupName = @group WHERE Name = @name";
        using var command = new SQLiteCommand(query, connection);

        // Параметризованный запрос
        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@age", student.Age);
        command.Parameters.AddWithValue("@group", student.GroupName);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine("✅ Данные студента успешно обновлены");
        }
        else
        {
            Console.WriteLine("❌ Студент с таким именем не найден");
        }
    }

    /// <summary>
    /// Удаление студента по имени
    /// </summary>
    public void DeleteStudent(string name)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "DELETE FROM Students WHERE Name = @name";
        using var command = new SQLiteCommand(query, connection);

        command.Parameters.AddWithValue("@name", name);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine("✅ Студент удалён");
        }
        else
        {
            Console.WriteLine("❌ Студент с таким именем не найден");
        }
    }

    /// <summary>
    /// Поиск студента по имени
    /// </summary>
    public Student FindStudentByName(string name)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        const string query = "SELECT Id, Name, Age, GroupName FROM Students WHERE Name = @name";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@name", name);

        using var reader = command.ExecuteReader();

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

        return null;
    }
}

// Класс для вывода данных в табличном виде
public class TablePrinter
{
    /// <summary>
    /// Вывод списка студентов в виде таблицы
    /// </summary>
    public static void PrintStudents(List<Student> students, string title = "СПИСОК СТУДЕНТОВ")
    {
        if (students == null || students.Count == 0)
        {
            Console.WriteLine("Студенты не найдены");
            return;
        }

        Console.WriteLine($"\n{title}");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("Имя            | Возраст | Группа");
        Console.WriteLine(new string('-', 50));

        foreach (var student in students)
        {
            Console.WriteLine($"{student.Name,-15} | {student.Age,-7} | {student.GroupName}");
        }

        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"Всего студентов: {students.Count}\n");
    }
}

// Главное приложение
public class StudentDBManager
{
    private DatabaseManager _dbManager;

    public StudentDBManager()
    {
        // Подключаемся к SQLite базе данных
        _dbManager = new DatabaseManager("Data Source=students.db;Version=3;");
    }

    /// <summary>
    /// Главное меню приложения
    /// </summary>
    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("🎓 STUDENT DB MANAGER");
            Console.WriteLine("======================");
            Console.WriteLine("1. Показать всех студентов");
            Console.WriteLine("2. Показать студентов по группе");
            Console.WriteLine("3. Показать студентов (сортировка по возрасту)");
            Console.WriteLine("4. Добавить студента");
            Console.WriteLine("5. Изменить данные студента");
            Console.WriteLine("6. Удалить студента");
            Console.WriteLine("7. Найти студента по имени");
            Console.WriteLine("0. Выход");
            Console.WriteLine("======================");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllStudents();
                    break;
                case "2":
                    ShowStudentsByGroup();
                    break;
                case "3":
                    ShowStudentsSortedByAge();
                    break;
                case "4":
                    AddStudent();
                    break;
                case "5":
                    UpdateStudent();
                    break;
                case "6":
                    DeleteStudent();
                    break;
                case "7":
                    FindStudent();
                    break;
                case "0":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("❌ Неверный выбор. Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// 1. Показать всех студентов
    /// </summary>
    private void ShowAllStudents()
    {
        var students = _dbManager.GetAllStudents();
        TablePrinter.PrintStudents(students);
        WaitForKey();
    }

    /// <summary>
    /// 2. Показать студентов по группе
    /// </summary>
    private void ShowStudentsByGroup()
    {
        Console.Write("Введите название группы: ");
        var group = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(group))
        {
            Console.WriteLine("❌ Название группы не может быть пустым");
            WaitForKey();
            return;
        }

        var students = _dbManager.GetByGroup(group);
        TablePrinter.PrintStudents(students, $"СТУДЕНТЫ ГРУППЫ {group}");
        WaitForKey();
    }

    /// <summary>
    /// 3. Показать студентов с сортировкой по возрасту
    /// </summary>
    private void ShowStudentsSortedByAge()
    {
        Console.WriteLine("Сортировка по возрасту:");
        Console.WriteLine("1 - По возрастанию");
        Console.WriteLine("2 - По убыванию");
        Console.Write("Выберите вариант: ");

        var choice = Console.ReadLine();
        var students = _dbManager.GetStudentsSortedByAge(choice == "1");

        TablePrinter.PrintStudents(students,
            choice == "1" ? "СТУДЕНТЫ (по возрастанию возраста)" : "СТУДЕНТЫ (по убыванию возраста)");
        WaitForKey();
    }

    /// <summary>
    /// 4. Добавление нового студента
    /// </summary>
    private void AddStudent()
    {
        Console.WriteLine("ДОБАВЛЕНИЕ НОВОГО СТУДЕНТА");
        Console.WriteLine("==========================");

        Console.Write("Введите ФИО студента: ");
        var name = Console.ReadLine();

        Console.Write("Введите возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 16 || age > 100)
        {
            Console.WriteLine("❌ Некорректный возраст");
            WaitForKey();
            return;
        }

        Console.Write("Введите название группы: ");
        var group = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(group))
        {
            Console.WriteLine("❌ Все поля должны быть заполнены");
            WaitForKey();
            return;
        }

        var student = new Student { Name = name, Age = age, GroupName = group };
        _dbManager.AddStudent(student);
        WaitForKey();
    }

    /// <summary>
    /// 5. Обновление данных студента
    /// </summary>
    private void UpdateStudent()
    {
        Console.WriteLine("ИЗМЕНЕНИЕ ДАННЫХ СТУДЕНТА");
        Console.WriteLine("=========================");

        Console.Write("Введите ФИО студента для изменения: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Имя не может быть пустым");
            WaitForKey();
            return;
        }

        // Проверяем, существует ли студент
        var existingStudent = _dbManager.FindStudentByName(name);
        if (existingStudent == null)
        {
            Console.WriteLine("❌ Студент с таким именем не найден");
            WaitForKey();
            return;
        }

        Console.WriteLine($"Текущие данные: {existingStudent}");

        Console.Write("Введите новый возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int newAge) || newAge < 16 || newAge > 100)
        {
            Console.WriteLine("❌ Некорректный возраст");
            WaitForKey();
            return;
        }

        Console.Write("Введите новую группу: ");
        var newGroup = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newGroup))
        {
            Console.WriteLine("❌ Группа не может быть пустой");
            WaitForKey();
            return;
        }

        var updatedStudent = new Student
        {
            Name = name,
            Age = newAge,
            GroupName = newGroup
        };

        _dbManager.UpdateStudent(updatedStudent);
        WaitForKey();
    }

    /// <summary>
    /// 6. Удаление студента
    /// </summary>
    private void DeleteStudent()
    {
        Console.WriteLine("УДАЛЕНИЕ СТУДЕНТА");
        Console.WriteLine("=================");

        Console.Write("Введите ФИО студента для удаления: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Имя не может быть пустым");
            WaitForKey();
            return;
        }

        // Подтверждение удаления
        var student = _dbManager.FindStudentByName(name);
        if (student == null)
        {
            Console.WriteLine("❌ Студент с таким именем не найден");
            WaitForKey();
            return;
        }

        Console.WriteLine($"Найден студент: {student}");
        Console.Write("Вы уверены, что хотите удалить этого студента? (y/n): ");
        var confirmation = Console.ReadLine()?.ToLower();

        if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
        {
            _dbManager.DeleteStudent(name);
        }
        else
        {
            Console.WriteLine("❌ Удаление отменено");
        }

        WaitForKey();
    }

    /// <summary>
    /// 7. Поиск студента по имени
    /// </summary>
    private void FindStudent()
    {
        Console.WriteLine("ПОИСК СТУДЕНТА");
        Console.WriteLine("==============");

        Console.Write("Введите ФИО студента: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Имя не может быть пустым");
            WaitForKey();
            return;
        }

        var student = _dbManager.FindStudentByName(name);
        if (student != null)
        {
            Console.WriteLine("\n✅ Студент найден:");
            Console.WriteLine($"ФИО: {student.Name}");
            Console.WriteLine($"Возраст: {student.Age}");
            Console.WriteLine($"Группа: {student.GroupName}");
        }
        else
        {
            Console.WriteLine("❌ Студент с таким именем не найден");
        }

        WaitForKey();
    }

    /// <summary>
    /// Ожидание нажатия клавиши для возврата в меню
    /// </summary>
    private void WaitForKey()
    {
        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }
}

// Главный класс программы
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            var app = new StudentDBManager();
            app.ShowMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Произошла ошибка: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}

