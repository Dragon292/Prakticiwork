// ПОЛИМОРФИЗМ
//Способность объектов с одинаковым интерфейсом иметь разное поведение. "Один интерфейс - много реализаций". Позволяет работать с разными типами через общий базовый тип.

//ИНТЕРФЕЙС
//Контракт, который определяет ЧТО должен делать класс, но не КАК. Содержит только сигнатуры методов без реализации. Класс может реализовывать несколько интерфейсов.

//ВИРТУАЛЬНЫЙ МЕТОД
//Метод в базовом классе, который МОЖНО переопределить в наследниках. Имеет базовую реализацию. Переопределение опционально с помощью override.

//АБСТРАКТНЫЙ МЕТОД
//Метод без реализации в абстрактном классе. ДОЛЖЕН быть реализован в наследниках. Класс с абстрактными методами должен быть абстрактным.

//КОЛЛЕКЦИЯ С РАЗНЫМИ ОБЪЕКТАМИ
//Возможность хранить в одной коллекции объекты разных типов через общий базовый тип или интерфейс. Обеспечивает полиморфную обработку элементов.

//КОЛЛЕКЦИЯ
//Структура данных для хранения группы объектов

//Основные характеристики:
//Контейнер для хранения множества элементов

//Динамический размер (в отличие от массива)

//Типизированная - хранит объекты определенного типа

//Предоставляет методы для добавления, удаления, поиска элементов

//Основные типы в C#:
//List<T> - список с доступом по индексу

//Dictionary<TKey, TValue> - коллекция ключ-значение

//HashSet<T> - множество уникальных элементов

//Queue<T> - очередь (FIFO)

//Stack<T> - стек (LIFO)

// 1. ВИРТУАЛЬНЫЕ МЕТОДЫ В НАСЛЕДОВАНИИ
// Базовый класс с виртуальным методом - может быть переопределен в наследниках
public class Animal
{
    // virtual позволяет наследникам переопределить этот метод
    public virtual void MakeSound()
    {
        Console.WriteLine("Животное издает звук");
    }
}

// Наследник Dog переопределяет виртуальный метод
public class Dog : Animal
{
    // override - переопределение виртуального метода базового класса
    public override void MakeSound()
    {
        Console.WriteLine("Собака гавкает: Гав-гав!");
    }
}

// Наследник Cat также переопределяет метод
public class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Кошка мяукает: Мяу-мяу!");
    }
}

// 2. ПОЛИМОРФИЗМ ЧЕРЕЗ АБСТРАКТНЫЙ КЛАСС
// Абстрактный класс - нельзя создать экземпляр, только наследовать
public abstract class Shape
{
    // Абстрактный метод - не имеет реализации, должен быть реализован в наследниках
    public abstract void Draw();
}

// Конкретные классы реализуют абстрактный метод
public class Circle : Shape
{
    // override - обязательная реализация абстрактного метода
    public override void Draw()
    {
        Console.WriteLine("Рисую круг ○");
    }
}

public class Rectangle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Рисую прямоугольник ▭");
    }
}

public class Triangle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Рисую треугольник △");
    }
}

// 3. ИНТЕРФЕЙС И ПОЛИМОРФИЗМ
// Интерфейс - определяет контракт, который должны реализовать классы
public interface IPayment 
{
    // Метод без реализации - только сигнатура
    void Pay(double amount);
}

// Классы реализуют интерфейс IPayment
public class CreditCardPayment : IPayment //интерфейс который он реализует
{
    // explicit implementation - явная реализация интерфейса
    public void Pay(double amount)
    {
        Console.WriteLine($"Оплата кредитной картой: ${amount}");
    }
}

public class CashPayment : IPayment //интерфейс который он реализует
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Наличная оплата: ${amount}");
    }
}

public class CryptoPayment : IPayment //интерфейс который он реализует
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Криптовалютная оплата: ${amount} BTC");
    }
}

// Класс, который работает с интерфейсом, а не с конкретными классами
public class PaymentProcessor
{
    // Метод принимает интерфейс - может работать с ЛЮБЫМ классом, реализующим IPayment
    public void ProcessPayment(IPayment payment, double amount)
    {
        payment.Pay(amount); // Полиморфный вызов
    }
}

// 4. ПОЛИМОРФИЗМ С ПЕРЕОПРЕДЕЛЕНИЕM ToString()
// Базовый класс Person
public class Person
{
    public string Name { get; set; }

    public Person(string name)
    {
        Name = name;
    }

    // virtual - позволяет наследникам переопределить ToString()
    public override string ToString()
    {
        return $"Человек: {Name}";
    }
}

// Наследник Student
public class Student : Person
{
    public string Major { get; set; }

    // base(name) - вызов конструктора базового класса
    public Student(string name, string major) : base(name)
    {
        Major = major;
    }

    // Переопределение ToString() для Student
    public override string ToString()
    {
        return $"Студент: {Name}, специальность: {Major}";
    }
}

// Наследник Teacher
public class Teacher : Person
{
    public string Subject { get; set; }

    public Teacher(string name, string subject) : base(name)
    {
        Subject = subject;
    }

