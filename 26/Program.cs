//1.GENERICS(ОБОБЩЕНИЯ)
//Механизм для создания типобезопасных классов, структур, интерфейсов и методов с параметрами типов.

//2. ОСНОВНЫЕ ПРЕИМУЩЕСТВА
//Type Safety - безопасность типов во время компиляции

//Performance - отсутствие boxing/unboxing для значимых типов

//Code Reuse - повторное использование кода для разных типов

//3. ОГРАНИЧЕНИЯ ТИПОВ (CONSTRAINTS)
//where T : struct -T должен быть значимым типом

//where T : class -T должен быть ссылочным типом

//where T : new() - T должен иметь конструктор по умолчанию

//where T : BaseClass - T должен наследоваться от BaseClass

//where T : Interface - T должен реализовывать интерфейс

//4. VARIANCE (ВАРИАНТНОСТЬ)
//Ковариантность (out) - возможность использовать более производный тип

//Контравариантность (in) - возможность использовать более базовый тип

//Инвариантность - только указанный тип

//5. СТАНДАРТНЫЕ GENERIC-КОЛЛЕКЦИИ
//List<T> - динамический массив

//Dictionary<TKey, TValue> - словарь ключ-значение

//Queue<T> - очередь (FIFO)

//Stack<T> - стек (LIFO)

//HashSet<T> - множество уникальных элементов

//Generics обеспечивают типобезопасность, производительность и повторное использование кода!



// Задание 1: Базовые обобщенные типы

/// <summary>
/// Обобщенный класс Box<T> демонстрирует основную концепцию Generics - параметризацию типов
/// T - это параметр типа, который будет заменен конкретным типом при создании экземпляра
/// Это позволяет создавать типобезопасные контейнеры без дублирования кода
/// </summary>
/// <typeparam name="T">Тип элемента, хранимого в коробке</typeparam>
public class Box<T>
{
    // Приватное поле типа T - компилятор заменит T на конкретный тип
    private T _item;

    /// <summary>
    /// Конструктор принимает элемент типа T
    /// На этапе компиляции будет создана специализированная версия класса для каждого используемого типа
    /// </summary>
    public Box(T item)
    {
        _item = item;
    }

    /// <summary>
    /// Метод возвращает элемент типа T
    /// Type safety: компилятор гарантирует, что возвращаемый тип соответствует T
    /// </summary>
    public T GetItem()
    {
        return _item;
    }

    /// <summary>
    /// Метод устанавливает новое значение элемента
    /// Type safety: компилятор проверяет, что newItem имеет правильный тип
    /// </summary>
    public void SetItem(T newItem)
    {
        _item = newItem;
    }

    /// <summary>
    /// Вспомогательный метод для демонстрации работы generics
    /// typeof(T) возвращает реальный тип, который был подставлен вместо T
    /// </summary>
    public void PrintItem()
    {
        Console.WriteLine($"Содержимое коробки: {_item} (тип: {typeof(T).Name})");
    }
}

// Задание 3: Обобщенные методы

/// <summary>
/// Статический класс с обобщенными методами демонстрирует, что generics можно применять не только к классам,
/// но и к отдельным методам. Это позволяет создавать переиспользуемые алгоритмы.
/// </summary>
public static class GenericUtils
{
    /// <summary>
    /// Обобщенный метод для обмена значений двух переменных
    /// Демонстрирует параметризацию метода - работает с любыми типами
    /// ref позволяет изменять оригинальные переменные, а не их копии
    /// </summary>
    /// <typeparam name="T">Тип переменных для обмена</typeparam>
    public static void Swap<T>(ref T a, ref T b)
    {
        // Временная переменная типа T - компилятор выведет конкретный тип
        T temp = a;
        a = b;
        b = temp;
    }

