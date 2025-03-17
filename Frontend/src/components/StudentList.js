import React, { useState, useEffect } from 'react';
import axios from 'axios';
import StudentModal from './StudentModal';
import Pagination from './Pagination';
import config from '../config';
import DataTable from './DataTable';

const StudentList = () => {
  const [students, setStudents] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [statuses, setStatuses] = useState([]);
  const [studyPrograms, setStudyPrograms] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  const fields = [
    { display: 'MSSV', accessor: 'mssv' },
    { display: 'Họ Tên', accessor: 'hoTen' },
    { display: 'Ngày Sinh', accessor: 'ngaySinh' },
    { display: 'Giới Tính', accessor: 'gioiTinh' },
    { display: 'Khoa', accessor: 'departmentId' },
    { display: 'Trạng Thái', accessor: 'statusId' },
    { display: 'Khóa Học', accessor: 'schoolYearId' },
    { display: 'Chương Trình', accessor: 'studyProgramId' },
    { display: 'Địa Chỉ', accessor: 'diaChi' },
    { display: 'Email', accessor: 'email' },
    { display: 'Số Điện Thoại', accessor: 'soDienThoai' },
  ];

  useEffect(() => {
    loadStudents(currentPage, searchTerm);
    loadMetadata();
  }, [currentPage, searchTerm]);

  // Gọi API lấy danh sách sinh viên
  const loadStudents = async (page, keyword = '') => {
    try {
      const url = keyword
        ? `${config.backendUrl}/api/students/search?keyword=${keyword}&page=${page}&pageSize=10`
        : `${config.backendUrl}/api/students?page=${page}&pageSize=10`;

      const response = await axios.get(url);
      setStudents(response.data.students || []);
      setCurrentPage(response.data.currentPage || 1);
      setTotalPages(response.data.totalPages || 1);
      console.log("search",response.data.students);
    } catch (error) {
      console.error("Lỗi khi tải danh sách sinh viên:", error);
      alert('Lỗi khi tải danh sách sinh viên!');
    }
  };

  // Gọi API lấy danh sách khoa, trạng thái, chương trình học
  const loadMetadata = async () => {
    try {
      const [depRes, statusRes, programRes] = await Promise.all([
        axios.get(`${config.backendUrl}/api/departments`),
        axios.get(`${config.backendUrl}/api/student-statuses`),
        axios.get(`${config.backendUrl}/api/programs`),
      ]);
      console.log(depRes.data, statusRes.data, programRes.data);
      setDepartments(depRes.data || []);
      setStatuses(statusRes.data || []);
      setStudyPrograms(programRes.data || []);
    } catch (error) {
      console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
    }
  };

  // Tìm tên theo ID từ danh sách
  const getNameById = (id, list) => {
    const item = list.find((item) => item.id === id);
    return item ? item.name : 'Chưa có';
  };

  const handleAddStudent = async (student) => {
    try {
      const response = await axios.post(`${config.backendUrl}/api/students`, student);
      alert(response.data.message);
      setShowModal(false);
      loadStudents(currentPage, searchTerm);
    } catch (error) {
      alert(error.response?.data?.message || 'Lỗi không xác định');
    }
  };

  const handleEditStudent = async (student) => {
    try {
      await axios.put(`${config.backendUrl}/api/students/${student.mssv}`, student);
      setShowModal(false);
      loadStudents(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi chỉnh sửa sinh viên!');
    }
  };

  const handleDeleteStudent = async (mssv) => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa sinh viên này không?')) return;
    try {
      await axios.delete(`${config.backendUrl}/api/students/${mssv}`);
      loadStudents(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi xóa sinh viên!');
    }
  };

  return (
    <div>
      <h2>Danh sách sinh viên</h2>
      <div className="d-flex mb-3">
        <button className="btn btn-success me-2" onClick={() => { setModalData(null); setShowModal(true); }}>
          Thêm Sinh Viên
        </button>
        <input
          type="text"
          className="form-control"
          placeholder="Tìm kiếm sinh viên..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>
      <DataTable fields={fields} dataSet={students} handleEdit={(student) => {setModalData(student); setShowModal(true);}} handleDelete={(student)=>{handleDeleteStudent(student.mssv)}}></DataTable>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {showModal && <StudentModal data={modalData} onSave={modalData ? handleEditStudent : handleAddStudent} onClose={() => setShowModal(false)} />}
    </div>
  );
};

export default StudentList;
