const IdentificationPopup = ({ identification, onClose }) => {
    if (!identification) return null;

    return (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
            <div className="bg-white p-6 rounded shadow-lg w-96">
                <h2 className="text-xl font-bold mb-4">Chi Tiết Giấy Tờ</h2>
                <div className="mb-2"><strong>Loại Giấy Tờ:</strong> {identification.identificationType}</div>
                <div className="mb-2"><strong>Số Giấy Tờ:</strong> {identification.number}</div>
                <div className="mb-2"><strong>Ngày Cấp:</strong> {new Date(identification.issueDate).toLocaleDateString()}</div>
                {identification.expiryDate && (
                    <div className="mb-2"><strong>Ngày Hết Hạn:</strong> {new Date(identification.expiryDate).toLocaleDateString()}</div>
                )}
                <div className="mb-2"><strong>Nơi Cấp:</strong> {identification.issuedBy}</div>
                {identification.hasChip !== undefined && (
                    <div className="mb-2"><strong>Có Gắn Chip:</strong> {identification.hasChip ? "Có" : "Không"}</div>
                )}
                {identification.issuingCountry && (
                    <div className="mb-2"><strong>Quốc Gia Cấp:</strong> {identification.issuingCountry}</div>
                )}
                {identification.notes && (
                    <div className="mb-2"><strong>Ghi Chú:</strong> {identification.notes}</div>
                )}
                <button
                    className="btn btn-secondary mt-4"
                    onClick={onClose}
                >
                    Đóng
                </button>
            </div>
        </div>
    );
};
  
export default IdentificationPopup;