import React, { useState, useEffect } from 'react';
import { getAllMachineTypes } from '../../services/apiService';
import { Save, X } from 'lucide-react';

const MachineForm = ({ onSubmit, onCancel, initialData = null }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    location: '',
    assetTag: '',
    status: 'Stopped',
    machineTypeId: ''
  });

  const [machineTypes, setMachineTypes] = useState([]);

  useEffect(() => {
    getAllMachineTypes()
      .then(response => {
        setMachineTypes(response.data);
        if (!initialData && response.data.length > 0) {
          setFormData(prev => ({ ...prev, machineTypeId: response.data[0].id }));
        }
      })
      .catch(error => console.error("Makine tipleri çekilemedi:", error));
  }, []);

  useEffect(() => {
    if (initialData) {
      setFormData({
        name: initialData.name || '',
        description: initialData.description || '',
        location: initialData.location || '',
        assetTag: initialData.assetTag || '',
        status: initialData.status || 'Stopped',
        machineTypeId: initialData.machineTypeId || ''
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white dark:bg-gray-800 p-6 rounded-xl shadow space-y-6">
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
        {[
          { label: 'Adı', name: 'name' },
          { label: 'Açıklama', name: 'description' },
          { label: 'Konum', name: 'location' },
          { label: 'Varlık Etiketi', name: 'assetTag' }
        ].map(({ label, name }) => (
          <div key={name}>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">{label}</label>
            <input
              type="text"
              name={name}
              value={formData[name]}
              onChange={handleChange}
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              required={name === 'name'}
            />
          </div>
        ))}

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Durum</label>
          <select
            name="status"
            value={formData.status}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="Running">Running</option>
            <option value="Stopped">Stopped</option>
            <option value="Maintenance">Maintenance</option>
          </select>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Makine Tipi</label>
          <select
            name="machineTypeId"
            value={formData.machineTypeId}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="" disabled>Seçiniz</option>
            {machineTypes.map((type) => (
              <option key={type.id} value={type.id}>{type.name}</option>
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

export default MachineForm;
