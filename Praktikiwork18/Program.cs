//Абстракция в объектно-ориентированном программировании (ООП) — это выделение общих характеристик объектов и скрытие их внутренних деталей.
//Программист показывает только те свойства и методы, которые действительно нужны для взаимодействия с объектом, игнорируя детали реализации. 
//Абстракция помогает:
//Упростить код — скрыть сложные детали реализации, сделать код более понятным и управляемым.
//Повторно использовать код — общие компоненты могут быть использованы в различных частях программы, что снижает дублирование кода.
//Разбить систему на более мелкие, управляемые части — это облегчает её понимание и поддержку.

// 1. Абстрактный класс «Фигура»
public abstract class Figure
{
    // Абстрактный метод для вычисления площади (должен быть реализован в наследниках)
    public abstract double GetArea();
}

// Класс Круг, наследующий от Figure
public class Circle : Figure
{
    public double Radius { get; set; }

    // Реализация абстрактного метода GetArea для круга
    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}

// Класс Прямоугольник, наследующий от Figure
public class Rectangle : Figure
{
    public double Width { get; set; }
    public double Height { get; set; }

    // Реализация абстрактного метода GetArea для прямоугольника
    public override double GetArea()
    {
        return Width * Height;
    }
}

// 2. Абстрактный класс «Транспорт»
public abstract class Transport
{
    // Абстрактные методы, которые должны быть реализованы в наследниках
    public abstract void Start();
    public abstract void Stop();
}

// Класс Автомобиль, наследующий от Transport
public class Car : Transport
{
    // Реализация метода Start для автомобиля
    public override void Start()
    {
        Console.WriteLine("Машина заводится");
    }

    // Реализация метода Stop для автомобиля
    public override void Stop()
    {
        Console.WriteLine("Машина останавливается");
    }
}

// Класс Велосипед, наследующий от Transport
public class Bicycle : Transport
{
    // Реализация метода Start для велосипеда
    public override void Start()
    {
        Console.WriteLine("Велосипед начинает движение");
    }

    // Реализация метода Stop для велосипеда
    public override void Stop()
    {
        Console.WriteLine("Велосипед останавливается");
    }
}

// 3. Интерфейс «IPlayable»
public interface IPlayable
{
    // Методы, которые должны быть реализованы в классах, имплементирующих интерфейс
    void Play();
    void Pause();
}

// Класс MusicPlayer, реализующий интерфейс IPlayable
public class MusicPlayer : IPlayable
{
    // Реализация метода Play интерфейса IPlayable
    public void Play()
    {
        Console.WriteLine("Музыка играет");
    }

    // Реализация метода Pause интерфейса IPlayable
    public void Pause()
    {
        Console.WriteLine("Музыка на паузе");
    }
}

// Класс VideoPlayer, реализующий интерфейс IPlayable
public class VideoPlayer : IPlayable
{
    // Реализация метода Play интерфейса IPlayable
    public void Play()
    {
        Console.WriteLine("Видео играет");
    }

    // Реализация метода Pause интерфейса IPlayable
    public void Pause()
    {
        Console.WriteLine("Видео на паузе");
    }
}

// 4. Комбинация абстрактного класса и интерфейса

// Абстрактный класс Device
public abstract class Device
{
    // Абстрактный метод, который должен быть реализован в наследниках
    public abstract void TurnOn();
}

// Интерфейс IRechargeable
public interface IRechargeable
{
    // Метод, который должны реализовать классы с возможностью зарядки
    void Recharge();
}

// Класс Smartphone наследует от Device и реализует интерфейс IRechargeable
public class Smartphone : Device, IRechargeable
{
    // Реализация абстрактного метода TurnOn из класса Device
    public override void TurnOn()
    {
        Console.WriteLine("Телефон включается");
    }

    // Реализация метода Recharge из интерфейса IRechargeable
    public void Recharge()
    {
        Console.WriteLine("Телефон заряжается");
    }
}

// 5. Система оплаты

// Интерфейс IPayment для системы оплаты
public interface IPayment
{
    // Метод для выполнения платежа
    void Pay(double amount);
}

// Класс CreditCardPayment, реализующий интерфейс IPayment
public class CreditCardPayment : IPayment
{
    // Реализация метода Pay для оплаты кредитной картой
    public void Pay(double amount)
    {
        Console.WriteLine($"Оплата картой: {amount} руб.");
    }
}

// Класс PayPalPayment, реализующий интерфейс IPayment
public class PayPalPayment : IPayment
{
    // Реализация метода Pay для оплаты через PayPal
    public void Pay(double amount)
    {
        Console.WriteLine($"Оплата PayPal: {amount} руб.");
    }
}

