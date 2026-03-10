using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureRealEstate.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == userName && !x.IsDeleted, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
        }

        public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.PasswordResetToken == token && !x.IsDeleted, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AnyAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AnyAsync(x => x.UserName == username && !x.IsDeleted, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
