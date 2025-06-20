import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import Pagination from './Pagination';
import config from '../config';
import DataTable from './DataTable';
import { loadData, handleAddRow, handleEditRow, handleDeleteRow, loadDataId } from '../util/callCRUDApi';
import DataForm from './DataForm';
import { Search, Plus, FileInput } from 'lucide-react';
import { formatDataSetForTable } from '../util/formatData';
import { useLanguage } from '../contexts/LanguageContext';

const StudentList = () => {
  const { translate } = useLanguage();
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
    { display: translate('student.fields.student_id'), accessor: 'studentId', type: "text", required: true, disabled: true },
    { display: translate('student.fields.full_name'), accessor: 'fullName', type: "text", required: true },
    { display: translate('student.fields.date_of_birth'), accessor: 'dateOfBirth', type: "date", required: true },
    { display: translate('student.fields.gender'), accessor: 'gender', type: "select", 
      options: [
        { id: "Nam", name: translate('student.form.gender_options.male') }, 
        { id: "Nữ", name: translate('student.form.gender_options.female') }, 
        { id: "Khác", name: translate('student.form.gender_options.other') }
      ], 
      required: true 
    },
    { display: translate('student.fields.department'), accessor: 'departmentId', type: "select", options: departments, required: true },
    { display: translate('student.fields.status'), accessor: 'statusId', type: "select", options: statuses, required: true },
    { display: translate('student.fields.school_year'), accessor: 'schoolYearId', type: "select", options: schoolYears, required: true },
    { display: translate('student.fields.study_program'), accessor: 'studyProgramId', type: "select", options: studyPrograms, required: true },
    { display: translate('student.fields.email'), accessor: 'email', type: "email", required: true },
    { display: translate('student.fields.phone'), accessor: 'phoneNumber', type: "text", required: true },
    { display: translate('student.fields.nationality'), accessor: 'nationality', type: "text", required: true },
    {
      display: translate('student.fields.permanent_address'),
      accessor: 'permanentAddress',
      type: "group",
      fields: [
        { display: translate('student.form.address.house_number'), accessor: 'houseNumber', type: "text", required: true },
        { display: translate('student.form.address.street_name'), accessor: 'streetName', type: "text", required: true },
        { display: translate('student.form.address.ward'), accessor: 'ward', type: "text", required: true },
        { display: translate('student.form.address.district'), accessor: 'district', type: "text", required: true },
        { display: translate('student.form.address.province'), accessor: 'province', type: "text", required: true },
        { display: translate('student.form.address.country'), accessor: 'country', type: "text", required: true },
      ],
    },
    { display: translate('student.fields.permanent_address'), accessor: 'permanentAddressId', type: "text", hidden: true },
    {
      display: translate('student.fields.temporary_address'),
      accessor: 'temporaryAddress',
      type: "group",
      fields: [
        { display: translate('student.form.address.house_number'), accessor: 'houseNumber', type: "text" },
        { display: translate('student.form.address.street_name'), accessor: 'streetName', type: "text" },
        { display: translate('student.form.address.ward'), accessor: 'ward', type: "text" },
        { display: translate('student.form.address.district'), accessor: 'district', type: "text" },
        { display: translate('student.form.address.province'), accessor: 'province', type: "text" },
        { display: translate('student.form.address.country'), accessor: 'country', type: "text" },
      ],
    },
    { display: translate('student.fields.temporary_address'), accessor: 'temporaryAddressId', type: "text", hidden: true },
    {
      display: translate('student.fields.registered_address'),
      accessor: 'registeredAddress',
      type: "group",
      fields: [
        { display: translate('student.form.address.house_number'), accessor: 'houseNumber', type: "text" },
        { display: translate('student.form.address.street_name'), accessor: 'streetName', type: "text" },
        { display: translate('student.form.address.ward'), accessor: 'ward', type: "text" },
        { display: translate('student.form.address.district'), accessor: 'district', type: "text" },
        { display: translate('student.form.address.province'), accessor: 'province', type: "text" },
        { display: translate('student.form.address.country'), accessor: 'country', type: "text" },
      ],
    },
    { display: translate('student.fields.registered_address'), accessor: 'registeredAddressId', type: "text", hidden: true },
    { display: "Identification Id", accessor: "identificationId", type: "text", hidden: true },
        {
      display: translate('student.fields.identification'),
      accessor: 'identification',
      type: "group",
      fields: [
        { 
          display: translate('student.form.identification.type'), 
          accessor: 'identificationType', 
          type: "select", 
          options: [
            { id: "CMND", name: translate('student.form.identification.types.cmnd') }, 
            { id: "CCCD", name: translate('student.form.identification.types.cccd') }, 
            { id: "Hộ Chiếu", name: translate('student.form.identification.types.passport') }
          ], 
          required: true, 
          customeType: "identificationType" 
        },
        { display: translate('student.form.identification.number'), accessor: 'number', type: "text", required: true },
        { display: translate('student.form.identification.issue_date'), accessor: 'issueDate', type: "date", required: true },
        { display: translate('student.form.identification.expiry_date'), accessor: 'expiryDate', type: "date" },
        { display: translate('student.form.identification.issued_by'), accessor: 'issuedBy', type: "text", required: true },
        { display: translate('student.form.identification.has_chip'), accessor: 'hasChip', type: "checkbox", condition: (formData) => formData.identification?.identificationType === "CCCD" },
        { display: translate('student.form.identification.issuing_country'), accessor: 'issuingCountry', type: "text", condition: (formData) => formData.identification?.identificationType === "Hộ Chiếu" },
        { display: translate('student.form.identification.notes'), accessor: 'notes', type: "text", condition: (formData) => formData.identification?.identificationType === "Hộ Chiếu" },
      ],
      customeType: "identification"
    },
  ];

  const addresses = {};
  const getViewAddress = async (id) => {
    if (!addresses[id]) { // Avoid duplicate API calls for the same ID
      try {
        
        const response = await loadDataId('address', id);
        
        addresses[id] = response.data;
        
      } catch (error) {
        console.error("Error fetching address:", error);
        addresses[id] = null;
      }
    }
  };

  const identifications = {};
  const getIdentifications = async (id) => {
    if (!identifications[id]) { // Avoid duplicate API calls for the same ID
      try {
        const response = await loadDataId('identification', id);
        identifications[id] = response.data;
      } catch (error) {
        console.error("Error fetching address:", error);
      }
    }
  };

  useEffect(() => {
    loadMetadata();
    loadStudents(currentPage, filters);
  }, [currentPage]);

  // Gọi API lấy danh sách sinh viên
  const loadStudents = async (page, filters = {}) => {
    try {
      const { data } = await loadData('students', page, filters);
      
      // Tạo mảng promises để lấy tất cả địa chỉ và thông tin định danh
      const promises = data.students.flatMap(student => {
        const addressPromises = [];
        
        const permanentId = student.permanentAddressId;
        const temporaryId = student.temporaryAddressId;
        const registeredId = student.registeredAddressId;
        
        if (permanentId) {
          addressPromises.push(getViewAddress(permanentId));
        }
        if (registeredId) {
          addressPromises.push(getViewAddress(registeredId));
        }
        if (temporaryId) {
          addressPromises.push(getViewAddress(temporaryId));
        }
        if (student.identificationId) {
          addressPromises.push(getIdentifications(student.identificationId));
        }
        return addressPromises;
      });

      // Đợi tất cả promises hoàn thành
      await Promise.all(promises);
      
      const formattedData = formatDataSetForTable(data.students, fields, {
        addresses,
        identifications,
      });
      
      setStudents(formattedData);
      setCurrentPage(data.currentPage || 1);
      setTotalPages(data.totalPages || 1);
    } catch (error) {
      console.error("Lỗi khi tải danh sách sinh viên:", error);
      alert(error.message || translate('student.messages.load_error'));
    }
  };

  // Gọi API lấy danh sách khoa, trạng thái, chương trình học
  const loadMetadata = async () => {
    try {
      const [depRes, statusRes, programRes, yearRes] = await Promise.all([
        axios.get(`${config.backendUrl}/api/departments`),
        axios.get(`${config.backendUrl}/api/studentstatus`),
        axios.get(`${config.backendUrl}/api/programs`),
        axios.get(`${config.backendUrl}/api/schoolyears`),
      ]);
      fields[4].options = depRes.data.data || [];
      fields[5].options = statusRes.data.data || [];
      fields[6].options = yearRes.data.data || [];
      fields[7].options = programRes.data.data || [];
      setDepartments(depRes.data.data || []);
      setStatuses(statusRes.data.data || []);
      setStudyPrograms(programRes.data.data || []);
      setSchoolYears(yearRes.data.data || []);
    } catch (error) {
      console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
    }
  };

  const handleAddStudent = async (studentForm) => {
    try {
      console.log('Starting add student with form data:', studentForm);
      // Gọi API tạo sinh viên mới
      const response = await handleAddRow("students", studentForm);
      console.log('API response:', response);
      
      if (response) {
        setShowModal(false);
        await loadStudents(currentPage);
        alert(translate('student.messages.add_success'));
        return true;
      }
      return false;
    } catch (error) {
      console.error("Error adding student:", error);
      console.error("Error details:", error.message); 
      alert(error.message || translate('student.messages.add_error'));
      throw error;
    }
  };

  const handleEditStudent = async (student) => {
    try {
      console.log('Starting edit student with data:', student);

      // Gọi API cập nhật thông tin sinh viên
      const response = await handleEditRow("students", student.studentId, student);
      console.log('Update response:', response);
      
      // Đóng modal và tải lại dữ liệu chỉ khi API trả về thành công
      if (response && response.success) {
        setShowModal(false);
        await loadStudents(currentPage);
        alert(translate('student.messages.edit_success'));
        return true;
      } else {
        throw new Error(response?.message || translate('student.messages.edit_error'));
      }
    } catch (error) {
      console.error("Error updating student:", error);
      alert(error.message || translate('student.messages.edit_error'));
      throw error;
    }
  };

  const handleDeleteStudent = async (StudentId) => {
    try {
      await handleDeleteRow('students', StudentId);
      loadStudents(currentPage, filters);
    } catch (error) {
      alert(error.message || translate('student.messages.delete_error'));
    }
  };

  const initializeFormData = async (fields, modalData) => {
  
    return modalData;
  };

  return (
    <div>
      <div className="flex flex-wrap items-center justify-between gap-4 mb-6">
        {/* Left side - Action buttons */}
        <div className="flex items-center gap-2">
          <button 
            className="btn bg-green-600 hover:bg-green-700 text-white flex items-center gap-2 px-4 py-2 rounded-md"
            onClick={() => { setModalData(null); setShowModal(true); }}
          >
            <Plus size={20} />
            {translate('student.add')}
          </button>
          <Link to='/data'>
            <button className='btn bg-blue-600 hover:bg-blue-700 text-white flex items-center gap-2 px-4 py-2 rounded-md'>
              <FileInput size={20} />
              {translate('student.import_export')}
            </button>
          </Link>
        </div>

        {/* Right side - Search form */}
        <form 
          id="searchForm" 
          className='flex flex-wrap items-center gap-2 flex-grow justify-end max-w-2xl' 
          onSubmit={(e) => { e.preventDefault(); loadStudents(1, filters); }}
        >
          <div className="flex-grow max-w-md">
            <input 
              type="text"
              id="searchInput"
              placeholder={translate('student.search.placeholder')}
              name="keyword"
              value={filters.keyword}
              onChange={handleChange}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          
          <select 
            id="departmentId" 
            onChange={handleChange} 
            name="departmentId" 
            value={filters.departmentId}
            className="px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="">{translate('student.search.department_placeholder')}</option>
            {departments.map((department) => (
              <option key={department.id} value={department.id}>{department.name}</option>
            ))}
          </select>

          <button 
            type="submit" 
            className='btn bg-blue-600 hover:bg-blue-700 text-white flex items-center gap-2 px-4 py-2 rounded-md min-w-[120px]'
          >
            <Search size={20} />
            {translate('student.search.button')}
          </button>
        </form>
      </div>

      <DataTable 
        fields={[
          { display: translate('student.fields.student_id'), accessor: 'studentId' },
          { display: translate('student.fields.full_name'), accessor: 'fullName', type: "text" },
          { display: translate('student.fields.date_of_birth'), accessor: 'dateOfBirth' },
          { display: translate('student.fields.gender'), accessor: 'gender' },
          { display: translate('student.fields.department'), accessor: 'departmentId' },
          { display: translate('student.fields.status'), accessor: 'statusId' },
          { display: translate('student.fields.school_year'), accessor: 'schoolYearId' },
          { display: translate('student.fields.study_program'), accessor: 'studyProgramId' },
          { display: translate('student.fields.email'), accessor: 'email' },
          { display: translate('student.fields.phone'), accessor: 'phoneNumber' },
          { display: translate('student.fields.nationality'), accessor: 'nationality' },
          { display: translate('student.fields.permanent_address'), accessor: 'permanentAddress', type: "group" },
          { display: translate('student.fields.temporary_address'), accessor: 'temporaryAddress', type: "group" },
          { display: translate('student.fields.registered_address'), accessor: 'registeredAddress', type: "group" },
          { display: translate('student.fields.identification'), accessor: 'identification', type: "group", customeType: "identification" }
        ]} 
        dataSet={students} 
        handleEdit={(student) => { 
          console.log('Student being edited:', student);
          setModalData(student.__original); 
          setShowModal(true); 
        }} 
        handleDelete={(student) => { handleDeleteStudent(student.studentId) }}
      />
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {showModal && <DataForm fields={fields} data={modalData} dataName={'student'} onSave={modalData ? handleEditStudent : handleAddStudent} onClose={() => setShowModal(false)} label={translate('student.title')} initializeFormData={initializeFormData} />}
    </div>
  );
};

export default StudentList;
