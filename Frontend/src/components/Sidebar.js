import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useLanguage } from '../contexts/LanguageContext';
import { ChevronLeft, ChevronRight, Users, BookOpen, GraduationCap, FileSpreadsheet, School, ClipboardList, FileInput } from 'lucide-react';

const Sidebar = ({ isCollapsed, onToggle }) => {
  const location = useLocation();
  const { translate } = useLanguage();

  const menuItems = [
    { path: '/students', icon: <Users size={20} />, label: translate('menu.students') },
    { path: '/departments', icon: <School size={20} />, label: translate('menu.departments') },
    { path: '/programs', icon: <BookOpen size={20} />, label: translate('menu.programs') },
    { path: '/statuses', icon: <ClipboardList size={20} />, label: translate('menu.statuses') },
    { path: '/courses', icon: <GraduationCap size={20} />, label: translate('menu.courses') },
    { path: '/classes', icon: <FileSpreadsheet size={20} />, label: translate('menu.classes') },
    { path: '/enrollment', icon: <ClipboardList size={20} />, label: translate('menu.enrollment') },
    { path: '/data', icon: <FileInput size={20} />, label: translate('menu.import_export') }
  ];

  return (
    <div 
      className={`bg-gray-800 text-white transition-all duration-300 ease-in-out flex flex-col fixed h-screen ${
        isCollapsed ? 'w-16' : 'w-64'
      }`}
    >
      <div className="p-2 flex items-center justify-end border-b border-gray-700">
        <button
          onClick={() => onToggle(!isCollapsed)}
          className="p-1 hover:bg-gray-700 rounded-full transition-colors"
          title={isCollapsed ? translate('menu.expand') : translate('menu.collapse')}
        >
          {isCollapsed ? <ChevronRight size={20} /> : <ChevronLeft size={20} />}
        </button>
      </div>

      <nav className="flex-1 overflow-y-auto">
        <ul className="py-4">
          {menuItems.map((item) => (
            <li key={item.path}>
              <Link
                to={item.path}
                className={`flex items-center px-4 py-3 hover:bg-gray-700 transition-colors ${
                  location.pathname === item.path ? 'bg-gray-700' : ''
                }`}
                title={isCollapsed ? item.label : ''}
              >
                <span className="flex items-center justify-center w-6">{item.icon}</span>
                {!isCollapsed && <span className="ml-3 truncate">{item.label}</span>}
              </Link>
            </li>
          ))}
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;