// 6. Полиморфизм через абстракцию
public class TestPolymorphism
{
    public static void Test()
    {
        // Создаем массив фигур разных типов (полиморфизм)
        Figure[] figures = {
            new Circle { Radius = 5 },
            new Rectangle { Width = 4, Height = 6 }
        };

        // Вызываем GetArea() для каждой фигуры - работает полиморфизм
        foreach (var figure in figures)
        {
            Console.WriteLine($"Площадь: {figure.GetArea():F2}");
        }

        // Создаем массив транспорта разных типов (полиморфизм)
        Transport[] transports = { new Car(), new Bicycle() };

        // Вызываем Start() и Stop() для каждого транспорта - работает полиморфизм
        foreach (var transport in transports)
        {
            transport.Start();
            transport.Stop();
        }
    }
}

// 7. Система «Животные»

// Абстрактный класс Animal
public abstract class Animal
{
    // Абстрактные методы, которые должны быть реализованы в наследниках
    public abstract void Eat();
    public abstract void MakeSound();
}

// Интерфейс IMovable для объектов, которые могут перемещаться
public interface IMovable
{
    void Move();
}

// Класс Dog наследует от Animal и реализует интерфейс IMovable
public class Dog : Animal, IMovable
{
    // Реализация метода Eat для собаки
    public override void Eat()
    {
        Console.WriteLine("Собака ест");
    }

    // Реализация метода MakeSound для собаки
    public override void MakeSound()
    {
        Console.WriteLine("Гав-гав!");
    }

    // Реализация метода Move из интерфейса IMovable
    public void Move()
    {
        Console.WriteLine("Собака бежит");
    }
}

// Класс Cat наследует от Animal и реализует интерфейс IMovable
public class Cat : Animal, IMovable
{
    // Реализация метода Eat для кошки
    public override void Eat()
    {
        Console.WriteLine("Кошка ест");
    }

    // Реализация метода MakeSound для кошки
    public override void MakeSound()
    {
        Console.WriteLine("Мяу!");
    }

    // Реализация метода Move из интерфейса IMovable
    public void Move()
    {
        Console.WriteLine("Кошка крадется");
    }
}

// Класс Bird наследует от Animal и реализует интерфейс IMovable
public class Bird : Animal, IMovable
{
    // Реализация метода Eat для птицы
    public override void Eat()
    {
        Console.WriteLine("Птица ест");
    }

    // Реализация метода MakeSound для птицы
    public override void MakeSound()
    {
        Console.WriteLine("Чирик-чирик!");
    }

    // Реализация метода Move из интерфейса IMovable
    public void Move()
    {
        Console.WriteLine("Птица летит");
    }
}

// Класс Zoo для демонстрации работы с животными
public class Zoo
{
    // Список для хранения животных
    private List<Animal> animals = new List<Animal>();

    // Метод для добавления животного в зоопарк
    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
    }

    // Метод для показа всех животных и их поведения
    public void ShowAnimals()
    {
        foreach (var animal in animals)
        {
            // Вызываем методы из абстрактного класса Animal
            animal.Eat();
            animal.MakeSound();

            // Проверяем, реализует ли животное интерфейс IMovable
            if (animal is IMovable movable)
            {
                // Если да, вызываем метод Move
                movable.Move();
            }
            Console.WriteLine(); // Пустая строка для разделения
        }
    }
}

class Program
{
    static void Main()
    {
        // Тест 1: Фигуры
        Console.WriteLine("1. Фигуры:");
        Figure circle = new Circle { Radius = 3 };
        Figure rectangle = new Rectangle { Width = 4, Height = 5 };
        Console.WriteLine($"Круг: {circle.GetArea():F2}");
        Console.WriteLine($"Прямоугольник: {rectangle.GetArea():F2}");

        // Тест 2: Транспорт
        Console.WriteLine("\n2. Транспорт:");
        Transport car = new Car();
        Transport bicycle = new Bicycle();
        car.Start(); car.Stop();
        bicycle.Start(); bicycle.Stop();

        // Тест 3: Плееры
        Console.WriteLine("\n3. Плееры:");
        IPlayable music = new MusicPlayer();
        IPlayable video = new VideoPlayer();
        music.Play(); music.Pause();
        video.Play(); video.Pause();

        // Тест 4: Устройство с зарядкой
        Console.WriteLine("\n4. Устройство:");
        Smartphone phone = new Smartphone();
        phone.TurnOn();
        phone.Recharge();

        // Тест 5: Система оплаты
        Console.WriteLine("\n5. Оплата:");
        IPayment card = new CreditCardPayment();
        IPayment paypal = new PayPalPayment();
        card.Pay(1000);
        paypal.Pay(500);

        // Тест 6: Полиморфизм
        Console.WriteLine("\n6. Полиморфизм:");
        TestPolymorphism.Test();

        // Тест 7: Зоопарк
        Console.WriteLine("\n7. Зоопарк:");
        Zoo zoo = new Zoo();
        zoo.AddAnimal(new Dog());
        zoo.AddAnimal(new Cat());
        zoo.AddAnimal(new Bird());
        zoo.ShowAnimals();
    }
}