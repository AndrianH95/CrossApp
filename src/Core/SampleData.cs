using Core.Domain;

namespace Core;

public static class SampleData
{
    public static IReadOnlyList<Product> Products() => new List<Product>
    {
        Product.Create("P-001", "SKU-001", "Цемент М400 25кг", "шт", 120),
        Product.Create("P-002", "SKU-002", "Пісок будівельний", "т", 18),
        Product.Create("P-003", "SKU-003", "Цегла червона", "шт", 4200),
        Product.Create("P-004", "SKU-004", "Фарба водоемульсійна 10л", "шт", 36),
        Product.Create("P-005", "SKU-005", "Шпаклівка фінішна", "кг", 250),
        Product.Create("P-006", "SKU-006", "Цвяхи будівельні 100мм", "кг", 5), // дефіцит
        Product.Create("P-007", "SKU-007", "Грунтовка глибокого проникнення", "л", 45),
        Product.Create("P-008", "SKU-008", "Пінопласт 50мм", "м2", 300),
        Product.Create("P-009", "SKU-009", "Сітка армуюча", "м2", 150),
        Product.Create("P-010", "SKU-010", "Плити OSB-3", "шт", 80),
        Product.Create("P-011", "SKU-011", "Монтажна піна", "шт", 4), // дефіцит
        Product.Create("P-012", "SKU-012", "Силікон санітарний", "шт", 12),
        Product.Create("P-013", "SKU-013", "Вапно гашене", "кг", 60),
        Product.Create("P-014", "SKU-014", "Блок газобетонний", "шт", 1200),
        Product.Create("P-015", "SKU-015", "Арматура 12мм", "т", 8),
        Product.Create("P-016", "SKU-016", "Скоба будівельна", "шт", 3) // дефіцит
    };
}