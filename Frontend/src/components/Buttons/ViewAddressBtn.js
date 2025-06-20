import { Eye } from "lucide-react";
import { useLanguage } from '../../contexts/LanguageContext';

export const ViewAddressBtn = ({ address }) => {
    const { translate } = useLanguage();
    
    const formatAddress = (address) => {
      if (!address) return translate('student.address.not_available');
      
      const parts = [];
      if (address.houseNumber || address.HouseNumber) parts.push(address.houseNumber || address.HouseNumber);
      if (address.streetName || address.StreetName) parts.push(address.streetName || address.StreetName);
      if (address.ward || address.Ward) parts.push(address.ward || address.Ward);
      if (address.district || address.District) parts.push(address.district || address.District);
      if (address.province || address.Province) parts.push(address.province || address.Province);
      if (address.country || address.Country) parts.push(address.country || address.Country);
      const formattedAddress = parts.join(", ") || translate('student.address.not_available');
      
      return formattedAddress;
    };
  
    
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