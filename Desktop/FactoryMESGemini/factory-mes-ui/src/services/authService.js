import axios from 'axios';

const API_URL = 'https://localhost:7007/api/auth'; // Port numaranızı kontrol edin

const register = (userData) => {
    return axios.post(`${API_URL}/register`, userData);
};

const login = (userData) => {
    return axios.post(`${API_URL}/login`, userData);
};

// Logout işlemi için backend'e gitmemize gerek yok, sadece token'ı sileceğiz.

export default {
    register,
    login,
};