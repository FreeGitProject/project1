using EcommerceBackend.Domain.Interfaces;
using EcommerceBackend.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
