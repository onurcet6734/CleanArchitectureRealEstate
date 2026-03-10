import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Home, PlusCircle, User, LogOut, LogIn, UserPlus, LayoutDashboard, List } from 'lucide-react';

const Navbar = () => {
  const { isAuthenticated, user, logout, token } = useAuth();
  const navigate = useNavigate();

  // Debug için
  console.log('Navbar render - isAuthenticated:', isAuthenticated, 'user:', user, 'token:', token);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="bg-white shadow-md sticky top-0 z-50">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link to="/" className="flex items-center space-x-2">
            <div className="w-10 h-10 bg-gradient-to-br from-emerald-500 to-green-600 rounded-lg flex items-center justify-center">
              <Home className="w-6 h-6 text-white" />
            </div>
            <span className="text-xl font-bold text-emerald-700">EmlakHub</span>
          </Link>

          {/* Navigation Links */}
          <div className="flex items-center space-x-6">
            <Link 
              to="/" 
              className="text-gray-700 hover:text-emerald-600 transition flex items-center space-x-1"
              data-testid="nav-home-link"
            >
              <Home className="w-4 h-4" />
              <span>Ana Sayfa</span>
            </Link>

            {isAuthenticated ? (
              <>
                <Link 
                  to="/create-property" 
                  className="bg-emerald-600 text-white px-4 py-2 rounded-lg hover:bg-emerald-700 transition flex items-center space-x-1"
                  data-testid="nav-create-property-link"
                >
                  <PlusCircle className="w-4 h-4" />
                  <span>İlan Ver</span>
                </Link>

                <Link 
                  to="/my-flats" 
                  className="text-gray-700 hover:text-emerald-600 transition flex items-center space-x-1"
                  data-testid="nav-my-flats-link"
                >
                  <List className="w-4 h-4" />
                  <span>İlanlarım</span>
                </Link>
{/* 
                <Link 
                  to="/admin" 
                  className="text-gray-700 hover:text-emerald-600 transition flex items-center space-x-1"
                  data-testid="nav-admin-link"
                >
                  <LayoutDashboard className="w-4 h-4" />
                  <span>Admin</span>
                </Link> */}

                <Link
                  to="/profile"
                  className="flex items-center space-x-2 text-gray-700 hover:text-emerald-600 transition"
                >
                  <div className="w-9 h-9 rounded-full bg-emerald-600 text-white flex items-center justify-center text-sm font-semibold">
                    {user?.fullname
                      ? user.fullname.charAt(0).toUpperCase()
                      : "U"}
                  </div>

                  <span className="hidden md:inline">
                    {user?.fullname || "Kullanıcı"}
                  </span>
                </Link>

                <Link 
                  to="/profile" 
                  className="text-gray-700 hover:text-emerald-600 transition flex items-center space-x-1"
                  data-testid="nav-profile-link"
                >
                  <span>Profil</span>
                </Link>

                <button 
                  onClick={handleLogout}
                  className="text-gray-700 hover:text-red-600 transition flex items-center space-x-1"
                  data-testid="nav-logout-button"
                >
                  <LogOut className="w-4 h-4" />
                  <span>Çıkış</span>
                </button>
              </>
            ) : (
              <>
                <Link 
                  to="/login" 
                  className="text-gray-700 hover:text-emerald-600 transition flex items-center space-x-1"
                  data-testid="nav-login-link"
                >
                  <LogIn className="w-4 h-4" />
                  <span>Giriş Yap</span>
                </Link>

                <Link 
                  to="/register" 
                  className="bg-emerald-600 text-white px-4 py-2 rounded-lg hover:bg-emerald-700 transition flex items-center space-x-1"
                  data-testid="nav-register-link"
                >
                  <UserPlus className="w-4 h-4" />
                  <span>Üye Ol</span>
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
