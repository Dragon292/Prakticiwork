//1)
public abstract class Figure
{
    public abstract double GetArea();

    public void DisplayArea()
    {
        Console.WriteLine($"Площадь: {GetArea():F2}");
    }
}

public class Circle : Figure
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Rectangle : Figure
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double GetArea()
    {
        return Width * Height;
    }
}

//2)
public abstract class Transport
{
    public string Name { get; set; }

    public Transport(string name)
    {
        Name = name;
    }

    public abstract void Start();
    public abstract void Stop();

    public void DisplayInfo()
    {
        Console.WriteLine($"Транспорт: {Name}");
    }
}
public class Car : Transport
{
    public Car(string name) : base(name) { }

    public override void Start()
    {
        Console.WriteLine($"{Name}: Поворот ключа, двигатель запускается");
    }

    public override void Stop()
    {
        Console.WriteLine($"{Name}: Выключение двигателя");
    }

    public void Honk()
    {
        Console.WriteLine($"{Name}: Бип-бип!");
    }
}

public class Bicycle : Transport
{
    public Bicycle(string name) : base(name) { }

    public override void Start()
    {
        Console.WriteLine($"{Name}: Начало кручения педалей");
    }

    public override void Stop()
    {
        Console.WriteLine($"{Name}: Применение тормозов и остановка");
    }

    public void RingBell()
    {
        Console.WriteLine($"{Name}: Дзинь-дзинь!");
    }
}

//3)
public interface IPlayable
{
    void Play();
    void Pause();
    void Stop();
}

public class MusicPlayer : IPlayable
{
    public string SongName { get; set; }

    public MusicPlayer(string songName)
    {
        SongName = songName;
    }

    public void Play()
    {
        Console.WriteLine($"Воспроизведение музыки: {SongName}");
    }

    public void Pause()
    {
        Console.WriteLine($"Музыка приостановлена: {SongName}");
    }

    public void Stop()
    {
        Console.WriteLine($"Музыка остановлена: {SongName}");
    }
}

public class VideoPlayer : IPlayable
{
    public string VideoName { get; set; }
    public string Quality { get; set; }

    public VideoPlayer(string videoName, string quality = "1080p")
    {
        VideoName = videoName;
        Quality = quality;
    }
    public void Play()
    {
        Console.WriteLine($"Воспроизведение видео: {VideoName} [{Quality}]");
    }
    public void Pause()
    {
        Console.WriteLine($"Видео приостановлено: {VideoName}");
    }
    public void Stop()
    {
        Console.WriteLine($"Видео остановлено: {VideoName}");
    }
    public void SetQuality(string quality)
    {
        Quality = quality;
        Console.WriteLine($"Качество установлено: {quality}");
    }
}

//4)
public abstract class Device
{
    public string Brand { get; set; }
    public string Model { get; set; }

    public Device(string brand, string model)
    {
        Brand = brand;
        Model = model;
    }

    public abstract void TurnOn();
    public abstract void TurnOff();

    public void DisplayInfo()
    {
        Console.WriteLine($"Устройство: {Brand} {Model}");
    }
}
public interface IRechargeable
{
    void Recharge();
    int BatteryLevel { get; }
}
public class Smartphone : Device, IRechargeable
{
    public int BatteryLevel { get; private set; }

    public Smartphone(string brand, string model, int batteryLevel = 50)
        : base(brand, model)
    {
        BatteryLevel = batteryLevel;
    }

    public override void TurnOn()
    {
        Console.WriteLine($"{Brand} {Model}: Экран загорается, добро пожаловать!");
    }

    public override void TurnOff()
    {
        Console.WriteLine($"{Brand} {Model}: До свидания! Экран гаснет");
    }

    public void Recharge()
    {
        BatteryLevel = 100;
        Console.WriteLine($"{Brand} {Model}: Полностью заряжено! Заряд: {BatteryLevel}%");
    }

    public void UseBattery(int amount)
    {
        BatteryLevel = Math.Max(0, BatteryLevel - amount);
        Console.WriteLine($"{Brand} {Model}: Заряд использован. Осталось: {BatteryLevel}%");
    }
}

//5)
public interface IPayment
{
    bool Pay(double amount);
    string PaymentMethod { get; }
}

