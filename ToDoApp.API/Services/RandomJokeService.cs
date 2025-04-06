using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoApp.API.Services
{
    public class RandomJokeService : IRandomJokeService
    {
        private readonly HttpClient _http;

        public RandomJokeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetRandomJokeAsync()
        {
            var response = await _http.GetAsync("https://api.chucknorris.io/jokes/random");

            if (!response.IsSuccessStatusCode)
                return "Could not fetch a joke at the moment.";

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var joke = doc.RootElement.GetProperty("value").GetString();

            return joke ?? "No joke found.";
        }
    }
}
