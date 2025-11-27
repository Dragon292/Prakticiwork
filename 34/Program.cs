using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;

// 1. Создание модели данных
public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(16, 100)]
    public int Age { get; set; }

    [Required]
    [StringLength(20)]
    public string GroupName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return $"{Id}. {Name} | {Age} лет | {GroupName}";
    }
}

// 1. Создание контекста базы данных
public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Используем SQLite базу данных
        optionsBuilder.UseSqlite("Data Source=students.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Добавляем начальные данные
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "Иванов Иван", Age = 18, GroupName = "П-21" },
            new Student { Id = 2, Name = "Петров Петр", Age = 19, GroupName = "П-22" },
            new Student { Id = 3, Name = "Сидорова Мария", Age = 20, GroupName = "П-21" }
        );
    }
}

// 6. Сервис для работы со студентами
public class StudentService
{
    private readonly AppDbContext _context;

    public StudentService()
    {
        _context = new AppDbContext();
    }

    /// <summary>
    /// Добавление нового студента
    /// </summary>
    public void Add(Student student)
    {
        try
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            Console.WriteLine("✅ Студент успешно добавлен!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при добавлении: {ex.Message}");
        }
    }

    /// <summary>
    /// Обновление возраста студента
    /// </summary>
    public void Update(int id, int newAge)
    {
        try
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                student.Age = newAge;
                _context.SaveChanges();
                Console.WriteLine("✅ Возраст успешно обновлен!");
            }
            else
            {
                Console.WriteLine("❌ Студент с указанным ID не найден");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при обновлении: {ex.Message}");
        }
    }

    /// <summary>
    /// Обновление возраста студента по имени
    /// </summary>
    public void UpdateAgeByName(string name, int newAge)
    {
        try
        {
            var student = _context.Students.FirstOrDefault(s => s.Name == name);
            if (student != null)
            {
                student.Age = newAge;
                _context.SaveChanges();
                Console.WriteLine($"✅ Возраст студента {name} успешно обновлен на {newAge}!");
            }
            else
            {
                Console.WriteLine("❌ Студент с указанным именем не найден");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при обновлении: {ex.Message}");
        }
    }

    /// <summary>
    /// Удаление студента по ID
    /// </summary>
    public void Delete(int id)
    {
        try
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                Console.WriteLine("✅ Студент успешно удален!");
            }
            else
            {
                Console.WriteLine("❌ Студент с указанным ID не найден");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при удалении: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение всех студентов
    /// </summary>
    public List<Student> GetAll()
    {
        return _context.Students.OrderBy(s => s.Id).ToList();
    }

    /// <summary>
    /// Поиск студента по ID
    /// </summary>
    public Student GetById(int id)
    {
        return _context.Students.FirstOrDefault(s => s.Id == id);
    }

    /// <summary>
    /// Получение количества студентов
    /// </summary>
    public int GetCount()
    {
        return _context.Students.Count();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

// Класс для инициализации базы данных
public static class DatabaseInitializer
{
    /// <summary>
    /// 2. Создание и инициализация базы данных
    /// </summary>
    public static void Initialize()
    {
        using var context = new AppDbContext();

        // Автоматическое создание базы данных и таблиц
        context.Database.EnsureCreated();

        Console.WriteLine("✅ База данных инициализирована");
        Console.WriteLine($"📍 Файл базы данных: students.db");
    }
}

// Главное приложение
public class EFStudentManager
{
    private readonly StudentService _studentService;

    public EFStudentManager()
    {
        _studentService = new StudentService();
    }

    /// <summary>
    /// Главное меню приложения
    /// </summary>
    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 2. Инициализация базы данных
        DatabaseInitializer.Initialize();

        Console.WriteLine("\n🎓 EF STUDENT MANAGER");
        Console.WriteLine("=====================\n");

        while (true)
        {
            ShowMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllStudents();
                    break;
                case "2":
                    AddNewStudent();
                    break;
                case "3":
                    UpdateStudentAge();
                    break;
                case "4":
                    DeleteStudent();
                    break;
                case "5":
                    Console.WriteLine("👋 До свидания!");
                    return;
                default:
                    Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("ГЛАВНОЕ МЕНЮ:");
        Console.WriteLine("1. Показать всех студентов");
        Console.WriteLine("2. Добавить нового студента");
        Console.WriteLine("3. Изменить возраст студента");
        Console.WriteLine("4. Удалить студента");
        Console.WriteLine("5. Выйти");
        Console.Write("Выберите действие: ");
    }

    /// <summary>
    /// 4. Чтение данных из таблицы - Показать всех студентов
    /// </summary>
    private void ShowAllStudents()
    {
        Console.WriteLine("\n📋 СПИСОК ВСЕХ СТУДЕНТОВ");
        Console.WriteLine("========================");

        var students = _studentService.GetAll();

        if (students.Count == 0)
        {
            Console.WriteLine("Студенты не найдены");
            return;
        }

        Console.WriteLine("ID | Имя студента      | Возраст | Группа | Дата добавления");
        Console.WriteLine(new string('-', 65));

        foreach (var student in students)
        {
            Console.WriteLine($"{student.Id,-2} | {student.Name,-16} | {student.Age,-6} | {student.GroupName,-6} | {student.CreatedAt:dd.MM.yyyy}");
        }

        Console.WriteLine($"\nВсего студентов: {students.Count}");
    }

    /// <summary>
    /// 3. Добавление данных в таблицу - Добавить нового студента
    /// </summary>
    private void AddNewStudent()
    {
        Console.WriteLine("\n➕ ДОБАВЛЕНИЕ НОВОГО СТУДЕНТА");
        Console.WriteLine("=============================");

        Console.Write("Введите ФИО студента: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Имя не может быть пустым");
            return;
        }

        Console.Write("Введите возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 16 || age > 100)
        {
            Console.WriteLine("❌ Некорректный возраст (должен быть от 16 до 100)");
            return;
        }

        Console.Write("Введите название группы: ");
        var group = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(group))
        {
            Console.WriteLine("❌ Группа не может быть пустой");
            return;
        }

        var student = new Student
        {
            Name = name.Trim(),
            Age = age,
            GroupName = group.Trim()
        };

        _studentService.Add(student);
    }

    /// <summary>
    /// 5. Обновление записи - Изменить возраст студента
    /// </summary>
    private void UpdateStudentAge()
    {
        Console.WriteLine("\n✏️ ИЗМЕНЕНИЕ ВОЗРАСТА СТУДЕНТА");
        Console.WriteLine("==============================");

        Console.WriteLine("Выберите способ поиска:");
        Console.WriteLine("1 - По ID");
        Console.WriteLine("2 - По имени");
        Console.Write("Ваш выбор: ");

        var searchChoice = Console.ReadLine();

        if (searchChoice == "1")
        {
            // Поиск по ID
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Некорректный ID");
                return;
            }

            var student = _studentService.GetById(id);
            if (student == null)
            {
                Console.WriteLine("❌ Студент с указанным ID не найден");
                return;
            }

            Console.WriteLine($"Найден студент: {student}");
            Console.Write("Введите новый возраст: ");

            if (!int.TryParse(Console.ReadLine(), out int newAge) || newAge < 16 || newAge > 100)
            {
                Console.WriteLine("❌ Некорректный возраст");
                return;
            }

            _studentService.Update(id, newAge);
        }
        else if (searchChoice == "2")
        {
            // Поиск по имени
            Console.Write("Введите имя студента: ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("❌ Имя не может быть пустым");
                return;
            }

            Console.Write("Введите новый возраст: ");

            if (!int.TryParse(Console.ReadLine(), out int newAge) || newAge < 16 || newAge > 100)
            {
                Console.WriteLine("❌ Некорректный возраст");
                return;
            }

            _studentService.UpdateAgeByName(name.Trim(), newAge);
        }
        else
        {
            Console.WriteLine("❌ Неверный выбор");
        }
    }

    /// <summary>
    /// 5. Удаление записи - Удалить студента
    /// </summary>
    private void DeleteStudent()
    {
        Console.WriteLine("\n🗑️ УДАЛЕНИЕ СТУДЕНТА");
        Console.WriteLine("===================");

        // Сначала показываем всех студентов для удобства
        var students = _studentService.GetAll();
        if (students.Count == 0)
        {
            Console.WriteLine("❌ В базе нет студентов для удаления");
            return;
        }

        Console.WriteLine("Текущий список студентов:");
        foreach (var student in students)
        {
            Console.WriteLine($"  {student}");
        }

        Console.Write("\nВведите ID студента для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ Некорректный ID");
            return;
        }

        var studentToDelete = _studentService.GetById(id);
        if (studentToDelete == null)
        {
            Console.WriteLine("❌ Студент с указанным ID не найден");
            return;
        }

        Console.WriteLine($"Вы действительно хотите удалить студента: {studentToDelete}?");
        Console.Write("Подтвердите удаление (y/n): ");

        var confirmation = Console.ReadLine()?.ToLower();
        if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
        {
            _studentService.Delete(id);
        }
        else
        {
            Console.WriteLine("❌ Удаление отменено");
        }
    }
}

