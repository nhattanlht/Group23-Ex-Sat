import React, { useState, useEffect } from 'react';
import axios from 'axios';
import StudentModal from './StudentModal';
import Pagination from './Pagination';
import config from '../config';

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
        axios.get(`${config.backendUrl}/api/statuses`),
        axios.get(`${config.backendUrl}/api/study-programs`),
      ]);
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
      <table className="table">
        <thead>
          <tr>
            <th>MSSV</th>
            <th>Họ Tên</th>
            <th>Ngày Sinh</th>
            <th>Giới Tính</th>
            <th>Khoa</th>
            <th>Trạng Thái</th>
            <th>Khóa Học</th>
            <th>Chương Trình</th>
            <th>Địa Chỉ</th>
            <th>Email</th>
            <th>Số Điện Thoại</th>
            <th>Hành Động</th>
          </tr>
        </thead>
        <tbody>
          {students.length > 0 ? (
            students.map((student) => (
              <tr key={student.mssv}>
                <td>{student.mssv}</td>
                <td>{student.hoTen}</td>
                <td>{new Date(student.ngaySinh).toLocaleDateString('vi-VN')}</td>
                <td>{student.gioiTinh}</td>
                <td>{getNameById(student.khoaId, departments)}</td>
                <td>{getNameById(student.status, statuses)}</td>
                <td>{student.khoaId ? student.khoaId : 'Chưa có'}</td>
                <td>{getNameById(student.studyProgram, studyPrograms)}</td>
                <td>{student.diaChi}</td>
                <td>{student.email}</td>
                <td>{student.soDienThoai}</td>
                <td>
                  <button className="btn btn-primary me-2" onClick={() => { setModalData(student); setShowModal(true); }}>
                    Sửa
                  </button>
                  <button className="btn btn-danger" onClick={() => handleDeleteStudent(student.mssv)}>
                    Xóa
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="12" className="text-center">Không có sinh viên nào</td>
            </tr>
          )}
        </tbody>
      </table>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {showModal && <StudentModal data={modalData} onSave={modalData ? handleEditStudent : handleAddStudent} onClose={() => setShowModal(false)} />}
    </div>
  );
};

export default StudentList;
