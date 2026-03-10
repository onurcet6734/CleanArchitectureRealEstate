import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Label } from '../components/ui/label';
import { Alert, AlertDescription } from '../components/ui/alert';
import { toast } from 'sonner';
import {authAPI} from '../services/api';

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      await authAPI.forgotPassword(email)
      setSuccess(true);
      toast.success('Şifre sıfırlama linki e-posta adresinize gönderildi');
    } catch (error) {
      console.error('Forgot password error:', error);
      // Güvenlik için her zaman başarılı mesajı göster
      setSuccess(true);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <div className="mx-auto w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mb-4">
            <span className="text-3xl">🔑</span>
          </div>
          <CardTitle className="text-2xl">Şifremi Unuttum</CardTitle>
          <CardDescription>
            E-posta adresinizi girin, size şifre sıfırlama linki gönderelim
          </CardDescription>
        </CardHeader>
        <CardContent>
          {success ? (
            <div className="space-y-4">
              <Alert>
                <AlertDescription>
                  <strong>✓ E-posta Gönderildi</strong>
                  <p className="mt-2 text-sm">
                    Eğer e-posta adresiniz sistemde kayıtlı ise, şifre sıfırlama linki gönderilmiştir.
                    Lütfen e-posta kutunuzu kontrol edin.
                  </p>
                </AlertDescription>
              </Alert>

              <div className="text-center space-y-2">
                <p className="text-sm text-gray-600">E-posta gelmediyse spam klasörünü kontrol edin</p>
                <Link to="/login">
                  <Button variant="outline" className="w-full">
                    Giriş Sayfasına Dön
                  </Button>
                </Link>
              </div>
            </div>
          ) : (
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="email">E-posta Adresi</Label>
                <Input
                  id="email"
                  name="email"
                  type="email"
                  placeholder="ornek@email.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  disabled={loading}
                />
              </div>

              <Button type="submit" className="w-full" disabled={loading}>
                {loading ? 'Gönderiliyor...' : 'Şifre Sıfırlama Linki Gönder'}
              </Button>

              <div className="text-center pt-4 border-t">
                <Link to="/login" className="text-sm text-blue-600 hover:underline">
                  ← Giriş sayfasına dön
                </Link>
              </div>
            </form>
          )}
        </CardContent>
      </Card>
    </div>
  );
};

export default ForgotPasswordPage;
