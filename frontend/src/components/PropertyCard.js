import React from 'react';
import { Link } from 'react-router-dom';
import { MapPin, Home } from 'lucide-react';

const PropertyCard = ({ property }) => {
  const {
    id,
    title,
    description,
    price,
    currency,
    city,
    district,
    type,
    status,
    imageUrl
  } = property;

  const displayImage =
    imageUrl || 'https://images.unsplash.com/photo-1560518883-ce09059eeffa?w=400&h=300&fit=crop';

  return (
    <Link
      to={`/property/${id}`}
      className="block bg-white rounded-xl shadow-md overflow-hidden hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1"
    >
      <div className="relative h-48 bg-gray-200 overflow-hidden">
        <img
          src={displayImage}
          alt={title}
          className="w-full h-full object-cover"
        />
        {status && (
          <div className="absolute top-3 right-3 bg-emerald-600 text-white px-3 py-1 rounded-full text-xs font-semibold">
            {status}
          </div>
        )}
      </div>

      <div className="p-4">
        <h3 className="text-lg font-bold text-gray-800 mb-2 line-clamp-1">
          {title}
        </h3>

        <p className="text-sm text-gray-600 mb-3 line-clamp-2">
          {description}
        </p>

        <div className="space-y-2">
          <div className="flex items-center text-gray-700 text-sm">
            <MapPin className="w-4 h-4 mr-2 text-emerald-600" />
            <span>{city} / {district}</span>
          </div>

          {type && (
            <div className="flex items-center text-gray-700 text-sm">
              <Home className="w-4 h-4 mr-2 text-emerald-600" />
              <span>{type}</span>
            </div>
          )}
        </div>

        <div className="mt-4 pt-4 border-t border-gray-200">
          <div className="flex items-center text-emerald-700 font-bold text-xl">
            <span>{price?.toLocaleString()}</span>
            <span className="text-sm ml-1">{currency}</span>
          </div>
        </div>
      </div>
    </Link>
  );
};

export default PropertyCard;