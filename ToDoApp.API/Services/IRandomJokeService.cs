namespace ToDoApp.API.Services
{
    public interface IRandomJokeService
    {
        Task<string> GetRandomJokeAsync();

    }
}
