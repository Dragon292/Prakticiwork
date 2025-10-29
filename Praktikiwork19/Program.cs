//Интерфейс в объектно-ориентированном программировании (ООП) — это набор методов, которые описывают, как объект может взаимодействовать с другими объектами.
//Интерфейс определяет, какие действия можно выполнить с объектом и какие данные можно получить от него. 
//Особенности:
//Интерфейс не содержит реализации самой логики методов — определяет только сигнатуры методов (их названия, возвращаемые значения и параметры).
//Реализация методов остаётся на усмотрение классов, реализующих интерфейс.
//Интерфейс не является полноценным типом данных, так как задаёт только внешнее поведение объектов.
// 1. Простейший интерфейс - IAnimal
public interface IAnimal
{
    void Eat();
    void MakeSound();
    string Name { get; set; }
}

// Класс Dog, реализующий IAnimal
public class Dog : IAnimal
{
    public string Name { get; set; }
    public string Breed { get; set; }

    public Dog(string name, string breed)
    {
        Name = name;
        Breed = breed;
    }

    public void Eat()
    {
        Console.WriteLine($"{Name} ест корм для собак");
    }

    public void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Гав-гав!");
    }
}

// Класс Cat, реализующий IAnimal
public class Cat : IAnimal
{
    public string Name { get; set; }
    public string Color { get; set; }

    public Cat(string name, string color)
    {
        Name = name;
        Color = color;
    }

    public void Eat()
    {
        Console.WriteLine($"{Name} ест рыбу");
    }

    public void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Мяу-мяу!");
    }
}

// 2. Интерфейс «Устройство» - IDevice
public interface IDevice
{
    void TurnOn();
    void TurnOff();
    string Brand { get; set; }
}

// Класс Компьютер
public class Computer : IDevice
{
    public string Brand { get; set; }

    public Computer(string brand)
    {
        Brand = brand;
    }

    public void TurnOn()
    {
        Console.WriteLine($"{Brand} компьютер включается");
    }

    public void TurnOff()
    {
        Console.WriteLine($"{Brand} компьютер выключается");
    }
}

// Класс Телевизор
public class TV : IDevice
{
    public string Brand { get; set; }

    public TV(string brand)
    {
        Brand = brand;
    }

    public void TurnOn()
    {
        Console.WriteLine($"{Brand} телевизор включается");
    }

    public void TurnOff()
    {
        Console.WriteLine($"{Brand} телевизор выключается");
    }
}

// 3. Интерфейс с свойствами - IMovable
public interface IMovable
{
    double Speed { get; set; }
    void Move();
    void Stop();
}

// Класс Автомобиль
public class Car : IMovable
{
    public string Model { get; set; }
    public double Speed { get; set; }

    public Car(string model)
    {
        Model = model;
        Speed = 0;
    }

    public void Move()
    {
        Speed = 60;
        Console.WriteLine($"{Model} едет со скоростью {Speed} км/ч");
    }

    public void Stop()
    {
        Speed = 0;
        Console.WriteLine($"{Model} остановился");
    }
}

// Класс Велосипед
public class Bicycle : IMovable
{
    public string Type { get; set; }
    public double Speed { get; set; }

    public Bicycle(string type)
    {
        Type = type;
        Speed = 0;
    }

    public void Move()
    {
        Speed = 15;
        Console.WriteLine($"{Type} велосипед едет со скоростью {Speed} км/ч");
    }

    public void Stop()
    {
        Speed = 0;
        Console.WriteLine($"{Type} велосипед остановился");
    }
}

// 4. Множественная реализация интерфейсов
public interface IReadable
{
    string ReadData();
}

public interface IWritable
{
    void WriteData(string data);
}

// Класс FileManager, реализующий оба интерфейса
public class FileManager : IReadable, IWritable
{
    private string _content = "";

    public string ReadData()
    {
        return $"Чтение данных: {_content}";
    }

    public void WriteData(string data)
    {
        _content = data;
        Console.WriteLine($"Запись данных: {data}");
    }
}

// 5. Интерфейс как параметр метода - ILogger
public interface ILogger
{
    void Log(string message);
}

// Консольный логгер
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[КОНСОЛЬ] {message}");
    }
}

// Файловый логгер
public class FileLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[ФАЙЛ] {message}");
    }
}

// Класс для работы с логгерами
public class MessageProcessor
{
    // Метод, принимающий интерфейс как параметр
    public void ProcessMessage(string message, ILogger logger)
    {
        logger.Log($"Обработка сообщения: {message}");
    }
}

// 6. Интерфейс для сортировки - IComparable
public class Student : IComparable<Student>
{
    public string Name { get; set; }
    public double AverageGrade { get; set; }

    public Student(string name, double averageGrade)
    {
        Name = name;
        AverageGrade = averageGrade;
    }

