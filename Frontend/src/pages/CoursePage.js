import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import config from '../config';
import axios from 'axios';
import { useLanguage } from '../contexts/LanguageContext';

const CoursePage = () => {
  const { translate } = useLanguage();
  const [courses, setCourses] = useState([]);
  const formFields = [
    { display: translate('course.fields.courseCode'), accessor: 'courseCode', type: 'text', required: true, disabled: true },
    { display: translate('course.fields.name'), accessor: 'name', type: 'text', required: true },
    { display: translate('course.fields.credits'), accessor: 'credits', type: 'number', required: true },
    { display: translate('course.fields.department'), accessor: 'departmentId', type: 'select', optionsEndpoint: 'departments', required: true },
    { display: translate('course.fields.description'), accessor: 'description', type: 'textarea' },
    { display: translate('course.fields.prerequisiteCourse'), accessor: 'prerequisiteCourseCode', type: 'select', options: courses },
    { display: translate('course.fields.isActive'), accessor: 'isActive', type: 'checkbox' },
  ];

  useEffect(() => {
    loadMetadata();
    // eslint-disable-next-line
  }, [])

  const loadMetadata = async () => {
    try {
      const courseRes = await axios.get(`${config.backendUrl}/api/course`);
      const course = courseRes.data.data.map((item) => ({
        id: item.courseCode,
        name: item.name
      }));
      setCourses(course);
    } catch (error) {
      console.error('Error loading course metadata:', error);
    }
  };

  return (
    <PageLayout title={translate('course.title')}>
      <DataList formFields={formFields} dataName="course" pk="courseCode" label={translate('course.label')} />
    </PageLayout>
  );
};

export default CoursePage;
