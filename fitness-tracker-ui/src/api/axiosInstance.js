// src/api/axiosInstance.js
import axios from 'axios';

// Backend API'nizin temel URL'ini buraya girin.
// launchSettings.json'daki https portunu kullanıyoruz (genellikle 7062'dir).
const API_URL = 'https://localhost:7062/api';

const axiosInstance = axios.create({
  baseURL: API_URL,
  // Gerekirse başka varsayılan ayarlar buraya eklenebilir (headers vb.)
});

// İstek gönderilmeden önce interceptor ekleyerek token'ı ekleyelim
// Bu sayede her istekte token'ı manuel olarak eklememiz gerekmeyecek.
axiosInstance.interceptors.request.use(
  (config) => {
    // localStorage'dan kaydedilmiş JWT token'ını al
    const token = localStorage.getItem('jwtToken');

    // Eğer token varsa, Authorization başlığına ekle
    if (token) {
      // 'Bearer ' öneki, JWT kimlik doğrulama için standart bir kullanımdır.
      config.headers.Authorization = `Bearer ${token}`;
    }
    // Konfigürasyon değişikliklerini veya orijinal config'i döndür
    return config;
  },
  (error) => {
    // İstek konfigürasyonunda bir hata olursa bu kısım çalışır
    // Genellikle bu noktada çok fazla işlem yapmaya gerek kalmaz.
    return Promise.reject(error);
  }
);

// Buraya isteğe bağlı olarak yanıt interceptor'ları da eklenebilir.
// Örneğin, 401 (Unauthorized) yanıt gelirse kullanıcıyı logout yapmak gibi.
// axiosInstance.interceptors.response.use(...);

export default axiosInstance;