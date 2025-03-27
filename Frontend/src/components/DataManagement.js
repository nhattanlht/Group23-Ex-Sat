import React, { useState, useCallback, useRef } from "react";
import axios from "axios";

const DataManagement = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [fileType, setFileType] = useState(""); // Loại file CSV hoặc JSON
    const [loading, setLoading] = useState(false);
    const fileInputRef = useRef(null); // Tạo ref cho input file
    const API_BASE_URL = process.env.REACT_APP_BACKEND_URL || "http://localhost:5136";
    const [error, setError] = useState([]);

    // Export dữ liệu
    const handleExport = useCallback((type) => {
        setLoading(true);
        window.location.href = `${API_BASE_URL}/api/data/export/${type}`;
        setTimeout(() => setLoading(false), 1000);
    }, [API_BASE_URL]);

    // Chọn định dạng import
    const handleFileTypeChange = (event) => {
        setFileType(event.target.value);
        setSelectedFile(null); // Reset file khi đổi định dạng
    };

    // Xử lý chọn file import
    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    // Ngăn chọn file nếu chưa chọn định dạng
    const handleFileInputClick = (event) => {
        if (!fileType) {
            event.preventDefault(); // Ngăn không cho mở hộp thoại chọn file
            alert("Vui lòng chọn định dạng file trước khi chọn file!");
        }
    };

    // Import dữ liệu
    const handleImport = useCallback(async () => {
        setError([]);
        if (!fileType) {
            alert("Vui lòng chọn định dạng file trước khi import!");
            return;
        }
        if (!selectedFile) {
            alert("Vui lòng chọn file trước khi import!");
            return;
        }

        const formData = new FormData();
        formData.append("file", selectedFile);
        setLoading(true);

        try {
            const response = await axios.post(
                `${API_BASE_URL}/api/data/import/${fileType}`,
                formData,
                { headers: { "Content-Type": "multipart/form-data" } }
            );
            alert(response.data.message || "Import thành công!");
        } catch (error) {
            console.error("Lỗi khi import:", error);
            alert(error.response?.data?.message || "Lỗi khi import dữ liệu!");
            setError(error.response?.data);
        } finally {
            setLoading(false);
        }
    }, [selectedFile, fileType, API_BASE_URL]);

    return (
        <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-2xl font-bold mb-4 text-center">Quản lý dữ liệu</h2>

            {/* Export dữ liệu */}
            <div className="mb-4 flex justify-between">
                <button
                    onClick={() => handleExport("json")}
                    className={`px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 ${loading ? "opacity-50 cursor-not-allowed" : ""}`}
                    disabled={loading}
                >
                    {loading ? "Đang xuất..." : "Export JSON"}
                </button>
                <button
                    onClick={() => handleExport("csv")}
                    className={`px-4 py-2 bg-green-500 text-white rounded-md hover:bg-green-600 ${loading ? "opacity-50 cursor-not-allowed" : ""}`}
                    disabled={loading}
                >
                    {loading ? "Đang xuất..." : "Export CSV"}
                </button>
            </div>

            {/* Import dữ liệu */}
            <div className="mb-4">
                <h3 className="text-lg font-semibold mb-2">Import dữ liệu</h3>

                {/* Chọn định dạng file */}
                <select
                    onChange={handleFileTypeChange}
                    value={fileType}
                    className="w-full p-2 mb-2 border rounded-md"
                >
                    <option value="">-- Chọn định dạng file --</option>
                    <option value="json">JSON</option>
                    <option value="csv">CSV</option>
                </select>

                {/* Input chọn file */}
                <input
                    ref={fileInputRef}
                    type="file"
                    onChange={handleFileChange}
                    className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:border-0 file:text-sm file:font-semibold file:bg-gray-100 file:text-gray-700 hover:file:bg-gray-200"
                    onClick={handleFileInputClick} // Ngăn chọn file nếu chưa chọn định dạng
                />

                {error && Array.isArray(error) && error.map((row, index) => <p className="text-red-500 text-sm mt-2" key={index}>{ row.errorMessage }</p>)}

                {/* Nút Import */}
                <div className="mt-3 flex justify-center">
                    <button
                        onClick={handleImport}
                        className={`px-4 py-2 bg-purple-500 text-white rounded-md hover:bg-purple-600 ${(!fileType || !selectedFile || loading) ? "opacity-50 cursor-not-allowed" : ""}`}
                        disabled={!fileType || !selectedFile || loading}
                    >
                        {loading ? "Đang import..." : `Import ${fileType.toUpperCase()}`}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default DataManagement;
