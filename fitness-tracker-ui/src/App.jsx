// src/App.jsx
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import './index.css';
import Register from './components/Auth/Register';
import Login from './components/Auth/Login';
import Dashboard from "./components/Dashboard/Dashboard";
import ProtectedRoute from './components/ProtectedRoute';
import Sidebar from './components/Sidebar/Sidebar';
import ClientsList from './components/Dashboard/ClientsList';

function App() {
    const jwtToken = localStorage.getItem("jwtToken");
    const userName = localStorage.getItem("userName") || 'Misafir';
    // userRole bilgisini de localStorage'dan alalım
    const userRole = localStorage.getItem("userRole") || 'Guest'; // Varsayılan olarak 'Guest' yapalım eğer yoksa
    const isLoggedIn = !!jwtToken;

    return (
        <Router>
            <div className="app-layout">

                {!isLoggedIn && (
                    <div className="auth-pages-wrapper">
                        <Routes>
                            <Route path="/register" element={<Register />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/" element={<Navigate to="/login" replace />} />
                            <Route path="*" element={<Navigate to="/login" replace />} />
                        </Routes>
                    </div>
                )}

                {isLoggedIn && (
                    <div className="main-app-content">
                        {/* Sidebar'a userName ve userRole prop'larını geçir */}
                        <Sidebar userName={userName} userRole={userRole} />
                        <div className="page-content">
                            <Routes>
                                <Route
                                    path="/dashboard"
                                    element={
                                        <ProtectedRoute>
                                            {/* Dashboard component'ine userName ve userRole'ü prop olarak ilet */}
                                            <Dashboard userName={userName} userRole={userRole} />
                                        </ProtectedRoute>
                                    }
                                />
                                <Route
                                    path="/clients"
                                    element={
                                        <ProtectedRoute>
                                            <ClientsList />
                                        </ProtectedRoute>
                                    }
                                />
                                {/* Diğer rotalar */}
                                <Route path="/workouts" element={<ProtectedRoute><h1>Antrenmanlar Sayfası</h1></ProtectedRoute>} />
                                <Route path="/nutrition" element={<ProtectedRoute><h1>Beslenme Takibi Sayfası</h1></ProtectedRoute>} />
                                <Route path="/progress" element={<ProtectedRoute><h1>İlerleme Takibi Sayfası</h1></ProtectedRoute>} />
                                <Route path="/goals" element={<ProtectedRoute><h1>Hedefler Sayfası</h1></ProtectedRoute>} />

                                <Route path="/" element={<Navigate to="/dashboard" replace />} />
                                <Route path="*" element={<Navigate to="/dashboard" replace />} />
                            </Routes>
                        </div>
                    </div>
                )}
            </div>
        </Router>
    );
}

export default App;