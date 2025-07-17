import React, { useState, useEffect } from 'react';
import {
  getAllMachines,
  deleteMachine,
  createMachine,
  updateMachine
} from '../services/apiService';
import MachineForm from '../components/machines/MachineForm';
import { Edit, Pencil, Trash2, Plus } from 'lucide-react';
import { useTheme } from '../context/ThemeContext';

const MachinePage = () => {
  const { theme } = useTheme();
  const [machines, setMachines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [editingMachine, setEditingMachine] = useState(null);

const loadMachines = async () => {
  try {
    setLoading(true);
    const response = await getAllMachines();

    // ID'ye göre büyükten küçüğe sırala
    const sortedMachines = response.data.sort((a, b) => b.id - a.id);

    setMachines(sortedMachines);
    setError(null);
  } catch (err) {
    setError('Makineler yüklenirken bir sorun oluştu.');
  } finally {
    setLoading(false);
  }
};

  useEffect(() => {
    loadMachines();
  }, []);

  const handleDelete = async (id, name) => {
    if (window.confirm(`'${name}' adlı makineyi silmek istediğinizden emin misiniz?`)) {
      try {
        await deleteMachine(id);
        loadMachines();
      } catch {
        alert('Silme işlemi başarısız oldu.');
      }
    }
  };

  const handleOpenForm = (machine = null) => {
  if (machine) {
    const enrichedMachine = {
      ...machine,
      machineTypeId: machine.machineTypeId || machine.machineType?.id || ''
    };
    console.log("Editlenecek veri:", enrichedMachine);
    setEditingMachine(enrichedMachine);
  } else {
    setEditingMachine(null);
  }
  setShowForm(true);
};

  const handleCloseForm = () => {
    setShowForm(false);
    setEditingMachine(null);
  };

  const handleFormSubmit = async (formData) => {
    try {
      if (editingMachine) {
        await updateMachine(editingMachine.id, formData);
      } else {
        await createMachine(formData);
      }
      handleCloseForm();
      loadMachines();
    } catch {
      alert('İşlem sırasında hata oluştu.');
    }
  };

  if (loading) return <p className="text-center py-10">Yükleniyor...</p>;
  if (error) return <p className="text-center text-red-500 py-10">{error}</p>;

  return (
    <div className="container mx-auto px-4 py-6 dark:bg-gray-900 dark:text-white min-h-screen">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">
        <h1 className="text-3xl font-bold text-gray-800 dark:text-white">Makine Yönetimi</h1>
        <div className="flex flex-col sm:flex-row gap-3 w-full sm:w-auto">
          <button
            onClick={() => setShowForm(!showForm)}
            className="inline-flex items-center gap-2 w-full sm:w-auto bg-blue-600 hover:bg-blue-700 text-white font-medium px-4 py-2 rounded shadow transition"
          >
            {showForm ? 'Formu Kapat' : (<><Plus size={16} /> Yeni Makine Ekle</>)}
          </button>
        </div>
      </div>

      {showForm && (
        <MachineForm
          onSubmit={handleFormSubmit}
          onCancel={handleCloseForm}
          initialData={editingMachine}
        />
      )}

        <div className="overflow-x-auto bg-white dark:bg-gray-800 shadow rounded-lg">
          <table className="min-w-full text-sm text-left text-gray-700 dark:text-gray-200">
            <thead className="bg-gray-800 dark:bg-gray-700 text-white">
            <tr>
              <th className="px-4 py-3 font-semibold">ID</th>
              <th className="px-4 py-3 font-semibold">Adı</th>
              <th className="px-4 py-3 font-semibold">Açıklama</th>
              <th className="px-4 py-3 font-semibold">Konum</th>
              <th className="px-4 py-3 font-semibold">Tipi</th>
              <th className="px-4 py-3 font-semibold">Durum</th>
              <th className="px-4 py-3 font-semibold">İşlemler</th>
            </tr>
          </thead>
          <tbody className="text-gray-700 dark:text-gray-200">
            {machines.map((machine) => (
              <tr
                key={machine.id}
                className="border-b border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700"
              >
                <td className="px-4 py-3">{machine.id}</td>
                <td className="px-4 py-3 font-medium">{machine.name}</td>
                <td className="px-4 py-3">{machine.description}</td>
                <td className="px-4 py-3">{machine.location}</td>
                <td className="px-4 py-3">{machine.machineTypeName || '-'}</td>
                <td className="px-4 py-3">
                  <span
                    className={`px-3 py-1 text-xs font-semibold rounded-full
                      ${machine.status === 'Running' ? 'bg-green-200 text-green-800' :
                        machine.status === 'Stopped' ? 'bg-red-200 text-red-800' :
                          'bg-yellow-200 text-yellow-800'}`}
                  >
                    {machine.status}
                  </span>
                </td>
                <td className="px-4 py-3 flex items-center gap-2">
                  <button
                    onClick={() => handleOpenForm(machine)}
                    className="inline-flex items-center justify-center p-2 text-white bg-green-500 hover:bg-green-600 rounded"
                    title="Düzenle"
                  >
                    <Edit size={16} />
                  </button>
                  <button
                    onClick={() => handleDelete(machine.id, machine.name)}
                    className="p-2 bg-red-500 hover:bg-red-600 text-white rounded-md"
                    title="Sil"
                  >
                    <Trash2 size={16} />
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

export default MachinePage;
