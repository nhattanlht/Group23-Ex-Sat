import axios from "axios";
import config from '../config';

export const loadData = async (dataName, page, filters = {}) => {
    try {
        console.log('Calling API with:', { dataName, page, filters });
        const queryString = buildQueryString(filters);
        const url = queryString
            ? `${config.backendUrl}/api/${dataName}/search?${queryString}&page=${page}&pageSize=10`
            : `${config.backendUrl}/api/${dataName}?page=${page}&pageSize=10`;
        console.log('Making request to URL:', url);
        const response = await axios.get(url, 
            {
                headers: {
                    'Accept-Language': localStorage.getItem('language') || 'vi'
                }
            }
        );
        console.log('API response data:', response.data.data);
        return {
            success: true,
            message: response.data.message,
            data: response.data.data
        };
    } catch (error) {
        console.error(`Error loading ${dataName}:`, error);
        throw error.response?.data.message || error.response?.data.errors || "Lỗi không xác định khi tải dữ liệu.";
    }
}

export const loadDataNoPaging = async (endpoint) => {
    try {
        const response = await axios.get(`${config.backendUrl}/api/${endpoint}`, 
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Accept-Language': localStorage.getItem('language') || 'vi'
                }
            }
        );
        return {
            success: true,
            message: response.data.message,
            data: response.data.data
        };
    } catch (error) {
        console.error(`Error loading data from ${endpoint}:`, error);
        throw {
            success: false,
            message: error.response?.data?.message || error.message,
            error: error.response?.data.errors || error
        };
    }
}

export const loadDataId = async (dataName, id) => {
    try {
        const response = await axios.get(`${config.backendUrl}/api/${dataName}/${id}`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept-Language': localStorage.getItem('language') || 'vi'
            }
        });
        return {
            success: true,
            message: response.data.message,
            data: response.data.data
        };
    } catch (error) {
        console.error(`Error loading data with ID ${id} from ${dataName}:`, error);
        throw {
            success: false,
            message: error.response?.data?.message || error.message,
            error: error.response?.data.errors || error
        };
    }
}

export const handleAddRow = async (dataName, data) => {
    try {
        const response = await axios.post(`${config.backendUrl}/api/${dataName}`, data, {
            headers: {
                'Content-Type': 'application/json',
                'Accept-Language': localStorage.getItem('language') || 'vi'
            }
        });
        return {
            success: true,
            message: response.data.message,
            data: response.data.data
        };
    } catch (error) {
        console.error("Lỗi khi thêm dữ liệu:", error);
        throw {
            success: false,
            message: error.response?.data?.message || error.message,
            error: error.response?.data.errors || error
        };
    }
}

export const handleEditRow = async (dataName, id, data, routeGenerator = null) => {
    let url = `${config.backendUrl}/api/${dataName}/${id}`;
    if (routeGenerator) {
      url = `${config.backendUrl}/api/${dataName}/${routeGenerator(data)}`;
    }
  try {
    const response = await axios.put(url, data, {
      headers: {
        "Content-Type": "application/json",
        "Accept-Language": localStorage.getItem("language") || "vi",
      },
    });
    return {
      success: response.status === 200,
      message: response.data.message,
      data: response.data.data,
    };
  } catch (error) {
    console.error("Lỗi khi cập nhật dữ liệu:", error);
    throw {
      success: false,
      message: error.response?.data?.message || error.message,
      error: error.response?.data.errors || error,
    };
  }
};

export const handleDeleteRow = async (dataName, id) => {
    try {
        const response = await axios.delete(`${config.backendUrl}/api/${dataName}/${id}`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept-Language': localStorage.getItem('language') || 'vi'
            }
        });
        return {
            success: true,
            message: response.data.message,
            data: response.data.data
        };
    } catch (error) {
        console.error("Lỗi khi xóa dữ liệu:", error);
        throw {
            success: false,
            message: error.response?.data?.message || error.message,
            error: error.response?.data.errors || error
        };
    }
}

const buildQueryString = (filters) => {
    if (!filters || Object.keys(filters).length === 0) return '';
    return Object.entries(filters)
        .filter(([_, value]) => value !== undefined && value !== null && value !== '')
        .map(([key, value]) => `${key}=${encodeURIComponent(value)}`)
        .join('&');
}