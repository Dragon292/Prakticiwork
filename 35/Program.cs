using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

// 1. Модели данных (расширенные для продвинутого задания)
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

    // 6. Внешний ключ для связи с Faculty
    public int FacultyId { get; set; }

    // 6. Навигационное свойство
    public virtual Faculty Faculty { get; set; }

    // 6. Навигационное свойство для оценок
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public override string ToString()
    {
        return $"{Id}. {Name} | {Age} лет | {GroupName}";
    }
}

// 6. Новые классы для связанных таблиц
public class Faculty
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(10)]
    public string Code { get; set; }

    // Навигационное свойство
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}

public class Subject
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    // Навигационное свойство
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public override string ToString()
    {
        return Name;
    }
}

public class Grade
{
    public int Id { get; set; }

    [Range(1, 5)]
    public int Score { get; set; }

    public DateTime Date { get; set; }

    // Внешние ключи
    public int StudentId { get; set; }
    public int SubjectId { get; set; }

    // Навигационные свойства
    public virtual Student Student { get; set; }
    public virtual Subject Subject { get; set; }

    public override string ToString()
    {
        return $"{Score} по {Subject?.Name} ({Date:dd.MM.yyyy})";
    }
}

// Контекст базы данных
public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public Microsoft.EntityFrameworkCore.DbSet<Student> Students { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Faculty> Faculties { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Subject> Subjects { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Grade> Grades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=students.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 6. Конфигурация связей
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Faculty)
            .WithMany(f => f.Students)
            .HasForeignKey(s => s.FacultyId);

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId);

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Subject)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.SubjectId);

        // Добавляем начальные данные
        modelBuilder.Entity<Faculty>().HasData(
            new Faculty { Id = 1, Name = "Информационные системы", Code = "ИС" },
            new Faculty { Id = 2, Name = "Программная инженерия", Code = "ПИ" },
            new Faculty { Id = 3, Name = "Прикладная информатика", Code = "ПИН" }
        );

        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Name = "Математика" },
            new Subject { Id = 2, Name = "Программирование" },
            new Subject { Id = 3, Name = "Базы данных" },
            new Subject { Id = 4, Name = "Алгоритмы" }
        );

        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "Иванов Иван", Age = 18, GroupName = "ИС101", FacultyId = 1 },
            new Student { Id = 2, Name = "Петров Петр", Age = 19, GroupName = "ИС101", FacultyId = 1 },
            new Student { Id = 3, Name = "Сидорова Мария", Age = 20, GroupName = "ПИ201", FacultyId = 2 },
            new Student { Id = 4, Name = "Козлов Алексей", Age = 18, GroupName = "ПИН301", FacultyId = 3 },
            new Student { Id = 5, Name = "Смирнова Анна", Age = 21, GroupName = "ИС102", FacultyId = 1 }
        );
    }
}

// Класс для демонстрации LINQ запросов
public class LinqDemo
{
    private readonly AppDbContext _context;

    public LinqDemo()
    {
        _context = new AppDbContext();
        InitializeDatabase();
    }

    /// <summary>
    /// 1. Подготовка проекта - инициализация базы данных
    /// </summary>
    private void InitializeDatabase()
    {
        var created = _context.Database.EnsureCreated();

        if (created)
        {
            Console.WriteLine("✅ База данных создана и инициализирована");
            AddTestData();
        }
        else
        {
            Console.WriteLine("✅ База данных уже существует");
        }

        Console.WriteLine($"📍 Файл базы данных: students.db");
        Console.WriteLine($"📊 Студентов в базе: {_context.Students.Count()}");
        Console.WriteLine($"🏛️ Факультетов в базе: {_context.Faculties.Count()}");
        Console.WriteLine($"📚 Предметов в базе: {_context.Subjects.Count()}");
        Console.WriteLine($"📝 Оценок в базе: {_context.Grades.Count()}\n");
    }

    /// <summary>
    /// Добавление тестовых данных (оценок)
    /// </summary>
    private void AddTestData()
    {
        if (!_context.Grades.Any())
        {
            var random = new Random();
            var grades = new List<Grade>();

            foreach (var student in _context.Students)
            {
                foreach (var subject in _context.Subjects)
                {
                    grades.Add(new Grade
                    {
                        StudentId = student.Id,
                        SubjectId = subject.Id,
                        Score = random.Next(3, 6), // Оценки от 3 до 5
                        Date = DateTime.Now.AddDays(-random.Next(1, 30))
                    });
                }
            }

            _context.Grades.AddRange(grades);
            _context.SaveChanges();
            Console.WriteLine("✅ Тестовые данные (оценки) добавлены");
        }
    }

    /// <summary>
    /// 2. Получение всех записей (SELECT)
    /// </summary>
    public void DemonstrateSelectAll()
    {
        Console.WriteLine("2. 📋 ПОЛУЧЕНИЕ ВСЕХ ЗАПИСЕЙ (SELECT *):");
        Console.WriteLine("========================================");

        // LINQ Query Syntax
        var students = from s in _context.Students
                       select s;

        // LINQ Method Syntax (альтернатива)
        // var students = _context.Students;

        PrintStudents(students.ToList());
    }

