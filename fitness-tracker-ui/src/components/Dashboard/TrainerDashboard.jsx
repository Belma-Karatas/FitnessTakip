// src/components/Dashboard/TrainerDashboard.jsx
import React, { useState, useEffect } from 'react';
import axiosInstance from '../../api/axiosInstance'; // Backend'e istek atmak için
import './Dashboard.css'; // Genel dashboard stilleri
import './TrainerDashboard.css'; // Trainer Dashboard'a özel stiller
import ClientsList from './ClientsList'; // Mevcut ClientsList component'ini kullanacağız
import TrainerStatsCard from './TrainerStatsCard'; // Yeni istatistik kartı component'i
import { FaUsers, FaRunning, FaCalendarCheck } from 'react-icons/fa'; // İkonlar

function TrainerDashboard() {
    const [dashboardData, setDashboardData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchDashboardData = async () => {
            try {
                const response = await axiosInstance.get('/dashboard'); // Backend'deki endpoint'imiz
                setDashboardData(response.data);
                console.log("Trainer Dashboard Data:", response.data); // Gelen veriyi konsola yazdır
            } catch (err) {
                console.error("Trainer Dashboard verileri çekilirken hata:", err);
                if (err.response) {
                    setError(err.response.data || 'Veriler yüklenemedi.');
                } else {
                    setError('Sunucuya bağlanılamadı.');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchDashboardData();
    }, []);

    if (loading) {
        return <div className="dashboard-content-wrapper">Yükleniyor...</div>;
    }

    if (error) {
        return <div className="dashboard-content-wrapper error-message">{error}</div>;
    }

    // Eğer dashboardData null ise veya beklenen yapıda değilse
    if (!dashboardData) {
        return <div className="dashboard-content-wrapper">Veri bulunamadı.</div>;
    }

    return (
        <div className="dashboard-content-wrapper trainer-dashboard">
            <div className="welcome-banner">
                <div className="welcome-message-box">
                    <h1>👋 Hoş Geldiniz Antrenör!</h1>
                    <p>Danışanlarınızın ilerlemesini ve genel istatistikleri görüntüleyin.</p>
                </div>
            </div>

            <div className="stats-grid">
                <TrainerStatsCard title="Toplam Danışan" value={dashboardData.totalClients} icon={<FaUsers />} />
                <TrainerStatsCard title="Aktif Danışan (30 Gün)" value={dashboardData.activeClientsLast30Days} icon={<FaRunning />} />
                <TrainerStatsCard title="Yeni Kayıtlar (7 Gün)" value={dashboardData.newClientsLast7Days} icon={<FaCalendarCheck />} />
            </div>

            <div className="clients-list-section">
                <h2>Son Aktif Danışanlar</h2>
                {dashboardData.recentActiveClients && dashboardData.recentActiveClients.length > 0 ? (
                    <ClientsList clients={dashboardData.recentActiveClients} />
                ) : (
                    <p>Yakın zamanda antrenman yapan danışan bulunmuyor.</p>
                )}
            </div>
        </div>
    );
}

export default TrainerDashboard;