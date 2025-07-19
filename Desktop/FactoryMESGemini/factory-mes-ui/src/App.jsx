import React, { useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, useNavigate } from 'react-router-dom';
import { ThemeProvider } from './context/ThemeContext';
import signalrService from './services/signalrService';

// Sayfalar
import HomePage from './pages/HomePage';
import ProductPage from './pages/ProductPage';
import MachinePage from './pages/MachinePage';
import WorkOrderPage from './pages/WorkOrderPage';
import MaintenancePage from './pages/MaintenancePage';
import UserManagementPage from './pages/UserManagementPage';
import LoginPage from './pages/LoginPage';
import TraceabilityPage from './pages/TraceabilityPage';
import ProcessPage from './pages/ProcessPage';
import RoutePage from './pages/RoutePage'; // YENİ EKLENDİ

// Context ve Bileşenler
import { useAuth } from './context/AuthContext';
import Sidebar from './components/Sidebar';
import Header from './components/Header';

function AppContent() {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      signalrService.startConnection();
    }
  }, [isAuthenticated]);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const isAdmin = user?.roles?.includes('Admin');

  return (
    <ThemeProvider>
      <div className="flex min-h-screen font-sans text-base text-gray-800 dark:text-gray-100 leading-relaxed bg-gradient-to-br from-gray-100 to-white dark:from-gray-900 dark:to-gray-950 transition-colors duration-300 ease-in-out">
        {isAuthenticated && <Sidebar isAdmin={isAdmin} />}
        <div className="flex-1 flex flex-col border-l border-gray-200 dark:border-gray-800 transition-all duration-300 ease-in-out">
          <Header username={user?.username} isAuthenticated={isAuthenticated} onLogout={handleLogout} />

          <main className="flex-1 p-4">
            <Routes>
              <Route path="/" element={<HomePage />} />
              <Route path="/products" element={<ProductPage />} />
              <Route path="/machines" element={<MachinePage />} />
              <Route path="/workorders" element={<WorkOrderPage />} />
              <Route path="/maintenance" element={<MaintenancePage />} />
              <Route path="/users" element={<UserManagementPage />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/traceability" element={<TraceabilityPage />} />
              <Route path="/processes" element={<ProcessPage />} />
              <Route path="/routes" element={<RoutePage />} /> {/* YENİ EKLENDİ */}
            </Routes>
          </main>
        </div>
      </div>
    </ThemeProvider>
  );
}

function App() {
  return (
    <Router>
      <AppContent />
    </Router>
  );
}

export default App;