import React, { createContext, useState, useContext, useEffect } from 'react';
import authService from '../services/authService';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(() => localStorage.getItem('token'));

    useEffect(() => {
        if (token) {
            try {
                const decoded = jwtDecode(token);
                console.log("Giriş Yapıldı, Çözümlenmiş Token:", decoded);
                const isExpired = decoded.exp * 1000 < Date.now();

                if (isExpired) {
                    localStorage.removeItem('token');
                    setToken(null);
                    setUser(null);
                } else {
                    setUser({ 
                    id: decoded.sub,
                    username: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"], 
                     roles: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
                    });
                }
            } catch(error) {
                console.error("Geçersiz token:", error);
                localStorage.removeItem('token');
                setToken(null);
                setUser(null);
            }
        }
    }, [token]);

    const login = async (userData) => {
        const response = await authService.login(userData);
        const { token } = response.data;
        localStorage.setItem('token', token);
        setToken(token);
        return token;
    };

    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
        setToken(null);
    };

    const value = { user, token, isAuthenticated: !!token, login, logout };

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
    return useContext(AuthContext);
};