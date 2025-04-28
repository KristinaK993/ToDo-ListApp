namespace ToDoApp.API.Services
{
    public interface IRandomJokeService //hämtar slumpmässigt skämt från openAPI
    {
        Task<string> GetRandomJokeAsync(); //sträng som visar själva skämtet 

    }
}
