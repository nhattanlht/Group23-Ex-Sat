import React, { useState, useEffect } from 'react';
import Pagination from './Pagination';
import DataTable from './DataTable';
import { loadData, handleAddRow, handleEditRow, handleDeleteRow } from '../util/callCRUDApi';
import DataForm from './DataForm';
const DataList = ({fields, dataName, pk, label}) => {
  const [dataSet, setDataSet] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    loadListData(currentPage, searchTerm);
  }, [currentPage, searchTerm]);

  // Gọi API lấy danh sách
  const loadListData = async (page, keyword = '') => {
    try {
      const {data, currentPage, totalPages} = await loadData(dataName, page, keyword);
      setDataSet(data || []);
      setCurrentPage(currentPage || 1);
      setTotalPages(totalPages || 1);
      console.log("search",data);
    } catch (error) {
      console.error("Lỗi khi tải danh sách sinh viên:", error);
      alert('Lỗi khi tải danh sách sinh viên!');
    }
  };

  const handleAddData = async (data) => {
    try {
      await handleAddRow(dataName, data);
      setShowModal(false);
      loadListData(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi thêm sinh viên!');
    }
  };

  const handleEditData = async (data) => {
    try {
      await handleEditRow(dataName, data[pk], data);
      setShowModal(false);
      loadListData(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi chỉnh sửa sinh viên!');
    }
  };

  const handleDeleteData = async (pk) => {
    try {
      await handleDeleteRow(dataName, pk);
      loadListData(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi xóa sinh viên!');
    }
  };

  return (
    <div>
      <h2>Danh sách {label}</h2>
      <div className="d-flex mb-3">
        <button className="btn btn-success me-2" onClick={() => { setModalData(null); setShowModal(true); }}>
          Thêm {label}
        </button>
        <input
          type="text"
          className="form-control"
          placeholder={`Tìm kiếm ${label}`}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>
      <DataTable fields={fields} dataSet={dataSet} handleEdit={(data) => {setModalData(data); setShowModal(true);}} handleDelete={(data)=>{handleDeleteData(data[pk])}}></DataTable>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {showModal && <DataForm fields={fields} data={modalData} onSave={modalData ? handleEditData : handleAddData} onClose={() => setShowModal(false)} />}
    </div>
  );
};

export default DataList;
