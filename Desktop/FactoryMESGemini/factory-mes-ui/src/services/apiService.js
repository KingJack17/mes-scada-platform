import axios from 'axios';

const BASE_URL = 'https://localhost:7007/api'; // Port numaranızı kontrol edin.

// Interceptor için özel bir axios instance'ı oluşturuyoruz.
const api = axios.create({
    baseURL: BASE_URL
});

// Gönderilen her isteğe otomatik olarak token ekleyen interceptor
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// --- YARDIMCI FONKSİYONLAR ---
export const getAllUnitOfMeasures = () => api.get('/unitofmeasures');
export const getAllMachineTypes = () => api.get('/machinetypes');


// --- ÜRÜN FONKSİYONLARI ---
export const getAllProducts = () => api.get('/products');
export const deleteProduct = (id) => api.delete(`/products/${id}`);
export const createProduct = (productDto) => api.post('/products', productDto);
export const updateProduct = (id, productDto) => api.put(`/products/${id}`, productDto);


// --- MAKİNE FONKSİYONLARI ---
export const getAllMachines = () => api.get('/machines');
export const deleteMachine = (id) => api.delete(`/machines/${id}`);
export const createMachine = (machineDto) => api.post('/machines', machineDto);
export const updateMachine = (id, machineDto) => api.put(`/machines/${id}`, machineDto);


// --- İŞ EMRİ FONKSİYONLARI ---
export const getAllWorkOrders = () => api.get('/workorders');
export const createWorkOrder = (workOrderDto) => api.post('/workorders', workOrderDto);
export const deleteWorkOrder = (id) => api.delete(`/workorders/${id}`);
export const updateWorkOrderStatus = (id, statusDto) => api.put(`/workorders/${id}/status`, statusDto);
export const updateWorkOrder = (id, data) => api.put(`/workorders/${id}`, data);

// --- BAKIM TALEBİ FONKSİYONLARI ---
export const getAllMaintenanceRequests = () => api.get('/maintenance/requests');
export const createMaintenanceRequest = (requestDto) => api.post('/maintenance/requests', requestDto);
export const deleteMaintenanceRequest = (id) => api.delete(`/maintenance/requests/${id}`);
export const updateMaintenanceRequestStatus = (id, statusDto) => api.put(`/maintenance/requests/${id}/status`, statusDto);


// --- KİMLİK DOĞRULAMA FONKSİYONLARI (Bunlar token eklenmemesi için global axios'u kullanır) ---
export const login = (userData) => {
    return axios.post(`${BASE_URL}/auth/login`, userData);
};
export const register = (userData) => {
    return axios.post(`${BASE_URL}/auth/register`, userData);
};

// === YENİ DASHBOARD FONKSİYONU ===
export const getDashboardSummary = () => {
    return api.get('/dashboard/summary');
};

export const getMonthlyActivity = () => {
    // Bu, backend'de oluşturduğumuz yeni endpoint'i çağırır.
    return api.get('/dashboard/monthly-activity');
};

export const getDailyProduction = (lastDays = 30) => {
    return api.get(`/dashboard/daily-production/${lastDays}`);
};
// === USERS FONKSİYONU ===
export const getAllUsers = () => api.get('/users');
export const assignRoleToUser = (userId, roleId) => api.post(`/users/${userId}/roles`, { roleId });
export const removeRoleFromUser = (userId, roleId) => api.delete(`/users/${userId}/roles/${roleId}`);
export const getAllRoles = () => {
    // Bu, backend'de oluşturduğumuz GET /api/roles endpoint'ini çağırır.
    return api.get('/roles');
};
export const deleteUser = (id) => {
    return api.delete(`/users/${id}`);
};

export const getOeeForMachine = (machineId, from, to) => {
    // Tarihleri, backend'in bekleyeceği standart ISO formatına çeviriyoruz.
    const fromISO = from.toISOString();
    const toISO = to.toISOString();
    // Korumalı endpoint olduğu için 'api' instance'ını kullanıyoruz.
    return api.get(`/oee/${machineId}?from=${fromISO}&to=${toISO}`);
};