// src/components/Dashboard/ClientsList.jsx
import React, { useState, useEffect } from 'react';
import axiosInstance from '../../api/axiosInstance'; // Axios instance'ımızı import ediyoruz
import './ClientsList.css'; // Stiller için CSS dosyası oluşturacağız

function ClientsList() {
    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchClients = async () => {
            try {
                // Backend'den danışanları çekiyoruz.
                // API endpoint'i '/api/kullanicilar/my-clients' olarak tanımlandı.
                const response = await axiosInstance.get('/kullanicilar/my-clients');
                setClients(response.data); // Gelen veriyi state'e kaydet
            } catch (err) {
                console.error("Danışanlar çekilirken hata oluştu:", err);
                if (err.response) {
                    // Backend'den gelen hata mesajını göster
                    setError(err.response.data || 'Danışanlar yüklenemedi.');
                } else {
                    // Ağ hatası gibi durumlarda genel bir mesaj göster
                    setError('Sunucuya bağlanırken bir hata oluştu.');
                }
            } finally {
                setLoading(false); // Yükleme bitti
            }
        };

        fetchClients();
    }, []); // Component ilk render edildiğinde çalışsın

    if (loading) {
        return <div className="clients-container">Yükleniyor...</div>;
    }

    if (error) {
        return <div className="clients-container error">{error}</div>;
    }

    return (
        <div className="clients-container">
            <h2>Danışanlarım</h2>
            {clients.length === 0 ? (
                <p>Henüz bir danışanınız bulunmuyor.</p>
            ) : (
                <ul className="clients-list">
                    {clients.map(client => (
                        <li key={client.kullaniciID} className="client-item">
                            <div className="client-info">
                                <strong>{client.ad} {client.soyad || ''}</strong> ({client.kullaniciAdi})
                                <br />
                                <small>{client.eposta}</small>
                            </div>
                            {client.guncelKiloKG && (
                                <div className="client-weight">
                                    Kilo: {client.guncelKiloKG} kg
                                </div>
                            )}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default ClientsList;