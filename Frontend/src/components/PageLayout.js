import React, { useState } from 'react';
import Sidebar from "./Sidebar";
import LanguageToggle from './LanguageToggle';

const PageLayout = ({ children, title = "Dashboard" }) => {
  const [isSidebarCollapsed, setIsSidebarCollapsed] = useState(false);

  return (
    <div className="flex h-screen bg-gray-100">
      <Sidebar isCollapsed={isSidebarCollapsed} onToggle={setIsSidebarCollapsed} />
      <div className={`flex-1 overflow-auto transition-all duration-300 ${isSidebarCollapsed ? 'ml-16' : 'ml-64'}`}>
        <div className="p-8">
          <div className="flex justify-between items-center mb-6">
            <h1 className="text-2xl font-semibold text-gray-800">{title}</h1>
            <LanguageToggle />
          </div>
          <div className="mt-6">
            {children}
          </div>
        </div>
      </div>
    </div>
  );
};

export default PageLayout;