using CleanArchitectureRealEstate.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Services
{
    public interface IEDevletService
    {
        Task<string> ExchangeCodeForTokenAsync(string code, CancellationToken cancellationToken = default);
        Task<EDevletUserDto> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default);
    }
}