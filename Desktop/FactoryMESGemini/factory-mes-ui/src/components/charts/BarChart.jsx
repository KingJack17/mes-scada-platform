import React from 'react';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const BarChart = ({ chartData, chartOptions }) => {
    const options = {
        responsive: true,
        plugins: {
            legend: { position: 'top' },
            title: { display: true, text: chartOptions.title }
        }
    };

    return (
        <div className="bg-white p-4 rounded-lg shadow-md">
            <Bar options={options} data={chartData} />
        </div>
    );
};

export default BarChart;