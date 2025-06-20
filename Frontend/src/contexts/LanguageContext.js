import React, { createContext, useState, useContext } from 'react';
import { en } from '../locales/en';
import { vi } from '../locales/vi';

const LanguageContext = createContext();

export const useLanguage = () => {
  const context = useContext(LanguageContext);
  if (!context) {
    throw new Error('useLanguage must be used within a LanguageProvider');
  }
  return context;
};

export const LanguageProvider = ({ children }) => {
  const storedLanguage = localStorage.getItem('language') || 'vi';
  const [currentLanguage, setCurrentLanguage] = useState(storedLanguage);
  const translations = { en, vi };

  const translate = (key) => {
    const keys = key.split('.');

    let translation = translations[currentLanguage];
    
    for (const k of keys) {
      translation = translation?.[k];
      if (!translation) break;
    }
    
    return translation || key;
  };

  const toggleLanguage = () => {
    setCurrentLanguage(prev => {
      const newLang = prev === 'vi' ? 'en' : 'vi';
      localStorage.setItem('language', newLang);
      return newLang;
    });
  };

  return (
    <LanguageContext.Provider value={{ currentLanguage, translate, toggleLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
}; 