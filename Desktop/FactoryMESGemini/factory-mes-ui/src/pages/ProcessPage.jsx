import React, { useState, useEffect } from 'react';
import { getAllProcesses, createProcess, updateProcess, deleteProcess } from '../services/apiService';
import ProcessForm from '../components/processes/ProcessForm';

const ProcessPage = () => {
    const [processes, setProcesses] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [showForm, setShowForm] = useState(false);
    const [editingProcess, setEditingProcess] = useState(null);

    const loadProcesses = async () => {
        try {
            setLoading(true);
            const response = await getAllProcesses();
            setProcesses(response.data);
            setError(null);
        } catch (err) {
            setError('Prosesler yüklenirken bir sorun oluştu.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadProcesses();
    }, []);

    const handleOpenForm = (process = null) => {
        setEditingProcess(process);
        setShowForm(true);
    };

    const handleCloseForm = () => {
        setEditingProcess(null);
        setShowForm(false);
    };

    const handleDelete = async (processId) => {
        if (window.confirm("Bu prosesi silmek istediğinizden emin misiniz?")) {
            try {
                await deleteProcess(processId);
                loadProcesses();
            } catch (error) {
                alert("Proses silinirken bir hata oluştu.");
            }
        }
    };

    const handleFormSubmit = async (formData) => {
        try {
            if (editingProcess) {
                await updateProcess(editingProcess.id, formData);
            } else {
                await createProcess(formData);
            }
            handleCloseForm();
            loadProcesses();
        } catch (error) {
            alert("İşlem sırasında bir hata oluştu.");
        }
    };

    if (loading) return <p className="text-center p-8">Yükleniyor...</p>;
    if (error) return <p className="text-center text-red-500 p-8">{error}</p>;

    return (
        <div className="container mx-auto p-8">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-3xl font-bold text-gray-800">Proses Yönetimi</h1>
                <button onClick={() => handleOpenForm(null)} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded shadow-lg">
                    + Yeni Proses Ekle
                </button>
            </div>
            
            {showForm && <ProcessForm onSubmit={handleFormSubmit} onCancel={handleCloseForm} initialData={editingProcess} />}

            <div className="bg-white shadow-md rounded-lg overflow-hidden">
                <table className="min-w-full">
                    <thead className="bg-gray-800 text-white">
                        <tr>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">ID</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">Proses Adı</th>
                            <th className="w-1/2 text-left py-3 px-4 uppercase font-semibold text-sm">Açıklama</th>
                            <th className="text-left py-3 px-4 uppercase font-semibold text-sm">İşlemler</th>
                        </tr>
                    </thead>
                    <tbody className="text-gray-700">
                        {processes.map((process) => (
                            <tr key={process.id} className="border-b border-gray-200 hover:bg-gray-100">
                                <td className="text-left py-3 px-4">{process.id}</td>
                                <td className="text-left py-3 px-4 font-medium">{process.name}</td>
                                <td className="text-left py-3 px-4">{process.description}</td>
                                <td className="text-left py-3 px-4">
                                    <button onClick={() => handleOpenForm(process)} className="text-sm bg-green-500 text-white py-1 px-3 rounded mr-2 hover:bg-green-600">Düzenle</button>
                                    <button onClick={() => handleDelete(process.id)} className="text-sm bg-red-500 text-white py-1 px-3 rounded hover:bg-red-600">Sil</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default ProcessPage;