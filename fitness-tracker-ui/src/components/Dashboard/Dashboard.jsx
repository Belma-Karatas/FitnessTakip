// src/components/Dashboard/Dashboard.jsx
import React, { useState, useEffect } from 'react';
import axiosInstance from '../../api/axiosInstance'; // Backend'e istek atmak için
import './Dashboard.css';
import './TrainerDashboard.css'; // TrainerDashboard için gerekli ise
import './ClientDashboard.css'; // ClientDashboard için gerekli stiller
import ClientsList from './ClientsList'; // Antrenör için
import TrainerStatsCard from './TrainerStatsCard'; // Antrenör için
import ClientStatsCard from './ClientStatsCard'; // Danışan için (bu component'i birazdan oluşturacağız)
import { FaUsers, FaRunning, FaCalendarCheck, FaUtensils, FaBullseye } from 'react-icons/fa'; // Antrenör ve Danışan için ikonlar

function Dashboard({ userName, userRole }) {
    const [dashboardData, setDashboardData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchDashboardData = async () => {
            try {
                // Backend'den veriyi çekmek için aynı endpoint'i kullanacağız.
                // Backend, isteği yapan kullanıcının rolüne göre doğru veriyi dönecek.
                const response = await axiosInstance.get('/dashboard');
                setDashboardData(response.data);
                console.log(`${userRole} Dashboard Data:`, response.data); // Rol bazlı loglama
            } catch (err) {
                console.error(`${userRole} Dashboard verileri çekilirken hata:`, err);
                if (err.response) {
                    setError(err.response.data || `${userRole} dashboard verileri yüklenemedi.`);
                } else {
                    setError('Sunucuya bağlanılamadı.');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchDashboardData();
    }, [userRole]); // userRole değiştiğinde veya component ilk yüklendiğinde çalışsın

    // Yükleme durumu
    if (loading) {
        return <div className="dashboard-content-wrapper">Yükleniyor...</div>;
    }

    // Hata durumu
    if (error) {
        return <div className="dashboard-content-wrapper error-message">{error}</div>;
    }

    // Veri yoksa veya boşsa
    if (!dashboardData) {
        // Hem antrenör hem danışan için boş veya null data durumunu yönet
        return (
            <div className="dashboard-content-wrapper">
                <div className="welcome-banner">
                    <div className="welcome-message-box">
                        <h1>👋 Hoş Geldiniz, {userName}!</h1>
                        <p>Dashboard verileri yüklenirken bir sorun oluştu.</p>
                    </div>
                </div>
            </div>
        );
    }

    // Ana render yapısı
    return (
        <div className="dashboard-content-wrapper">
            <div className="welcome-banner">
                <div className="welcome-message-box">
                    <h1>👋 Hoş Geldiniz, {userName}!</h1> {/* Dinamik kullanıcı adı */}
                    <p>
                        {userRole === 'Antrenor' ? 'Danışanlarınızın ilerlemesini ve genel istatistikleri görüntüleyin.' : 'Antrenmanlarınızı ve hedeflerinizi takip edin.'}
                    </p>
                </div>
            </div>

            {/* Rol bazlı içerik gösterme */}
            {userRole === 'Antrenor' && (
                <>
                    <div className="stats-grid">
                        <TrainerStatsCard title="Toplam Danışan" value={dashboardData.totalClients || 0} icon={<FaUsers />} />
                        <TrainerStatsCard title="Aktif Danışan (30 Gün)" value={dashboardData.activeClientsLast30Days || 0} icon={<FaRunning />} />
                        <TrainerStatsCard title="Yeni Kayıtlar (7 Gün)" value={dashboardData.newClientsLast7Days || 0} icon={<FaCalendarCheck />} />
                    </div>

                    <div className="clients-list-section">
                        <h2>Danışanlarım</h2>
                        {dashboardData.recentActiveClients && dashboardData.recentActiveClients.length > 0 ? (
                            <ClientsList clients={dashboardData.recentActiveClients} />
                        ) : (
                            <p>Yakın zamanda antrenman yapan danışan bulunmuyor.</p>
                        )}
                    </div>
                </>
            )}

            {userRole === 'Danisan' && ( // Danışan rolü için
                <div className="client-dashboard-sections"> {/* Danışan bölümleri için ana container */}
                    <h2>Genel Bakış</h2>
                    <div className="client-stats-grid">
                        {/* Danışan için istatistik kartları */}
                        <ClientStatsCard title="Toplam Antrenman" value={dashboardData.totalWorkouts || 0} icon={<FaRunning />} />
                        <ClientStatsCard title="Beslenme Kayıtları" value={dashboardData.totalNutritionEntries || 0} icon={<FaUtensils />} />
                        <ClientStatsCard title="Tamamlanan Hedefler" value={dashboardData.completedGoals || 0} icon={<FaBullseye />} />
                    </div>

                    {/* Son Antrenman Özeti */}
                    {dashboardData.lastWorkout ? (
                        <div className="summary-card last-workout-summary">
                            <h3>Son Antrenman</h3>
                            <p>Tarih: {new Date(dashboardData.lastWorkout.workoutDate).toLocaleDateString('tr-TR')}</p>
                            {dashboardData.lastWorkout.notes && <p>Notlar: {dashboardData.lastWorkout.notes}</p>}
                        </div>
                    ) : (
                        <div className="summary-card">
                            <h3>Son Antrenman</h3>
                            <p>Henüz antrenman kaydınız bulunmuyor.</p>
                        </div>
                    )}

                    {/* Hedef Özeti */}
                    {dashboardData.currentGoal ? (
                        <div className="summary-card current-goal-summary">
                            <h3>Mevcut Hedef</h3>
                            <p>Türü: {dashboardData.currentGoal.goalType}</p>
                            <p>Hedeflenen: {dashboardData.currentGoal.targetValue.toFixed(2)}</p>
                            <p>Mevcut: {dashboardData.currentGoal.currentValue.toFixed(2)}</p>
                            <p>Bitiş: {new Date(dashboardData.currentGoal.endDate).toLocaleDateString('tr-TR')}</p>
                            <p>Durum: {dashboardData.currentGoal.isAchieved ? 'Tamamlandı' : 'Devam Ediyor'}</p>
                        </div>
                    ) : (
                        <div className="summary-card">
                            <h3>Hedefleriniz</h3>
                            <p>Henüz aktif bir hedefiniz bulunmuyor.</p>
                        </div>
                    )}

                    {/* Atanmış Antrenör */}
                    {dashboardData.assignedCoach ? (
                        <div className="summary-card assigned-coach-summary">
                            <h3>Antrenörünüz</h3>
                            <p>Adı: {dashboardData.assignedCoach.coachFullName}</p>
                            <p>Kullanıcı Adı: {dashboardData.assignedCoach.coachUserName}</p>
                        </div>
                    ) : (
                        <div className="summary-card">
                            <h3>Antrenörünüz</h3>
                            <p>Henüz bir antrenör atanmamış.</p>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}

export default Dashboard;