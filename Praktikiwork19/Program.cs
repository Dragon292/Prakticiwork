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
        Console.WriteLine($"{Name} the {Breed} is eating dog food");
    }

    public void MakeSound()
    {
        Console.WriteLine($"{Name} says: Woof! Woof!");
    }

    public void Fetch()
    {
        Console.WriteLine($"{Name} is fetching the ball!");
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
        Console.WriteLine($"{Name} the {Color} cat is eating fish");
    }

    public void MakeSound()
    {
        Console.WriteLine($"{Name} says: Meow! Meow!");
    }

    public void ClimbTree()
    {
        Console.WriteLine($"{Name} is climbing a tree");
    }
}

public interface IDevice
{
    void TurnOn();
    void TurnOff();
    string Brand { get; set; }
    string Model { get; set; }
    bool IsOn { get; }
}

// Класс Компьютер
public class Computer : IDevice
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsOn { get; private set; }
    public string OS { get; set; }

    public Computer(string brand, string model, string os)
    {
        Brand = brand;
        Model = model;
        OS = os;
        IsOn = false;
    }

    public void TurnOn()
    {
        if (!IsOn)
        {
            IsOn = true;
            Console.WriteLine($"{Brand} {Model} is booting up {OS}...");
            Console.WriteLine("Computer is ready to use");
        }
        else
        {
            Console.WriteLine("Computer is already on");
        }
    }

    public void TurnOff()
    {
        if (IsOn)
        {
            IsOn = false;
            Console.WriteLine($"{Brand} {Model} is shutting down...");
            Console.WriteLine("Computer is off");
        }
        else
        {
            Console.WriteLine("Computer is already off");
        }
    }

    public void RunProgram(string programName)
    {
        if (IsOn)
        {
            Console.WriteLine($"Running {programName} on {Brand} {Model}");
        }
        else
        {
            Console.WriteLine("Cannot run program - computer is off");
        }
    }
}

// Класс Телевизор
public class TV : IDevice
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsOn { get; private set; }
    public int Channel { get; private set; }
    public int Volume { get; private set; }

    public TV(string brand, string model)
    {
        Brand = brand;
        Model = model;
        IsOn = false;
        Channel = 1;
        Volume = 20;
    }

    public void TurnOn()
    {
        if (!IsOn)
        {
            IsOn = true;
            Console.WriteLine($"{Brand} {Model} is turning on...");
            Console.WriteLine($"Channel: {Channel}, Volume: {Volume}");
        }
        else
        {
            Console.WriteLine("TV is already on");
        }
    }

    public void TurnOff()
    {
        if (IsOn)
        {
            IsOn = false;
            Console.WriteLine($"{Brand} {Model} is turning off...");
        }
        else
        {
            Console.WriteLine("TV is already off");
        }
    }

    public void ChangeChannel(int channel)
    {
        if (IsOn)
        {
            Channel = channel;
            Console.WriteLine($"Changed to channel {Channel}");
        }
        else
        {
            Console.WriteLine("Cannot change channel - TV is off");
        }
    }

    public void SetVolume(int volume)
    {
        if (IsOn)
        {
            Volume = Math.Max(0, Math.Min(100, volume));
            Console.WriteLine($"Volume set to {Volume}");
        }
        else
        {
            Console.WriteLine("Cannot set volume - TV is off");
        }
    }
}

public interface IMovable
{
    double Speed { get; set; }
    string MovementType { get; }
    void Move();
    void Stop();
}

// Класс Автомобиль
public class Car : IMovable
{
    public string Model { get; set; }
    public double Speed { get; set; }
    public string MovementType => "driving";

    public Car(string model)
    {
        Model = model;
        Speed = 0;
    }

    public void Move()
    {
        if (Speed == 0)
        {
            Speed = 60; // начальная скорость
        }
        Console.WriteLine($"{Model} is {MovementType} at {Speed} km/h");
    }

    public void Stop()
    {
        Speed = 0;
        Console.WriteLine($"{Model} has stopped");
    }

    public void Accelerate(double acceleration)
    {
        Speed += acceleration;
        Console.WriteLine($"{Model} accelerated to {Speed} km/h");
    }
}

// Класс Велосипед
public class Bicycle : IMovable
{
    public string Type { get; set; }
    public double Speed { get; set; }
    public string MovementType => "pedaling";

    public Bicycle(string type)
    {
        Type = type;
        Speed = 0;
    }

