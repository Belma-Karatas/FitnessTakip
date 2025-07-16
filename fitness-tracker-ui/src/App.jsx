// src/App.jsx
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css'; // Genel App stilleri
import './index.css'; // Genel Index stilleri (arka plan görseli dahil)
import Register from './components/Auth/Register';
import Login from './components/Auth/Login';
import Dashboard from "./components/Dashboard/Dashboard";
import ProtectedRoute from './components/ProtectedRoute';
import Sidebar from './components/Sidebar/Sidebar'; // Sidebar'ı import ettik

function App() {
    const isLoggedIn = !!localStorage.getItem("jwtToken"); // 'jwtToken' kontrolü

    return (
        <Router>
            <div className="app-layout"> {/* Ana layout container */}

                {/*
                    Eğer kullanıcı giriş yapmamışsa, sadece Login ve Register sayfalarını göster.
                    Bu sayfalarda Sidebar görünmeyecek.
                */}
                {!isLoggedIn && (
                    <div className="auth-pages-wrapper"> {/* Auth sayfaları için ayrı bir wrapper */}
                        <Routes>
                            <Route path="/register" element={<Register />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/" element={<Navigate to="/login" replace />} /> {/* Ana sayfa da Login'e yönlenir */}
                            <Route path="*" element={<Navigate to="/login" replace />} /> {/* Diğer tüm bilinmeyen rotalar Login'e */}
                        </Routes>
                    </div>
                )}

                {/*
                    Eğer kullanıcı giriş yapmışsa, Sidebar ve ana içerik alanını göster.
                    Dashboard rotası ProtectedRoute ile korunmalı.
                */}
                {isLoggedIn && (
                    <div className="main-app-content"> {/* Sidebar ve page-content'i kapsar */}
                        <Sidebar /> {/* Yan Paneli buraya ekledik */}
                        <div className="page-content"> {/* Ana içerik alanı */}
                            <Routes>
                                {/* Dashboard rotası ProtectedRoute ile korunuyor */}
                                <Route
                                    path="/dashboard"
                                    element={
                                        <ProtectedRoute>
                                            <Dashboard />
                                        </ProtectedRoute>
                                    }
                                />
                                {/* Diğer korumalı rotalar buraya eklenebilir */}
                                {/* Örneğin: */}
                                {/* <Route path="/clients" element={<ProtectedRoute><ClientsPage /></ProtectedRoute>} /> */}

                                {/* Giriş yapmış kullanıcılar için ana sayfa "/" rotası da Dashboard'a yönlendirilebilir */}
                                <Route path="/" element={<Navigate to="/dashboard" replace />} />

                                {/* Tanımsız rotalar için fallback */}
                                <Route path="*" element={<Navigate to="/dashboard" replace />} /> {/* Dashboard dışında bir korumalı rota aranırsa */}
                            </Routes>
                        </div>
                    </div>
                )}
            </div>
        </Router>
    );
}

export default App;