public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class Student : Person
{
    public string Grade { get; set; }

    public void DisplayInfo()
    {
        Console.WriteLine($"Студент: {Name}, Возраст: {Age}, Оценка: {Grade}");
    }
}

// Проверка доступа
public class TestInheritance
{
    public static void Test()
    {
        Student student = new Student();
        student.Name = "Алиса";     // Доступ к полю базового класса
        student.Age = 20;           // Доступ к полю базового класса
        student.Grade = "A";        // Доступ к собственному полю
        student.DisplayInfo();
    }
}

public class Vehicle
{
    public virtual void Move()
    {
        Console.WriteLine("Транспортное средство движется");
    }
}

public class Car : Vehicle
{
    public void Honk()
    {
        Console.WriteLine("Автомобиль сигналит");
    }

    public override void Move()
    {
        Console.WriteLine("Автомобиль едет по дороге");
    }
}

public class Bicycle : Vehicle
{
    public void RingBell()
    {
        Console.WriteLine("Велосипед звонит в звонок");
    }

    public override void Move()
    {
        Console.WriteLine("Велосипед крутит педали");
    }
}

public class TestVehicles
{
    public static void Test()
    {
        Vehicle car = new Car();
        Vehicle bicycle = new Bicycle();

        car.Move();        // Автомобиль едет по дороге
        bicycle.Move();    // Велосипед крутит педали

        ((Car)car).Honk(); // Автомобиль сигналит
    }
}

public class Animal
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Животное издает звук");
    }
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Собака лает: Гав! Гав!");
    }
}

public class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Кошка мяукает: Мяу! Мяу!");
    }
}

public class TestAnimals
{
    public static void Test()
    {
        Animal dog = new Dog();
        Animal cat = new Cat();

        dog.MakeSound(); // Собака лает: Гав! Гав!
        cat.MakeSound(); // Кошка мяукает: Мяу! Мяу!
    }
}

public class Employee
{
    public string Name { get; set; }

    public Employee(string name)
    {
        Name = name;
        Console.WriteLine($"Конструктор Employee: {name}");
    }
}

public class Manager : Employee
{
    public string Department { get; set; }

    public Manager(string name, string department) : base(name)
    {
        Department = department;
        Console.WriteLine($"Конструктор Manager: {name}, {department}");
    }
}

public class TestConstructors
{
    public static void Test()
    {
        Manager manager = new Manager("Иван Петров", "ИТ");
        Console.WriteLine($"Менеджер: {manager.Name}, Отдел: {manager.Department}");
    }
}

public class BankAccount
{
    protected decimal balance;

    public BankAccount(decimal initialBalance = 0)
    {
        balance = initialBalance;
    }

    public decimal GetBalance()
    {
        return balance;
    }
}

public class SavingsAccount : BankAccount
{
    public SavingsAccount(decimal initialBalance = 0) : base(initialBalance)
    {
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount; // Доступ к защищённому полю
            Console.WriteLine($"Внесено: {amount}, Новый баланс: {balance}");
        }
    }

    public void AddInterest(decimal rate)
    {
        decimal interest = balance * rate / 100;
        balance += interest; // Доступ к защищённому полю
        Console.WriteLine($"Начислены проценты: {interest}, Новый баланс: {balance}");
    }
}

public class TestBankAccount
{
    public static void Test()
    {
        SavingsAccount account = new SavingsAccount(1000);
        account.Deposit(500);
        account.AddInterest(5);
        Console.WriteLine($"Итоговый баланс: {account.GetBalance()}");
    }
}

public class Device
{
    public string Brand { get; set; }

    public virtual void TurnOn()
    {
        Console.WriteLine("Устройство включается");
    }

    public void DisplayBrand()
    {
        Console.WriteLine($"Бренд: {Brand}");
    }
}

public class Computer : Device
{
    public string Processor { get; set; }

    public override void TurnOn()
    {
        Console.WriteLine("Компьютер загружается");
    }

    public void RunProgram()
    {
        Console.WriteLine("Компьютер запускает программу");
    }
}

public class Laptop : Computer
{
    public int BatteryLife { get; set; }

    public override void TurnOn()
    {
        Console.WriteLine("Ноутбук запускается от батареи");
    }

    public void CloseLid()
    {
        Console.WriteLine("Крышка ноутбука закрывается, переход в спящий режим");
    }
}

