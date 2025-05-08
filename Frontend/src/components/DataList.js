import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../config';
import DataTable from './DataTable';
import { handleAddRow, handleEditRow, handleDeleteRow, loadDataNoPaging } from '../util/callCRUDApi';
import DataForm from './DataForm';
import { formatDataSetForTable } from '../util/formatData';

const DataList = ({ formFields, tableFields=formFields, dataName, pk, label, formatDataSet = formatDataSetForTable, actions=[] }) => {
  const [dataSet, setDataSet] = useState([]);
  const [options, setOptions] = useState({});
  const [validSelectedOptions, setValidSelectedOptions] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);

  const fetchOptions = async () => {
    const newOptions = {};
    for (const field of tableFields) {
      if (field.type === 'select' && field.optionsEndpoint) {
        try {
          const response = await axios.get(`${config.backendUrl}/api/${field.optionsEndpoint}`);
          newOptions[field.accessor] = response.data.map((item) => ({
            id: String(item.id),
            name: item.name,
          }));
          field.options = newOptions[field.accessor]; // Update field options directly
        } catch (error) {
          console.error(`Error fetching options for ${field.accessor}:`, error);
          newOptions[field.accessor] = []; // Ensure options are initialized to an empty array
        }
      }
    }
    setOptions(newOptions);
    const newValidSelectedOptions = {};
    for (const field of formFields) {
      if (field.type === 'select' && field.optionsEndpoint) {
        try {
            const response = await axios.get(`${config.backendUrl}/api/${field.optionsEndpoint}`);
            newValidSelectedOptions[field.accessor] = response.data.map((item) => ({
              id: String(item.id || item.courseCode),
              name: item.name || item.courseCode,
            }));
            field.options = newValidSelectedOptions[field.accessor]; // Update field options directly
        } catch (error) {
          console.error(`Error fetching options for ${field.accessor}:`, error);
          newValidSelectedOptions[field.accessor] = []; // Ensure options are initialized to an empty array
        }
      }
    }
    setValidSelectedOptions(newValidSelectedOptions);
  };

  useEffect(() => {
    fetchOptions();
    loadListData();
  }, []);

  const loadListData = async () => {
    try {
      const data = await loadDataNoPaging(dataName);
      const formattedData = formatDataSet(data, tableFields);
      setDataSet(formattedData || []);
    } catch (error) {
      console.error(`Error loading ${label} list:`, error);
      alert(`Error loading ${label} list: ${error.response.data || ''}`);
    }
  };

  const handleAddData = async (data) => {
    try {
      await handleAddRow(dataName, data);
      setShowModal(false);
      loadListData();
    } catch (error) {
      alert(`Error adding ${label}: ${error.response.data || ''}`);
    }
  };

  const handleEditData = async (data) => {
    try {
      await handleEditRow(dataName, data[pk], data);
      setShowModal(false);
      loadListData();
    } catch (error) {
      alert(`Error editing ${label}: ${error.response.data || ''}`);
    }
  };

  const handleDeleteData = async (pk) => {
    try {
      await handleDeleteRow(dataName, pk);
      loadListData();
    } catch (error) {
      alert(`Error deleting ${label}: ${error.response.data || ''}`);
    }
  };

  return (
    <div>
      <div className="flex mb-3">
        <button className="btn btn-success me-2" onClick={() => { setModalData(null); setShowModal(true); }}>
          Thêm {label}
        </button>
      </div>
      <DataTable
        fields={tableFields.map((field) => ({
          ...field,
          options: field.type === 'select' && field.optionsEndpoint ? options[field.accessor] : field.options,
        }))}
        dataSet={dataSet}
        handleEdit={(data) => { setModalData(data.__original); setShowModal(true); }}
        handleDelete={(data) => { handleDeleteData(data[pk]); }}
        actions={actions}
      />
      {showModal && (
        <DataForm
          fields={formFields.map((field) => ({
            ...field,
            options: field.type === 'select' && field.optionsEndpoint ? validSelectedOptions[field.accessor] : field.options,
          }))}
          data={modalData}
          onSave={modalData ? handleEditData : handleAddData}
          onClose={() => setShowModal(false)}
          label={label}
        />
      )}
    </div>
  );
};

export default DataList;