    // Реализация интерфейса IComparable
    public int CompareTo(Student other)
    {
        // Сравнение по среднему баллу (по убыванию)
        return other.AverageGrade.CompareTo(this.AverageGrade);
    }

    public override string ToString()
    {
        return $"{Name} - {AverageGrade:F1}";
    }
}

// 7. Система «Умный дом»
public interface ISwitchable
{
    void On();
    void Off();
}

public interface IAdjustable
{
    void SetLevel(int level);
}

// Класс Свет
public class Light : ISwitchable, IAdjustable
{
    private bool _isOn = false;
    private int _brightness = 100;

    public void On()
    {
        _isOn = true;
        Console.WriteLine("Свет включен");
    }

    public void Off()
    {
        _isOn = false;
        Console.WriteLine("Свет выключен");
    }

    public void SetLevel(int level)
    {
        _brightness = level;
        Console.WriteLine($"Яркость установлена: {level}%");
    }
}

// Класс Обогреватель
public class Heater : ISwitchable
{
    private bool _isOn = false;

    public void On()
    {
        _isOn = true;
        Console.WriteLine("Обогреватель включен");
    }

    public void Off()
    {
        _isOn = false;
        Console.WriteLine("Обогреватель выключен");
    }
}

// Класс Кондиционер
public class AirConditioner : ISwitchable, IAdjustable
{
    private bool _isOn = false;
    private int _temperature = 22;

    public void On()
    {
        _isOn = true;
        Console.WriteLine("Кондиционер включен");
    }

    public void Off()
    {
        _isOn = false;
        Console.WriteLine("Кондиционер выключен");
    }

    public void SetLevel(int level)
    {
        _temperature = level;
        Console.WriteLine($"Температура установлена: {level}°C");
    }
}

// Класс для управления умным домом
public class SmartHome
{
    private List<ISwitchable> _devices = new List<ISwitchable>();

    public void AddDevice(ISwitchable device)
    {
        _devices.Add(device);
    }

    public void TurnOnAll()
    {
        Console.WriteLine("Включаем все устройства:");
        foreach (var device in _devices)
        {
            device.On();
        }
    }

    public void TurnOffAll()
    {
        Console.WriteLine("Выключаем все устройства:");
        foreach (var device in _devices)
        {
            device.Off();
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== 1. Простейший интерфейс IAnimal ===");
        IAnimal dog = new Dog("Бобик", "Овчарка");
        IAnimal cat = new Cat("Мурка", "Рыжая");
        dog.Eat();
        dog.MakeSound();
        cat.Eat();
        cat.MakeSound();

        Console.WriteLine("\n=== 2. Интерфейс IDevice ===");
        IDevice computer = new Computer("Dell");
        IDevice tv = new TV("Samsung");
        computer.TurnOn();
        computer.TurnOff();
        tv.TurnOn();
        tv.TurnOff();

        Console.WriteLine("\n=== 3. Интерфейс IMovable с свойствами ===");
        IMovable car = new Car("Toyota");
        IMovable bicycle = new Bicycle("Горный");
        car.Move();
        car.Stop();
        bicycle.Move();
        bicycle.Stop();

        Console.WriteLine("\n=== 4. Множественная реализация интерфейсов ===");
        FileManager fileManager = new FileManager();
        fileManager.WriteData("Привет, мир!");
        Console.WriteLine(fileManager.ReadData());

        Console.WriteLine("\n=== 5. Интерфейс как параметр метода ===");
        MessageProcessor processor = new MessageProcessor();
        ILogger consoleLogger = new ConsoleLogger();
        ILogger fileLogger = new FileLogger();
        processor.ProcessMessage("Тестовое сообщение", consoleLogger);
        processor.ProcessMessage("Другое сообщение", fileLogger);

        Console.WriteLine("\n=== 6. Интерфейс для сортировки ===");
        List<Student> students = new List<Student>
        {
            new Student("Анна", 4.5),
            new Student("Борис", 3.8),
            new Student("Виктор", 4.2)
        };

        Console.WriteLine("До сортировки:");
        foreach (var student in students)
            Console.WriteLine(student);

        students.Sort();

        Console.WriteLine("После сортировки по оценкам:");
        foreach (var student in students)
            Console.WriteLine(student);

        Console.WriteLine("\n=== 7. Система «Умный дом» ===");
        SmartHome smartHome = new SmartHome();
        smartHome.AddDevice(new Light());
        smartHome.AddDevice(new Heater());
        smartHome.AddDevice(new AirConditioner());

        smartHome.TurnOnAll();
        Console.WriteLine();
        smartHome.TurnOffAll();

        // Демонстрация работы с регулируемыми устройствами
        Console.WriteLine("\nРабота с регулируемыми устройствами:");
        Light light = new Light();
        light.On();
        light.SetLevel(75);

        AirConditioner ac = new AirConditioner();
        ac.On();
        ac.SetLevel(20);
    }
}