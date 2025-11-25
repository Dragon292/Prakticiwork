using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

// Класс Student для работы с JSON
public class Student
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("group")]
    public string Group { get; set; }

    public Student(string name, int age, string group)
    {
        Name = name;
        Age = age;
        Group = group;
    }

    // Конструктор по умолчанию для десериализации
    public Student() { }

    public override string ToString()
    {
        return $"{Name} — {Age} лет — группа {Group}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== РАБОТА С JSON-ФАЙЛАМИ ===\n");

        // Задание 1: Создание JSON-файла
        CreateJsonFile();

        // Задание 2: Чтение данных из JSON
        ReadJsonFile();

        // Задание 3: Добавление новых записей
        AddNewStudentToJson();

        // Задание 4: Фильтрация данных
        FilterStudentsByGroup();

        // Задание 5: Работа с форматированием JSON
        DemonstrateJsonFormatting();

        // Задание 6: Продвинутое - JSON Student Manager
        JsonStudentManager();

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }

    // Задание 1: Создание JSON-файла
    static void CreateJsonFile()
    {
        Console.WriteLine("1. СОЗДАНИЕ JSON-ФАЙЛА\n");

        try
        {
            // Создаем список студентов
            List<Student> students = new List<Student>
            {
                new Student("Иванов Иван", 18, "П-21"),
                new Student("Петров Петр", 19, "П-22"),
                new Student("Сидорова Анна", 18, "П-21"),
                new Student("Козлов Алексей", 20, "П-23"),
                new Student("Смирнова Мария", 19, "П-22")
            };

            // Настройки сериализации для красивого вывода
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,  // Форматирование с отступами
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // camelCase для свойств
            };

            // Сериализуем в JSON
            string jsonString = JsonSerializer.Serialize(students, options);

            // Сохраняем в файл
            File.WriteAllText("students.json", jsonString);

            Console.WriteLine("✅ JSON-файл 'students.json' успешно создан");
            Console.WriteLine("📊 Содержимое файла:");
            Console.WriteLine(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при создании JSON-файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 2: Чтение данных из JSON-файла
    static void ReadJsonFile()
    {
        Console.WriteLine("2. ЧТЕНИЕ ДАННЫХ ИЗ JSON-ФАЙЛА\n");

        try
        {
            if (!File.Exists("students.json"))
            {
                Console.WriteLine("❌ Файл 'students.json' не найден!");
                return;
            }

            // Читаем JSON из файла
            string jsonString = File.ReadAllText("students.json");

            // Десериализуем JSON обратно в список студентов
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true  // Игнорировать регистр свойств
            };

            List<Student> students = JsonSerializer.Deserialize<List<Student>>(jsonString, options);

            Console.WriteLine("📋 СПИСОК СТУДЕНТОВ:");
            Console.WriteLine(new string('=', 45));

            if (students != null && students.Count > 0)
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

            Console.WriteLine(new string('=', 45));
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"❌ Ошибка формата JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении JSON-файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 3: Добавление новых записей в JSON
    static void AddNewStudentToJson()
    {
        Console.WriteLine("3. ДОБАВЛЕНИЕ НОВОГО СТУДЕНТА В JSON\n");

        try
        {
            if (!File.Exists("students.json"))
            {
                Console.WriteLine("❌ Файл 'students.json' не найден!");
                return;
            }

            Console.WriteLine("Введите данные нового студента:");

            Console.Write("Имя: ");
            string name = Console.ReadLine();

            Console.Write("Возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age < 16 || age > 100)
            {
                Console.WriteLine("❌ Неверный формат возраста!");
                return;
            }

            Console.Write("Группа: ");
            string group = Console.ReadLine();

            // Читаем существующие данные
            string jsonString = File.ReadAllText("students.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            List<Student> students = JsonSerializer.Deserialize<List<Student>>(jsonString, options)
                                    ?? new List<Student>();

            // Добавляем нового студента
            students.Add(new Student(name, age, group));

            // Сохраняем обновленный список
            string updatedJson = JsonSerializer.Serialize(students, options);
            File.WriteAllText("students.json", updatedJson);

            Console.WriteLine($"✅ Студент {name} успешно добавлен в JSON-файл");

            // Показываем обновленный список
            Console.WriteLine("\n📋 ОБНОВЛЕННЫЙ СПИСОК СТУДЕНТОВ:");
            ReadJsonFileSimple();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при добавлении студента: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 4: Фильтрация данных
    static void FilterStudentsByGroup()
    {
        Console.WriteLine("4. ФИЛЬТРАЦИЯ СТУДЕНТОВ ПО ГРУППЕ\n");

        try
        {
            if (!File.Exists("students.json"))
            {
                Console.WriteLine("❌ Файл 'students.json' не найден!");
                return;
            }

            Console.Write("Введите номер группы для фильтрации (например, П-21): ");
            string targetGroup = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(targetGroup))
            {
                Console.WriteLine("❌ Группа не может быть пустой!");
                return;
            }

            // Загружаем данные
            string jsonString = File.ReadAllText("students.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Student> students = JsonSerializer.Deserialize<List<Student>>(jsonString, options);

            // Фильтруем с помощью LINQ
            var filteredStudents = students?
                .Where(s => s.Group.Equals(targetGroup, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.WriteLine($"\n📋 СТУДЕНТЫ ГРУППЫ '{targetGroup}':");
            Console.WriteLine(new string('=', 45));

            if (filteredStudents != null && filteredStudents.Count > 0)
            {
                foreach (var student in filteredStudents)
                {
                    Console.WriteLine(student.ToString());
                }
                Console.WriteLine($"\nВсего студентов в группе: {filteredStudents.Count}");
            }
            else
            {
                Console.WriteLine("Студенты в указанной группе не найдены");
            }

            Console.WriteLine(new string('=', 45));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при фильтрации: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 5: Работа с форматированием JSON
    static void DemonstrateJsonFormatting()
    {
        Console.WriteLine("5. ФОРМАТИРОВАНИЕ JSON\n");

        try
        {
            // Создаем тестовые данные
            var testStudent = new Student("Тестовый Студент", 21, "Т-01");

            // JSON без форматирования
            var optionsNoFormatting = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // JSON с форматированием
            var optionsWithFormatting = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string jsonWithoutFormatting = JsonSerializer.Serialize(testStudent, optionsNoFormatting);
            string jsonWithFormatting = JsonSerializer.Serialize(testStudent, optionsWithFormatting);

            Console.WriteLine("=== JSON БЕЗ ФОРМАТИРОВАНИЯ ===");
            Console.WriteLine(jsonWithoutFormatting);

            Console.WriteLine("\n=== JSON С ФОРМАТИРОВАНИЕМ ===");
            Console.WriteLine(jsonWithFormatting);

            Console.WriteLine("\n=== СРАВНЕНИЕ РАЗМЕРА ===");
            Console.WriteLine($"Без форматирования: {jsonWithoutFormatting.Length} символов");
            Console.WriteLine($"С форматированием: {jsonWithFormatting.Length} символов");
            Console.WriteLine($"Разница: {jsonWithFormatting.Length - jsonWithoutFormatting.Length} символов");

            // Демонстрация разных настроек сериализации
            Console.WriteLine("\n=== ДОПОЛНИТЕЛЬНЫЕ НАСТРОЙКИ ===");

            var customOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };

            string customJson = JsonSerializer.Serialize(testStudent, customOptions);
            Console.WriteLine("С пользовательскими настройками:");
            Console.WriteLine(customJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 6: Продвинутое - JSON Student Manager
    static void JsonStudentManager()
    {
        Console.WriteLine("6. JSON STUDENT MANAGER\n");

        List<Student> students = new List<Student>();

        // Загружаем студентов из JSON файла
        LoadStudentsFromJson(students);

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== JSON STUDENT MANAGER ===");
            Console.WriteLine($"Загружено студентов: {students.Count}");
            Console.WriteLine(new string('=', 40));

            DisplayStudentsTable(students);

            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Показать всех студентов");
            Console.WriteLine("2. Добавить студента");
            Console.WriteLine("3. Изменить данные студента");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Поиск по имени");
            Console.WriteLine("6. Фильтр по группе");
            Console.WriteLine("7. Сортировка студентов");
            Console.WriteLine("8. Сохранить и выйти");
            Console.WriteLine("0. Выйти без сохранения");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllStudents(students);
                    break;
                case "2":
                    AddStudentToManager(students);
                    break;
                case "3":
                    EditStudentInManager(students);
                    break;
                case "4":
                    DeleteStudentFromManager(students);
                    break;
                case "5":
                    SearchStudentInManager(students);
                    break;
                case "6":
                    FilterStudentsInManager(students);
                    break;
                case "7":
                    SortStudentsInManager(students);
                    break;
                case "8":
                    SaveStudentsToJson(students);
                    exit = true;
                    Console.WriteLine("✅ Изменения сохранены в файл 'students.json'");
                    Console.ReadKey();
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("❌ Изменения не сохранены");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор! Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Методы для JSON Student Manager

    static void LoadStudentsFromJson(List<Student> students)
    {
        try
        {
            if (!File.Exists("students.json"))
            {
                Console.WriteLine("Файл 'students.json' не найден. Будет создан новый при сохранении.");
                return;
            }

            string jsonString = File.ReadAllText("students.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var loadedStudents = JsonSerializer.Deserialize<List<Student>>(jsonString, options);
            if (loadedStudents != null)
            {
                students.Clear();
                students.AddRange(loadedStudents);
            }

            Console.WriteLine($"✅ Загружено {students.Count} студентов из JSON-файла");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при загрузке студентов: {ex.Message}");
        }
    }

    static void SaveStudentsToJson(List<Student> students)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string jsonString = JsonSerializer.Serialize(students, options);
            File.WriteAllText("students.json", jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при сохранении студентов: {ex.Message}");
        }
    }

    static void DisplayStudentsTable(List<Student> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст");
            return;
        }

        Console.WriteLine("№  Имя                | Возраст | Группа");
        Console.WriteLine(new string('-', 40));

        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i + 1,-2} {students[i].Name,-18} | {students[i].Age,-7} | {students[i].Group}");
        }
    }

    static void DisplayAllStudents(List<Student> students)
    {
        Console.WriteLine("\n=== ПОЛНЫЙ СПИСОК СТУДЕНТОВ ===");

        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст");
        }
        else
        {
            foreach (var student in students)
            {
                Console.WriteLine($"  {student}");
            }
            Console.WriteLine($"\nВсего студентов: {students.Count}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void AddStudentToManager(List<Student> students)
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ СТУДЕНТА ===");

        Console.Write("Имя: ");
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Имя не может быть пустым!");
            Console.ReadKey();
            return;
        }

        Console.Write("Возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 16 || age > 100)
        {
            Console.WriteLine("❌ Неверный формат возраста!");
            Console.ReadKey();
            return;
        }

        Console.Write("Группа: ");
        string group = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(group))
        {
            Console.WriteLine("❌ Группа не может быть пустой!");
            Console.ReadKey();
            return;
        }

        // Проверяем, нет ли уже студента с таким именем
        if (students.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"❌ Студент с именем '{name}' уже существует!");
            Console.ReadKey();
            return;
        }

        students.Add(new Student(name, age, group));
        Console.WriteLine($"✅ Студент {name} добавлен");
        Console.ReadKey();
    }

    static void EditStudentInManager(List<Student> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("❌ Список студентов пуст!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n=== РЕДАКТИРОВАНИЕ СТУДЕНТА ===");
        DisplayStudentsTable(students);

        Console.Write("\nВведите номер студента для редактирования: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > students.Count)
        {
            Console.WriteLine("❌ Неверный номер студента!");
            Console.ReadKey();
            return;
        }

        Student student = students[index - 1];

        Console.WriteLine($"\nРедактирование: {student}");
        Console.WriteLine("(Оставьте поле пустым, чтобы не изменять значение)");

        Console.Write($"Новое имя [{student.Name}]: ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
        {
            student.Name = newName;
        }

        Console.Write($"Новый возраст [{student.Age}]: ");
        string ageInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(ageInput) && int.TryParse(ageInput, out int newAge))
        {
            student.Age = newAge;
        }

        Console.Write($"Новая группа [{student.Group}]: ");
        string newGroup = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newGroup))
        {
            student.Group = newGroup;
        }

        Console.WriteLine($"✅ Данные студента обновлены: {student}");
        Console.ReadKey();
    }

    static void DeleteStudentFromManager(List<Student> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("❌ Список студентов пуст!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n=== УДАЛЕНИЕ СТУДЕНТА ===");
        DisplayStudentsTable(students);

        Console.Write("\nВведите номер студента для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > students.Count)
        {
            Console.WriteLine("❌ Неверный номер студента!");
            Console.ReadKey();
            return;
        }

        Student student = students[index - 1];

        Console.Write($"Вы уверены, что хотите удалить студента {student.Name}? (y/n): ");
        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "y")
        {
            students.RemoveAt(index - 1);
            Console.WriteLine($"✅ Студент {student.Name} удален");
        }
        else
        {
            Console.WriteLine("❌ Удаление отменено");
        }

        Console.ReadKey();
    }

    static void SearchStudentInManager(List<Student> students)
    {
        Console.WriteLine("\n=== ПОИСК СТУДЕНТА ===");

        Console.Write("Введите имя или часть имени для поиска: ");
        string searchTerm = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("❌ Поисковый запрос не может быть пустым!");
            Console.ReadKey();
            return;
        }

        var foundStudents = students
            .Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Console.WriteLine($"\n📋 РЕЗУЛЬТАТЫ ПОИСКА ('{searchTerm}'):");

        if (foundStudents.Any())
        {
            foreach (var student in foundStudents)
            {
                Console.WriteLine($"  {student}");
            }
            Console.WriteLine($"\nНайдено: {foundStudents.Count} студентов");
        }
        else
        {
            Console.WriteLine("  Студенты не найдены");
        }

        Console.ReadKey();
    }

    static void FilterStudentsInManager(List<Student> students)
    {
        Console.WriteLine("\n=== ФИЛЬТР ПО ГРУППЕ ===");

        Console.Write("Введите группу для фильтрации: ");
        string group = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(group))
        {
            Console.WriteLine("❌ Группа не может быть пустой!");
            Console.ReadKey();
            return;
        }

        var filteredStudents = students
            .Where(s => s.Group.Equals(group, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Console.WriteLine($"\n📋 СТУДЕНТЫ ГРУППЫ '{group}':");

        if (filteredStudents.Any())
        {
            foreach (var student in filteredStudents)
            {
                Console.WriteLine($"  {student}");
            }
            Console.WriteLine($"\nВсего в группе: {filteredStudents.Count} студентов");
        }
        else
        {
            Console.WriteLine("  Студенты в указанной группе не найдены");
        }

        Console.ReadKey();
    }

    static void SortStudentsInManager(List<Student> students)
    {
        Console.WriteLine("\n=== СОРТИРОВКА СТУДЕНТОВ ===");
        Console.WriteLine("1. По имени (А-Я)");
        Console.WriteLine("2. По имени (Я-А)");
        Console.WriteLine("3. По возрасту (по возрастанию)");
        Console.WriteLine("4. По возрасту (по убыванию)");
        Console.WriteLine("5. По группе");
        Console.Write("Выберите тип сортировки: ");

        string choice = Console.ReadLine();
        List<Student> sortedStudents = new List<Student>();

        switch (choice)
        {
            case "1":
                sortedStudents = students.OrderBy(s => s.Name).ToList();
                Console.WriteLine("✅ Сортировка по имени (А-Я)");
                break;
            case "2":
                sortedStudents = students.OrderByDescending(s => s.Name).ToList();
                Console.WriteLine("✅ Сортировка по имени (Я-А)");
                break;
            case "3":
                sortedStudents = students.OrderBy(s => s.Age).ToList();
                Console.WriteLine("✅ Сортировка по возрасту (по возрастанию)");
                break;
            case "4":
                sortedStudents = students.OrderByDescending(s => s.Age).ToList();
                Console.WriteLine("✅ Сортировка по возрасту (по убыванию)");
                break;
            case "5":
                sortedStudents = students.OrderBy(s => s.Group).ThenBy(s => s.Name).ToList();
                Console.WriteLine("✅ Сортировка по группе и имени");
                break;
            default:
                Console.WriteLine("❌ Неверный выбор!");
                Console.ReadKey();
                return;
        }

        // Временно заменяем список для отображения
        var originalStudents = new List<Student>(students);
        students.Clear();
        students.AddRange(sortedStudents);

        Console.WriteLine("\n📋 ОТСОРТИРОВАННЫЙ СПИСОК:");
        DisplayStudentsTable(students);

        // Восстанавливаем оригинальный порядок (если пользователь не сохранит)
        students.Clear();
        students.AddRange(originalStudents);

        Console.WriteLine("\nСортировка применена только для просмотра.");
        Console.WriteLine("Для сохранения порядка выберите 'Сохранить и выйти' в главном меню.");
        Console.ReadKey();
    }

    // Вспомогательный метод для простого чтения JSON
    static void ReadJsonFileSimple()
    {
        try
        {
            string jsonString = File.ReadAllText("students.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Student> students = JsonSerializer.Deserialize<List<Student>>(jsonString, options);

            if (students != null)
            {
                foreach (var student in students)
                {
                    Console.WriteLine(student.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении файла: {ex.Message}");
        }
    }
}

//1.SYSTEM.TEXT.JSON БИБЛИОТЕКА:
//JsonSerializer - основной класс для сериализации/десериализации

//JsonSerializerOptions - настройки форматирования и поведения

//JsonPropertyNameAttribute - кастомные имена свойств в JSON

//2. ОСНОВНЫЕ НАСТРОЙКИ СЕРИАЛИЗАЦИИ:
//WriteIndented - форматирование с отступами(true/false)

//PropertyNamingPolicy - политика именования свойств(CamelCase)

//PropertyNameCaseInsensitive - игнорирование регистра при десериализации

//DefaultIgnoreCondition - игнорирование значений по умолчанию

//3. СЕРИАЛИЗАЦИЯ И ДЕСЕРИАЛИЗАЦИЯ:
//Serialize() - преобразование объектов в JSON строку

//Deserialize() - преобразование JSON строки в объекты

//Async методы - для работы с потоками (не показаны в примере)

//4. LINQ ДЛЯ РАБОТЫ С КОЛЛЕКЦИЯМИ:
//Where() - фильтрация по условию

//OrderBy() / OrderByDescending() - сортировка

//Select() - проекция данных

//Any() / Count() - проверка наличия элементов

//5. CRUD ОПЕРАЦИИ:
//Create - добавление новых элементов в коллекцию

//Read - загрузка и чтение данных из JSON

//Update - изменение существующих элементов

//Delete - удаление элементов из коллекции

//6. ОБРАБОТКА ОШИБОК:
//JsonException - ошибки формата JSON

//FileNotFoundException - файл не найден

//ArgumentNullException - нулевые ссылки

//Программа демонстрирует полный цикл работы с JSON-файлами от базовых операций до продвинутой системы управления данными с использованием современных практик C#!

