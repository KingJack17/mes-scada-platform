import React from 'react';
import { Pie } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';

// Chart.js'e pasta grafiği için gerekli elemanları tanıtıyoruz.
ChartJS.register(ArcElement, Tooltip, Legend);

const PieChart = ({ chartData }) => {
    return (
        <div className="bg-white p-4 rounded-lg shadow-md">
            <Pie data={chartData} />
        </div>
    );
};

export default PieChart;