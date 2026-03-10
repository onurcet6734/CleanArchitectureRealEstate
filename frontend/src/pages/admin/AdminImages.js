import React, { useState, useEffect } from 'react';
import { flatImagesAPI } from '../../services/api';
import { 
  Image as ImageIcon, 
  Loader2, 
  Search, 
  Edit, 
  Trash2,
  X,
  Save,
  Eye,
  Star
} from 'lucide-react';
import { toast } from 'sonner';
import { Link } from 'react-router-dom';

const AdminImages = () => {
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [editForm, setEditForm] = useState({});
  const [deletingId, setDeletingId] = useState(null);
  const [filterCover, setFilterCover] = useState('all'); // all, cover, notCover

  useEffect(() => {
    fetchImages();
  }, []);

  const fetchImages = async () => {
    setLoading(true);
    try {
      const data = await flatImagesAPI.getFlatImages();
      const imagesArray = Array.isArray(data) ? data : [];
      setImages(imagesArray);
    } catch (error) {
      console.error('Resimler yüklenirken hata:', error);
      toast.error('Resimler yüklenemedi');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (image) => {
    setEditingId(image.id);
    setEditForm({
      url: image.url || '',
      isCover: image.isCover || false,
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
        url: editForm.url,
        isCover: editForm.isCover,
      };
      
      await flatImagesAPI.updateFlatImage(id, updateData);
      toast.success('Resim güncellendi');
      setEditingId(null);
      fetchImages();
    } catch (error) {
      console.error('Resim güncellenirken hata:', error);
      toast.error('Resim güncellenemedi');
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Bu resmi silmek istediğinize emin misiniz?')) {
      return;
    }

    setDeletingId(id);
    try {
      // API'de delete endpoint'i yok, bu yüzden sadece uyarı gösteriyoruz
      toast.error('Resim silme özelliği henüz mevcut değil');
      // await flatImagesAPI.deleteFlatImage(id);
      // toast.success('Resim silindi');
      // fetchImages();
    } catch (error) {
      console.error('Resim silinirken hata:', error);
      toast.error('Resim silinemedi');
    } finally {
      setDeletingId(null);
    }
  };

  const filteredImages = images.filter((image) => {
    const matchesSearch = 
      (image.url || '').toLowerCase().includes(searchTerm.toLowerCase()) ||
      (image.flat?.title || '').toLowerCase().includes(searchTerm.toLowerCase()) ||
      (image.flat?.city || '').toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesFilter = 
      filterCover === 'all' ||
      (filterCover === 'cover' && image.isCover) ||
      (filterCover === 'notCover' && !image.isCover);
    
    return matchesSearch && matchesFilter;
  });

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
      <div>
        <h1 className="text-3xl font-bold text-gray-800">Resim Yönetimi</h1>
        <p className="text-gray-600 mt-2">Tüm resimleri görüntüleyin, düzenleyin ve yönetin</p>
      </div>

      {/* Filters */}
      <div className="bg-white rounded-lg shadow-md p-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
            <input
              type="text"
              placeholder="Resim ara (URL, ilan başlığı, şehir)..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
            />
          </div>
          <div>
            <select
              value={filterCover}
              onChange={(e) => setFilterCover(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
            >
              <option value="all">Tüm Resimler</option>
              <option value="cover">Sadece Kapak Resimleri</option>
              <option value="notCover">Kapak Olmayan Resimler</option>
            </select>
          </div>
        </div>
      </div>

      {/* Images Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredImages.length === 0 ? (
          <div className="col-span-full text-center py-12 text-gray-500">
            {searchTerm || filterCover !== 'all' 
              ? 'Arama sonucu bulunamadı' 
              : 'Henüz resim bulunmamaktadır'}
          </div>
        ) : (
          filteredImages.map((image) => (
            <div
              key={image.id}
              className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition"
            >
              {/* Image Preview */}
              <div className="relative h-48 bg-gray-100">
                {editingId === image.id ? (
                  <div className="p-4 h-full flex items-center justify-center bg-gray-50">
                    <input
                      type="url"
                      value={editForm.url}
                      onChange={(e) => setEditForm({ ...editForm, url: e.target.value })}
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm"
                      placeholder="Resim URL"
                    />
                  </div>
                ) : (
                  <>
                    <img
                      src={image.url || 'https://via.placeholder.com/400x300?text=No+Image'}
                      alt={image.flat?.title || 'Resim'}
                      className="w-full h-full object-cover"
                      onError={(e) => {
                        e.target.src = 'https://via.placeholder.com/400x300?text=Image+Error';
                      }}
                    />
                    {image.isCover && (
                      <div className="absolute top-2 right-2 bg-yellow-500 text-white px-2 py-1 rounded-full text-xs font-semibold flex items-center space-x-1">
                        <Star className="w-3 h-3 fill-current" />
                        <span>Kapak</span>
                      </div>
                    )}
                  </>
                )}
              </div>

              {/* Image Info */}
              <div className="p-4">
                {editingId === image.id ? (
                  <div className="space-y-3">
                    <div className="flex items-center space-x-2">
                      <input
                        type="checkbox"
                        checked={editForm.isCover}
                        onChange={(e) => setEditForm({ ...editForm, isCover: e.target.checked })}
                        className="w-4 h-4 text-emerald-600 rounded focus:ring-emerald-500"
                      />
                      <label className="text-sm text-gray-700">Kapak Resmi</label>
                    </div>
                    <div className="flex space-x-2">
                      <button
                        onClick={() => handleSaveEdit(image.id)}
                        className="flex-1 bg-emerald-600 text-white px-3 py-2 rounded text-sm hover:bg-emerald-700 transition flex items-center justify-center space-x-1"
                      >
                        <Save className="w-4 h-4" />
                        <span>Kaydet</span>
                      </button>
                      <button
                        onClick={handleCancelEdit}
                        className="flex-1 bg-gray-200 text-gray-700 px-3 py-2 rounded text-sm hover:bg-gray-300 transition flex items-center justify-center space-x-1"
                      >
                        <X className="w-4 h-4" />
                        <span>İptal</span>
                      </button>
                    </div>
                  </div>
                ) : (
                  <>
                    <div className="mb-2">
                      <p className="text-xs text-gray-500">İlan ID: {image.flatId}</p>
                      {image.flat && (
                        <Link
                          to={`/property/${image.flat.id}`}
                          className="text-sm font-medium text-emerald-600 hover:text-emerald-700 mt-1 block"
                        >
                          {image.flat.title || 'Başlıksız İlan'}
                        </Link>
                      )}
                      {image.flat && (
                        <p className="text-xs text-gray-500 mt-1">
                          {image.flat.city || ''} / {image.flat.district || ''}
                        </p>
                      )}
                    </div>
                    <div className="text-xs text-gray-500 mb-3 break-all">
                      {image.url?.substring(0, 50)}...
                    </div>
                    <div className="flex items-center justify-between">
                      <div className="flex items-center space-x-2">
                        <button
                          onClick={() => handleEdit(image)}
                          className="p-2 text-emerald-600 hover:bg-emerald-50 rounded transition"
                          title="Düzenle"
                        >
                          <Edit className="w-4 h-4" />
                        </button>
                        <button
                          onClick={() => handleDelete(image.id)}
                          disabled={deletingId === image.id}
                          className="p-2 text-red-600 hover:bg-red-50 rounded transition disabled:opacity-50"
                          title="Sil"
                        >
                          {deletingId === image.id ? (
                            <Loader2 className="w-4 h-4 animate-spin" />
                          ) : (
                            <Trash2 className="w-4 h-4" />
                          )}
                        </button>
                        {image.flat && (
                          <Link
                            to={`/property/${image.flat.id}`}
                            className="p-2 text-blue-600 hover:bg-blue-50 rounded transition"
                            title="İlanı Görüntüle"
                          >
                            <Eye className="w-4 h-4" />
                          </Link>
                        )}
                      </div>
                    </div>
                  </>
                )}
              </div>
            </div>
          ))
        )}
      </div>

      {/* Stats */}
      <div className="bg-white rounded-lg shadow-md p-4">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 text-sm">
          <div>
            <span className="text-gray-600">Toplam Resim: </span>
            <span className="font-semibold">{filteredImages.length}</span>
          </div>
          <div>
            <span className="text-gray-600">Kapak Resimleri: </span>
            <span className="font-semibold">
              {filteredImages.filter(img => img.isCover).length}
            </span>
          </div>
          <div>
            <span className="text-gray-600">Diğer Resimler: </span>
            <span className="font-semibold">
              {filteredImages.filter(img => !img.isCover).length}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminImages;
