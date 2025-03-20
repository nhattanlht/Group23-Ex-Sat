import React, { useState, useEffect } from 'react';
import Pagination from './Pagination';
import DataTable from './DataTable';
import { handleAddRow, handleEditRow, handleDeleteRow, loadDataNoPaging } from '../util/callCRUDApi';
import DataForm from './DataForm';
const DataList = ({fields, dataName, pk, label}) => {
  const [dataSet, setDataSet] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    loadListData();
  }, []);

  // Gọi API lấy danh sách
  const loadListData = async () => {
    try {
      const data = await loadDataNoPaging(dataName);
      setDataSet(data || []);
      console.log("search",data);
    } catch (error) {
      console.error(`Lỗi khi tải danh sách ${label}`, error);
      alert(`Lỗi khi tải danh sách ${label}`);
    }
  };

  const handleAddData = async (data) => {
    try {
      await handleAddRow(dataName, data);
      setShowModal(false);
      loadListData();
    } catch (error) {
      alert(`Lỗi khi thêm ${label}`);
    }
  };

  const handleEditData = async (data) => {
    try {
      await handleEditRow(dataName, data[pk], data);
      setShowModal(false);
      loadListData();
    } catch (error) {
      alert(`Lỗi khi chỉnh sửa ${label}`);
    }
  };

  const handleDeleteData = async (pk) => {
    try {
      await handleDeleteRow(dataName, pk);
      loadListData();
    } catch (error) {
      alert(`Lỗi khi xóa ${label}`);
    }
  };

  return (
    <div>
      <h2>Danh sách {label}</h2>
      <div className="d-flex mb-3">
        <button className="btn btn-success me-2" onClick={() => { setModalData(null); setShowModal(true); }}>
          Thêm {label}
        </button>
        {/* <input
          type="text"
          className="form-control"
          placeholder={`Tìm kiếm ${label}`}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        /> */}
      </div>
      <DataTable fields={fields} dataSet={dataSet} handleEdit={(data) => {setModalData(data); setShowModal(true);}} handleDelete={(data)=>{handleDeleteData(data[pk])}}></DataTable>
      {/* <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} /> */}
      {showModal && <DataForm fields={fields} data={modalData} onSave={modalData ? handleEditData : handleAddData} onClose={() => setShowModal(false)} label={label} />}
    </div>
  );
};

export default DataList;