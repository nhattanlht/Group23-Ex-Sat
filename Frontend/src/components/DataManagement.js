import React, { useState, useCallback, useRef } from "react";
import axios from "axios";
import Papa from "papaparse"; // Thêm thư viện xử lý CSV

const DataManagement = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [fileType, setFileType] = useState(""); // Loại file CSV hoặc JSON
    const [loading, setLoading] = useState(false);
    const [importedData, setImportedData] = useState(null); // Dữ liệu sau khi import
    const fileInputRef = useRef(null);
    const API_BASE_URL = process.env.REACT_APP_BACKEND_URL || "http://localhost:5136";

    // Export dữ liệu
    const handleExport = useCallback((type) => {
        setLoading(true);
        window.location.href = `${API_BASE_URL}/api/data/export/${type}`;
        setTimeout(() => setLoading(false), 1000);
    }, [API_BASE_URL]);

    // Chọn định dạng file
    const handleFileTypeChange = (event) => {
        setFileType(event.target.value);
        setSelectedFile(null); // Reset file khi đổi định dạng
        setImportedData(null); // Reset dữ liệu đã import
    };

    // Xử lý chọn file
    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    // Ngăn chọn file nếu chưa chọn định dạng
    const handleFileInputClick = (event) => {
        if (!fileType) {
            event.preventDefault();
            alert("Vui lòng chọn định dạng file trước khi chọn file!");
        }
    };

    // Gửi dữ liệu lên backend
    const sendDataToBackend = async (data) => {
        try {
            setLoading(true);
            const response = await axios.post(`${API_BASE_URL}/api/data/import/json`, data, {
                headers: { "Content-Type": "application/json" },
            });
            setImportedData(response.data);
            alert("Import thành công!");
        } catch (error) {
            console.error("Lỗi import dữ liệu:", error);
            alert("Import thất bại! Kiểm tra lại dữ liệu.");
        } finally {
            setLoading(false);
        }
    };

    // Xử lý import file JSON hoặc CSV
    const handleImport = useCallback(() => {
        if (!fileType) {
            alert("Vui lòng chọn định dạng file trước khi import!");
            return;
        }
        if (!selectedFile) {
            alert("Vui lòng chọn file trước khi import!");
            return;
        }
    
        const reader = new FileReader();
        reader.onload = async (event) => {
            try {
                if (fileType === "json") {
                    const jsonData = JSON.parse(event.target.result);
                    console.log("Dữ liệu JSON đã đọc:", jsonData); // Log dữ liệu JSON
                    setImportedData(jsonData);
                    await sendDataToBackend(jsonData); // Gửi dữ liệu JSON trực tiếp
                } else if (fileType === "csv") {
                    Papa.parse(event.target.result, {
                        header: true,
                        skipEmptyLines: true,
                        complete: async (result) => {
                            console.log("Dữ liệu CSV đã đọc:", result.data); // Log dữ liệu CSV
                            setImportedData(result.data);
                            await sendDataToBackend(result.data); // Gửi dữ liệu CSV
                        },
                    });
                }
            } catch (error) {
                console.error("Lỗi khi xử lý file:", error);
                alert("File không hợp lệ!");
            }
        };
    
        reader.readAsText(selectedFile);
    }, [selectedFile, fileType]);

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
                    accept=".json,.csv"
                    onChange={handleFileChange}
                    className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:border-0 file:text-sm file:font-semibold file:bg-gray-100 file:text-gray-700 hover:file:bg-gray-200"
                    onClick={handleFileInputClick}
                />

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

            {/* Hiển thị dữ liệu JSON hoặc CSV đã import */}
            {importedData && (
                <div className="mt-4 p-4 bg-gray-100 rounded-md">
                    <h3 className="text-lg font-semibold mb-2">Dữ liệu đã import:</h3>
                    <pre className="text-sm text-gray-700 bg-white p-2 rounded-md overflow-x-auto">
                        {JSON.stringify(importedData, null, 2)}
                    </pre>
                </div>
            )}
        </div>
    );
};

export default DataManagement;
