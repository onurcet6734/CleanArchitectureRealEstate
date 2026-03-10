import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { flatImagesAPI, flatsAPI, profileAPI } from '../services/api';
import {
  MapPin,
  Home,
  Pencil,
  Trash2,
  Loader2,
  PlusCircle,
  Building2,
} from 'lucide-react';
import { toast } from 'sonner';

const MyListingsPage = () => {
  const [listings, setListings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [editingListing, setEditingListing] = useState(null);
  const [deleteConfirmId, setDeleteConfirmId] = useState(null);
  const [updateLoading, setUpdateLoading] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);

  const [editForm, setEditForm] = useState({
    title: '',
    description: '',
    price: '',
    currency: 'TRY',
    city: '',
    district: '',
    addressLine: '',
    type: '',
    status: 'Satılık',
  });
  
  const fetchUserListings = async () => {
    setLoading(true);
    try {
      const userInfo = await profileAPI.getProfile();

      console.log("Kullanici id'si :", userInfo.id);

      const data = await flatImagesAPI.getFlatImages({
        IsCover: true,
        UserId: userInfo.id
      });

      setListings(Array.isArray(data) ? data : []);
    } catch (error) {
      toast.error('İlanlar yüklenemedi');
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUserListings();
  }, []);

  const handleEditClick = (flat) => {
    setEditingListing(flat);
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

  const handleUpdateSubmit = async (e) => {
    e.preventDefault();
    if (!editingListing?.id) return;

    setUpdateLoading(true);
    try {
      await flatsAPI.updateFlat(editingListing.id, {
        ...editForm,
        price: parseFloat(editForm.price),
      });
      toast.success('İlan güncellendi');
      setEditingListing(null);
      fetchUserListings();
    } catch {
      toast.error('İlan güncellenemedi');
    } finally {
      setUpdateLoading(false);
    }
  };

  const handleDeleteConfirm = async () => {
    if (!deleteConfirmId) return;

    setDeleteLoading(true);
    try {
      await flatsAPI.deleteFlat(deleteConfirmId);
      toast.success('İlan silindi' , );
      setDeleteConfirmId(null);
      fetchUserListings();
    } catch {
      toast.error('İlan silinemedi');
    } finally {
      setDeleteLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <Loader2 className="w-10 h-10 animate-spin text-emerald-600" />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-emerald-50 to-green-50 py-12 px-4">
      <div className="max-w-6xl mx-auto">
        <div className="bg-white rounded-2xl shadow-xl p-8 mb-8 flex justify-between items-center">
          <div className="flex items-center space-x-3">
            <div className="w-12 h-12 bg-emerald-600 rounded-full flex items-center justify-center">
              <Building2 className="w-6 h-6 text-white" />
            </div>
            <div>
              <h1 className="text-3xl font-bold">İlanlarım</h1>
              <p className="text-gray-600">Eklediğiniz ilanlar</p>
            </div>
          </div>
          <Link
            to="/create-property"
            className="bg-emerald-600 text-white px-6 py-3 rounded-lg flex items-center space-x-2"
          >
            <PlusCircle className="w-5 h-5" />
            <span>Yeni İlan</span>
          </Link>
        </div>

        {listings.length === 0 ? (
          <div className="bg-white rounded-2xl shadow-xl p-16 text-center">
            <Building2 className="w-16 h-16 text-gray-300 mx-auto mb-4" />
            <h2 className="text-xl font-bold mb-2">Henüz ilan yok</h2>
            <Link
              to="/create-property"
              className="inline-flex items-center space-x-2 bg-emerald-600 text-white px-6 py-3 rounded-lg"
            >
              <PlusCircle className="w-5 h-5" />
              <span>İlan Oluştur</span>
            </Link>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {listings.map((item) => (
              <div
                key={item.flat.id}
                className="bg-white rounded-xl shadow-md overflow-hidden"
              >
                <div className="h-48 bg-gray-200">
                  <img
                    src={`${process.env.REACT_APP_BACKEND_URL}${item.url}`}
                    alt={item.flat.title}
                    className="w-full h-full object-cover"
                  />
                </div>

                <div className="p-4">
                  <h3 className="font-bold text-lg mb-1">
                    {item.flat.title}
                  </h3>
                  <p className="text-sm text-gray-600 mb-2">
                    {item.flat.description}
                  </p>

                  <div className="text-sm text-gray-700 space-y-1">
                    <div className="flex items-center">
                      <MapPin className="w-4 h-4 mr-2 text-emerald-600" />
                      {item.flat.city} / {item.flat.district}
                    </div>
                    <div className="flex items-center">
                      <Home className="w-4 h-4 mr-2 text-emerald-600" />
                      {item.flat.type}
                    </div>
                  </div>

                  <div className="mt-3 flex items-center font-bold text-emerald-700">
                    <span>{item.flat.price.toLocaleString()}</span>
                    <span className="ml-1 text-sm">{item.flat.currency}</span>
                  </div>

                  <div className="flex space-x-2 mt-4">
                    <button
                      onClick={() => handleEditClick(item.flat)}
                      className="flex-1 bg-blue-50 text-blue-600 py-2 rounded-lg flex items-center justify-center space-x-1"
                    >
                      <Pencil className="w-4 h-4" />
                      <span>Düzenle</span>
                    </button>
                    <button
                      onClick={() => setDeleteConfirmId(item.flat.id)}
                      className="flex-1 bg-red-50 text-red-600 py-2 rounded-lg flex items-center justify-center space-x-1"
                    >
                      <Trash2 className="w-4 h-4" />
                      <span>Sil</span>
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default MyListingsPage;