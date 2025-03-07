using Shared.Events;

namespace CatalogService.Extensions
{
    public static class ProductAddedEventExtensions
    {
        public static ProductEntity ToEntity(this ProductAddedEvent @event) =>
            new ProductEntity(
                Id: @event.Id, 
                Description: @event.Description, 
                Price: @event.Price);
    }
}
