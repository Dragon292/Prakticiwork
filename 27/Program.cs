using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

// Задание 1: Основы async/await

/// <summary>
/// Класс для демонстрации базовых асинхронных операций
/// Показывает разницу между синхронным и асинхронным выполнением
/// </summary>
public class AsyncBasics
{
    /// <summary>
    /// Синхронный метод - блокирует вызывающий поток на время выполнения
    /// </summary>
    public void SynchronousOperation(string operationName, int delayMs)
    {
        Console.WriteLine($"[Sync] Начало операции '{operationName}' в потоке {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(delayMs); // Блокирующая задержка
        Console.WriteLine($"[Sync] Завершение операции '{operationName}'");
    }

    /// <summary>
    /// Асинхронный метод - не блокирует вызывающий поток
    /// async/await позволяет освободить поток во время ожидания
    /// </summary>
    public async Task AsynchronousOperation(string operationName, int delayMs)
    {
        Console.WriteLine($"[Async] Начало операции '{operationName}' в потоке {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(delayMs); // Неблокирующая задержка
        Console.WriteLine($"[Async] Завершение операции '{operationName}' в потоке {Thread.CurrentThread.ManagedThreadId}");
    }

    /// <summary>
    /// Асинхронный метод с возвратом значения
    /// Task<T> представляет асинхронную операцию, возвращающую результат типа T
    /// </summary>
    public async Task<int> AsynchronousOperationWithResult(string operationName, int delayMs)
    {
        Console.WriteLine($"[Async-Result] Начало операции '{operationName}'");
        await Task.Delay(delayMs);
        int result = delayMs * 2;
        Console.WriteLine($"[Async-Result] Завершение операции '{operationName}' с результатом {result}");
        return result;
    }

    /// <summary>
    /// Асинхронный метод, который не использует await - выполняется синхронно
    /// Компилятор выдаст предупреждение, но метод корректен
    /// </summary>
    public async Task AsyncMethodWithoutAwait()
    {
        Console.WriteLine("Этот метод помечен async, но не содержит await");
        // Выполняется синхронно, несмотря на async
    }
}

// Задание 2: Параллельное выполнение задач

/// <summary>
/// Класс для демонстрации параллельного выполнения асинхронных операций
/// Показывает разницу между последовательным и параллельным выполнением
/// </summary>
public class ParallelExecution
{
    /// <summary>
    /// Последовательное выполнение асинхронных операций
    /// Каждая операция ожидает завершения предыдущей
    /// </summary>
    public async Task SequentialExecutionAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine("=== ПОСЛЕДОВАТЕЛЬНОЕ ВЫПОЛНЕНИЕ ===");

        await SimulateWorkAsync("Операция 1", 1000);
        await SimulateWorkAsync("Операция 2", 1000);
        await SimulateWorkAsync("Операция 3", 1000);

        stopwatch.Stop();
        Console.WriteLine($"Общее время: {stopwatch.ElapsedMilliseconds}ms\n");
    }

    /// <summary>
    /// Параллельное выполнение асинхронных операций с Task.WhenAll
    /// Все операции запускаются одновременно и ожидаются вместе
    /// </summary>
    public async Task ParallelExecutionWithWhenAllAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine("=== ПАРАЛЛЕЛЬНОЕ ВЫПОЛНЕНИЕ (WhenAll) ===");

        var task1 = SimulateWorkAsync("Операция 1", 1000);
        var task2 = SimulateWorkAsync("Операция 2", 1000);
        var task3 = SimulateWorkAsync("Операция 3", 1000);

        // Ожидаем завершения всех задач одновременно
        await Task.WhenAll(task1, task2, task3);

        stopwatch.Stop();
        Console.WriteLine($"Общее время: {stopwatch.ElapsedMilliseconds}ms\n");
    }