public class CreditCardPayment : IPayment
{
    public string CardNumber { get; set; }
    public string CardHolder { get; set; }
    public string PaymentMethod => "Кредитная карта";

    public CreditCardPayment(string cardNumber, string cardHolder)
    {
        CardNumber = cardNumber;
        CardHolder = cardHolder;
    }

    public bool Pay(double amount)
    {
        Console.WriteLine($"Обработка платежа по кредитной карте на ${amount:F2}");
        Console.WriteLine($"Карта: {CardNumber}, Владелец: {CardHolder}");

        // Симуляция обработки платежа
        bool success = SimulatePaymentProcessing();

        if (success)
        {
            Console.WriteLine("Платеж успешен!");
        }
        else
        {
            Console.WriteLine("Платеж не прошел!");
        }

        return success;
    }
    private bool SimulatePaymentProcessing()
    {
        // В реальной системе здесь была бы логика обработки платежа
        return true;
    }
}

public class PayPalPayment : IPayment
{
    public string Email { get; set; }
    public string PaymentMethod => "PayPal";

    public PayPalPayment(string email)
    {
        Email = email;
    }

    public bool Pay(double amount)
    {
        Console.WriteLine($"Обработка PayPal платежа на ${amount:F2}");
        Console.WriteLine($"Email: {Email}");

        // Симуляция обработки платежа
        bool success = SimulatePaymentProcessing();

        if (success)
        {
            Console.WriteLine("PayPal платеж успешен!");
        }
        else
        {
            Console.WriteLine("PayPal платеж не прошел!");
        }

        return success;
    }

    private bool SimulatePaymentProcessing()
    {
        return true;
    }
}

public class OrderProcessor
{
    public void ProcessOrder(IPayment payment, double amount, string orderDescription)
    {
        Console.WriteLine($"\nОбработка заказа: {orderDescription}");
        Console.WriteLine($"Сумма: ${amount:F2}");
        Console.WriteLine($"Способ оплаты: {payment.PaymentMethod}");

        bool paymentSuccess = payment.Pay(amount);

        if (paymentSuccess)
        {
            Console.WriteLine("Заказ успешно завершен!");
        }
        else
        {
            Console.WriteLine("Заказ не выполнен из-за проблем с оплатой.");
        }
    }
}

//6)
public class TestPolymorphism
{
    public static void Test()
    {
        Console.WriteLine("\n=== Полиморфизм через абстракцию ===");

        // Создаем список объектов разных типов
        Figure[] figures = new Figure[]
        {
            new Circle(10),
            new Rectangle(8, 12),
            new Circle(15),
            new Rectangle(5, 5)
        };

        Transport[] transports = new Transport[]
        {
            new Car("Honda Civic"),
            new Bicycle("Городской велосипед"),
            new Car("Ford Focus"),
            new Bicycle("Шоссейный велосипед")
        };

        IPlayable[] mediaPlayers = new IPlayable[]
        {
            new MusicPlayer("Imagine - Джон Леннон"),
            new VideoPlayer("Боевик", "1080p"),
            new MusicPlayer("Yesterday - Битлз")
        };

        // Демонстрация полиморфизма с фигурами
        Console.WriteLine("Вычисление площади фигур:");
        double totalArea = 0;
        foreach (var figure in figures)
        {
            double area = figure.GetArea(); // Полиморфный вызов
            Console.WriteLine($"Площадь: {area:F2}");
            totalArea += area;
        }
        Console.WriteLine($"Общая площадь: {totalArea:F2}");

        // Демонстрация полиморфизма с транспортом
        Console.WriteLine("\nОперации с транспортом:");
        foreach (var transport in transports)
        {
            transport.Start(); // Полиморфный вызов
            transport.Stop();  // Полиморфный вызов
            Console.WriteLine();
        }

        // Демонстрация полиморфизма с медиаплеерами
        Console.WriteLine("Медиаплееры:");
        foreach (var player in mediaPlayers)
        {
            player.Play();  // Полиморфный вызов
            player.Pause(); // Полиморфный вызов
            Console.WriteLine();
        }
    }
}

//7)
public abstract class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Animal(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public abstract void Eat();
    public abstract void MakeSound();

    public virtual void Sleep()
    {
        Console.WriteLine($"{Name} спит...");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Животное: {Name}, Возраст: {Age}");
    }
}

