using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

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
            .FirstOrDefaultAsync(x => x.Username == userName && !x.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
