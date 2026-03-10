// LoginPage.js
import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { LogIn, Loader2 } from 'lucide-react';
import { toast } from 'sonner';

const LoginPage = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: '',
  });
  const [loading, setLoading] = useState(false);
  const { login, forgotPassword } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const result = await login(formData.username, formData.password);

      if (result.success) {
        toast.success('Giriş başarılı! Hoş geldiniz.', {duration:1000});
        navigate('/');
      } else {
        toast.error(result.error || 'Giriş başarısız oldu');
      }
    } catch (error) {
      toast.error('Bir hata oluştu. Lütfen tekrar deneyin.');
    } finally {
      setLoading(false);
    }
  };

  const handleForgotPassword = async () => {
    const email = prompt('Lütfen kayıtlı e-posta adresinizi girin:');

    if (!email) return;

    try {
      const result = await forgotPassword(email);

      if (result.success) {
        toast.success('Şifre sıfırlama maili gönderildi!', );
      } else {
        toast.error(result.error || 'İşlem başarısız oldu');
      }
    } catch (error) {
      toast.error('Bir hata oluştu. Lütfen tekrar deneyin.');
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-emerald-50 to-green-50 flex items-center justify-center px-4">
      <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-8">
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-br from-emerald-500 to-green-600 rounded-full mb-4">
            <LogIn className="w-8 h-8 text-white" />
          </div>
          <h2 className="text-3xl font-bold text-gray-800">Giriş Yap</h2>
          <p className="text-gray-600 mt-2">Hesabınıza giriş yapın</p>
        </div>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Kullanıcı Adı
            </label>
            <input
              type="text"
              name="username"
              value={formData.username}
              onChange={handleChange}
              required
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
              placeholder="Kullanıcı adınızı girin"
              data-testid="login-username-input"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Şifre
            </label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
              placeholder="Şifrenizi girin"
              data-testid="login-password-input"
            />
            <div className="text-right mt-2">
              <button
                type="button"
                onClick={handleForgotPassword}
                className="text-sm text-emerald-600 hover:text-emerald-700"
              >
                Şifremi Unuttum?
              </button>
            </div>
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-gradient-to-r from-emerald-600 to-green-600 text-white py-3 rounded-lg font-semibold hover:from-emerald-700 hover:to-green-700 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
            data-testid="login-submit-button"
          >
            {loading ? (
              <>
                <Loader2 className="w-5 h-5 mr-2 animate-spin" />
                Giriş yapılıyor...
              </>
            ) : (
              'Giriş Yap'
            )}
          </button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-gray-600">
            Hesabınız yok mu?{' '}
            <Link to="/register" className="text-emerald-600 hover:text-emerald-700 font-semibold">
              Üye Ol
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
