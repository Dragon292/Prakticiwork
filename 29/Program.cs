using Microsoft.VisualBasic.FileIO; // Для TextFieldParser
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Unicode;

// Класс Student для продвинутого задания
public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Group { get; set; }

    public Student(string name, int age, string group)
    {
        Name = name;
        Age = age;
        Group = group;
    }

    public override string ToString()
    {
        return $"Имя: {Name} | Возраст: {Age} | Группа: {Group}";
    }

    // Метод для преобразования в CSV строку
    public string ToCsvString()
    {
        // Если имя содержит запятые, заключаем в кавычки
        string name = Name.Contains(",") ? $"\"{Name}\"" : Name;
        return $"{name},{Age},{Group}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== РАБОТА С CSV-ФАЙЛАМИ ===\n");

        // Задание 1: Создание CSV-файла
        CreateCsvFile();

        // Задание 2: Чтение данных из CSV
        ReadCsvFile();

        // Задание 3: Добавление новых записей
        AddNewStudent();

        // Задание 4: Фильтрация по группе
        FilterByGroup();

        // Задание 5: Работа с разделителями и кавычками
        WorkWithSpecialCharacters();

        // Задание 6: Продвинутое - Учёт студентов
        StudentManagementSystem();

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }

    // Задание 1: Создание CSV-файла вручную
    static void CreateCsvFile()
    {
        Console.WriteLine("1. СОЗДАНИЕ CSV-ФАЙЛА\n");

        try
        {
            using (StreamWriter writer = new StreamWriter("students.csv", false, System.Text.Encoding.UTF8))
            {
                // Записываем заголовок
                writer.WriteLine("Имя,Возраст,Группа");

                // Записываем данные студентов
                writer.WriteLine("Иванов Иван,18,П-21");
                writer.WriteLine("Петров Петр,19,П-22");
                writer.WriteLine("Сидорова Анна,18,П-21");
                writer.WriteLine("Козлов Алексей,20,П-23");
                writer.WriteLine("Смирнова Мария,19,П-22");
            }

            Console.WriteLine("✅ Файл 'students.csv' успешно создан");
            Console.WriteLine("📊 Содержимое файла:");
            Console.WriteLine(File.ReadAllText("students.csv"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при создании файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 2: Чтение данных из CSV-файла
    static void ReadCsvFile()
    {
        Console.WriteLine("2. ЧТЕНИЕ ДАННЫХ ИЗ CSV-ФАЙЛА\n");

        try
        {
            if (!File.Exists("students.csv"))
            {
                Console.WriteLine("❌ Файл 'students.csv' не найден!");
                return;
            }

            string[] lines = File.ReadAllLines("students.csv", System.Text.Encoding.UTF8);

            // Пропускаем заголовок (первую строку)
            var studentLines = lines.Skip(1);

            Console.WriteLine("📋 СПИСОК СТУДЕНТОВ:");
            Console.WriteLine(new string('=', 50));

            foreach (string line in studentLines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 3)
                {
                    string name = parts[0];
                    string age = parts[1];
                    string group = parts[2];

                    Console.WriteLine($"Имя: {name} | Возраст: {age} | Группа: {group}");
                }
            }

            Console.WriteLine(new string('=', 50));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 3: Добавление новых записей в CSV-файл
    static void AddNewStudent()
    {
        Console.WriteLine("3. ДОБАВЛЕНИЕ НОВОГО СТУДЕНТА\n");

        try
        {
            if (!File.Exists("students.csv"))
            {
                Console.WriteLine("❌ Файл 'students.csv' не найден!");
                return;
            }

            Console.WriteLine("Введите данные нового студента:");

            Console.Write("Имя: ");
            string name = Console.ReadLine();

            Console.Write("Возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("❌ Неверный формат возраста!");
                return;
            }

            Console.Write("Группа: ");
            string group = Console.ReadLine();

            // Добавляем новую строку в файл
            using (StreamWriter writer = new StreamWriter("students.csv", true, System.Text.Encoding.UTF8))
            {
                writer.WriteLine($"{name},{age},{group}");
            }

            Console.WriteLine($"✅ Студент {name} успешно добавлен в файл");

            // Показываем обновленный список
            Console.WriteLine("\n📋 ОБНОВЛЕННЫЙ СПИСОК СТУДЕНТОВ:");
            ReadCsvFileSimple();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при добавлении студента: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 4: Фильтрация данных по группе
    static void FilterByGroup()
    {
        Console.WriteLine("4. ФИЛЬТРАЦИЯ ПО ГРУППЕ\n");

        try
        {
            if (!File.Exists("students.csv"))
            {
                Console.WriteLine("❌ Файл 'students.csv' не найден!");
                return;
            }

            Console.Write("Введите номер группы для фильтрации (например, П-21): ");
            string targetGroup = Console.ReadLine();

            string[] lines = File.ReadAllLines("students.csv", System.Text.Encoding.UTF8);
            var studentLines = lines.Skip(1);

            var filteredStudents = studentLines
                .Select(line => line.Split(','))
                .Where(parts => parts.Length == 3 && parts[2] == targetGroup);

            Console.WriteLine($"\n📋 СТУДЕНТЫ ГРУППЫ {targetGroup}:");
            Console.WriteLine(new string('=', 50));

            if (filteredStudents.Any())
            {
                foreach (var parts in filteredStudents)
                {
                    Console.WriteLine($"Имя: {parts[0]} | Возраст: {parts[1]} | Группа: {parts[2]}");
                }
            }
            else
            {
                Console.WriteLine("Студентов в указанной группе не найдено");
            }

            Console.WriteLine(new string('=', 50));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при фильтрации: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 5: Работа с разделителями и кавычками
    static void WorkWithSpecialCharacters()
    {
        Console.WriteLine("5. РАБОТА С РАЗДЕЛИТЕЛЯМИ И КАВЫЧКАМИ\n");

        try
        {
            // Создаем тестовый файл с особыми случаями
            string testFile = "students_special.csv";

            using (StreamWriter writer = new StreamWriter(testFile, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("Имя,Возраст,Группа");
                writer.WriteLine("\"Иванов, Иван Иванович\",18,П-21");
                writer.WriteLine("Петров Петр,19,П-22");
                writer.WriteLine("\"Сидорова, Анна; Мария\",20,П-23");
            }

            Console.WriteLine("✅ Создан тестовый файл с особыми символами");
            Console.WriteLine("📊 Исходное содержимое:");
            Console.WriteLine(File.ReadAllText(testFile));

            // Чтение с помощью TextFieldParser (корректно обрабатывает кавычки)
            Console.WriteLine("\n📖 ЧТЕНИЕ С TextFieldParser:");
            Console.WriteLine(new string('=', 50));

            using (TextFieldParser parser = new TextFieldParser(testFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true; // Важно для обработки кавычек

                // Читаем заголовок
                string[] header = parser.ReadFields();
                Console.WriteLine($"Заголовок: {string.Join(" | ", header)}");

                Console.WriteLine(new string('-', 50));

                // Читаем данные
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields != null && fields.Length == 3)
                    {
                        Console.WriteLine($"Имя: {fields[0]} | Возраст: {fields[1]} | Группа: {fields[2]}");
                    }
                }
            }

            Console.WriteLine(new string('=', 50));

            // Удаляем тестовый файл
            File.Delete(testFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 6: Продвинутое - Учёт студентов
    static void StudentManagementSystem()
    {
        Console.WriteLine("6. СИСТЕМА УЧЁТА СТУДЕНТОВ\n");

        List<Student> students = new List<Student>();

        // Загружаем студентов из файла
        LoadStudentsFromFile(students);

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== СИСТЕМА УЧЁТА СТУДЕНТОВ ===");
            Console.WriteLine($"Загружено студентов: {students.Count}");
            Console.WriteLine(new string('=', 40));

            DisplayStudents(students);

            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Редактировать студента");
            Console.WriteLine("3. Удалить студента");
            Console.WriteLine("4. Поиск по имени");
            Console.WriteLine("5. Фильтр по группе");
            Console.WriteLine("6. Сохранить и выйти");
            Console.WriteLine("0. Выйти без сохранения");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent(students);
                    break;
                case "2":
                    EditStudent(students);
                    break;
                case "3":
                    DeleteStudent(students);
                    break;
                case "4":
                    SearchStudent(students);
                    break;
                case "5":
                    FilterStudents(students);
                    break;
                case "6":
                    SaveStudentsToFile(students);
                    exit = true;
                    Console.WriteLine("✅ Изменения сохранены в файл 'students.csv'");
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("❌ Изменения не сохранены");
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор! Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Методы для системы учёта студентов

    static void LoadStudentsFromFile(List<Student> students)
    {
        try
        {
            if (!File.Exists("students.csv"))
            {
                Console.WriteLine("Файл 'students.csv' не найден. Будет создан новый при сохранении.");
                return;
            }

            using (TextFieldParser parser = new TextFieldParser("students.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                // Пропускаем заголовок
                if (!parser.EndOfData)
                    parser.ReadFields();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length == 3)
                    {
                        string name = fields[0];
                        if (int.TryParse(fields[1], out int age))
                        {
                            string group = fields[2];
                            students.Add(new Student(name, age, group));
                        }
                    }
                }
            }

            Console.WriteLine($"✅ Загружено {students.Count} студентов из файла");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при загрузке студентов: {ex.Message}");
        }
    }

    static void SaveStudentsToFile(List<Student> students)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("students.csv", false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("Имя,Возраст,Группа");

                foreach (Student student in students)
                {
                    writer.WriteLine(student.ToCsvString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при сохранении студентов: {ex.Message}");
        }
    }

    static void DisplayStudents(List<Student> students)
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

    static void AddStudent(List<Student> students)
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

        students.Add(new Student(name, age, group));
        Console.WriteLine($"✅ Студент {name} добавлен");
        Console.ReadKey();
    }

    static void EditStudent(List<Student> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("❌ Список студентов пуст!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n=== РЕДАКТИРОВАНИЕ СТУДЕНТА ===");
        DisplayStudents(students);

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

    static void DeleteStudent(List<Student> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("❌ Список студентов пуст!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n=== УДАЛЕНИЕ СТУДЕНТА ===");
        DisplayStudents(students);

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

    static void SearchStudent(List<Student> students)
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
        }
        else
        {
            Console.WriteLine("  Студенты не найдены");
        }

        Console.ReadKey();
    }

    static void FilterStudents(List<Student> students)
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
        }
        else
        {
            Console.WriteLine("  Студенты в указанной группе не найдены");
        }

        Console.ReadKey();
    }

    // Вспомогательный метод для простого чтения CSV
    static void ReadCsvFileSimple()
    {
        try
        {
            string[] lines = File.ReadAllLines("students.csv", System.Text.Encoding.UTF8);
            var studentLines = lines.Skip(1);

            foreach (string line in studentLines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 3)
                {
                    Console.WriteLine($"Имя: {parts[0]} | Возраст: {parts[1]} | Группа: {parts[2]}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении файла: {ex.Message}");
        }
    }
}

//1.ФОРМАТ CSV:
//Разделитель: Запятая(,) между полями

//Кавычки: Для значений, содержащих запятые или специальные символы

//Кодировка: UTF - 8 для поддержки кириллицы

//Заголовок: Первая строка с названиями колонок

//2. МЕТОДЫ ЧТЕНИЯ CSV:
//Split(','): Простой способ, но не обрабатывает кавычки

//TextFieldParser: Корректно обрабатывает кавычки и специальные символы

//Сторонние библиотеки: CsvHelper, LINQ to CSV

//3. ОБРАБОТКА ОШИБОК:
//Проверка существования файла: File.Exists()

//Валидация данных: Проверка формата чисел, обязательных полей

//Обработка исключений: FileNotFoundException, FormatException

//4. ОПЕРАЦИИ С ДАННЫМИ:
//Фильтрация: LINQ Where() для отбора по условиям

//Поиск: Contains() для текстового поиска

//Сортировка: OrderBy() для упорядочивания данных

//Программа демонстрирует полный цикл работы с CSV-файлами от создания до продвинутого управления данными!