    /// <summary>
    /// Ожидание первой завершенной задачи с Task.WhenAny
    /// Полезно для реализации таймаутов или выбора самого быстрого источника данных
    /// </summary>
    public async Task WaitForFirstCompletionAsync()
    {
        Console.WriteLine("=== ОЖИДАНИЕ ПЕРВОЙ ЗАВЕРШЕННОЙ ЗАДАЧИ (WhenAny) ===");

        var tasks = new[]
        {
            SimulateWorkAsync("Быстрая операция", 500),
            SimulateWorkAsync("Медленная операция", 2000),
            SimulateWorkAsync("Средняя операция", 1000)
        };

        // Ожидаем завершения первой задачи
        Task<string> firstCompletedTask = await Task.WhenAny(tasks);
        string result = await firstCompletedTask;

        Console.WriteLine($"Первой завершилась: {result}");

        // Можно продолжить ожидание остальных задач если нужно
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Обработка результатов параллельно выполненных задач
    /// </summary>
    public async Task ProcessResultsAsync()
    {
        Console.WriteLine("=== ОБРАБОТКА РЕЗУЛЬТАТОВ ПАРАЛЛЕЛЬНЫХ ОПЕРАЦИЙ ===");

        var tasks = new[]
        {
            CalculateAsync(10),
            CalculateAsync(20),
            CalculateAsync(30)
        };

        // Когда все задачи завершены, получаем результаты
        int[] results = await Task.WhenAll(tasks);

        Console.WriteLine($"Результаты: {string.Join(", ", results)}");
        Console.WriteLine($"Сумма: {results.Sum()}");
    }

    private async Task<string> SimulateWorkAsync(string operationName, int delayMs)
    {
        Console.WriteLine($"[{operationName}] Запуск в потоке {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(delayMs);
        Console.WriteLine($"[{operationName}] Завершено");
        return $"{operationName} завершена за {delayMs}ms";
    }

    private async Task<int> CalculateAsync(int value)
    {
        await Task.Delay(500); // Имитация вычислений
        return value * value;
    }
}

// Задание 3: Отмена асинхронных операций

/// <summary>
/// Класс для демонстрации отмены асинхронных операций
/// Использует CancellationTokenSource и CancellationToken для кооперативной отмены
/// </summary>
public class CancellationDemo
{
    /// <summary>
    /// Длительная операция с поддержкой отмены
    /// </summary>
    public async Task LongRunningOperationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Console.WriteLine("Длительная операция начата");

            for (int i = 1; i <= 10; i++)
            {
                // Проверяем запрос на отмену перед каждой итерацией
                cancellationToken.ThrowIfCancellationRequested();

                Console.WriteLine($"Шаг {i}/10");
                await Task.Delay(500, cancellationToken); // Передаем токен в Task.Delay
            }

            Console.WriteLine("Длительная операция успешно завершена");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция была отменена");
            throw; // Пробрасываем исключение дальше
        }
    }

    /// <summary>
    /// Демонстрация отмены по требованию пользователя
    /// </summary>
    public async Task DemonstrateCancellationAsync()
    {
        using var cts = new CancellationTokenSource();

        // Запускаем длительную операцию в фоне
        var longRunningTask = LongRunningOperationAsync(cts.Token);

        // Имитируем пользовательский ввод через 2 секунды
        _ = Task.Delay(2000).ContinueWith(_ =>
        {
            Console.WriteLine("Пользователь запросил отмену...");
            cts.Cancel();
        });

        try
        {
            await longRunningTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция была отменена пользователем");
        }
    }

    /// <summary>
    /// Демонстрация отмены по таймауту
    /// </summary>
    public async Task DemonstrateTimeoutAsync()
    {
        // Создаем CancellationTokenSource с таймаутом 3 секунды
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        try
        {
            Console.WriteLine("Запуск операции с таймаутом 3 секунды...");
            await LongRunningOperationAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция отменена по таймауту");
        }
    }

    /// <summary>
    /// Операция с возможностью отмены через ссылку на CancellationToken
    /// </summary>
    public async Task CancelableOperationWithLinkedTokenAsync(
        CancellationToken externalToken,
        TimeSpan timeout)
    {
        // Создаем связанный токен, который отменяется либо по внешнему токену, либо по таймауту
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            externalToken,
            new CancellationTokenSource(timeout).Token
        );

        await LongRunningOperationAsync(linkedCts.Token);
    }
}

// Задание 4: Асинхронные события и потоки

/// <summary>
/// Класс с асинхронными событиями
/// async void используется только для обработчиков событий
/// </summary>
public class AsyncEventPublisher
{
    /// <summary>
    /// Событие с асинхронными обработчиками
    /// </summary>
    public event Func<string, Task>? DataReceived;

    /// <summary>
    /// Асинхронный метод для вызова события
    /// </summary>
    public async Task PublishDataAsync(string data)
    {
        Console.WriteLine($"Публикация данных: {data}");

        if (DataReceived != null)
        {
            // Вызываем все обработчики асинхронно и ожидаем их завершения
            await Task.WhenAll(DataReceived.GetInvocationList()
                .Cast<Func<string, Task>>()
                .Select(handler => handler(data)));
        }
    }
}

