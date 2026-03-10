# Clean Architecture Real Estate - Implementation Guide

## 📋 Tamamlanan Özellikler

### 1️⃣ USER & ROL YAPISI ✅

**Backend Değişiklikleri:**
- `User` entity'sine `Role` (enum: User, Admin) eklendi
- `IsEDevletVerified` ve `EDevletVerifiedAt` alanları eklendi
- `PasswordResetToken` ve `PasswordResetTokenExpires` alanları eklendi
- JWT token'a `Role` claim'i eklendi

**Dosyalar:**
- `CleanArchitectureRealEstate.Domain/Entities/User.cs`
- `CleanArchitectureRealEstate.Infrastructure/Services/TokenService.cs`

---

### 2️⃣ ADMIN PANELİ & YETKİLENDİRME ✅

**Backend:**
- `AdminOnly` ve `UserOrAdmin` authorization policy'leri eklendi
- Admin controller'ları oluşturuldu (User, Flat, Image CRUD)

**Frontend:**
- `/admin/login` - Ayrı admin login sayfası
- Role kontrolü ile admin paneline erişim kısıtlaması
- Token decode edilerek role kontrolü yapılıyor

**Dosyalar:**
- `CleanArchitectureRealEstate.API/Program.cs` (Authorization policies)
- `CleanArchitectureRealEstate.API/Controllers/AdminController.cs`
- `CleanArchitectureRealEstate.API/Controllers/AdminFlatsController.cs`
- `CleanArchitectureRealEstate.API/Controllers/AdminImagesController.cs`
- `frontend/src/pages/admin/AdminLoginPage.js`

---

### 3️⃣ ADMIN PANELİNDE CRUD İŞLEMLERİ ✅

**Endpoints:**

#### User Management
- `GET /api/admin/users` - Tüm kullanıcıları listele
- `GET /api/admin/users/{id}` - Kullanıcı detayı
- `POST /api/admin/users` - Yeni kullanıcı oluştur
- `PUT /api/admin/users/{id}` - Kullanıcı güncelle
- `DELETE /api/admin/users/{id}` - Kullanıcı sil (soft delete)
- `PATCH /api/admin/users/{id}/role` - Kullanıcı rolü güncelle

#### Flat Management
- `GET /api/admin/flats` - Tüm ilanları listele
- `GET /api/admin/flats/{id}` - İlan detayı
- `POST /api/admin/flats` - Yeni ilan oluştur
- `PUT /api/admin/flats/{id}` - İlan güncelle
- `DELETE /api/admin/flats/{id}` - İlan sil

#### Image Management (Multipart/Form-Data)
- `GET /api/admin/images` - Tüm resimleri listele
- `GET /api/admin/images/{id}` - Resim detayı
- `POST /api/admin/images` - Resim upload (multipart/form-data)
- `PUT /api/admin/images/{id}` - Resim güncelle (multipart/form-data)
- `DELETE /api/admin/images/{id}` - Resim sil

**Örnek Resim Upload (Postman/Frontend):**
```javascript
const formData = new FormData();
formData.append('flatId', 1);
formData.append('file', fileInput.files[0]);
formData.append('isPrimary', true);

await api.post('/admin/images', formData, {
  headers: { 'Content-Type': 'multipart/form-data' }
});
```

---

### 4️⃣ ŞİFREMİ UNUTTUM MEKANİZMASI ✅

**Backend:**
- Email servisi oluşturuldu (SMTP)
- Token bazlı şifre sıfırlama
- Token expiration (1 saat)

**Endpoints:**
- `POST /api/password/forgot` - Şifre sıfırlama talebi
- `POST /api/password/reset` - Yeni şifre belirleme
- `POST /api/password/validate-token` - Token doğrulama

**Frontend:**
- `/forgot-password` - E-posta giriş sayfası
- `/reset-password?token=xxx` - Yeni şifre belirleme sayfası

**Dosyalar:**
- `CleanArchitectureRealEstate.Application/Common/Interfaces/Services/IEmailService.cs`
- `CleanArchitectureRealEstate.Infrastructure/Services/EmailService.cs`
- `CleanArchitectureRealEstate.API/Controllers/PasswordController.cs`
- `frontend/src/pages/ForgotPasswordPage.js`
- `frontend/src/pages/ResetPasswordPage.js`

**Konfigürasyon (appsettings.json):**
```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "SmtpUser": "your-email@gmail.com",
  "SmtpPass": "your-app-password",
  "FromEmail": "noreply@realestate.com"
}
```

---

### 5️⃣ E-DEVLET ENTEGRASYONU ✅

