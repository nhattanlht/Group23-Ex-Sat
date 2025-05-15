import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';

const ClassPage = () => {
  const [courses, setCourses] = useState([]);
  const [activeCourses, setActiveCourses] = useState([]);

  const formFields = [
    { display: 'Mã Lớp Học', accessor: 'classId', type: 'text', required: true, disabled: true },
    { display: 'Tên Khóa Học', accessor: 'courseCode', type: 'select', options: activeCourses, required: true },
    { display: 'Năm Học', accessor: 'academicYear', type: 'select', optionsEndpoint: 'schoolyears', required: true },
    { display: 'Học Kỳ', accessor: 'semester', type: 'text', required: true },
    { display: 'Giảng Viên', accessor: 'teacher', type: 'text', required: true },
    { display: 'Số Lượng Tối Đa', accessor: 'maxStudents', type: 'number', required: true },
    { display: 'Lịch Học', accessor: 'schedule', type: 'text' },
    { display: 'Phòng Học', accessor: 'room', type: 'text' },
    { display: 'Thời gian hủy đăng ký', accessor: 'cancelDeadline', type: 'date', required: true },
  ];

  const tableFields = [
    { display: 'Mã Lớp Học', accessor: 'classId', type: 'text' },
    { display: 'Tên Khóa Học', accessor: 'courseCode', type: 'select', options: courses },
    { display: 'Năm Học', accessor: 'academicYear', type: 'select', optionsEndpoint: 'schoolyears' },
    { display: 'Học Kỳ', accessor: 'semester', type: 'text' },
    { display: 'Giảng Viên', accessor: 'teacher', type: 'text' },
    { display: 'Số Lượng Tối Đa', accessor: 'maxStudents', type: 'number' },
    { display: 'Lịch Học', accessor: 'schedule', type: 'text' },
    { display: 'Phòng Học', accessor: 'room', type: 'text' },
    { display: 'Thời gian hủy đăng ký', accessor: 'cancelDeadline', type: 'date' },
  ];

  useEffect(() => {
    loadMetadata();
  }, [])

  const loadMetadata = async () => {
    try {
      const courseRes = await axios.get(`${config.backendUrl}/api/course`);
      const course = courseRes.data.map((item) => ({
        id: item.courseCode,
        name: item.name
      }));
      setCourses(course);

      const activeCourseRes = await axios.get(`${config.backendUrl}/api/course/active`);
      const activeCourse = activeCourseRes.data.map((item) => ({
        id: item.courseCode,
        name: item.name
      }));
      setActiveCourses(activeCourse);


    } catch (error) {
      console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
    }

  };
  if (courses.length === 0 || activeCourses.length === 0) {
    return <PageLayout title="Danh sách lớp học"><p>Đang tải dữ liệu...</p></PageLayout>;
  }

  return (
    <PageLayout title="Danh sách lớp học">
      <DataList formFields={formFields} tableFields={tableFields} dataName="class" pk="classId" label="Lớp Học" />
    </PageLayout>
  );
};

export default ClassPage;