    // Переопределение ToString() для Teacher
    public override string ToString()
    {
        return $"Учитель: {Name}, предмет: {Subject}";
    }
}

// 5. КОЛЛЕКЦИЯ С РАЗНЫМИ ОБЪЕКТАМИ
// Абстрактный класс Transport
public abstract class Transport
{
    // Абстрактный метод - должен быть реализован в наследниках
    public abstract void Move();
}

// Конкретные реализации транспорта
public class Car : Transport
{
    public override void Move()
    {
        Console.WriteLine("Машина едет по дороге");
    }
}

public class Plane : Transport
{
    public override void Move()
    {
        Console.WriteLine("Самолет летит по небу");
    }
}

public class Ship : Transport
{
    public override void Move()
    {
        Console.WriteLine("Корабль плывет по морю");
    }
}

// 6. ПОЛИМОРФИЗМ И ИНТЕРФЕЙСЫ ВМЕСТЕ
// Интерфейс для логирования
public interface ILogger //интерфейс который он реализует
{
    void Log(string message);
}

// Реализация логирования в консоль
public class ConsoleLogger : ILogger //интерфейс который он реализует
{
    public void Log(string message)
    {
        Console.WriteLine($"[CONSOLE] {DateTime.Now:HH:mm:ss}: {message}");
    }
}

// Реализация логирования в файл (здесь имитация)
public class FileLogger : ILogger //интерфейс который он реализует
{
    public void Log(string message)
    {
        Console.WriteLine($"[FILE] {DateTime.Now:HH:mm:ss}: {message} (записано в файл)");
    }
}

// Класс Application зависит от интерфейса ILogger, а не от конкретных классов
public class Application
{
    private ILogger _logger; // Поле типа интерфейса

    // Внедрение зависимости через конструктор
    public Application(ILogger logger)
    {
        _logger = logger; // Принимаем ЛЮБОЙ объект, реализующий ILogger
    }

    public void Run()
    {
        _logger.Log("Приложение запущено");
        _logger.Log("Выполняется важная операция");
        _logger.Log("Приложение завершено");
    }
}

// 7. РАСШИРЕННОЕ ЗАДАНИЕ: СИСТЕМА «ЗООПАРК»
// Интерфейс для перемещения
public interface IMovable //интерфейс который он реализует
{
    void Move();
}

// Абстрактный базовый класс для животных
public abstract class ZooAnimal
{
    public string Name { get; set; }

    public ZooAnimal(string name)
    {
        Name = name;
    }

    // Абстрактные методы - должны быть реализованы в наследниках
    public abstract void Eat();
    public abstract void MakeSound();
}

// Класс Lion наследует ZooAnimal и реализует IMovable
public class Lion : ZooAnimal, IMovable
{
    public Lion(string name) : base(name) { }

    public override void Eat()
    {
        Console.WriteLine($"{Name} (лев) ест мясо");
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} рычит: Р-р-р-р!");
    }

    // Реализация метода из интерфейса IMovable
    public void Move()
    {
        Console.WriteLine($"{Name} бежит по саванне");
    }
}

public class Elephant : ZooAnimal, IMovable
{
    public Elephant(string name) : base(name) { }

    public override void Eat()
    {
        Console.WriteLine($"{Name} (слон) ест траву и фрукты");
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} трубит: Ту-ту-у-у!");
    }

    public void Move()
    {
        Console.WriteLine($"{Name} тяжело шагает");
    }
}

public class Monkey : ZooAnimal, IMovable
{
    public Monkey(string name) : base(name) { }

