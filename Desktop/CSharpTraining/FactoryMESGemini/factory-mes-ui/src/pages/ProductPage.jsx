import React, { useState, useEffect } from 'react';
import { getAllProducts, deleteProduct, createProduct, updateProduct } from '../services/apiService';
import ProductForm from '../components/products/ProductForm';
import { Plus, Trash2, Edit } from 'lucide-react';

const ProductPage = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [editingProduct, setEditingProduct] = useState(null);

  const fetchProducts = async () => {
    try {
      setLoading(true);
      const response = await getAllProducts();
      setProducts(response.data);
      setError(null);
    } catch (err) {
      setError('Veri yüklenirken bir sorun oluştu. API sunucusunun çalıştığından emin olun.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  const handleDelete = async (productId, productName) => {
    if (window.confirm(`'${productName}' adlı ürünü silmek istediğinizden emin misiniz?`)) {
      try {
        await deleteProduct(productId);
        fetchProducts();
      } catch (err) {
        alert('Ürün silinirken bir hata oluştu.');
        console.error(err);
      }
    }
  };

  const handleOpenForm = (product = null) => {
    setEditingProduct(product);
    setShowForm(true);
  };

  const handleCloseForm = () => {
    setShowForm(false);
    setEditingProduct(null);
  };

  const handleFormSubmit = async (formData) => {
    try {
      if (editingProduct) {
        await updateProduct(editingProduct.id, formData);
      } else {
        await createProduct(formData);
      }
      handleCloseForm();
      fetchProducts();
    } catch (err) {
      if (err.response && err.response.status === 409) {
        alert(err.response.data);
      } else {
        alert('İşlem sırasında bir hata oluştu.');
      }
      console.error(err);
    }
  };

  return (
    <div className="container mx-auto px-4 py-6 dark:bg-gray-900 dark:text-white min-h-screen">
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">
            <h1 className="text-3xl font-bold text-gray-800 dark:text-white">Ürün Yönetimi</h1>
            <div className="flex flex-col sm:flex-row gap-3 w-full sm:w-auto">
              <button
                onClick={() => setShowForm(!showForm)}
                className="inline-flex items-center gap-2 w-full sm:w-auto bg-blue-600 hover:bg-blue-700 text-white font-medium px-4 py-2 rounded shadow transition"
              >
                {showForm ? 'Formu Kapat' : (<><Plus size={16} /> Yeni Ürün Ekle</>)}
              </button>
            </div>
          </div>
    

      {showForm && (
        <ProductForm
          onSubmit={handleFormSubmit}
          onCancel={handleCloseForm}
          initialData={editingProduct}
        />
      )}

      {loading ? (
        <p className="text-center text-gray-500 py-8">Ürünler yükleniyor...</p>
      ) : error ? (
        <p className="text-center text-red-500 py-8">{error}</p>
      ) : (
        <div className="overflow-x-auto bg-white dark:bg-gray-800 shadow rounded-lg">
          <table className="min-w-full text-sm text-left text-gray-700 dark:text-gray-200">
            <thead className="bg-gray-800 dark:bg-gray-700 text-white">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Ürün Kodu</th>
                <th className="px-4 py-3">Adı</th>
                <th className="px-4 py-3">Tipi</th>
                <th className="px-4 py-3">Ölçü Birimi</th>
                <th className="px-4 py-3">Min. Stok</th>
                <th className="px-4 py-3">İşlemler</th>
              </tr>
            </thead>
            <tbody>
              {products.map((product) => (
                <tr key={product.id} className="border-b hover:bg-gray-50">
                  <td className="px-4 py-3">{product.id}</td>
                  <td className="px-4 py-3 font-semibold">{product.productCode}</td>
                  <td className="px-4 py-3">{product.name}</td>
                  <td className="px-4 py-3">{product.productType}</td>
                  <td className="px-4 py-3">{product.stockingUnitOfMeasure?.name}</td>
                  <td className="px-4 py-3">{product.minStockLevel}</td>
                  <td className="px-4 py-3 space-x-2">
                    <button
                      onClick={() => handleOpenForm(product)}
                      title="Düzenle"
                      className="inline-flex items-center justify-center p-2 text-white bg-green-500 hover:bg-green-600 rounded"
                    >
                      <Edit size={16} />
                    </button>
                    <button
                      onClick={() => handleDelete(product.id, product.name)}
                      title="Sil"
                      className="inline-flex items-center justify-center p-2 text-white bg-red-500 hover:bg-red-600 rounded"
                    >
                      <Trash2 size={16} />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ProductPage;
