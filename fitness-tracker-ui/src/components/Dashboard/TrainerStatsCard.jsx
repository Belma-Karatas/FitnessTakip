// src/components/Dashboard/TrainerStatsCard.jsx
import React from 'react';
import './Dashboard.css'; // Genel dashboard stilleri veya buraya özel stiller eklenebilir
import './TrainerStatsCard.css'; // TrainerStatsCard'a özel stiller

function TrainerStatsCard({ title, value, icon }) {
    return (
        <div className="stats-card">
            <div className="stats-icon">{icon}</div>
            <div className="stats-content">
                <h3 className="stats-title">{title}</h3>
                <p className="stats-value">{value}</p>
            </div>
        </div>
    );
}

export default TrainerStatsCard;