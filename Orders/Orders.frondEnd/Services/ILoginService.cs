namespace Orders.frondEnd.Services
{
    public interface ILoginService
    {
        Task LoginAsync(string token);
        Task LogoutAsync();

    }
}
