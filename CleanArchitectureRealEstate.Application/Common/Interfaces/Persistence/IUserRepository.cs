using CleanArchitectureRealEstate.Domain.Entities;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken requestAborted);
    }
}
