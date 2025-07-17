import React from 'react';
import { Line } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

const LineChart = ({ chartData, chartOptions }) => {
    const options = {
        responsive: true,
        plugins: {
            legend: { position: 'top' },
            title: { display: true, text: chartOptions.title }
        },
        scales: {
            y: { beginAtZero: true }
        }
    };

    return (
        <div className="bg-white p-4 rounded-lg shadow-md">
            <Line options={options} data={chartData} />
        </div>
    );
};

export default LineChart;