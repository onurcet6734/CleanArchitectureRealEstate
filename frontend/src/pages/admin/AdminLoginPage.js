import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Label } from '../../components/ui/label';
import { Alert, AlertDescription } from '../../components/ui/alert';
import { toast } from 'sonner';
import {authAPI} from '../../services/api';

const AdminLoginPage = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    username: '',
    password: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const response = await authAPI.login(formData);
      const { accessToken } = response.data;

      // Token'ı decode et ve role kontrolü yap
      const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));
      const userRole = tokenPayload.role || tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      if (userRole !== 'Admin') {
        setError('Bu sayfaya erişim yetkiniz yok. Sadece Admin kullanıcıları giriş yapabilir.');
        return;
      }

      // Token'ı kaydet
      localStorage.setItem('token', accessToken);
      localStorage.setItem('userRole', userRole);

      toast.success('Admin girişi başarılı!');
      navigate('/admin/dashboard');
    } catch (error) {
      console.error('Admin login error:', error);
      if (error.response?.status === 401) {
        setError('Kullanıcı adı veya şifre hatalı');
      } else {
        setError('Giriş yapılırken bir hata oluştu');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-900 via-gray-800 to-gray-900 px-4">
      <Card className="w-full max-w-md shadow-2xl">
        <CardHeader className="space-y-1 text-center">
          <div className="mx-auto w-16 h-16 bg-red-600 rounded-full flex items-center justify-center mb-4">
            <span className="text-3xl">🔐</span>
          </div>
          <CardTitle className="text-3xl font-bold">Admin Paneli</CardTitle>
          <CardDescription>
            Yönetici hesabınızla giriş yapın
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            {error && (
              <Alert variant="destructive">
                <AlertDescription>{error}</AlertDescription>
              </Alert>
            )}

            <div className="space-y-2">
              <Label htmlFor="username">Kullanıcı Adı</Label>
              <Input
                id="username"
                name="username"
                type="text"
                placeholder="admin"
                value={formData.username}
                onChange={handleChange}
                required
                disabled={loading}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="password">Şifre</Label>
              <Input
                id="password"
                name="password"
                type="password"
                placeholder="••••••••"
                value={formData.password}
                onChange={handleChange}
                required
                disabled={loading}
              />
            </div>

            <Button
              type="submit"
              className="w-full bg-red-600 hover:bg-red-700"
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="animate-spin mr-2">⏳</span>
                  Giriş yapılıyor...
                </>
              ) : (
                'Admin Girişi'
              )}
            </Button>

            <div className="text-center pt-4 border-t">
              <Button
                type="button"
                variant="link"
                onClick={() => navigate('/login')}
                className="text-sm text-gray-600"
              >
                Normal kullanıcı girişi için tıklayın
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};

export default AdminLoginPage;