// Демонстрационный класс для базовых операций
public static class BasicEFOperations
{
    /// <summary>
    /// Демонстрация основных операций EF Core
    /// </summary>
    public static void DemonstrateBasicOperations()
    {
        Console.WriteLine("🔍 ДЕМОНСТРАЦИЯ ОСНОВНЫХ ОПЕРАЦИЙ EF CORE");
        Console.WriteLine("=========================================\n");

        using var context = new AppDbContext();

        // 2. Создание базы данных
        context.Database.EnsureCreated();
        Console.WriteLine("✅ База данных создана\n");

        // 3. Добавление данных
        Console.WriteLine("➕ ДОБАВЛЕНИЕ ДАННЫХ:");
        if (!context.Students.Any())
        {
            // Добавляем тестовые данные, если таблица пуста
            var newStudents = new List<Student>
            {
                new Student { Name = "Иванов Иван", Age = 18, GroupName = "П-21" },
                new Student { Name = "Петров Петр", Age = 19, GroupName = "П-22" },
                new Student { Name = "Сидорова Мария", Age = 20, GroupName = "П-21" },
                new Student { Name = "Козлов Алексей", Age = 18, GroupName = "П-23" }
            };

            context.Students.AddRange(newStudents);
            context.SaveChanges();
            Console.WriteLine("✅ Данные успешно добавлены!\n");
        }
        else
        {
            Console.WriteLine("✅ В базе уже есть данные\n");
        }

        // 4. Чтение данных
        Console.WriteLine("📖 ЧТЕНИЕ ДАННЫХ:");
        var students = context.Students.ToList();
        Console.WriteLine("Все студенты:");
        foreach (var student in students)
        {
            Console.WriteLine($"  {student}");
        }
        Console.WriteLine();

        // 5. Обновление данных
        Console.WriteLine("✏️ ОБНОВЛЕНИЕ ДАННЫХ:");
        var studentToUpdate = context.Students.FirstOrDefault(s => s.Name == "Иванов Иван");
        if (studentToUpdate != null)
        {
            Console.WriteLine($"Обновление возраста студента {studentToUpdate.Name}");
            studentToUpdate.Age = 20;
            context.SaveChanges();
            Console.WriteLine("✅ Возраст успешно обновлен!\n");
        }

        // 5. Удаление данных
        Console.WriteLine("🗑️ УДАЛЕНИЕ ДАННЫХ:");
        var studentToDelete = context.Students.FirstOrDefault(s => s.Name == "Козлов Алексей");
        if (studentToDelete != null)
        {
            Console.WriteLine($"Удаление студента {studentToDelete.Name}");
            context.Students.Remove(studentToDelete);
            context.SaveChanges();
            Console.WriteLine("✅ Студент успешно удален!\n");
        }

        // Финальный список
        Console.WriteLine("📋 ФИНАЛЬНЫЙ СПИСОК СТУДЕНТОВ:");
        var finalStudents = context.Students.ToList();
        foreach (var student in finalStudents)
        {
            Console.WriteLine($"  {student}");
        }
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
            if (args.Length > 0 && args[0] == "demo")
            {
                // Запуск демонстрации базовых операций
                BasicEFOperations.DemonstrateBasicOperations();
            }
            else
            {
                // Запуск полноценного приложения
                var app = new EFStudentManager();
                app.Run();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Произошла ошибка: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}