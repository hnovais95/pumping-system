namespace PumpingSystem.Server.Repository
{
    public interface IAuthentication
    {
        void Insert(string username, string password, int timeout);
        bool CheckIfItExistsByUsernameAndPassword(string username, string password, int timeout);
    }
}
