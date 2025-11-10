//1.ДЕЛЕГАТ(Delegate)
//Типобезопасный указатель на метод

//Определяет сигнатуру методов, которые может вызывать

//Поддержка multicast (цепочка вызовов)

//2. EVENT (Событие)
//Механизм оповещения подписчиков

//Инкапсуляция делегата

//Безопасная модель publish-subscribe

//3. ACTION<>
//Встроенный делегат для методов без возвращаемого значения

//Может принимать до 16 параметров

//4. FUNC<>
//Встроенный делегат для методов с возвращаемым значением

//Последний generic-параметр - тип возврата

//5. PREDICATE<>
//Специальный делегат для методов проверки условия

//Всегда возвращает bool

//6. EVENTARGS
//Базовый класс для передачи данных в событиях

//Кастомные EventArgs для специфичных данных

//7. MULTICAST DELEGATE
//Делегат, содержащий несколько методов

//Вызов происходит последовательно

//Возвращает результат последнего метода

//8. ПОДПИСКА НА СОБЫТИЯ
//+= оператор для добавления обработчика

//-= оператор для удаления обработчика

//Проверка на null перед вызовом

//9. ЛЯМБДА-ВЫРАЖЕНИЯ
//Анонимные методы для быстрого создания обработчиков

//Упрощенный синтаксис



// Задание 1: Базовые делегаты

// Объявление делегата для математических операций
public delegate double MathOperation(double a, double b);

// Задание 5: Кастомные EventArgs для системы уведомлений
public class NotificationEventArgs : EventArgs
{
    public string Message { get; }
    public string Priority { get; }
    public DateTime Timestamp { get; }

    public NotificationEventArgs(string message, string priority)
    {
        Message = message;
        Priority = priority;
        Timestamp = DateTime.Now;
    }
}

// Задание 3: Класс Timer с событиями
public class Timer
{
    // Объявление событий
    public event Action onStart;
    public event Action<int> onTick;  // int - текущий тик
    public event Action onFinish;

    private int _duration;
    private int _currentTick;

    public Timer(int duration)
    {
        _duration = duration;
        _currentTick = 0;
    }

    public void Start()
    {
        // Вызов события onStart
        onStart?.Invoke();

        for (_currentTick = 1; _currentTick <= _duration; _currentTick++)
        {
            // Вызов события onTick
            onTick?.Invoke(_currentTick);
            Thread.Sleep(1000); // Пауза 1 секунда
        }

        // Вызов события onFinish
        onFinish?.Invoke();
    }
}

// Задание 4: Класс Button с событиями UI
public class Button
{
    public string Name { get; }

    // Объявление событий
    public event Action<Button> Click;
    public event Action<Button> MouseOver;
    public event Action<Button> MouseLeave;

    public Button(string name)
    {
        Name = name;
    }

    // Методы для имитации взаимодействия
    public void SimulateClick()
    {
        Console.WriteLine($"Кнопка '{Name}' нажата");
        Click?.Invoke(this);
    }

    public void SimulateMouseOver()
    {
        Console.WriteLine($"Курсор наведен на кнопку '{Name}'");
        MouseOver?.Invoke(this);
    }

    public void SimulateMouseLeave()
    {
        Console.WriteLine($"Курсор ушел с кнопки '{Name}'");
        MouseLeave?.Invoke(this);
    }
}

// Задание 5: Служба уведомлений
public class NotificationService
{
    // Событие с кастомными EventArgs
    public event EventHandler<NotificationEventArgs> NotificationSent;

    public void SendNotification(string message, string priority)
    {
        var args = new NotificationEventArgs(message, priority);

        // Вызов события
        NotificationSent?.Invoke(this, args);
    }
}

// Подписчики для системы уведомлений
public class EmailNotifier
{
    public void OnNotificationReceived(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"[EMAIL] Приоритет: {e.Priority}, Время: {e.Timestamp:HH:mm:ss}");
        Console.WriteLine($"        Сообщение: {e.Message}");
    }
}

public class SMSNotifier
{
    public void OnNotificationReceived(object sender, NotificationEventArgs e)
    {
        if (e.Priority == "High")
        {
            Console.WriteLine($"[SMS] СРОЧНО! {e.Message}");
        }
    }
}

public class Logger
{
    public void OnNotificationReceived(object sender, NotificationEventArgs e)
    {
        Console.WriteLine($"[LOG] {e.Timestamp:yyyy-MM-dd HH:mm:ss} | {e.Priority} | {e.Message}");
    }
}

