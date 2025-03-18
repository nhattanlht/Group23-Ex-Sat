import React, { useState, useEffect } from "react";

const DataForm = ({ fields, data, onSave, onClose }) => {
    const [formData, setFormData] = useState({});

    useEffect(() => {
        setFormData(
            data || fields.reduce((acc, field) => ({ ...acc, [field.accessor]: "" }), {})
        );
    }, [data, fields]);

    const handleChange = (e) => {
        const { name, value, type } = e.target;
        setFormData({
            ...formData,
            [name]: type === "number" ? Number(value) : value
        });
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
                            <form onSubmit={handleSubmit}>
                                {fields.map((field) => (
                                    <div key={field.accessor} className="mb-3">
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

                                <div className="flex justify-end gap-2">
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