
namespace Catalogue.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender mediator) =>
            {
                try
                {
                    var query = new GetProductByIdQuery(id);
                    var result = await mediator.Send(query);

                    var response = result.Adapt<GetProductByIdResponse>();
                    return Results.Ok(response);
                }
                catch (ProductNotFoundException)
                {
                    return Results.BadRequest("Product Not Found!");
                }
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
        }
    }
}
