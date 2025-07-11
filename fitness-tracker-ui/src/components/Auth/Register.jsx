import React, { useState } from 'react';
import axios from 'axios';
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
        setMessage('');
        setIsError(false);

        try {
            const response = await axios.post('https://localhost:7062/api/auth/register', formData);
            setMessage(response.data.message || 'Kayıt başarıyla tamamlandı!');
            setIsError(false);

            setTimeout(() => {
                navigate('/login');
            }, 1000);

        } catch (error) {
            console.error("Kayıt hatası:", error);
            if (error.response) {
                setMessage(error.response.data || 'Kayıt sırasında bir hata oluştu.');
            } else {
                setMessage('Sunucuya bağlanırken bir hata oluştu.');
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
                    <input type="text" id="kullaniciAdi" name="kullaniciAdi" value={formData.kullaniciAdi} onChange={handleChange} required minLength="3" />
                </div>
                <div className="form-group">
                    <label htmlFor="eposta">E-posta:</label>
                    <input type="email" id="eposta" name="eposta" value={formData.eposta} onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label htmlFor="sifre">Şifre:</label>
                    <input type="password" id="sifre" name="sifre" value={formData.sifre} onChange={handleChange} required minLength="6" />
                </div>
                <div className="form-group">
                    <label htmlFor="ad">Ad :</label>
                    <input type="text" id="ad" name="ad" value={formData.ad} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label htmlFor="soyad">Soyad :</label>
                    <input type="text" id="soyad" name="soyad" value={formData.soyad} onChange={handleChange} />
                </div>
                <button type="submit">Kayıt Ol</button>
            </form>

            {/* 🔽 Giriş yap linki */}
            <div className="auth-links">
                <p>Zaten bir hesabın var mı? <Link to="/login">Giriş Yap</Link></p>
            </div>
        </div>
    );
}

export default Register;

