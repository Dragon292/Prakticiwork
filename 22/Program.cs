using System;
using System.IO;
using System.Collections.Generic;

// Задание 3: Пользовательское исключение
public class AgeException : Exception
{
    public int InvalidAge { get; }

    public AgeException(string message, int invalidAge) : base(message)
    {
        InvalidAge = invalidAge;
    }
}

// Задание 5: Пользовательские исключения для валидации
public class InvalidEmailException : Exception
{
    public InvalidEmailException(string message) : base(message) { }
}

public class WeakPasswordException : Exception
{
    public WeakPasswordException(string message) : base(message) { }
}

// Задание 6: Исключения для банковского счета
public class InsufficientFundsException : Exception
{
    public decimal CurrentBalance { get; }
    public decimal RequestedAmount { get; }

    public InsufficientFundsException(decimal balance, decimal amount)
        : base($"Недостаточно средств. Баланс: {balance}, запрошено: {amount}")
    {
        CurrentBalance = balance;
        RequestedAmount = amount;
    }
}

public class InvalidAmountException : Exception
{
    public InvalidAmountException(string message) : base(message) { }
}

// Задание 5: Класс валидации
public class UserValidator
{
    public void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@") || !email.Contains("."))
        {
            throw new InvalidEmailException($"Некорректный email: {email}");
        }
    }

    public void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            throw new WeakPasswordException("Пароль должен содержать минимум 6 символов");
        }
    }

    public void ValidateAge(int age)
    {
        if (age < 0 || age > 150)
        {
            throw new AgeException($"Некорректный возраст: {age}", age);
        }
    }
}

// Задание 6: Банковский счет
public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountNumber, decimal initialBalance = 0)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException($"Сумма депозита должна быть положительной: {amount}");
        }

        Balance += amount;
        Console.WriteLine($"Зачислено: {amount}. Новый баланс: {Balance}");
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException($"Сумма снятия должна быть положительной: {amount}");
        }

        if (amount > Balance)
        {
            throw new InsufficientFundsException(Balance, amount);
        }

        Balance -= amount;
        Console.WriteLine($"Снято: {amount}. Новый баланс: {Balance}");
    }

    public void Transfer(BankAccount targetAccount, decimal amount)
    {
        try
        {
            Console.WriteLine($"Перевод {amount} со счета {AccountNumber} на счет {targetAccount.AccountNumber}");
            Withdraw(amount);
            targetAccount.Deposit(amount);
            Console.WriteLine("Перевод выполнен успешно");
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка перевода: {ex.Message}", ex);
        }
    }
}

// Задание 7: Калькулятор
public class Calculator
{
    public decimal Calculate(decimal num1, decimal num2, string operation)
    {
        return operation.ToLower() switch
        {
            "+" => Add(num1, num2),
            "-" => Subtract(num1, num2),
            "*" => Multiply(num1, num2),
            "/" => Divide(num1, num2),
            _ => throw new InvalidOperationException($"Неизвестная операция: {operation}")
        };
    }

    private decimal Add(decimal a, decimal b) => a + b;
    private decimal Subtract(decimal a, decimal b) => a - b;
    private decimal Multiply(decimal a, decimal b) => a * b;

    private decimal Divide(decimal a, decimal b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Деление на ноль невозможно");
        }
        return a / b;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ОБРАБОТКИ ИСКЛЮЧЕНИЙ ===\n");

        // Задание 1: Базовые исключения
        DemonstrateBasicExceptions();

        // Задание 2: Работа с файлами
        DemonstrateFileExceptions();

        // Задание 3: Пользовательские исключения
        DemonstrateCustomExceptions();

        // Задание 4: Вложенные исключения
        DemonstrateNestedExceptions();

        // Задание 5: Система валидации
        DemonstrateValidationSystem();

        // Задание 6: Банковский счет
        DemonstrateBankAccount();

