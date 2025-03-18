import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../config';
import DataForm from './DataForm';
const StudentModal = ({ fields, data, onSave, onClose }) => {
  const [student, setStudent] = useState({
    mssv: '',
    hoTen: '',
    ngaySinh: '',
    gioiTinh: 'Nam',
    departmentId: '',
    schoolYearId: '',
    statusId: '',
    studyProgramId: '',
    diaChi: '',
    email: '',
    soDienThoai: ''
  });

  const [departments, setDepartments] = useState([]);
  const [statuses, setStatuses] = useState([]);
  const [schoolYears, setSchoolYears] = useState([]);
  const [programs, setPrograms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const genders = ['Nam', 'Nữ', 'Khác'];

  useEffect(() => {
    if (data) {
      setStudent(data);
    } else {
      setStudent({
        mssv: '',
        hoTen: '',
        ngaySinh: '',
        gioiTinh: 'Nam',
        departmentId: '',
        schoolYearId: '',
        statusId: '',
        studyProgramId: '',
        diaChi: '',
        email: '',
        soDienThoai: ''
      });
    }
  }, [data]);

  useEffect(() => {
    loadDropdownData();
  }, []);

  const loadDropdownData = async () => {
    try {
      setLoading(true);
      const [departmentsRes, statusesRes, khoasRes, programsRes] = await Promise.all([
        axios.get(`${config.backendUrl}/api/departments`),
        axios.get(`${config.backendUrl}/api/student-statuses`),
        axios.get(`${config.backendUrl}/api/schoolyears`),
        axios.get(`${config.backendUrl}/api/programs`)
      ]);
      setDepartments(departmentsRes.data);
      setStatuses(statusesRes.data);
      setSchoolYears(khoasRes.data);
      setPrograms(programsRes.data);
    } catch (error) {
      console.error("Error loading dropdown data:", error);
      setError("Lỗi khi tải dữ liệu! Vui lòng thử lại.");
    } finally {
      setLoading(false);
      setError(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    console.log(name, value);
    setStudent((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(student);
    if (!student.mssv || !student.hoTen || !student.ngaySinh || !student.departmentId) {
      console.error("Vui lòng nhập đầy đủ thông tin!");
      return;
    }
    onSave(student);
  };

  return (
    <div className="modal show" style={{ display: 'block' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{data ? 'Sửa Thông Tin Sinh Viên' : 'Thêm Sinh Viên'}</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>
          <div className="modal-body">
            {loading ? (
              <p>Đang tải dữ liệu...</p>
            ) : error ? (
              <div className="alert alert-danger">{error}</div>
            ) : (
              < DataForm fields={fields} data={data} onSave={onSave} onClose={onClose} />
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default StudentModal;