public class TestMultipleInheritance
{
    public static void Test()
    {
        Laptop laptop = new Laptop
        {
            Brand = "Dell",
            Processor = "Intel i7",
            BatteryLife = 8
        };

        // Вызов методов с разных уровней наследования
        laptop.DisplayBrand();  // Из Device
        laptop.TurnOn();        // Из Laptop (переопределён)
        laptop.RunProgram();    // Из Computer
        laptop.CloseLid();      // Из Laptop
    }
}

public class Person1
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person1(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public virtual void GetInfo()
    {
        Console.WriteLine($"Человек: {Name}, Возраст: {Age}");
    }
}

public class Teacher : Person1
{
    public string Subject { get; set; }

    public Teacher(string name, int age, string subject) : base(name, age)
    {
        Subject = subject;
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Учитель: {Name}, Возраст: {Age}, Предмет: {Subject}");
    }
}

public class Student1 : Person1
{
    public string Grade { get; set; }

    public Student1(string name, int age, string grade) : base(name, age)
    {
        Grade = grade;
    }

    public override void GetInfo()
    {
        Console.WriteLine($"Студент: {Name}, Возраст: {Age}, Класс: {Grade}");
    }
}

public class SchoolClass
{
    public string ClassName { get; set; }
    private List<Person1> people;

    public SchoolClass(string className)
    {
        ClassName = className;
        people = new List<Person1>();
    }

    public void AddPerson(Person1 person)
    {
        people.Add(person);
    }

    public void DisplayAllInfo()
    {
        Console.WriteLine($"\n--- Класс: {ClassName} ---");
        foreach (var person in people)
        {
            person.GetInfo(); // Полиморфизм: один метод работает с разными типами
        }
    }

    public void DisplayStatistics()
    {
        int teachers = people.OfType<Teacher>().Count();
        int students = people.OfType<Student1>().Count();

        Console.WriteLine($"\nСтатистика для {ClassName}:");
        Console.WriteLine($"Всего людей: {people.Count}");
        Console.WriteLine($"Учителей: {teachers}");
        Console.WriteLine($"Студентов: {students}");
    }
}

public class TestSchoolSystem
{
    public static void Test()
    {
        // Создаем объекты разных типов
        Teacher mathTeacher = new Teacher("Иван Иванов", 45, "Математика");
        Teacher scienceTeacher = new Teacher("Петр Петров", 38, "Физика");

        Student1 student1 = new Student1("Анна Смирнова", 15, "10А");
        Student1 student2 = new Student1("Борис Орлов", 16, "10Б");
        Student1 student3 = new Student1("Светлана Ковалева", 15, "10А");

        // Создаем класс и добавляем людей
        SchoolClass mathClass = new SchoolClass("Математический класс");
        mathClass.AddPerson(mathTeacher);
        mathClass.AddPerson(student1);
        mathClass.AddPerson(student2);
        mathClass.AddPerson(student3);

        // Демонстрация полиморфизма
        SchoolClass scienceClass = new SchoolClass("Физический класс");
        scienceClass.AddPerson(scienceTeacher);
        scienceClass.AddPerson(student1);

        // Вывод информации
        mathClass.DisplayAllInfo();
        scienceClass.DisplayAllInfo();

        // Статистика
        mathClass.DisplayStatistics();
        scienceClass.DisplayStatistics();

        // Дополнительная демонстрация полиморфизма
        Console.WriteLine("\n--- Индивидуальная информация ---");
        Person1[] people = new Person1[]
        {
            mathTeacher,
            scienceTeacher,
            student1,
            student2,
            student3
        };

        foreach (var person in people)
        {
            person.GetInfo(); // Один метод, разное поведение
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== 1. Простейшее наследование ===");
        TestInheritance.Test();

        Console.WriteLine("\n=== 2. Методы базового класса ===");
        TestVehicles.Test();

        Console.WriteLine("\n=== 3. Переопределение методов ===");
        TestAnimals.Test();

        Console.WriteLine("\n=== 4. Конструкторы в наследовании ===");
        TestConstructors.Test();

        Console.WriteLine("\n=== 5. Доступ к защищённым членам ===");
        TestBankAccount.Test();

        Console.WriteLine("\n=== 6. Множественный уровень наследования ===");
        TestMultipleInheritance.Test();

        Console.WriteLine("\n=== 7. Расширенное задание: Система «Школа» ===");
        TestSchoolSystem.Test();
    }
}