        // Задание 7: Калькулятор
        DemonstrateCalculator();

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
    }

    static void DemonstrateBasicExceptions()
    {
        Console.WriteLine("1. БАЗОВЫЕ ИСКЛЮЧЕНИЯ (деление чисел):");

        try
        {
            Console.Write("Введите первое число: ");
            string input1 = Console.ReadLine();

            Console.Write("Введите второе число: ");
            string input2 = Console.ReadLine();

            decimal num1 = decimal.Parse(input1);
            decimal num2 = decimal.Parse(input2);

            decimal result = num1 / num2;
            Console.WriteLine($"Результат: {num1} / {num2} = {result}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Введены некорректные числа!");
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Ошибка: Деление на ноль невозможно!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Блок finally: Проверка деления завершена\n");
        }
    }

    static void DemonstrateFileExceptions()
    {
        Console.WriteLine("2. РАБОТА С ФАЙЛАМИ:");

        // Способ 1: try-catch-finally
        string filename = "test.txt";
        StreamReader reader = null;

        try
        {
            Console.Write("Введите имя файла для чтения: ");
            filename = Console.ReadLine();

            reader = new StreamReader(filename);
            string content = reader.ReadToEnd();
            Console.WriteLine($"Содержимое файла: {content}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Ошибка: Файл {filename} не найден!");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
        }
        finally
        {
            reader?.Close();
            Console.WriteLine("Блок finally: Файл закрыт, ресурсы освобождены");
        }

        // Способ 2: using statement
        Console.WriteLine("\nСпособ 2 (using):");
        try
        {
            using (var reader2 = new StreamReader(filename))
            {
                string content = reader2.ReadToEnd();
                Console.WriteLine($"Содержимое файла: {content}");
            } // Автоматический вызов Dispose()
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Файл {filename} не найден (using)");
        }
        Console.WriteLine();
    }

    static void DemonstrateCustomExceptions()
    {
        Console.WriteLine("3. ПОЛЬЗОВАТЕЛЬСКИЕ ИСКЛЮЧЕНИЯ:");

        try
        {
            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());

            if (age < 0 || age > 150)
            {
                throw new AgeException("Возраст должен быть от 0 до 150 лет", age);
            }
            Console.WriteLine($"Возраст {age} корректен");
        }
        catch (AgeException ex)
        {
            Console.WriteLine($"AgeException: {ex.Message}");
            Console.WriteLine($"Некорректное значение: {ex.InvalidAge}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Введен некорректный формат возраста");
        }
        Console.WriteLine();
    }

    static void DemonstrateNestedExceptions()
    {
        Console.WriteLine("4. ВЛОЖЕННЫЕ ИСКЛЮЧЕНИЯ:");

        try // Внешний блок
        {
            int[] numbers = { 1, 2, 3 };
            string text = null;

            try // Внутренний блок
            {
                Console.WriteLine($"Элемент массива: {numbers[5]}"); // IndexOutOfRangeException
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Внутренний catch: Выход за границы массива");
                // Пробрасываем исключение дальше
                throw;
            }

            try // Другой внутренний блок
            {
                Console.WriteLine($"Длина строки: {text.Length}"); // NullReferenceException
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Внутренний catch: Ссылка на null объект");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Внешний catch: Перехвачено исключение - {ex.GetType().Name}");
        }
        Console.WriteLine();
    }

    static void DemonstrateValidationSystem()
    {
        Console.WriteLine("5. СИСТЕМА ВАЛИДАЦИИ:");

        UserValidator validator = new UserValidator();

        try
        {
            Console.Write("Введите email: ");
            string email = Console.ReadLine();
            validator.ValidateEmail(email);

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            validator.ValidatePassword(password);

            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());
            validator.ValidateAge(age);

            Console.WriteLine("✅ Все данные валидны!");
        }
        catch (InvalidEmailException ex)
        {
            Console.WriteLine($"Ошибка email: {ex.Message}");
        }
        catch (WeakPasswordException ex)
        {
            Console.WriteLine($"Ошибка пароля: {ex.Message}");
        }
        catch (AgeException ex)
        {
            Console.WriteLine($"Ошибка возраста: {ex.Message}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат возраста");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка валидации: {ex.Message}");
        }
        Console.WriteLine();
    }

    static void DemonstrateBankAccount()
    {
        Console.WriteLine("6. БАНКОВСКИЙ СЧЕТ:");

        BankAccount account1 = new BankAccount("12345", 1000);
        BankAccount account2 = new BankAccount("67890", 500);

        try
        {
            account1.Deposit(500);
            account1.Withdraw(200);
            account1.Transfer(account2, 300);
            account1.Withdraw(2000); // Вызовет InsufficientFundsException
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Ошибка снятия: {ex.Message}");
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"Ошибка суммы: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Банковская ошибка: {ex.Message}");
        }
        Console.WriteLine();
    }

    static void DemonstrateCalculator()
    {
        Console.WriteLine("7. КАЛЬКУЛЯТОР:");

        Calculator calculator = new Calculator();
        bool continueCalculations = true;

        while (continueCalculations)
        {
            try
            {
                Console.Write("Введите первое число: ");
                decimal num1 = decimal.Parse(Console.ReadLine());

                Console.Write("Введите второе число: ");
                decimal num2 = decimal.Parse(Console.ReadLine());

                Console.Write("Введите операцию (+, -, *, /): ");
                string operation = Console.ReadLine();

                decimal result = calculator.Calculate(num1, num2, operation);
                Console.WriteLine($"Результат: {num1} {operation} {num2} = {result}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введены некорректные числа!");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Ошибка: Деление на ноль невозможно!");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка: Переполнение при вычислении!");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка операции: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Расчет завершен");
            }

            Console.Write("Продолжить вычисления? (y/n): ");
            continueCalculations = Console.ReadLine().ToLower() == "y";
            Console.WriteLine();
        }
    }
}

