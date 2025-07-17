// src/components/Dashboard/Dashboard.jsx
import React from 'react';
import './Dashboard.css';
import ClientsList from '../Dashboard/ClientsList'; // ClientsList'i import ettik

// Dashboard component'ine userName ve userRole prop'larını alıyoruz
function Dashboard({ userName, userRole }) {
  return (
    <div className="dashboard-content-wrapper">
      <div className="welcome-banner"> {/* Bu yeni bir container, üstte ve sola yaslı olacak */}
        <div className="welcome-message-box"> {/* Gerçek mesaj kutusu */}
          <h1>👋 Hoş Geldiniz, {userName}!</h1> {/* Dinamik kullanıcı adı */}
          <p>Antrenmanlarınızı ve danışanlarınızı takip edin.</p>
        </div>
      </div>

      {/* Rol bazlı içerik gösterme */}
      {/* Eğer userRole bilgisi doğru geliyorsa bu koşullu renderlama çalışacaktır. */}
      {userRole === 'Antrenor' && (
        <div className="dashboard-section"> {/* Bu yeni bir div olabilir, stillerini Dashboard.css'te ayarlayabiliriz */}
          <h2>Danışanlarım</h2>
          <ClientsList /> {/* Antrenörse Danışan Listesini Göster */}
        </div>
      )}

      {/* Eğer Danışan rolündeyse farklı bir şey gösterebiliriz. */}
      {userRole === 'Danisan' && (
        <div className="dashboard-section">
          <h2>Antrenman Özetiniz</h2>
          <p>Henüz antrenman kaydınız bulunmuyor veya antrenman modülü aktif değil.</p>
          {/* Buraya danışana özel antrenman özetleri, hedef takibi gibi şeyler gelebilir */}
        </div>
      )}

      {/* Eğer rol bilgisi gelmezse veya başka bir rol varsa ne olacak? */}
      {/* Bu durumlarda hiçbir extra section gösterilmez, sadece hoş geldiniz mesajı görünür. */}
      {/* Veya isterseniz bir default mesaj da ekleyebilirsiniz: */}
      {/* {(userRole !== 'Antrenor' && userRole !== 'Danisan') && (
         <div className="dashboard-section">
           <p>Hesabınıza özel bölümler için lütfen rolünüzü kontrol edin.</p>
         </div>
      )} */}
    </div>
  );
}

export default Dashboard;
