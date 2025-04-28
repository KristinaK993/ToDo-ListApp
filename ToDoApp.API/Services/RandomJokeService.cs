using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoApp.API.Services
{
    public class RandomJokeService : IRandomJokeService
    {
        private readonly HttpClient _http; //gör http anrop

        public RandomJokeService(HttpClient http) //konstruktor m DI av htpClient
        {
            _http = http; //tilldelar den injecerade httpClient till privata fält
        }

        public async Task<string> GetRandomJokeAsync() //metod som hämtar en randomJoke från openAPI
        {
            var response = await _http.GetAsync("https://api.chucknorris.io/jokes/random"); //gör ett httpAnrop till APIt

            if (!response.IsSuccessStatusCode) //om nt api anrop lyckas return sträng nedan
                return "Could not fetch a joke at the moment.";

            var json = await response.Content.ReadAsStringAsync();//Läser innehållet från HTTP-svaret (body) som en textsträng (oftast JSON)

            using var doc = JsonDocument.Parse(json);// Gör om JSON-strängen till ett objekt så man kan läsa värden ur den
            var joke = doc.RootElement.GetProperty("value").GetString();// Hämtar skämttexten från Jsons "value"-fält som sträng

            return joke ?? "No joke found.";//ret joke, eller fallback om null 
        }
    }
}