    /// <summary>
    /// Обобщенный метод с ограничением where T : IComparable<T>
    /// Ограничение гарантирует, что тип T реализует интерфейс IComparable<T>,
    /// что позволяет использовать метод CompareTo для сравнения элементов
    /// Без этого ограничения компилятор не знал бы, как сравнивать элементы типа T
    /// </summary>
    public static T FindMax<T>(IEnumerable<T> collection) where T : IComparable<T>
    {
        if (collection == null || !collection.Any())
            throw new ArgumentException("Коллекция не может быть пустой");

        T max = collection.First();

        // Благодаря ограничению IComparable<T> можем использовать CompareTo
        foreach (T item in collection)
        {
            // CompareTo возвращает > 0 если item больше max
            if (item.CompareTo(max) > 0)
                max = item;
        }
        return max;
    }

    /// <summary>
    /// Универсальный метод для вывода любой коллекции
    /// IEnumerable<T> - базовый интерфейс для всех коллекций в .NET
    /// Работает с List<T>, T[], HashSet<T> и любыми другими коллекциями
    /// </summary>
    public static void PrintCollection<T>(IEnumerable<T> collection, string title = "Коллекция")
    {
        // string.Join работает с любыми типами, вызывая ToString() для каждого элемента
        Console.WriteLine($"{title}: [{string.Join(", ", collection)}]");
    }

    /// <summary>
    /// Метод с двумя параметрами типа демонстрирует преобразование между разными типами
    /// TInput - тип входных данных, TOutput - тип выходных данных
    /// Func<TInput, TOutput> - делегат для функции преобразования
    /// </summary>
    public static IEnumerable<TOutput> ConvertCollection<TInput, TOutput>(
        IEnumerable<TInput> collection,
        Func<TInput, TOutput> converter)
    {
        // LINQ Select применяет функцию преобразования к каждому элементу
        return collection.Select(converter);
    }
}

// Задание 4: Ограничения типов (constraints)

/// <summary>
/// Интерфейс-маркер для объектов, которые могут быть проверены на валидность
/// Используется как ограничение в обобщенном классе Validator
/// </summary>
public interface IValidatable
{
    bool IsValid();
}

/// <summary>
/// Класс AppUser реализует IValidatable для демонстрации ограничений
/// Переименован из User чтобы избежать конфликта с другим классом User
/// </summary>
public class AppUser : IValidatable
{
    public string Name { get; set; }
    public int Age { get; set; }

    /// <summary>
    /// Реализация метода проверки валидности
    /// Благодаря этому методу AppUser можно использовать с Validator<T>
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Name) && Age > 0 && Age < 150;
    }

    public override string ToString()
    {
        return $"{Name} ({Age} лет)";
    }
}

/// <summary>
/// Обобщенный класс валидатора с ограничением where T : IValidatable
/// Ограничение гарантирует, что у всех объектов типа T есть метод IsValid()
/// Без ограничения компилятор не позволил бы вызвать item.IsValid()
/// </summary>
/// <typeparam name="T">Тип должен реализовывать IValidatable</typeparam>
public class Validator<T> where T : IValidatable
{
    /// <summary>
    /// Метод может работать с любым типом T, но только если T реализует IValidatable
    /// Это пример ограничения на уровне интерфейса
    /// </summary>
    public bool Validate(T item)
    {
        // Метод IsValid доступен благодаря ограничению where T : IValidatable
        return item.IsValid();
    }

    /// <summary>
    /// Метод фильтрует коллекцию, оставляя только валидные элементы
    /// Демонстрирует работу с коллекциями generic-типов
    /// </summary>
    public List<T> ValidateCollection(IEnumerable<T> items)
    {
        // LINQ Where + ToList работают с generic-типами
        return items.Where(item => item.IsValid()).ToList();
    }
}

/// <summary>
/// Класс с несколькими ограничениями демонстрирует комбинирование constraints
/// where T : IComparable - можно сравнивать объекты
/// where T : new() - можно создавать экземпляры с конструктором по умолчанию
/// </summary>
public class GenericWithConstraints<T> where T : IComparable, new()
{
    /// <summary>
    /// Метод демонстрирует преимущества ограничений:
    /// - new T() работает благодаря where T : new()
    /// - CompareTo работает благодаря where T : IComparable
    /// </summary>
    public T CreateAndCompare(T other)
    {
        // Создание экземпляра возможно благодаря ограничению new()
        T newInstance = new T();
        Console.WriteLine($"Создан новый экземпляр: {newInstance}");

        // Сравнение возможно благодаря ограничению IComparable
        Console.WriteLine($"Сравнение: {newInstance.CompareTo(other)}");
        return newInstance;
    }
}

