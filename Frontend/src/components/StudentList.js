import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import Pagination from './Pagination';
import config from '../config';
import DataTable from './DataTable';
import { loadData, handleAddRow, handleEditRow, handleDeleteRow } from '../util/callCRUDApi';
import DataForm from './DataForm';
import { Search } from 'lucide-react';

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

  const [filters, setFilters] = useState({
    keyword: "",
    departmentId: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFilters((prevFilters) => ({
      ...prevFilters,
      [name]: value,
    }));
    console.log('filter', filters);
  };

  const fields = [
    { display: 'MSSV', accessor: 'mssv', type: "text", required: true },
    { display: 'Họ Tên', accessor: 'hoTen', type: "text", required: true },
    { display: 'Ngày Sinh', accessor: 'ngaySinh', type: "date", required: true },
    { display: 'Giới Tính', accessor: 'gioiTinh', type: "select", options: [{ id: "Nam", name: "Nam" }, { id: "Nữ", name: "Nữ" }, { id: "Khác", name: "Khác" }], required: true },
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
    { display: "Identification Id", accessor: "identificationId", type: "text", hidden: true },
    { display: 'Loại Giấy Tờ', accessor: 'identificationType', type: "select", options: [{ id: "CMND", name: "CMND" }, { id: "CCCD", name: "CCCD" }, { id: "Hộ Chiếu", name: "Hộ Chiếu" }], required: true, customeType: "identificationType" },
    {
      display: 'Thông Tin Giấy Tờ',
      accessor: 'identification',
      type: "group",
      fields: [
        { display: 'Loại Giấy Tờ', accessor: 'identification.identificationType', type: "text", required: true, hidden: true },
        { display: 'Số Giấy Tờ', accessor: 'identification.number', type: "text", required: true },
        { display: 'Ngày Cấp', accessor: 'identification.issueDate', type: "date", required: true },
        { display: 'Ngày Hết Hạn', accessor: 'identification.expiryDate', type: "date" },
        { display: 'Nơi Cấp', accessor: 'identification.issuedBy', type: "text", required: true },
        { display: 'Có Gắn Chip', accessor: 'identification.hasChip', type: "checkbox", condition: (formData) => formData.identificationType === "CCCD" },
        { display: 'Quốc Gia Cấp', accessor: 'identification.issuingCountry', type: "text", condition: (formData) => formData.identificationType === "Hộ Chiếu" },
        { display: 'Ghi Chú', accessor: 'identification.notes', type: "text", condition: (formData) => formData.identificationType === "Hộ Chiếu" },
      ],
      customeType: "identification"
    },
  ];

  useEffect(() => {
    loadStudents(currentPage, filters);
    loadMetadata();
  }, [currentPage]);

  // Gọi API lấy danh sách sinh viên
  const loadStudents = async (page, filters) => {
    try {
      const data = await loadData('students', page, filters);
      setStudents(data.students || []);
      setCurrentPage(data.currentPage || 1);
      setTotalPages(data.totalPages || 1);
      console.log("search", data.students);
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

  const transformToNestedObject2 = (flatObject) => {
    const nestedObject = {};

    Object.keys(flatObject).forEach((key) => {
      // Directly assign the value to the corresponding key
      nestedObject[key] = flatObject[key];
    });

    return nestedObject;
  };

  const handleAddStudent = async (student) => {
    try {
      student.identification["identification.identificationType"] = student.identificationType;

      if (!student.identification?.["identification.hasChip"]) {
        student.identification["identification.hasChip"] = null;
      }
      if (!student.identification?.["identification.issuingCountry"]) {
        student.identification["identification.issuingCountry"] = null;
      }
      if (!student.identification?.["identification.notes"]) {
        student.identification["identification.notes"] = null;
      }

      const newStudent4 = transformToNestedObject(student.identification);

      const identification = await axios.post(`${config.backendUrl}/api/identification`, newStudent4.identification);
      student.identificationId = identification.data.id;

      delete student.identification;
      delete student.identificationType;

      const newStudent1 = transformToNestedObject(student.diaChiNhanThu);

      const diaChiNhanThu = await axios.post(`${config.backendUrl}/api/address`, newStudent1.diaChiNhanThu);
      student.diaChiNhanThuId = diaChiNhanThu.data.id;

      delete student.diaChiNhanThu;


      if (student.diaChiThuongTru) {
        if (student.diaChiThuongTru?.["diaChiThuongTru.houseNumber"]) {
          const newStudent2 = transformToNestedObject(student.diaChiThuongTru);
          const diaChiThuongTru = await axios.post(`${config.backendUrl}/api/address`, newStudent2.diaChiThuongTru);
          student.diaChiThuongTruId = diaChiThuongTru.data.id;
        }
        else {
          student.diaChiThuongTruId = null;
        }
      }
      else {
        student.diaChiThuongTruId = null;
      }

      delete student.diaChiThuongTru;

      if (student.diaChiTamTru) {
        if (student.diaChiTamTru?.["diaChiTamTru.houseNumber"]) {
          const newStudent3 = transformToNestedObject(student.diaChiTamTru);
          const diaChiTamTru = await axios.post(`${config.backendUrl}/api/address`, newStudent3.diaChiTamTru);
          student.diaChiTamTruId = diaChiTamTru.data.id;
        }
        else {
          student.diaChiTamTruId = null;
        }
      }
      else {
        student.diaChiTamTruId = null;
      }

      delete student.diaChiTamTru;

      const response = await handleAddRow('students', student);
      
      loadStudents(currentPage, filters);

      return response;
    } catch (error) {
      alert('Lỗi khi thêm sinh viên!');
      throw error;
    }
  };

  const handleEditStudent = async (student) => {
    try {
      student.identification["identificationType"] = student.identificationType;

      if (!student.identification["hasChip"]) {
        student.identification["hasChip"] = null;
      }
      if (!student.identification["issuingCountry"]) {
        student.identification["issuingCountry"] = null;
      }
      if (!student.identification["notes"]) {
        student.identification["notes"] = null;
      }

      delete student.identification["id"];

      const newStudent4 = transformToNestedObject2(student.identification);

      const identification = await axios.post(`${config.backendUrl}/api/identification`, newStudent4);
      student.identificationId = identification.data.id;

      delete student.identification;
      delete student.identificationType;
      // Handle diaChiNhanThu
      delete student.diaChiNhanThu["id"];
      const newStudent1 = transformToNestedObject2(student.diaChiNhanThu);

      const diaChiNhanThu = await axios.post(`${config.backendUrl}/api/address`, newStudent1);
      student.diaChiNhanThuId = diaChiNhanThu.data.id;

      delete student.diaChiNhanThu;

      // Handle diaChiThuongTru
      if (student.diaChiThuongTru) {
        delete student.diaChiThuongTru["id"];

        const newStudent2 = transformToNestedObject2(student.diaChiThuongTru);
        const diaChiThuongTru = await axios.post(`${config.backendUrl}/api/address`, newStudent2);
        student.diaChiThuongTruId = diaChiThuongTru.data.id;
      }
      else {
        student.diaChiThuongTruId = null;
      }
      delete student.diaChiThuongTru;

      // Handle diaChiTamTru
      if (student.diaChiTamTru) {
        delete student.diaChiTamTru["id"];

        const newStudent3 = transformToNestedObject2(student.diaChiTamTru);
        const diaChiTamTru = await axios.post(`${config.backendUrl}/api/address`, newStudent3);
        student.diaChiTamTruId = diaChiTamTru.data.id;
      }
      else {
        student.diaChiTamTruId = null;
      }
      delete student.diaChiTamTru;

      console.log(student);

      const response = await handleEditRow('students', student.mssv, student);
      loadStudents(currentPage, filters);
      
      return response;
    } catch (error) {
      alert('Lỗi khi chỉnh sửa sinh viên!');
      throw error;
    }
  };

  const handleDeleteStudent = async (mssv) => {
    try {
      await handleDeleteRow('students', mssv);
      loadStudents(currentPage, filters);
    } catch (error) {
      alert('Lỗi khi xóa sinh viên!');
    }
  };

  const initializeFormData = async (fields, modalData) => {
    const initialData = fields.reduce((acc, field) => {
      if (field.type === "group") {
        // Initialize nested group fields
        acc[field.accessor] = field.fields.reduce((subAcc, subField) => {
          subAcc[subField.accessor] = "";
          return subAcc;
        }, {});
      } else {
        // Initialize flat fields
        acc[field.accessor] = "";
      }
      return acc;
    }, {});

    // Populate with existing data
    if (modalData) {
      Object.keys(modalData).forEach((key) => {
        initialData[key] = modalData[key];
      });

      if (modalData.diaChiNhanThuId) {
        try {
          const response = await axios.get(`${config.backendUrl}/api/address/${modalData.diaChiNhanThuId}`);
          initialData.diaChiNhanThu = response.data; // Populate address fields
        } catch (error) {
          console.error("Error fetching address:", error);
        }
      }

      if (modalData.diaChiThuongTruId) {
        try {
          const response = await axios.get(`${config.backendUrl}/api/address/${modalData.diaChiThuongTruId}`);
          initialData.diaChiThuongTru = response.data; // Populate address fields
        } catch (error) {
          console.error("Error fetching address:", error);
        }
      }

      if (modalData.diaChiTamTruId) {
        try {
          const response = await axios.get(`${config.backendUrl}/api/address/${modalData.diaChiTamTruId}`);
          initialData.diaChiTamTru = response.data; // Populate address fields
        } catch (error) {
          console.error("Error fetching address:", error);
        }
      }

      try {
        const response = await axios.get(`${config.backendUrl}/api/identification/${modalData.identificationId}`);
        initialData.identification = response.data; // Populate identification fields
        initialData.identification["issueDate"] = initialData.identification["issueDate"].split("T")[0];
        initialData.identification["expiryDate"] = initialData.identification["expiryDate"].split("T")[0];
      } catch (error) {
        console.error("Error fetching identification:", error);
      }

      initialData.identificationType = initialData.identification["identificationType"];
    }

    return initialData;
  };

  return (
    <div>
      <div className="flex space-x-2">
        <button className="btn btn-success" onClick={() => { setModalData(null); setShowModal(true); }}>
          Thêm Sinh Viên
        </button>
        <Link to='/data'>
          <button className='btn btn-primary'>
            Import/Export
          </button>
        </Link>
        <form id="searchForm" className='flex space-x-2' onSubmit={(e) => { e.preventDefault(); loadStudents(1, filters); }}>
          <input type="text"
            id="searchInput"
            placeholder="Tìm kiếm theo tên, MSSV"
            name="keyword"
            value={filters.keyword}
            onChange={handleChange}
          />

          <select id="departmentId" onChange={handleChange} name="departmentId" value={filters.departmentId}>
            <option value="">Chọn Khoa</option>
            {departments.map((department) => (
              <option key={department.id} value={department.id}>{department.name}</option>
            ))}
          </select>

          <button type="submit" className='btn-primary'><Search size={16} /></button>
        </form>

      </div>
      <DataTable fields={fields} dataSet={students} handleEdit={(student) => { setModalData(student); setShowModal(true); }} handleDelete={(student) => { handleDeleteStudent(student.mssv) }}></DataTable>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {showModal && <DataForm fields={fields} data={modalData} onSave={modalData ? handleEditStudent : handleAddStudent} onClose={() => setShowModal(false)} label='Sinh Viên' initializeFormData={initializeFormData} />}
    </div>
  );
};

export default StudentList;
