// src/App.jsx
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import Register from './components/Auth/Register';
import Login from './components/Auth/Login';
// import Dashboard from './components/Dashboard'; // Giriş sonrası açılacak sayfa varsa ekleyebilirsin

function App() {
    // Token varsa kullanıcı giriş yapmış kabul edilir
    const isLoggedIn = !!localStorage.getItem("token");

    return (
        <Router>
            <div className="App">
                <Routes>
                    <Route path="/register" element={<Register />} />
                    <Route path="/login" element={<Login />} />

                    {/* Anasayfa yönlendirmesi */}
                    <Route
                        path="/"
                        element={
                            isLoggedIn ? (
                                // Dashboard varsa yönlendir, yoksa Register'a
                                // <Navigate to="/dashboard" replace />
                                <Navigate to="/register" replace />
                            ) : (
                                <Navigate to="/login" replace />
                            )
                        }
                    />

                    {/* 404 veya tanımsız rotalar için fallback */}
                    <Route path="*" element={<Navigate to="/" replace />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
