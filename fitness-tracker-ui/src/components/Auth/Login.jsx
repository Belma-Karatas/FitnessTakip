// src/components/Auth/Login.jsx
import React, { useState } from 'react';
import axiosInstance from '../../api/axiosInstance'; // Axios instance'ımızı import ediyoruz
// useNavigate'ı kaldırıyoruz çünkü artık kullanmıyoruz
import { Link } from 'react-router-dom';
import './Login.css'; // Login için CSS dosyasını import edin

function Login() {
    const [formData, setFormData] = useState({
        kullaniciAdiVeyaEposta: '', // Backend'deki LoginRequestDto ile uyumlu
        sifre: ''
    });

    const [message, setMessage] = useState('');
    const [isError, setIsError] = useState(false);

    // const navigate = useNavigate(); // Bu satırı kaldırıyoruz

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
            const response = await axiosInstance.post('/auth/login', formData);

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

            // Başarılı giriş sonrası sayfayı yenilemek,
            // App component'inin localStorage'ı doğru okuyup isLoggedIn state'ini güncellemesini sağlar.
            window.location.reload();

        } catch (error) {
            console.error("Giriş hatası:", error);
            if (error.response) {
                // Backend'den gelen hata mesajını göster
                setMessage(error.response.data || 'Giriş sırasında bir hata oluştu.');
            } else {
                // Ağ hatası gibi durumlarda genel bir mesaj göster
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
                <p>Hesabınız yok mu? <Link to="/register">Kayıt Ol</Link></p>
            </div>
        </div>
    );
}

export default Login;