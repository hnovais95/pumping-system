namespace PumpingSystem.Domain.Repository
{
    public interface IAuthenticationRepository
    {
        void Insert(string username, string password, int timeout);
        bool CheckIfItExistsByUsernameAndPassword(string username, string password, int timeout);
    }
}
