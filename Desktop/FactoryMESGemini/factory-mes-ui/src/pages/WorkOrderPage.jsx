import React, { useState, useEffect } from 'react';
import {
  getAllWorkOrders,
  deleteWorkOrder,
  createWorkOrder,
  updateWorkOrder,
  updateWorkOrderStatus,
  getAllProducts,
  getAllMachines
} from '../services/apiService';
import WorkOrderForm from '../components/workorders/WorkOrderForm';
import { Plus, Trash2, Edit } from 'lucide-react';

const statusTextMap = {
  'InProgress': 'Üretimde',
  'Completed': 'Tamamlandı',
  'Canceled': 'İptal',
  'Pending': 'Beklemede',
  'Planlandı': 'Planlandı'
};

const statusColors = {
  'Planlandı': 'bg-gray-200 text-gray-800',
  'InProgress': 'bg-blue-200 text-blue-800',
  'Completed': 'bg-green-200 text-green-800',
  'Canceled': 'bg-red-200 text-red-800',
  'Pending': 'bg-yellow-200 text-yellow-800'
};

const WorkOrderPage = () => {
  const [workOrders, setWorkOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [editingWorkOrder, setEditingWorkOrder] = useState(null);
  const [statusDropdownOpenId, setStatusDropdownOpenId] = useState(null);
  const [products, setProducts] = useState([]);
  const [machines, setMachines] = useState([]);

  const loadWorkOrders = async () => {
    try {
      setLoading(true);
      const [workOrdersRes, productsRes, machinesRes] = await Promise.all([
        getAllWorkOrders(),
        getAllProducts(),
        getAllMachines()
      ]);
      const sorted = workOrdersRes.data.sort((a, b) => b.id - a.id);
      setWorkOrders(sorted);
      setProducts(productsRes.data);
      setMachines(machinesRes.data);
      setError(null);
    } catch (err) {
      setError('İş emirleri yüklenirken bir sorun oluştu.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadWorkOrders();
  }, []);

  const handleDelete = async (id, orderNumber) => {
    if (window.confirm(`'${orderNumber}' numaralı iş emrini silmek istediğinizden emin misiniz?`)) {
      try {
        await deleteWorkOrder(id);
        loadWorkOrders();
      } catch (err) {
        alert('İş emri silinirken bir hata oluştu.');
        console.error(err);
      }
    }
  };

  const handleOpenForm = (workOrder = null) => {
    if (workOrder) {
      const product = products.find(p => p.name === workOrder.productName);
      const machine = machines.find(m => m.name === workOrder.machineName);
      const enrichedWorkOrder = {
        ...workOrder,
        productId: product?.id,
        machineId: machine?.id
      };
      setEditingWorkOrder(enrichedWorkOrder);
    } else {
      setEditingWorkOrder(null);
    }
    setShowForm(true);
  };

  const handleCloseForm = () => {
    setShowForm(false);
    setEditingWorkOrder(null);
  };

  // === GÜNCELLENEN VE HATAYI ÇÖZEN FONKSİYON ===
  const handleFormSubmit = async (formData) => {
    try {
      if (editingWorkOrder) {
        // --- DÜZENLEME SENARYOSU ---
        // Backend'deki UpdateWorkOrderDto'ya tam olarak uyan bir veri paketi hazırlıyoruz.
        const updateData = {
          id: editingWorkOrder.id,
          orderNumber: formData.orderNumber,
          productId: Number(formData.productId),
          machineId: Number(formData.machineId),
          plannedQuantity: Number(formData.plannedQuantity),
          plannedStartDate: new Date(formData.plannedStartDate).toISOString(),
          plannedEndDate: new Date(formData.plannedEndDate).toISOString(),
          status: formData.status
        };
        await updateWorkOrder(editingWorkOrder.id, updateData);
      } else {
        // --- YENİ KAYIT SENARYOSU ---
        // Backend'deki CreateWorkOrderDto'ya tam olarak uyan bir veri paketi hazırlıyoruz.
        // Bu DTO'da machineId olmadığını unutmuyoruz.
        const createData = {
          orderNumber: formData.orderNumber,
          productId: Number(formData.productId),
          plannedQuantity: Number(formData.plannedQuantity),
          plannedStartDate: new Date(formData.plannedStartDate).toISOString(),
          plannedEndDate: new Date(formData.plannedEndDate).toISOString()
        };
        await createWorkOrder(createData);
      }

      handleCloseForm();
      loadWorkOrders();

    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + (err.response?.data?.title || err.response?.data || err.message));
      console.error(err);
    }
  };

  const handleStatusChange = async (id, newStatus) => {
    try {
      await updateWorkOrderStatus(id, { status: newStatus });
      setStatusDropdownOpenId(null);
      // Listeyi yeniden çekmek yerine state'i anlık güncellemek daha iyi bir kullanıcı deneyimi sunar
      setWorkOrders(prev =>
        prev.map(wo => wo.id === id ? { ...wo, status: newStatus } : wo)
      );
    } catch (err) {
      alert('Durum güncellenirken bir hata oluştu.');
      console.error(err);
    }
  };

  if (loading) return <p className="text-center p-8">Yükleniyor...</p>;
  if (error) return <p className="text-center text-red-500 p-8">{error}</p>;

  return (
    <div className="container mx-auto p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-800 dark:text-white">İş Emirleri Yönetimi</h1>
          <button
            onClick={() => handleOpenForm(null)}
            className="inline-flex items-center gap-2 w-full sm:w-auto bg-blue-600 hover:bg-blue-700 text-white font-medium px-4 py-2 rounded shadow transition"
          >
            {showForm ? 'Formu Kapat' : (<><Plus size={16} /> Yeni İş Emri Ekle</>)}
          </button>
      </div>

      {showForm && (
        <WorkOrderForm
          onSubmit={handleFormSubmit}
          onCancel={handleCloseForm}
          initialData={editingWorkOrder}
        />
      )}

      <div className="bg-white dark:bg-gray-800 shadow-md rounded-lg overflow-x-auto">
        <table className="min-w-full text-sm text-left text-gray-700 dark:text-gray-200">
          <thead className="bg-gray-800 dark:bg-gray-700 text-white text-sm uppercase">
            <tr>
              <th className="px-4 py-3">İş Emri No</th>
              <th className="px-4 py-3">Ürün Adı</th>
              <th className="px-4 py-3">Makine Adı</th>
              <th className="px-4 py-3">Planlanan</th>
              <th className="px-4 py-3">Üretilen</th>
              <th className="px-4 py-3">Başlangıç Tarihi</th>
              <th className="px-4 py-3">Bitiş Tarihi</th>
              <th className="px-4 py-3">Durum</th>
              <th className="px-4 py-3">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {workOrders.map((wo) => (
              <tr key={wo.id} className="border-b hover:bg-gray-50 dark:hover:bg-gray-700">
                <td className="px-4 py-3 font-mono">{wo.orderNumber}</td>
                <td className="px-4 py-3">{wo.productName}</td>
                <td className="px-4 py-3">{wo.machineName}</td>
                <td className="px-4 py-3">{wo.plannedQuantity}</td>
                <td className="px-4 py-3">{wo.actualQuantity}</td>
                <td className="px-4 py-3">{new Date(wo.plannedStartDate).toLocaleString('tr-TR')}</td>
                <td className="px-4 py-3">{new Date(wo.plannedEndDate).toLocaleString('tr-TR')}</td>
                <td className="px-4 py-3 relative">
                  <button
                    onClick={() => setStatusDropdownOpenId(prev => prev === wo.id ? null : wo.id)}
                    className={`py-1 px-3 rounded-full text-xs font-semibold ${statusColors[wo.status] || 'bg-gray-200 text-gray-800'}`}
                  >
                    {statusTextMap[wo.status] || wo.status}
                  </button>
                  {statusDropdownOpenId === wo.id && (
                    <div className="absolute z-10 mt-1 bg-white border border-gray-200 rounded shadow-md w-32">
                      {[
                        { label: 'Üretimde', value: 'InProgress' },
                        { label: 'Tamamlandı', value: 'Completed' },
                        { label: 'İptal', value: 'Canceled' },
                        { label: 'Beklemede', value: 'Pending' }
                      ].map(({ label, value }) => (
                        <div
                          key={value}
                          onClick={() => handleStatusChange(wo.id, value)}
                          className="px-4 py-2 cursor-pointer hover:bg-gray-100 text-sm"
                        >
                          {label}
                        </div>
                      ))}
                    </div>
                  )}
                </td>
                <td className="px-4 py-3 space-x-2">
                  <button
                    onClick={() => handleOpenForm(wo)}
                    title="Düzenle"
                    className="inline-flex items-center gap-1 text-sm bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600"
                  >
                    <Edit size={14} />
                  </button>
                  <button
                    onClick={() => handleDelete(wo.id, wo.orderNumber)}
                    className="text-sm bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600"
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

export default WorkOrderPage;