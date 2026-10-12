using Core;
using Core.Abstractions;
using Core.Domain;
using Core.Dto;
using Core.Events;
using Core.Import;
using Core.Services;
using Core.Storage;

// ==========================================
// ЛАБОРАТОРНІ РОБОТИ №1 & №2: Середовище та предметна область
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
// ЛАБОРАТОРНІ РОБОТИ №5 & №6: Сервіс, сховища, події та делегати
// ==========================================
Console.WriteLine("СЕРВІСНИЙ ШАР, ПОДІЇ ТА ДЕЛЕГАТИ (Лаб. 5-6)");
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
Console.WriteLine();

// Підписка на доменні події (Лаб. 6)
var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "app.log");
Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

void OnCatalogChanged(object? sender, CatalogChangedEventArgs e)
{
    var line = $"{e.At:HH:mm:ss} {e.Kind,-8} {e.Id} {e.Delta,4:+0;-0} → залишок {e.QuantityAfter}";
    Console.WriteLine($"[ПОДІЯ] {line}");
    File.AppendAllText(logPath, line + Environment.NewLine);
}

service.Changed += OnCatalogChanged;

int changesCount = 0;
service.Changed += (_, _) => changesCount++;

// Демонстрація операцій сервісу з генерацією подій
var created = service.Add("SKU-101", "Кабель UTP cat6", "м", 50);
service.Receive(created.Id, 25);

Console.WriteLine($"\nВсього подій отримано (через лічильник-лямбду): {changesCount}");

// Демонстрація відписки
service.Changed -= OnCatalogChanged;
service.Receive(created.Id, 5); // Ця операція пройде тихо для консолі та файлу логів
Console.WriteLine($"Подій після відписки (лічильник): {changesCount}");

// Демонстрація використання Func (FindAll)
Console.WriteLine("\nФільтрація через FindAll (залишок < 100):");
var filtered = service.FindAll(p => p.Quantity < 100);
foreach (var p in filtered)
{
    Console.WriteLine($"  {p.Sku} — {p.Name} ({p.Quantity} {p.Unit})");
}

// Сценарій відмови в сервісі
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