import React, { useState, useEffect } from 'react';
import { getAllMaintenanceRequests, createMaintenanceRequest, deleteMaintenanceRequest } from '../services/apiService';
import MaintenanceRequestForm from '../components/maintenance/MaintenanceRequestForm';

const MaintenancePage = () => {
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showForm, setShowForm] = useState(false);

    const loadRequests = async () => {
        try {
            setLoading(true);
            const response = await getAllMaintenanceRequests();
            setRequests(response.data);
            setError(null);
        } catch (err) {
            setError('Bakım talepleri yüklenirken bir sorun oluştu.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadRequests();
    }, []);

    const handleFormSubmit = async (formData) => {
        try {
            await createMaintenanceRequest(formData);
            setShowForm(false);
            loadRequests();
        } catch (err) {
            alert('Talep oluşturulurken bir hata oluştu.');
            console.error(err);
        }
    };

    // EKSİK OLAN SİLME FONKSİYONU GERİ EKLENDİ
    const handleDelete = async (requestId, description) => {
        if (window.confirm(`'${description}' açıklamasındaki talebi silmek istediğinizden emin misiniz?`)) {
            try {
                await deleteMaintenanceRequest(requestId);
                loadRequests();
            } catch (err) {
                alert('Bakım talebi silinirken bir hata oluştu.');
                console.error(err);
            }
        }
    };

    if (loading) return <p className="text-center p-8">Yükleniyor...</p>;
    if (error) return <p className="text-center text-red-500 p-8">{error}</p>;

    return (
        <div className="container mx-auto p-8">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-3xl font-bold text-gray-800">Bakım Talepleri</h1>
                <button onClick={() => setShowForm(!showForm)} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded shadow-lg">
                    {showForm ? 'Formu Kapat' : '+ Yeni Talep Oluştur'}
                </button>
            </div>
            
            {showForm && <MaintenanceRequestForm onSubmit={handleFormSubmit} onCancel={() => setShowForm(false)} />}

            <div className="bg-white shadow-md rounded-lg overflow-hidden">
                <table className="min-w-full">
                    <thead className="bg-gray-800 text-white">
                        <tr>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">ID</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">Makine Adı</th>
                            <th className="w-1/3 text-left py-3 px-4 uppercase font-semibold text-sm">Açıklama</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">Talebi Açan</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">Tarih</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">Öncelik</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">Durum</th>
                            {/* EKSİK OLAN "İŞLEMLER" BAŞLIĞI GERİ EKLENDİ */}
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">İşlemler</th>
                        </tr>
                    </thead>
                    <tbody className="text-gray-700">
                        {requests.map((req) => (
                            <tr key={req.id} className="border-b border-gray-200 hover:bg-gray-100">
                                <td className="text-left py-3 px-4">{req.id}</td>
                                <td className="text-left py-3 px-4 font-medium">{req.machineName}</td>
                                <td className="text-left py-3 px-4">{req.description}</td>
                                <td className="text-left py-3 px-4">{req.reportedByUserName}</td>
                                <td className="text-left py-3 px-4">{new Date(req.requestDate).toLocaleString('tr-TR')}</td>
                                <td className="text-left py-3 px-4">{req.priority}</td>
                                <td className="text-left py-3 px-4">
                                    <span className={`py-1 px-3 rounded-full text-xs text-white ${req.status === 'Açık' ? 'bg-red-500' : 'bg-gray-500'}`}>
                                        {req.status}
                                    </span>
                                </td>
                                {/* EKSİK OLAN "SİL" BUTONU VE SÜTUNU GERİ EKLENDİ */}
                                <td className="text-left py-3 px-4">
                                    <button 
                                        onClick={() => handleDelete(req.id, req.description)} 
                                        className="text-sm bg-red-500 text-white py-1 px-3 rounded hover:bg-red-600">
                                        Sil
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default MaintenancePage;