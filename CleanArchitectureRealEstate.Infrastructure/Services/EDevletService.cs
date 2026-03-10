using CleanArchitectureRealEstate.Application.Common.DTOs;
using CleanArchitectureRealEstate.Application.Common.Interfaces;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureRealEstate.Infrastructure.Services
{
    public class EDevletService : IEDevletService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public EDevletService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> ExchangeCodeForTokenAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            var tokenUrl = _config["EDevlet:TokenUrl"];
            var clientId = _config["EDevlet:ClientId"];
            var clientSecret = _config["EDevlet:ClientSecret"];
            var redirectUri = _config["EDevlet:RedirectUri"];

            var requestUrl =
                $"{tokenUrl}?" +
                $"grant_type=authorization_code&" +
                $"client_id={clientId}&" +
                $"client_secret={clientSecret}&" +
                $"code={code}&" +
                $"redirect_uri={redirectUri}";

            var response = await _httpClient.PostAsync(requestUrl, null, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"e-Devlet token alınamadı. Status: {response.StatusCode}, Body: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var json = JsonDocument.Parse(content);

            if (!json.RootElement.TryGetProperty("access_token", out var tokenProp))
            {
                var error = json.RootElement.TryGetProperty("error_description", out var err)
                    ? err.GetString()
                    : "Bilinmeyen hata";

                throw new InvalidOperationException($"Access token bulunamadı: {error}");
            }

            return tokenProp.GetString()!;
        }

        public async Task<EDevletUserDto> GetUserInfoAsync(
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            var userInfoUrl = _config["EDevlet:UserInfoUrl"];
            var clientId = _config["EDevlet:ClientId"];

            var url =
                $"{userInfoUrl}?" +
                $"accessToken={accessToken}&" +
                $"resourceId=1&" +
                $"kapsam=Ad-Soyad&" +
                $"clientId={clientId}";

            var response = await _httpClient.PostAsync(url, null, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"e-Devlet kullanıcı bilgisi alınamadı. Status: {response.StatusCode}, Body: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var json = JsonDocument.Parse(content);

            var root = json.RootElement;

            if (root.TryGetProperty("sonucKodu", out var sonuc) &&
                sonuc.GetString() != "EDV09.000")
            {
                var error = root.TryGetProperty("sonucAciklamasi", out var err)
                    ? err.GetString()
                    : "Bilinmeyen hata";

                throw new InvalidOperationException($"e-Devlet hata: {error}");
            }

            var userNode = root.GetProperty("kullaniciBilgileri");

            return new EDevletUserDto
            {
                Identity = userNode.GetProperty("kimlikNo").GetString()!,
                Name = userNode.GetProperty("ad").GetString()!,
                Surname = userNode.GetProperty("soyad").GetString()!,
                MotherName = userNode.TryGetProperty("anneAdi", out var m) ? m.GetString() : null,
                FatherName = userNode.TryGetProperty("babaAdi", out var f) ? f.GetString() : null,
                BirthDate = userNode.TryGetProperty("dogumTarihi", out var d)
                    ? DateTime.Parse(d.GetString()!)
                    : null
            };
        }
    }
}
