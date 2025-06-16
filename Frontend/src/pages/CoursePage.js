import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import config from '../config';
import axios from 'axios';

const CoursePage = () => {
  const [courses, setCourses] = useState([]);
  const formFields = [
    { display: 'Mã Khóa Học', accessor: 'courseCode', type: 'text', required: true, disabled: true },
    { display: 'Tên Khóa Học', accessor: 'name', type: 'text', required: true },
    { display: 'Số Tín Chỉ', accessor: 'credits', type: 'number', required: true },
    { display: 'Khoa', accessor: 'departmentId', type: 'select', optionsEndpoint: 'departments', required: true },
    { display: 'Mô Tả', accessor: 'description', type: 'textarea' },
    { display: 'Môn Tiên Quyết', accessor: 'prerequisiteCourseCode', type: 'select', options: courses },
    { display: 'Hoạt Động', accessor: 'isActive', type: 'checkbox' },
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
    } catch (error) {
      console.error('Error loading course metadata:', error);
    }
  };

  return (
    <PageLayout title="Danh sách khóa học">
      <DataList formFields={formFields} dataName="course" pk="courseCode" label="Khóa Học" />
    </PageLayout>
  );
};

export default CoursePage;