    public void Move()
    {
        if (Speed == 0)
        {
            Speed = 15; // начальная скорость
        }
        Console.WriteLine($"{Type} bicycle is {MovementType} at {Speed} km/h");
    }

    public void Stop()
    {
        Speed = 0;
        Console.WriteLine($"{Type} bicycle has stopped");
    }

    public void RingBell()
    {
        Console.WriteLine("Ring! Ring!");
    }
}

public interface IReadable
{
    string ReadData();
    bool CanRead { get; }
}

// Интерфейс для записи
public interface IWritable
{
    void WriteData(string data);
    bool CanWrite { get; }
}

// Класс FileManager, реализующий оба интерфейса
public class FileManager : IReadable, IWritable
{
    private string _filePath;
    private string _content;

    public bool CanRead => true;
    public bool CanWrite => true;

    public FileManager(string filePath)
    {
        _filePath = filePath;
        _content = "";
    }

    public string ReadData()
    {
        if (string.IsNullOrEmpty(_content))
        {
            return $"No data in file {_filePath}";
        }
        return $"Reading from {_filePath}: {_content}";
    }

    public void WriteData(string data)
    {
        _content = data;
        Console.WriteLine($"Written to {_filePath}: {data}");
    }

    public void DisplayFileInfo()
    {
        Console.WriteLine($"File: {_filePath}");
        Console.WriteLine($"Can read: {CanRead}, Can write: {CanWrite}");
        Console.WriteLine($"Content: {_content}");
    }
}

// Класс ReadOnlyFile, реализующий только IReadable
public class ReadOnlyFile : IReadable
{
    private string _filePath;
    private string _content;

    public bool CanRead => true;

    public ReadOnlyFile(string filePath, string content)
    {
        _filePath = filePath;
        _content = content;
    }

    public string ReadData()
    {
        return $"Reading from read-only file {_filePath}: {_content}";
    }
}

public interface ILogger
{
    void Log(string message);
    void LogError(string error);
    void LogWarning(string warning);
    string LoggerName { get; }
}

// Консольный логгер
public class ConsoleLogger : ILogger
{
    public string LoggerName => "Console Logger";

    public void Log(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss}: {message}");
    }

    public void LogError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss}: {error}");
        Console.ResetColor();
    }

    public void LogWarning(string warning)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING] {DateTime.Now:HH:mm:ss}: {warning}");
        Console.ResetColor();
    }
}

// Файловый логгер
public class FileLogger : ILogger
{
    public string LoggerName => "File Logger";
    private string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        string logEntry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
        WriteToFile(logEntry);
    }

    public void LogError(string error)
    {
        string logEntry = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {error}";
        WriteToFile(logEntry);
    }

    public void LogWarning(string warning)
    {
        string logEntry = $"[WARNING] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {warning}";
        WriteToFile(logEntry);
    }

    private void WriteToFile(string message)
    {
        // В реальном приложении здесь была бы запись в файл
        Console.WriteLine($"(File: {_filePath}) {message}");
    }
}

// Класс для работы с логгерами
public class Application
{
    private ILogger _logger;

    public Application(ILogger logger)
    {
        _logger = logger;
    }

    public void SetLogger(ILogger logger)
    {
        _logger = logger;
    }

    // Метод, принимающий интерфейс как параметр
    public void ProcessData(string data, ILogger processorLogger = null)
    {
        ILogger logger = processorLogger ?? _logger;

        logger.Log($"Starting to process data: {data}");

        // Имитация обработки
        if (data.Length > 10)
        {
            logger.LogWarning("Data length is large, processing might take time");
        }

        try
        {
            // Имитация работы
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data cannot be empty");
            }

            logger.Log($"Data processed successfully: {data.ToUpper()}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error processing data: {ex.Message}");
        }
    }

    public static void UseLogger(ILogger logger, string message)
    {
        Console.WriteLine($"\nUsing {logger.LoggerName}:");
        logger.Log(message);
        logger.LogWarning("This is a warning");
        logger.LogError("This is an error");
    }
}

public class Student : IComparable<Student>
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double AverageGrade { get; set; }
    public string Group { get; set; }

    public Student(string name, int age, double averageGrade, string group)
    {
        Name = name;
        Age = age;
        AverageGrade = averageGrade;
        Group = group;
    }

    // Реализация интерфейса IComparable
    public int CompareTo(Student other)
    {
        if (other == null) return 1;

        // Сравнение по среднему баллу (по убыванию)
        return other.AverageGrade.CompareTo(this.AverageGrade);
    }

    public override string ToString()
    {
        return $"{Name} (Age: {Age}, Group: {Group}, Grade: {AverageGrade:F2})";
    }
}

