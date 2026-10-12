using Core.Domain;

namespace Api.Contracts;

public static class ProductExtensions
{
    public static ProductDto ToDto(this Product product) =>
        new(product.Id, product.Sku, product.Name, product.Unit, product.Quantity);
}