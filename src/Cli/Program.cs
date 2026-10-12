using Core;
using Core.Dto;
using Core.Import;

// ==========================================
// ЛАБОРАТОРНА РОБОТА №1 & №2: Інформація та середовище
// ==========================================
EnvironmentReport report = EnvironmentInfo.Collect();

Console.WriteLine("╔════════════════════════════════════════════════════╗");
Console.WriteLine("║        CrossApp – крос-платформне програмування    ║");
Console.WriteLine("╚════════════════════════════════════════════════════╝");
Console.WriteLine("Студент: Хархаліс Андріан");
Console.WriteLine("Група: ФЕІ-31\n");

Console.WriteLine("ІНФОРМАЦІЯ ПРО СЕРЕДОВИЩЕ (Лаб. 2)");
Console.WriteLine(new string('─', 52));
Console.WriteLine($"Операційна система : {report.OsDescription}");
Console.WriteLine($"Runtime            : {report.FrameworkDescription}");
Console.WriteLine($"Архітектура        : {report.ProcessArchitecture}");
Console.WriteLine($"RID (визначено)    : {report.DetectedRid}");
Console.WriteLine($"RID (від .NET)     : {report.ReportedRid}");
Console.WriteLine($"Каталог застосунку : {report.BaseDirectory}\n");

Console.WriteLine("ПРЕДМЕТНА ОБЛАСТЬ (Лаб. 1)");
Console.WriteLine(new string('─', 52));
Console.WriteLine("Замовлення / Склад");
Console.WriteLine("Сутності           : Customer, Product, Order, OrderLine");
Console.WriteLine("Призначення        : оформлення замовлень і підрахунок вартості.\n");

// ==========================================
// ЛАБОРАТОРНА РОБОТА №3: Імпорт даних з файлу
// ==========================================
string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

ImportResult<ProductDto> result = ProductCsvImporter.Load(path);

Console.WriteLine("ІМПОРТ ДАНИХ З ФАЙЛУ (Лаб. 3)");
Console.WriteLine(new string('─', 52));
Console.WriteLine($"Завантажено записів: {result.Items.Count}");

foreach (ProductDto p in result.Items.Take(5))
{
    Console.WriteLine($" {p.Id,-6} {p.Sku,-10} {p.Name,-26} {p.Quantity,5} {p.Unit}");
}

if (result.Errors.Count > 0)
{
    Console.WriteLine($"\nПропущено рядків: {result.Errors.Count}");
    foreach (string e in result.Errors)
    {
        Console.WriteLine($" ! {e}");
    }
}

return 0;