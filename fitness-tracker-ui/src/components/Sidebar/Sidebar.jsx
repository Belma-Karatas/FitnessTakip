// src/components/Sidebar/Sidebar.jsx
import React from 'react';
// useNavigate'ı kaldırıyoruz çünkü artık kullanmıyoruz
import { NavLink } from 'react-router-dom';
import './Sidebar.css'; // Stiller için CSS dosyasını import edin
// İkonları import ediyoruz:
import { FaTachometerAlt, FaUsers, FaDumbbell, FaUtensils, FaClipboardList, FaBullseye, FaSignOutAlt } from 'react-icons/fa';

// Sidebar component'ine userName ve userRole prop'larını alıyoruz
function Sidebar({ userName, userRole }) { // userRole prop'unu ekledik
    // const navigate = useNavigate(); // useNavigate kullanmadığımız için bu satırı kaldırıyoruz

    // Logout fonksiyonu
    const handleLogout = () => {
        // localStorage'dan token ve diğer kullanıcı bilgilerini temizle
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('userId');
        localStorage.removeItem('userName');
        localStorage.removeItem('userRole');

        // Yönlendirme ve state senkronizasyonu için sayfayı yeniliyoruz.
        // Bu, App component'inin localStorage'ı doğru okuyup isLoggedIn state'ini güncellemesini sağlar.
        window.location.reload();
    };

    // Aktif linki belirlemek için className fonksiyonu
    const getNavLinkClass = ({ isActive }) => isActive ? 'nav-link active' : 'nav-link';

    return (
        <div className="sidebar-container">
            <div className="sidebar-header">
                <div className="sidebar-user-icon"><FaUsers /></div>
                {/* userName prop'unu kullanarak kullanıcı adını dinamik olarak gösteriyoruz */}
                {/* Eğer userName gelmezse varsayılan olarak 'Misafir' gösterilir */}
                <p className="sidebar-username">{userName || 'Misafir'}</p>
            </div>
            <nav className="sidebar-nav">
                <ul>
                    <li>
                        <NavLink to="/dashboard" className={getNavLinkClass}>
                            <FaTachometerAlt /> Dashboard
                        </NavLink>
                    </li>
                    
                    {/* Danışanlar menüsü sadece Antrenör rolündekilere gösterilsin */}
                    {userRole === 'Antrenor' && (
                        <li>
                            <NavLink to="/clients" className={getNavLinkClass}>
                                <FaUsers /> Danışanlar
                            </NavLink>
                        </li>
                    )}

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