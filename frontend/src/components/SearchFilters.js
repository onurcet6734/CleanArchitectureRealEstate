import React, { useState } from 'react';
import { Search, SlidersHorizontal, X } from 'lucide-react';

const SearchFilters = ({ onFilterChange }) => {
  const [showFilters, setShowFilters] = useState(false);
  const [filters, setFilters] = useState({
    Title: '',
    City: '',
    District: '',
    MinPrice: '',
    MaxPrice: '',
    Type: '',
    Status: '',
  });

  // SADECE state günceller (request YOK)
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFilters((prev) => ({ ...prev, [name]: value }));
  };

  // ARA butonuna basılınca request atılır
  const handleSearch = () => {
    onFilterChange(filters);
  };

  const handleClearFilters = () => {
    const clearedFilters = {
      Title: '',
      City: '',
      District: '',
      MinPrice: '',
      MaxPrice: '',
      Type: '',
      Status: '',
    };
    setFilters(clearedFilters);
    onFilterChange(clearedFilters);
  };

  return (
    <div className="bg-white rounded-xl shadow-md p-6 mb-8">
      {/* Search Bar */}
      <div className="flex items-center space-x-4">
        <div className="flex-1 relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 w-5 h-5" />
          <input
            type="text"
            name="Title"
            value={filters.Title}
            onChange={handleChange}
            placeholder="İlan ara (başlık, açıklama...)"
            className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500"
          />
        </div>

        {/* ARA BUTONU */}
        <button
          onClick={handleSearch}
          className="px-6 py-3 bg-emerald-600 text-white rounded-lg hover:bg-emerald-700 transition"
        >
          Ara
        </button>

        {/* Filtre Toggle */}
        <button
          onClick={() => setShowFilters(!showFilters)}
          className="flex items-center space-x-2 px-4 py-3 bg-gray-100 rounded-lg hover:bg-gray-200 transition"
        >
          <SlidersHorizontal className="w-5 h-5" />
          <span>{showFilters ? 'Filtreleri Gizle' : 'Filtrele'}</span>
        </button>
      </div>

      {/* Advanced Filters */}
      {showFilters && (
        <div className="mt-6 grid grid-cols-1 md:grid-cols-3 gap-4">
          {[
            { name: 'City', placeholder: 'Şehir' },
            { name: 'District', placeholder: 'İlçe' },
            { name: 'Type', placeholder: 'Tip (Daire, Villa)' },
            { name: 'MinPrice', placeholder: 'Min Fiyat', type: 'number' },
            { name: 'MaxPrice', placeholder: 'Max Fiyat', type: 'number' },
            { name: 'Status', placeholder: 'Durum (Satılık, Kiralık)' },
          ].map((item) => (
            <input
              key={item.name}
              type={item.type || 'text'}
              name={item.name}
              value={filters[item.name]}
              onChange={handleChange}
              placeholder={item.placeholder}
              className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-emerald-500"
            />
          ))}

          <button
            onClick={handleClearFilters}
            className="flex items-center justify-center space-x-2 px-4 py-2 bg-gray-200 rounded-lg hover:bg-gray-300 transition"
          >
            <X className="w-4 h-4" />
            <span>Temizle</span>
          </button>
        </div>
      )}
    </div>
  );
};

export default SearchFilters;