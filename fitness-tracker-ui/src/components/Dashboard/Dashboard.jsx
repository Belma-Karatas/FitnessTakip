// src/components/Dashboard/Dashboard.jsx
import React from 'react';
import './Dashboard.css';

function Dashboard() {
  return (
    <div className="dashboard-top-banner">
      <div className="welcome-box">
        <h1>👋 Hoş Geldiniz, Şevval!</h1>
        <p>Antrenmanlarınızı ve danışanlarınızı takip edin.</p>
        <div className="empty-message">
          Bu antrenöre ait hiç danışan bulunamadı.
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
