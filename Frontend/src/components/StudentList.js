import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Pagination from './Pagination';
import config from '../config';
import DataTable from './DataTable';
import { loadData, handleAddRow, handleEditRow, handleDeleteRow } from '../util/callCRUDApi';
import DataForm from './DataForm';
const StudentList = () => {
  const [students, setStudents] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [statuses, setStatuses] = useState([]);
  const [studyPrograms, setStudyPrograms] = useState([]);
  const [schoolYears, setSchoolYears] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showModal, setShowModal] = useState(false);
  const [modalData, setModalData] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  const fields = [
    { display: 'MSSV', accessor: 'mssv', type: "text", required: true },
    { display: 'Họ Tên', accessor: 'hoTen', type: "text", required: true },
    { display: 'Ngày Sinh', accessor: 'ngaySinh', type: "date", required: true },
    { display: 'Giới Tính', accessor: 'gioiTinh', type: "select", options: [{id:"Nam", name:"Nam"}, {id:"Nữ", name: "Nữ"}, {id:"Khác", name:"Khác"}], required: true },
    { display: 'Khoa', accessor: 'departmentId', type: "select", options: departments, required: true },
    { display: 'Trạng Thái', accessor: 'statusId', type: "select", options: statuses, required: true },
    { display: 'Khóa Học', accessor: 'schoolYearId', type: "select", options: schoolYears, required: true },
    { display: 'Chương Trình', accessor: 'studyProgramId', type: "select", options: studyPrograms, required: true },
    { display: 'Email', accessor: 'email', type: "email", required: true },
    { display: 'Số Điện Thoại', accessor: 'soDienThoai', type: "text", required: true },
    { display: 'Quốc Tịch', accessor: 'quocTich', type: "text", required: true },
    {
      display: 'Địa Chỉ Nhận Thư',
      accessor: 'diaChiNhanThu',
      type: "group",
      fields: [
        { display: 'Số Nhà', accessor: 'diaChiNhanThu.houseNumber', type: "text", required: true },
        { display: 'Tên Đường', accessor: 'diaChiNhanThu.streetName', type: "text", required: true },
        { display: 'Phường/Xã', accessor: 'diaChiNhanThu.ward', type: "text", required: true },
        { display: 'Quận/Huyện', accessor: 'diaChiNhanThu.district', type: "text", required: true },
        { display: 'Tỉnh/Thành Phố', accessor: 'diaChiNhanThu.province', type: "text", required: true },
        { display: 'Quốc Gia', accessor: 'diaChiNhanThu.country', type: "text", required: true },
      ],
      required: true,
    },
    { display: 'Địa Chỉ Nhận Thư Id', accessor: 'diaChiNhanThuId', type: "text", required: true, hidden: true },
    {
      display: 'Địa Chỉ Thường Trú',
      accessor: 'diaChiThuongTru',
      type: "group",
      fields: [
        { display: 'Số Nhà', accessor: 'diaChiThuongTru.houseNumber', type: "text" },
        { display: 'Tên Đường', accessor: 'diaChiThuongTru.streetName', type: "text" },
        { display: 'Phường/Xã', accessor: 'diaChiThuongTru.ward', type: "text" },
        { display: 'Quận/Huyện', accessor: 'diaChiThuongTru.district', type: "text" },
        { display: 'Tỉnh/Thành Phố', accessor: 'diaChiThuongTru.province', type: "text" },
        { display: 'Quốc Gia', accessor: 'diaChiThuongTru.country', type: "text" },
      ],
    },
    { display: 'Địa Chỉ Thường Trú Id', accessor: 'diaChiThuongTruId', type: "text", hidden: true },
    {
      display: 'Địa Chỉ Tạm Trú',
      accessor: 'diaChiTamTru',
      type: "group",
      fields: [
        { display: 'Số Nhà', accessor: 'diaChiTamTru.houseNumber', type: "text" },
        { display: 'Tên Đường', accessor: 'diaChiTamTru.streetName', type: "text" },
        { display: 'Phường/Xã', accessor: 'diaChiTamTru.ward', type: "text" },
        { display: 'Quận/Huyện', accessor: 'diaChiTamTru.district', type: "text" },
        { display: 'Tỉnh/Thành Phố', accessor: 'diaChiTamTru.province', type: "text" },
        { display: 'Quốc Gia', accessor: 'diaChiTamTru.country', type: "text" },
      ],
    },
    { display: 'Địa Chỉ Tạm Trú Id', accessor: 'diaChiTamTruId', type: "text", hidden: true },
  ];

  useEffect(() => {
    loadStudents(currentPage, searchTerm);
    loadMetadata();
  }, [currentPage, searchTerm]);

  // Gọi API lấy danh sách sinh viên
  const loadStudents = async (page, keyword = '') => {
    try {
      const data = await loadData('students', page, keyword);
      setStudents(data.students || []);
      setCurrentPage(data.currentPage || 1);
      setTotalPages(data.totalPages || 1);
      console.log("search",data.students);
    } catch (error) {
      console.error("Lỗi khi tải danh sách sinh viên:", error);
      alert('Lỗi khi tải danh sách sinh viên!');
    }
  };

  // Gọi API lấy danh sách khoa, trạng thái, chương trình học
  const loadMetadata = async () => {
    try {
      const [depRes, statusRes, programRes, yearRes] = await Promise.all([
        axios.get(`${config.backendUrl}/api/departments`),
        axios.get(`${config.backendUrl}/api/student-statuses`),
        axios.get(`${config.backendUrl}/api/programs`),
        axios.get(`${config.backendUrl}/api/schoolyears`),
      ]);
      console.log(depRes.data, statusRes.data, programRes.data);
      setDepartments(depRes.data || []);
      setStatuses(statusRes.data || []);
      setStudyPrograms(programRes.data || []);
      setSchoolYears(yearRes.data || []);
    } catch (error) {
      console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
    }
  };

  const transformToNestedObject = (flatObject) => {
    const nestedObject = {};

    Object.keys(flatObject).forEach((key) => {
        const keys = key.split('.'); // Split the key by dots
        let current = nestedObject;

        keys.forEach((subKey, index) => {
            if (index === keys.length - 1) {
                // If it's the last key, assign the value
                current[subKey] = flatObject[key];
            } else {
                // Otherwise, create an object if it doesn't exist
                current[subKey] = current[subKey] || {};
                current = current[subKey];
            }
        });
    });

    return nestedObject;
  };

  const handleAddStudent = async (student) => {
    try {
      const newStudent1 = transformToNestedObject(student.diaChiNhanThu);

      const diaChiNhanThu = await axios.post(`${config.backendUrl}/api/address`, newStudent1.diaChiNhanThu);
      student.diaChiNhanThuId = diaChiNhanThu.data.id;

      delete student.diaChiNhanThu;

      if (student.diaChiThuongTru) {
          const newStudent2 = transformToNestedObject(student.diaChiThuongTru);
          const diaChiThuongTru = await axios.post(`${config.backendUrl}/api/address`, newStudent2.diaChiThuongTru);
          student.diaChiThuongTruId = diaChiThuongTru.data.id;
      }
      else {
        student.diaChiThuongTruId = null;
      }

      delete student.diaChiThuongTru;

      if (student.diaChiTamTru) {
          const newStudent3 = transformToNestedObject(student.diaChiTamTru);
          const diaChiTamTru = await axios.post(`${config.backendUrl}/api/address`, newStudent3.diaChiTamTru);
          student.diaChiTamTruId = diaChiTamTru.data.id;
      }
      else {
        student.diaChiTamTruId = null;
      }

      delete student.diaChiTamTru;

      console.log(student);

      await handleAddRow('students', student);
      setShowModal(false);
      loadStudents(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi thêm sinh viên!');
    }
  };

  const handleEditStudent = async (student) => {
    try {
      // Handle diaChiNhanThu
      const newStudent1 = transformToNestedObject(student.diaChiNhanThu);
      const diaChiNhanThu = await axios.post(`${config.backendUrl}/api/address`, newStudent1.diaChiNhanThu);
      student.diaChiNhanThuId = diaChiNhanThu.data.id;

      delete student.diaChiNhanThu;

      // Handle diaChiThuongTru
      if (student.diaChiThuongTru) {
          const newStudent2 = transformToNestedObject(student.diaChiThuongTru);
          const diaChiThuongTru = await axios.post(`${config.backendUrl}/api/address`, newStudent2.diaChiThuongTru);
          student.diaChiThuongTruId = diaChiThuongTru.data.id;
      }
      else {
        student.diaChiThuongTruId = null;
      }
      delete student.diaChiThuongTru;

      // Handle diaChiTamTru
      if (student.diaChiTamTru) {
          const newStudent3 = transformToNestedObject(student.diaChiTamTru);
          const diaChiTamTru = await axios.post(`${config.backendUrl}/api/address`, newStudent3.diaChiTamTru);
          student.diaChiTamTruId = diaChiTamTru.data.id;
      }
      else {
        student.diaChiTamTruId = null;
      }
      delete student.diaChiTamTru;

      console.log(student);

      await handleEditRow('students', student.mssv, student);
      setShowModal(false);
      loadStudents(currentPage, searchTerm);
    } catch (error) {
      alert('Lỗi khi chỉnh sửa sinh viên!');
    }
  };

  const handleDeleteStudent = async (mssv) => {
    try {
      await handleDeleteRow('students', mssv);
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
      {showModal && <DataForm fields={fields} data={modalData} onSave={modalData ? handleEditStudent : handleAddStudent} onClose={() => setShowModal(false)} />}
    </div>
  );
};

export default StudentList;
