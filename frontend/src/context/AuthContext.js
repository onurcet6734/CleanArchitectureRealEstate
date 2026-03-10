// AuthContext.js
import React, { createContext, useState, useContext, useEffect } from 'react';
import { authAPI } from '../services/api';

const AuthContext = createContext();

const decodeJWT = (token) => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error('JWT decode error:', error);
    return null;
  }
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const savedToken = localStorage.getItem('token') || localStorage.getItem('accessToken');
    const savedUser = localStorage.getItem('user');

    if (savedToken) {
      setToken(savedToken);

      if (savedUser) {
        const parsedUser = JSON.parse(savedUser);

        if (!parsedUser.id && savedToken) {
          const decodedToken = decodeJWT(savedToken);
          if (decodedToken) {
            parsedUser.username = parsedUser.username || decodedToken.username || decodedToken.unique_name;
            parsedUser.email = parsedUser.email || decodedToken.email;
            localStorage.setItem('user', JSON.stringify(parsedUser));
          }
        }
        setUser(parsedUser);
      }
    }
    setLoading(false);
  }, []);

  const register = async (data) => {
    try {
      const response = await authAPI.register(data);
      return { success: true, data: response };
    } catch (error) {
      return {
        success: false,
        error: error.response?.data?.message || 'Kayıt başarısız oldu',
      };
    }
  };

  const login = async (username, password) => {
    try {
      const response = await authAPI.login({ username, password });
      const fullname = response.fullName;

      const authToken =
        response.accessToken ||
        response.token ||
        response.data?.accessToken ||
        response.data?.token;
      
      const userId = response.userId

      if (authToken) {
        localStorage.setItem('token', authToken);
        localStorage.setItem('accessToken', authToken);
        // localStorage.setItem("userId", userId)

        let userData = response.user || response.data?.user || response.userData || { username };

        if (!userData.id && authToken) {
          const decodedToken = decodeJWT(authToken);
          if (decodedToken) {
            userData = {
              ...userData,
              id:
                decodedToken.userId ||
                decodedToken.id ||
                decodedToken.sub ||
                decodedToken.nameid,
              username:
                userData.username ||
                decodedToken.username ||
                decodedToken.unique_name ||
                username,
              email: userData.email || decodedToken.email,
              fullname: fullname,
            };
          }
        }

        const fullUserData = {
          ...userData,
          username: userData.username || username,
        };

        localStorage.setItem('user', JSON.stringify(fullUserData));
        setToken(authToken);
        setUser(fullUserData);
      }

      return { success: true, data: response };
    } catch (error) {
      console.error('Login error:', error);
      return {
        success: false,
        error: error.response?.data?.message || 'Giriş başarısız oldu',
      };
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  };

  const updateUser = async (id, firstName, lastName) => {
    try {
      const response = await authAPI.updateUser(id, { firstName, lastName });
      const updatedUser = { ...user, firstName, lastName };
      localStorage.setItem('user', JSON.stringify(updatedUser));
      setUser(updatedUser);
      return { success: true, data: response };
    } catch (error) {
      return {
        success: false,
        error: error.response?.data?.message || 'Güncelleme başarısız oldu',
      };
    }
  };

  const forgotPassword = async (email) => {
    try {
      await authAPI.forgotPassword(email);
      return { success: true };
    } catch (error) {
      return {
        success: false,
        error:
          error.response?.data?.message ||
          'Şifre sıfırlama isteği başarısız oldu',
      };
    }
  };

  const value = {
    user,
    token,
    loading,
    isAuthenticated: !!token,
    register,
    login,
    logout,
    updateUser,
    forgotPassword,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};