    /// <summary>
    /// 3. Фильтрация данных (WHERE) - по группе
    /// </summary>
    public void DemonstrateWhereByGroup()
    {
        Console.WriteLine("3. 🔍 ФИЛЬТРАЦИЯ ПО ГРУППЕ (WHERE):");
        Console.WriteLine("===================================");

        string groupName = "ИС101";

        // LINQ Query Syntax
        var students = from s in _context.Students
                       where s.GroupName == groupName
                       select s;

        // LINQ Method Syntax (альтернатива)
        // var students = _context.Students.Where(s => s.GroupName == groupName);

        Console.WriteLine($"Студенты группы '{groupName}':");
        PrintStudents(students.ToList());
    }

    /// <summary>
    /// 3. Фильтрация данных (WHERE) - по возрасту
    /// </summary>
    public void DemonstrateWhereByAge()
    {
        Console.WriteLine("3. 🔍 ФИЛЬТРАЦИЯ ПО ВОЗРАСТУ (WHERE):");
        Console.WriteLine("=====================================");

        int minAge = 18;

        // LINQ Query Syntax
        var students = from s in _context.Students
                       where s.Age > minAge
                       select s;

        Console.WriteLine($"Студенты старше {minAge} лет:");
        PrintStudents(students.ToList());
    }

    /// <summary>
    /// 4. Сортировка (ORDER BY)
    /// </summary>
    public void DemonstrateOrderBy()
    {
        Console.WriteLine("4. 📊 СОРТИРОВКА ПО ВОЗРАСТУ (ORDER BY):");
        Console.WriteLine("=======================================");

        // ORDER BY ASC - по возрастанию (Query Syntax)
        Console.WriteLine("По возрастанию возраста:");
        var studentsAsc = from s in _context.Students
                          orderby s.Age
                          select s;
        PrintStudents(studentsAsc.ToList());

        // ORDER BY DESC - по убыванию (Method Syntax)
        Console.WriteLine("По убыванию возраста:");
        var studentsDesc = _context.Students
            .OrderByDescending(s => s.Age)
            .ThenBy(s => s.Name); // Дополнительная сортировка по имени
        PrintStudents(studentsDesc.ToList());
    }

