import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { flatsAPI, flatImagesAPI } from '../../services/api';
import { Home, Image as ImageIcon, TrendingUp, Loader2, ArrowRight } from 'lucide-react';
import { toast } from 'sonner';

const AdminDashboard = () => {
  const [stats, setStats] = useState({
    totalFlats: 0,
    totalImages: 0,
    totalValue: 0,
    loading: true,
  });
  const [recentFlats, setRecentFlats] = useState([]);
  const [loadingRecent, setLoadingRecent] = useState(true);

  useEffect(() => {
    fetchStats();
    fetchRecentFlats();
  }, []);

  const fetchStats = async () => {
    try {
      const [flats, images] = await Promise.all([
        flatsAPI.getFlats(),
        flatImagesAPI.getFlatImages(),
      ]);

      const flatsArray = Array.isArray(flats) ? flats : [];
      const imagesArray = Array.isArray(images) ? images : [];

      const totalValue = flatsArray.reduce((sum, flat) => {
        return sum + (parseFloat(flat.price) || 0);
      }, 0);

      setStats({
        totalFlats: flatsArray.length,
        totalImages: imagesArray.length,
        totalValue: totalValue,
        loading: false,
      });
    } catch (error) {
      console.error('İstatistikler yüklenirken hata:', error);
      toast.error('İstatistikler yüklenemedi');
      setStats((prev) => ({ ...prev, loading: false }));
    }
  };

  const fetchRecentFlats = async () => {
    setLoadingRecent(true);
    try {
      const flats = await flatsAPI.getFlats({ Limit: 5 });
      const flatsArray = Array.isArray(flats) ? flats : [];
      setRecentFlats(flatsArray.slice(0, 5));
    } catch (error) {
      console.error('Son ilanlar yüklenirken hata:', error);
      toast.error('Son ilanlar yüklenemedi');
    } finally {
      setLoadingRecent(false);
    }
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY',
      maximumFractionDigits: 0,
    }).format(amount);
  };

  if (stats.loading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <Loader2 className="w-8 h-8 animate-spin text-emerald-600" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-gray-800">Dashboard</h1>
        <p className="text-gray-600 mt-2">Genel bakış ve istatistikler</p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white rounded-xl shadow-md p-6 border-l-4 border-emerald-500">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-600 text-sm font-medium">Toplam İlan</p>
              <p className="text-3xl font-bold text-gray-800 mt-2">{stats.totalFlats}</p>
            </div>
            <div className="w-12 h-12 bg-emerald-100 rounded-lg flex items-center justify-center">
              <Home className="w-6 h-6 text-emerald-600" />
            </div>
          </div>
          <Link
            to="/admin/flats"
            className="mt-4 inline-flex items-center text-sm text-emerald-600 hover:text-emerald-700 font-medium"
          >
            Tümünü Gör <ArrowRight className="w-4 h-4 ml-1" />
          </Link>
        </div>

        <div className="bg-white rounded-xl shadow-md p-6 border-l-4 border-blue-500">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-600 text-sm font-medium">Toplam Resim</p>
              <p className="text-3xl font-bold text-gray-800 mt-2">{stats.totalImages}</p>
            </div>
            <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
              <ImageIcon className="w-6 h-6 text-blue-600" />
            </div>
          </div>
          <Link
            to="/admin/images"
            className="mt-4 inline-flex items-center text-sm text-blue-600 hover:text-blue-700 font-medium"
          >
            Tümünü Gör <ArrowRight className="w-4 h-4 ml-1" />
          </Link>
        </div>

        <div className="bg-white rounded-xl shadow-md p-6 border-l-4 border-purple-500">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-600 text-sm font-medium">Toplam Değer</p>
              <p className="text-3xl font-bold text-gray-800 mt-2">
                {formatCurrency(stats.totalValue)}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Recent Flats */}
      <div className="bg-white rounded-xl shadow-md p-6">
        <div className="flex items-center justify-between mb-6">
          <div>
            <h2 className="text-xl font-bold text-gray-800">Son İlanlar</h2>
            <p className="text-gray-600 text-sm mt-1">En son eklenen ilanlar</p>
          </div>
          <Link
            to="/admin/flats"
            className="text-emerald-600 hover:text-emerald-700 font-medium text-sm flex items-center"
          >
            Tümünü Gör <ArrowRight className="w-4 h-4 ml-1" />
          </Link>
        </div>

        {loadingRecent ? (
          <div className="flex items-center justify-center py-8">
            <Loader2 className="w-6 h-6 animate-spin text-emerald-600" />
          </div>
        ) : recentFlats.length === 0 ? (
          <div className="text-center py-8 text-gray-500">
            Henüz ilan bulunmamaktadır.
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b">
                  <th className="text-left py-3 px-4 text-sm font-semibold text-gray-700">ID</th>
                  <th className="text-left py-3 px-4 text-sm font-semibold text-gray-700">Başlık</th>
                  <th className="text-left py-3 px-4 text-sm font-semibold text-gray-700">Şehir</th>
                  <th className="text-left py-3 px-4 text-sm font-semibold text-gray-700">Fiyat</th>
                  <th className="text-left py-3 px-4 text-sm font-semibold text-gray-700">Durum</th>
                </tr>
              </thead>
              <tbody>
                {recentFlats.map((flat) => (
                  <tr key={flat.id} className="border-b hover:bg-gray-50">
                    <td className="py-3 px-4 text-sm text-gray-600">{flat.id}</td>
                    <td className="py-3 px-4 text-sm font-medium text-gray-800">
                      {flat.title || 'Başlıksız'}
                    </td>
                    <td className="py-3 px-4 text-sm text-gray-600">
                      {flat.city || '-'} / {flat.district || '-'}
                    </td>
                    <td className="py-3 px-4 text-sm text-gray-600">
                      {formatCurrency(flat.price || 0)}
                    </td>
                    <td className="py-3 px-4">
                      <span
                        className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                          flat.status === 'Satılık'
                            ? 'bg-green-100 text-green-800'
                            : flat.status === 'Kiralık'
                            ? 'bg-blue-100 text-blue-800'
                            : 'bg-gray-100 text-gray-800'
                        }`}
                      >
                        {flat.status || '-'}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminDashboard;