// Интерфейс Перемещаемый
public interface IMovable
{
    void Move();
    string MovementType { get; }
}

public class Dog : Animal, IMovable
{
    public string Breed { get; set; }
    public string MovementType => "бег";

    public Dog(string name, int age, string breed) : base(name, age)
    {
        Breed = breed;
    }

    public override void Eat()
    {
        Console.WriteLine($"{Name} породы {Breed} ест собачий корм");
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Гав! Гав!");
    }

    public void Move()
    {
        Console.WriteLine($"{Name} {MovementType} радостно виляя хвостом");
    }

    public void Fetch()
    {
        Console.WriteLine($"{Name} приносит мячик!");
    }
}

public class Cat : Animal, IMovable
{
    public string Color { get; set; }
    public string MovementType => "крадется";

    public Cat(string name, int age, string color) : base(name, age)
    {
        Color = color;
    }

    public override void Eat()
    {
        Console.WriteLine($"{Name} {Color} кошка ест рыбу");
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Мяу! Мяу!");
    }

    public void Move()
    {
        Console.WriteLine($"{Name} {MovementType} бесшумно и грациозно");
    }

    public void ClimbTree()
    {
        Console.WriteLine($"{Name} залезает на дерево");
    }
}

public class Bird : Animal, IMovable
{
    public string Species { get; set; }
    public double Wingspan { get; set; }
    public string MovementType => "полет";

    public Bird(string name, int age, string species, double wingspan)
        : base(name, age)
    {
        Species = species;
        Wingspan = wingspan;
    }

    public override void Eat()
    {
        Console.WriteLine($"{Name} вида {Species} ест семена и насекомых");
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} говорит: Чик-чирик!");
    }

    public void Move()
    {
        Console.WriteLine($"{Name} {MovementType} с размахом крыльев {Wingspan}см");
    }

    public void BuildNest()
    {
        Console.WriteLine($"{Name} строит гнездо");
    }
}

public class Zoo
{
    private List<Animal> animals;
    private List<IMovable> movables;

    public Zoo()
    {
        animals = new List<Animal>();
        movables = new List<IMovable>();
    }

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);

        // Если животное может перемещаться, добавляем в соответствующий список
        if (animal is IMovable movable)
        {
            movables.Add(movable);
        }
    }

    public void StartDay()
    {
        Console.WriteLine("=== НАЧАЛО ДНЯ В ЗООПАРКЕ ===");

        Console.WriteLine("\n1. Все животные просыпаются:");
        foreach (var animal in animals)
        {
            animal.DisplayInfo();
        }

        Console.WriteLine("\n2. Утреннее кормление:");
        foreach (var animal in animals)
        {
            animal.Eat(); // Полиморфный вызов
        }

        Console.WriteLine("\n3. Звуки животных:");
        foreach (var animal in animals)
        {
            animal.MakeSound(); // Полиморфный вызов
        }

        Console.WriteLine("\n4. Время движения:");
        foreach (var movable in movables)
        {
            movable.Move(); // Полиморфный вызов через интерфейс
        }

        Console.WriteLine("\n5. Особые активности:");
        foreach (var animal in animals)
        {
            if (animal is Dog dog)
            {
                dog.Fetch();
            }
            else if (animal is Cat cat)
            {
                cat.ClimbTree();
            }
            else if (animal is Bird bird)
            {
                bird.BuildNest();
            }
        }

        Console.WriteLine("\n6. Время сна:");
        foreach (var animal in animals)
        {
            animal.Sleep();
        }

        Console.WriteLine("=== КОНЕЦ ДНЯ В ЗООПАРКЕ ===");
    }

    public void DisplayStatistics()
    {
        Console.WriteLine("\n=== СТАТИСТИКА ЗООПАРКА ===");
        Console.WriteLine($"Всего животных: {animals.Count}");
        Console.WriteLine($"Подвижных животных: {movables.Count}");

        var dogs = animals.OfType<Dog>().Count();
        var cats = animals.OfType<Cat>().Count();
        var birds = animals.OfType<Bird>().Count();

        Console.WriteLine($"Собак: {dogs}");
        Console.WriteLine($"Кошек: {cats}");
        Console.WriteLine($"Птиц: {birds}");
    }
}


