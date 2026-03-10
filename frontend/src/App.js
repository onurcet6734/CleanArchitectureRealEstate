import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { Toaster } from "sonner";
import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";

// Pages
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import ProfilePage from "./pages/ProfilePage";
import CreatePropertyPage from "./pages/CreatePropertyPage";
import PropertyDetailPage from "./pages/PropertyDetailPage";
import MyListingsPage from "./pages/MyListingsPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import ResetPasswordPage from "./pages/ResetPasswordPage";

// Admin Pages
import AdminLayout from "./components/AdminLayout";
import AdminLoginPage from "./pages/admin/AdminLoginPage";
import AdminDashboard from "./pages/admin/AdminDashboard";
import AdminFlats from "./pages/admin/AdminFlats";
import AdminImages from "./pages/admin/AdminImages";
import AdminUsers from "./pages/admin/AdminUsers";

import "./App.css";

const EDevletCallback = () => {
  React.useEffect(() => {
    window.location.href = "/profile?edevlet=success";
  }, []);

  return (
    <div className="flex items-center justify-center min-h-screen">
      <p>e-Devlet doğrulaması tamamlanıyor...</p>
    </div>
  );
};

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <div className="App">
          <Navbar />
          <Routes>
            {/* Public Routes */}
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/forgot-password" element={<ForgotPasswordPage />} />
            <Route path="/reset-password" element={<ResetPasswordPage />} />
            <Route path="/property/:id" element={<PropertyDetailPage />} />

            {/* e-Devlet Callback */}
            <Route path="/edevlet/callback" element={<EDevletCallback />} />

            {/* Admin Login (Public) */}
            <Route path="/admin/login" element={<AdminLoginPage />} />

            {/* Protected Routes */}
            <Route
              path="/profile"
              element={
                <ProtectedRoute>
                  <ProfilePage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/create-property"
              element={
                <ProtectedRoute>
                  <CreatePropertyPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/my-flats"
              element={
                <ProtectedRoute>
                  <MyListingsPage />
                </ProtectedRoute>
              }
            />

            {/* Admin Routes */}
            <Route
              path="/admin"
              element={
                <ProtectedRoute>
                  <AdminLayout />
                </ProtectedRoute>
              }
            >
              <Route index element={<AdminDashboard />} />
              <Route path="flats" element={<AdminFlats />} />
              <Route path="images" element={<AdminImages />} />
             <Route path="users" element={<AdminUsers />} />
            </Route>
          </Routes>
          <Toaster position="top-right" richColors />
        </div>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
