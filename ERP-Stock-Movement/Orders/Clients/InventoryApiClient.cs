using System.Net.Http.Json;
using ERP_Stock_Movement.Inventory.Models;

namespace ERP_Stock_Movement.Orders.Clients;

public class InventoryApiClient : IInventoryApiClient
{
    private readonly HttpClient _httpClient;

    public InventoryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TryDeductResponse> TryDeductAsync(TryDeductRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/inventory/try-deduct", request);
        var result = await response.Content.ReadFromJsonAsync<TryDeductResponse>();

        if (result is not null)
        {
            return result;
        }

        return new TryDeductResponse
        {
            Success = false,
            Message = $"Inventory service returned {(int)response.StatusCode}."
        };
    }
}
