using System.Net.Http.Json;
using ERP_Stock_Movement.Products.Models;

namespace ERP_Stock_Movement.Clients;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"api/product/{productId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Product>();
    }
}
