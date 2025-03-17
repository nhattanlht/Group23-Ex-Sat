import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../config';

const StudentModal = ({ data, onSave, onClose }) => {
  const [student, setStudent] = useState({
    mssv: '',
    hoTen: '',
    ngaySinh: '',
    gioiTinh: 'Nam',
    departmentId: '',
    statusId: '',
    khoaId: '',
    programId: '',
    diaChi: '',
    email: '',
    soDienThoai: ''
  });

  const [departments, setDepartments] = useState([]);
  const [statuses, setStatuses] = useState([]);
  const [khoas, setKhoas] = useState([]);
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
        statusId: '',
        khoaId: '',
        programId: '',
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
      setKhoas(khoasRes.data);
      setPrograms(programsRes.data);
    } catch (error) {
      console.error("Error loading dropdown data:", error);
      setError("Lỗi khi tải dữ liệu! Vui lòng thử lại.");
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setStudent((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!student.mssv || !student.hoTen || !student.ngaySinh || !student.departmentId) {
      setError("Vui lòng nhập đầy đủ thông tin!");
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
              <form onSubmit={handleSubmit}>
                <div className="form-group">
                  <label htmlFor="mssv">MSSV</label>
                  <input type="text" className="form-control" id="mssv" name="mssv" value={student.mssv} onChange={handleChange} required disabled={!!data} />
                </div>
                <div className="form-group">
                  <label htmlFor="hoTen">Họ Tên</label>
                  <input type="text" className="form-control" id="hoTen" name="hoTen" value={student.hoTen} onChange={handleChange} required />
                </div>
                <div className="form-group">
                  <label htmlFor="ngaySinh">Ngày Sinh</label>
                  <input type="date" className="form-control" id="ngaySinh" name="ngaySinh" value={student.ngaySinh} onChange={handleChange} required />
                </div>
                <div className="form-group">
                  <label htmlFor="gioiTinh">Giới Tính</label>
                  <select className="form-control" id="gioiTinh" name="gioiTinh" value={student.gioiTinh} onChange={handleChange} required>
                    {genders.map(gender => <option key={gender} value={gender}>{gender}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="departmentId">Khoa</label>
                  <select className="form-control" id="departmentId" name="departmentId" value={student.departmentId} onChange={handleChange} required>
                    {departments.map(department => <option key={department.id} value={department.id}>{department.name}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="statusId">Trạng Thái</label>
                  <select className="form-control" id="statusId" name="statusId" value={student.statusId} onChange={handleChange} required>
                    {statuses.map(status => <option key={status.id} value={status.id}>{status.name}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="khoaId">Khóa Học</label>
                  <select className="form-control" id="khoaId" name="khoaId" value={student.khoaId} onChange={handleChange} required>
                    {khoas.map(khoa => <option key={khoa.id} value={khoa.id}>{khoa.name}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="programId">Chương Trình</label>
                  <select className="form-control" id="programId" name="programId" value={student.programId} onChange={handleChange} required>
                    {programs.map(program => <option key={program.id} value={program.id}>{program.name}</option>)}
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="diaChi">Địa Chỉ</label>
                  <input type="text" className="form-control" id="diaChi" name="diaChi" value={student.diaChi} onChange={handleChange} />
                </div>
                <div className="form-group">
                  <label htmlFor="email">Email</label>
                  <input type="email" className="form-control" id="email" name="email" value={student.email} onChange={handleChange} />
                </div>
                <div className="form-group">
                  <label htmlFor="soDienThoai">Số Điện Thoại</label>
                  <input type="text" className="form-control" id="soDienThoai" name="soDienThoai" value={student.soDienThoai} onChange={handleChange} />
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" onClick={onClose}>Đóng</button>
                  <button type="submit" className="btn btn-primary">Lưu</button>
                </div>
              </form>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default StudentModal;
