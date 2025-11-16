using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Классы для демонстрации
public class Student
{
    public string Name { get; set; }
    public int Grade { get; set; }
    public int Age { get; set; }
    public string City { get; set; }

    public Student(string name, int grade, int age, string city)
    {
        Name = name;
        Grade = grade;
        Age = age;
        City = city;
    }
}

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public Product(string name, string category, decimal price, int stock)
    {
        Name = name;
        Category = category;
        Price = price;
        Stock = stock;
    }
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime OrderDate { get; set; }

    public Order(int id, int customerId, decimal amount, DateTime orderDate)
    {
        Id = id;
        CustomerId = customerId;
        Amount = amount;
        OrderDate = orderDate;
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    public Customer(int id, string name, string city)
    {
        Id = id;
        Name = name;
        City = city;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ LINQ ===\n");

        // Задание 1: Базовые операции LINQ
        DemonstrateBasicLinq();

        // Задание 2: Синтаксис запросов vs Методы расширения
        DemonstrateQueryVsMethodSyntax();

        // Задание 3: Группировка и агрегация
        DemonstrateGroupingAndAggregation();

        // Задание 4: Соединения (Joins)
        DemonstrateJoins();

        // Задание 5: Работа с множествами
        DemonstrateSetOperations();

        // Задание 6: Продвинутые техники LINQ
        DemonstrateAdvancedTechniques();

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
    }

    static void DemonstrateBasicLinq()
    {
        Console.WriteLine("1. БАЗОВЫЕ ОПЕРАЦИИ LINQ:");

        // Создание коллекции чисел
        List<int> numbers = new List<int> { 5, 2, 8, 1, 9, 3, 6, 4, 7, 10, 2, 8 };

        // ========== ФИЛЬТРАЦИЯ (WHERE) ==========
        Console.WriteLine("\n--- Фильтрация (Where) ---");
        var evenNumbers = numbers.Where(n => n % 2 == 0);
        Console.WriteLine("Четные числа: " + string.Join(", ", evenNumbers));

        var largeNumbers = numbers.Where(n => n > 5);
        Console.WriteLine("Числа > 5: " + string.Join(", ", largeNumbers));

        // ========== ПРОЕКЦИЯ (SELECT) ==========
        Console.WriteLine("\n--- Проекция (Select) ---");
        var squaredNumbers = numbers.Select(n => n * n);
        Console.WriteLine("Квадраты чисел: " + string.Join(", ", squaredNumbers));

        var numberDescriptions = numbers.Select(n => $"Число: {n}, Квадрат: {n * n}");
        Console.WriteLine("Описания: " + string.Join("; ", numberDescriptions));

        // ========== СОРТИРОВКА (ORDERBY) ==========
        Console.WriteLine("\n--- Сортировка (OrderBy) ---");
        var ascending = numbers.OrderBy(n => n);
        Console.WriteLine("По возрастанию: " + string.Join(", ", ascending));

        var descending = numbers.OrderByDescending(n => n);
        Console.WriteLine("По убыванию: " + string.Join(", ", descending));

        // ========== ПОЛУЧЕНИЕ ЭЛЕМЕНТОВ ==========
        Console.WriteLine("\n--- Получение элементов ---");
        Console.WriteLine($"First: {numbers.First()}");
        Console.WriteLine($"First > 5: {numbers.First(n => n > 5)}");
        Console.WriteLine($"Last: {numbers.Last()}");
        Console.WriteLine($"Last < 5: {numbers.Last(n => n < 5)}");

        // ========== ОТЛОЖЕННОЕ ВЫПОЛНЕНИЕ ==========
        Console.WriteLine("\n--- Отложенное выполнение ---");
        var deferredQuery = numbers.Where(n => n > 3).Select(n => n * 2);
        Console.WriteLine("Запрос создан, но еще не выполнен");

        numbers.Add(15); // Добавляем элемент ПОСЛЕ создания запроса
        Console.WriteLine("После добавления числа 15: " + string.Join(", ", deferredQuery));

        // ========== НЕМЕДЛЕННОЕ ВЫПОЛНЕНИЕ ==========
        Console.WriteLine("\n--- Немедленное выполнение ---");
        var immediateResult = numbers.Where(n => n > 3).Select(n => n * 2).ToList();
        Console.WriteLine("Результат материализован в список");

        numbers.Add(20); // Добавляем элемент ПОСЛЕ материализации
        Console.WriteLine("После добавления числа 20: " + string.Join(", ", immediateResult));
    }

    static void DemonstrateQueryVsMethodSyntax()
    {
        Console.WriteLine("\n\n2. СИНТАКСИС ЗАПРОСОВ VS МЕТОДЫ РАСШИРЕНИЯ:");

        // Создание коллекции студентов
        List<Student> students = new List<Student>
        {
            new Student("Иван", 85, 20, "Москва"),
            new Student("Мария", 92, 19, "Санкт-Петербург"),
            new Student("Петр", 78, 21, "Москва"),
            new Student("Анна", 95, 20, "Казань"),
            new Student("Сергей", 88, 22, "Санкт-Петербург")
        };

        // ========== ПРОСТОЙ ЗАПРОС - СИНТАКСИС ЗАПРОСОВ ==========
        Console.WriteLine("\n--- Простой запрос (Query Syntax) ---");
        var querySyntax = from s in students
                          where s.Grade > 80
                          orderby s.Grade descending
                          select new { s.Name, s.Grade };

        Console.WriteLine("Студенты с оценкой > 80:");
        foreach (var student in querySyntax)
        {
            Console.WriteLine($"  {student.Name}: {student.Grade}");
        }

        // ========== ПРОСТОЙ ЗАПРОС - МЕТОДЫ РАСШИРЕНИЯ ==========
        Console.WriteLine("\n--- Простой запрос (Method Syntax) ---");
        var methodSyntax = students
            .Where(s => s.Grade > 80)
            .OrderByDescending(s => s.Grade)
            .Select(s => new { s.Name, s.Grade });

        Console.WriteLine("Студенты с оценкой > 80:");
        foreach (var student in methodSyntax)
        {
            Console.WriteLine($"  {student.Name}: {student.Grade}");
        }

        // ========== ГРУППИРОВКА - СИНТАКСИС ЗАПРОСОВ ==========
        Console.WriteLine("\n--- Группировка по городу (Query Syntax) ---");
        var groupQuery = from s in students
                         group s by s.City into cityGroup
                         select new
                         {
                             City = cityGroup.Key,
                             Count = cityGroup.Count(),
                             AverageGrade = cityGroup.Average(s => s.Grade)
                         };

        foreach (var group in groupQuery)
        {
            Console.WriteLine($"  {group.City}: {group.Count} студентов, средняя оценка: {group.AverageGrade:F1}");
        }

        // ========== ГРУППИРОВКА - МЕТОДЫ РАСШИРЕНИЯ ==========
        Console.WriteLine("\n--- Группировка по городу (Method Syntax) ---");
        var groupMethod = students
            .GroupBy(s => s.City)
            .Select(g => new
            {
                City = g.Key,
                Count = g.Count(),
                AverageGrade = g.Average(s => s.Grade)
            });

        foreach (var group in groupMethod)
        {
            Console.WriteLine($"  {group.City}: {group.Count} студентов, средняя оценка: {group.AverageGrade:F1}");
        }

        // ========== СЛОЖНЫЙ ЗАПРОС С СОРТИРОВКОЙ ==========
        Console.WriteLine("\n--- Сложный запрос с сортировкой ---");

        // Query Syntax
        var complexQuery = from s in students
                           where s.Age >= 20
                           orderby s.City, s.Grade descending
                           select s;

        Console.WriteLine("Студенты >= 20 лет (Query Syntax):");
        foreach (var student in complexQuery)
        {
            Console.WriteLine($"  {student.Name}, {student.Age} лет, {student.City}, оценка: {student.Grade}");
        }

        // Method Syntax
        var complexMethod = students
            .Where(s => s.Age >= 20)
            .OrderBy(s => s.City)
            .ThenByDescending(s => s.Grade);

        Console.WriteLine("\nСтуденты >= 20 лет (Method Syntax):");
        foreach (var student in complexMethod)
        {
            Console.WriteLine($"  {student.Name}, {student.Age} лет, {student.City}, оценка: {student.Grade}");
        }
    }

    static void DemonstrateGroupingAndAggregation()
    {
        Console.WriteLine("\n\n3. ГРУППИРОВКА И АГРЕГАЦИЯ:");

        // Создание коллекции продуктов
        List<Product> products = new List<Product>
        {
            new Product("Ноутбук", "Электроника", 1200, 5),
            new Product("Мышь", "Электроника", 25, 15),
            new Product("Телефон", "Электроника", 800, 12),
            new Product("Стул", "Мебель", 80, 8),
            new Product("Стол", "Мебель", 200, 3),
            new Product("Диван", "Мебель", 500, 2),
            new Product("Книга", "Книги", 15, 30),
            new Product("Журнал", "Книги", 5, 50),
            new Product("Ручка", "Канцелярия", 2, 100)
        };

        // ========== ГРУППИРОВКА ПО КАТЕГОРИЯМ ==========
        Console.WriteLine("\n--- Группировка продуктов по категориям ---");
        var productsByCategory = products.GroupBy(p => p.Category);

        foreach (var group in productsByCategory)
        {
            Console.WriteLine($"\nКатегория: {group.Key}");
            foreach (var product in group)
            {
                Console.WriteLine($"  {product.Name} - ${product.Price} (в наличии: {product.Stock})");
            }
        }

        // ========== АГРЕГАТНЫЕ ФУНКЦИИ ПО КАТЕГОРИЯМ ==========
        Console.WriteLine("\n--- Агрегатные функции по категориям ---");
        var categoryStats = products
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                ProductCount = g.Count(),
                TotalValue = g.Sum(p => p.Price * p.Stock),
                AveragePrice = g.Average(p => p.Price),
                MinPrice = g.Min(p => p.Price),
                MaxPrice = g.Max(p => p.Price),
                TotalStock = g.Sum(p => p.Stock)
            });

        foreach (var stat in categoryStats)
        {
            Console.WriteLine($"\n{stat.Category}:");
            Console.WriteLine($"  Количество товаров: {stat.ProductCount}");
            Console.WriteLine($"  Общая стоимость: ${stat.TotalValue}");
            Console.WriteLine($"  Средняя цена: ${stat.AveragePrice:F2}");
            Console.WriteLine($"  Диапазон цен: ${stat.MinPrice} - ${stat.MaxPrice}");
            Console.WriteLine($"  Общий запас: {stat.TotalStock} шт.");
        }

        // ========== ПОИСК ЭКСТРЕМАЛЬНЫХ ЗНАЧЕНИЙ ==========
        Console.WriteLine("\n--- Самые дорогие и дешевые товары ---");

        var mostExpensive = products.OrderByDescending(p => p.Price).First();
        var cheapest = products.OrderBy(p => p.Price).First();
        var bestStockValue = products.OrderByDescending(p => p.Price * p.Stock).First();

        Console.WriteLine($"Самый дорогой: {mostExpensive.Name} - ${mostExpensive.Price}");
        Console.WriteLine($"Самый дешевый: {cheapest.Name} - ${cheapest.Price}");
        Console.WriteLine($"Наибольшая стоимость запасов: {bestStockValue.Name} - ${bestStockValue.Price * bestStockValue.Stock}");

        // ========== ОБЩАЯ СТАТИСТИКА ==========
        Console.WriteLine("\n--- Общая статистика по всем товарам ---");
        Console.WriteLine($"Общее количество товаров: {products.Count}");
        Console.WriteLine($"Общая стоимость всех товаров: ${products.Sum(p => p.Price * p.Stock)}");
        Console.WriteLine($"Средняя цена товара: ${products.Average(p => p.Price):F2}");
        Console.WriteLine($"Общее количество единиц: {products.Sum(p => p.Stock)}");
    }

    static void DemonstrateJoins()
    {
        Console.WriteLine("\n\n4. СОЕДИНЕНИЯ (JOINS):");

        // Создание коллекций
        List<Customer> customers = new List<Customer>
        {
            new Customer(1, "Иван Иванов", "Москва"),
            new Customer(2, "Петр Петров", "Санкт-Петербург"),
            new Customer(3, "Мария Сидорова", "Москва"),
            new Customer(4, "Анна Козлова", "Казань")
        };

        List<Order> orders = new List<Order>
        {
            new Order(1, 1, 1500, new DateTime(2024, 1, 15)),
            new Order(2, 1, 800, new DateTime(2024, 2, 20)),
            new Order(3, 2, 1200, new DateTime(2024, 1, 10)),
            new Order(4, 3, 500, new DateTime(2024, 3, 5)),
            new Order(5, 3, 900, new DateTime(2024, 3, 15))
        };

        // ========== INNER JOIN ==========
        Console.WriteLine("\n--- Inner Join (заказы с информацией о клиентах) ---");

        // Query Syntax
        var innerJoinQuery = from o in orders
                             join c in customers on o.CustomerId equals c.Id
                             select new { OrderId = o.Id, CustomerName = c.Name, Amount = o.Amount, City = c.City };

        Console.WriteLine("Inner Join (Query Syntax):");
        foreach (var item in innerJoinQuery)
        {
            Console.WriteLine($"  Заказ #{item.OrderId}: {item.CustomerName}, ${item.Amount}, {item.City}");
        }

        // Method Syntax
        var innerJoinMethod = orders.Join(customers,
            o => o.CustomerId,
            c => c.Id,
            (o, c) => new { OrderId = o.Id, CustomerName = c.Name, Amount = o.Amount, City = c.City });

        Console.WriteLine("\nInner Join (Method Syntax):");
        foreach (var item in innerJoinMethod)
        {
            Console.WriteLine($"  Заказ #{item.OrderId}: {item.CustomerName}, ${item.Amount}, {item.City}");
        }

        // ========== GROUP JOIN ==========
        Console.WriteLine("\n--- Group Join (клиенты с их заказами) ---");

        var groupJoin = from c in customers
                        join o in orders on c.Id equals o.CustomerId into customerOrders
                        select new
                        {
                            CustomerName = c.Name,
                            City = c.City,
                            Orders = customerOrders,
                            TotalAmount = customerOrders.Sum(o => o.Amount)
                        };

        foreach (var customer in groupJoin)
        {
            Console.WriteLine($"\n{customer.CustomerName} ({customer.City}):");
            Console.WriteLine($"  Общая сумма заказов: ${customer.TotalAmount}");
            if (customer.Orders.Any())
            {
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($"    Заказ #{order.Id}: ${order.Amount} ({order.OrderDate:dd.MM.yyyy})");
                }
            }
            else
            {
                Console.WriteLine("    Заказов нет");
            }
        }

        // ========== LEFT OUTER JOIN ==========
        Console.WriteLine("\n--- Left Outer Join (все клиенты, даже без заказов) ---");

        var leftOuterJoin = from c in customers
                            join o in orders on c.Id equals o.CustomerId into customerOrders
                            from co in customerOrders.DefaultIfEmpty()
                            select new
                            {
                                CustomerName = c.Name,
                                City = c.City,
                                OrderAmount = co?.Amount ?? 0,
                                HasOrders = customerOrders.Any()
                            };

        foreach (var item in leftOuterJoin.Distinct())
        {
            Console.WriteLine($"  {item.CustomerName} ({item.City}): {(item.HasOrders ? $"заказы на ${item.OrderAmount}" : "нет заказов")}");
        }
    }

    static void DemonstrateSetOperations()
    {
        Console.WriteLine("\n\n5. РАБОТА С МНОЖЕСТВАМИ:");

        // Создание коллекций с перекрывающимися данными
        List<int> collection1 = new List<int> { 1, 2, 3, 4, 5, 6 };
        List<int> collection2 = new List<int> { 4, 5, 6, 7, 8, 9 };
        List<int> collection3 = new List<int> { 2, 4, 6, 8, 10 };

        // ========== DISTINCT ==========
        Console.WriteLine("\n--- Distinct (уникальные элементы) ---");
        List<int> duplicates = new List<int> { 1, 2, 2, 3, 4, 4, 4, 5 };
        var distinctNumbers = duplicates.Distinct();
        Console.WriteLine($"Исходная коллекция: {string.Join(", ", duplicates)}");
        Console.WriteLine($"Уникальные элементы: {string.Join(", ", distinctNumbers)}");

        // ========== UNION ==========
        Console.WriteLine("\n--- Union (объединение множеств) ---");
        var union = collection1.Union(collection2);
        Console.WriteLine($"Коллекция 1: {string.Join(", ", collection1)}");
        Console.WriteLine($"Коллекция 2: {string.Join(", ", collection2)}");
        Console.WriteLine($"Объединение: {string.Join(", ", union)}");

        // ========== INTERSECT ==========
        Console.WriteLine("\n--- Intersect (пересечение множеств) ---");
        var intersect = collection1.Intersect(collection2);
        Console.WriteLine($"Коллекция 1: {string.Join(", ", collection1)}");
        Console.WriteLine($"Коллекция 2: {string.Join(", ", collection2)}");
        Console.WriteLine($"Пересечение: {string.Join(", ", intersect)}");

        // ========== EXCEPT ==========
        Console.WriteLine("\n--- Except (разность множеств) ---");
        var except1 = collection1.Except(collection2);
        var except2 = collection2.Except(collection1);
        Console.WriteLine($"Коллекция 1: {string.Join(", ", collection1)}");
        Console.WriteLine($"Коллекция 2: {string.Join(", ", collection2)}");
        Console.WriteLine($"collection1 \\ collection2: {string.Join(", ", except1)}");
        Console.WriteLine($"collection2 \\ collection1: {string.Join(", ", except2)}");

        // ========== МНОЖЕСТВЕННЫЕ ОПЕРАЦИИ ==========
        Console.WriteLine("\n--- Множественные операции ---");
        var complexSet = collection1.Union(collection2).Intersect(collection3);
        Console.WriteLine($"Коллекция 1: {string.Join(", ", collection1)}");
        Console.WriteLine($"Коллекция 2: {string.Join(", ", collection2)}");
        Console.WriteLine($"Коллекция 3: {string.Join(", ", collection3)}");
        Console.WriteLine($"(collection1 ∪ collection2) ∩ collection3: {string.Join(", ", complexSet)}");

        // ========== РАБОТА С КОМПЛЕКСНЫМИ ОБЪЕКТАМИ ==========
        Console.WriteLine("\n--- Операции с комплексными объектами ---");

        List<Product> products1 = new List<Product>
        {
            new Product("Ноутбук", "Электроника", 1200, 5),
            new Product("Мышь", "Электроника", 25, 15),
            new Product("Стул", "Мебель", 80, 8)
        };

        List<Product> products2 = new List<Product>
        {
            new Product("Мышь", "Электроника", 25, 15),
            new Product("Стол", "Мебель", 200, 3),
            new Product("Книга", "Книги", 15, 30)
        };

        // Distinct по имени продукта
        var allProducts = products1.Union(products2);
        Console.WriteLine("Все уникальные продукты:");
        foreach (var product in allProducts)
        {
            Console.WriteLine($"  {product.Name} - ${product.Price}");
        }
    }

    static void DemonstrateAdvancedTechniques()
    {
        Console.WriteLine("\n\n6. ПРОДВИНУТЫЕ ТЕХНИКИ LINQ:");

        List<Student> students = new List<Student>
        {
            new Student("Иван", 85, 20, "Москва"),
            new Student("Мария", 92, 19, "Санкт-Петербург"),
            new Student("Петр", 78, 21, "Москва"),
            new Student("Анна", 95, 20, "Казань"),
            new Student("Сергей", 88, 22, "Санкт-Петербург"),
            new Student("Ольга", 65, 19, "Москва"),
            new Student("Дмитрий", 72, 20, "Казань"),
            new Student("Елена", 89, 21, "Москва")
        };

        // ========== LET ДЛЯ ПРОМЕЖУТОЧНЫХ РЕЗУЛЬТАТОВ ==========
        Console.WriteLine("\n--- Использование let для промежуточных результатов ---");

        var studentsWithStatus = from s in students
                                 let status = s.Grade >= 90 ? "Отличник" : s.Grade >= 80 ? "Хорошист" : "Удовлетворительно"
                                 let canGraduate = s.Grade >= 75 && s.Age >= 20
                                 select new
                                 {
                                     s.Name,
                                     s.Grade,
                                     Status = status,
                                     CanGraduate = canGraduate ? "Да" : "Нет"
                                 };

        foreach (var student in studentsWithStatus)
        {
            Console.WriteLine($"  {student.Name}: {student.Grade} баллов - {student.Status}, Может выпускаться: {student.CanGraduate}");
        }

        // ========== PAGINATION С TAKE И SKIP ==========
        Console.WriteLine("\n--- Постраничный вывод (пагинация) ---");

        int pageSize = 3;
        int pageNumber = 1;

        var page1 = students.OrderBy(s => s.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        Console.WriteLine($"Страница {pageNumber}:");
        foreach (var student in page1)
        {
            Console.WriteLine($"  {student.Name}");
        }

        pageNumber = 2;
        var page2 = students.OrderBy(s => s.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        Console.WriteLine($"\nСтраница {pageNumber}:");
        foreach (var student in page2)
        {
            Console.WriteLine($"  {student.Name}");
        }

        // ========== TAKEWHILE И SKIPWHILE ==========
        Console.WriteLine("\n--- TakeWhile и SkipWhile ---");

        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };

        var takeWhile = numbers.TakeWhile(n => n < 4);
        var skipWhile = numbers.SkipWhile(n => n < 4);

        Console.WriteLine($"Исходная коллекция: {string.Join(", ", numbers)}");
        Console.WriteLine($"TakeWhile(n < 4): {string.Join(", ", takeWhile)}");
        Console.WriteLine($"SkipWhile(n < 4): {string.Join(", ", skipWhile)}");

        // ========== ZIP ДЛЯ ПОПАРНОГО СОЕДИНЕНИЯ ==========
        Console.WriteLine("\n--- Zip для попарного соединения ---");

        List<string> names = new List<string> { "Иван", "Мария", "Петр" };
        List<int> ages = new List<int> { 20, 19, 21 };
        List<string> cities = new List<string> { "Москва", "СПб", "Казань" };

        var zipped = names.Zip(ages, (name, age) => $"{name} ({age} лет)");
        Console.WriteLine("Zip names + ages: " + string.Join(", ", zipped));

        var tripleZip = names.Zip(ages, (name, age) => (Name: name, Age: age))
                            .Zip(cities, (person, city) => $"{person.Name}, {person.Age} лет, {city}");
        Console.WriteLine("Triple zip: " + string.Join("; ", tripleZip));

        // ========== SELECTMANY ДЛЯ РАБОТЫ С ВЛОЖЕННЫМИ КОЛЛЕКЦИЯМИ ==========
        Console.WriteLine("\n--- SelectMany для вложенных коллекций ---");

        List<Classroom> classrooms = new List<Classroom>
        {
            new Classroom("10A", new List<string> { "Математика", "Физика", "Химия" }),
            new Classroom("10B", new List<string> { "Литература", "История", "Биология" }),
            new Classroom("11A", new List<string> { "Информатика", "Математика", "Английский" })
        };

        var allSubjects = classrooms.SelectMany(c => c.Subjects).Distinct();
        Console.WriteLine("Все предметы: " + string.Join(", ", allSubjects));

        var classroomSubjects = classrooms.SelectMany(c =>
            c.Subjects.Select(s => new { Classroom = c.Name, Subject = s }));

        Console.WriteLine("Классы и предметы:");
        foreach (var item in classroomSubjects)
        {
            Console.WriteLine($"  {item.Classroom}: {item.Subject}");
        }

        // ========== ОПТИМИЗАЦИЯ ДЛЯ БОЛЬШИХ КОЛЛЕКЦИЙ ==========
        Console.WriteLine("\n--- Оптимизация запросов ---");

        // Плохой подход - многократное выполнение запросов
        var badApproach = students.Where(s => s.City == "Москва");
        int badCount = badApproach.Count();
        var badList = badApproach.ToList(); // Запрос выполняется второй раз!

        // Хороший подход - материализация результата
        var goodApproach = students.Where(s => s.City == "Москва").ToList();
        int goodCount = goodApproach.Count;
        var goodList = goodApproach; // Работаем с материализованным списком

        Console.WriteLine($"Московских студентов: {goodCount}");
        Console.WriteLine("Оптимизированный подход использует ToList() для материализации");
    }
}

