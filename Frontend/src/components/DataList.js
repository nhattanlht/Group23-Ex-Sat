import React, { useState, useEffect, useMemo } from 'react';
import axios from 'axios';
import config from '../config';
import DataTable from './DataTable';
import { handleAddRow, handleEditRow, handleDeleteRow, loadDataNoPaging } from '../util/callCRUDApi';
import DataForm from './DataForm';
import { formatDataSetForTable } from '../util/formatData';
import { useLanguage } from '../contexts/LanguageContext';
import { Plus, Pencil, Trash2 } from 'lucide-react';

const DataList = ({ formFields, tableFields=formFields, dataName, pk, label, formatDataSet = formatDataSetForTable, actions=[], customRoute = null }) => {
  const { translate } = useLanguage();
  const [dataSet, setDataSet] = useState([]);
  const [options, setOptions] = useState({});
  const [validSelectedOptions, setValidSelectedOptions] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);

  const tableFieldsWithOptions = useMemo(() => {
    return tableFields.filter(field => field.optionsEndpoint || field.options);
  }, [tableFields]);

  const fetchOptions = async () => {
    const newOptions = { ...options };
    const newValidSelectedOptions = { ...validSelectedOptions };

    for (const field of tableFieldsWithOptions) {
      if (field.optionsEndpoint && !options[field.accessor]) {
        try {
          const { data } = await loadDataNoPaging(field.optionsEndpoint);
          console.log(`Fetched options for ${field.accessor}:`, data);
          newOptions[field.accessor] = data;
          newValidSelectedOptions[field.accessor] = data;
          field.options = newOptions[field.accessor];
        } catch (error) {
          console.error(`Error fetching options for ${field.accessor}:`, error);
          newOptions[field.accessor] = [];
          newValidSelectedOptions[field.accessor] = new Set();
        }
      }
    }

    setOptions(newOptions);
    setValidSelectedOptions(newValidSelectedOptions);
  };

  useEffect(() => {
    loadListData();
    fetchOptions();
  }, []);

  const loadListData = async () => {
    try {
      const { data } = await loadDataNoPaging(dataName);
      console.log(`Loaded ${label} data:`, data);
      const formattedData = formatDataSet(data, tableFields, {translate});
      console.log(`Loaded and formatted ${label} data:`, formattedData);
      setDataSet(formattedData || []);
    } catch (error) {
      console.error(`Error loading ${label} list:`, error);
      alert(error.message || translate('common.error'), error);
    }
  };

  const handleAddData = async (data) => {
    try {
      const response = await handleAddRow(dataName, data);
      setShowModal(false);
      loadListData();
      alert(response.message || translate('common.success'));
    } catch (error) {
      alert(error.message || translate("common.error"));
      throw error;
    }
  };

  const handleEditData = async (data) => {
    try {
      const response = await handleEditRow(dataName, data[pk], data, customRoute?.edit);
      setShowModal(false);
      loadListData();
      alert(response.message || translate('common.success'));
    } catch (error) {
      alert(error.message || translate("common.error"));
      throw error;
    }
  };

  const handleDeleteData = async (id) => {
    if (window.confirm(translate('common.confirm_delete'))) {
      try {
        const response = await handleDeleteRow(dataName, id);
        loadListData();
        alert(response.message || translate('common.success'));
      } catch (error) {
        alert(error.message|| translate("common.error"));
      }
    }
  };

  const allOptionsReady = tableFieldsWithOptions
    .filter(field => field.optionsEndpoint && field.type === 'select')
    .every(field => Array.isArray(options?.[field.accessor]));
  
  if (!allOptionsReady) {
    return <div>{translate('common.loading')}</div>; 
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-4">
        <button
          className="btn bg-green-600 hover:bg-green-700 text-white flex items-center gap-2 px-4 py-2 rounded-md"
          onClick={() => { setModalData(null); setShowModal(true); }}
        >
          <Plus size={20} />
          {translate('common.add')} {label}
        </button>
      </div>

      <DataTable
        fields={tableFields}
        dataSet={dataSet}
        handleEdit={(data) => {
          setModalData(data.__original);
          setShowModal(true);
        }}
        handleDelete={(data) => handleDeleteData(data.__original[pk])}
        actions={actions}
      />

      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
          <div className="bg-white p-6 rounded-lg shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
            <DataForm
              fields={formFields.map((field) => ({
                ...field,
                options: field.type === 'select' && field.optionsEndpoint ? validSelectedOptions[field.accessor] : field.options,
              }))}
              data={modalData}
              onSave={(data) => modalData ? handleEditData(data) : handleAddData(data)}
              onClose={() => setShowModal(false)}
              label={label}
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default DataList;