using System.Security.Cryptography;
using Api.Contracts;
using Core.Abstractions;
using Core.Domain;
using Core.Dto;

namespace Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        // GET: Отримати список усіх продуктів
        group.MapGet("/", (ICatalogStore store) =>
            TypedResults.Ok(store.List().Select(p => p.ToDto()).ToList()));

        // GET: Отримати продукт за ID
        group.MapGet("/{id}", (string id, ICatalogStore store) =>
        {
            var found = store.GetById(id);
            if (found is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(found.ToDto());
        });

        // POST: Створити новий продукт
        group.MapPost("/", (SaveProductRequest request, ICatalogStore store) =>
        {
            if (!request.TryValidate(out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var id = Convert.ToHexString(RandomNumberGenerator.GetBytes(4)).ToLowerInvariant();
            var created = Product.Create(id, request.Sku, request.Name, request.Unit, request.Quantity);
            store.Add(created);

            return Results.Created($"/api/products/{created.Id}", created.ToDto());
        });

        // PUT: Оновити існуючий продукт
        group.MapPut("/{id}", (string id, SaveProductRequest request, ICatalogStore store) =>
        {
            if (!request.TryValidate(out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var existing = store.GetById(id);
            if (existing is null)
            {
                return Results.NotFound();
            }

            var replacement = Product.Create(id, request.Sku, request.Name, request.Unit, request.Quantity);
            store.Update(replacement);

            return Results.Ok(replacement.ToDto());
        });

        // DELETE: Видалити продукт
        group.MapDelete("/{id}", (string id, ICatalogStore store) =>
        {
            var success = store.Remove(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}