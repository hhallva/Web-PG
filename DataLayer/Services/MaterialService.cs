using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class MaterialService
    {

        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5199/api/Materials/";

        public MaterialService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
            _client.GetStringAsync("1");
        }

        public MaterialService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<Material?> GetAsync(int id)
            => await _client.GetFromJsonAsync<Material?>($"{id}");

        public async Task AddMaterialAsync(Material material)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync("", material);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteMaterialAsync(int id)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
