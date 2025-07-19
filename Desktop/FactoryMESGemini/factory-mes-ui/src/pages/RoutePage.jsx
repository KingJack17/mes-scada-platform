import React, { useState, useEffect } from 'react';
import { getAllProducts, getRouteForProduct, addStepToRoute, updateRouteStep, deleteRouteStep } from '../services/apiService';
import RouteStepForm from '../components/routes/RouteStepForm';

const RoutePage = () => {
    const [products, setProducts] = useState([]);
    const [selectedProductId, setSelectedProductId] = useState('');
    const [route, setRoute] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    
    const [showForm, setShowForm] = useState(false);
    const [editingStep, setEditingStep] = useState(null);

    useEffect(() => {
        getAllProducts().then(res => setProducts(res.data));
    }, []);

    const fetchRoute = (productId) => {
        if (!productId) {
            setRoute(null);
            return;
        }
        setLoading(true);
        setError('');
        getRouteForProduct(productId)
            .then(res => setRoute(res.data))
            .catch(err => {
                setRoute({ steps: [] });
                setError('');
            })
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        fetchRoute(selectedProductId);
    }, [selectedProductId]);

    const handleOpenForm = (step = null) => {
        setEditingStep(step);
        setShowForm(true);
    };

    const handleCloseForm = () => {
        setEditingStep(null);
        setShowForm(false);
    };

    const handleDeleteStep = async (routeId) => {
        if (window.confirm("Bu rota adımını silmek istediğinizden emin misiniz?")) {
            try {
                await deleteRouteStep(routeId);
                fetchRoute(selectedProductId);
            } catch (error) {
                alert("Rota adımı silinirken bir hata oluştu.");
            }
        }
    };

    const handleFormSubmit = async (formData) => {
        try {
            // Düzenleme modunda productId'yi initialData'dan almamız gerekebilir
            const dataToSend = editingStep ? { ...formData } : { ...formData, productId: parseInt(selectedProductId) };
            
            if (editingStep) {
                await updateRouteStep(editingStep.routeId, dataToSend);
            } else {
                await addStepToRoute(dataToSend);
            }
            handleCloseForm();
            fetchRoute(selectedProductId);
        } catch (error) {
            alert("İşlem sırasında bir hata oluştu.");
        }
    };

    return (
        <div className="container mx-auto p-8">
            <h1 className="text-3xl font-bold text-gray-800 mb-6">Üretim Rotaları Yönetimi</h1>
            
            <div className="bg-white p-4 rounded-lg shadow-md mb-8">
                <label htmlFor="product-select" className="block text-sm font-medium text-gray-700 mb-2">Ürün Seçin:</label>
                <select id="product-select" value={selectedProductId} onChange={e => setSelectedProductId(e.target.value)} className="block w-full p-2 border rounded-md">
                    <option value="">-- Ürün Seçiniz --</option>
                    {products.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
                </select>
            </div>

            {loading && <p className="text-center">Yükleniyor...</p>}
            
            {selectedProductId && !loading && (
                <div className="bg-white p-6 rounded-lg shadow-md">
                    <div className="flex justify-between items-center mb-4">
                        <h2 className="text-2xl font-bold">{route?.productName || '...'} - Rota Adımları</h2>
                        <button onClick={() => handleOpenForm(null)} className="bg-blue-500 text-white py-2 px-4 rounded">
                            {showForm ? 'Formu Kapat' : 'Yeni Adım Ekle'}
                        </button>
                    </div>

                    {showForm && <RouteStepForm 
                                    onSubmit={handleFormSubmit} 
                                    onCancel={handleCloseForm} 
                                    productId={parseInt(selectedProductId)}
                                    initialData={editingStep} 
                                />}

                    <ul className="list-inside space-y-3">
                        {route?.steps?.map(step => (
                            <li key={step.routeId} className="flex justify-between items-center bg-gray-50 p-3 rounded">
                                <div>
                                    <span className="font-semibold text-lg">Adım {step.stepNumber}:</span>
                                    <span className="ml-4 text-gray-700">{step.processName}</span>
                                    {/* DÜZELTME: Makine adını gösteriyoruz */}
                                    <span className="ml-2 text-sm text-gray-500">({step.machineName || 'Makine Belirtilmemiş'})</span>
                                </div>
                                <div>
                                    <button onClick={() => handleOpenForm(step)} className="text-sm bg-green-500 text-white py-1 px-2 rounded mr-2 hover:bg-green-600">Düzenle</button>
                                    <button onClick={() => handleDeleteStep(step.routeId)} className="text-sm bg-red-500 text-white py-1 px-2 rounded hover:bg-red-600">Sil</button>
                                </div>
                            </li>
                        ))}
                    </ul>
                    {route?.steps?.length === 0 && <p className="text-center text-gray-500">Bu ürün için henüz bir rota adımı tanımlanmamış.</p>}
                </div>
            )}
        </div>
    );
};

export default RoutePage;