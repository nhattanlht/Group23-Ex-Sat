import React from "react";
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
  // Tìm tên theo ID từ danh sách
  const getNameById = (id, list) => {
    const item = list.find((item) => item.id === id);
    return item ? item.name : 'Chưa có';
  };

  return (
    <div className="py-4">
      <table className="table-auto w-full">
        <thead>
          <tr>
            {fields.map((field) => (
              <th key={field.display} className="p-2">{field.display}</th>
            ))}
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          {dataSet.map((row, rowIndex) => (
            <tr key={rowIndex}>
              {fields.map((field) => {
                switch (field.type) {
                  case 'select':
                    return <td key={field.accessor} className="p-2">{getNameById(row[field.accessor], field.options)}</td>
                  case 'date':
                    return <td key={field.accessor} className="p-2">{new Date(row[field.accessor]).toLocaleDateString()}</td>
                  default:
                    return <td key={field.accessor} className="p-2">{row[field.accessor]}</td>
                }
              })}
              <td>
                <button className="btn btn-primary me-2" onClick={() => { handleEdit(row); }}>
                  Sửa
                </button>
                <button className="btn btn-danger" onClick={() => { handleDelete(row); }}>
                  Xóa
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DataTable;
