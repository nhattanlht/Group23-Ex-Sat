import React from 'react';
import { useLanguage } from '../contexts/LanguageContext';
import { Languages } from 'lucide-react';

const LanguageToggle = () => {
  const { currentLanguage, toggleLanguage } = useLanguage();

  return (
    <button
      onClick={toggleLanguage}
      className="flex items-center gap-2 px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500"
      title={currentLanguage === 'vi' ? 'Switch to English' : 'Chuyển sang tiếng Việt'}
    >
      <Languages size={20} />
      <span>{currentLanguage === 'vi' ? 'EN' : 'VI'}</span>
    </button>
  );
};

export default LanguageToggle; 