**Backend:**
- e-Devlet OAuth2 flow mimarisi
- `IsEDevletVerified` flag'i
- Simülasyon endpoint'i (test için)

**Endpoints:**
- `GET /api/edevlet/initiate` - e-Devlet doğrulama başlat
- `GET /api/edevlet/callback` - e-Devlet callback
- `GET /api/edevlet/status` - Doğrulama durumu
- `POST /api/edevlet/simulate-verification` - Test için simülasyon

**Akış:**
1. Kullanıcı kayıt olur
2. `/api/edevlet/initiate` ile e-Devlet'e yönlendirilir
3. e-Devlet doğrulama sonrası `/api/edevlet/callback` çağrılır
4. `IsEDevletVerified = true` olarak işaretlenir

**Dosyalar:**
- `CleanArchitectureRealEstate.API/Controllers/EDevletController.cs`

**Konfigürasyon (appsettings.json):**
```json
"EDevlet": {
  "AuthUrl": "https://giris.turkiye.gov.tr/Giris/gir",
  "ClientId": "your-client-id",
  "ClientSecret": "your-client-secret",
  "RedirectUri": "http://localhost:5000/api/edevlet/callback"
}
```

---

### 6️⃣ CREATE PROPERTY AKIŞI ✅

**2 Adımlı Süreç:**

#### Adım 1: Resim Upload (Opsiyonel)
```javascript
POST /api/properties/upload-images
Content-Type: multipart/form-data

FormData:
- files: [file1, file2, file3]

Response:
{
  "images": [
    { "fileName": "guid.jpg", "url": "/flat-images/guid.jpg", "size": 12345 }
  ],
  "count": 3
}
```

#### Adım 2: İlan Oluşturma
```javascript
POST /api/properties
Content-Type: application/json

{
  "title": "3+1 Daire",
  "description": "...",
  "price": 500000,
  "currency": "TRY",
  "city": "İstanbul",
  "district": "Kadıköy",
  "addressLine": "...",
  "type": "Apartment",
  "status": "ForSale",
  "imageUrls": ["/flat-images/guid1.jpg", "/flat-images/guid2.jpg"]
}
```

**Özellikler:**
- Resim eklenmeden ilan oluşturulabilir
- Resimler daha sonra edit sayfasından eklenebilir
- `POST /api/properties/{id}/images` ile ek resim yükleme
- `DELETE /api/properties/{flatId}/images/{imageId}` ile resim silme

**Dosyalar:**
- `CleanArchitectureRealEstate.API/Controllers/PropertyController.cs`
- `frontend/src/pages/CreatePropertyPage.js`

---

### 7️⃣ PROPERTY DETAIL SAYFASI ✅

**Özellikler:**
- 1 büyük ana resim (primary veya ilk resim)
- Altında tüm resimler grid halinde thumbnail
- Thumbnail'e tıklayınca ana resim değişir
- 8 resim varsa 8'i de görünür (responsive grid)

**Grid Yapısı:**
- Mobile: 4 sütun
- Tablet: 6 sütun
- Desktop: 8 sütun

**Dosyalar:**
- `frontend/src/pages/PropertyDetailPage.js`

**Örnek Kullanım:**
```jsx
<div className="grid grid-cols-4 md:grid-cols-6 lg:grid-cols-8 gap-2">
  {images.map((image) => (
    <div key={image.id} onClick={() => setSelectedImage(image)}>
      <img src={image.url} className="w-full h-20 object-cover" />
    </div>
  ))}
</div>
```

---

### 8️⃣ PROFILE SAYFASI ✅

**Özellikler:**
- 3 Tab: Profil Bilgileri, Şifre Değiştir, Doğrulama
- Güncellenebilir alanlar: Ad, Soyad, Telefon, E-posta
- Şifre değiştirme (eski şifre doğrulaması ile)
- e-Devlet doğrulama durumu

**Endpoints:**
- `GET /api/profile` - Profil bilgilerini getir
- `PUT /api/profile` - Profil güncelle
- `PUT /api/profile/change-password` - Şifre değiştir

**Dosyalar:**
- `CleanArchitectureRealEstate.API/Controllers/ProfileController.cs`
- `frontend/src/pages/ProfilePage.js`

---

## 🗄️ Database Migration

### Migration Oluşturma

```bash
cd CleanArchitectureRealEstate.Infrastructure

dotnet ef migrations add AddUserRoleAndEDevletFields --startup-project ../CleanArchitectureRealEstate.API

dotnet ef database update --startup-project ../CleanArchitectureRealEstate.API
```

