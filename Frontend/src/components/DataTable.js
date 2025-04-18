import React, { useState, useEffect } from "react";
import axios from 'axios';
import config from '../config';
import { Pencil, Trash2 } from "lucide-react";

const DataTable = ({ fields, dataSet, handleEdit, handleDelete }) => {
  const [addresses, setAddresses] = useState({});
  const [identifications, setIdentifications] = useState({});
  const [selectedIdentification, setSelectedIdentification] = useState(null);
  const [isPopupVisible, setIsPopupVisible] = useState(false);
  // Tìm tên theo ID từ danh sách
  const getNameById = (id, list) => {
    if (!list || !Array.isArray(list)) return 'Chưa có'; // Handle undefined or invalid list
    const item = list.find((item) => String(item.id) === String(id));
    return item ? item.name : 'Chưa có';
  };

  const getViewAddress = async (id) => {
    if (!addresses[id]) { // Avoid duplicate API calls for the same ID
      try {
          const response = await axios.get(`${config.backendUrl}/api/address/${id}`);
          const formattedAddress = `${response.data.houseNumber} ${response.data.streetName}, ${response.data.ward}, ${response.data.district}, ${response.data.province}, ${response.data.country}`;
          setAddresses((prev) => ({ ...prev, [id]: formattedAddress }));
      } catch (error) {
          console.error("Error fetching address:", error);
      }
  }
  };

  const getIdentifications = async (id) => {
    if (!identifications[id]) { // Avoid duplicate API calls for the same ID
      try {
          const response = await axios.get(`${config.backendUrl}/api/identification/${id}`);
          setIdentifications((prev) => ({ ...prev, [id]: response.data }));
      } catch (error) {
          console.error("Error fetching address:", error);
      }
  }
  };

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

  useEffect(() => {
    const groupFieldIds = dataSet
        .flatMap((row) =>
            fields
                .filter((field) => field.type === "group" && row[`${field.accessor}Id`] && field.customeType !== "identification")
                .map((field) => row[`${field.accessor}Id`])
        )
        .filter((id, index, self) => id && self.indexOf(id) === index); // Remove duplicates
    groupFieldIds.forEach((id) => getViewAddress(id));

    const groupFieldIds2 = dataSet
        .flatMap((row) =>
            fields
                .filter((field) => field.type === "group" && row[`${field.accessor}Id`] && field.customeType === "identification")
                .map((field) => row[`${field.accessor}Id`])
        )
        .filter((id, index, self) => id && self.indexOf(id) === index); // Remove duplicates
    groupFieldIds2.forEach((id) => getIdentifications(id));
  }, [dataSet, fields]);

  return (
    (dataSet.length > 0) ? (
    <div className="py-4">
      <table className="table-auto whitespace-nowrap overflow-x-scroll">
        <thead>
          <tr>
            <th>Hành động</th>
            {fields
              .filter((field) => !field.hidden)
              .map((field) => (
              <th key={field.display} className="p-2  max-w-xs">{field.display}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {dataSet.map((row, rowIndex) => (
            <tr key={rowIndex}>
              <td>
                <button className="btn btn-primary mr-2" onClick={() => { handleEdit(row); }}>
                  <Pencil size={16} />
                </button>
                <button className="btn btn-danger" onClick={() => { handleDelete(row); }}>
                  <Trash2 size={16} />
                </button>
              </td>
              {fields
              .filter((field) => !field.hidden)
              .map((field) => {
                switch (field.type) {
                  case 'select':
                    if (field.customeType === "identificationType") {
                      return <td key={field.accessor} className="p-2">{identifications[row["identificationId"]]?.["identificationType"]}</td>
                    }
                    return <td key={field.accessor} className="p-2">{getNameById(row[field.accessor], field.options)}</td>
                  case 'date':
                    return <td key={field.accessor} className="p-2">{new Date(row[field.accessor]).toLocaleDateString()}</td>
                  case 'group':
                    if (field.customeType === "identification") {
                      return (
                          <td key={field.accessor} className="p-2 text-center">
                              <button
                                  className="btn btn-primary"
                                  onClick={() => {
                                      const id = row["identificationId"];
                                      if (id && !identifications[id]) {
                                          getIdentifications(id).then(() => {
                                              setSelectedIdentification(identifications[id]);
                                              setIsPopupVisible(true);
                                          });
                                      } else {
                                          setSelectedIdentification(identifications[id]);
                                          setIsPopupVisible(true);
                                      }
                                  }}
                              >
                                  Xem
                              </button>
                          </td>
                      );
                    }
                    return row[`${field.accessor}Id`] ? (
                        <td key={field.accessor} className="p-2 max-w-3xs truncate">
                            {addresses[row[`${field.accessor}Id`]] || "Loading..."}
                        </td>
                    ) : (
                        <td key={field.accessor} className="p-2">Chưa có</td>
                    );
                  default:
                    return <td key={field.accessor} className="p-2">{row[field.accessor]}</td>
                }
              })}
            </tr>
          ))}
        </tbody>
      </table>
  
      {isPopupVisible && (
            <IdentificationPopup
                identification={selectedIdentification}
                onClose={() => setIsPopupVisible(false)}
            />
        )}
    </div>
    ) : (
      <div className="flex justify-center mt-4">Không tìm thấy dữ liệu</div>
    )
  );
};

export default DataTable;
