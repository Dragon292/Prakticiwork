using System;
using System.Collections.Generic;
using System.Linq;

// Задание 1: Базовые делегаты для математических операций
public delegate int MathOperation(int a, int b);
public delegate void SimpleAction();
public delegate bool NumberCheck(int number);

// Задание 3: Класс для демонстрации событий с лямбдами
public class NotificationService
{
    // События для демонстрации разных способов подписки
    public event Action<string> MessageReceived;
    public event EventHandler<string> StandardMessageReceived;

    public void SendMessage(string message)
    {
        Console.WriteLine($"Служба отправляет: {message}");
        MessageReceived?.Invoke(message);
        StandardMessageReceived?.Invoke(this, message);
    }
}

// Класс Product для демонстрации LINQ с лямбдами
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public int Stock { get; set; }

    public Product(string name, decimal price, string category, int stock)
    {
        Name = name;
        Price = price;
        Category = category;
        Stock = stock;
    }

    public override string ToString()
    {
        return $"{Name} - ${Price} ({Category}), в наличии: {Stock}";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ЛЯМБДА-ВЫРАЖЕНИЙ ===\n");

        // Задание 1: Базовые лямбда-выражения
        DemonstrateBasicLambdas();

        // Задание 2: Лямбды с встроенными делегатами
        DemonstrateBuiltInDelegates();

        // Задание 3: Лямбды в событиях
        DemonstrateEventLambdas();

        // Задание 4: Лямбды в LINQ
        DemonstrateLinqLambdas();

        // Задание 5: Захват переменных в лямбдах
        DemonstrateVariableCapture();

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
    }

    static void DemonstrateBasicLambdas()
    {
        Console.WriteLine("1. БАЗОВЫЕ ЛЯМБДА-ВЫРАЖЕНИЯ:");

        // ========== ЛЯМБДЫ С ОДНИМ ПАРАМЕТРОМ ==========
        Console.WriteLine("\n--- Лямбды с одним параметром ---");
        NumberCheck isEven = n => n % 2 == 0;
        NumberCheck isPositive = x => x > 0;

        Console.WriteLine($"Число 5 четное: {isEven(5)}");
        Console.WriteLine($"Число -3 положительное: {isPositive(-3)}");

        // ========== ЛЯМБДЫ С НЕСКОЛЬКИМИ ПАРАМЕТРАМИ ==========
        Console.WriteLine("\n--- Лямбды с несколькими параметрами ---");
        MathOperation add = (a, b) => a + b;
        MathOperation multiply = (x, y) => x * y;

        Console.WriteLine($"Сложение: 10 + 5 = {add(10, 5)}");
        Console.WriteLine($"Умножение: 4 * 7 = {multiply(4, 7)}");

        // ========== ВЫРАЖЕНЧЕСКИЕ ЛЯМБДЫ ==========
        Console.WriteLine("\n--- Выраженческие лямбды (одна строка) ---");
        MathOperation subtract = (a, b) => a - b;
        SimpleAction greet = () => Console.WriteLine("Привет из лямбды!");

        Console.WriteLine($"Вычитание: 10 - 4 = {subtract(10, 4)}");
        greet();

        // ========== БЛОЧНЫЕ ЛЯМБДЫ ==========
        Console.WriteLine("\n--- Блочные лямбды (несколько операторов) ---");
        MathOperation divide = (a, b) =>
        {
            if (b == 0)
            {
                Console.WriteLine("Ошибка: деление на ноль!");
                return 0;
            }
            return a / b;
        };

        SimpleAction complexAction = () =>
        {
            Console.WriteLine("Начало сложной операции...");
            Console.WriteLine("Выполнение...");
            Console.WriteLine("Операция завершена!");
        };

        Console.WriteLine($"Деление: 15 / 3 = {divide(15, 3)}");
        complexAction();

        // ========== MULTICAST ДЕЛЕГАТ С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Multicast делегат с лямбдами ---");
        SimpleAction multiAction = () => Console.WriteLine("Первое действие");
        multiAction += () => Console.WriteLine("Второе действие");
        multiAction += () => Console.WriteLine("Третье действие");

        Console.WriteLine("Вызов multicast делегата:");
        multiAction();

        Console.WriteLine();
    }

    static void DemonstrateBuiltInDelegates()
    {
        Console.WriteLine("2. ЛЯМБДЫ С ВСТРОЕННЫМИ ДЕЛЕГАТАМИ:");

        // ========== FUNC ДЕЛЕГАТ С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Func делегат (с возвращаемым значением) ---");
        Func<int, int, int> add = (a, b) => a + b;
        Func<int, int, double> divide = (x, y) => y != 0 ? (double)x / y : 0;
        Func<string, string, string> concat = (s1, s2) => s1 + " " + s2;

        Console.WriteLine($"Func Add: {add(15, 25)}");
        Console.WriteLine($"Func Divide: {divide(10, 3):F2}");
        Console.WriteLine($"Func Concat: {concat("Hello", "World")}");

        // ========== ACTION ДЕЛЕГАТ С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Action делегат (без возвращаемого значения) ---");
        Action<string> print = message => Console.WriteLine($"Сообщение: {message}");
        Action<int, int> printSum = (x, y) => Console.WriteLine($"Сумма: {x + y}");

        print("Тестовое сообщение");
        printSum(5, 8);

        // ========== PREDICATE ДЕЛЕГАТ С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Predicate делегат (проверка условия) ---");
        Predicate<int> isEven = n => n % 2 == 0;
        Predicate<string> isLong = s => s.Length > 5;

        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<string> words = new List<string> { "яблоко", "кот", "программирование", "дом" };

        Console.WriteLine("Четные числа (используя Predicate):");
        numbers.FindAll(isEven).ForEach(n => Console.Write($"{n} "));

        Console.WriteLine("\nДлинные слова (используя Predicate):");
        words.FindAll(isLong).ForEach(w => Console.Write($"{w} "));

        // ========== ЗАХВАТ ПЕРЕМЕННЫХ ИЗ ВНЕШНЕЙ ОБЛАСТИ ==========
        Console.WriteLine("\n\n--- Захват переменных из внешней области ---");
        int multiplier = 3;
        Func<int, int> multiplyByExternal = x => x * multiplier;

        Console.WriteLine($"Захват переменной: 5 * {multiplier} = {multiplyByExternal(5)}");

        // Демонстрация изменения захваченной переменной
        multiplier = 5;
        Console.WriteLine($"После изменения: 5 * {multiplier} = {multiplyByExternal(5)}");

        Console.WriteLine();
    }

    static void DemonstrateEventLambdas()
    {
        Console.WriteLine("3. ЛЯМБДЫ В СОБЫТИЯХ:");

        NotificationService service = new NotificationService();

        // ========== ПОДПИСКА НА СОБЫТИЯ ЧЕРЕЗ ЛЯМБДЫ ==========
        Console.WriteLine("\n--- Подписка на события через лямбды ---");

        // Подписка на Action<string> событие
        service.MessageReceived += message =>
        {
            Console.WriteLine($"[ЛЯМБДА] Получено сообщение: {message}");
        };

        // Подписка на EventHandler событие
        service.StandardMessageReceived += (sender, message) =>
        {
            Console.WriteLine($"[EVENTHANDLER] От {sender}: {message}");
        };

        // ========== МНОЖЕСТВЕННАЯ ПОДПИСКА ==========
        Console.WriteLine("\n--- Множественная подписка ---");
        service.MessageReceived += m => Console.WriteLine($"[КОПИЯ] {m}");
        service.MessageReceived += m =>
        {
            if (m.Length > 10)
                Console.WriteLine($"[ДЛИННОЕ] {m.Substring(0, 10)}...");
            else
                Console.WriteLine($"[КОРОТКОЕ] {m}");
        };

        // ========== ОТПРАВКА СООБЩЕНИЙ ==========
        Console.WriteLine("\n--- Отправка тестовых сообщений ---");
        service.SendMessage("Первое тестовое сообщение");
        service.SendMessage("Очень длинное тестовое сообщение для демонстрации");

        // ========== ДЕМОНСТРАЦИЯ ОТПИСКИ ==========
        Console.WriteLine("\n--- Демонстрация отписки ---");

        // Создаем ссылку на лямбду для возможности отписки
        Action<string> specificHandler = message =>
            Console.WriteLine($"[СПЕЦИАЛЬНЫЙ] {message}");

        service.MessageReceived += specificHandler;
        Console.WriteLine("До отписки:");
        service.SendMessage("Сообщение до отписки");

        service.MessageReceived -= specificHandler;
        Console.WriteLine("После отписки:");
        service.SendMessage("Сообщение после отписки");

        Console.WriteLine();
    }

    static void DemonstrateLinqLambdas()
    {
        Console.WriteLine("4. ЛЯМБДЫ В LINQ:");

        // ========== СОЗДАНИЕ ТЕСТОВОЙ КОЛЛЕКЦИИ ==========
        Console.WriteLine("\n--- Создание тестовой коллекции продуктов ---");
        List<Product> products = new List<Product>
        {
            new Product("Ноутбук", 1200, "Электроника", 5),
            new Product("Мышь", 25, "Электроника", 15),
            new Product("Книга", 15, "Книги", 30),
            new Product("Стул", 80, "Мебель", 8),
            new Product("Стол", 200, "Мебель", 3),
            new Product("Телефон", 800, "Электроника", 12),
            new Product("Ручка", 2, "Канцелярия", 100),
            new Product("Блокнот", 5, "Канцелярия", 50)
        };

        // ========== FILTER (WHERE) С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Фильтрация (WHERE) с лямбда-выражениями ---");
        Console.WriteLine("Дорогие товары (> $100):");
        var expensiveProducts = products.Where(p => p.Price > 100);
        foreach (var product in expensiveProducts)
        {
            Console.WriteLine($"  {product}");
        }

        // ========== PROJECTION (SELECT) С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Проекция (SELECT) с лямбда-выражениями ---");
        Console.WriteLine("Названия электронных товаров:");
        var electronicNames = products
            .Where(p => p.Category == "Электроника")
            .Select(p => p.Name);
        electronicNames.ToList().ForEach(name => Console.WriteLine($"  {name}"));

        // ========== SORTING (ORDERBY) С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Сортировка (ORDERBY) с лямбда-выражениями ---");
        Console.WriteLine("Товары отсортированные по цене:");
        var sortedByPrice = products.OrderBy(p => p.Price);
        sortedByPrice.ToList().ForEach(p => Console.WriteLine($"  {p}"));

        // ========== СЛОЖНЫЕ ФИЛЬТРЫ С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Сложные фильтры с лямбда-выражениями ---");
        Console.WriteLine("Товары с низким запасом и высокой ценой:");
        var criticalProducts = products
            .Where(p => p.Stock < 10 && p.Price > 50)
            .OrderByDescending(p => p.Price);
        criticalProducts.ToList().ForEach(p => Console.WriteLine($"  {p}"));

        // ========== GROUPING (GROUPBY) С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Группировка (GROUPBY) с лямбда-выражениями ---");
        Console.WriteLine("Товары по категориям:");
        var productsByCategory = products.GroupBy(p => p.Category);
        foreach (var group in productsByCategory)
        {
            Console.WriteLine($"  {group.Key}:");
            group.ToList().ForEach(p => Console.WriteLine($"    {p.Name} - ${p.Price}"));
        }

        // ========== АГРЕГАТНЫЕ ФУНКЦИИ С ЛЯМБДАМИ ==========
        Console.WriteLine("\n--- Агрегатные функции с лямбда-выражениями ---");
        Console.WriteLine("Статистика по товарам:");
        Console.WriteLine($"  Общее количество товаров: {products.Count}");
        Console.WriteLine($"  Средняя цена: {products.Average(p => p.Price):C}");
        Console.WriteLine($"  Максимальная цена: {products.Max(p => p.Price):C}");
        Console.WriteLine($"  Минимальная цена: {products.Min(p => p.Price):C}");
        Console.WriteLine($"  Общая стоимость запасов: {products.Sum(p => p.Price * p.Stock):C}");

        Console.WriteLine();
    }

    static void DemonstrateVariableCapture()
    {
        Console.WriteLine("5. ЗАХВАТ ПЕРЕМЕННЫХ В ЛЯМБДАХ:");

        // ========== ЗАХВАТ ЛОКАЛЬНЫХ ПЕРЕМЕННЫХ ==========
        Console.WriteLine("\n--- Захват локальных переменных ---");
        int baseNumber = 10;
        Func<int, int> addBase = x => x + baseNumber;

        Console.WriteLine($"Захват переменной: 5 + {baseNumber} = {addBase(5)}");

        // Демонстрация изменения захваченной переменной
        baseNumber = 20;
        Console.WriteLine($"После изменения: 5 + {baseNumber} = {addBase(5)}");

        // ========== ПРОБЛЕМА ЗАХВАТА В ЦИКЛАХ ==========
        Console.WriteLine("\n--- Проблема захвата в циклах ---");
        List<Action> actions = new List<Action>();

        for (int i = 0; i < 5; i++)
        {
            // ПРОБЛЕМА: все лямбды захватывают одну и ту же переменную i
            actions.Add(() => Console.WriteLine($"Неправильно: i = {i}"));
        }

        Console.WriteLine("Все лямбды видят одно значение i (последнее):");
        foreach (var action in actions)
        {
            action(); // Все выведут i = 5 (последнее значение)
        }

        // ========== РЕШЕНИЕ ПРОБЛЕМЫ ЗАХВАТА ==========
        Console.WriteLine("\n--- Правильное решение проблемы захвата ---");
        List<Action> correctActions = new List<Action>();

        for (int i = 0; i < 5; i++)
        {
            int temp = i; // Локальная копия для каждой итерации
            correctActions.Add(() => Console.WriteLine($"Правильно: i = {temp}"));
        }

        Console.WriteLine("Каждая лямбда видит свое значение:");
        foreach (var action in correctActions)
        {
            action(); // Каждая выведет свое значение
        }

        // ========== ЗАМЫКАНИЯ (CLOSURES) ==========
        Console.WriteLine("\n--- Замыкания (closures) ---");
        // Функция, которая создает и возвращает другую функцию
        Func<int, Func<int, int>> createMultiplier = factor =>
        {
            // factor захватывается возвращаемой лямбдой
            return x => x * factor;
        };

        var doubleMultiplier = createMultiplier(2);
        var tripleMultiplier = createMultiplier(3);

        Console.WriteLine($"Удвоитель: 5 * 2 = {doubleMultiplier(5)}");
        Console.WriteLine($"Утроитель: 5 * 3 = {tripleMultiplier(5)}");

        // ========== ВРЕМЯ ЖИЗНИ ЗАХВАЧЕННЫХ ПЕРЕМЕННЫХ ==========
        Console.WriteLine("\n--- Время жизни захваченных переменных ---");

        // ПРАВИЛЬНО: Func<int> возвращает int, а не другую функцию
        Func<int> simpleCounterCreator = () =>
        {
            int count = 0;
            count++; // Просто увеличиваем счетчик
            return count;
        };

        // ПРАВИЛЬНО: Если нужно возвращать функцию, используем Func<Func<int>>
        Func<Func<int>> counterCreator = () =>
        {
            int count = 0; // Локальная переменная, которая захватывается
            return () => ++count; // Возвращаем лямбду, которая запоминает count
        };

        // Вариант 1: Простой счетчик
        Console.WriteLine("Простой счетчик:");
        var simpleCounter = simpleCounterCreator;
        Console.WriteLine($"Counter: {simpleCounter()}, {simpleCounter()}, {simpleCounter()}");

        // Вариант 2: Счетчик с замыканием (правильная версия)
        Console.WriteLine("\nСчетчики с замыканием:");
        Func<int> counter1 = counterCreator(); // counterCreator() возвращает Func<int>
        Func<int> counter2 = counterCreator(); // counterCreator() возвращает Func<int>

        Console.WriteLine("Два независимых счетчика:");
        Console.WriteLine($"Counter1: {counter1()}, {counter1()}, {counter1()}");
        Console.WriteLine($"Counter2: {counter2()}, {counter2()}");
        Console.WriteLine($"Counter1 снова: {counter1()}");

        // Вариант 3: Упрощенная версия без вложенных делегатов
        Console.WriteLine("\nУпрощенная версия:");
        Func<Func<int>> createCounter = () =>
        {
            int count = 0;
            return () => ++count;
        };

        var counterA = createCounter();
        var counterB = createCounter();

        Console.WriteLine($"CounterA: {counterA()}, {counterA()}");
        Console.WriteLine($"CounterB: {counterB()}, {counterB()}, {counterB()}");
        Console.WriteLine($"CounterA снова: {counterA()}");
    }
}

