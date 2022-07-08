using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services
{
    public interface ICatalogService
    {
        Task<CatalogModel> GetCatalog(string id);
    }
}
