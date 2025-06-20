import { Pencil, Trash2 } from "lucide-react";
import { useLanguage } from '../contexts/LanguageContext';

const DataTable = ({ fields, dataSet, handleEdit, handleDelete, actions = [] }) => {
  const { translate } = useLanguage();

  return (
    (dataSet.length > 0) ? (
      <div className="py-4 overflow-x-auto">
        <table className="min-w-full table-auto">
          <thead>
            <tr>
              {fields
                .filter((field) => !field.hidden)
                .map((field) => (
                  <th key={field.display} className="px-4 py-2">{field.display}</th>
                ))}
              <th className="px-4 py-2">{translate('student.fields.actions')}</th>
            </tr>
          </thead>
          <tbody>
            {dataSet.map((row, rowIndex) => (
              <tr key={rowIndex}>
                {fields.map((field) => {
                  if (field.hidden) return null;
                  return (
                    <td key={field.accessor} className="px-4 py-2 break-words">
                      {row[field.accessor]}
                    </td>
                  );
                })}
                <td className="px-4 py-2">
                  <div className="flex flex-col space-y-2">
                    <button 
                      className="btn btn-primary h-10 px-4 flex items-center justify-center w-full" 
                      onClick={() => { handleEdit(row); }}
                      title={translate('student.tooltips.edit')}
                    >
                      <Pencil size={20} className="mr-2" />
                      {translate('student.actions.edit')}
                    </button>
                    <button 
                      className="btn btn-danger h-10 px-4 flex items-center justify-center w-full" 
                      onClick={() => { handleDelete(row); }}
                      title={translate('student.tooltips.delete')}
                    >
                      <Trash2 size={20} className="mr-2" />
                      {translate('student.actions.delete')}
                    </button>

                    {actions.map((action, index) => {
                      if (action.condition && !action.condition(row)) return null;
                      return (
                        <button
                          key={index}
                          className={`${action.className || 'btn btn-secondary'} h-10 px-4 flex items-center justify-center w-full`}
                          onClick={() => action.onClick(row)}
                          title={translate(action.tooltip || action.label)}
                        >
                          {action.icon && <span className="mr-2">{action.icon}</span>}
                          {action.label}
                        </button>
                      );
                    })}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    ) : (
      <div className="flex justify-center mt-4">{translate('student.messages.no_data')}</div>
    )
  );
};

export default DataTable;
