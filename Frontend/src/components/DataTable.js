import React from "react";
import { Pencil, Trash2 } from "lucide-react";

const DataTable = ({ fields, dataSet, handleEdit, handleDelete, actions = [] }) => {
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
                <td className="flex space-x-2">
                  <button className="btn btn-primary" onClick={() => { handleEdit(row); }}>
                    <Pencil size={16} />
                  </button>
                  <button className="btn btn-danger" onClick={() => { handleDelete(row); }}>
                    <Trash2 size={16} />
                  </button>

                  {actions.map((action, index) => {
                    if (action.condition && !action.condition(row)) return null;
                    return (
                      <button
                        key={index}
                        className={action.className || 'btn btn-secondary'}
                        onClick={() => action.onClick(row)}
                        title={action.label}
                      >
                        {action.icon || action.label}
                      </button>
                    );
                  })}
                </td>
                {fields.map((field) => {
                  if (field.hidden) return null;
                  return <td key={field.accessor} className="p-2 max-w-3xs truncate">{row[field.accessor]}</td>
                })}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    ) : (
      <div className="flex justify-center mt-4">Không tìm thấy dữ liệu</div>
    )
  );
};

export default DataTable;