//1.ЛЯМБДА - ВЫРАЖЕНИЕ
//Анонимная функция, которая может содержать выражения и операторы.

//2. СИНТАКСИС ЛЯМБД
//(параметры) => выражение

//(параметры) => { операторы }

//3.ВИДЫ ЛЯМБДА - ВЫРАЖЕНИЙ
//Выраженческие - одно выражение, возвращает результат

//Блочные - несколько операторов в { }

//4.ВСТРОЕННЫЕ ДЕЛЕГАТЫ
//Func<> - с возвращаемым значением

//Action<> - без возвращаемого значения

//Predicate<> - возвращает bool

//5. ЗАХВАТ ПЕРЕМЕННЫХ
//Лямбды могут захватывать переменные из окружающей области видимости.

//6. ЗАМЫКАНИЯ (CLOSURES)
//Функция, которая запоминает окружение, в котором была создана.

//7. ПРОБЛЕМА ЗАХВАТА В ЦИКЛАХ
//Переменная захватывается по ссылке, а не по значению.

//8. ОБРАБОТЧИКИ СОБЫТИЙ
//Лямбды могут использоваться как компактные обработчики событий.

//9. LINQ С ЛЯМБДАМИ
//Лямбда-выражения - основа синтаксиса методов расширения LINQ.

//Лямбда-выражения делают код более компактным и выразительным!