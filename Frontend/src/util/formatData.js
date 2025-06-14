import { ViewIdentificationBtn } from "../components/Buttons/ViewIdentificationBtn";
import { Eye } from "lucide-react";
import { useLanguage } from '../contexts/LanguageContext';

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
                        console.log('Processing permanentAddress, ID:', addressId);
                        const address = addressId ? helpers.addresses?.[addressId] : null;
                        console.log('Found address:', address);
                        formattedRow[key] = <ViewAddressBtn address={address} />;
                    } else if (key === 'temporaryAddress') {
                        const addressId = row.temporaryAddressId || row.TemporaryAddressId;
                        console.log('Processing temporaryAddress, ID:', addressId);
                        const address = addressId ? helpers.addresses?.[addressId] : null;
                        console.log('Found address:', address);
                        formattedRow[key] = <ViewAddressBtn address={address} />;
                    } else if (key === 'registeredAddress') {
                        const addressId = row.registeredAddressId || row.RegisteredAddressId;
                        console.log('Processing registeredAddress, ID:', addressId);
                        const address = addressId ? helpers.addresses?.[addressId] : null;
                        console.log('Found address:', address);
                        formattedRow[key] = <ViewAddressBtn address={address} />;
                    }
                    break;

                case "select":
                    if (key === 'departmentId') {
                        formattedRow[key] = row.Department?.Name || row.department?.name || 'N/A';
                    } else if (key === 'schoolYearId') {
                        formattedRow[key] = row.SchoolYear?.Name || row.schoolYear?.name || 'N/A';
                    } else if (key === 'studyProgramId') {
                        formattedRow[key] = row.StudyProgram?.Name || row.studyProgram?.name || 'N/A';
                    } else if (key === 'statusId') {
                        formattedRow[key] = row.StudentStatus?.Name || row.studentStatus?.name || 'N/A';
                    } else if (key === 'Gender') {
                        formattedRow[key] = row.gender || row.Gender || 'N/A';
                    } else if (key === 'identificationType') {
                        const idType = row.identification?.identificationType || row.identificationType;
                        formattedRow[key] = idType || 'N/A';
                    } else {
                        formattedRow[key] = getNameById(value, field.options, helpers.translate) ?? value;
                    }
                    break;

                case "date":
                    if (key === 'DateOfBirth') {
                        formattedRow[key] = row.dateOfBirth ? new Date(row.dateOfBirth).toLocaleDateString() : 
                                          row.DateOfBirth ? new Date(row.DateOfBirth).toLocaleDateString() : 'N/A';
                    } else {
                        formattedRow[key] = value ? new Date(value).toLocaleDateString() : "";
                    }
                    break;

                case "checkbox":
                    formattedRow[key] = value ? "Có" : "Không";
                    break;

                default:
                    if (key === 'FullName') {
                        formattedRow[key] = row.fullName || row.FullName || 'N/A';
                    } else if (key === 'StudentId') {
                        formattedRow[key] = row.studentId || row.StudentId || 'N/A';
                    } else if (key === 'PhoneNumber') {
                        formattedRow[key] = row.phoneNumber || row.PhoneNumber || 'N/A';
                    } else if (key === 'Nationality') {
                        formattedRow[key] = row.nationality || row.Nationality || 'N/A';
                    } else {
                        formattedRow[key] = value || 'N/A';
                    }
            }
        });

        formattedRow.__original = row;
        return formattedRow;
    });
};

const ViewAddressBtn = ({ address }) => {
  const { translate } = useLanguage();
  
  const formatAddress = (address) => {
    if (!address) return translate('student.address.not_available');
    console.log('Formatting address:', address);
    const parts = [];
    if (address.houseNumber || address.HouseNumber) parts.push(address.houseNumber || address.HouseNumber);
    if (address.streetName || address.StreetName) parts.push(address.streetName || address.StreetName);
    if (address.ward || address.Ward) parts.push(address.ward || address.Ward);
    if (address.district || address.District) parts.push(address.district || address.District);
    if (address.province || address.Province) parts.push(address.province || address.Province);
    if (address.country || address.Country) parts.push(address.country || address.Country);
    const formattedAddress = parts.join(", ") || translate('student.address.not_available');
    console.log('Formatted address result:', formattedAddress);
    return formattedAddress;
  };

  console.log('ViewAddressBtn received address:', address);
  if (!address) return translate('student.address.not_available');
  const formattedAddress = formatAddress(address);
  
  return (
    <div className="relative group">
      <button 
        className="btn btn-secondary h-8 px-3 flex items-center justify-center"
        title={formattedAddress}
      >
        <Eye size={16} className="mr-2" />
        {translate('student.actions.view')}
      </button>
      <div className="absolute z-10 invisible group-hover:visible bg-gray-800 text-white text-sm rounded-md p-2 min-w-[200px] bottom-full left-1/2 transform -translate-x-1/2 mb-2 shadow-lg">
        {formattedAddress}
        <div className="absolute bottom-0 left-1/2 transform -translate-x-1/2 translate-y-1/2 rotate-45 w-2 h-2 bg-gray-800"></div>
      </div>
    </div>
  );
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
