// src/components/Dashboard/ClientStatsCard.jsx
import React from 'react';
import './Dashboard.css'; // Genel dashboard stilleri
import './ClientStatsCard.css'; // ClientStatsCard'a özel stiller

function ClientStatsCard({ title, value, icon }) {
    return (
        <div className="client-stats-card">
            <div className="stats-icon">{icon}</div>
            <div className="stats-content">
                <h3 className="stats-title">{title}</h3>
                <p className="stats-value">{value}</p>
            </div>
        </div>
    );
}

export default ClientStatsCard;