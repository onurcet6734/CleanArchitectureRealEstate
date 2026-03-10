import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Label } from '../components/ui/label';
import { Textarea } from '../components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../components/ui/select';
import { toast } from 'sonner';
import {flatsAPI} from '../services/api';

const CreatePropertyPage = () => {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);

  const [uploadedImages, setUploadedImages] = useState([]);
  const [selectedFiles, setSelectedFiles] = useState([]);

  const [formData, setFormData] = useState({
    title: '',
    description: '',
    price: '',
    currency: 'TRY',
    city: '',
    district: '',
    addressLine: '',
    type: 'Apartment',
    status: 'ForSale'
  });

  const handleFileSelect = (e) => {
    const files = Array.from(e.target.files);
    setSelectedFiles(files);
  };
  
  const handleUploadImages = async () => {
    if (selectedFiles.length === 0) {
      setStep(2);
      return;
    }
    setLoading(true);
    try {
      const imageFormData = new FormData();
      selectedFiles.forEach((file) => {
        imageFormData.append("files", file);
      });
      const data = await flatsAPI.uploadImages(imageFormData);
      setUploadedImages(data.images);
      toast.success(`${data.count} resim yüklendi` );
      setStep(2);
    } catch (error) {

      console.error("Image upload error:", error);
      toast.error(
        error.response?.data?.error ||
        "Resimler yüklenirken hata oluştu"
      );
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSelectChange = (name, value) => {
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const payload = {
        ...formData,
        price: parseFloat(formData.price),
        imageUrls: uploadedImages.map(img => img.url)
      };
      const data = await flatsAPI.createFlat(payload);
      toast.success('İlan başarıyla oluşturuldu!');
      navigate(`/property/${data.id}`);
    } catch (error) {
      console.error('Property creation error:', error);
      toast.error(error.response?.data?.error || 'İlan oluşturulurken hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8 max-w-4xl">
      <h1 className="text-3xl font-bold mb-6">Yeni İlan Oluştur</h1>

      <div className="flex items-center justify-center mb-8">
        <div className="flex items-center">
          <div className={`w-10 h-10 rounded-full flex items-center justify-center ${
            step >= 1 ? 'bg-blue-600 text-white' : 'bg-gray-300'
          }`}>
            1
          </div>
          <div className={`w-24 h-1 ${step >= 2 ? 'bg-blue-600' : 'bg-gray-300'}`}></div>
          <div className={`w-10 h-10 rounded-full flex items-center justify-center ${
            step >= 2 ? 'bg-blue-600 text-white' : 'bg-gray-300'
          }`}>
            2
          </div>
        </div>
      </div>

      {step === 1 && (
        <Card>
          <CardHeader>
            <CardTitle>1. Adım: Resim Yükleme (Opsiyonel)</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="border-2 border-dashed border-gray-300 rounded-lg p-8 text-center">
              <input
                type="file"
                multiple
                accept="image/*"
                onChange={handleFileSelect}
                className="hidden"
                id="file-upload"
              />
              <label htmlFor="file-upload" className="cursor-pointer">
                <div className="text-6xl mb-4">📷</div>
                <p className="text-lg font-semibold mb-2">Resimleri Seçin</p>
                <p className="text-sm text-gray-600">
                  Birden fazla resim seçebilirsiniz (JPG, PNG)
                </p>
                <Button type="button" variant="outline" className="mt-4">
                  Dosya Seç
                </Button>
              </label>
            </div>

            {selectedFiles.length > 0 && (
              <div>
                <p className="font-semibold mb-2">Seçilen Resimler ({selectedFiles.length}):</p>
                <div className="grid grid-cols-4 gap-2">
                  {Array.from(selectedFiles).map((file, index) => (
                    <div key={index} className="border rounded p-2">
                      <img
                        src={URL.createObjectURL(file)}
                        alt={`Preview ${index + 1}`}
                        className="w-full h-24 object-cover rounded"
                      />
                      <p className="text-xs text-gray-600 mt-1 truncate">{file.name}</p>
                    </div>
                  ))}
                </div>
              </div>
            )}

            <div className="flex gap-2">
              <Button
                onClick={handleUploadImages}
                disabled={loading}
                className="flex-1"
              >
                {loading ? 'Yükleniyor...' : selectedFiles.length > 0 ? 'Resimleri Yükle ve Devam Et' : 'Resimsiz Devam Et'}
              </Button>
            </div>

            <p className="text-sm text-gray-500 text-center">
              💡 İpucu: Resimleri daha sonra ilan düzenleme sayfasından ekleyebilirsiniz
            </p>
          </CardContent>
        </Card>
      )}

      {step === 2 && (
        <Card>
          <CardHeader>
            <CardTitle>2. Adım: İlan Detayları</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit} className="space-y-4">
              {uploadedImages.length > 0 && (
                <div className="bg-green-50 border border-green-200 rounded-lg p-4">
                  <p className="text-green-800 font-semibold">
                    ✓ {uploadedImages.length} resim yüklendi
                  </p>
                </div>
              )}

              <div className="space-y-2">
                <Label htmlFor="title">İlan Başlığı *</Label>
                <Input
                  id="title"
                  name="title"
                  value={formData.title}
                  onChange={handleChange}
                  placeholder="Örn: Merkezi Konumda 3+1 Daire"
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="description">Açıklama *</Label>
                <Textarea
                  id="description"
                  name="description"
                  value={formData.description}
                  onChange={handleChange}
                  placeholder="İlan detaylarını yazın..."
                  rows={5}
                  required
                />
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="price">Fiyat *</Label>
                  <Input
                    id="price"
                    name="price"
                    type="number"
                    value={formData.price}
                    onChange={handleChange}
                    placeholder="0"
                    required
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="currency">Para Birimi *</Label>
                  <Select value={formData.currency} onValueChange={(value) => handleSelectChange('currency', value)}>
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="TRY">TRY (₺)</SelectItem>
                      <SelectItem value="USD">USD ($)</SelectItem>
                      <SelectItem value="EUR">EUR (€)</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="type">İlan Tipi *</Label>
                  <Select value={formData.type} onValueChange={(value) => handleSelectChange('type', value)}>
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="Apartment">Daire</SelectItem>
                      <SelectItem value="Villa">Villa</SelectItem>
                      <SelectItem value="Office">Ofis</SelectItem>
                      <SelectItem value="Land">Arsa</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="status">Durum *</Label>
                  <Select value={formData.status} onValueChange={(value) => handleSelectChange('status', value)}>
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="ForSale">Satılık</SelectItem>
                      <SelectItem value="ForRent">Kiralık</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="city">Şehir *</Label>
                  <Input
                    id="city"
                    name="city"
                    value={formData.city}
                    onChange={handleChange}
                    placeholder="İstanbul"
                    required
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="district">İlçe *</Label>
                  <Input
                    id="district"
                    name="district"
                    value={formData.district}
                    onChange={handleChange}
                    placeholder="Kadıköy"
                    required
                  />
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="addressLine">Adres *</Label>
                <Input
                  id="addressLine"
                  name="addressLine"
                  value={formData.addressLine}
                  onChange={handleChange}
                  placeholder="Mahalle, Sokak, No"
                  required
                />
              </div>

              <div className="flex gap-2 pt-4">
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => setStep(1)}
                  disabled={loading}
                >
                  ← Geri
                </Button>
                <Button type="submit" disabled={loading} className="flex-1">
                  {loading ? 'Oluşturuluyor...' : 'İlanı Oluştur'}
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      )}
    </div>
  );
};

export default CreatePropertyPage;
