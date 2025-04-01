using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class VersionsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5199/api/Versions/";

        public VersionsService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
            _client.GetStringAsync("1");
        }

        public VersionsService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task AddVersionAsync(GameVersion version)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync("", version);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteVersionAsync(int id)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
