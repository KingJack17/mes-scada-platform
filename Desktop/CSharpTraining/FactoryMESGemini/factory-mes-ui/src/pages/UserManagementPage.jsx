import React, { useState, useEffect } from 'react';
import { getAllUsers, deleteUser } from '../services/apiService';
import authService from '../services/authService';
import Modal from '../components/common/Modal';
import UserRoleManager from '../components/users/UserRoleManager';
import UserForm from '../components/users/UserForm';

const UserManagementPage = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isRoleModalOpen, setIsRoleModalOpen] = useState(false);
    const [selectedUser, setSelectedUser] = useState(null);
    const [showCreateForm, setShowCreateForm] = useState(false);

    const fetchUsers = async () => {
        try {
            setLoading(true);
            const response = await getAllUsers();
            setUsers(response.data);
            setError(null);
        } catch (error) {
            setError("Kullanıcılar yüklenemedi. Bu sayfayı görmek için 'Admin' rolüne sahip olmalısınız.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const handleDelete = async (userId, username) => {
        if (window.confirm(`'${username}' kullanıcısını silmek istediğinizden emin misiniz?`)) {
            try {
                await deleteUser(userId);
                fetchUsers();
            } catch (error) {
                alert('Kullanıcı silinirken bir hata oluştu.');
            }
        }
    };

    const handleCreateUserSubmit = async (formData) => {
        try {
            await authService.register(formData);
            setShowCreateForm(false);
            fetchUsers();
        } catch (error) {
            alert("Kullanıcı oluşturulurken hata oluştu: " + (error.response?.data || error.message));
        }
    };

    const handleOpenRoleModal = (user) => {
        setSelectedUser(user);
        setIsRoleModalOpen(true);
    };

    const handleCloseRoleModal = () => {
        setIsRoleModalOpen(false);
        setSelectedUser(null);
    };

    if (loading) return <p className="text-center p-8">Kullanıcılar yükleniyor...</p>;
    if (error) return <p className="text-center text-red-500 p-8">{error}</p>;

    return (
        <div className="container mx-auto p-8">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-3xl font-bold text-gray-800">Kullanıcı Yönetimi</h1>
                <button onClick={() => setShowCreateForm(!showCreateForm)} className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded">
                    {showCreateForm ? 'Formu Kapat' : '+ Yeni Kullanıcı Ekle'}
                </button>
            </div>

            {showCreateForm && <UserForm onSubmit={handleCreateUserSubmit} onCancel={() => setShowCreateForm(false)} />}

            <div className="bg-white shadow-md rounded-lg overflow-hidden">
                <table className="min-w-full">
                    <thead className="bg-gray-800 text-white">
                        <tr>
                            <th className="text-left py-3 px-4">ID</th>
                            <th className="text-left py-3 px-4">Kullanıcı Adı</th>
                            <th className="text-left py-3 px-4">Ad Soyad</th>
                            <th className="text-left py-3 px-4">Sicil No</th>
                            <th className="text-left py-3 px-4">Roller</th>
                            <th className="text-left py-3 px-4">İşlemler</th>
                        </tr>
                    </thead>
                    <tbody className="text-gray-700">
                        {users.map(user => (
                            <tr key={user.id} className="border-b hover:bg-gray-100">
                                <td className="py-3 px-4">{user.id}</td>
                                <td className="py-3 px-4">{user.username}</td>
                                <td className="py-3 px-4">{`${user.firstName} ${user.lastName}`}</td>
                                <td className="py-3 px-4">{user.sicilNo}</td>
                                <td className="py-3 px-4">
                                    {user.roles?.map(role => (
                                        <span key={role} className="bg-purple-200 text-purple-800 py-1 px-2 rounded-full text-xs mr-2">
                                            {role}
                                        </span>
                                    ))}
                                </td>
                                <td className="py-3 px-4">
                                    <button onClick={() => handleOpenRoleModal(user)} className="text-sm bg-indigo-500 text-white py-1 px-3 rounded mr-2 hover:bg-indigo-600">Rolleri Yönet</button>
                                    <button onClick={() => handleDelete(user.id, user.username)} className="text-sm bg-red-500 text-white py-1 px-3 rounded hover:bg-red-600">Sil</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <Modal isOpen={isRoleModalOpen} onClose={handleCloseRoleModal} title={`${selectedUser?.username} - Rolleri Yönet`}>
                {selectedUser && (
                    <UserRoleManager user={selectedUser} onRolesChanged={() => {
                        handleCloseRoleModal();
                        fetchUsers();
                    }}/>
                )}
            </Modal>
        </div>
    );
};

export default UserManagementPage;