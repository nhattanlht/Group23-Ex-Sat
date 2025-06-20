import { ViewIdentificationBtn } from "../components/Buttons/ViewIdentificationBtn";
import { ViewAddressBtn } from "../components/Buttons/ViewAddressBtn";

/**
 * Formats a dataSet based on fields config and helper functions.
 *
 * @param {Array<Object>} dataSet - Raw data array.
 * @param {Array<Object>} fields - Field definitions (with type, accessor, etc.).
 * @param {Object} helpers - Custom helper functions and data (e.g., getNameById, etc.).
 * @returns {Array<Object>} Formatted data array, ready for display in table.
 */
export const formatDataSetForTable = (dataSet, fields, helpers = {}) => {
    console.log('formatDataSetForTable received helpers:', helpers);
    console.log('formatDataSetForTable received dataSet:', dataSet);
    return dataSet.map((row) => {
        const formattedRow = {};

        fields.forEach((field) => {
            if (field.hidden) return;

            const key = field.accessor;
            const value = row[key];

            switch (field.type) {
                case "group":
                    if (field.customeType === "identification") {
                        formattedRow[key] = (
                            <ViewIdentificationBtn identification={row.identification} />
                        );
                    } else if (key === 'permanentAddress') {
                        const addressId = row.permanentAddressId || row.PermanentAddressId;
                        
                        const address = addressId ? helpers.addresses?.[addressId] : null;
                        
                        formattedRow[key] = <ViewAddressBtn address={address} />;
                    } else if (key === 'temporaryAddress') {
                        const addressId = row.temporaryAddressId || row.TemporaryAddressId;
                        
                        const address = addressId ? helpers.addresses?.[addressId] : null;
                        
                        formattedRow[key] = <ViewAddressBtn address={address} />;
                    } else if (key === 'registeredAddress') {
                        const addressId = row.registeredAddressId || row.RegisteredAddressId;
                        
                        const address = addressId ? helpers.addresses?.[addressId] : null;
                        
                        formattedRow[key] = <ViewAddressBtn address={address} />;
                    }
                    break;

                case "select":
                    formattedRow[key] = getNameById(value, field.options, helpers.translate) ?? value;
                    break;

                case "date":
                    formattedRow[key] = value ? new Date(value).toLocaleDateString() : "";
                    break;

                case "checkbox":
                    formattedRow[key] = value ? "Có" : "Không";
                    break;

                default:
                    formattedRow[key] = value || 'N/A';
            }
        });

        formattedRow.__original = row;
        return formattedRow;
    });
};

export const getNameById = (id, list, translate) => {
  if (!list || !Array.isArray(list)) return translate('student.address.not_available');
  const item = list.find((item) => String(item.id) === String(id));
  return item ? item.name : translate('student.address.not_available');
};

export const transformToNestedObject = (flatObject) => {
    console.log('before nested', flatObject);
    const nestedObject = {};

    Object.keys(flatObject).forEach((key) => {
        const keys = key.split('.');
        let current = nestedObject;

        keys.forEach((subKey, index) => {
            if (index === keys.length - 1) {
                // If it's the last key, assign the value
                current[subKey] = flatObject[key];
            } else {
                // Otherwise, create an object if it doesn't exist
                current[subKey] = current[subKey] || {};
                current = current[subKey];
            }
        });
    });
    console.log("nestedObject", nestedObject);
    return nestedObject;
};

export const transformToNestedObject2 = (flatObject) => {
    console.log('before nested2', flatObject);
    const nestedObject = {};

    Object.keys(flatObject).forEach((key) => {
        // Directly assign the value to the corresponding key
        nestedObject[key] = flatObject[key];
    });
    console.log("nestedObject2", nestedObject);
    return nestedObject;
};
