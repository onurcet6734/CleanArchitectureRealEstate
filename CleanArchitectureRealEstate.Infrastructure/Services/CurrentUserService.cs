using CleanArchitectureRealEstate.Application.Common.Interfaces;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;

namespace CleanArchitectureRealEstate.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
