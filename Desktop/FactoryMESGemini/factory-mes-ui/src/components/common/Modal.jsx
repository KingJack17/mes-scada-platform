import React from 'react';

const Modal = ({ isOpen, onClose, title, children }) => {
    if (!isOpen) return null;

    return (
        // Arka planı karartan overlay
        <div className="fixed inset-0 bg-black bg-opacity-50 z-40 flex justify-center items-center" onClick={onClose}>
            {/* Modal içeriği */}
            <div className="bg-white rounded-lg shadow-xl w-11/12 md:w-1/2 lg:w-1/3 z-50 animate-fade-in-down" onClick={e => e.stopPropagation()}>
                {/* Modal Başlığı */}
                <div className="border-b px-4 py-3 flex justify-between items-center">
                    <h3 className="text-lg font-semibold text-gray-800">{title}</h3>
                    <button onClick={onClose} className="text-gray-500 hover:text-gray-800 text-2xl leading-none">&times;</button>
                </div>
                {/* Modal Gövdesi */}
                <div className="p-6">
                    {children}
                </div>
            </div>
        </div>
    );
};

export default Modal;