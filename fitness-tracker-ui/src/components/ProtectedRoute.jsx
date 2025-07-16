// src/components/ProtectedRoute.jsx
import React from 'react';
import { Navigate } from 'react-router-dom';

// Bu component, çocuğunu render etmeden önce kullanıcının giriş yapıp yapmadığını kontrol eder.
// Eğer giriş yapmamışsa, kullanıcıyı login sayfasına yönlendirir.
const ProtectedRoute = ({ children }) => {
    // localStorage'dan token'ı kontrol et
    const isLoggedIn = !!localStorage.getItem("jwtToken");

    // Eğer kullanıcı giriş yapmışsa (token varsa), çocuğunu render et (örn: Dashboard)
    // Aksi halde, login sayfasına yönlendir
    return isLoggedIn ? children : <Navigate to="/login" replace />;
};

export default ProtectedRoute;