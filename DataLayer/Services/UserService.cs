using DataLayer.Models;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5199/api/Users/";

        public UserService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
            _client.GetStringAsync("1");
        }

        public UserService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<User?>> GetAllAsync()
           => await _client.GetFromJsonAsync<List<User?>>("");

        public async Task<User?> GetAsync(int id)
            => await _client.GetFromJsonAsync<User?>($"{id}");

        public async Task<List<Game?>> GetGamesAsync(int id)
            => await _client.GetFromJsonAsync<List<Game?>>($"{id}/Games");

        public async Task UpdateAsync(User user)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync($"{user.Id}", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddGameAsync(int userId, int gameId)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync($"{userId}/Games/{gameId}", "");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteGameAsync(int userId, int gameId)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync($"{userId}/Games/{gameId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