    /// <summary>
    /// 5. Проекции и выборка отдельных столбцов (SELECT new)
    /// </summary>
    public void DemonstrateProjection()
    {
        Console.WriteLine("5. 🎯 ПРОЕКЦИЯ - ВЫБОРКА ОТДЕЛЬНЫХ СТОЛБЦОВ (SELECT new):");
        Console.WriteLine("=========================================================");

        // LINQ Query Syntax с анонимным типом
        var studentInfo = from s in _context.Students
                          select new
                          {
                              StudentName = s.Name,
                              Group = s.GroupName,
                              Faculty = s.Faculty.Name
                          };

        Console.WriteLine("Имена студентов, группы и факультеты:");
        Console.WriteLine("Имя студента      | Группа | Факультет");
        Console.WriteLine(new string('-', 50));

        foreach (var info in studentInfo)
        {
            Console.WriteLine($"{info.StudentName,-16} | {info.Group,-6} | {info.Faculty}");
        }
        Console.WriteLine();

        // Method Syntax альтернатива
        var studentInfoMethod = _context.Students
            .Select(s => new
            {
                Name = s.Name,
                Age = s.Age,
                FacultyCode = s.Faculty.Code
            });

        Console.WriteLine("Дополнительная проекция (Method Syntax):");
        Console.WriteLine("Имя студента      | Возраст | Код факультета");
        Console.WriteLine(new string('-', 50));

        foreach (var info in studentInfoMethod)
        {
            Console.WriteLine($"{info.Name,-16} | {info.Age,-7} | {info.FacultyCode}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// 6. Продвинутое задание - JOIN запросы
    /// </summary>
    public void DemonstrateJoins()
    {
        Console.WriteLine("6. 🔗 JOIN ЗАПРОСЫ (ПРОДВИНУТОЕ ЗАДАНИЕ):");
        Console.WriteLine("========================================");

        // INNER JOIN - студенты с факультетами
        Console.WriteLine("INNER JOIN - Студенты с факультетами:");
        var studentsWithFaculties = from s in _context.Students
                                    join f in _context.Faculties on s.FacultyId equals f.Id
                                    select new
                                    {
                                        StudentName = s.Name,
                                        Group = s.GroupName,
                                        FacultyName = f.Name,
                                        FacultyCode = f.Code
                                    };

        foreach (var item in studentsWithFaculties)
        {
            Console.WriteLine($"  {item.StudentName} - {item.Group} - {item.FacultyName} ({item.FacultyCode})");
        }
        Console.WriteLine();

        // GROUP JOIN - студенты с их оценками
        Console.WriteLine("GROUP JOIN - Студенты с оценками:");
        var studentsWithGrades = from s in _context.Students
                                 join g in _context.Grades on s.Id equals g.StudentId into studentGrades
                                 select new
                                 {
                                     StudentName = s.Name,
                                     AverageGrade = studentGrades.Average(g => g.Score),
                                     GradeCount = studentGrades.Count()
                                 };

        foreach (var student in studentsWithGrades)
        {
            Console.WriteLine($"  {student.StudentName}: средний балл {student.AverageGrade:F1}, всего оценок: {student.GradeCount}");
        }
        Console.WriteLine();

        // MULTIPLE JOIN - сложный запрос с несколькими объединениями
        Console.WriteLine("MULTIPLE JOIN - Полная информация об оценках:");
        var fullGradeInfo = from g in _context.Grades
                            join s in _context.Students on g.StudentId equals s.Id
                            join subj in _context.Subjects on g.SubjectId equals subj.Id
                            join f in _context.Faculties on s.FacultyId equals f.Id
                            select new
                            {
                                Student = s.Name,
                                Faculty = f.Name,
                                Subject = subj.Name,
                                Grade = g.Score,
                                Date = g.Date
                            };

        foreach (var info in fullGradeInfo.Take(8)) // Показываем первые 8 записей
        {
            Console.WriteLine($"  {info.Student} ({info.Faculty}) - {info.Subject}: {info.Grade} ({info.Date:dd.MM.yyyy})");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Дополнительные LINQ операции
    /// </summary>
    public void DemonstrateAdditionalOperations()
    {
        Console.WriteLine("🎓 ДОПОЛНИТЕЛЬНЫЕ LINQ ОПЕРАЦИИ:");
        Console.WriteLine("================================");

        // GROUP BY - группировка студентов по факультетам
        Console.WriteLine("GROUP BY - Студенты по факультетам:");
        var studentsByFaculty = from s in _context.Students
                                group s by s.Faculty.Name into facultyGroup
                                select new
                                {
                                    Faculty = facultyGroup.Key,
                                    StudentCount = facultyGroup.Count(),
                                    AverageAge = facultyGroup.Average(s => s.Age),
                                    Students = facultyGroup.Select(s => s.Name)
                                };

        foreach (var facultyGroup in studentsByFaculty)
        {
            Console.WriteLine($"\n{facultyGroup.Faculty}:");
            Console.WriteLine($"  Количество: {facultyGroup.StudentCount}, Средний возраст: {facultyGroup.AverageAge:F1}");
            Console.WriteLine($"  Студенты: {string.Join(", ", facultyGroup.Students)}");
        }
        Console.WriteLine();

        // Агрегатные функции
        Console.WriteLine("АГРЕГАТНЫЕ ФУНКЦИИ:");
        var stats = new
        {
            TotalStudents = _context.Students.Count(),
            AverageAge = _context.Students.Average(s => s.Age),
            MaxAge = _context.Students.Max(s => s.Age),
            MinAge = _context.Students.Min(s => s.Age),
            TotalFaculties = _context.Faculties.Count()
        };

        Console.WriteLine($"  Всего студентов: {stats.TotalStudents}");
        Console.WriteLine($"  Средний возраст: {stats.AverageAge:F1}");
        Console.WriteLine($"  Минимальный возраст: {stats.MinAge}");
        Console.WriteLine($"  Максимальный возраст: {stats.MaxAge}");
        Console.WriteLine($"  Всего факультетов: {stats.TotalFaculties}");
    }

    /// <summary>
    /// Вспомогательный метод для вывода студентов
    /// </summary>
    private void PrintStudents(List<Student> students)
    {
        if (students == null || students.Count == 0)
        {
            Console.WriteLine("  Студенты не найдены");
            return;
        }

        Console.WriteLine("  ID | Имя студента      | Возраст | Группа | Факультет");
        Console.WriteLine("  " + new string('-', 55));

        foreach (var student in students)
        {
            Console.WriteLine($"  {student.Id,-2} | {student.Name,-16} | {student.Age,-6} | {student.GroupName,-6} | {student.Faculty?.Name}");
        }

        Console.WriteLine($"  Всего: {students.Count} студентов\n");
    }

    public void Dispose()
    {
        _context?.Dispose();
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
            using var linqDemo = new LinqDemo();

            Console.WriteLine("🎓 LINQ TO ENTITIES - ДЕМОНСТРАЦИЯ");
            Console.WriteLine("==================================\n");

            // 2. Получение всех записей
            linqDemo.DemonstrateSelectAll();

            // 3. Фильтрация данных
            linqDemo.DemonstrateWhereByGroup();
            linqDemo.DemonstrateWhereByAge();

            // 4. Сортировка
            linqDemo.DemonstrateOrderBy();

            // 5. Проекции
            linqDemo.DemonstrateProjection();

            // 6. JOIN запросы
            linqDemo.DemonstrateJoins();

            // Дополнительные операции
            linqDemo.DemonstrateAdditionalOperations();

            Console.WriteLine("✅ Демонстрация LINQ запросов завершена!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Произошла ошибка: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}