/// <summary>
/// Класс для работы с асинхронными потоками (IAsyncEnumerable)
/// </summary>
public class AsyncStreamsDemo
{
    /// <summary>
    /// Генерация асинхронной последовательности данных
    /// IAsyncEnumerable позволяет yield return в асинхронном контексте
    /// </summary>
    public async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            await Task.Delay(200); // Асинхронная операция между элементами
            yield return i;
        }
    }

    /// <summary>
    /// Асинхронная последовательность с поддержкой отмены
    /// </summary>
    public async IAsyncEnumerable<string> GenerateDataWithCancellationAsync(
        int count,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int i = 1; i <= count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(300, cancellationToken);
            yield return $"Элемент {i}";
        }
    }

    /// <summary>
    /// Демонстрация использования await foreach для обработки асинхронных последовательностей
    /// </summary>
    public async Task ProcessAsyncStreamAsync()
    {
        Console.WriteLine("=== ОБРАБОТКА АСИНХРОННОГО ПОТОКА ===");

        await foreach (var number in GenerateNumbersAsync(5))
        {
            Console.WriteLine($"Получено: {number}");
        }
    }

    /// <summary>
    /// Демонстрация ConfigureAwait для управления контекстом синхронизации
    /// </summary>
    public async Task DemonstrateConfigureAwaitAsync()
    {
        Console.WriteLine($"Начало в потоке {Thread.CurrentThread.ManagedThreadId}");

        // ConfigureAwait(false) - не восстанавливать контекст синхронизации
        await Task.Delay(100).ConfigureAwait(false);

        Console.WriteLine($"После await в потоке {Thread.CurrentThread.ManagedThreadId}");

        // Без ConfigureAwait(false) обычно возвращаемся в исходный контекст
        await Task.Delay(100);

        Console.WriteLine($"В конце в потоке {Thread.CurrentThread.ManagedThreadId}");
    }
}

// Задание 5: Параллельные вычисления с Task Parallel Library

/// <summary>
/// Класс для демонстрации CPU-bound операций и параллельных вычислений
/// </summary>
public class ParallelComputations
{
    /// <summary>
    /// Вычисление числа Фибоначчи (CPU-bound операция)
    /// </summary>
    public long Fibonacci(int n)
    {
        if (n <= 1) return n;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    /// <summary>
    /// Выполнение CPU-bound операции в thread pool с помощью Task.Run
    /// </summary>
    public async Task<long> CalculateFibonacciAsync(int n)
    {
        Console.WriteLine($"Запуск вычисления Fibonacci({n}) в потоке {Thread.CurrentThread.ManagedThreadId}");

        // Task.Run выгружает вычисление в thread pool
        return await Task.Run(() => Fibonacci(n));
    }

    /// <summary>
    /// Последовательное выполнение CPU-bound операций
    /// </summary>
    public async Task SequentialCpuBoundAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine("=== ПОСЛЕДОВАТЕЛЬНЫЕ CPU-BOUND ВЫЧИСЛЕНИЯ ===");

        var result1 = await CalculateFibonacciAsync(35);
        var result2 = await CalculateFibonacciAsync(36);
        var result3 = await CalculateFibonacciAsync(37);

        Console.WriteLine($"Результаты: {result1}, {result2}, {result3}");
        Console.WriteLine($"Время: {stopwatch.ElapsedMilliseconds}ms\n");
    }

    /// <summary>
    /// Параллельное выполнение CPU-bound операций
    /// </summary>
    public async Task ParallelCpuBoundAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine("=== ПАРАЛЛЕЛЬНЫЕ CPU-BOUND ВЫЧИСЛЕНИЯ ===");

        var task1 = CalculateFibonacciAsync(35);
        var task2 = CalculateFibonacciAsync(36);
        var task3 = CalculateFibonacciAsync(37);

        var results = await Task.WhenAll(task1, task2, task3);

