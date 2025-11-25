using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

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
        return $"{Name} — {Age} лет — группа {Group}";
    }

    // Метод для преобразования в XElement
    public XElement ToXElement()
    {
        return new XElement("student",
            new XElement("name", Name),
            new XElement("age", Age),
            new XElement("group", Group)
        );
    }

    // Статический метод для создания Student из XElement
    public static Student FromXElement(XElement element)
    {
        return new Student(
            element.Element("name")?.Value ?? "",
            int.Parse(element.Element("age")?.Value ?? "0"),
            element.Element("group")?.Value ?? ""
        );
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== РАБОТА С XML-ФАЙЛАМИ ===\n");

        // Задание 1: Создание XML-файла
        CreateXmlFile();

        // Задание 2: Чтение данных из XML
        ReadXmlFile();

        // Задание 3: Добавление новых элементов
        AddNewStudentToXml();

        // Задание 4: Поиск элементов
        SearchInXml();

        // Задание 5: Изменение и удаление данных
        UpdateAndDeleteInXml();

        // Задание 6: Продвинутое - XML Student Manager
        XmlStudentManager();

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }

    // Задание 1: Создание XML-файла вручную
    static void CreateXmlFile()
    {
        Console.WriteLine("1. СОЗДАНИЕ XML-ФАЙЛА\n");

        try
        {
            // Создаем XML документ с помощью XDocument
            XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("students",
                    new XElement("student",
                        new XElement("name", "Иванов Иван"),
                        new XElement("age", 18),
                        new XElement("group", "П-21")
                    ),
                    new XElement("student",
                        new XElement("name", "Петров Петр"),
                        new XElement("age", 19),
                        new XElement("group", "П-22")
                    ),
                    new XElement("student",
                        new XElement("name", "Сидорова Анна"),
                        new XElement("age", 18),
                        new XElement("group", "П-21")
                    )
                )
            );

            // Сохраняем XML файл
            xmlDoc.Save("students.xml");

            Console.WriteLine("✅ XML-файл 'students.xml' успешно создан");
            Console.WriteLine("📊 Содержимое файла:");
            Console.WriteLine(File.ReadAllText("students.xml"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при создании XML-файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 2: Чтение данных из XML-файла
    static void ReadXmlFile()
    {
        Console.WriteLine("2. ЧТЕНИЕ ДАННЫХ ИЗ XML-ФАЙЛА\n");

        try
        {
            if (!File.Exists("students.xml"))
            {
                Console.WriteLine("❌ Файл 'students.xml' не найден!");
                return;
            }

            // Способ 1: Использование XDocument (LINQ to XML) - современный подход
            Console.WriteLine("=== ЧТЕНИЕ С XDocument (LINQ to XML) ===");
            XDocument xdoc = XDocument.Load("students.xml");

            var students = from student in xdoc.Descendants("student")
                           select new
                           {
                               Name = student.Element("name")?.Value,
                               Age = student.Element("age")?.Value,
                               Group = student.Element("group")?.Value
                           };

            Console.WriteLine("📋 СПИСОК СТУДЕНТОВ:");
            Console.WriteLine(new string('=', 40));

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name} — {student.Age} лет — группа {student.Group}");
            }

            Console.WriteLine(new string('=', 40));

            // Способ 2: Использование XmlDocument (устаревший, но полезный для понимания)
            Console.WriteLine("\n=== ЧТЕНИЕ С XmlDocument ===");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("students.xml");

            XmlNodeList studentNodes = xmlDoc.SelectNodes("//student");

            Console.WriteLine("📋 СПИСОК СТУДЕНТОВ (XmlDocument):");
            Console.WriteLine(new string('=', 40));

            foreach (XmlNode studentNode in studentNodes)
            {
                string name = studentNode.SelectSingleNode("name")?.InnerText ?? "";
                string age = studentNode.SelectSingleNode("age")?.InnerText ?? "";
                string group = studentNode.SelectSingleNode("group")?.InnerText ?? "";

                Console.WriteLine($"{name} — {age} лет — группа {group}");
            }

            Console.WriteLine(new string('=', 40));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении XML-файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 3: Добавление новых элементов в XML
    static void AddNewStudentToXml()
    {
        Console.WriteLine("3. ДОБАВЛЕНИЕ НОВОГО СТУДЕНТА В XML\n");

        try
        {
            if (!File.Exists("students.xml"))
            {
                Console.WriteLine("❌ Файл 'students.xml' не найден!");
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

            // Загружаем существующий XML
            XDocument xdoc = XDocument.Load("students.xml");

            // Создаем новый элемент студента
            XElement newStudent = new XElement("student",
                new XElement("name", name),
                new XElement("age", age),
                new XElement("group", group)
            );

            // Добавляем нового студента в корневой элемент
            xdoc.Root.Add(newStudent);

            // Сохраняем изменения
            xdoc.Save("students.xml");

            Console.WriteLine($"✅ Студент {name} успешно добавлен в XML-файл");

            // Показываем обновленный список
            Console.WriteLine("\n📋 ОБНОВЛЕННЫЙ СПИСОК СТУДЕНТОВ:");
            ReadXmlFileSimple();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при добавлении студента: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 4: Поиск элементов в XML
    static void SearchInXml()
    {
        Console.WriteLine("4. ПОИСК В XML-ФАЙЛЕ\n");

        try
        {
            if (!File.Exists("students.xml"))
            {
                Console.WriteLine("❌ Файл 'students.xml' не найден!");
                return;
            }

            Console.WriteLine("Выберите критерий поиска:");
            Console.WriteLine("1. Поиск по имени");
            Console.WriteLine("2. Поиск по группе");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            string searchTerm = "";

            switch (choice)
            {
                case "1":
                    Console.Write("Введите имя для поиска: ");
                    searchTerm = Console.ReadLine();
                    SearchByName(searchTerm);
                    break;
                case "2":
                    Console.Write("Введите группу для поиска: ");
                    searchTerm = Console.ReadLine();
                    SearchByGroup(searchTerm);
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при поиске: {ex.Message}");
        }

        Console.WriteLine();
    }

    static void SearchByName(string name)
    {
        XDocument xdoc = XDocument.Load("students.xml");

        var students = from student in xdoc.Descendants("student")
                       where student.Element("name")?.Value.Contains(name) == true
                       select new
                       {
                           Name = student.Element("name")?.Value,
                           Age = student.Element("age")?.Value,
                           Group = student.Element("group")?.Value
                       };

        Console.WriteLine($"\n📋 РЕЗУЛЬТАТЫ ПОИСКА ПО ИМЕНИ '{name}':");
        Console.WriteLine(new string('=', 50));

        if (students.Any())
        {
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name} — {student.Age} лет — группа {student.Group}");
            }
        }
        else
        {
            Console.WriteLine("Студенты с указанным именем не найдены");
        }

        Console.WriteLine(new string('=', 50));
    }

    static void SearchByGroup(string group)
    {
        XDocument xdoc = XDocument.Load("students.xml");

        var students = from student in xdoc.Descendants("student")
                       where student.Element("group")?.Value == @group
                      select new
                      {
                          Name = student.Element("name")?.Value,
                          Age = student.Element("age")?.Value,
                          Group = student.Element("group")?.Value
                      };

        Console.WriteLine($"\n📋 СТУДЕНТЫ ГРУППЫ '{group}':");
        Console.WriteLine(new string('=', 50));

        if (students.Any())
        {
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name} — {student.Age} лет — группа {student.Group}");
            }
        }
        else
        {
            Console.WriteLine("Студенты в указанной группе не найдены");
        }

        Console.WriteLine(new string('=', 50));
    }

    // Задание 5: Изменение и удаление данных
    static void UpdateAndDeleteInXml()
    {
        Console.WriteLine("5. ИЗМЕНЕНИЕ И УДАЛЕНИЕ ДАННЫХ В XML\n");

        try
        {
            if (!File.Exists("students.xml"))
            {
                Console.WriteLine("❌ Файл 'students.xml' не найден!");
                return;
            }

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Изменить данные студента");
            Console.WriteLine("2. Удалить студента");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UpdateStudent();
                    break;
                case "2":
                    DeleteStudent();
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        Console.WriteLine();
    }

    static void UpdateStudent()
    {
        XDocument xdoc = XDocument.Load("students.xml");

        Console.Write("Введите имя студента для изменения: ");
        string studentName = Console.ReadLine();

        // Находим студента по имени
        XElement studentElement = xdoc.Descendants("student")
            .FirstOrDefault(s => s.Element("name")?.Value == studentName);

        if (studentElement == null)
        {
            Console.WriteLine($"❌ Студент с именем '{studentName}' не найден");
            return;
        }

        Console.WriteLine($"\nТекущие данные: {studentElement.Element("name")?.Value} — " +
                         $"{studentElement.Element("age")?.Value} лет — " +
                         $"{studentElement.Element("group")?.Value}");

        Console.WriteLine("\nВведите новые данные (оставьте пустым, чтобы не изменять):");

        Console.Write($"Новое имя [{studentElement.Element("name")?.Value}]: ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
        {
            studentElement.Element("name").Value = newName;
        }

        Console.Write($"Новый возраст [{studentElement.Element("age")?.Value}]: ");
        string newAge = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newAge) && int.TryParse(newAge, out int age))
        {
            studentElement.Element("age").Value = age.ToString();
        }

        Console.Write($"Новая группа [{studentElement.Element("group")?.Value}]: ");
        string newGroup = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newGroup))
        {
            studentElement.Element("group").Value = newGroup;
        }

        xdoc.Save("students.xml");
        Console.WriteLine($"✅ Данные студента обновлены");

        // Показываем обновленный список
        Console.WriteLine("\n📋 ОБНОВЛЕННЫЙ СПИСОК СТУДЕНТОВ:");
        ReadXmlFileSimple();
    }

    static void DeleteStudent()
    {
        XDocument xdoc = XDocument.Load("students.xml");

        Console.Write("Введите имя студента для удаления: ");
        string studentName = Console.ReadLine();

        // Находим студента по имени
        XElement studentElement = xdoc.Descendants("student")
            .FirstOrDefault(s => s.Element("name")?.Value == studentName);

        if (studentElement == null)
        {
            Console.WriteLine($"❌ Студент с именем '{studentName}' не найден");
            return;
        }

        Console.Write($"Вы уверены, что хотите удалить студента {studentName}? (y/n): ");
        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "y")
        {
            studentElement.Remove();
            xdoc.Save("students.xml");
            Console.WriteLine($"✅ Студент {studentName} удален");

            // Показываем обновленный список
            Console.WriteLine("\n📋 ОБНОВЛЕННЫЙ СПИСОК СТУДЕНТОВ:");
            ReadXmlFileSimple();
        }
        else
        {
            Console.WriteLine("❌ Удаление отменено");
        }
    }

    // Задание 6: Продвинутое - XML Student Manager
    static void XmlStudentManager()
    {
        Console.WriteLine("6. XML STUDENT MANAGER\n");

        List<Student> students = new List<Student>();

        // Загружаем студентов из XML файла
        LoadStudentsFromXml(students);

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== XML STUDENT MANAGER ===");
            Console.WriteLine($"Загружено студентов: {students.Count}");
            Console.WriteLine(new string('=', 40));

            DisplayStudents(students);

            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Показать всех студентов");
            Console.WriteLine("2. Добавить студента");
            Console.WriteLine("3. Изменить данные");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Поиск по имени");
            Console.WriteLine("6. Фильтр по группе");
            Console.WriteLine("7. Сохранить и выйти");
            Console.WriteLine("0. Выйти без сохранения");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayStudentsDetailed(students);
                    break;
                case "2":
                    AddStudent(students);
                    break;
                case "3":
                    EditStudent(students);
                    break;
                case "4":
                    DeleteStudentFromList(students);
                    break;
                case "5":
                    SearchStudent(students);
                    break;
                case "6":
                    FilterStudentsByGroup(students);
                    break;
                case "7":
                    SaveStudentsToXml(students);
                    exit = true;
                    Console.WriteLine("✅ Изменения сохранены в файл 'students.xml'");
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

    // Методы для XML Student Manager

    static void LoadStudentsFromXml(List<Student> students)
    {
        try
        {
            if (!File.Exists("students.xml"))
            {
                Console.WriteLine("Файл 'students.xml' не найден. Будет создан новый при сохранении.");
                return;
            }

            XDocument xdoc = XDocument.Load("students.xml");

            students.Clear();
            students.AddRange(
                from student in xdoc.Descendants("student")
                select Student.FromXElement(student)
            );

            Console.WriteLine($"✅ Загружено {students.Count} студентов из XML-файла");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при загрузке студентов: {ex.Message}");
        }
    }

    static void SaveStudentsToXml(List<Student> students)
    {
        try
        {
            XDocument xdoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("students",
                    students.Select(student => student.ToXElement())
                )
            );

            xdoc.Save("students.xml");
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

    static void DisplayStudentsDetailed(List<Student> students)
    {
        Console.WriteLine("\n=== ПОДРОБНЫЙ СПИСОК СТУДЕНТОВ ===");

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
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
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

    static void DeleteStudentFromList(List<Student> students)
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

    static void FilterStudentsByGroup(List<Student> students)
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

    // Вспомогательный метод для простого чтения XML
    static void ReadXmlFileSimple()
    {
        try
        {
            XDocument xdoc = XDocument.Load("students.xml");

            var students = from student in xdoc.Descendants("student")
                           select new
                           {
                               Name = student.Element("name")?.Value,
                               Age = student.Element("age")?.Value,
                               Group = student.Element("group")?.Value
                           };

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name} — {student.Age} лет — группа {student.Group}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении файла: {ex.Message}");
        }
    }
}

//1.ТЕХНОЛОГИИ ДЛЯ РАБОТЫ С XML:
//XmlDocument(устаревший) - DOM - подход, полная загрузка в память

//XDocument (современный) - LINQ to XML, удобный синтаксис

//XmlReader/XmlWriter - потоковое чтение/запись для больших файлов

//2. ОСНОВНЫЕ ЭЛЕМЕНТЫ XML:
//Элементы(Elements): < student > ...</ student >

//Атрибуты(Attributes): < student id = "1" > (не использовались в примере)

//Корневой элемент: Самый внешний элемент

//Декларация: <? xml version = "1.0" encoding = "utf-8" ?>

//3.LINQ TO XML ОСНОВНЫЕ МЕТОДЫ:
//XDocument.Load() - загрузка XML из файла

//Descendants() - получение всех элементов с указанным именем

//Elements() - получение дочерних элементов

//Element() - получение первого дочернего элемента

//Add() - добавление новых элементов

//Remove() - удаление элементов

//4. ПОИСК И ФИЛЬТРАЦИЯ:
//LINQ запросы: from...where...select

//Методы расширения: Where(), FirstOrDefault()

//XPath: SelectNodes(), SelectSingleNode() в XmlDocument

//5. CRUD ОПЕРАЦИИ:
//Create: Создание новых элементов с new XElement()

//Read: Загрузка и чтение данных с LINQ

//Update: Изменение значений элементов

//Delete: Удаление элементов с Remove()

//Программа демонстрирует полный цикл работы с XML-файлами от базового создания до продвинутой системы управления данными!

