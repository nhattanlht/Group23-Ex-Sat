import axios from "axios";
import config from '../config';

export const loadData = async (dataName, page, filters = '') => {
    try {
        const queryString = buildQueryString(filters);
        const url = queryString
            ? `${config.backendUrl}/api/${dataName}/search?${queryString}&page=${page}&pageSize=10`
            : `${config.backendUrl}/api/${dataName}?page=${page}&pageSize=10`;
        const response = await axios.get(url);
        return response.data;
    } catch (error) {
        console.error("Lỗi khi tải dữ liệu:", error);
        return null;
    }
}

export const loadDataNoPaging = async (dataName) => {
    try {
        const response = await axios.get(`${config.backendUrl}/api/${dataName}`);
        return response.data;
    } catch (error) {
        console.error("Lỗi khi tải dữ liệu:", error);
        return null;
    }
}

export const handleAddRow = async (dataName, data) => {
    try {
        const response = await axios.post(`${config.backendUrl}/api/${dataName}`, data);
        return response.data;
    } catch (error) {
        console.error("Lỗi khi thêm dữ liệu:", error);
        throw error;
    }
}

export const handleEditRow = async (dataName, id, data) => {
    try {
        const response = await axios.put(`${config.backendUrl}/api/${dataName}/${id}`, data);
        alert('Cập nhật dữ liệu thành công');
        return response.data;
    } catch (error) {
        console.error("Lỗi khi cập nhật dữ liệu:", error);
        throw error;
    }
}

export const handleDeleteRow = async (dataName, id) => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa dòng này không?')) return;
    try {
        const response = await axios.delete(`${config.backendUrl}/api/${dataName}/${id}`);
        alert('Xóa dữ liệu thành công');
        return response.data;
    } catch (error) {
        alert(error.response?.data?.message || 'Lỗi không xác định');
        console.error("Lỗi khi xóa dữ liệu:", error);
        return null;
    }
}

const buildQueryString = (filters) => {
    const params = new URLSearchParams();

    Object.entries(filters).forEach(([key, value]) => {
        if (value !== null && value !== undefined && value !== "") {
            params.append(key, value);
        }
    });

    return params.toString();
};