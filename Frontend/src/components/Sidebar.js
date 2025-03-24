import { useState } from "react";
import { Menu } from "lucide-react";
import { DynamicIcon } from 'lucide-react/dynamic';
import { Link } from "react-router-dom";
const Sidebar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const pages = [
        { name: "Sinh viên", href: "/students", icon: 'circle-user-round' },
        { name: "Nhập/xuất file", href: '/data', icon: 'file-spreadsheet' },
        { name: "Khoa", href: "/departments", icon: 'building-2' },
        { name: "Chương trình", href: "/programs", icon: 'book-open' },
        { name: "Tình trạng", href: "/statuses", icon: 'user-round-check' },
    ];

    return (
        <div className="h-auto flex items-center">
            {/* Sidebar */}
            <div
                className={`h-full bg-gray-900 text-white ${isOpen ? "w-48" : "w-16"} p-4 shadow-lg transition-all duration-300 flex flex-col items-center`}
                onMouseEnter={() => setIsOpen(true)}
                onMouseLeave={() => setIsOpen(false)}
            >
                <ul className="w-full">
                    <li className="block pointer mb-4 hover:bg-gray-700 py-2 rounded flex items-center gap-2"><Menu size={24} /></li>
                    {pages.map((page, index) => (
                        <li key={index} className="block mb-4 hover:bg-gray-700 py-2 rounded flex items-center gap-2">
                            <Link to={page.href}>
                                <div className="flex space-x-2 items-center">
                                    <DynamicIcon name={page.icon} size={24} />
                                    {isOpen && <span> {page.name}</span>}
                                </div>
                            </Link>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default Sidebar;
