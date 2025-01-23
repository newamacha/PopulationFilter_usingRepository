using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Repository.Interface;

namespace test.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TestDb _context;
        public AuthRepository(TestDb context)
        {
            _context = context;
        }
        public async Task<bool> UserExists(string username, string password)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && u.Password == password);
        }
    }
}