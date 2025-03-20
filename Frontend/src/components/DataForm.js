import React, { useState, useEffect } from "react";
import axios from 'axios';
import config from '../config';

const DataForm = ({ fields, data, onSave, onClose }) => {
    const [formData, setFormData] = useState({});

    useEffect(() => {
        const initializeFormData = async () => {
            const initialData = fields.reduce((acc, field) => {
                if (field.type === "group") {
                    // Initialize nested group fields
                    acc[field.accessor] = field.fields.reduce((subAcc, subField) => {
                        subAcc[subField.accessor] = "";
                        return subAcc;
                    }, {});
                } else {
                    // Initialize flat fields
                    acc[field.accessor] = "";
                }
                return acc;
            }, {});

            // Populate with existing data
            if (data) {
                Object.keys(data).forEach((key) => {
                    initialData[key] = data[key];
                });

                if (data.diaChiNhanThuId) {
                    try {
                        const response = await axios.get(`${config.backendUrl}/api/address/${data.diaChiNhanThuId}`);
                        initialData.diaChiNhanThu = response.data; // Populate address fields
                    } catch (error) {
                        console.error("Error fetching address:", error);
                    }
                }

                if (data.diaChiThuongTruId) {
                    try {
                        const response = await axios.get(`${config.backendUrl}/api/address/${data.diaChiThuongTruId}`);
                        initialData.diaChiThuongTru = response.data; // Populate address fields
                    } catch (error) {
                        console.error("Error fetching address:", error);
                    }
                }

                if (data.diaChiTamTruId) {
                    try {
                        const response = await axios.get(`${config.backendUrl}/api/address/${data.diaChiTamTruId}`);
                        initialData.diaChiTamTru = response.data; // Populate address fields
                    } catch (error) {
                        console.error("Error fetching address:", error);
                    }
                }

                try {
                    const response = await axios.get(`${config.backendUrl}/api/identification/${data.identificationId}`);
                    initialData.identification = response.data; // Populate identification fields
                    initialData.identification["issueDate"] = initialData.identification["issueDate"].split("T")[0];
                    initialData.identification["expiryDate"] = initialData.identification["expiryDate"].split("T")[0];
                } catch (error) {
                    console.error("Error fetching identification:", error);
                }

                initialData.identificationType = initialData.identification["identificationType"];
            }

            setFormData(initialData);
        };

        initializeFormData();
    }, [data, fields]);

    const handleChange = (e) => {
        const { name, value, type } = e.target;
        setFormData((formData) => {
            const updatedFormData = {...formData,
                [name]: type === "number" ? Number(value) : value}
            
            if (name === "identificationType") {
                updatedFormData.identification = {}; // Clear existing identification fields
            }
    
            return updatedFormData;
        });
    };

    const handleGroupChange = (groupAccessor, subFieldAccessor, value) => {
        setFormData((prev) => ({
            ...prev,
            [groupAccessor]: {
                ...prev[groupAccessor],
                [subFieldAccessor]: value, // Use subFieldAccessor directly
            },
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(formData);
        onClose();
    };

    return (
        <div className="modal show" style={{ display: 'block' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{data ? 'Sửa Thông Tin Sinh Viên' : 'Thêm Sinh Viên'}</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">
                        <div className="p-4 border rounded-lg bg-white shadow-md">
                            <form onSubmit={handleSubmit} className="grid grid-cols-3 gap-2">
                                {fields
                                    .filter((field) => !field.hidden)
                                    .map((field) => (
                                        <div key={field.accessor} className={field.type === "group" ? "col-span-3" : ""}>
                                            <label htmlFor={field.accessor}>{field.display}</label>

                                            {/* Detect input type */}
                                            {field.type === "select" ? (
                                                <select
                                                    id={field.accessor}
                                                    name={field.accessor}
                                                    value={formData[field.accessor] || ""}
                                                    onChange={handleChange}
                                                    required={field.required}
                                                    className="w-full border p-2"

                                                >
                                                    <option value="">Select an option</option>
                                                    {field.options.map((option) => (
                                                        <option key={option.id} value={option.id}>{option.name}</option>
                                                    ))}
                                                </select>
                                            ) : field.type === "group" ? (
                                                <div className="mb-3 border border-gray-300 p-2 rounded-md grid grid-cols-3 gap-2">
                                                    {field.fields.map((subField) => {
                                                        if (subField.condition && !subField.condition(formData)) {
                                                            return null; // Skip rendering this field
                                                        }
                                                        if (subField.hidden) {
                                                            return null;
                                                        }
                                                        
                                                        return data ? (
                                                        <div key={subField.accessor} className="mb-2">
                                                            <label htmlFor={`${field.accessor}.${subField.accessor}`}>{subField.display}</label>
                                                            <input
                                                                id={`${field.accessor}.${subField.accessor}`}
                                                                name={`${field.accessor}.${subField.accessor}`}
                                                                type={subField.type || "text"} // Default to "text"
                                                                value={formData[field.accessor]?.[subField.accessor.split(".")[1]] || ""}
                                                                onChange={(e) =>
                                                                    handleGroupChange(field.accessor, subField.accessor.split(".")[1], e.target.value)
                                                                }
                                                                className="w-full border p-2"
                                                                required={subField.required}
                                                            />
                                                        </div>
                                                        ) : (
                                                            <div key={subField.accessor} className="mb-2">
                                                            <label htmlFor={`${field.accessor}.${subField.accessor}`}>{subField.display}</label>
                                                            <input
                                                                id={`${field.accessor}.${subField.accessor}`}
                                                                name={`${field.accessor}.${subField.accessor}`}
                                                                type={subField.type || "text"} // Default to "text"
                                                                value={formData[field.accessor]?.[subField.accessor] || ""}
                                                                onChange={(e) =>
                                                                    handleGroupChange(field.accessor, subField.accessor, e.target.value)
                                                                }
                                                                className="w-full border p-2"
                                                                required={subField.required}
                                                            />
                                                        </div>
                                                        );
                                                    })}
                                                </div>
                                            ) : (
                                                <input
                                                    id={field.accessor}
                                                    name={field.accessor}
                                                    type={field.type || "text"} // Default to "text"
                                                    value={field.type === "date" ?
                                                        (formData[field.accessor]?.split("T")[0]) :
                                                        (formData[field.accessor] || '')}
                                                    onChange={handleChange}
                                                    className="w-full border p-2"
                                                    required={field.required}
                                                />
                                            )}
                                        </div>
                                    ))}

                                <div className="flex justify-end gap-2 col-span-3">
                                    <button type="button" className="btn btn-secondary" onClick={onClose}>Hủy</button>
                                    <button type="submit" className="btn btn-primary ">{data ? "Cập nhật" : "Thêm"}</button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default DataForm;