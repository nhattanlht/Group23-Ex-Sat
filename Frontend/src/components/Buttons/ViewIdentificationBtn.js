import { useState } from "react";
import IdentificationPopup from "../IdentificationPopup";

export const ViewIdentificationBtn = ({ identification, onClick }) => {
    const [isPopupVisible, setIsPopupVisible] = useState(false);

    return (
        <div>
            <button
                className="btn btn-primary"
                onClick={() => {
                    setIsPopupVisible(true);
                }}
            >
                Xem
            </button>
            {isPopupVisible && (
                <IdentificationPopup
                    identification={identification}
                    onClose={() => setIsPopupVisible(false)}
                />
            )}
        </div>
    )
}