    public override void Eat()
    {
        Console.WriteLine($"{Name} (обезьяна) ест бананы");
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} кричит: У-у-у-а-а-а!");
    }

    public void Move()
    {
        Console.WriteLine($"{Name} прыгает по деревьям");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ПОЛИМОРФИЗМА В C# ===\n");

        // 1. ДЕМОНСТРАЦИЯ ВИРТУАЛЬНЫХ МЕТОДОВ
        // Полиморфизм: работа с разными типами через общий базовый класс
        Console.WriteLine("1. ВИРТУАЛЬНЫЕ МЕТОДЫ:");
        List<Animal> animals = new List<Animal> { new Dog(), new Cat(), new Animal() };
        foreach (Animal animal in animals)
        {
            // Во время выполнения определяется, какой именно метод вызывать
            animal.MakeSound(); // Полиморфный вызов - разное поведение для каждого типа
        }

        // 2. ДЕМОНСТРАЦИЯ АБСТРАКТНЫХ КЛАССОВ
        // Абстрактные классы обеспечивают общий интерфейс для группы родственных классов
        Console.WriteLine("\n2. АБСТРАКТНЫЕ КЛАССЫ:");
        List<Shape> shapes = new List<Shape> { new Circle(), new Rectangle(), new Triangle() };
        foreach (Shape shape in shapes)
        {
            shape.Draw(); // Каждая фигура рисуется по-своему
        }

        // 3. ДЕМОНСТРАЦИЯ ИНТЕРФЕЙСОВ
        // Интерфейсы позволяют разным классам иметь общее поведение
        Console.WriteLine("\n3. ИНТЕРФЕЙСЫ:");
        PaymentProcessor processor = new PaymentProcessor();
        List<IPayment> payments = new List<IPayment>
        {
            new CreditCardPayment(),
            new CashPayment(),
            new CryptoPayment()
        };

        for (int i = 0; i < payments.Count; i++)
        {
            // Один метод работает с разными типами платежей
            processor.ProcessPayment(payments[i], 100 + i * 50);
        }

        // 4. ДЕМОНСТРАЦИЯ ПЕРЕОПРЕДЕЛЕНИЯ ToString()
        // ToString() - виртуальный метод System.Object, который можно переопределить
        Console.WriteLine("\n4. ПЕРЕОПРЕДЕЛЕНИЕ ToString():");
        List<Person> people = new List<Person>
        {
            new Person("Иван"),
            new Student("Мария", "Информатика"),
            new Teacher("Петр", "Математика")
        };

        foreach (Person person in people)
        {
            // Console.WriteLine автоматически вызывает ToString()
            Console.WriteLine(person); // Полиморфное поведение
        }

        // 5. ДЕМОНСТРАЦИЯ КОЛЛЕКЦИИ АБСТРАКТНЫХ ОБЪЕКТОВ
        // Работа с разными типами через общий абстрактный класс
        Console.WriteLine("\n5. КОЛЛЕКЦИЯ ТРАНСПОРТА:");
        List<Transport> transports = new List<Transport> { new Car(), new Plane(), new Ship() };
        foreach (Transport transport in transports)
        {
            transport.Move(); // Каждый транспорт движется по-своему
        }

        // 6. ДЕМОНСТРАЦИЯ ЗАВИСИМОСТИ ОТ ИНТЕРФЕЙСОВ
        // Принцип инверсии зависимостей: зависеть от абстракций, а не от конкретных классов
        Console.WriteLine("\n6. ПОЛИМОРФИЗМ С ИНТЕРФЕЙСАМИ:");

        // Application работает с ConsoleLogger
        Application consoleApp = new Application(new ConsoleLogger());
        consoleApp.Run();

        Console.WriteLine();

        // Тот же Application работает с FileLogger - без изменения кода Application!
        Application fileApp = new Application(new FileLogger());
        fileApp.Run();

        // 7. ДЕМОНСТРАЦИЯ КОМПЛЕКСНОЙ СИСТЕМЫ
        // Сочетание наследования и интерфейсов
        Console.WriteLine("\n7. СИСТЕМА 'ЗООПАРК':");

        // Коллекция животных через базовый класс
        List<ZooAnimal> zooAnimals = new List<ZooAnimal>
        {
            new Lion("Симба"),
            new Elephant("Дамбо"),
            new Monkey("Чита")
        };

        // Такие же животные, но через интерфейс перемещения
        List<IMovable> movableAnimals = new List<IMovable>
        {
            new Lion("Симба"),
            new Elephant("Дамбо"),
            new Monkey("Чита")
        };

        Console.WriteLine("--- Методы животных (через ZooAnimal):");
        foreach (ZooAnimal animal in zooAnimals)
        {
            animal.Eat();       // Методы из абстрактного класса
            animal.MakeSound(); // Методы из абстрактного класса
        }

        Console.WriteLine("\n--- Движение животных (через IMovable):");
        foreach (IMovable movable in movableAnimals)
        {
            movable.Move(); // Метод из интерфейса
        }

        // Демонстрация, что один объект может использоваться через разные типы
        Console.WriteLine("\n--- Один объект через разные типы:");
        Lion simba = new Lion("Симба");

        // Как ZooAnimal
        ZooAnimal animalSimba = simba;
        animalSimba.Eat();
        animalSimba.MakeSound();

        // Как IMovable
        IMovable movableSimba = simba;
        movableSimba.Move();

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");

        Console.ReadLine();
    }
}


//1.Полиморфизм
//Один интерфейс - много реализаций

//Метод MakeSound() ведет себя по-разному для разных животных

//Метод Draw() по-разному реализован для разных фигур

//2. Виртуальные методы (virtual)
//Могут быть переопределены в наследниках

//Позволяют изменять поведение в производных классах

//Базовая реализация существует

//3. Абстрактные методы (abstract)
//Не имеют реализации в базовом классе

//Должны быть реализованы в наследниках

//Класс с абстрактными методами должен быть абстрактным

//4. Интерфейсы
//Определяют контракт (что делать)

//Не определяют реализацию (как делать)

//Класс может реализовывать несколько интерфейсов

//5. Преимущества полиморфизма:
//Гибкость - легко добавлять новые типы

//Расширяемость - минимальные изменения при добавлении функциональности

//Поддержка - код легче понимать и поддерживать

//Тестируемость - легко подменять реализации (как с ILogger)

//6. Разница между virtual и abstract:
//virtual -есть базовая реализация, переопределение опционально

//abstract -нет реализации, переопределение обязательно

//Этот код показывает, как полиморфизм позволяет создавать гибкие, расширяемые и поддерживаемые системы!