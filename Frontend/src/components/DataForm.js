import React, { useState, useEffect } from "react";
import SelectInput from "./Input/SelectInput";
import EmailInput from "./Input/EmailInput";
import config from "../config";

const DataForm = ({ fields, data, onSave, onClose, label, initializeFormData = null, customInput= [] }) => {
    const ALLOWED_EMAIL_ENDING = config.ALLOWED_EMAIL_ENDING;
    const [formData, setFormData] = useState({});
    const [errors, setErrors] = useState({});
    useEffect(() => {
        handleInitializeFormData(fields, data);
    }, [data, fields]);

    const defaultInitializeFormData = () => {
        setFormData(
            data || fields.reduce((acc, field) => ({ ...acc, [field.accessor]: "" }), {})
        );
    }
    const handleInitializeFormData = async (fields, data) => {
        if (initializeFormData) {
            const initialFormData = await initializeFormData(fields, data);
            setFormData(initialFormData || {});
            console.log('custom initializeFormData', initializeFormData);
        } else {
            defaultInitializeFormData();
            console.log('default initializeFormData');
        }
    }

    const handleChange = (e) => {
        const { name, value, type } = e.target;
        setFormData((formData) => {
            const updatedFormData = {
                ...formData,
                [name]: type === "number" ? Number(value) : value
            }

            if (type === 'checkbox') {
                updatedFormData[name] = e.target.checked
            }
            if (name === "identificationType") {
                updatedFormData.identification = {}; // Clear existing identification fields
            }

            return updatedFormData;
        });
        console.log('formData', formData);

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

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors({});
        try {
            const response = await onSave(formData);
            if (response) {
                onClose();
            }
        } catch (error) {
            if (error.response && error.response.data.errors) {
                setErrors(error.response.data.errors); // Store validation errors
            }
            console.error("Lỗi khi cập nhật dữ liệu", error);
        }
    };

    function capitalize(string) {
        return string[0].toUpperCase() + string.slice(1);
    }
    return (
        <div className="modal show" style={{ display: 'block' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{data ? `Sửa Thông Tin ${label}` : `Thêm ${label}`}</h5>
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
                                                <SelectInput options={field.options}
                                                    id={field.accessor}
                                                    name={field.accessor}
                                                    value={formData[field.accessor] || ""}
                                                    required={field.required}
                                                    onChange={handleChange} />
                                            ) : field.type === "group" ? (
                                                <div className="mb-3 border border-gray-300 p-2 rounded-md grid grid-cols-3 gap-2">
                                                    {field.fields.map((subField) => {
                                                        if (subField.condition && !subField.condition(formData)) {
                                                            return null; // Skip rendering this field
                                                        }
                                                        if (subField.hidden) {
                                                            return null;
                                                        }

                                                        return (
                                                            <div key={subField.accessor} className="mb-2">
                                                                <label htmlFor={`${field.accessor}.${subField.accessor}`}>{subField.display}</label>
                                                                <input
                                                                    id={`${field.accessor}.${subField.accessor}`}
                                                                    name={`${field.accessor}.${subField.accessor}`}
                                                                    type={subField.type || "text"} // Default to "text"
                                                                    value={formData[field.accessor]?.[subField.accessor] || ""}
                                                                    onChange={(e) =>
                                                                        handleGroupChange(field.accessor, subField.accessor, subField.type === "checkbox" ? e.target.checked : e.target.value)
                                                                    }
                                                                    className="w-full border p-2"
                                                                    required={subField.required}
                                                                />
                                                            </div>
                                                        )
                                                    })}
                                                </div>
                                            ) : field.type === "email" ? (
                                                <EmailInput id={field.accessor}
                                                    name={field.accessor}
                                                    value={formData[field.accessor]}
                                                    required={field.required}
                                                    disabled={field.disabled && data}
                                                    onChange={handleChange}
                                                    ALLOWED_EMAIL_ENDING={ALLOWED_EMAIL_ENDING} />
                                            ) : field.type === 'checkbox' ? (
                                                <input
                                                    id={field.accessor}
                                                    name={field.accessor}
                                                    type="checkbox"
                                                    checked={formData[field.accessor] || false}
                                                    onChange={handleChange}
                                                    className='w-full border p-2'
                                                    required={field.required}
                                                    disabled={field.disabled && data}
                                                />
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
                                                    disabled={field.disabled && data}
                                                />
                                            )}
                                            
                                            {errors && errors[capitalize(field.accessor)] && (
                                                <p className="text-red-600">{errors[capitalize(field.accessor)][0]}</p>
                                            )}
                                        </div>
                                    ))}

                                <div className="flex justify-end gap-2 col-span-3">
                                    <button type="button" className="btn btn-secondary" onClick={onClose}>Hủy</button>
                                    <button
                                        type="submit"
                                        className="btn btn-primary"
                                    >
                                        {data ? "Cập nhật" : "Thêm"}
                                    </button>
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