class Program
{
    // Методы для делегатов (Задание 1)
    static double Add(double a, double b)
    {
        double result = a + b;
        Console.WriteLine($"Add: {a} + {b} = {result}");
        return result;
    }

    static double Subtract(double a, double b)
    {
        double result = a - b;
        Console.WriteLine($"Subtract: {a} - {b} = {result}");
        return result;
    }

    static double Multiply(double a, double b)
    {
        double result = a * b;
        Console.WriteLine($"Multiply: {a} * {b} = {result}");
        return result;
    }

    static double Divide(double a, double b)
    {
        if (b == 0)
        {
            Console.WriteLine("Divide: Деление на ноль!");
            return double.NaN;
        }
        double result = a / b;
        Console.WriteLine($"Divide: {a} / {b} = {result}");
        return result;
    }

    // Методы для Action (Задание 2)
    static void DisplayResult(string message)
    {
        Console.WriteLine($"Результат: {message}");
    }

    static void DisplayError(string message)
    {
        Console.WriteLine($"ОШИБКА: {message}");
    }

    // Predicate для фильтрации (Задание 2)
    static bool IsEven(int number)
    {
        return number % 2 == 0;
    }

    static bool IsPositive(int number)
    {
        return number > 0;
    }

    // Обработчики событий для кнопок (Задание 4)
    static void HandleButtonClick(Button button)
    {
        Console.WriteLine($"   → Обработчик 1: Кнопка '{button.Name}' была кликнута!");
    }

    static void HandleButtonClick2(Button button)
    {
        Console.WriteLine($"   → Обработчик 2: Нажата кнопка '{button.Name}'");
    }

    static void HandleMouseOver(Button button)
    {
        Console.WriteLine($"   → Курсор над кнопкой '{button.Name}'");
    }

    static void HandleMouseLeave(Button button)
    {
        Console.WriteLine($"   → Курсор покинул кнопку '{button.Name}'");
    }

    // Обработчики событий таймера (Задание 3)
    static void Timer_OnStart()
    {
        Console.WriteLine("Таймер запущен!");
    }

    static void Timer_OnTick(int tick)
    {
        Console.WriteLine($"Тик: {tick}");
    }

    static void Timer_OnFinish()
    {
        Console.WriteLine("Таймер завершил работу!");
    }

    static void Timer_OnTickExtra(int tick)
    {
        if (tick % 2 == 0)
        {
            Console.WriteLine($"   (четный тик: {tick})");
        }
    }

    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ДЕЛЕГАТОВ И СОБЫТИЙ ===\n");

        // Задание 1: Базовые делегаты
        DemonstrateBasicDelegates();

        // Задание 2: Func и Action
        DemonstrateFuncAndAction();

        // Задание 3: События в классе
        DemonstrateTimerEvents();

        // Задание 4: UI Events Simulator
        DemonstrateUIEvents();