// Задание 5: Обобщенные интерфейсы и наследование

/// <summary>
/// Базовый интерфейс репозитория с generic-параметром
/// where T : class - ограничение, что T должен быть ссылочным типом
/// Это предотвращает использование значимых типов (int, struct) в репозитории
/// </summary>
public interface IRepository<T> where T : class
{
    void Add(T item);
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Update(T item);
    void Delete(int id);
}

/// <summary>
/// Абстрактный базовый класс с общей логикой для репозиториев
/// Наследует IRepository<T> и реализует часть методов
/// Шаблон Template Method - общая логика в базовом классе, специфичная в производных
/// </summary>
public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    // Защищенные поля доступны в производных классах
    protected List<T> _items = new List<T>();
    protected int _nextId = 1;

    /// <summary>
    /// Виртуальный метод может быть переопределен в производных классах
    /// Базовая реализация просто добавляет элемент в список
    /// </summary>
    public virtual void Add(T item)
    {
        _items.Add(item);
        Console.WriteLine($"Добавлен элемент: {item}");
    }

    // Абстрактные методы должны быть реализованы в производных классах
    // Это заставляет каждую конкретную реализацию определить свою логику
    public abstract T GetById(int id);
    public abstract void Update(T item);
    public abstract void Delete(int id);

    /// <summary>
    /// Виртуальный метод получения всех элементов
    /// Производные классы могут переопределить при необходимости
    /// </summary>
    public virtual IEnumerable<T> GetAll()
    {
        return _items;
    }
}

/// <summary>
/// Класс RepositoryUser для демонстрации репозитория
/// Переименован из User чтобы избежать конфликта имен
/// </summary>
public class RepositoryUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public override string ToString()
    {
        return $"User[Id: {Id}, Name: {Name}, Email: {Email}]";
    }
}

/// <summary>
/// Класс Product для демонстрации работы с разными типами в репозиториях
/// Показывает, что один базовый класс RepositoryBase может работать с разными типами
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"Product[Id: {Id}, Name: {Name}, Price: {Price:C}]";
    }
}

/// <summary>
/// Конкретная реализация репозитория для пользователей
/// Наследует общую логику от RepositoryBase и реализует специфичные методы
/// Явно реализует IReadableRepository для демонстрации ковариантности
/// </summary>
public class UserRepository : RepositoryBase<RepositoryUser>, IReadableRepository<RepositoryUser>
{
    /// <summary>
    /// Реализация поиска по ID - специфичная для пользователей
    /// Использует LINQ для поиска в коллекции
    /// </summary>
    public override RepositoryUser GetById(int id)
    {
        return _items.FirstOrDefault(u => u.Id == id);
    }

    /// <summary>
    /// Реализация обновления - заменяет старый объект новым
    /// В реальном приложении здесь была бы более сложная логика
    /// </summary>
    public override void Update(RepositoryUser item)
    {
        var existing = GetById(item.Id);
        if (existing != null)
        {
            _items.Remove(existing);
            _items.Add(item);
            Console.WriteLine($"Обновлен пользователь: {item}");
        }
    }

    /// <summary>
    /// Реализация удаления по ID
    /// Демонстрирует работу с generic-коллекцией
    /// </summary>
    public override void Delete(int id)
    {
        var user = GetById(id);
        if (user != null)
        {
            _items.Remove(user);
            Console.WriteLine($"Удален пользователь с ID: {id}");
        }
    }

    /// <summary>
    /// Переопределение метода добавления с автоматической установкой ID
    /// Демонстрирует расширение функциональности базового класса
    /// </summary>
    public override void Add(RepositoryUser item)
    {
        item.Id = _nextId++;
        base.Add(item);
    }
}

/// <summary>
/// Репозиторий продуктов демонстрирует повторное использование кода
/// Та же структура, что и UserRepository, но для другого типа
/// </summary>
public class ProductRepository : RepositoryBase<Product>
{
    public override Product GetById(int id)
    {
        return _items.FirstOrDefault(p => p.Id == id);
    }

