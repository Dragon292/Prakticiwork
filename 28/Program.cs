using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ФАЙЛОВЫЙ МЕНЕДЖЕР ===\n");

        // Основные задания
        CreateAndWriteFile();      // Задание 1
        ReadFileContent();         // Задание 2
        GetFileInfo();             // Задание 3
        WorkWithDirectories();     // Задание 4
        CopyFileWithStream();      // Задание 5

        // Продвинутое задание - файловый менеджер
        FileManager();             // Задание 6

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }

    // Задание 1: Создание и запись файла
    static void CreateAndWriteFile()
    {
        Console.WriteLine("1. СОЗДАНИЕ И ЗАПИСЬ ФАЙЛА\n");

        try
        {
            // Создаем и записываем в файл
            using (StreamWriter writer = new StreamWriter("info.txt", false, Encoding.UTF8))
            {
                writer.WriteLine($"Имя пользователя: {Environment.UserName}");
                writer.WriteLine($"Дата и время: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                writer.WriteLine($"Операционная система: {Environment.OSVersion}");
                writer.WriteLine($"Версия .NET: {Environment.Version}");
                writer.WriteLine("=== КОНЕЦ ИНФОРМАЦИИ ===");
            }

            Console.WriteLine("✅ Файл 'info.txt' успешно создан и заполнен данными");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при создании файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 2: Чтение данных из файла
    static void ReadFileContent()
    {
        Console.WriteLine("2. ЧТЕНИЕ ДАННЫХ ИЗ ФАЙЛА\n");

        try
        {
            if (File.Exists("info.txt"))
            {
                Console.WriteLine("Содержимое файла 'info.txt':");
                Console.WriteLine(new string('=', 40));

                // Чтение всего содержимого файла
                string[] lines = File.ReadAllLines("info.txt", Encoding.UTF8);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }

                Console.WriteLine(new string('=', 40));
            }
            else
            {
                Console.WriteLine("❌ Файл 'info.txt' не найден!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 3: Работа с классом FileInfo
    static void GetFileInfo()
    {
        Console.WriteLine("3. ИНФОРМАЦИЯ О ФАЙЛЕ (FileInfo)\n");

        try
        {
            FileInfo fileInfo = new FileInfo("info.txt");

            if (fileInfo.Exists)
            {
                Console.WriteLine($"📄 Информация о файле: {fileInfo.Name}");
                Console.WriteLine($"📁 Полный путь: {fileInfo.FullName}");
                Console.WriteLine($"📏 Размер файла: {fileInfo.Length} байт");
                Console.WriteLine($"📅 Дата создания: {fileInfo.CreationTime:dd.MM.yyyy HH:mm:ss}");
                Console.WriteLine($"✏️ Дата изменения: {fileInfo.LastWriteTime:dd.MM.yyyy HH:mm:ss}");
                Console.WriteLine($"🔓 Дата последнего доступа: {fileInfo.LastAccessTime:dd.MM.yyyy HH:mm:ss}");
                Console.WriteLine($"👤 Атрибуты: {fileInfo.Attributes}");
                Console.WriteLine($"📂 Директория: {fileInfo.DirectoryName}");
            }
            else
            {
                Console.WriteLine("❌ Файл 'info.txt' не найден!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при получении информации о файле: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 4: Работа с каталогами (Directory)
    static void WorkWithDirectories()
    {
        Console.WriteLine("4. РАБОТА С КАТАЛОГАМИ\n");

        try
        {
            string dataDirectory = "Data";

            // Создаем каталог Data, если он не существует
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
                Console.WriteLine($"✅ Создан каталог: {dataDirectory}");
            }
            else
            {
                Console.WriteLine($"ℹ️ Каталог '{dataDirectory}' уже существует");
            }

            // Копируем файл в каталог Data с новым именем
            if (File.Exists("info.txt"))
            {
                string backupPath = Path.Combine(dataDirectory, "backup.txt");
                File.Copy("info.txt", backupPath, true); // true - разрешить перезапись
                Console.WriteLine($"✅ Файл скопирован как: {backupPath}");

                // Выводим список файлов в каталоге Data
                Console.WriteLine($"\n📋 Список файлов в каталоге '{dataDirectory}':");
                string[] files = Directory.GetFiles(dataDirectory);

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        Console.WriteLine($"  📄 {Path.GetFileName(file)} ({fi.Length} байт)");
                    }
                }
                else
                {
                    Console.WriteLine("  Каталог пуст");
                }
            }
            else
            {
                Console.WriteLine("❌ Исходный файл 'info.txt' не найден для копирования");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при работе с каталогами: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 5: Использование потоков (FileStream)
    static void CopyFileWithStream()
    {
        Console.WriteLine("5. КОПИРОВАНИЕ ФАЙЛА С ПОТОКОМ (FileStream)\n");

        try
        {
            string sourceFile = "info.txt";
            string destinationFile = "info_copy.txt";

            if (!File.Exists(sourceFile))
            {
                Console.WriteLine("❌ Исходный файл не найден!");
                return;
            }

            FileInfo sourceInfo = new FileInfo(sourceFile);
            long fileSize = sourceInfo.Length;

            Console.WriteLine($"📋 Копирование файла: {sourceFile} -> {destinationFile}");
            Console.WriteLine($"📏 Размер файла: {fileSize} байт");
            Console.WriteLine("⏳ Начало копирования...\n");

            // Используем FileStream для побайтового копирования
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (FileStream destStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[1024]; // Буфер 1 КБ
                int bytesRead;
                long totalBytesRead = 0;

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destStream.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;

                    // Выводим прогресс
                    double progress = (double)totalBytesRead / fileSize * 100;
                    Console.WriteLine($"📊 Прогресс: {totalBytesRead}/{fileSize} байт ({progress:F1}%)");

                    // Имитация медленного копирования для наглядности
                    Thread.Sleep(50);
                }
            }

            Console.WriteLine($"\n✅ Файл успешно скопирован: {destinationFile}");

            // Проверяем размер скопированного файла
            FileInfo destInfo = new FileInfo(destinationFile);
            Console.WriteLine($"📏 Размер скопированного файла: {destInfo.Length} байт");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при копировании файла: {ex.Message}");
        }

        Console.WriteLine();
    }

    // Задание 6: Продвинутое - Файловый менеджер
    static void FileManager()
    {
        Console.WriteLine("6. ФАЙЛОВЫЙ МЕНЕДЖЕР\n");

        string currentDirectory = Directory.GetCurrentDirectory();
        bool exitManager = false;

        while (!exitManager)
        {
            Console.Clear();
            Console.WriteLine("=== ФАЙЛОВЫЙ МЕНЕДЖЕР ===");
            Console.WriteLine($"Текущая директория: {currentDirectory}");
            Console.WriteLine(new string('=', 50));

            DisplayDirectoryContents(currentDirectory);

            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Перейти в папку");
            Console.WriteLine("2. Создать файл");
            Console.WriteLine("3. Создать папку");
            Console.WriteLine("4. Переименовать файл/папку");
            Console.WriteLine("5. Удалить файл/папку");
            Console.WriteLine("6. Открыть текстовый файл");
            Console.WriteLine("7. Редактировать текстовый файл");
            Console.WriteLine("8. Сменить директорию");
            Console.WriteLine("0. Выход из менеджера");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EnterDirectory(currentDirectory, ref currentDirectory);
                    break;
                case "2":
                    CreateFile(currentDirectory);
                    break;
                case "3":
                    CreateDirectory(currentDirectory);
                    break;
                case "4":
                    RenameFileOrDirectory(currentDirectory);
                    break;
                case "5":
                    DeleteFileOrDirectory(currentDirectory);
                    break;
                case "6":
                    OpenTextFile(currentDirectory);
                    break;
                case "7":
                    EditTextFile(currentDirectory);
                    break;
                case "8":
                    ChangeDirectory(ref currentDirectory);
                    break;
                case "0":
                    exitManager = true;
                    Console.WriteLine("Выход из файлового менеджера...");
                    break;
                default:
                    Console.WriteLine("❌ Неверный выбор! Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Отображение содержимого директории
    static void DisplayDirectoryContents(string directory)
    {
        try
        {
            Console.WriteLine("\n📂 ПАПКИ:");
            string[] directories = Directory.GetDirectories(directory);

            if (directories.Length > 0)
            {
                foreach (string dir in directories)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    Console.WriteLine($"  📁 {Path.GetFileName(dir)}");
                }
            }
            else
            {
                Console.WriteLine("  Нет папок");
            }

            Console.WriteLine("\n📄 ФАЙЛЫ:");
            string[] files = Directory.GetFiles(directory);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    Console.WriteLine($"  📄 {Path.GetFileName(file)} ({fi.Length} байт)");
                }
            }
            else
            {
                Console.WriteLine("  Нет файлов");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении директории: {ex.Message}");
        }
    }

    // Переход в папку
    static void EnterDirectory(string currentDir, ref string newCurrentDir)
    {
        Console.Write("\nВведите имя папки: ");
        string folderName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(folderName))
        {
            Console.WriteLine("❌ Имя папки не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string newPath = Path.Combine(currentDir, folderName);

        try
        {
            if (Directory.Exists(newPath))
            {
                newCurrentDir = newPath;
                Console.WriteLine($"✅ Переход в папку: {folderName}");
            }
            else
            {
                Console.WriteLine($"❌ Папка '{folderName}' не найдена!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Создание файла
    static void CreateFile(string directory)
    {
        Console.Write("\nВведите имя файла: ");
        string fileName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("❌ Имя файла не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string filePath = Path.Combine(directory, fileName);

        try
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, $"Файл создан: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n", Encoding.UTF8);
                Console.WriteLine($"✅ Файл '{fileName}' успешно создан");
            }
            else
            {
                Console.WriteLine($"❌ Файл '{fileName}' уже существует!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при создании файла: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Создание папки
    static void CreateDirectory(string directory)
    {
        Console.Write("\nВведите имя папки: ");
        string folderName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(folderName))
        {
            Console.WriteLine("❌ Имя папки не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string newDirPath = Path.Combine(directory, folderName);

        try
        {
            if (!Directory.Exists(newDirPath))
            {
                Directory.CreateDirectory(newDirPath);
                Console.WriteLine($"✅ Папка '{folderName}' успешно создана");
            }
            else
            {
                Console.WriteLine($"❌ Папка '{folderName}' уже существует!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при создании папки: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Переименование файла или папки
    static void RenameFileOrDirectory(string directory)
    {
        Console.Write("\nВведите текущее имя файла/папки: ");
        string oldName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(oldName))
        {
            Console.WriteLine("❌ Имя не может быть пустым!");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите новое имя: ");
        string newName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("❌ Новое имя не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string oldPath = Path.Combine(directory, oldName);
        string newPath = Path.Combine(directory, newName);

        try
        {
            if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
                Console.WriteLine($"✅ Файл '{oldName}' переименован в '{newName}'");
            }
            else if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
                Console.WriteLine($"✅ Папка '{oldName}' переименована в '{newName}'");
            }
            else
            {
                Console.WriteLine($"❌ Файл/папка '{oldName}' не найдена!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при переименовании: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Удаление файла или папки
    static void DeleteFileOrDirectory(string directory)
    {
        Console.Write("\nВведите имя файла/папки для удаления: ");
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Имя не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string path = Path.Combine(directory, name);

        try
        {
            if (File.Exists(path))
            {
                Console.Write($"Вы уверены, что хотите удалить файл '{name}'? (y/n): ");
                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y")
                {
                    File.Delete(path);
                    Console.WriteLine($"✅ Файл '{name}' удален");
                }
                else
                {
                    Console.WriteLine("❌ Удаление отменено");
                }
            }
            else if (Directory.Exists(path))
            {
                Console.Write($"Вы уверены, что хотите удалить папку '{name}'? (y/n): ");
                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y")
                {
                    Directory.Delete(path, true); // true - удалить рекурсивно
                    Console.WriteLine($"✅ Папка '{name}' удалена");
                }
                else
                {
                    Console.WriteLine("❌ Удаление отменено");
                }
            }
            else
            {
                Console.WriteLine($"❌ Файл/папка '{name}' не найдена!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при удалении: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Открытие текстового файла
    static void OpenTextFile(string directory)
    {
        Console.Write("\nВведите имя текстового файла: ");
        string fileName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("❌ Имя файла не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string filePath = Path.Combine(directory, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine($"\n📖 СОДЕРЖИМОЕ ФАЙЛА '{fileName}':");
                Console.WriteLine(new string('=', 50));

                string content = File.ReadAllText(filePath, Encoding.UTF8);
                Console.WriteLine(content);

                Console.WriteLine(new string('=', 50));
                FileInfo fi = new FileInfo(filePath);
                Console.WriteLine($"Размер: {fi.Length} байт, кодировка: UTF-8");
            }
            else
            {
                Console.WriteLine($"❌ Файл '{fileName}' не найден!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при чтении файла: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    // Редактирование текстового файла
    static void EditTextFile(string directory)
    {
        Console.Write("\nВведите имя текстового файла для редактирования: ");
        string fileName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("❌ Имя файла не может быть пустым!");
            Console.ReadKey();
            return;
        }

        string filePath = Path.Combine(directory, fileName);

        try
        {
            string currentContent = "";

            if (File.Exists(filePath))
            {
                currentContent = File.ReadAllText(filePath, Encoding.UTF8);
                Console.WriteLine($"\n📝 РЕДАКТИРОВАНИЕ ФАЙЛА '{fileName}'");
                Console.WriteLine("Текущее содержимое:");
                Console.WriteLine(new string('=', 30));
                Console.WriteLine(currentContent);
                Console.WriteLine(new string('=', 30));
            }
            else
            {
                Console.WriteLine($"\n📝 СОЗДАНИЕ НОВОГО ФАЙЛА '{fileName}'");
            }

            Console.WriteLine("\nВведите новое содержимое (для завершения введите 'END' на отдельной строке):");
            Console.WriteLine("(Поддерживается многострочный ввод)");

            StringBuilder newContent = new StringBuilder();
            string line;

            while ((line = Console.ReadLine()) != "END")
            {
                newContent.AppendLine(line);
            }

            File.WriteAllText(filePath, newContent.ToString(), Encoding.UTF8);
            Console.WriteLine($"✅ Файл '{fileName}' успешно сохранен");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при редактировании файла: {ex.Message}");
        }

        Console.ReadKey();
    }

    // Смена директории
    static void ChangeDirectory(ref string currentDirectory)
    {
        Console.Write("\nВведите новый путь к директории: ");
        string newPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newPath))
        {
            Console.WriteLine("❌ Путь не может быть пустым!");
            Console.ReadKey();
            return;
        }

        try
        {
            if (Directory.Exists(newPath))
            {
                currentDirectory = newPath;
                Console.WriteLine($"✅ Текущая директория изменена на: {newPath}");
            }
            else
            {
                Console.WriteLine($"❌ Директория '{newPath}' не найдена!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        Console.ReadKey();
    }
}


//1.КЛАССЫ ДЛЯ РАБОТЫ С ФАЙЛАМИ:
//File - статические методы для операций с файлами

//FileInfo - экземплярные методы, кэширует информацию о файле

//Directory - статические методы для работы с каталогами

//DirectoryInfo - экземплярные методы для каталогов

//FileStream - низкоуровневый доступ для побайтовой работы

//StreamReader/StreamWriter - высокоуровневый доступ для текста

//2. ОСНОВНЫЕ ОПЕРАЦИИ:
//Создание: File.Create(), File.WriteAllText()

//Чтение: File.ReadAllText(), File.ReadAllLines()

//Копирование: File.Copy(), побайтовое копирование через Stream

//Удаление: File.Delete(), Directory.Delete()

//Информация: Размер, даты создания/изменения, атрибуты

//3. ОБРАБОТКА ОШИБОК:
//FileNotFoundException - файл не найден

//DirectoryNotFoundException - каталог не найден

//UnauthorizedAccessException - нет прав доступа

//IOException - общие ошибки ввода-вывода

//4. КОДИРОВКИ (ENCODING):
//UTF - 8 - стандартная кодировка для текстовых файлов

//ASCII - только английские символы

//Unicode - UTF-16 для Windows

//Default - системная кодировка

//Программа демонстрирует все основные операции с файловой системой от простых до продвинутых!