import React, { useState, useEffect, useCallback } from 'react';
import { flatImagesAPI } from '../services/api';
import PropertyCard from '../components/PropertyCard';
import SearchFilters from '../components/SearchFilters';
import { Loader2 } from 'lucide-react';

const BACKEND_URL = process.env.REACT_APP_BACKEND_URL;

const HomePage = () => {
  const [properties, setProperties] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filters, setFilters] = useState({});

  const fetchProperties = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const cleanFilters = Object.fromEntries(
        Object.entries(filters).filter(([_, value]) => value !== '')
      );

      cleanFilters.IsCover = true;

      const data = await flatImagesAPI.getFlatImages(cleanFilters);

      if (Array.isArray(data)) {
        const flatsWithImages = data.map(item => ({
          ...item.flat,
          imageUrl: item.url ? `${BACKEND_URL}${item.url}` : null,
          imageId: item.id
        }));
        setProperties(flatsWithImages);
      } else {
        setProperties([]);
      }
    } catch (err) {
      setError('İlanlar yüklenemedi.');
      setProperties([]);
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchProperties();
  }, [fetchProperties]);

  const handleFilterChange = (newFilters) => {
    setFilters(newFilters);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-emerald-50 to-green-50">
      <div className="bg-gradient-to-r from-emerald-600 to-green-600 text-white py-16">
        <div className="container mx-auto px-4 text-center">
          <h1 className="text-4xl md:text-5xl font-bold mb-4">
            Hayalinizdeki Evi Bulun
          </h1>
          <p className="text-xl text-emerald-100">
            Binlerce emlak ilanı arasından size en uygun olanı seçin
          </p>
        </div>
      </div>

      <div className="container mx-auto px-4 py-8">
        <SearchFilters onFilterChange={handleFilterChange} />

        {loading ? (
          <div className="flex justify-center items-center py-20">
            <Loader2 className="w-12 h-12 text-emerald-600 animate-spin" />
          </div>
        ) : error ? (
          <div className="text-center py-20">
            <p className="text-red-600 text-lg">{error}</p>
          </div>
        ) : properties.length === 0 ? (
          <div className="text-center py-20">
            <p className="text-gray-600 text-lg">İlan bulunamadı.</p>
          </div>
        ) : (
          <>
            <div className="mb-6 text-gray-700">
              <span className="font-semibold">{properties.length}</span> ilan bulundu
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {properties.map(property => (
                <PropertyCard key={property.id} property={property} />
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default HomePage;