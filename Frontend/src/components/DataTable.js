import React, { useState, useEffect } from "react";
import axios from 'axios';
import config from '../config';
// import readXlsxFile from "read-excel-file";
// import { Button } from "@/components/ui/button";

const DataTable = ({ fields, dataSet, handleEdit, handleDelete }) => {
  // const handleFileUpload = async (event) => {
  //   const file = event.target.files[0];
  //   if (file) {
  //     readXlsxFile(file).then((rows) => {
  //       const headers = rows[0];
  //       const formattedData = rows.slice(1).map((row) => {
  //         return headers.reduce((acc, header, index) => {
  //           acc[header] = row[index] || "";
  //           return acc;
  //         }, {});
  //       });
  //       setData(formattedData);
  //       if (onFileUpload) {
  //         onFileUpload(formattedData);
  //       }
  //     });
  //   }
  // };
  const [addresses, setAddresses] = useState({});
  // Tìm tên theo ID từ danh sách
  const getNameById = (id, list) => {
    const item = list.find((item) => item.id === id);
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

  useEffect(() => {
    const groupFieldIds = dataSet
        .flatMap((row) =>
            fields
                .filter((field) => field.type === "group" && row[`${field.accessor}Id`])
                .map((field) => row[`${field.accessor}Id`])
        )
        .filter((id, index, self) => id && self.indexOf(id) === index); // Remove duplicates

    groupFieldIds.forEach((id) => getViewAddress(id));
  }, [dataSet, fields]);

  return (
    <div className="py-4">
      <table className="table-auto w-full whitespace-nowrap">
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
                <button className="btn btn-primary me-2" onClick={() => { handleEdit(row); }}>
                  Sửa
                </button>
                <button className="btn btn-danger" onClick={() => { handleDelete(row); }}>
                  Xóa
                </button>
              </td>
              {fields
              .filter((field) => !field.hidden)
              .map((field) => {
                switch (field.type) {
                  case 'select':
                    return <td key={field.accessor} className="p-2">{getNameById(row[field.accessor], field.options)}</td>
                  case 'date':
                    return <td key={field.accessor} className="p-2">{new Date(row[field.accessor]).toLocaleDateString()}</td>
                  case 'group':
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
    </div>
  );
};

export default DataTable;
