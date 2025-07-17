import React, { useState, useEffect } from 'react';
import signalrService from '../services/signalrService';
import { useAuth } from '../context/AuthContext';
import { Monitor, Info } from 'lucide-react';

const HomePage = () => {
  const [machines, setMachines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const { isAuthenticated } = useAuth();
  const [selectedMachine, setSelectedMachine] = useState(null);

  useEffect(() => {
    if (!isAuthenticated) return;

    const handleOeeUpdate = (machineId, oeeData) => {
      const now = new Date().toLocaleTimeString();

      setMachines(prev => {
        const existing = prev.find(m => m.machineId === machineId);
        const updated = { machineId, ...oeeData, lastUpdated: now };

        if (existing) {
          return prev.map(m => m.machineId === machineId ? updated : m);
        } else {
          return [...prev, updated];
        }
      });

      setLoading(false);
    };

    signalrService.startConnection();
    signalrService.on("ReceiveOeeUpdate", handleOeeUpdate);

    return () => {
      signalrService.off("ReceiveOeeUpdate", handleOeeUpdate);
    };
  }, [isAuthenticated]);

  const ProgressBar = ({ value, color }) => {
    const clampedValue = Math.min(value, 100);
    return (
      <div className="h-2 w-full bg-gray-300 dark:bg-gray-700 rounded overflow-hidden">
        <div
          className={`h-full rounded ${color}`}
          style={{ width: `${clampedValue}%` }}
        ></div>
      </div>
    );
  };

  if (loading) return (
    <p className="text-center p-10 text-gray-600 dark:text-gray-300 animate-pulse">
      Dashboard verileri yükleniyor...
    </p>
  );

  if (error) return (
    <p className="text-center p-10 text-red-500">
      {error}
    </p>
  );

  return (
    <div className="p-6 min-h-screen bg-gradient-to-br from-gray-100 to-white dark:from-gray-900 dark:to-gray-950 transition-colors duration-300">
      <h1 className="text-3xl md:text-4xl font-bold text-gray-800 dark:text-white mb-6 text-center tracking-tight">
        Makine Durum Paneli
      </h1>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {machines.map(m => (
          <div
            key={m.machineId}
            className="bg-white dark:bg-gray-800 p-5 rounded-2xl shadow-md hover:shadow-xl transition duration-300 border border-gray-200 dark:border-gray-700 relative"
          >
            {/* Status Icon with Label */}
            <div className="absolute top-4 right-4 flex flex-col items-center text-xs text-gray-500 dark:text-gray-400">
              <span className={`h-5 w-5 rounded-full ${
                m.status === 'Running' ? 'bg-green-500' : 'bg-red-500'
              }`} />
              <span className="mt-1">
                {m.status === 'Running' ? 'Çalışıyor' : 'Durduruldu'}
              </span>
            </div>

            <div className="flex items-center gap-2 mb-2">
              <Monitor className="w-5 h-5 text-blue-500 dark:text-blue-300" />
              <h2 className="text-lg font-semibold text-gray-700 dark:text-white truncate">
                Makine {m.machineId}
              </h2>
            </div>

            <div className="text-xs text-gray-400 dark:text-gray-500 mb-2">
              Güncelleme: {m.lastUpdated}
            </div>

            <div className="space-y-3 text-sm text-gray-600 dark:text-gray-300">
              <div>
                <div className="flex justify-between">
                  <span>Kullanılabilirlik</span><span>{m.availability?.toFixed(2)}%</span>
                </div>
                <ProgressBar value={m.availability} color="bg-yellow-400" />
              </div>

              <div>
                <div className="flex justify-between">
                  <span>Performans</span><span>{m.performance?.toFixed(2)}%</span>
                </div>
                <ProgressBar value={m.performance} color="bg-blue-400" />
              </div>

              <div>
                <div className="flex justify-between">
                  <span>Kalite</span><span>{m.quality?.toFixed(2)}%</span>
                </div>
                <ProgressBar value={m.quality} color="bg-green-400" />
              </div>
            </div>

            <div className="mt-4 flex justify-between items-center">
              <span className="text-lg font-bold text-gray-800 dark:text-white">
                OEE: {m.overallOee?.toFixed(2)}%
              </span>
              <button
                onClick={() => setSelectedMachine(m)}
                className="text-blue-500 hover:text-blue-700 dark:hover:text-blue-300"
                title="Detay"
              >
                <Info className="w-5 h-5" />
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Modal */}
      {selectedMachine && (
        <div className="fixed inset-0 z-50 bg-black/50 flex items-center justify-center px-4">
          <div className="bg-white dark:bg-gray-900 text-gray-800 dark:text-white p-6 rounded-xl w-full max-w-md shadow-xl">
            <h2 className="text-xl font-bold mb-4">Makine {selectedMachine.machineId} Detay</h2>
            <p><strong>Durum:</strong> {selectedMachine.status === 'Running'
              ? 'Çalışıyor'
              : 
               'Durduruldu'
              }
            </p>
            <p><strong>OEE:</strong> {selectedMachine.overallOee?.toFixed(2)}%</p>
            <p><strong>Kullanılabilirlik:</strong> {selectedMachine.availability?.toFixed(2)}%</p>
            <p><strong>Performans:</strong> {selectedMachine.performance?.toFixed(2)}%</p>
            <p><strong>Kalite:</strong> {selectedMachine.quality?.toFixed(2)}%</p>
            <p className="text-xs text-gray-400 mt-3">Son güncelleme: {selectedMachine.lastUpdated}</p>
            <div className="text-right mt-6">
              <button
                onClick={() => setSelectedMachine(null)}
                className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
              >
                Kapat
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default HomePage;