// Дополнительный компаратор для сортировки по имени
public class StudentNameComparer : IComparer<Student>
{
    public int Compare(Student x, Student y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        return string.Compare(x.Name, y.Name);
    }
}

// Дополнительный компаратор для сортировки по возрасту
public class StudentAgeComparer : IComparer<Student>
{
    public int Compare(Student x, Student y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        return x.Age.CompareTo(y.Age);
    }
}

public interface ISwitchable
{
    void On();
    void Off();
    bool IsOn { get; }
    string DeviceName { get; }
}

// Интерфейс для регулировки уровня
public interface IAdjustable
{
    void SetLevel(int value);
    int CurrentLevel { get; }
    int MinLevel { get; }
    int MaxLevel { get; }
}

// Интерфейс для устройств с температурой
public interface ITemperatureControllable
{
    void SetTemperature(int temperature);
    int CurrentTemperature { get; }
    int TargetTemperature { get; }
}

// Класс Свет
public class Light : ISwitchable, IAdjustable
{
    public string DeviceName { get; private set; }
    public string Location { get; private set; }
    public bool IsOn { get; private set; }
    public int CurrentLevel { get; private set; }
    public int MinLevel => 0;
    public int MaxLevel => 100;

    public Light(string location)
    {
        DeviceName = "Light";
        Location = location;
        IsOn = false;
        CurrentLevel = 100; // 100% по умолчанию
    }

    public void On()
    {
        if (!IsOn)
        {
            IsOn = true;
            Console.WriteLine($"{Location} light turned ON at {CurrentLevel}% brightness");
        }
        else
        {
            Console.WriteLine($"{Location} light is already ON");
        }
    }

    public void Off()
    {
        if (IsOn)
        {
            IsOn = false;
            Console.WriteLine($"{Location} light turned OFF");
        }
        else
        {
            Console.WriteLine($"{Location} light is already OFF");
        }
    }

    public void SetLevel(int value)
    {
        if (value < MinLevel || value > MaxLevel)
        {
            Console.WriteLine($"Brightness level must be between {MinLevel} and {MaxLevel}");
            return;
        }

        CurrentLevel = value;
        if (IsOn)
        {
            Console.WriteLine($"{Location} light brightness set to {CurrentLevel}%");
        }
    }

    public void Toggle()
    {
        if (IsOn) Off();
        else On();
    }
}

// Класс Обогреватель
public class Heater : ISwitchable, ITemperatureControllable
{
    public string DeviceName { get; private set; }
    public string Location { get; private set; }
    public bool IsOn { get; private set; }
    public int CurrentTemperature { get; private set; }
    public int TargetTemperature { get; private set; }

    public Heater(string location)
    {
        DeviceName = "Heater";
        Location = location;
        IsOn = false;
        CurrentTemperature = 20; // комнатная температура
        TargetTemperature = 22;
    }

    public void On()
    {
        if (!IsOn)
        {
            IsOn = true;
            Console.WriteLine($"{Location} heater turned ON, target: {TargetTemperature}°C");
            SimulateHeating();
        }
        else
        {
            Console.WriteLine($"{Location} heater is already ON");
        }
    }

    public void Off()
    {
        if (IsOn)
        {
            IsOn = false;
            Console.WriteLine($"{Location} heater turned OFF");
        }
        else
        {
            Console.WriteLine($"{Location} heater is already OFF");
        }
    }

    public void SetTemperature(int temperature)
    {
        if (temperature < 15 || temperature > 30)
        {
            Console.WriteLine("Temperature must be between 15°C and 30°C");
            return;
        }

        TargetTemperature = temperature;
        Console.WriteLine($"{Location} heater target temperature set to {TargetTemperature}°C");

        if (IsOn)
        {
            SimulateHeating();
        }
    }

    private void SimulateHeating()
    {
        if (CurrentTemperature < TargetTemperature)
        {
            CurrentTemperature = TargetTemperature;
            Console.WriteLine($"{Location} heater reached target temperature: {CurrentTemperature}°C");
        }
    }
}

// Класс Кондиционер
public class AirConditioner : ISwitchable, ITemperatureControllable, IAdjustable
{
    public string DeviceName { get; private set; }
    public string Location { get; private set; }
    public bool IsOn { get; private set; }
    public int CurrentTemperature { get; private set; }
    public int TargetTemperature { get; private set; }
    public int CurrentLevel { get; private set; } // скорость вентилятора
    public int MinLevel => 1;
    public int MaxLevel => 3;

