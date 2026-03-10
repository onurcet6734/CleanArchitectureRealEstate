# Database Migration Komutları

## Yeni Migration Oluşturma

```bash
# Infrastructure klasöründe çalıştırın
cd CleanArchitectureRealEstate.Infrastructure

# Migration oluştur
dotnet ef migrations add AddUserRoleAndEDevletFields --startup-project ../CleanArchitectureRealEstate.API

# Database'i güncelle
dotnet ef database update --startup-project ../CleanArchitectureRealEstate.API
```

## Değişiklikler

### User Entity
- `Role` (UserRole enum): User, Admin
- `IsEDevletVerified` (bool)
- `EDevletVerifiedAt` (DateTime?)
- `PasswordResetToken` (string?)
- `PasswordResetTokenExpires` (DateTime?)

### Yeni Endpoints
- `/api/admin/*` - Admin panel endpoints (AdminOnly policy)
- `/api/password/forgot` - Şifremi unuttum
- `/api/password/reset` - Şifre sıfırlama
- `/api/edevlet/*` - e-Devlet doğrulama
- `/api/properties/*` - İlan yönetimi (resim upload dahil)
- `/api/profile` - Kullanıcı profil yönetimi