// Вспомогательный класс для демонстрации SelectMany
public class Classroom
{
    public string Name { get; set; }
    public List<string> Subjects { get; set; }

    public Classroom(string name, List<string> subjects)
    {
        Name = name;
        Subjects = subjects;
    }
}

//1.LINQ(Language Integrated Query)
//Технология интеграции возможностей запросов непосредственно в язык C#. Позволяет выполнять запросы к различным источникам данных с единым синтаксисом.

//2. СИНТАКСИС LINQ
//Синтаксис запросов (Query Syntax) - похож на SQL

//Методы расширения (Method Syntax) - цепочка методов

//3. ОСНОВНЫЕ ОПЕРАЦИИ LINQ
//Фильтрация - Where - отбор элементов по условию

//Проекция - Select - преобразование элементов

//Сортировка - OrderBy, OrderByDescending - упорядочивание

//Группировка - GroupBy - объединение в группы

//Агрегация - Sum, Average, Min, Max, Count - вычисления

//4. ВЫПОЛНЕНИЕ ЗАПРОСОВ
//Отложенное выполнение - запрос выполняется при перечислении результатов

//Немедленное выполнение - запрос выполняется сразу (ToList, ToArray, Count)

//5. СОЕДИНЕНИЯ (JOINS)
//Inner Join - только совпадающие записи из обеих коллекций