    public override void Update(Product item)
    {
        var existing = GetById(item.Id);
        if (existing != null)
        {
            _items.Remove(existing);
            _items.Add(item);
            Console.WriteLine($"Обновлен продукт: {item}");
        }
    }

    public override void Delete(int id)
    {
        var product = GetById(id);
        if (product != null)
        {
            _items.Remove(product);
            Console.WriteLine($"Удален продукт с ID: {id}");
        }
    }

    public override void Add(Product item)
    {
        item.Id = _nextId++;
        base.Add(item);
    }
}

/// <summary>
/// Ковариантный интерфейс только для чтения
/// out T - ключевое слово для ковариантности
/// Ковариантность позволяет использовать производный тип вместо базового
/// Например: IReadableRepository<RepositoryUser> можно присвоить IReadableRepository<object>
/// потому что RepositoryUser наследуется от object
/// </summary>
/// <typeparam name="T">Ковариантный параметр типа</typeparam>
public interface IReadableRepository<out T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
}

/// <summary>
/// Контравариантный интерфейс только для записи
/// in T - ключевое слово для контравариантности
/// Контравариантность позволяет использовать базовый тип вместо производного
/// Например: IWritableRepository<object> можно присвоить IWritableRepository<RepositoryUser>
/// потому что можно записать RepositoryUser (производный) в коллекцию object (базовый)
/// </summary>
/// <typeparam name="T">Контравариантный параметр типа</typeparam>
public interface IWritableRepository<in T> where T : class
{
    void Add(T item);
    void Update(T item);
}

/// <summary>
/// Non-generic класс для сравнения с generic-подходом
/// Демонстрирует проблемы, которые решают generics:
/// - Необходимость приведения типов
/// - Возможность ошибок во время выполнения
/// - Отсутствие type safety
/// </summary>
public class NonGenericBox
{
    // object может хранить любой тип, но без type safety
    private object _item;

    public void SetItem(object item)
    {
        _item = item;
    }

    public object GetItem()
    {
        return _item;
    }
}

/// <summary>
/// Реализация репозитория для любых объектов
/// Используется для демонстрации контравариантности
/// Может принимать любой объект, так как все типы наследуются от object
/// </summary>
public class ObjectRepository : IWritableRepository<object>
{
    private List<object> _items = new List<object>();

    public void Add(object item)
    {
        _items.Add(item);
        Console.WriteLine($"Добавлен объект: {item}");
    }

    public void Update(object item)
    {
        Console.WriteLine($"Обновлен объект: {item}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ GENERICS В C# ===\n");

        // Последовательное выполнение всех заданий
        DemonstrateBasicGenerics();        // Задание 1: Базовые обобщенные типы
        DemonstrateGenericCollections();   // Задание 2: Стандартные generic-коллекции  
        DemonstrateGenericMethods();       // Задание 3: Обобщенные методы
        DemonstrateTypeConstraints();      // Задание 4: Ограничения типов
        DemonstrateGenericInterfaces();    // Задание 5: Обобщенные интерфейсы

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
    }

