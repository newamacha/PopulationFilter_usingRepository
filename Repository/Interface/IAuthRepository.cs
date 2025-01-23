namespace test.Repository.Interface
{
    public interface IAuthRepository
    {
        public Task<bool> UserExists(string username, string password);
    }
}