//Group Join - группировка связанных записей

//Left Outer Join - все записи из левой коллекции

//6. ОПЕРАЦИИ НАД МНОЖЕСТВАМИ
//Distinct - уникальные элементы

//Union - объединение множеств

//Intersect - пересечение множеств

//Except - разность множеств

//7. ПОСТРАНИЧНЫЙ ВЫВОД (PAGINATION)
//Skip - пропуск элементов

//Take - взятие элементов

//TakeWhile/SkipWhile - условное взятие/пропуск

//8. РАБОТА С ВЛОЖЕННЫМИ КОЛЛЕКЦИЯМИ
//SelectMany - "разворачивание" вложенных коллекций в плоскую структуру

//9. ОПТИМИЗАЦИЯ ЗАПРОСОВ
//Материализация - сохранение результатов (ToList, ToArray)

//Избегание повторных вычислений - кэширование результатов

//10. ПРОМЕЖУТОЧНЫЕ РЕЗУЛЬТАТЫ
//Let - создание временных переменных в запросах

//Into - сохранение результатов группировки

//11. ПОЛУЧЕНИЕ ЭЛЕМЕНТОВ
//First/FirstOrDefault - первый элемент

//Last/LastOrDefault - последний элемент

//Single/SingleOrDefault - единственный элемент

//12. АГРЕГАТНЫЕ ФУНКЦИИ
//Суммирование - Sum

//Среднее значение - Average

//Минимальное значение - Min

//Максимальное значение - Max

//Количество - Count

//13. ПРОВЕРКИ УСЛОВИЙ
//Any - проверка наличия элементов

//All - проверка выполнения условия для всех элементов

//Contains - проверка наличия элемента

//14. ПОПАРНОЕ СОЕДИНЕНИЕ
//Zip - соединение элементов из нескольких коллекций по позициям

//15. ИСТОЧНИКИ ДАННЫХ LINQ
//LINQ to Objects - работа с коллекциями в памяти

//LINQ to SQL - работа с базами данных

//LINQ to XML - работа с XML документами

//LINQ to Entities - работа с Entity Framework

//LINQ унифицирует работу с данными из различных источников и делает код более читаемым и выразительным!