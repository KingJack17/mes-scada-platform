import React, { useState } from 'react';
import { getTraceabilityBySerialNumber } from '../services/apiService';
import Timeline from '../components/traceability/Timeline';

const TraceabilityPage = () => {
    const [serialNumber, setSerialNumber] = useState('');
    const [traceData, setTraceData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!serialNumber.trim()) {
            alert("Lütfen bir seri numarası girin.");
            return;
        }
        setLoading(true);
        setError('');
        setTraceData(null);
        try {
            const response = await getTraceabilityBySerialNumber(serialNumber);
            setTraceData(response.data);
        } catch (err) {
            setError(err.response?.data || "Seri numarası bulunamadı veya bir hata oluştu.");
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container mx-auto p-8">
            <h1 className="text-3xl font-bold text-gray-800 mb-6">Ürün İzlenebilirlik Sorgulama</h1>
            
            {/* Sorgulama Formu */}
            <form onSubmit={handleSearch} className="bg-white p-4 rounded-lg shadow-md mb-8 flex items-center space-x-4">
                <input 
                    type="text"
                    value={serialNumber}
                    onChange={(e) => setSerialNumber(e.target.value)}
                    placeholder="Seri Numarası Girin (Örn: SN-001)"
                    className="flex-grow px-3 py-2 border rounded-md"
                />
                <button type="submit" disabled={loading} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded disabled:bg-gray-400">
                    {loading ? 'Aranıyor...' : 'Ara'}
                </button>
            </form>

            {/* Sonuç Alanı */}
            {error && <p className="text-center text-red-500 p-4 bg-red-100 rounded-md">{error}</p>}
            
            {traceData && (
                <div className="bg-white p-6 rounded-lg shadow-md animate-fade-in">
                    <h2 className="text-2xl font-bold mb-4 border-b pb-2">İzlenebilirlik Sonuçları</h2>
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm mb-6">
                        <div>
                            <p className="text-gray-500">Seri Numarası</p>
                            <p className="font-bold text-lg font-mono">{traceData.serialNumber}</p>
                        </div>
                        <div>
                            <p className="text-gray-500">Parti Numarası</p>
                            <p className="font-bold text-lg font-mono">{traceData.batchNumber}</p>
                        </div>
                        <div>
                            <p className="text-gray-500">Ürün Adı</p>
                            <p className="font-bold text-lg">{traceData.productName}</p>
                        </div>
                        <div>
                            <p className="text-gray-500">Mevcut Durum</p>
                            <p className="font-bold text-lg">{traceData.currentStatus}</p>
                        </div>
                    </div>
                    
                    <h3 className="text-xl font-semibold mb-4 mt-6">Üretim Geçmişi</h3>
                    <Timeline history={traceData.history} />
                </div>
            )}
        </div>
    );
};

export default TraceabilityPage;