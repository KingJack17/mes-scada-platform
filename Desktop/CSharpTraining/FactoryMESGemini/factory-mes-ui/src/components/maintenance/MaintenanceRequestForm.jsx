import React, { useState, useEffect } from 'react';
import { getAllMachines } from "@/services/apiService";

const MaintenanceRequestForm = ({ onSubmit, onCancel }) => {
    const [formData, setFormData] = useState({
        machineId: '',
        reportedByUserId: 1, // Login sistemi olmadığı için şimdilik varsayılan kullanıcı ID'si 1
        description: '',
        priority: 'Normal'
    });

    const [machines, setMachines] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        getAllMachines()
            .then(response => {
                setMachines(response.data);
                if (response.data.length > 0) {
                    setFormData(prev => ({ ...prev, machineId: response.data[0].id }));
                }
            })
            .catch(error => console.error("Makineler çekilemedi:", error))
            .finally(() => setIsLoading(false));
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        const isNumeric = ['machineId', 'reportedByUserId'].includes(name);
        setFormData(prevState => ({
            ...prevState,
            [name]: isNumeric ? (value === '' ? '' : parseInt(value, 10)) : value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    if (isLoading) return <div className="p-6 text-center">Form için makineler yükleniyor...</div>;

    return (
        <div className="bg-white p-6 rounded-lg shadow-md border mb-8">
            <h2 className="text-xl font-bold mb-4 text-gray-800">Yeni Bakım Talebi Oluştur</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="machineId">Makine</label>
                    <select name="machineId" value={formData.machineId} onChange={handleChange} required className="mt-1 block w-full px-3 py-2 border rounded-md">
                        {machines.map(m => <option key={m.id} value={m.id}>{m.name}</option>)}
                    </select>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="description">Sorun Açıklaması</label>
                    <textarea name="description" value={formData.description} onChange={handleChange} required rows="3" className="mt-1 block w-full px-3 py-2 border rounded-md"></textarea>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="priority">Öncelik</label>
                    <select name="priority" value={formData.priority} onChange={handleChange} className="mt-1 block w-full px-3 py-2 border rounded-md">
                        <option value="Düşük">Düşük</option>
                        <option value="Normal">Normal</option>
                        <option value="Yüksek">Yüksek</option>
                    </select>
                </div>
                <div className="flex items-center justify-end space-x-4 pt-4">
                    <button type="button" onClick={onCancel} className="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded">İptal</button>
                    <button type="submit" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Talep Oluştur</button>
                </div>
            </form>
        </div>
    );
};

export default MaintenanceRequestForm;