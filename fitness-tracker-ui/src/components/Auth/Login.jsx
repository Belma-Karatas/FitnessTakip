// src/components/Auth/Login.jsx
import React, { useState } from 'react';
import axios from 'axios';
import './Login.css'; // Login için CSS dosyasını import edin

function Login() {
    const [formData, setFormData] = useState({
        kullaniciAdiVeyaEposta: '', // Backend'deki LoginRequestDto ile uyumlu
        sifre: ''
    });

    const [message, setMessage] = useState('');
    const [isError, setIsError] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage(''); // Önceki mesajları temizle
        setIsError(false);

        try {
            // Backend API URL'sini doğru ayarlayın.
            // Lokal olarak çalışıyorsa genellikle http://localhost:PORT veya https://localhost:PORT olur.
            // Projenizde backend'in çalıştığı port numarasını kontrol edin.
            const response = await axios.post('https://localhost:7062/api/auth/login', formData);

            // Backend'den gelen token'ı ve kullanıcı bilgilerini sakla
            const token = response.data.token;
            const userId = response.data.kullaniciID;
            const userName = response.data.kullaniciAdi;
            const userRole = response.data.rol;

            // localStorage'a kaydetme
            localStorage.setItem('jwtToken', token);
            localStorage.setItem('userId', userId);
            localStorage.setItem('userName', userName);
            localStorage.setItem('userRole', userRole);

            setMessage('Başarıyla giriş yapıldı!');
            setIsError(false);

            // Başarılı giriş sonrası ana sayfaya veya dashboard'a yönlendirme yap
            // Eğer react-router-dom kullanıyorsanız:
            // import { useNavigate } from 'react-router-dom';
            // const navigate = useNavigate();
            // navigate('/dashboard');
            // Şimdilik basit bir yönlendirme veya sayfa yenileme yapabiliriz:
            window.location.href = '/dashboard'; // Veya başka bir sayfaya yönlendirin

        } catch (error) {
            console.error("Giriş hatası:", error);
            if (error.response) {
                // Backend'den gelen hata mesajını göster
                setMessage(error.response.data || 'Giriş sırasında bir hata oluştu.');
            } else {
                setMessage('Sunucuya bağlanırken bir hata oluştu. Backend servisinin çalıştığından emin olun.');
            }
            setIsError(true);
        }
    };

    return (
        <div className="login-container">
            <h2>Giriş Yap</h2>
            {message && (
                <p className={isError ? "message error" : "message success"}>
                    {message}
                </p>
            )}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="kullaniciAdiVeyaEposta">Kullanıcı Adı veya E-posta:</label>
                    <input
                        type="text"
                        id="kullaniciAdiVeyaEposta"
                        name="kullaniciAdiVeyaEposta"
                        value={formData.kullaniciAdiVeyaEposta}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="sifre">Şifre:</label>
                    <input
                        type="password"
                        id="sifre"
                        name="sifre"
                        value={formData.sifre}
                        onChange={handleChange}
                        required
                    />
                </div>
                <button type="submit">Giriş Yap</button>
            </form>

            {/* Kayıt ol linki */}
            <div className="auth-links">
                <p>Hesabınız yok mu? <a href="/register">Kayıt Ol</a></p>
                {/* Şifremi unuttum linki de eklenebilir */}
            </div>
        </div>
    );
}

export default Login;