//1.ИСКЛЮЧЕНИЕ(Exception)
//Ошибка времени выполнения, прерывающая нормальный поток выполнения программы. Объект, содержащий информацию об ошибке.

//2. TRY-BLOCK
//Блок кода, в котором может возникнуть исключение. Помечает область для мониторинга ошибок.

//3. CATCH-BLOCK
//Блок обработки исключений. Перехватывает и обрабатывает исключения определенного типа.

//4. FINALLY-BLOCK
//Блок, выполняющийся всегда, независимо от возникновения исключения. Для освобождения ресурсов.

//5. STANDARD EXCEPTIONS
//Встроенные исключения .NET для распространенных ошибок:

//FormatException - неверный формат данных

//DivideByZeroException - деление на ноль

//FileNotFoundException - файл не найден

//IOException - ошибки ввода-вывода

//NullReferenceException - обращение к null объекту

//IndexOutOfRangeException - выход за границы массива

//OverflowException - переполнение значения

//InvalidOperationException - недопустимая операция

//6. CUSTOM EXCEPTIONS
//Пользовательские классы исключений для специфичных ошибок приложения. Наследуются от Exception.

//7. EXCEPTION PROPAGATION
//Механизм передачи исключения вверх по стеку вызовов до нахождения обработчика.

//8. THROW STATEMENT
//Оператор для генерации исключения. Может создавать новые или перебрасывать существующие.

//9. USING STATEMENT
//Синтаксическая конструкция для автоматического управления ресурсами. Гарантирует вызов Dispose().

//10. EXCEPTION HANDLING STRATEGIES
//Принципы обработки ошибок:

//Specific to general - от конкретных к общим исключениям

//Fail fast - быстрое прерывание при ошибках

//Graceful degradation - плавное снижение функциональности

//11. INNER EXCEPTION
//Вложенное исключение для сохранения оригинальной ошибки при создании новой.

//12. STACK TRACE
//Информация о пути выполнения, приведшем к исключению. Для диагностики.

//13. RESOURCE MANAGEMENT
//Правильное управление ресурсами (файлы, сетевые соединения) с использованием finally или using.

//14.VALIDATION
//Проверка входных данных и условий до выполнения операций. Предотвращение исключений.

//15. BUSINESS LOGIC EXCEPTIONS
//Специализированные исключения для нарушений бизнес-правил приложения.

//16. EXCEPTION SAFETY
//Гарантия сохранения консистентности состояния программы при возникновении исключений.

//17. GLOBAL EXCEPTION HANDLER
//Обработчик неперехваченных исключений на уровне приложения.

//Обработка исключений - это стратегия создания надежных и отказоустойчивых приложений!