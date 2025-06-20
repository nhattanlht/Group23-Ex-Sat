import React, { useState, useEffect } from "react";
import SelectInput from "./Input/SelectInput";
import EmailInput from "./Input/EmailInput";
import config from "../config";
import { useLanguage } from "../contexts/LanguageContext";
import { X } from "lucide-react";
const DataForm = ({ fields, data, onSave, onClose, label, initializeFormData = null }) => {
    const { translate } = useLanguage();
    const ALLOWED_EMAIL_ENDING = config.ALLOWED_EMAIL_ENDING;
    const [formData, setFormData] = useState({});
    const [errors, setErrors] = useState({});
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        console.log('Initializing form data with fields:', fields, 'and data:', data);
        handleInitializeFormData(fields, data);
    }, [data, fields]);

    const defaultInitializeFormData = () => {
        const initialData = fields.reduce((acc, field) => {
            if (field.type === 'checkbox') {
              acc[field.accessor] = false;
            } else {
              acc[field.accessor] = "";
            }
            return acc;
          }, {});
        
          setFormData(data || initialData);
    }

    const handleInitializeFormData = async (fields, data) => {
        if (initializeFormData) {
            const initialFormData = await initializeFormData(fields, data);
            setFormData(initialFormData || {});
            console.log('custom initializeFormData', initialFormData);
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
            
            return updatedFormData;
        });
        console.log('formData', formData);
    };

    const handleGroupChange = (groupAccessor, subFieldAccessor, value, type) => {
        setFormData((prev) => {
            let newValue = value;

            const updatedFormData = {
                ...prev,
                [groupAccessor]: {
                    ...prev[groupAccessor],
                    [subFieldAccessor]: newValue,
                },
            };
            
            // Handle date fields
            if (type === 'date') {
                if (value) {
                    // Ensure the date is in YYYY-MM-DD format
                    const date = new Date(value);
                    if (!isNaN(date)) {
                        newValue = date.toISOString().split('T')[0];
                    }
                }
            }
 
            if (subFieldAccessor === "identificationType") {
                updatedFormData[groupAccessor] = {[subFieldAccessor]: newValue}; // Clear existing identification fields
            }
            
            return updatedFormData;
        });
        console.log('formData', formData);
    };

    const formatDateValue = (value) => {
        if (!value) return '';
        const date = new Date(value);
        return !isNaN(date) ? date.toISOString().split('T')[0] : value;
    };

    const handleSave = async (e) => {
        if (e) {
            e.preventDefault();
        }
        setIsLoading(true);
        
        try {
            // Validate form data
            const validationErrors = {};

            // Check if there are any validation errors
            if (Object.keys(validationErrors).length > 0) {
                setErrors(validationErrors);
                const firstError = Object.values(validationErrors)[0];
                alert(firstError);
                return;
            }

            // Clear any existing errors
            setErrors({});

            console.log('Saving form data:', formData);

            // Call the onSave function passed from parent
            const success = await onSave(formData);
            
            if (success) {
                // Reset form data and close modal only if save was successful
                setFormData({});
                onClose();
            }
        } catch (error) {
            console.error("Error saving form:", error);
            if (error.error) {
                setErrors(error.error);
                console.log("Validation errors:", errors);
            }
        }finally {
            setIsLoading(false);
        }
        
    };

    function capitalize(string) {
        return string[0].toUpperCase() + string.slice(1);
    }

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
            <div className="bg-white p-6 rounded-lg shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-xl font-bold">
                        {data ? translate(`common.edit`) : translate(`common.add`)} {translate(label)}
                    </h2>
                    <button 
                        type="button" 
                        className="text-gray-500 hover:text-gray-700"
                        onClick={onClose}
                    >
                        <X size={24} />
                    </button>
                </div>

                <form onSubmit={handleSave} className="space-y-4">
                    {fields
                        .filter((field) => !field.hidden)
                        .map((field) => (
                            <div key={field.accessor} className={field.type === "group" ? "border rounded-lg p-4" : ""}>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    {field.display}
                                </label>

                                {field.type === "select" ? (
                                    <SelectInput
                                        options={field.options}
                                        id={field.accessor}
                                        name={field.accessor}
                                        value={formData[field.accessor] || ""}
                                        required={field.required}
                                        onChange={handleChange}
                                        disabled={field.disabled && data}
                                    />
                                ) : field.type === "group" ? (
                                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                        {field.fields.map((subField) => {
                                            if (subField.condition && !subField.condition(formData)) {
                                                return null;
                                            }
                                            if (subField.hidden) {
                                                return null;
                                            }

                                            return (
                                                <div key={subField.accessor}>
                                                    <label className="block text-sm font-medium text-gray-700 mb-1">
                                                        {subField.display}
                                                    </label>
                                                    {subField.type === "select" ? (
                                                        <SelectInput
                                                            options={subField.options}
                                                            id={subField.accessor}
                                                            name={subField.accessor}
                                                            value={formData[field.accessor]?.[subField.accessor] || ""}
                                                            required={subField.required}
                                                            onChange={(e) =>
                                                                handleGroupChange(
                                                                    field.accessor,
                                                                    subField.accessor,
                                                                    e.target.value,
                                                                    subField.type
                                                                )
                                                            }
                                                            disabled={subField.disabled && data}
                                                        />
                                                    ) : (
                                                    <input
                                                        type={subField.type || "text"}
                                                        value={
                                                            subField.type === "date" 
                                                                ? formatDateValue(formData[field.accessor]?.[subField.accessor])
                                                                : formData[field.accessor]?.[subField.accessor] || ""
                                                        }
                                                        onChange={(e) =>
                                                            handleGroupChange(
                                                                field.accessor,
                                                                subField.accessor,
                                                                e.target.value,
                                                                subField.type
                                                            )
                                                        }
                                                        className="w-full border rounded-md p-2"
                                                        required={subField.required}
                                                        
                                                    />
                                                    )}
                                                </div>
                                            );
                                        })}
                                    </div>
                                ) : field.type === "email" ? (
                                    <EmailInput
                                        id={field.accessor}
                                        name={field.accessor}
                                        value={formData[field.accessor] || ""}
                                        required={field.required}
                                        disabled={field.disabled && data}
                                        onChange={handleChange}
                                        ALLOWED_EMAIL_ENDING={ALLOWED_EMAIL_ENDING}
                                    />
                                ) : field.type === "checkbox" ? (
                                    <input
                                        type="checkbox"
                                        id={field.accessor}
                                        name={field.accessor}
                                        checked={formData[field.accessor] || false}
                                        onChange={handleChange}
                                        disabled={field.disabled}
                                        className="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                                    />
                                ) : (
                                    <input
                                        type={field.type || "text"}
                                        id={field.accessor}
                                        name={field.accessor}
                                        value={
                                            field.type === "date"
                                                ? formatDateValue(formData[field.accessor])
                                                : formData[field.accessor] || ""
                                        }
                                        onChange={handleChange}
                                        required={field.required}
                                        disabled={field.disabled && data}
                                        className="w-full border rounded-md p-2"
                                    />
                                )}

                                {errors[capitalize(field.accessor)] && (
                                    <p className="text-red-600 text-sm mt-1">
                                        {errors[capitalize(field.accessor)].map((error, index) => (
                                            <span key={index}>{error}</span>))}
                                    </p>
                                )}
                            </div>
                        ))}

                    <div className="flex justify-end gap-2 mt-6">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-4 py-2 border rounded-md text-gray-700 hover:bg-gray-50"
                        >
                            {translate('student.form.buttons.cancel')}
                        </button>
                        <button
                            type="submit"
                            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50"
                            disabled={isLoading}
                        >
                            {translate('student.form.buttons.save')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default DataForm;