    static void DemonstrateBasicGenerics()
    {
        Console.WriteLine("1. БАЗОВЫЕ ОБОБЩЕННЫЕ ТИПЫ:\n");
        Console.WriteLine("Демонстрация параметризации типов и type safety\n");

        Console.WriteLine("--- Box с разными типами данных ---");
        Console.WriteLine("Создание специализированных версий Box<T> для разных типов:\n");

        // Компилятор создает Box<int> - специализированную версию для int
        Box<int> intBox = new Box<int>(42);
        intBox.PrintItem();

        // Компилятор создает Box<string> - специализированную версию для string
        Box<string> stringBox = new Box<string>("Hello Generics!");
        stringBox.PrintItem();

        // Компилятор создает Box<double> - специализированную версию для double
        Box<double> doubleBox = new Box<double>(3.14159);
        doubleBox.PrintItem();

        Console.WriteLine("\n--- Методы GetItem и SetItem ---");
        Console.WriteLine("Type safety: компилятор проверяет типы на этапе компиляции\n");
        Console.WriteLine($"Исходное значение: {intBox.GetItem()}");
        intBox.SetItem(100); // OK - тип int
        // intBox.SetItem("строка"); // ОШИБКА КОМПИЛЯЦИИ - несовместимые типы
        Console.WriteLine($"Новое значение: {intBox.GetItem()}");

        Console.WriteLine("\n--- Type Safety ---");
        Console.WriteLine("Generics предотвращают ошибки типов на этапе компиляции");
        Console.WriteLine("В отличие от non-generic подхода, где ошибки возникают во время выполнения\n");

        Console.WriteLine("--- Сравнение с non-generic подходом ---");
        Console.WriteLine("Non-generic подход использует object, что приводит к:");
        Console.WriteLine("- Необходимости приведения типов");
        Console.WriteLine("- Возможным ошибкам во время выполнения\n");

        NonGenericBox nonGenericBox = new NonGenericBox();
        nonGenericBox.SetItem(42); // Безопасно
        nonGenericBox.SetItem("строка"); // Опасно - теряем информацию о типе

        try
        {
            // Опасное приведение - может вызвать InvalidCastException
            int value = (int)nonGenericBox.GetItem();
            Console.WriteLine($"Значение: {value}");
        }
        catch (InvalidCastException)
        {
            Console.WriteLine("ОШИБКА: Неверное приведение типов во время выполнения!");
            Console.WriteLine("Эта ошибка могла быть предотвращена с помощью Generics\n");
        }
    }

    static void DemonstrateGenericCollections()
    {
        Console.WriteLine("\n\n2. СТАНДАРТНЫЕ GENERIC-КОЛЛЕКЦИИ:\n");
        Console.WriteLine("Демонстрация встроенных generic-коллекций .NET\n");

        Console.WriteLine("--- List<T> (динамический массив) ---");
        Console.WriteLine("Type-safe аналог ArrayList - хранит только указанный тип\n");
        List<string> names = new List<string> { "Анна", "Борис", "Виктор" };
        names.Add("Дарья"); // Можно добавить только string
        names.Add("Елена");
        // names.Add(123); // ОШИБКА КОМПИЛЯЦИИ - несовместимый тип
        Console.WriteLine("Список имен: " + string.Join(", ", names));

        Console.WriteLine("\n--- Dictionary<TKey, TValue> (словарь) ---");
        Console.WriteLine("Type-safe аналог Hashtable - строгая типизация ключей и значений\n");
        Dictionary<int, string> employees = new Dictionary<int, string>
        {
            [1] = "Иван Иванов", // Ключ int, значение string
            [2] = "Петр Петров",
            [3] = "Мария Сидорова"
        };
        employees.Add(4, "Анна Козлова");
        // employees.Add("пять", "Ошибка"); // ОШИБКА - ключ должен быть int

        foreach (var emp in employees)
        {
            // emp.Key - int, emp.Value - string (type safety)
            Console.WriteLine($"ID: {emp.Key}, Name: {emp.Value}");
        }

        Console.WriteLine("\n--- Queue<T> (FIFO - очередь) ---");
        Console.WriteLine("Очередь First-In-First-Out с типобезопасностью\n");
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(1); // Добавление в конец
        queue.Enqueue(2);
        queue.Enqueue(3);

        Console.WriteLine("Очередь: " + string.Join(" -> ", queue));
        Console.WriteLine($"Dequeue: {queue.Dequeue()}"); // Извлечение из начала (первый добавленный)
        Console.WriteLine($"Dequeue: {queue.Dequeue()}");

        Console.WriteLine("\n--- Stack<T> (LIFO - стек) ---");
        Console.WriteLine("Стек Last-In-First-Out с типобезопасностью\n");
        Stack<int> stack = new Stack<int>();
        stack.Push(1); // Добавление на вершину
        stack.Push(2);
        stack.Push(3);

        Console.WriteLine("Стек: " + string.Join(" <- ", stack));
        Console.WriteLine($"Pop: {stack.Pop()}"); // Извлечение с вершины (последний добавленный)
        Console.WriteLine($"Pop: {stack.Pop()}");

        Console.WriteLine("\n--- HashSet<T> (множество) ---");
        Console.WriteLine("Коллекция уникальных элементов с операциями над множествами\n");
        HashSet<int> set1 = new HashSet<int> { 1, 2, 3, 4, 5 };
        HashSet<int> set2 = new HashSet<int> { 4, 5, 6, 7, 8 };

        Console.WriteLine($"Set1: {string.Join(", ", set1)}");
        Console.WriteLine($"Set2: {string.Join(", ", set2)}");
        Console.WriteLine($"Объединение (Union): {string.Join(", ", set1.Union(set2))}");
        Console.WriteLine($"Пересечение (Intersect): {string.Join(", ", set1.Intersect(set2))}");
        Console.WriteLine($"Разность (Except): {string.Join(", ", set1.Except(set2))}");
    }

