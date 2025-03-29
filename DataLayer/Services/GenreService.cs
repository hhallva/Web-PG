using DataLayer.Models;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class GenreService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5199/api/Genres/";

        public GenreService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
            _client.GetStringAsync("1");
        }

        public GenreService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<Genre>?> GetAllAsync()
           => await _client.GetFromJsonAsync<List<Genre>?>("");
    }
}