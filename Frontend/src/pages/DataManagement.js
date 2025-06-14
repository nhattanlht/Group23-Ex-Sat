import React, { useState, useCallback, useRef } from "react";
import axios from "axios";
import PageLayout from "../components/PageLayout";
import { useLanguage } from "../contexts/LanguageContext";

const DataManagement = () => {
    const { translate } = useLanguage();
    const [selectedFile, setSelectedFile] = useState(null);
    const [fileType, setFileType] = useState("");
    const [loading, setLoading] = useState(false);
    const fileInputRef = useRef(null);
    const API_BASE_URL = process.env.REACT_APP_BACKEND_URL || "http://localhost:5136";
    const [error, setError] = useState([]);

    const handleExport = useCallback((type) => {
        setLoading(true);
        window.location.href = `${API_BASE_URL}/api/data/export/${type}`;
        setTimeout(() => setLoading(false), 1000);
    }, [API_BASE_URL]);

    const handleFileTypeChange = (event) => {
        setFileType(event.target.value);
        setSelectedFile(null);
    };

    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const handleFileInputClick = (event) => {
        if (!fileType) {
            event.preventDefault();
            alert(translate('data_management.import.select_format_first'));
        }
    };

    const handleImport = useCallback(async () => {
        setError([]);
        if (!fileType) {
            alert(translate('data_management.import.select_format_first'));
            return;
        }
        if (!selectedFile) {
            alert(translate('data_management.import.select_file'));
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
            alert(response.data.message || translate('data_management.import.success'));
        } catch (error) {
            console.error("Lỗi khi import:", error);
            alert(error.response?.data?.message || translate('data_management.import.error'));
            setError(error.response?.data);
        } finally {
            setLoading(false);
        }
    }, [selectedFile, fileType, API_BASE_URL, translate]);

    return (
        <PageLayout title={translate('data_management.title')}>
            <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-lg">
                <h2 className="text-2xl font-bold mb-4 text-center">{translate('data_management.title')}</h2>

                <div className="mb-4 flex justify-between">
                    <button
                        onClick={() => handleExport("json")}
                        className={`px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 ${loading ? "opacity-50 cursor-not-allowed" : ""}`}
                        disabled={loading}
                    >
                        {loading ? translate('data_management.export.exporting') : translate('data_management.export.json_button')}
                    </button>
                    <button
                        onClick={() => handleExport("csv")}
                        className={`px-4 py-2 bg-green-500 text-white rounded-md hover:bg-green-600 ${loading ? "opacity-50 cursor-not-allowed" : ""}`}
                        disabled={loading}
                    >
                        {loading ? translate('data_management.export.exporting') : translate('data_management.export.csv_button')}
                    </button>
                </div>

                <div className="mb-4">
                    <h3 className="text-lg font-semibold mb-2">{translate('data_management.import.title')}</h3>

                    <select
                        onChange={handleFileTypeChange}
                        value={fileType}
                        className="w-full p-2 mb-2 border rounded-md"
                    >
                        <option value="">{translate('data_management.import.format_select')}</option>
                        <option value="json">{translate('data_management.import.json_option')}</option>
                        <option value="csv">{translate('data_management.import.csv_option')}</option>
                    </select>

                    <div className="relative">
                        <input
                            ref={fileInputRef}
                            type="file"
                            onChange={handleFileChange}
                            className="opacity-0 absolute inset-0 w-full h-full cursor-pointer"
                        />
                        <div className="flex items-center">
                            <label 
                                onClick={handleFileInputClick}
                                className="bg-gray-100 text-gray-700 py-2 px-4 rounded text-sm font-semibold hover:bg-gray-200 cursor-pointer"
                            >
                                {translate('data_management.import.choose_file')}
                            </label>
                            <span className="ml-3 text-sm text-gray-500">
                                {selectedFile ? selectedFile.name : translate('data_management.import.no_file')}
                            </span>
                        </div>
                    </div>

                    {error && Array.isArray(error) && error.map((row, index) => (
                        <p className="text-red-500 text-sm mt-2" key={index}>{row.errorMessage}</p>
                    ))}

                    <div className="mt-3 flex justify-center">
                        <button
                            onClick={handleImport}
                            className={`px-4 py-2 bg-purple-500 text-white rounded-md hover:bg-purple-600 ${(!fileType || !selectedFile || loading) ? "opacity-50 cursor-not-allowed" : ""}`}
                            disabled={!fileType || !selectedFile || loading}
                        >
                            {loading ? translate('data_management.import.importing') : `${translate('data_management.import.import_button')} ${fileType.toUpperCase()}`}
                        </button>
                    </div>
                </div>
            </div>
        </PageLayout>
    );
};

export default DataManagement;