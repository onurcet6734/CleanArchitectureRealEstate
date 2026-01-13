using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User User);
    }
}
