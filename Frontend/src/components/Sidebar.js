import { useState } from "react";
import { Menu } from "lucide-react";

const Sidebar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const pages = [
        { name: "Sinh viên", href: "/students" },
        { name: "Khoa", href: "/departments" },
        { name: "Chương trình", href: "/programs" },
        { name: "Tình trạng", href: "/statuses" },
    ];

    return (
        <div className="h-full flex items-center">
            {/* Sidebar Icon */}
            <div 
                className="p-2 bg-gray-800 text-white cursor-pointer rounded-r-lg hover:bg-gray-700 transition-all"
                onMouseEnter={() => setIsOpen(true)}
            >
                <Menu size={24} />
            </div>
            
            {/* Sidebar Menu */}
            <div 
                className={`fixed left-0 top-0 h-full bg-gray-900 text-white w-48 p-4 shadow-lg transform ${isOpen ? "translate-x-0" : "-translate-x-full"} transition-transform duration-300`}
                onMouseLeave={() => setIsOpen(false)}
            >
                <ul>
                    {pages.map((page, index) => (
                        <li key={index} className="mb-4 hover:bg-gray-700 p-2 rounded">
                            <a href={page.href} className="block">{page.name}</a>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default Sidebar;
