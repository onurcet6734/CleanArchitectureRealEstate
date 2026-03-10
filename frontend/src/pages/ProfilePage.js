import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Label } from '../components/ui/label';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../components/ui/tabs';
import { Badge } from '../components/ui/badge';
import { Alert, AlertDescription } from '../components/ui/alert';
import { toast } from 'sonner';
import { profileAPI } from '../services/api';

const ProfilePage = () => {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  // Profile form
  const [profileForm, setProfileForm] = useState({
    email: '',
    firstName: '',
    lastName: '',
    phoneNumber: ''
  });

  // Password form
  const [passwordForm, setPasswordForm] = useState({
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  });

  useEffect(() => {
    fetchProfile();
    checkEDevletCallback();
  }, []);

  const checkEDevletCallback = () => {
    const params = new URLSearchParams(window.location.search);
    const status = params.get('edevlet');

    if (status === 'success') {
      toast.success('e-Devlet doğrulaması başarıyla tamamlandı');
      fetchProfile();
      window.history.replaceState({}, '', window.location.pathname);
    }

    if (status === 'error') {
      toast.error('e-Devlet doğrulaması başarısız oldu');
      window.history.replaceState({}, '', window.location.pathname);
    }
  };

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const data = await profileAPI.getProfile();
      setProfile(data);
      setProfileForm({
        email: data.email || '',
        firstName: data.firstName || '',
        lastName: data.lastName || '',
        phoneNumber: data.phoneNumber || ''
      });
    } catch (error) {
      console.error('Profile fetch error:', error);
      toast.error('Profil bilgileri yüklenirken hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleProfileChange = (e) => {
    setProfileForm({
      ...profileForm,
      [e.target.name]: e.target.value
    });
  };

  const handlePasswordChange = (e) => {
    setPasswordForm({
      ...passwordForm,
      [e.target.name]: e.target.value
    });
  };

  const handleProfileSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);

    try {
      await profileAPI.updateProfile(profileForm);
      toast.success('Profil bilgileri güncellendi', );
      fetchProfile();
    } catch (error) {
      console.error('Profile update error:', error);
      toast.error(error.response?.data?.error || 'Profil güncellenirken hata oluştu');
    } finally {
      setSaving(false);
    }
  };

  const handlePasswordSubmit = async (e) => {
    e.preventDefault();

    if (passwordForm.newPassword !== passwordForm.confirmPassword) {
      toast.error('Yeni şifreler eşleşmiyor');
      return;
    }

    if (passwordForm.newPassword.length < 6) {
      toast.error('Yeni şifre en az 6 karakter olmalıdır');
      return;
    }

    setSaving(true);

    try {
      await profileAPI.changePassword({
        oldPassword: passwordForm.oldPassword,
        newPassword: passwordForm.newPassword
      });
      toast.success('Şifre başarıyla değiştirildi', );
      setPasswordForm({
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
      });
    } catch (error) {
      console.error('Password change error:', error);
      toast.error(error.response?.data?.error || 'Şifre değiştirilirken hata oluştu');
    } finally {
      setSaving(false);
    }
  };

  const handleEDevletVerification = async () => {
    try {
      const data = await profileAPI.initiateEDevlet();
      window.location.href = data.authUrl;
    } catch (error) {
      console.error('e-Devlet verification error:', error);
      toast.error('e-Devlet doğrulama başlatılamadı');
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Yükleniyor...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8 max-w-4xl">
      <h1 className="text-3xl font-bold mb-6">Profilim</h1>

      {/* e-Devlet Doğrulama Uyarısı */}
      {!profile?.isEDevletVerified && (
        <Alert className="mb-6 border-yellow-500 bg-yellow-50">
          <AlertDescription className="flex items-center justify-between">
            <div>
              <strong>⚠️ e-Devlet Doğrulaması Gerekli</strong>
              <p className="text-sm mt-1">
                Yasal gereklilikler nedeniyle hesabınızı e-Devlet ile doğrulamanız gerekmektedir.
              </p>
            </div>
            <Button onClick={handleEDevletVerification} variant="outline" size="sm">
              Şimdi Doğrula
            </Button>
          </AlertDescription>
        </Alert>
      )}

      <Tabs defaultValue="profile" className="space-y-6">
        <TabsList className="grid w-full grid-cols-3">
          <TabsTrigger value="profile">Profil Bilgileri</TabsTrigger>
          <TabsTrigger value="password">Şifre Değiştir</TabsTrigger>
          <TabsTrigger value="verification">Doğrulama</TabsTrigger>
        </TabsList>

        {/* Profil Bilgileri */}
        <TabsContent value="profile">
          <Card>
            <CardHeader>
              <CardTitle>Profil Bilgileri</CardTitle>
            </CardHeader>
            <CardContent>
              <form onSubmit={handleProfileSubmit} className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="firstName">Ad</Label>
                    <Input
                      id="firstName"
                      name="firstName"
                      value={profileForm.firstName}
                      onChange={handleProfileChange}
                      required
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="lastName">Soyad</Label>
                    <Input
                      id="lastName"
                      name="lastName"
                      value={profileForm.lastName}
                      onChange={handleProfileChange}
                      required
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="email">E-posta</Label>
                  <Input
                    id="email"
                    name="email"
                    type="email"
                    value={profileForm.email}
                    onChange={handleProfileChange}
                    required
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="phoneNumber">Telefon</Label>
                  <Input
                    id="phoneNumber"
                    name="phoneNumber"
                    type="tel"
                    value={profileForm.phoneNumber}
                    onChange={handleProfileChange}
                    required
                  />
                </div>

                <div className="space-y-2">
                  <Label>Kullanıcı Adı</Label>
                  <Input value={profile?.username} disabled />
                  <p className="text-sm text-gray-500">Kullanıcı adı değiştirilemez</p>
                </div>

                <Button type="submit" disabled={saving}>
                  {saving ? 'Kaydediliyor...' : 'Değişiklikleri Kaydet'}
                </Button>
              </form>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Şifre Değiştir */}
        <TabsContent value="password">
          <Card>
            <CardHeader>
              <CardTitle>Şifre Değiştir</CardTitle>
            </CardHeader>
            <CardContent>
              <form onSubmit={handlePasswordSubmit} className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="oldPassword">Mevcut Şifre</Label>
                  <Input
                    id="oldPassword"
                    name="oldPassword"
                    type="password"
                    value={passwordForm.oldPassword}
                    onChange={handlePasswordChange}
                    required
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="newPassword">Yeni Şifre</Label>
                  <Input
                    id="newPassword"
                    name="newPassword"
                    type="password"
                    value={passwordForm.newPassword}
                    onChange={handlePasswordChange}
                    required
                    minLength={6}
                  />
                  <p className="text-sm text-gray-500">En az 6 karakter olmalıdır</p>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="confirmPassword">Yeni Şifre (Tekrar)</Label>
                  <Input
                    id="confirmPassword"
                    name="confirmPassword"
                    type="password"
                    value={passwordForm.confirmPassword}
                    onChange={handlePasswordChange}
                    required
                    minLength={6}
                  />
                </div>

                <Button type="submit" disabled={saving}>
                  {saving ? 'Kaydediliyor...' : 'Şifreyi Değiştir'}
                </Button>
              </form>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Doğrulama */}
        <TabsContent value="verification">
          <Card>
            <CardHeader>
              <CardTitle>Hesap Doğrulama</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center justify-between p-4 border rounded-lg">
                <div>
                  <h3 className="font-semibold">e-Devlet Doğrulaması</h3>
                  <p className="text-sm text-gray-600">
                    {profile?.isEDevletVerified
                      ? `Doğrulandı: ${new Date(profile.eDevletVerifiedAt).toLocaleDateString('tr-TR')}`
                      : 'Henüz doğrulanmadı'}
                  </p>
                </div>
                <Badge variant={profile?.isEDevletVerified ? 'default' : 'secondary'}>
                  {profile?.isEDevletVerified ? '✓ Doğrulandı' : '⚠ Bekliyor'}
                </Badge>
              </div>

              {!profile?.isEDevletVerified && (
                <Button onClick={handleEDevletVerification} className="w-full">
                  e-Devlet ile Doğrula
                </Button>
              )}

              <div className="flex items-center justify-between p-4 border rounded-lg">
                <div>
                  <h3 className="font-semibold">Hesap Rolü</h3>
                  <p className="text-sm text-gray-600">Mevcut yetki seviyeniz</p>
                </div>
                <Badge variant="outline">{profile?.role}</Badge>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
};

export default ProfilePage;
