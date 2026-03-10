using CleanArchitectureRealEstate.Domain.Entities;
using System.Threading.Tasks;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
