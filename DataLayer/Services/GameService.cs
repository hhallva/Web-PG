using DataLayer.Models;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class GameService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5199/api/Games/";

        public GameService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
            _client.GetStringAsync("1");
        }

        public GameService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<Game>?> GetAllAsync()
           => await _client.GetFromJsonAsync<List<Game>?>("");

        public async Task<Game?> GetAsync(int id)
            => await _client.GetFromJsonAsync<Game?>($"{id}");

        public async Task<List<Material?>> GetMaterialsAsync(int id)
           => await _client.GetFromJsonAsync<List<Material?>>($"{id}/Materials");

        public async Task<Game?> AddGameAsync(Game game)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync("", game);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            if (int.TryParse(responseBody, out int gameId))
            {
                return await GetAsync(gameId);
            }
            else
            {

                return null;
            }
        }

        public async Task DeleteGameAsync(int id)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Game game)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync($"{game.Id}", game);
            response.EnsureSuccessStatusCode();
        }
    }
}