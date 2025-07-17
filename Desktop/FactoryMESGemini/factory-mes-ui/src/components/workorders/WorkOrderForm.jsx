import React, { useState, useEffect } from 'react';
import { getAllProducts, getAllMachines } from '../../services/apiService';
import { Save, X } from 'lucide-react';

// Türkçe - İngilizce eşleme
const statusMap = {
  'Planlandı': 'Planned',
  'Beklemede': 'Pending',
  'Üretimde': 'InProgress',
  'Tamamlandı': 'Completed',
  'İptal': 'Canceled',
  'Planned': 'Planned',
  'InProgress': 'InProgress',
  'Completed': 'Completed',
  'Canceled': 'Canceled',
  'Pending': 'Pending'
};

const reverseStatusMap = {
  Planned: 'Planlandı',
  InProgress: 'Üretimde',
  Completed: 'Tamamlandı',
  Canceled: 'İptal',
  Pending: 'Beklemede'
};

const WorkOrderForm = ({ onSubmit, onCancel, initialData }) => {
  const toLocalISOString = (date) => {
    const offset = date.getTimezoneOffset();
    const localDate = new Date(date.getTime() - offset * 60000);
    return localDate.toISOString().slice(0, 16);
  };

  const [formData, setFormData] = useState({
    orderNumber: '',
    productId: '',
    machineId: '',
    plannedQuantity: 1,
    plannedStartDate: toLocalISOString(new Date()),
    plannedEndDate: toLocalISOString(new Date(new Date().setDate(new Date().getDate() + 1))),
    status: 'Planned',
  });

  const [products, setProducts] = useState([]);
  const [machines, setMachines] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [productRes, machineRes] = await Promise.all([
          getAllProducts(),
          getAllMachines(),
        ]);
        const products = productRes.data || [];
        const machines = machineRes.data || [];

        setProducts(products);
        setMachines(machines);

        const defaultProductId = String(products[0]?.id || '');
        const defaultMachineId = String(machines[0]?.id || '');

        setFormData((prev) => {
          if (initialData) {
            const matchedProduct = products.find(p => p.id === initialData.productId);
            const matchedMachine = machines.find(m => m.id === initialData.machineId);

            return {
              orderNumber: initialData.orderNumber || '',
              productId: matchedProduct ? String(matchedProduct.id) : defaultProductId,
              machineId: matchedMachine ? String(matchedMachine.id) : defaultMachineId,
              plannedQuantity: initialData.plannedQuantity || 1,
              plannedStartDate: toLocalISOString(new Date(initialData.plannedStartDate)),
              plannedEndDate: toLocalISOString(new Date(initialData.plannedEndDate)),
              status: statusMap[initialData.status] || 'Planned',
            };
          } else {
            return {
              ...prev,
              productId: defaultProductId,
              machineId: defaultMachineId,
            };
          }
        });
      } catch (err) {
        console.error("Form verileri alınamadı:", err);
      } finally {
        setIsLoading(false);
      }

    };
    fetchData();
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: ['productId', 'machineId'].includes(name)
        ? String(value)
        : name === 'plannedQuantity'
        ? parseInt(value, 10)
        : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ ...formData, id: parseInt(initialData?.id, 10) });
  };

  if (isLoading) return <p className="text-center p-4">Yükleniyor...</p>;

  return (
    
    <form onSubmit={handleSubmit} className="bg-white dark:bg-gray-800 p-6 rounded-xl shadow space-y-6">
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">İş Emri Numarası</label>
          <input
            type="text"
            name="orderNumber"
            value={formData.orderNumber}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Durum</label>
          <select
            name="status"
            value={formData.status}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          >
            {Object.entries(reverseStatusMap).map(([value, label]) => (
              <option key={value} value={value}>{label}</option>
            ))}
          </select>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Ürün</label>
          <select
            name="productId"
            value={formData.productId}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          >
            {products.map(product => (
              <option key={product.id} value={String(product.id)}>{product.name}</option>
            ))}
          </select>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Makine</label>
          <select
            name="machineId"
            value={formData.machineId}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          >
            {machines.map(machine => (
              <option key={machine.id} value={String(machine.id)}>{machine.name}</option>
            ))}
          </select>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Planlanan Adet</label>
          <input
            type="number"
            name="plannedQuantity"
            value={formData.plannedQuantity}
            onChange={handleChange}
            min="1"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Başlangıç Tarihi</label>
          <input
            type="datetime-local"
            name="plannedStartDate"
            value={formData.plannedStartDate}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Bitiş Tarihi</label>
          <input
            type="datetime-local"
            name="plannedEndDate"
            value={formData.plannedEndDate}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          />
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

export default WorkOrderForm;
