using System.Runtime.InteropServices;

Console.WriteLine("╔════════════════════════════════════════════════════╗");
Console.WriteLine("║       CrossApp – крос-платформне програмування    ║");
Console.WriteLine("╚════════════════════════════════════════════════════╝");

Console.WriteLine("Студент: Хархаліс Андріан");
Console.WriteLine("Група: ФЕІ-31");

Console.WriteLine();
Console.WriteLine("ІНФОРМАЦІЯ ПРО СЕРЕДОВИЩЕ");
Console.WriteLine("────────────────────────────────────────────────────");

Console.WriteLine($"Операційна система : {RuntimeInformation.OSDescription}");
Console.WriteLine($"Версія ОС          : {Environment.OSVersion}");
Console.WriteLine($"Архітектура        : {RuntimeInformation.ProcessArchitecture}");
Console.WriteLine($"Версія .NET         : {Environment.Version}");
Console.WriteLine($"Runtime             : {RuntimeInformation.FrameworkDescription}");

Console.WriteLine();
Console.WriteLine("ШЛЯХИ ЗАПУСКУ");
Console.WriteLine("────────────────────────────────────────────────────");

Console.WriteLine($"Каталог застосунку : {AppContext.BaseDirectory}");
Console.WriteLine($"Поточний каталог   : {Environment.CurrentDirectory}");

Console.WriteLine();
Console.WriteLine("ПРЕДМЕТНА ОБЛАСТЬ");
Console.WriteLine("────────────────────────────────────────────────────");

Console.WriteLine("Замовлення");
Console.WriteLine("Сутності: Customer, Product, Order, OrderLine");
Console.WriteLine("Призначення: оформлення замовлень і підрахунок їхньої вартості.");

Console.WriteLine();
Console.WriteLine("════════════════════════════════════════════════════");
Console.WriteLine("             Лабораторна робота №1");
Console.WriteLine("════════════════════════════════════════════════════");