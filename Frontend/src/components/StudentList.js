import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import Pagination from './Pagination';
import config from '../config';
import DataTable from './DataTable';
import { loadData, handleAddRow, handleEditRow, handleDeleteRow } from '../util/callCRUDApi';
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
    { display: translate('student.fields.student_id'), accessor: 'StudentId', type: "text", required: true},
    { display: translate('student.fields.full_name'), accessor: 'FullName', type: "text", required: true },
    { display: translate('student.fields.date_of_birth'), accessor: 'DateOfBirth', type: "date", required: true },
    { display: translate('student.fields.gender'), accessor: 'Gender', type: "select", 
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
    { display: translate('student.fields.phone'), accessor: 'PhoneNumber', type: "text", required: true },
    { display: translate('student.fields.nationality'), accessor: 'Nationality', type: "text", required: true },
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
    {
      display: translate('student.fields.identification'),
      accessor: 'identification',
      type: "group",
      fields: [
        { display: translate('student.form.identification.type'), accessor: 'identificationType', type: "text", required: true, hidden: true },
        { display: translate('student.form.identification.number'), accessor: 'number', type: "text", required: true },
        { display: translate('student.form.identification.issue_date'), accessor: 'issueDate', type: "date", required: true },
        { display: translate('student.form.identification.expiry_date'), accessor: 'expiryDate', type: "date" },
        { display: translate('student.form.identification.issued_by'), accessor: 'issuedBy', type: "text", required: true },
        { display: translate('student.form.identification.has_chip'), accessor: 'hasChip', type: "checkbox", condition: (formData) => formData.identificationType === "CCCD" },
        { display: translate('student.form.identification.issuing_country'), accessor: 'issuingCountry', type: "text", condition: (formData) => formData.identificationType === "Hộ Chiếu" },
        { display: translate('student.form.identification.notes'), accessor: 'notes', type: "text", condition: (formData) => formData.identificationType === "Hộ Chiếu" },
      ],
      customeType: "identification"
    },
  ];

  const addresses = {};
  const getViewAddress = async (id) => {
    if (!addresses[id]) { // Avoid duplicate API calls for the same ID
      try {
        console.log('Fetching address for ID:', id);
        const response = await axios.get(`${config.backendUrl}/api/address/${id}`);
        console.log('Address API response:', response.data);
        addresses[id] = response.data;
        console.log('Updated addresses object:', addresses);
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
        const response = await axios.get(`${config.backendUrl}/api/identification/${id}`);
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
      const data = await loadData('students', page, filters);
      console.log('Raw student data with addresses:', data.students);
      console.log('First student full data:', JSON.stringify(data.students[0], null, 2));
      
      // Tạo mảng promises để lấy tất cả địa chỉ và thông tin định danh
      const promises = data.students.flatMap(student => {
        const addressPromises = [];
        console.log('Processing student:', student);
        
        // Kiểm tra cả hai dạng tên trường có thể có
        const permanentId = student.permanentAddressId || student.PermanentAddressId;
        const temporaryId = student.temporaryAddressId || student.TemporaryAddressId;
        const registeredId = student.registeredAddressId || student.RegisteredAddressId;
        
        console.log('Student address IDs:', {
          permanentId,
          temporaryId,
          registeredId
        });
        
        if (permanentId) {
          console.log('Adding permanent address promise for ID:', permanentId);
          addressPromises.push(getViewAddress(permanentId));
        }
        if (registeredId) {
          console.log('Adding registered address promise for ID:', registeredId);
          addressPromises.push(getViewAddress(registeredId));
        }
        if (temporaryId) {
          console.log('Adding temporary address promise for ID:', temporaryId);
          addressPromises.push(getViewAddress(temporaryId));
        }
        if (student.identificationId) {
          addressPromises.push(getIdentifications(student.identificationId));
        }
        return addressPromises;
      });

      // Đợi tất cả promises hoàn thành
      await Promise.all(promises);
      
      console.log('Final addresses object before formatting:', addresses);
      const formattedData = formatDataSetForTable(data.students, fields, {
        addresses,
        identifications,
      });
      console.log('Formatted student data:', formattedData);
      setStudents(formattedData);
      setCurrentPage(data.currentPage || 1);
      setTotalPages(data.totalPages || 1);
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
      fields[4].options = depRes.data || [];
      fields[5].options = statusRes.data || [];
      fields[6].options = yearRes.data || [];
      fields[7].options = programRes.data || [];
      setDepartments(depRes.data || []);
      setStatuses(statusRes.data || []);
      setStudyPrograms(programRes.data || []);
      setSchoolYears(yearRes.data || []);
    } catch (error) {
      console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
    }
  };

  const handleAddStudent = async (studentForm) => {
    try {
      console.log('Starting add student with form data:', studentForm);

      // Normalize the data
      const normalizedStudent = {
        ...studentForm,
        FullName: studentForm.FullName?.trim(),
        StudentId: studentForm.StudentId,
        Gender: studentForm.Gender || 'Nam',
        DateOfBirth: studentForm.DateOfBirth,
        PhoneNumber: studentForm.PhoneNumber,
        Nationality: studentForm.Nationality?.trim() || 'Việt Nam',
        DepartmentId: parseInt(studentForm.departmentId || studentForm.DepartmentId),
        SchoolYearId: parseInt(studentForm.schoolYearId || studentForm.SchoolYearId),
        StudyProgramId: parseInt(studentForm.studyProgramId || studentForm.StudyProgramId),
        StatusId: parseInt(studentForm.statusId || studentForm.StatusId || 1) // Default to "Đang học"
      };

      console.log('Normalized student data:', normalizedStudent);

      // Xử lý giấy tờ tùy thân
      if (studentForm.identification) {
        console.log('Processing identification:', studentForm.identification);
        try {
          // Chuẩn hóa dữ liệu giấy tờ tùy thân
          const identificationData = {
            identificationType: studentForm.identification.identificationType || 'CCCD',
            number: studentForm.identification.number,
            issueDate: studentForm.identification.issueDate,
            expiryDate: studentForm.identification.expiryDate,
            issuedBy: studentForm.identification.issuedBy,
            hasChip: studentForm.identification.identificationType === 'CCCD' ? true : null,
            issuingCountry: studentForm.identification.identificationType === 'Passport' ? studentForm.identification.issuingCountry : null,
            notes: studentForm.identification.notes
          };

          // Validate required fields
          if (!identificationData.number) {
            throw new Error('Số giấy tờ tùy thân không được để trống');
          }
          if (!identificationData.issueDate) {
            throw new Error('Ngày cấp không được để trống');
          }
          if (!identificationData.issuedBy) {
            throw new Error('Nơi cấp không được để trống');
          }

          console.log('Sending identification data:', identificationData);

          const identification = await axios.post(
            `${config.backendUrl}/api/identification`,
            identificationData,
            {
              headers: {
                'Content-Type': 'application/json'
              }
            }
          );
          console.log('Identification created:', identification.data);
          normalizedStudent.identificationId = identification.data.id;
          delete normalizedStudent.identification;
        } catch (error) {
          console.error('Error creating identification:', error.response?.data);
          if (error.response?.data?.errors) {
            const errorMessages = Object.values(error.response.data.errors).flat().join('\n');
            throw new Error(`Lỗi khi tạo thông tin giấy tờ tùy thân:\n${errorMessages}`);
          }
          throw new Error(error.message || 'Lỗi khi tạo thông tin giấy tờ tùy thân');
        }
      }

      // Xử lý địa chỉ thường trú
      if (studentForm.permanentAddress) {
        console.log('Processing permanent address:', studentForm.permanentAddress);
        try {
          const permanentAddress = await axios.post(
            `${config.backendUrl}/api/address`,
            studentForm.permanentAddress,
            {
              headers: {
                'Content-Type': 'application/json'
              }
            }
          );
          console.log('Permanent address created:', permanentAddress.data);
          normalizedStudent.permanentAddressId = permanentAddress.data.id;
          delete normalizedStudent.permanentAddress;
        } catch (error) {
          console.error('Error creating permanent address:', error.response?.data);
          throw new Error('Lỗi khi tạo địa chỉ thường trú');
        }
      }

      // Xử lý địa chỉ tạm trú
      if (studentForm.temporaryAddress && Object.values(studentForm.temporaryAddress).some(value => value)) {
        console.log('Processing temporary address:', studentForm.temporaryAddress);
        try {
          const temporaryAddress = await axios.post(
            `${config.backendUrl}/api/address`,
            studentForm.temporaryAddress,
            {
              headers: {
                'Content-Type': 'application/json'
              }
            }
          );
          console.log('Temporary address created:', temporaryAddress.data);
          normalizedStudent.temporaryAddressId = temporaryAddress.data.id;
          delete normalizedStudent.temporaryAddress;
        } catch (error) {
          console.error('Error creating temporary address:', error.response?.data);
          throw new Error('Lỗi khi tạo địa chỉ tạm trú');
        }
      }

      // Xử lý địa chỉ đăng ký
      if (studentForm.registeredAddress && Object.values(studentForm.registeredAddress).some(value => value)) {
        console.log('Processing registered address:', studentForm.registeredAddress);
        try {
          const registeredAddress = await axios.post(
            `${config.backendUrl}/api/address`,
            studentForm.registeredAddress,
            {
              headers: {
                'Content-Type': 'application/json'
              }
            }
          );
          console.log('Registered address created:', registeredAddress.data);
          normalizedStudent.registeredAddressId = registeredAddress.data.id;
          delete normalizedStudent.registeredAddress;
        } catch (error) {
          console.error('Error creating registered address:', error.response?.data);
          throw new Error('Lỗi khi tạo địa chỉ đăng ký');
        }
      }

      console.log('Sending final student data to API:', normalizedStudent);

      // Gọi API tạo sinh viên mới
      const response = await handleAddRow("students", normalizedStudent);
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
      console.error("Error details:", error.response?.data);
      alert(error.response?.data?.message || error.message || translate('student.messages.add_error'));
      return false;
    }
  };

  const handleEditStudent = async (student) => {
    try {
      console.log('Starting edit student with data:', student);

      // Validate required fields
      if (!student.FullName || !student.FullName.trim()) {
        throw new Error(translate('student.messages.required_field'));
      }

      if (!student.Nationality || !student.Nationality.trim()) {
        throw new Error(translate('student.messages.nationality_required'));
      }

      // Normalize the data
      const normalizedStudent = {
        ...student,
        FullName: student.FullName.trim(),
        StudentId: student.StudentId || student.studentId,
        Gender: student.Gender || student.gender,
        DateOfBirth: student.DateOfBirth || student.dateOfBirth,
        PhoneNumber: student.PhoneNumber || student.phoneNumber,
        Nationality: student.Nationality.trim() || student.nationality,
        DepartmentId: parseInt(student.DepartmentId || student.departmentId),
        SchoolYearId: parseInt(student.SchoolYearId || student.schoolYearId),
        StudyProgramId: parseInt(student.StudyProgramId || student.studyProgramId),
        StatusId: parseInt(student.StatusId || student.statusId)
      };

      console.log('Normalized student data:', normalizedStudent);

      // Xử lý giấy tờ tùy thân
      if (student.identification) {
        console.log('Processing identification:', student.identification);
        const oldIdentificationId = student.identificationId;
        delete student.identification["id"];

        const identification = await axios.post(`${config.backendUrl}/api/identification`, student.identification);
        console.log('Created new identification:', identification.data);
        normalizedStudent.identificationId = identification.data.id;

        if (oldIdentificationId) {
          try {
            await axios.delete(`${config.backendUrl}/api/identification/${oldIdentificationId}`);
            console.log('Deleted old identification:', oldIdentificationId);
          } catch (error) {
            console.error("Error deleting old identification:", error);
          }
        }

        delete normalizedStudent.identification;
        delete normalizedStudent.identificationType;
      }

      // Xử lý địa chỉ thường trú
      if (student.permanentAddress) {
        console.log('Processing permanent address:', student.permanentAddress);
        const oldPermanentAddressId = student.permanentAddressId;
        delete student.permanentAddress["id"];

        const permanentAddress = await axios.post(`${config.backendUrl}/api/address`, student.permanentAddress);
        console.log('Created new permanent address:', permanentAddress.data);
        normalizedStudent.permanentAddressId = permanentAddress.data.id;

        if (oldPermanentAddressId) {
          try {
            await axios.delete(`${config.backendUrl}/api/address/${oldPermanentAddressId}`);
            console.log('Deleted old permanent address:', oldPermanentAddressId);
          } catch (error) {
            console.error("Error deleting old permanent address:", error);
          }
        }
        delete normalizedStudent.permanentAddress;
      }

      // Xử lý địa chỉ tạm trú
      if (student.temporaryAddress && Object.values(student.temporaryAddress).some(value => value)) {
        console.log('Processing temporary address:', student.temporaryAddress);
        const oldTemporaryAddressId = student.temporaryAddressId;
        delete student.temporaryAddress["id"];

        const temporaryAddress = await axios.post(`${config.backendUrl}/api/address`, student.temporaryAddress);
        console.log('Created new temporary address:', temporaryAddress.data);
        normalizedStudent.temporaryAddressId = temporaryAddress.data.id;

        if (oldTemporaryAddressId) {
          try {
            await axios.delete(`${config.backendUrl}/api/address/${oldTemporaryAddressId}`);
            console.log('Deleted old temporary address:', oldTemporaryAddressId);
          } catch (error) {
            console.error("Error deleting old temporary address:", error);
          }
        }
      } else {
        normalizedStudent.temporaryAddressId = null;
      }
      delete normalizedStudent.temporaryAddress;

      // Xử lý địa chỉ đăng ký
      if (student.registeredAddress && Object.values(student.registeredAddress).some(value => value)) {
        console.log('Processing registered address:', student.registeredAddress);
        const oldRegisteredAddressId = student.registeredAddressId;
        delete student.registeredAddress["id"];

        const registeredAddress = await axios.post(`${config.backendUrl}/api/address`, student.registeredAddress);
        console.log('Created new registered address:', registeredAddress.data);
        normalizedStudent.registeredAddressId = registeredAddress.data.id;

        if (oldRegisteredAddressId) {
          try {
            await axios.delete(`${config.backendUrl}/api/address/${oldRegisteredAddressId}`);
            console.log('Deleted old registered address:', oldRegisteredAddressId);
          } catch (error) {
            console.error("Error deleting old registered address:", error);
          }
        }
      } else {
        normalizedStudent.registeredAddressId = null;
      }
      delete normalizedStudent.registeredAddress;

      console.log('Sending update request with data:', normalizedStudent);

      // Gọi API cập nhật thông tin sinh viên
      const response = await handleEditRow("students", normalizedStudent.StudentId, normalizedStudent);
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
      return false;
    }
  };

  const handleDeleteStudent = async (StudentId) => {
    try {
      await handleDeleteRow('students', StudentId);
      loadStudents(currentPage, filters);
    } catch (error) {
      alert('Lỗi khi xóa sinh viên!');
    }
  };

  const initializeFormData = async (fields, modalData) => {
    const initialData = {};

    if (!modalData) {
      // Generate new StudentId
      try {
        const response = await axios.get(`${config.backendUrl}/api/students/newid`);
        initialData['StudentId'] = response.data;
      } catch (error) {
        console.error('Error getting new student ID:', error);
      }
      return initialData;
    }

    // Đảm bảo trường StudentId luôn có giá trị
    initialData['StudentId'] = modalData.studentId || modalData.StudentId || '';

    // Đảm bảo trường FullName luôn có giá trị
    initialData['FullName'] = modalData.fullName || modalData.FullName || '';

    // Đảm bảo trường Gender luôn có giá trị
    initialData['Gender'] = modalData.gender || modalData.Gender || '';

    // Đảm bảo trường PhoneNumber luôn có giá trị
    initialData['PhoneNumber'] = modalData.phoneNumber || modalData.PhoneNumber || '';

    // Đảm bảo trường Nationality luôn có giá trị
    initialData['Nationality'] = modalData.nationality || modalData.Nationality || '';

    // Đảm bảo trường DateOfBirth luôn có giá trị đúng định dạng
    if (modalData.dateOfBirth || modalData.DateOfBirth) {
      const dateValue = modalData.dateOfBirth || modalData.DateOfBirth;
      const date = new Date(dateValue);
      if (!isNaN(date)) {
        initialData['DateOfBirth'] = date.toISOString().split('T')[0];
      }
    }

    // Đảm bảo các trường khác được sao chép
    fields.forEach(field => {
      const fieldName = field.accessor;
      if (!initialData[fieldName] && modalData[fieldName] !== undefined) {
        initialData[fieldName] = modalData[fieldName];
      }
    });

    // Khởi tạo địa chỉ thường trú
    if (modalData.permanentAddressId) {
      try {
        const response = await axios.get(`${config.backendUrl}/api/address/${modalData.permanentAddressId}`);
        initialData.permanentAddress = response.data;
      } catch (error) {
        console.error("Error fetching permanent address:", error);
      }
    }

    // Khởi tạo địa chỉ tạm trú
    if (modalData.temporaryAddressId) {
      try {
        const response = await axios.get(`${config.backendUrl}/api/address/${modalData.temporaryAddressId}`);
        initialData.temporaryAddress = response.data;
      } catch (error) {
        console.error("Error fetching temporary address:", error);
      }
    }

    // Khởi tạo địa chỉ đăng ký
    if (modalData.registeredAddressId) {
      try {
        const response = await axios.get(`${config.backendUrl}/api/address/${modalData.registeredAddressId}`);
        initialData.registeredAddress = response.data;
      } catch (error) {
        console.error("Error fetching registered address:", error);
      }
    }

    // Khởi tạo thông tin giấy tờ
    if (modalData.identificationId) {
      try {
        const response = await axios.get(`${config.backendUrl}/api/identification/${modalData.identificationId}`);
        const identificationData = response.data;
        
        // Format dates for identification
        if (identificationData.issueDate) {
          identificationData.issueDate = new Date(identificationData.issueDate).toISOString().split('T')[0];
        }
        if (identificationData.expiryDate) {
          identificationData.expiryDate = new Date(identificationData.expiryDate).toISOString().split('T')[0];
        }
        
        initialData.identification = identificationData;
        initialData.identificationType = identificationData.identificationType;
      } catch (error) {
        console.error("Error fetching identification:", error);
      }
    }

    console.log('Initialized form data:', initialData);
    return initialData;
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
          { display: translate('student.fields.student_id'), accessor: 'StudentId' },
          { display: translate('student.fields.full_name'), accessor: 'FullName', type: "text" },
          { display: translate('student.fields.date_of_birth'), accessor: 'DateOfBirth' },
          { display: translate('student.fields.gender'), accessor: 'Gender' },
          { display: translate('student.fields.department'), accessor: 'departmentId' },
          { display: translate('student.fields.status'), accessor: 'statusId' },
          { display: translate('student.fields.school_year'), accessor: 'schoolYearId' },
          { display: translate('student.fields.study_program'), accessor: 'studyProgramId' },
          { display: translate('student.fields.email'), accessor: 'email' },
          { display: translate('student.fields.phone'), accessor: 'PhoneNumber' },
          { display: translate('student.fields.nationality'), accessor: 'Nationality' },
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
        handleDelete={(student) => { handleDeleteStudent(student.StudentId) }}
      />
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {showModal && <DataForm fields={fields} data={modalData} onSave={modalData ? handleEditStudent : handleAddStudent} onClose={() => setShowModal(false)} label={translate('student.title')} initializeFormData={initializeFormData} />}
    </div>
  );
};

export default StudentList;