        // Задание 5: Event Args и кастомные события
        DemonstrateCustomEvents();

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
    }

    static void DemonstrateBasicDelegates()
    {
        Console.WriteLine("1. БАЗОВЫЕ ДЕЛЕГАТЫ:");

        // Создание экземпляра делегата
        MathOperation operation = Add;
        double result = operation(10, 5);

        operation = Subtract;
        result = operation(10, 5);

        operation = Multiply;
        result = operation(10, 5);

        operation = Divide;
        result = operation(10, 5);

        // Multicast делегат
        Console.WriteLine("\nMulticast делегат:");
        MathOperation multiOperation = Add;
        multiOperation += Subtract;
        multiOperation += Multiply;
        multiOperation += Divide;

        // Вызов всех методов в цепочке
        result = multiOperation(20, 4);
        Console.WriteLine($"Финальный результат: {result}");

        Console.WriteLine();
    }

    static void DemonstrateFuncAndAction()
    {
        Console.WriteLine("2. Func И Action:");

        // Использование Func<>
        Func<int, int, int> funcOperation = (a, b) => a + b;
        int result = funcOperation(15, 25);
        Console.WriteLine($"Func результат: {result}");

        // Несколько Func операций
        Func<int, int, int>[] operations = {
            (a, b) => a + b,
            (a, b) => a - b,
            (a, b) => a * b,
            (a, b) => b != 0 ? a / b : 0
        };

        foreach (var op in operations)
        {
            result = op(100, 50);
            Console.WriteLine($"Операция результат: {result}");
        }

        // Использование Action<>
        Action<string> actionDisplay = DisplayResult;
        actionDisplay("Привет, мир!");

        actionDisplay += DisplayError;
        actionDisplay("Что-то пошло не так");

        // Использование Predicate<>
        Predicate<int> predicate = IsEven;
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Console.WriteLine("Четные числа:");
        foreach (var number in numbers.FindAll(IsEven))
        {
            Console.Write($"{number} ");
        }

        Console.WriteLine("\nПоложительные числа:");
        foreach (var number in numbers.FindAll(IsPositive))
        {
            Console.Write($"{number} ");
        }

        Console.WriteLine("\n");
    }

    static void DemonstrateTimerEvents()
    {
        Console.WriteLine("3. СОБЫТИЯ ТАЙМЕРА:");

        Timer timer = new Timer(5);

        // Подписка на события
        timer.onStart += Timer_OnStart;
        timer.onTick += Timer_OnTick;
        timer.onTick += Timer_OnTickExtra; // Множественная подписка
        timer.onFinish += Timer_OnFinish;

        Console.WriteLine("Запуск таймера на 5 секунд...");
        timer.Start();

        // Демонстрация отписки
        Console.WriteLine("\nДемонстрация отписки:");
        timer.onTick -= Timer_OnTickExtra;

        Timer shortTimer = new Timer(3);
        shortTimer.onStart += () => Console.WriteLine("Короткий таймер стартовал!");
        shortTimer.onTick += (tick) => Console.WriteLine($"Быстрый тик: {tick}");
        shortTimer.onFinish += () => Console.WriteLine("Короткий таймер завершен!");

        shortTimer.Start();

        Console.WriteLine();
    }

    static void DemonstrateUIEvents()
    {
        Console.WriteLine("4. UI EVENTS SIMULATOR:");

        // Создание кнопок
        Button okButton = new Button("OK");
        Button cancelButton = new Button("Cancel");
        Button submitButton = new Button("Submit");

        // Подписка на события кнопки OK
        okButton.Click += HandleButtonClick;
        okButton.Click += HandleButtonClick2; // Множественная подписка
        okButton.MouseOver += HandleMouseOver;
        okButton.MouseLeave += HandleMouseLeave;

        // Подписка на события кнопки Cancel
        cancelButton.Click += (btn) => Console.WriteLine($"   → Лямбда-обработчик: {btn.Name} отменен");
        cancelButton.MouseOver += (btn) => Console.WriteLine($"   → Курсор на {btn.Name}");

        // Подписка на события кнопки Submit
        submitButton.Click += (btn) =>
        {
            Console.WriteLine($"   → Отправка формы кнопкой {btn.Name}");
            Console.WriteLine($"   → Данные валидированы и отправлены");
        };

        // Имитация взаимодействия с кнопками
        Console.WriteLine("Имитация наведения на OK:");
        okButton.SimulateMouseOver();

        Console.WriteLine("\nИмитация клика на OK:");
        okButton.SimulateClick();

        Console.WriteLine("\nИмитация ухода курсора с OK:");
        okButton.SimulateMouseLeave();

        Console.WriteLine("\nИмитация клика на Cancel:");
        cancelButton.SimulateClick();

        Console.WriteLine("\nИмитация клика на Submit:");
        submitButton.SimulateClick();

        // Демонстрация отписки
        Console.WriteLine("\nДемонстрация отписки от OK click:");
        okButton.Click -= HandleButtonClick2;
        okButton.SimulateClick();

        Console.WriteLine();
    }

    static void DemonstrateCustomEvents()
    {
        Console.WriteLine("5. КАСТОМНЫЕ СОБЫТИЯ С EventArgs:");

        NotificationService notificationService = new NotificationService();

        // Создание подписчиков
        EmailNotifier emailNotifier = new EmailNotifier();
        SMSNotifier smsNotifier = new SMSNotifier();
        Logger logger = new Logger();

        // Подписка на событие
        notificationService.NotificationSent += emailNotifier.OnNotificationReceived;
        notificationService.NotificationSent += smsNotifier.OnNotificationReceived;
        notificationService.NotificationSent += logger.OnNotificationReceived;

        // Отправка уведомлений
        Console.WriteLine("Отправка уведомлений:");
        notificationService.SendNotification("Система запущена", "Low");
        Console.WriteLine();
        notificationService.SendNotification("Обнаружена критическая ошибка", "High");
        Console.WriteLine();
        notificationService.SendNotification("Резервное копирование завершено", "Medium");

        // Демонстрация отписки
        Console.WriteLine("\nПосле отписки SMS нотификатора:");
        notificationService.NotificationSent -= smsNotifier.OnNotificationReceived;
        notificationService.SendNotification("Тестовое сообщение", "High");

        Console.WriteLine();
    }
}