        Console.WriteLine($"Результаты: {string.Join(", ", results)}");
        Console.WriteLine($"Время: {stopwatch.ElapsedMilliseconds}ms\n");
    }

    /// <summary>
    /// Использование Parallel.For для параллельной обработки данных
    /// </summary>
    public void ParallelForDemo()
    {
        Console.WriteLine("=== PARALLEL.FOR ДЛЯ ОБРАБОТКИ КОЛЛЕКЦИЙ ===");

        var numbers = Enumerable.Range(1, 20).ToArray();
        var results = new long[numbers.Length];

        var stopwatch = Stopwatch.StartNew();

        Parallel.For(0, numbers.Length, i =>
        {
            results[i] = Fibonacci(numbers[i]);
            Console.WriteLine($"Обработан элемент {numbers[i]} в потоке {Thread.CurrentThread.ManagedThreadId}");
        });

        stopwatch.Stop();

        Console.WriteLine($"Обработано {numbers.Length} элементов за {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Результаты: {string.Join(", ", results.Take(10))}...\n");
    }

    /// <summary>
    /// Использование Parallel.ForEach для параллельной обработки коллекций
    /// </summary>
    public void ParallelForEachDemo()
    {
        Console.WriteLine("=== PARALLEL.FOREACH ДЛЯ ОБРАБОТКИ КОЛЛЕКЦИЙ ===");

        var data = new[] { "item1", "item2", "item3", "item4", "item5" };
        var results = new List<string>();
        var lockObject = new object(); // Для потокобезопасного доступа к коллекции

        Parallel.ForEach(data, item =>
        {
            // Имитация обработки
            Thread.Sleep(200);
            var processedItem = $"{item}_processed";

            lock (lockObject) // Синхронизация доступа к коллекции
            {
                results.Add(processedItem);
            }

            Console.WriteLine($"Обработан {item} в потоке {Thread.CurrentThread.ManagedThreadId}");
        });

        Console.WriteLine($"Результаты: {string.Join(", ", results)}\n");
    }

    /// <summary>
    /// Демонстрация AsyncLocal для хранения контекста выполнения
    /// </summary>
    public class AsyncContextDemo
    {
        private static readonly AsyncLocal<string> _asyncLocal = new AsyncLocal<string>();
        private static readonly ThreadLocal<string> _threadLocal = new ThreadLocal<string>();

        public async Task DemonstrateAsyncLocal()
        {
            _asyncLocal.Value = "Начальное значение AsyncLocal";
            _threadLocal.Value = "Начальное значение ThreadLocal";

            Console.WriteLine($"До await - AsyncLocal: {_asyncLocal.Value}, ThreadLocal: {_threadLocal.Value}, Поток: {Thread.CurrentThread.ManagedThreadId}");

            await Task.Delay(100).ConfigureAwait(false);

            Console.WriteLine($"После await - AsyncLocal: {_asyncLocal.Value}, ThreadLocal: {_threadLocal.Value}, Поток: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ASYNC/AWAIT И TPL ===\n");

        // Задание 1: Основы async/await
        await DemonstrateAsyncBasics();

        // Задание 2: Параллельное выполнение задач
        await DemonstrateParallelExecution();

        // Задание 3: Отмена асинхронных операций
        await DemonstrateCancellation();

        // Задание 4: Асинхронные события и потоки
        await DemonstrateAsyncEventsAndStreams();

        // Задание 5: Параллельные вычисления с TPL
        await DemonstrateParallelComputations();

        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
    }

    static async Task DemonstrateAsyncBasics()
    {
        Console.WriteLine("1. ОСНОВЫ ASYNC/AWAIT\n");

        var basics = new AsyncBasics();

        // Демонстрация синхронного выполнения
        Console.WriteLine("--- Синхронное выполнение ---");
        basics.SynchronousOperation("Синхронная операция", 1000);

        // Демонстрация асинхронного выполнения
        Console.WriteLine("\n--- Асинхронное выполнение ---");
        await basics.AsynchronousOperation("Асинхронная операция", 1000);

        // Демонстрация асинхронной операции с результатом
        Console.WriteLine("\n--- Асинхронная операция с результатом ---");
        int result = await basics.AsynchronousOperationWithResult("Операция с результатом", 500);
        Console.WriteLine($"Получен результат: {result}");
    }

    static async Task DemonstrateParallelExecution()
    {
        Console.WriteLine("\n\n2. ПАРАЛЛЕЛЬНОЕ ВЫПОЛНЕНИЕ ЗАДАЧ\n");

        var parallelDemo = new ParallelExecution();

        // Последовательное vs параллельное выполнение
        await parallelDemo.SequentialExecutionAsync();
        await parallelDemo.ParallelExecutionWithWhenAllAsync();

        // Ожидание первой завершенной задачи
        await parallelDemo.WaitForFirstCompletionAsync();

        // Обработка результатов
        await parallelDemo.ProcessResultsAsync();
    }

    static async Task DemonstrateCancellation()
    {
        Console.WriteLine("\n\n3. ОТМЕНА АСИНХРОННЫХ ОПЕРАЦИЙ\n");

        var cancellationDemo = new CancellationDemo();

        // Отмена по требованию пользователя
        Console.WriteLine("--- Отмена по требованию пользователя ---");
        await cancellationDemo.DemonstrateCancellationAsync();

        // Отмена по таймауту
        Console.WriteLine("\n--- Отмена по таймауту ---");
        await cancellationDemo.DemonstrateTimeoutAsync();

        // Связанные токены отмены
        Console.WriteLine("\n--- Связанные токены отмены ---");
        using var cts = new CancellationTokenSource();
        try
        {
            await cancellationDemo.CancelableOperationWithLinkedTokenAsync(cts.Token, TimeSpan.FromSeconds(2));
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция отменена (связанный токен)");
        }
    }

    static async Task DemonstrateAsyncEventsAndStreams()
    {
        Console.WriteLine("\n\n4. АСИНХРОННЫЕ СОБЫТИЯ И ПОТОКИ\n");

        // Асинхронные события
        Console.WriteLine("--- Асинхронные события ---");
        var publisher = new AsyncEventPublisher();

        // Подписываем асинхронные обработчики
        publisher.DataReceived += async (data) =>
        {
            await Task.Delay(100);
            Console.WriteLine($"Обработчик 1 получил: {data}");
        };

        publisher.DataReceived += async (data) =>
        {
            await Task.Delay(200);
            Console.WriteLine($"Обработчик 2 получил: {data}");
        };

        await publisher.PublishDataAsync("Тестовые данные");

        // Асинхронные потоки
        Console.WriteLine("\n--- Асинхронные потоки (IAsyncEnumerable) ---");
        var streamsDemo = new AsyncStreamsDemo();
        await streamsDemo.ProcessAsyncStreamAsync();

        // ConfigureAwait
        Console.WriteLine("\n--- ConfigureAwait ---");
        await streamsDemo.DemonstrateConfigureAwaitAsync();

        // Асинхронный поток с отменой
        Console.WriteLine("\n--- Асинхронный поток с отменой ---");
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        try
        {
            await foreach (var item in streamsDemo.GenerateDataWithCancellationAsync(10, cts.Token))
            {
                Console.WriteLine($"Обработка: {item}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Асинхронный поток отменен");
        }
    }

    static async Task DemonstrateParallelComputations()
    {
        Console.WriteLine("\n\n5. ПАРАЛЛЕЛЬНЫЕ ВЫЧИСЛЕНИЯ С TPL\n");

        var computations = new ParallelComputations();

        // CPU-bound операции
        await computations.SequentialCpuBoundAsync();
        await computations.ParallelCpuBoundAsync();

        // Parallel.For и Parallel.ForEach
        computations.ParallelForDemo();
        computations.ParallelForEachDemo();

        // AsyncLocal
        Console.WriteLine("--- AsyncLocal для контекста выполнения ---");
        var contextDemo = new ParallelComputations.AsyncContextDemo();
        await contextDemo.DemonstrateAsyncLocal();
    }
}

//1.АСИНХРОННАЯ МОДЕЛЬ C#
//async/await - syntactic sugar для работы с Task/Task<T>

//Не создает новые потоки - использует thread pool для продолжений

//Освобождает поток во время операций ввода-вывода

//2. КЛЮЧЕВЫЕ ПРАВИЛА
//Async all the way - асинхронные методы должны вызываться асинхронно

//Не использовать async void - кроме обработчиков событий

//ConfigureAwait(false) в библиотеках - не восстанавливать контекст

//3. PARALLEL VS ASYNC
//Async - для I/O bound операций (сетевые запросы, файловые операции)

//Parallel - для CPU bound операций (вычисления, обработка данных)

//4. CANCELLATION TOKEN
//Кооперативная отмена - операция должна периодически проверять токен

//Таймауты - через CancellationTokenSource с временем

//Связанные токены - комбинация нескольких источников отмены

//Асинхронное программирование позволяет эффективно использовать ресурсы системы без блокировки потоков!
