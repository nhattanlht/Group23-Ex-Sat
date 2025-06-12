import { Eye } from "lucide-react";
import { useLanguage } from "../../contexts/LanguageContext";
import { useState } from "react";
import IdentificationPopup from "../IdentificationPopup";

export const ViewIdentificationBtn = ({ identification }) => {
    const [isPopupVisible, setIsPopupVisible] = useState(false);
    const { translate } = useLanguage();

    return (
        <div className="relative group">
            <button
                className="btn btn-secondary h-8 px-3 flex items-center justify-center"
                onClick={() => setIsPopupVisible(true)}
                title={translate('student.tooltips.view_identification')}
            >
                <Eye size={16} className="mr-2" />
                {translate('student.actions.view')}
            </button>
            {isPopupVisible && (
                <IdentificationPopup
                    identification={identification}
                    onClose={() => setIsPopupVisible(false)}
                />
            )}
        </div>
    );
};