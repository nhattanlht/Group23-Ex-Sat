import { Eye } from "lucide-react";
import { useLanguage } from "../../contexts/LanguageContext";

export const ViewIdentificationBtn = ({ identification }) => {
  const { translate } = useLanguage();

  const formatIdentification = (identification) => {
    if (!identification) return translate('student.identification.not_available');
    const parts = [];
    if (identification.identificationType) parts.push(identification.identificationType);
    if (identification.identificationNumber) parts.push(identification.identificationNumber);
    if (identification.issuedDate) parts.push(new Date(identification.issuedDate).toLocaleDateString());
    if (identification.issuedPlace) parts.push(identification.issuedPlace);
    return parts.join(" - ") || translate('student.identification.not_available');
  };

  if (!identification) return translate('student.identification.not_available');
  const formattedIdentification = formatIdentification(identification);

  return (
    <div className="relative group">
      <button
        className="btn btn-secondary h-8 px-3 flex items-center justify-center"
        title={formattedIdentification}
      >
        <Eye size={16} className="mr-2" />
        {translate('student.actions.view')}
      </button>
      <div className="absolute z-10 invisible group-hover:visible bg-gray-800 text-white text-sm rounded-md p-2 min-w-[200px] bottom-full left-1/2 transform -translate-x-1/2 mb-2 shadow-lg">
        {formattedIdentification}
        <div className="absolute bottom-0 left-1/2 transform -translate-x-1/2 translate-y-1/2 rotate-45 w-2 h-2 bg-gray-800"></div>
      </div>
    </div>
  );
}; 