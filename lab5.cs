using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

public class ApiResponse<T>
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public List<T> Data { get; set; }
}

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<T>>(content);

            return new ApiResponse<T>
            {
                Message = "Success",
                StatusCode = (int)response.StatusCode,
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>
            {
                Message = ex.Message,
                StatusCode = 500,
                Data = null
            };
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string url, object payload)
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, httpContent);
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<T>>(content);

            return new ApiResponse<T>
            {
                Message = "Success",
                StatusCode = (int)response.StatusCode,
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>
            {
                Message = ex.Message,
                StatusCode = 500,
                Data = null
            };
        }
    }
}