    static void DemonstrateGenericMethods()
    {
        Console.WriteLine("\n\n3. ОБОБЩЕННЫЕ МЕТОДЫ:\n");
        Console.WriteLine("Демонстрация параметризации отдельных методов\n");

        Console.WriteLine("--- Swap<T> (обмен значений) ---");
        Console.WriteLine("Один метод работает с разными типами благодаря type inference\n");

        int a = 10, b = 20;
        Console.WriteLine($"До swap: a = {a}, b = {b}");
        GenericUtils.Swap(ref a, ref b); // Компилятор выводит T = int
        Console.WriteLine($"После swap: a = {a}, b = {b}");

        string str1 = "Hello", str2 = "World";
        Console.WriteLine($"До swap: str1 = {str1}, str2 = {str2}");
        GenericUtils.Swap(ref str1, ref str2); // Компилятор выводит T = string
        Console.WriteLine($"После swap: str1 = {str1}, str2 = {str2}");

        Console.WriteLine("\n--- FindMax<T> с ограничением IComparable ---");
        Console.WriteLine("Ограничения гарантируют, что тип поддерживает необходимые операции\n");

        List<int> numbers = new List<int> { 5, 2, 8, 1, 9, 3 };
        GenericUtils.PrintCollection(numbers, "Числа");
        // int реализует IComparable<int>, поэтому можно использовать FindMax
        Console.WriteLine($"Максимальное число: {GenericUtils.FindMax(numbers)}");

        List<string> words = new List<string> { "яблоко", "банан", "апельсин" };
        GenericUtils.PrintCollection(words, "Слова");
        // string реализует IComparable<string>, поэтому можно использовать FindMax
        Console.WriteLine($"Максимальное слово (лексикографически): {GenericUtils.FindMax(words)}");

        Console.WriteLine("\n--- ConvertCollection<TInput, TOutput> (преобразование) ---");
        Console.WriteLine("Метод с двумя параметрами типа для преобразования между типами\n");

        List<int> integers = new List<int> { 1, 2, 3, 4, 5 };
        // Преобразование int в int (вычисление квадратов)
        var squares = GenericUtils.ConvertCollection(integers, x => x * x);
        GenericUtils.PrintCollection(squares, "Квадраты чисел");

        List<double> doubles = new List<double> { 1.1, 2.2, 3.3 };
        // Преобразование double в string (форматирование)
        var strings = GenericUtils.ConvertCollection(doubles, d => $"Value: {d:F1}");
        GenericUtils.PrintCollection(strings, "Преобразованные строки");
    }