    public AirConditioner(string location)
    {
        DeviceName = "Air Conditioner";
        Location = location;
        IsOn = false;
        CurrentTemperature = 25;
        TargetTemperature = 22;
        CurrentLevel = 2; // средняя скорость
    }

    public void On()
    {
        if (!IsOn)
        {
            IsOn = true;
            Console.WriteLine($"{Location} AC turned ON, target: {TargetTemperature}°C, fan speed: {CurrentLevel}");
            SimulateCooling();
        }
        else
        {
            Console.WriteLine($"{Location} AC is already ON");
        }
    }

    public void Off()
    {
        if (IsOn)
        {
            IsOn = false;
            Console.WriteLine($"{Location} AC turned OFF");
        }
        else
        {
            Console.WriteLine($"{Location} AC is already OFF");
        }
    }

    public void SetTemperature(int temperature)
    {
        if (temperature < 16 || temperature > 30)
        {
            Console.WriteLine("Temperature must be between 16°C and 30°C");
            return;
        }

        TargetTemperature = temperature;
        Console.WriteLine($"{Location} AC target temperature set to {TargetTemperature}°C");

        if (IsOn)
        {
            SimulateCooling();
        }
    }

    public void SetLevel(int value)
    {
        if (value < MinLevel || value > MaxLevel)
        {
            Console.WriteLine($"Fan speed must be between {MinLevel} and {MaxLevel}");
            return;
        }

        CurrentLevel = value;
        string speed = CurrentLevel == 1 ? "Low" : CurrentLevel == 2 ? "Medium" : "High";
        Console.WriteLine($"{Location} AC fan speed set to {speed} ({CurrentLevel})");
    }

    private void SimulateCooling()
    {
        if (CurrentTemperature > TargetTemperature)
        {
            CurrentTemperature = TargetTemperature;
            Console.WriteLine($"{Location} AC reached target temperature: {CurrentTemperature}°C");
        }
    }
}

// Класс для управления умным домом
public class SmartHome
{
    private List<ISwitchable> _devices;

    public SmartHome()
    {
        _devices = new List<ISwitchable>();
    }

    public void AddDevice(ISwitchable device)
    {
        _devices.Add(device);
        Console.WriteLine($"Added device: {device.DeviceName}");
    }

    public void TurnOnAll()
    {
        Console.WriteLine("\n=== Turning ON all devices ===");
        foreach (var device in _devices)
        {
            device.On();
        }
    }

    public void TurnOffAll()
    {
        Console.WriteLine("\n=== Turning OFF all devices ===");
        foreach (var device in _devices)
        {
            device.Off();
        }
    }

    public void DisplayStatus()
    {
        Console.WriteLine("\n=== Smart Home Status ===");
        foreach (var device in _devices)
        {
            string status = device.IsOn ? "ON" : "OFF";
            Console.WriteLine($"{device.DeviceName}: {status}");

            // Дополнительная информация для регулируемых устройств
            if (device is IAdjustable adjustable)
            {
                Console.WriteLine($"  Level: {adjustable.CurrentLevel} ({adjustable.MinLevel}-{adjustable.MaxLevel})");
            }

            // Дополнительная информация для устройств с температурой
            if (device is ITemperatureControllable tempDevice)
            {
                Console.WriteLine($"  Temperature: {tempDevice.CurrentTemperature}°C (Target: {tempDevice.TargetTemperature}°C)");
            }
        }
    }

    public void AdjustDeviceLevel(string deviceName, int level)
    {
        var device = _devices.FirstOrDefault(d => d.DeviceName == deviceName);
        if (device is IAdjustable adjustable)
        {
            adjustable.SetLevel(level);
        }
        else
        {
            Console.WriteLine($"Device {deviceName} is not adjustable");
        }
    }

    public void SetDeviceTemperature(string deviceName, int temperature)
    {
        var device = _devices.FirstOrDefault(d => d.DeviceName == deviceName);
        if (device is ITemperatureControllable tempDevice)
        {
            tempDevice.SetTemperature(temperature);
        }
        else
        {
            Console.WriteLine($"Device {deviceName} does not support temperature control");
        }
    }

    public List<IAdjustable> GetAdjustableDevices()
    {
        return _devices.OfType<IAdjustable>().ToList();
    }

    public List<ITemperatureControllable> GetTemperatureDevices()
    {
        return _devices.OfType<ITemperatureControllable>().ToList();
    }
}

