import React from 'react';
import { CheckCircle, XCircle, Clock, Cog, User, Package } from 'lucide-react';

const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString('tr-TR', {
        year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit'
    });
};

const TimelineItem = ({ step, isLast }) => {
    const isSuccess = step.result === 'Başarılı';
    
    return (
        <div className="relative pl-8">
            {!isLast && <div className="absolute left-3 top-5 h-full w-0.5 bg-gray-300 dark:bg-gray-700"></div>}
            <div className="absolute left-0 top-2 flex h-6 w-6 items-center justify-center rounded-full bg-blue-500 text-white">
                <Cog size={14} />
            </div>
            <div className="ml-4">
                <div className="flex items-center justify-between">
                    <h4 className="font-bold text-gray-800 dark:text-white">{step.processName || 'Bilinmeyen İşlem'}</h4>
                    {isSuccess ? 
                        <span className="flex items-center text-xs font-semibold text-green-600 bg-green-100 dark:bg-green-900/50 dark:text-green-400 px-2 py-1 rounded-full">
                            <CheckCircle size={14} className="mr-1" /> Başarılı
                        </span> :
                        <span className="flex items-center text-xs font-semibold text-red-600 bg-red-100 dark:bg-red-900/50 dark:text-red-400 px-2 py-1 rounded-full">
                            <XCircle size={14} className="mr-1" /> Başarısız
                        </span>
                    }
                </div>
                <div className="mt-2 text-sm text-gray-600 dark:text-gray-400 space-y-1">
                    <p className="flex items-center"><Package size={14} className="mr-2 text-gray-400"/>Makine: <span className="font-medium ml-1">{step.machineName}</span></p>
                    <p className="flex items-center"><User size={14} className="mr-2 text-gray-400"/>Operatör: <span className="font-medium ml-1">{step.operatorUsername}</span></p>
                    <p className="flex items-center"><Clock size={14} className="mr-2 text-gray-400"/>Zaman: <span className="font-mono text-xs ml-1">{formatDate(step.processStartTime)}</span></p>
                </div>
            </div>
        </div>
    );
};

const Timeline = ({ history }) => {
    if (!history || history.length === 0) {
        return <p className="text-center text-gray-500">Bu birim için geçmiş kaydı bulunamadı.</p>;
    }

    return (
        <div className="space-y-8">
            {history.map((step, index) => (
                <TimelineItem key={index} step={step} isLast={index === history.length - 1} />
            ))}
        </div>
    );
};

export default Timeline;