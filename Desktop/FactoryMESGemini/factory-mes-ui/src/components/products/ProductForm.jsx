import React, { useState, useEffect } from 'react';
import { getAllUnitOfMeasures } from '../../services/apiService';
import { Save, X } from 'lucide-react';

const ProductForm = ({ onSubmit, onCancel, initialData = null }) => {
  const [formData, setFormData] = useState({
    productCode: '',
    name: '',
    productType: 'Component',
    minStockLevel: 0,
    stockingUnitOfMeasureId: '',
  });

  const [units, setUnits] = useState([]);

  useEffect(() => {
    getAllUnitOfMeasures()
      .then((response) => {
        setUnits(response.data);
        if (!initialData && response.data.length > 0) {
          setFormData((prev) => ({ ...prev, stockingUnitOfMeasureId: response.data[0].id }));
        }
      })
      .catch((error) => console.error('Ölçü birimleri çekilemedi:', error));
  }, []);

  useEffect(() => {
    if (initialData) {
      setFormData({
        productCode: initialData.productCode || '',
        name: initialData.name || '',
        productType: initialData.productType || 'Component',
        minStockLevel: initialData.minStockLevel || 0,
        stockingUnitOfMeasureId: initialData.stockingUnitOfMeasureId || '',
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white dark:bg-gray-800 p-6 rounded-xl shadow space-y-6">
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
        {/* Ürün Kodu */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Ürün Kodu</label>
          <input
            type="text"
            name="productCode"
            value={formData.productCode}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Adı */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Adı</label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Tipi */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Tipi</label>
          <select
            name="productType"
            value={formData.productType}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="Component">Component</option>
            <option value="RawMaterial">Raw Material</option>
            <option value="FinishedGood">Finished Good</option>
          </select>
        </div>

        {/* Min Stok */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Min. Stok</label>
          <input
            type="number"
            name="minStockLevel"
            value={formData.minStockLevel}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        {/* Ölçü Birimi */}
        <div className="sm:col-span-2">
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Ölçü Birimi</label>
          <select
            name="stockingUnitOfMeasureId"
            value={formData.stockingUnitOfMeasureId}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="" disabled>Seçiniz</option>
            {units.map((unit) => (
              <option key={unit.id} value={unit.id}>{unit.name}</option>
            ))}
          </select>
        </div>
      </div>

      <div className="flex flex-col sm:flex-row justify-end gap-3 pt-4">
        <button
          type="button"
          onClick={onCancel}
          className="flex items-center justify-center gap-2 px-4 py-2 text-sm bg-gray-200 dark:bg-gray-600 dark:text-white hover:bg-gray-300 dark:hover:bg-gray-500 rounded-md"
        >
          <X className="w-4 h-4" /> İptal
        </button>
        <button
          type="submit"
          className="flex items-center justify-center gap-2 px-4 py-2 text-sm bg-blue-600 hover:bg-blue-700 text-white rounded-md"
        >
          <Save className="w-4 h-4" /> Kaydet
        </button>
      </div>
    </form>
  );
};

export default ProductForm;
