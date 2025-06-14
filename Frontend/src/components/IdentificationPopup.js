import { useLanguage } from "../contexts/LanguageContext";

const IdentificationPopup = ({ identification, onClose }) => {
    const { translate } = useLanguage();
    
    if (!identification) return null;

    return (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
            <div className="bg-white p-6 rounded shadow-lg w-96">
                <h2 className="text-xl font-bold mb-4">{translate('student.identification.details')}</h2>
                <div className="mb-2">
                    <strong>{translate('student.identification.type')}:</strong> {identification.identificationType}
                </div>
                <div className="mb-2">
                    <strong>{translate('student.identification.number')}:</strong> {identification.number}
                </div>
                <div className="mb-2">
                    <strong>{translate('student.identification.issue_date')}:</strong> {new Date(identification.issueDate).toLocaleDateString()}
                </div>
                {identification.expiryDate && (
                    <div className="mb-2">
                        <strong>{translate('student.identification.expiry_date')}:</strong> {new Date(identification.expiryDate).toLocaleDateString()}
                    </div>
                )}
                <div className="mb-2">
                    <strong>{translate('student.identification.issued_by')}:</strong> {identification.issuedBy}
                </div>
                {identification.hasChip !== undefined && (
                    <div className="mb-2">
                        <strong>{translate('student.identification.has_chip')}:</strong> {identification.hasChip ? translate('common.yes') : translate('common.no')}
                    </div>
                )}
                {identification.issuingCountry && (
                    <div className="mb-2">
                        <strong>{translate('student.identification.issuing_country')}:</strong> {identification.issuingCountry}
                    </div>
                )}
                {identification.notes && (
                    <div className="mb-2">
                        <strong>{translate('student.identification.notes')}:</strong> {identification.notes}
                    </div>
                )}
                <button
                    className="btn btn-secondary mt-4"
                    onClick={onClose}
                >
                    {translate('common.close')}
                </button>
            </div>
        </div>
    );
};
  
export default IdentificationPopup;