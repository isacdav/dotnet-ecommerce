namespace Ecommerce.API.Search.Interfaces;

public interface ISearchService
{
    Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId);
}
