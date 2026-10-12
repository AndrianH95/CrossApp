using Core;
using Core.Abstractions;
using Core.Domain;
using Core.Dto;
using Core.Import;
using Core.Services;
using Core.Storage;

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
// Автоматично шукаємо sample.csv у стандартних папка (не залежить від args)
string samplePath = Path.Combine("data", "sample.csv");
if (!File.Exists(samplePath))
{
    samplePath = Path.Combine("src", "data", "sample.csv");
}

if (File.Exists(samplePath))
{
    ImportResult<ProductDto> result = ProductCsvImporter.Load(samplePath);

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
    Console.WriteLine();
}

// ==========================================
// ЛАБОРАТОРНА РОБОТА №4: Доменна модель та інваріанти
// ==========================================
Console.WriteLine("ДОМЕННА МОДЕЛЬ ТА ІНВАРІАНТИ (Лаб. 4)");
Console.WriteLine(new string('─', 52));

Console.WriteLine("Сценарій 1: успіх");
Product sampleProduct = Product.Create("P-001", "sku-001", "Цемент М400 25кг", "шт", 100);
Console.WriteLine(sampleProduct);
sampleProduct.RegisterArrival(50);
sampleProduct.Issue(30);
Console.WriteLine(sampleProduct);
Console.WriteLine();

Console.WriteLine("Сценарій 2: порушення інваріантів");
TryDo("видача більша за залишок", () => sampleProduct.Issue(1000));
TryDo("порожній SKU", () => Product.Create("P-002", "", "Пісок", "т", 10));
TryDo("від'ємний залишок", () => Product.Create("P-003", "SKU-003", "Цегла", "шт", -5));
Console.WriteLine();

// ==========================================
// ЛАБОРАТОРНА РОБОТА №5: Сервіс, сховища та ручний DI
// ==========================================
Console.WriteLine("СЕРВІСНИЙ ШАР ТА СХОВИЩА (Лаб. 5)");
Console.WriteLine(new string('─', 52));

bool useFile = args.Contains("--file");
string dataPath = Path.Combine(AppContext.BaseDirectory, "data", "catalog.json");

ICatalogStore store = useFile
    ? new FileCatalogStore(dataPath)
    : new InMemoryCatalogStore(SampleData.Products());

var service = new CatalogService(store);

Console.WriteLine($"Поточне сховище    : {store.GetType().Name}");
if (useFile)
{
    Console.WriteLine($"Шлях до файлу      : {dataPath}");
}

// Демонстрація роботи сервісу
var created = service.Add("SKU-101", "Кабель UTP cat6", "м", 50);
service.Receive(created.Id, 25);

Console.WriteLine("\nВміст сховища через сервіс:");
foreach (var p in service.All())
{
    Console.WriteLine($" {p.Id} {p.Sku,-10} {p.Name,-22} {p.Quantity,5} {p.Unit}");
}

// Демонстрація обробки помилок у сервісі (сценарій відмови)
Console.WriteLine("\nСценарій відмови в сервісі:");
TryDo("отримання по неіснуючому ID", () => service.Receive("non-existent", 10));

static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  {title}: виняток НЕ спрацював — інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  {title}: {ex.GetType().Name} - {ex.Message}");
    }
}

return 0;