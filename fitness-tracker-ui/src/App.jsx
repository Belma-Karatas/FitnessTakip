import { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [message, setMessage] = useState('Yükleniyor...');

  useEffect(() => {
    // Backend'deki API endpoint'ini çağırıyoruz.
    // Port numarasını (7062) kendi backend'inin port numarasıyla değiştir.
    axios.get('https://localhost:7062/api/test')
      .then(response => {
        // Başarılı olursa mesajı state'e yaz
        setMessage(response.data.message);
      })
      .catch(error => {
        // Hata olursa hatayı göster
        console.error("API bağlantı hatası!", error);
        setMessage("API'ye bağlanılamadı. Backend'in çalıştığından ve CORS ayarlarından emin olun.");
      });
  }, []); // [] sayesinde bu kod sadece bileşen ilk yüklendiğinde bir kez çalışır

  return (
    <div className="App">
      <header className="App-header">
        <h1>Fitness Takip Sistemi</h1>
        <p style={{ marginTop: '20px', color: 'lightgreen' }}>
          Backend'den Gelen Mesaj: <strong>{message}</strong>
        </p>
      </header>
    </div>
  );
}

export default App;