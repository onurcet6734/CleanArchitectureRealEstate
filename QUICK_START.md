# 🚀 Quick Start Guide

## Yapılan Değişiklikler Özeti

### ✅ 1. USER & ROL YAPISI
- User entity'sine `Role` (User/Admin), `IsEDevletVerified`, `PasswordResetToken` eklendi
- JWT token'a role claim'i eklendi

### ✅ 2. ADMIN PANELİ
- `/admin/login` - Ayrı admin login sayfası
- `AdminOnly` policy ile endpoint koruması
- Admin controller'ları: User, Flat, Image CRUD

### ✅ 3. ADMIN CRUD
- **Users:** GET, POST, PUT, DELETE, PATCH /api/admin/users
- **Flats:** GET, POST, PUT, DELETE /api/admin/flats
- **Images:** GET, POST, PUT, DELETE /api/admin/images (multipart/form-data)

### ✅ 4. ŞİFREMİ UNUTTUM
- POST /api/password/forgot
- POST /api/password/reset
- Email servisi (SMTP)
- Token expiration (1 saat)

### ✅ 5. E-DEVLET ENTEGRASYONU
- GET /api/edevlet/initiate
- GET /api/edevlet/callback
- GET /api/edevlet/status
- POST /api/edevlet/simulate-verification (test)

### ✅ 6. CREATE PROPERTY AKIŞI
- POST /api/properties/upload-images (multipart)
- POST /api/properties (resimler ile veya resimsiz)
- POST /api/properties/{id}/images (sonradan resim ekleme)

### ✅ 7. PROPERTY DETAIL
- 1 büyük ana resim
- Grid halinde tüm thumbnail'ler
- Responsive (4/6/8 sütun)

### ✅ 8. PROFILE SAYFASI
- GET /api/profile
- PUT /api/profile
- PUT /api/profile/change-password
- 3 Tab: Profil, Şifre, Doğrulama

---

## 🔧 Kurulum

### 1. Database Migration

```bash
cd CleanArchitectureRealEstate.Infrastructure
dotnet ef migrations add AddUserRoleAndEDevletFields --startup-project ../CleanArchitectureRealEstate.API
dotnet ef database update --startup-project ../CleanArchitectureRealEstate.API
```

### 2. appsettings.json Konfigürasyonu

```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "your-email@gmail.com",
    "SmtpPass": "your-app-password",
    "FromEmail": "noreply@realestate.com"
  },
  "EDevlet": {
    "AuthUrl": "https://giris.turkiye.gov.tr/Giris/gir",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RedirectUri": "http://localhost:5000/api/edevlet/callback"
  },
  "AppUrl": "http://localhost:5000",
  "FrontendUrl": "http://localhost:3000"
}
```

### 3. Backend Çalıştır

```bash
cd CleanArchitectureRealEstate.API
dotnet run
```

### 4. Frontend Çalıştır

```bash
cd frontend
npm install
npm start
```

---

## 🧪 Test Adımları

### Admin Login
1. `http://localhost:3000/admin/login`
2. Admin kullanıcısı ile giriş yap
3. Dashboard'a yönlendirildiğini kontrol et

### Şifremi Unuttum
1. `http://localhost:3000/forgot-password`
2. E-posta gir
3. E-posta kutusunu kontrol et
4. Reset link ile yeni şifre belirle

### İlan Oluşturma
1. `http://localhost:3000/create-property`
2. Resimleri yükle (opsiyonel)
3. İlan detaylarını doldur
4. İlanı oluştur

### e-Devlet Doğrulama (Test)
```bash
# Postman ile
POST http://localhost:5000/api/edevlet/simulate-verification
Authorization: Bearer {token}
```

---

## 📁 Yeni Dosyalar

### Backend
- `Controllers/AdminController.cs`
- `Controllers/AdminFlatsController.cs`
- `Controllers/AdminImagesController.cs`
- `Controllers/PasswordController.cs`
- `Controllers/EDevletController.cs`
- `Controllers/PropertyController.cs`
- `Controllers/ProfileController.cs`
- `Services/EmailService.cs`
- `Interfaces/IEmailService.cs`

### Frontend
- `pages/admin/AdminLoginPage.js`
- `pages/ForgotPasswordPage.js`
- `pages/ResetPasswordPage.js`
- `pages/PropertyDetailPage.js` (güncellendi)
- `pages/ProfilePage.js` (güncellendi)
- `pages/CreatePropertyPage.js` (güncellendi)

---

## 🔑 Önemli Endpoint'ler

### Admin (AdminOnly Policy)
```
GET    /api/admin/users
POST   /api/admin/users
PUT    /api/admin/users/{id}
DELETE /api/admin/users/{id}
PATCH  /api/admin/users/{id}/role

GET    /api/admin/flats
POST   /api/admin/flats
PUT    /api/admin/flats/{id}
DELETE /api/admin/flats/{id}

GET    /api/admin/images
POST   /api/admin/images (multipart/form-data)
PUT    /api/admin/images/{id} (multipart/form-data)
DELETE /api/admin/images/{id}
```

### Password
```
POST /api/password/forgot
POST /api/password/reset
POST /api/password/validate-token
```

### e-Devlet
```
GET  /api/edevlet/initiate
GET  /api/edevlet/callback
GET  /api/edevlet/status
POST /api/edevlet/simulate-verification
```

### Properties
```
POST   /api/properties/upload-images (multipart/form-data)
POST   /api/properties
GET    /api/properties/{id}
PUT    /api/properties/{id}
POST   /api/properties/{id}/images (multipart/form-data)
DELETE /api/properties/{flatId}/images/{imageId}
```

### Profile
```
GET /api/profile
PUT /api/profile
PUT /api/profile/change-password
```

---

## 📸 Resim Upload Örneği

### JavaScript/React
```javascript
const formData = new FormData();
formData.append('files', file1);
formData.append('files', file2);

const response = await api.post('/properties/upload-images', formData, {
  headers: { 'Content-Type': 'multipart/form-data' }
});

// Response: { images: [...], count: 2 }
```

### Postman
```
POST http://localhost:5000/api/admin/images
Content-Type: multipart/form-data

Body (form-data):
- flatId: 1
- file: [select file]
- isPrimary: true
```

---

## 🎯 Tamamlanan Tüm Gereksinimler

✅ 1. User & Rol yapısı (User, Admin)  
✅ 2. Admin paneli & yetkilendirme (AdminOnly policy)  
✅ 3. Admin CRUD (User, Flat, Image) + Multipart upload  
✅ 4. Şifremi unuttum (Email + Token)  
✅ 5. e-Devlet entegrasyonu (OAuth2 mimarisi)  
✅ 6. Create property akışı (Resim → İlan)  
✅ 7. Property detail (Ana resim + Grid thumbnails)  
✅ 8. Profile sayfası (Profil, Şifre, Doğrulama)  

---

## 📚 Detaylı Dokümantasyon

Daha fazla bilgi için: `IMPLEMENTATION_GUIDE.md`

Swagger UI: `http://localhost:5000/swagger`

---

## 🐛 Sorun mu var?

1. Migration hatası → `dotnet ef database drop` sonra tekrar `update`
2. Email gönderilmiyor → Gmail App Password kullan
3. JWT hatası → localStorage'ı temizle
4. CORS hatası → Backend'de CORS policy'yi kontrol et

---

## 🎉 Başarıyla Tamamlandı!

Tüm 8 gereksinim Clean Architecture prensiplerine uygun şekilde implement edildi.
