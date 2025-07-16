// src/components/Sidebar/Sidebar.jsx
import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom'; // useNavigate'ı da import ettik
import './Sidebar.css'; // Stiller için CSS dosyasını import edin
// İkonları import ediyoruz:
import { FaTachometerAlt, FaUsers, FaDumbbell, FaUtensils, FaClipboardList, FaBullseye, FaSignOutAlt } from 'react-icons/fa';

function Sidebar() {
    const navigate = useNavigate(); // useNavigate hook'unu aldık

    // Logout fonksiyonu
    const handleLogout = () => {
        // localStorage'dan token ve diğer kullanıcı bilgilerini temizle
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('userId');
        localStorage.removeItem('userName');
        localStorage.removeItem('userRole');

        // Kullanıcıyı giriş sayfasına yönlendir
        navigate('/login');
    };

    const getNavLinkClass = ({ isActive }) => isActive ? 'nav-link active' : 'nav-link';

    return (
        <div className="sidebar-container">
            <div className="sidebar-header">
                <div className="sidebar-user-icon"><FaUsers /></div>
                <p className="sidebar-username">Antrenör</p> {/* Kullanıcının adını buraya getirebiliriz */}
            </div>
            <nav className="sidebar-nav">
                <ul>
                    <li>
                        <NavLink to="/dashboard" className={getNavLinkClass}>
                            <FaTachometerAlt /> Dashboard
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/clients" className={getNavLinkClass}>
                            <FaUsers /> Danışanlar
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/workouts" className={getNavLinkClass}>
                            <FaDumbbell /> Antrenmanlar
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/nutrition" className={getNavLinkClass}>
                            <FaUtensils /> Beslenme
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/progress" className={getNavLinkClass}>
                            <FaClipboardList /> İlerleme Takibi
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/goals" className={getNavLinkClass}>
                            <FaBullseye /> Hedefler
                        </NavLink>
                    </li>
                </ul>
            </nav>
            {/* Çıkış Yap Butonu */}
            <div className="logout-section"> {/* Buton için ayrı bir container */}
                <button onClick={handleLogout} className="logout-button">
                    <FaSignOutAlt /> Çıkış Yap
                </button>
            </div>
        </div>
    );
}

export default Sidebar;