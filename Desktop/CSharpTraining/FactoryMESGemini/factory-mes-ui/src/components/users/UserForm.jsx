import React, { useState } from 'react';

const UserForm = ({ onSubmit, onCancel }) => {
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        firstName: '',
        lastName: '',
        email: '',
        sicilNo: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        const isNumeric = name === 'sicilNo';
        setFormData(prevState => ({ 
            ...prevState, 
            [name]: isNumeric ? (value === '' ? '' : parseInt(value, 10)) : value 
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <div className="bg-white p-6 rounded-lg shadow-md border mb-8">
            <h2 className="text-xl font-bold mb-4 text-gray-800">Yeni Kullanıcı Ekle</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="username">Kullanıcı Adı</label>
                    <input type="text" name="username" value={formData.username} onChange={handleChange} required className="mt-1 block w-full px-3 py-2 border rounded-md"/>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="password">Şifre</label>
                    <input type="password" name="password" value={formData.password} onChange={handleChange} required className="mt-1 block w-full px-3 py-2 border rounded-md"/>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="firstName">İsim</label>
                    <input type="text" name="firstName" value={formData.firstName} onChange={handleChange} required className="mt-1 block w-full px-3 py-2 border rounded-md"/>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="lastName">Soyisim</label>
                    <input type="text" name="lastName" value={formData.lastName} onChange={handleChange} required className="mt-1 block w-full px-3 py-2 border rounded-md"/>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="sicilNo">Sicil Numarası</label>
                    <input type="number" name="sicilNo" value={formData.sicilNo} onChange={handleChange} className="mt-1 block w-full px-3 py-2 border rounded-md"/>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700" htmlFor="email">Email (İsteğe Bağlı)</label>
                    <input type="email" name="email" value={formData.email} onChange={handleChange} className="mt-1 block w-full px-3 py-2 border rounded-md"/>
                </div>
                <div className="flex items-center justify-end space-x-4 pt-4">
                    <button type="button" onClick={onCancel} className="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded">İptal</button>
                    <button type="submit" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Kullanıcı Oluştur</button>
                </div>
            </form>
        </div>
    );
};

export default UserForm;