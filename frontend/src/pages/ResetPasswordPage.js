import React, { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Label } from '../components/ui/label';
import { Alert, AlertDescription } from '../components/ui/alert';
import { toast } from 'sonner';
import {authAPI} from '../services/api';

const ResetPasswordPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token');

  const [formData, setFormData] = useState({
    newPassword: '',
    confirmPassword: ''
  });
  const [loading, setLoading] = useState(false);
  const [validating, setValidating] = useState(true);
  const [tokenValid, setTokenValid] = useState(false);

  useEffect(() => {
    validateToken();
  }, [token]);

  const validateToken = async () => {
    if (!token) {
      setTokenValid(false);
      setValidating(false);
      return;
    }

    try {
      await authAPI.validateToken(token)
      setTokenValid(true);
    } catch (error) {
      console.error('Token validation error:', error);
      setTokenValid(false);
      toast.error('Geçersiz veya süresi dolmuş link');
    } finally {
      setValidating(false);
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (formData.newPassword !== formData.confirmPassword) {
      toast.error('Şifreler eşleşmiyor');
      return;
    }

    if (formData.newPassword.length < 6) {
      toast.error('Şifre en az 6 karakter olmalıdır');
      return;
    }

    setLoading(true);

    try {
      await authAPI.passwordReset({
        token,
        newPassword: formData.newPassword
      });
      toast.success('Şifreniz başarıyla güncellendi');
      setTimeout(() => navigate('/login'), 2000);
    } catch (error) {
      console.error('Password reset error:', error);
      toast.error(error.response?.data?.error || 'Şifre sıfırlanırken hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  if (validating) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Doğrulanıyor...</p>
        </div>
      </div>
    );
  }

  if (!tokenValid) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
        <Card className="w-full max-w-md">
          <CardHeader className="text-center">
            <div className="mx-auto w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mb-4">
              <span className="text-3xl">❌</span>
            </div>
            <CardTitle className="text-2xl">Geçersiz Link</CardTitle>
            <CardDescription>
              Bu şifre sıfırlama linki geçersiz veya süresi dolmuş
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <Alert variant="destructive">
              <AlertDescription>
                Lütfen yeni bir şifre sıfırlama talebi oluşturun
              </AlertDescription>
            </Alert>
            <Button onClick={() => navigate('/forgot-password')} className="w-full">
              Yeni Şifre Sıfırlama Talebi
            </Button>
            <Button onClick={() => navigate('/login')} variant="outline" className="w-full">
              Giriş Sayfasına Dön
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <div className="mx-auto w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mb-4">
            <span className="text-3xl">🔒</span>
          </div>
          <CardTitle className="text-2xl">Yeni Şifre Belirle</CardTitle>
          <CardDescription>
            Hesabınız için yeni bir şifre oluşturun
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="newPassword">Yeni Şifre</Label>
              <Input
                id="newPassword"
                name="newPassword"
                type="password"
                placeholder="••••••••"
                value={formData.newPassword}
                onChange={handleChange}
                required
                minLength={6}
                disabled={loading}
              />
              <p className="text-sm text-gray-500">En az 6 karakter olmalıdır</p>
            </div>

            <div className="space-y-2">
              <Label htmlFor="confirmPassword">Yeni Şifre (Tekrar)</Label>
              <Input
                id="confirmPassword"
                name="confirmPassword"
                type="password"
                placeholder="••••••••"
                value={formData.confirmPassword}
                onChange={handleChange}
                required
                minLength={6}
                disabled={loading}
              />
            </div>

            <Button type="submit" className="w-full" disabled={loading}>
              {loading ? 'Kaydediliyor...' : 'Şifreyi Güncelle'}
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};

export default ResetPasswordPage;
