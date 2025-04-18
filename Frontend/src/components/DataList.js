import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../config';
import DataTable from './DataTable';
import { handleAddRow, handleEditRow, handleDeleteRow, loadDataNoPaging } from '../util/callCRUDApi';
import DataForm from './DataForm';

const DataList = ({ fields, dataName, pk, label }) => {
  const [dataSet, setDataSet] = useState([]);
  const [options, setOptions] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);

  const fetchOptions = async () => {
    const newOptions = {};
    for (const field of fields) {
      if (field.type === 'select' && field.optionsEndpoint) {
        try {
          const response = await axios.get(`${config.backendUrl}/api/${field.optionsEndpoint}`);
          newOptions[field.accessor] = response.data.map((item) => ({
            id: String(item.id || item.courseCode),
            name: item.name || item.courseCode,
          }));
        } catch (error) {
          console.error(`Error fetching options for ${field.accessor}:`, error);
          newOptions[field.accessor] = []; // Ensure options are initialized to an empty array
        }
      }
    }
    setOptions(newOptions);
  };

  useEffect(() => {
    fetchOptions();
    loadListData();
  }, []);

  const loadListData = async () => {
    try {
      const data = await loadDataNoPaging(dataName);
      setDataSet(data || []);
    } catch (error) {
      console.error(`Error loading ${label} list:`, error);
      alert(`Error loading ${label} list`);
    }
  };

  const handleAddData = async (data) => {
    try {
      await handleAddRow(dataName, data);
      setShowModal(false);
      loadListData();
    } catch (error) {
      alert(`Error adding ${label}`);
    }
  };

  const handleEditData = async (data) => {
    try {
      await handleEditRow(dataName, data[pk], data);
      setShowModal(false);
      loadListData();
    } catch (error) {
      alert(`Error editing ${label}`);
    }
  };

  const handleDeleteData = async (pk) => {
    try {
      await handleDeleteRow(dataName, pk);
      loadListData();
    } catch (error) {
      alert(`Error deleting ${label}`);
    }
  };

  return (
    <div>
      <div className="flex mb-3">
        <button className="btn btn-success me-2" onClick={() => { setModalData(null); setShowModal(true); }}>
          Add {label}
        </button>
      </div>
      <DataTable
        fields={fields.map((field) => ({
          ...field,
          options: field.type === 'select' && field.optionsEndpoint ? options[field.accessor] || [] : field.options,
        }))}
        dataSet={dataSet}
        handleEdit={(data) => { setModalData(data); setShowModal(true); }}
        handleDelete={(data) => { handleDeleteData(data[pk]); }}
      />
      {showModal && (
        <DataForm
          fields={fields.map((field) => ({
            ...field,
            options: field.type === 'select' && field.optionsEndpoint ? options[field.accessor] || [] : field.options,
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