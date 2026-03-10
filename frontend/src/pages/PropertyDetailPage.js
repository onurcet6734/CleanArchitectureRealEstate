import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { flatsAPI, flatImagesAPI } from '../services/api';
import {
  MapPin,
  Home,
  ArrowLeft,
  Loader2,
  ChevronLeft,
  ChevronRight
} from 'lucide-react';
import { toast } from 'sonner';

const BACKEND_URL = process.env.REACT_APP_BACKEND_URL;
const FALLBACK_IMAGE =
  'https://images.unsplash.com/photo-1560518883-ce09059eeffa?w=800&h=600&fit=crop';

const PropertyDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [property, setProperty] = useState(null);
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  const fetchPropertyDetails = useCallback(async () => {
    setLoading(true);
    try {
      const imageData = await flatImagesAPI.getFlatImages({});

      if (Array.isArray(imageData)) {
        const flatImages = imageData.filter(
          img => img.flat && img.flat.id === Number(id)
        );

        if (flatImages.length > 0) {
          setProperty(flatImages[0].flat);
          setImages(
            flatImages.map(img => ({
              url: img.url ? `${BACKEND_URL}${img.url}` : null,
              isCover: img.isCover
            }))
          );
          setCurrentImageIndex(0);
          return;
        }
      }

      // Fallback
      const flatData = await flatsAPI.getFlatById(id);
      setProperty(flatData);
      setImages([{ url: FALLBACK_IMAGE }]);
      setCurrentImageIndex(0);
    } catch (error) {
      toast.error('İlan bulunamadı');
      navigate('/');
    } finally {
      setLoading(false);
    }
  }, [id, navigate]);

  useEffect(() => {
    fetchPropertyDetails();
  }, [fetchPropertyDetails]);

  const nextImage = () => {
    setCurrentImageIndex(prev => (prev + 1) % images.length);
  };

  const prevImage = () => {
    setCurrentImageIndex(prev => (prev - 1 + images.length) % images.length);
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <Loader2 className="w-12 h-12 animate-spin" />
      </div>
    );
  }

  if (!property) return null;

  const owner = property.user;

  return (
    <div className="min-h-screen py-8 px-4">
      <div className="max-w-6xl mx-auto">
        <button
          onClick={() => navigate('/')}
          className="flex items-center space-x-2 mb-6"
        >
          <ArrowLeft className="w-5 h-5" />
          <span>Geri Dön</span>
        </button>

        <div className="bg-white rounded-2xl shadow-xl overflow-hidden">
          {/* Images */}
          <div className="relative h-96 bg-gray-200">
            <img
              src={images[currentImageIndex]?.url}
              alt={property.title}
              className="w-full h-full object-cover"
            />

            {images.length > 1 && (
              <>
                <button
                  onClick={prevImage}
                  className="absolute left-4 top-1/2 -translate-y-1/2 bg-white p-2 rounded-full"
                >
                  <ChevronLeft />
                </button>
                <button
                  onClick={nextImage}
                  className="absolute right-4 top-1/2 -translate-y-1/2 bg-white p-2 rounded-full"
                >
                  <ChevronRight />
                </button>
              </>
            )}
          </div>

          <div className="p-8 space-y-8">
            {/* Title */}
            <div>
              <p className="text-sm text-gray-500 mb-1">İlan Başlığı</p>
              <h1 className="text-3xl font-bold text-gray-900">
                {property.title}
              </h1>
            </div>

            {/* Price */}
            <div>
              <p className="text-sm text-gray-500 mb-1">Fiyat</p>
              <div className="flex items-center text-emerald-700 font-bold text-3xl">
                <span>{property.price?.toLocaleString()}</span>
                <span className="text-lg ml-2">{property.currency}</span>
              </div>
            </div>

            {/* Location & Type */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

              {/* Address */}
              <div className="flex items-start space-x-3">
                <MapPin className="text-gray-500 mt-1" />
                <div>
                  <p className="text-sm text-gray-500 mb-1">Adres</p>
                  <p className="text-gray-900">
                    {property.city} / {property.district}
                  </p>
                  <p className="text-gray-600 text-sm">
                    {property.addressLine}
                  </p>
                </div>
              </div>

              {/* Type */}
              <div className="flex items-start space-x-3">
                <Home className="text-gray-500 mt-1" />
                <div>
                  <p className="text-sm text-gray-500 mb-1">İlan Türü</p>
                  <p className="text-gray-900 font-medium">
                    {property.type}
                  </p>
                </div>
              </div>

            </div>

            {/* Description Section */}
            <div className="border-t border-gray-200 mt-10 pt-8">
              <div className="bg-gray-50 rounded-2xl p-6 md:p-8">
                <h3 className="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  📝 İlan Açıklaması
                </h3>

                <p className="text-gray-700 leading-relaxed whitespace-pre-line">
                  {property.description || 'Bu ilan için açıklama girilmemiş.'}
                </p>
              </div>
            </div>

            {/* Contact Section */}
            <div className="border-t border-gray-200 mt-10 pt-8">
              <div className="bg-white rounded-2xl shadow-md p-6 md:p-8 flex flex-col md:flex-row gap-6 items-start">

                {/* Avatar */}
                <div className="flex-shrink-0">
                  <div className="w-20 h-20 rounded-full bg-emerald-600 text-white flex items-center justify-center text-3xl font-bold">
                    {owner?.fullName?.charAt(0)}
                  </div>
                </div>

                {/* Info */}
                <div className="flex-1">
                  <h3 className="text-2xl font-bold text-gray-900 mb-1">
                    {owner?.fullName}
                  </h3>
                  <p className="text-gray-500 mb-4">@{owner?.username}</p>

                  <div className="space-y-3 text-gray-800">
                    <div className="flex items-center gap-3">
                      <span className="text-emerald-600">📞</span>
                      <a
                        href={`tel:${owner?.phoneNumber}`}
                        className="hover:text-emerald-700 font-medium"
                      >
                        {owner?.phoneNumber}
                      </a>
                    </div>

                    <div className="flex items-center gap-3">
                      <span className="text-emerald-600">✉️</span>
                      <a
                        href={`mailto:${owner?.email}`}
                        className="hover:text-emerald-700 font-medium"
                      >
                        {owner?.email}
                      </a>
                    </div>
                  </div>
                </div>

                {/* Actions */}
                <div className="flex flex-col gap-3 w-full md:w-auto">
                  <a
                    href={`tel:${owner?.phoneNumber}`}
                    className="bg-gradient-to-r from-emerald-600 to-green-600 text-white px-6 py-3 rounded-xl font-semibold text-center hover:from-emerald-700 hover:to-green-700 transition"
                  >
                    📞 Hemen Ara
                  </a>

                  <a
                    href={`mailto:${owner?.email}`}
                    className="border border-emerald-600 text-emerald-700 px-6 py-3 rounded-xl font-semibold text-center hover:bg-emerald-50 transition"
                  >
                    ✉️ E-posta Gönder
                  </a>
                </div>
              </div>
            </div>

          </div>
        </div>
      </div>
    </div>
  );
};

export default PropertyDetailPage;