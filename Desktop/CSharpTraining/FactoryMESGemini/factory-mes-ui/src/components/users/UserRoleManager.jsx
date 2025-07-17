import React, { useState, useEffect } from 'react';
import { getAllRoles, assignRoleToUser, removeRoleFromUser } from '../../services/apiService';

const UserRoleManager = ({ user, onRolesChanged }) => {
    const [allRoles, setAllRoles] = useState([]);
    const [userRoles, setUserRoles] = useState(user.roles);
    const [selectedRole, setSelectedRole] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getAllRoles().then(res => {
            setAllRoles(res.data);
            // Kullanıcının henüz sahip olmadığı ilk rolü varsayılan olarak seç
            const roleToAdd = res.data.find(r => !user.roles.includes(r.name));
            if(roleToAdd) {
                setSelectedRole(roleToAdd.id);
            }
        }).finally(() => setLoading(false));
    }, [user.roles]);

    const handleAddRole = async () => {
        if (!selectedRole) {
            alert("Lütfen eklenecek bir rol seçin.");
            return;
        }
        try {
            await assignRoleToUser(user.id, selectedRole);
            onRolesChanged(); // Ana sayfadaki listeyi yenilemek için
        } catch (error) {
            alert("Rol eklenirken hata oluştu.");
        }
    };

    const handleRemoveRole = async (roleName) => {
        const roleToRemove = allRoles.find(r => r.name === roleName);
        if (!roleToRemove) return;
        try {
            await removeRoleFromUser(user.id, roleToRemove.id);
            onRolesChanged(); // Ana sayfadaki listeyi yenilemek için
        } catch (error) {
            alert("Rol kaldırılırken hata oluştu.");
        }
    };

    const availableRolesToAdd = allRoles.filter(r => !userRoles.includes(r.name));

    if(loading) return <p>Roller yükleniyor...</p>;

    return (
        <div>
            <h4 className="font-bold mb-2">Mevcut Roller</h4>
            {userRoles.length > 0 ? (
                <ul className="mb-4">
                    {userRoles.map(role => (
                        <li key={role} className="flex justify-between items-center mb-1 bg-gray-100 p-2 rounded">
                            <span>{role}</span>
                            <button onClick={() => handleRemoveRole(role)} className="text-xs bg-red-500 text-white px-2 py-1 rounded hover:bg-red-700">Kaldır</button>
                        </li>
                    ))}
                </ul>
            ) : <p className="text-sm text-gray-500 mb-4">Kullanıcının henüz bir rolü yok.</p>}

            <hr className="my-4"/>

            <h4 className="font-bold mb-2">Yeni Rol Ekle</h4>
            {availableRolesToAdd.length > 0 ? (
                <div className="flex space-x-2">
                    <select value={selectedRole} onChange={e => setSelectedRole(parseInt(e.target.value))} className="flex-grow border rounded p-2">
                        {availableRolesToAdd.map(role => <option key={role.id} value={role.id}>{role.name}</option>)}
                    </select>
                    <button onClick={handleAddRole} className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Ekle</button>
                </div>
            ) : <p className="text-sm text-gray-500">Eklenecek yeni rol bulunmuyor.</p>}
        </div>
    );
};

export default UserRoleManager;