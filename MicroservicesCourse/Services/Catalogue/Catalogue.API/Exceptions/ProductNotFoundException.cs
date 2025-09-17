using BuildingBlocks.Exceptions;

namespace Catalogue.API.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
            
        }
    }
}
