import React, { useState, useEffect } from 'react';
import { getAllProcesses, getMachinesByProcess } from '../../services/apiService';

const RouteStepForm = ({ onSubmit, onCancel, productId, initialData = null }) => {
    const [formData, setFormData] = useState({
        productId: productId,
        processId: '',
        machineId: '',
        stepNumber: 10
    });

    const [processes, setProcesses] = useState([]);
    const [machines, setMachines] = useState([]);
    const [loading, setLoading] = useState(true);

    // Component ilk yüklendiğinde tüm prosesleri çek
    useEffect(() => {
        getAllProcesses()
            .then(res => {
                setProcesses(res.data);
                // Eğer düzenleme modunda değilsek, ilk prosesi seçili getir.
                if (!initialData && res.data.length > 0) {
                    setFormData(prev => ({ ...prev, processId: res.data[0].id }));
                }
            })
            .catch(err => console.error("Prosesler yüklenemedi", err))
            .finally(() => setLoading(false));
    }, [initialData]);

    // SEÇİLİ PROSES DEĞİŞTİĞİNDE makineleri yeniden çek
    useEffect(() => {
        if (formData.processId) {
            getMachinesByProcess(formData.processId)
                .then(res => {
                    setMachines(res.data);
                    // Eğer düzenleme modunda değilsek veya makine listesi değiştiyse,
                    // ilk makineyi varsayılan olarak seç.
                    if (!initialData && res.data.length > 0) {
                         setFormData(prev => ({ ...prev, machineId: res.data[0].id }));
                    }
                })
                .catch(err => console.error("Makineler yüklenemedi", err));
        } else {
            setMachines([]); // Proses seçili değilse makine listesini boşalt
        }
    }, [formData.processId, initialData]);

    // Düzenleme modu için formu doldurma
    useEffect(() => {
        if (initialData) {
            setFormData({
                productId: initialData.productId,
                processId: initialData.processId,
                machineId: initialData.machineId,
                stepNumber: initialData.stepNumber
            });
        }
    }, [initialData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevState => ({ 
            ...prevState, 
            [name]: parseInt(value, 10)
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    if (loading) return <p>Prosesler yükleniyor...</p>;

    return (
        <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner border mb-8">
            <h3 className="text-xl font-bold mb-4">
                {initialData ? 'Rota Adımını Düzenle' : 'Yeni Rota Adımı Ekle'}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-sm font-medium" htmlFor="processId">Proses</label>
                    <select name="processId" value={formData.processId} onChange={handleChange} required className="mt-1 block w-full p-2 border rounded-md">
                        <option value="">Seçiniz...</option>
                        {processes.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
                    </select>
                </div>
                <div>
                    <label className="block text-sm font-medium" htmlFor="machineId">Makine</label>
                    <select name="machineId" value={formData.machineId} onChange={handleChange} required disabled={!formData.processId || machines.length === 0} className="mt-1 block w-full p-2 border rounded-md disabled:bg-gray-200">
                        {machines.length > 0 ? (
                            machines.map(m => <option key={m.id} value={m.id}>{m.name}</option>)
                        ) : (
                            <option value="">Bu prosese ait makine bulunamadı</option>
                        )}
                    </select>
                </div>
                <div>
                    <label className="block text-sm font-medium" htmlFor="stepNumber">Adım Numarası</label>
                    <input type="number" name="stepNumber" value={formData.stepNumber} onChange={handleChange} required min="1" className="mt-1 block w-full p-2 border rounded-md"/>
                </div>
                <div className="flex items-center justify-end space-x-4 pt-4">
                    <button type="button" onClick={onCancel} className="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded">İptal</button>
                    <button type="submit" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Kaydet</button>
                </div>
            </form>
        </div>
    );
};

export default RouteStepForm;