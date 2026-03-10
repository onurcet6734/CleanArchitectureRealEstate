import React, { useState, useEffect } from 'react';
import { flatsAPI } from '../../services/api';
import { 
  Home, 
  Loader2, 
  Search, 
  Edit, 
  Trash2, 
  Plus,
  X,
  Save,
  Eye
} from 'lucide-react';
import { toast } from 'sonner';
import { Link } from 'react-router-dom';

const AdminFlats = () => {
  const [flats, setFlats] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [editForm, setEditForm] = useState({});
  const [deletingId, setDeletingId] = useState(null);

  useEffect(() => {
    fetchFlats();
  }, []);

  const fetchFlats = async () => {
    setLoading(true);
    try {
      const data = await flatsAPI.getFlats();
      const flatsArray = Array.isArray(data) ? data : [];
      setFlats(flatsArray);
    } catch (error) {
      console.error('İlanlar yüklenirken hata:', error);
      toast.error('İlanlar yüklenemedi');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (flat) => {
    setEditingId(flat.id);
    setEditForm({
      title: flat.title || '',
      description: flat.description || '',
      price: flat.price || '',
      currency: flat.currency || 'TRY',
      city: flat.city || '',
      district: flat.district || '',
      addressLine: flat.addressLine || '',
      type: flat.type || '',
      status: flat.status || 'Satılık',
    });
  };

  const handleCancelEdit = () => {
    setEditingId(null);
    setEditForm({});
  };

  const handleSaveEdit = async (id) => {
    try {
      const updateData = {
        id: id,
        ...editForm,
        price: parseFloat(editForm.price),
      };
      
      await flatsAPI.updateFlat(id, updateData);
      toast.success('İlan güncellendi');
      setEditingId(null);
      fetchFlats();
    } catch (error) {
      console.error('İlan güncellenirken hata:', error);
      toast.error('İlan güncellenemedi');
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Bu ilanı silmek istediğinize emin misiniz?')) {
      return;
    }

    setDeletingId(id);
    try {
      await flatsAPI.deleteFlat(id);
      toast.success('İlan silindi' , );
      fetchFlats();
    } catch (error) {
      console.error('İlan silinirken hata:', error);
      toast.error('İlan silinemedi');
    } finally {
      setDeletingId(null);
    }
  };

  const filteredFlats = flats.filter((flat) => {
    const searchLower = searchTerm.toLowerCase();
    return (
      (flat.title || '').toLowerCase().includes(searchLower) ||
      (flat.city || '').toLowerCase().includes(searchLower) ||
      (flat.district || '').toLowerCase().includes(searchLower) ||
      (flat.description || '').toLowerCase().includes(searchLower)
    );
  });

  const formatCurrency = (amount, currency = 'TRY') => {
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: currency,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <Loader2 className="w-8 h-8 animate-spin text-emerald-600" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">İlan Yönetimi</h1>
          <p className="text-gray-600 mt-2">Tüm ilanları görüntüleyin, düzenleyin ve silin</p>
        </div>
        <Link
          to="/create-property"
          className="bg-emerald-600 text-white px-4 py-2 rounded-lg hover:bg-emerald-700 transition flex items-center space-x-2"
        >
          <Plus className="w-5 h-5" />
          <span>Yeni İlan</span>
        </Link>
      </div>

      {/* Search */}
      <div className="bg-white rounded-lg shadow-md p-4">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
          <input
            type="text"
            placeholder="İlan ara (başlık, şehir, ilçe, açıklama)..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
          />
        </div>
      </div>

      {/* Flats Table */}
      <div className="bg-white rounded-lg shadow-md overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">ID</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Başlık</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Konum</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Fiyat</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Durum</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">İşlemler</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {filteredFlats.length === 0 ? (
                <tr>
                  <td colSpan="6" className="px-6 py-8 text-center text-gray-500">
                    {searchTerm ? 'Arama sonucu bulunamadı' : 'Henüz ilan bulunmamaktadır'}
                  </td>
                </tr>
              ) : (
                filteredFlats.map((flat) => (
                  <tr key={flat.id} className="hover:bg-gray-50">
                    {editingId === flat.id ? (
                      <>
                        <td className="px-6 py-4 text-sm text-gray-600">{flat.id}</td>
                        <td className="px-6 py-4">
                          <input
                            type="text"
                            value={editForm.title}
                            onChange={(e) => setEditForm({ ...editForm, title: e.target.value })}
                            className="w-full px-3 py-1 border border-gray-300 rounded text-sm"
                            placeholder="Başlık"
                          />
                        </td>
                        <td className="px-6 py-4">
                          <div className="space-y-1">
                            <input
                              type="text"
                              value={editForm.city}
                              onChange={(e) => setEditForm({ ...editForm, city: e.target.value })}
                              className="w-full px-3 py-1 border border-gray-300 rounded text-sm"
                              placeholder="Şehir"
                            />
                            <input
                              type="text"
                              value={editForm.district}
                              onChange={(e) => setEditForm({ ...editForm, district: e.target.value })}
                              className="w-full px-3 py-1 border border-gray-300 rounded text-sm"
                              placeholder="İlçe"
                            />
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <div className="flex space-x-1">
                            <input
                              type="number"
                              value={editForm.price}
                              onChange={(e) => setEditForm({ ...editForm, price: e.target.value })}
                              className="w-24 px-3 py-1 border border-gray-300 rounded text-sm"
                              placeholder="Fiyat"
                            />
                            <select
                              value={editForm.currency}
                              onChange={(e) => setEditForm({ ...editForm, currency: e.target.value })}
                              className="px-2 py-1 border border-gray-300 rounded text-sm"
                            >
                              <option value="TRY">TRY</option>
                              <option value="USD">USD</option>
                              <option value="EUR">EUR</option>
                            </select>
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <select
                            value={editForm.status}
                            onChange={(e) => setEditForm({ ...editForm, status: e.target.value })}
                            className="w-full px-3 py-1 border border-gray-300 rounded text-sm"
                          >
                            <option value="Satılık">Satılık</option>
                            <option value="Kiralık">Kiralık</option>
                            <option value="Satıldı">Satıldı</option>
                          </select>
                        </td>
                        <td className="px-6 py-4">
                          <div className="flex items-center space-x-2">
                            <button
                              onClick={() => handleSaveEdit(flat.id)}
                              className="p-2 text-green-600 hover:bg-green-50 rounded transition"
                              title="Kaydet"
                            >
                              <Save className="w-4 h-4" />
                            </button>
                            <button
                              onClick={handleCancelEdit}
                              className="p-2 text-red-600 hover:bg-red-50 rounded transition"
                              title="İptal"
                            >
                              <X className="w-4 h-4" />
                            </button>
                          </div>
                        </td>
                      </>
                    ) : (
                      <>
                        <td className="px-6 py-4 text-sm text-gray-600">{flat.id}</td>
                        <td className="px-6 py-4">
                          <div className="font-medium text-gray-800">{flat.title || 'Başlıksız'}</div>
                          <div className="text-xs text-gray-500 mt-1 line-clamp-2">
                            {flat.description || 'Açıklama yok'}
                          </div>
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-600">
                          {flat.city || '-'} / {flat.district || '-'}
                          <div className="text-xs text-gray-500 mt-1">{flat.addressLine || ''}</div>
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-600">
                          {formatCurrency(flat.price || 0, flat.currency || 'TRY')}
                        </td>
                        <td className="px-6 py-4">
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
                        <td className="px-6 py-4">
                          <div className="flex items-center space-x-2">
                            <Link
                              to={`/property/${flat.id}`}
                              className="p-2 text-blue-600 hover:bg-blue-50 rounded transition"
                              title="Görüntüle"
                            >
                              <Eye className="w-4 h-4" />
                            </Link>
                            <button
                              onClick={() => handleEdit(flat)}
                              className="p-2 text-emerald-600 hover:bg-emerald-50 rounded transition"
                              title="Düzenle"
                            >
                              <Edit className="w-4 h-4" />
                            </button>
                            <button
                              onClick={() => handleDelete(flat.id)}
                              disabled={deletingId === flat.id}
                              className="p-2 text-red-600 hover:bg-red-50 rounded transition disabled:opacity-50"
                              title="Sil"
                            >
                              {deletingId === flat.id ? (
                                <Loader2 className="w-4 h-4 animate-spin" />
                              ) : (
                                <Trash2 className="w-4 h-4" />
                              )}
                            </button>
                          </div>
                        </td>
                      </>
                    )}
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Stats */}
      <div className="bg-white rounded-lg shadow-md p-4">
        <p className="text-sm text-gray-600">
          Toplam <span className="font-semibold">{filteredFlats.length}</span> ilan gösteriliyor
        </p>
      </div>
    </div>
  );
};

export default AdminFlats;