    static void DemonstrateTypeConstraints()
    {
        Console.WriteLine("\n\n4. ОГРАНИЧЕНИЯ ТИПОВ (CONSTRAINTS):\n");
        Console.WriteLine("Демонстрация ограничений, которые гарантируют наличие определенных возможностей у типа\n");

        Console.WriteLine("--- Validator с ограничением IValidatable ---");
        Console.WriteLine("Ограничение where T : IValidatable гарантирует, что у T есть метод IsValid()\n");

        Validator<AppUser> userValidator = new Validator<AppUser>();

        AppUser validUser = new AppUser { Name = "Иван", Age = 25 };
        AppUser invalidUser = new AppUser { Name = "", Age = -5 }; // Невалидные данные

        Console.WriteLine($"Валидный пользователь: {validUser} - {userValidator.Validate(validUser)}");
        Console.WriteLine($"Невалидный пользователь: {invalidUser} - {userValidator.Validate(invalidUser)}");

        Console.WriteLine("\n--- GenericWithConstraints с ограничениями new() и IComparable ---");
        Console.WriteLine("Комбинирование ограничений: создание экземпляров + сравнение\n");

        GenericWithConstraints<int> constrainedGeneric = new GenericWithConstraints<int>();
        // int имеет конструктор по умолчанию и реализует IComparable
        constrainedGeneric.CreateAndCompare(10);

        Console.WriteLine("\n--- Ограничения в методах ---");
        Console.WriteLine("Те же ограничения можно применять к методам\n");

        List<decimal> prices = new List<decimal> { 10.5m, 20.3m, 15.7m };
        // decimal реализует IComparable<decimal>, поэтому можно использовать FindMax
        decimal maxPrice = GenericUtils.FindMax(prices);
        Console.WriteLine($"Максимальная цена: {maxPrice}");
    }

    static void DemonstrateGenericInterfaces()
    {
        Console.WriteLine("\n\n5. ОБОБЩЕННЫЕ ИНТЕРФЕЙСЫ И НАСЛЕДОВАНИЕ:\n");
        Console.WriteLine("Демонстрация наследования generic-классов и вариантности\n");

        Console.WriteLine("--- UserRepository (репозиторий пользователей) ---");
        Console.WriteLine("Конкретная реализация generic-репозитория\n");

        UserRepository userRepo = new UserRepository();
        userRepo.Add(new RepositoryUser { Name = "Иван Иванов", Email = "ivan@mail.com" });
        userRepo.Add(new RepositoryUser { Name = "Петр Петров", Email = "petr@mail.com" });

        Console.WriteLine("Все пользователи:");
        foreach (var userItem in userRepo.GetAll())
        {
            Console.WriteLine($"  {userItem}");
        }

        Console.WriteLine("\n--- ProductRepository (репозиторий продуктов) ---");
        Console.WriteLine("Другая реализация того же базового класса для другого типа\n");

        ProductRepository productRepo = new ProductRepository();
        productRepo.Add(new Product { Name = "Ноутбук", Price = 50000 });
        productRepo.Add(new Product { Name = "Мышь", Price = 1500 });

        Console.WriteLine("Все продукты:");
        foreach (var product in productRepo.GetAll())
        {
            Console.WriteLine($"  {product}");
        }

        Console.WriteLine("\n--- Обновление пользователя ---");
        Console.WriteLine("Демонстрация работы с конкретными объектами\n");

        var userToUpdate = userRepo.GetById(1);
        if (userToUpdate != null)
        {
            userToUpdate.Email = "ivan.new@mail.com";
            userRepo.Update(userToUpdate);
        }

        Console.WriteLine("\n--- Ковариантность (out T) ---");
        Console.WriteLine("Ковариантность позволяет использовать производный тип как базовый\n");
        Console.WriteLine("IReadableRepository<RepositoryUser> → IReadableRepository<object>\n");

        // Ковариантность: производный тип можно использовать как базовый
        IReadableRepository<RepositoryUser> userReadRepo = userRepo;
        IReadableRepository<object> objectReadRepo = userReadRepo; // Ковариантность в действии

        var firstUser = objectReadRepo.GetById(1);
        Console.WriteLine($"Первый пользователь через object репозиторий: {firstUser}");

        Console.WriteLine("\n--- Контравариантность (in T) ---");
        Console.WriteLine("Контравариантность позволяет использовать базовый тип как производный\n");
        Console.WriteLine("IWritableRepository<object> → IWritableRepository<RepositoryUser>\n");

        IWritableRepository<object> objectWriteRepo = new ObjectRepository();
        IWritableRepository<RepositoryUser> userWriteRepo = objectWriteRepo; // Контравариантность в действии

        userWriteRepo.Add(new RepositoryUser { Name = "Новый пользователь", Email = "new@mail.com" });
    }
}