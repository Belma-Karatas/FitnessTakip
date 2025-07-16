// src/components/Auth/Register.jsx
import React, { useState } from 'react';
// import axios from 'axios'; // Axios'u doğrudan import etmeyi bırakıyoruz
import axiosInstance from '../../api/axiosInstance'; // Axios instance'ımızı import ediyoruz
import { useNavigate, Link } from 'react-router-dom';
import './Register.css';

function Register() {
    const [formData, setFormData] = useState({
        kullaniciAdi: '',
        eposta: '',
        sifre: '',
        ad: '',
        soyad: ''
    });

    const [message, setMessage] = useState('');
    const [isError, setIsError] = useState(false);

    const navigate = useNavigate();

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
            // await axios.post('https://localhost:7062/api/auth/register', formData); // Eski kullanım, теперь используем axiosInstance
            // Sadece route'un geri kalanını belirtmek yeterli, baseURL axiosInstance'da tanımlı.
            const response = await axiosInstance.post('/auth/register', formData);

            // Backend'den gelen başarılı mesajı işle
            setMessage(response.data.message || 'Kayıt başarıyla tamamlandı!');
            setIsError(false);

            // Başarılı kayıt sonrası kısa bir süre bekleyip giriş sayfasına yönlendir
            setTimeout(() => {
                navigate('/login');
            }, 1500); // 1.5 saniye sonra yönlendir

        } catch (error) {
            console.error("Kayıt hatası:", error);
            if (error.response) {
                // Backend'den gelen hata mesajını göster
                // error.response.data'nın bir string olduğunu varsayıyoruz.
                setMessage(error.response.data || 'Kayıt sırasında bir hata oluştu.');
            } else {
                // Ağ hatası gibi durumlarda genel bir mesaj göster
                setMessage('Sunucuya bağlanırken bir hata oluştu. Backend servisinin çalıştığından emin olun.');
            }
            setIsError(true);
        }
    };

    return (
        <div className="register-container">
            <h2>Kayıt Ol</h2>
            {message && (
                <p className={isError ? "message error" : "message success"}>
                    {message}
                </p>
            )}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="kullaniciAdi">Kullanıcı Adı:</label>
                    <input
                        type="text"
                        id="kullaniciAdi"
                        name="kullaniciAdi"
                        value={formData.kullaniciAdi}
                        onChange={handleChange}
                        required
                        minLength="3" // Kullanıcı adı için minimum uzunluk
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="eposta">E-posta:</label>
                    <input
                        type="email"
                        id="eposta"
                        name="eposta"
                        value={formData.eposta}
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
                        minLength="6" // Şifre için minimum uzunluk
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="ad">Ad :</label>
                    <input
                        type="text"
                        id="ad"
                        name="ad"
                        value={formData.ad}
                        onChange={handleChange}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="soyad">Soyad :</label>
                    <input
                        type="text"
                        id="soyad"
                        name="soyad"
                        value={formData.soyad}
                        onChange={handleChange}
                    />
                </div>
                <button type="submit">Kayıt Ol</button>
            </form>

            {/* Giriş yap linki */}
            <div className="auth-links">
                <p>Zaten bir hesabın var mı? <Link to="/login">Giriş Yap</Link></p>
            </div>
        </div>
    );
}

export default Register;

