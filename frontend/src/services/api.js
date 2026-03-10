import axios from 'axios';

// Environment variable'dan backend URL'i al - ZORUNLU
const BACKEND_URL = process.env.REACT_APP_BACKEND_URL;

if (!BACKEND_URL) {
  const errorMessage = `
    ⚠️ HATA: REACT_APP_BACKEND_URL environment variable tanımlı değil!
    
    Lütfen frontend klasöründe .env dosyası oluşturun ve şu satırı ekleyin:
    REACT_APP_BACKEND_URL=https://localhost:7066
    
    Not: Uygulamayı yeniden başlatmanız gerekecek.
  `;
  console.error(errorMessage);
  throw new Error('REACT_APP_BACKEND_URL environment variable tanımlı değil. Lütfen .env dosyası oluşturun.');
}

const API_BASE = `${BACKEND_URL}/api`;
console.log('Backend URL yüklendi:', BACKEND_URL);

// Axios instance
const api = axios.create({
  baseURL: API_BASE,
  headers: {
    'Content-Type': 'application/json',
  },
});

//          'Content-Type': 'multipart/form-data',


// Request interceptor - Token ekle
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token') || localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor - Hata yönetimi
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('accessToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// ==================== E DEVLET ====================

export const edevlet = {
  initiate: () => api.get('/edevlet/initiate'),
  getStatus: () => api.get('/edevlet/status')
};


// ==================== AUTH API ====================
export const authAPI = {
  register: async (data) => {
    const response = await api.post('/auth/register', data);
    return response.data;
  },

  login: async (data) => {
    const response = await api.post('/auth/login', data);
    return response.data;
  },

  updateUser: async (id, data) => {
    const response = await api.patch(`/auth/users/${id}`, data);
    return response.data;
  },
  forgotPassword: async (email) => {
    const response = await api.post('/password/forgot', { email });
    return response.data;
  },
  passwordReset : async (formData) => {
    const response = await api.post("/password/reset" , formData);
    return response.data;
  },
  validateToken : async (token) => {
    const response = await api.post("/password/validate-token" , {token});
    return response.data;
  }
};

export const profileAPI = {
  getProfile : async() => {
    const response = await api.get("/profile/");
    return response.data
  },
  updateProfile: async(data) => {
    const response = await api.put("/profile", data);
    return response.data
  },
  changePassword: async (data) => {
    const response = await api.put('/profile/change-password', data);
    return response.data;
  },
  initiateEDevlet: async () => {
    const response = await api.get('/edevlet/initiate');
    return response.data;
  }
}

// ==================== FLATS API ====================
export const flatsAPI = {
  getFlats: async (params = {}) => {
    const response = await api.get('/flats', { params });
    return response.data;
  },

  getFlatById: async (id) => {
    const response = await api.get(`/flats/${id}`);
    return response.data;
  },

  createFlat: async (data) => {
    const response = await api.post('/flats', data);
    return response.data;
  },

  updateFlat: async (id, data) => {
    const response = await api.put(`/flats/${id}`, data);
    return response.data;
  },

  partialUpdateFlat: async (id, data) => {
    const response = await api.patch(`/flats/${id}`, data);
    return response.data;
  },

  deleteFlat: async (id) => {
    const response = await api.delete(`/flats/${id}`);
    return response.data;
  },
  uploadImages : async(formData) =>  {
    const response = await api.post('/flats/upload-images', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    return response.data
  }
};

// ==================== FLAT IMAGES API ====================
export const flatImagesAPI = {
  getFlatImages: async (params = {}) => {
    const response = await api.get('/flat-images', { params });
    return response.data;
  },
  updateFlatImage: async (id, data) => {
    const response = await api.patch(`/flat-images/${id}`, data);
    return response.data;
  },
};

export default api;