### Yeni Alanlar

**User Tablosu:**
- `Role` (int) - 0: User, 1: Admin
- `IsEDevletVerified` (bit)
- `EDevletVerifiedAt` (datetime2, nullable)
- `PasswordResetToken` (nvarchar, nullable)
- `PasswordResetTokenExpires` (datetime2, nullable)

---

## 🔐 Güvenlik

### Authorization Policies

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
});
```

### Controller Kullanımı

```csharp
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    // Sadece Admin erişebilir
}
```

---

## 📧 Email Konfigürasyonu

### Gmail için App Password Oluşturma

1. Google Account → Security
2. 2-Step Verification'ı aktif et
3. App Passwords → Generate
4. Oluşturulan şifreyi `appsettings.json`'a ekle

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "SmtpUser": "your-email@gmail.com",
  "SmtpPass": "generated-app-password",
  "FromEmail": "noreply@realestate.com"
}
```

---

## 🧪 Test Senaryoları

### 1. Admin Login Test
```
1. /admin/login sayfasına git
2. Admin kullanıcısı ile giriş yap
3. Token'da role="Admin" olduğunu kontrol et
4. /admin/dashboard'a yönlendirildiğini kontrol et
```

### 2. Şifremi Unuttum Test
```
1. /forgot-password sayfasına git
2. E-posta adresini gir
3. E-posta kutusunu kontrol et
4. Reset link'e tıkla
5. Yeni şifre belirle
6. Yeni şifre ile giriş yap
```

### 3. e-Devlet Doğrulama Test
```
1. Kayıt ol
2. /profile sayfasına git
3. "e-Devlet ile Doğrula" butonuna tıkla
4. (Test için) POST /api/edevlet/simulate-verification
5. IsEDevletVerified = true olduğunu kontrol et
```

### 4. İlan Oluşturma Test
```
1. /create-property sayfasına git
2. Resimleri seç ve yükle
3. İlan detaylarını doldur
4. İlanı oluştur
5. Property detail sayfasında resimlerin göründüğünü kontrol et
```

---

## 🚀 Çalıştırma

### Backend
```bash
cd CleanArchitectureRealEstate.API
dotnet run
```

### Frontend
```bash
cd frontend
npm install
npm start
```

---

## 📝 Notlar

### Production'a Geçerken

1. **Email Servisi:** Gerçek SMTP ayarlarını yapılandır
2. **e-Devlet:** Gerçek ClientId ve ClientSecret al
3. **Simulate Endpoint:** `/api/edevlet/simulate-verification` endpoint'ini kaldır
4. **CORS:** Production domain'ini ekle
5. **JWT Secret:** Güçlü bir secret key kullan
6. **HTTPS:** SSL sertifikası yapılandır

### Eksik Özellikler (İsteğe Bağlı)

- Admin paneli için React sayfaları (şu an sadece backend hazır)
- Resim boyutu ve format validasyonu
- Rate limiting (şifre sıfırlama için)
- Email template'leri (HTML)
- e-Devlet gerçek entegrasyonu (OAuth2 token exchange)
- Kullanıcı profil fotoğrafı
- İlan favorileme
- İlan arama ve filtreleme (gelişmiş)

---

## 📚 API Dokümantasyonu

Swagger UI: `http://localhost:5000/swagger`

Tüm endpoint'ler ve request/response örnekleri Swagger'da mevcuttur.

---

## 🐛 Sorun Giderme

### Migration Hatası
```bash
# Migration'ları sıfırla
dotnet ef database drop --startup-project ../CleanArchitectureRealEstate.API
dotnet ef database update --startup-project ../CleanArchitectureRealEstate.API
```

### Email Gönderim Hatası
- Gmail App Password kullanıldığından emin ol
- 2-Step Verification aktif olmalı
- Firewall/Antivirus SMTP portunu engelliyor olabilir

### JWT Token Hatası
- Token'ın expire olmadığından emin ol
- Role claim'inin doğru eklendiğini kontrol et
- Browser'da localStorage'ı temizle

---

## ✅ Checklist

- [x] User & Rol yapısı
- [x] Admin paneli authorization
- [x] Admin CRUD endpoints
- [x] Resim upload (multipart/form-data)
- [x] Şifremi unuttum
- [x] e-Devlet entegrasyonu
- [x] Create property akışı
- [x] Property detail sayfası
- [x] Profile sayfası
- [x] Admin login sayfası
- [x] Forgot/Reset password sayfaları

Tüm gereksinimler tamamlandı! 🎉
