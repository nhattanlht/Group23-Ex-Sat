import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../config';
import DataList from '../components/DataList';
const StudentPage = () => {
  const [departments, setDepartments] = useState([]);
  const [statuses, setStatuses] = useState([]);
  const [studyPrograms, setStudyPrograms] = useState([]);
  const [schoolYears, setSchoolYears] = useState([]);
  const fields = [
    { display: 'MSSV', accessor: 'mssv', type: "text", required: true },
    { display: 'Họ Tên', accessor: 'hoTen', type: "text", required: true },
    { display: 'Ngày Sinh', accessor: 'ngaySinh', type: "date", required: true },
    { display: 'Giới Tính', accessor: 'gioiTinh', type: "select", options: [{id:"Nam", name:"Nam"}, {id:"Nữ", name: "Nữ"}, {id:"Khác", name:"Khác"}], required: true },
    { display: 'Khoa', accessor: 'departmentId', type: "select", options: departments, required: true },
    { display: 'Trạng Thái', accessor: 'statusId', type: "select", options: statuses, required: true },
    { display: 'Khóa Học', accessor: 'schoolYearId', type: "select", options: schoolYears, required: true },
    { display: 'Chương Trình', accessor: 'studyProgramId', type: "select", options: studyPrograms, required: true },
    { display: 'Địa Chỉ', accessor: 'diaChi', type: "text", required: true },
    { display: 'Email', accessor: 'email', type: "email", required: true },
    { display: 'Số Điện Thoại', accessor: 'soDienThoai', type: "text", required: true },
  ];

  useEffect(() => {
    loadMetadata();
  }, []);

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

  return (
      <DataList fields={fields} dataName='students' pk='mssv' label='Sinh Viên'/>
  );
};

export default StudentPage;
