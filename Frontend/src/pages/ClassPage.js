import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';
import { loadData, loadDataNoPaging } from '../util/callCRUDApi';
import { useLanguage } from '../contexts/LanguageContext';

const ClassPage = () => {
  const { translate } = useLanguage();
  const [courses, setCourses] = useState([]);
  const [activeCourses, setActiveCourses] = useState([]);

  const formFields = [
    { display: translate('class.fields.classId'), accessor: 'classId', type: 'text', required: true, disabled: true },
    { display: translate('class.fields.courseCode'), accessor: 'courseCode', type: 'select', options: activeCourses, required: true },
    { display: translate('class.fields.academicYear'), accessor: 'academicYear', type: 'select', optionsEndpoint: 'schoolyears', required: true },
    { display: translate('class.fields.semester'), accessor: 'semester', type: 'text', required: true },
    { display: translate('class.fields.teacher'), accessor: 'teacher', type: 'text', required: true },
    { display: translate('class.fields.maxStudents'), accessor: 'maxStudents', type: 'number', required: true },
    { display: translate('class.fields.schedule'), accessor: 'schedule', type: 'text' },
    { display: translate('class.fields.room'), accessor: 'room', type: 'text' },
    { display: translate('class.fields.cancelDeadline'), accessor: 'cancelDeadline', type: 'date', required: true },
  ];

  const tableFields = [
    { display: translate('class.fields.classId'), accessor: 'classId', type: 'text' },
    { display: translate('class.fields.courseCode'), accessor: 'courseCode', type: 'select', options: courses },
    { display: translate('class.fields.academicYear'), accessor: 'academicYear', type: 'select', optionsEndpoint: 'schoolyears' },
    { display: translate('class.fields.semester'), accessor: 'semester', type: 'text' },
    { display: translate('class.fields.teacher'), accessor: 'teacher', type: 'text' },
    { display: translate('class.fields.maxStudents'), accessor: 'maxStudents', type: 'number' },
    { display: translate('class.fields.schedule'), accessor: 'schedule', type: 'text' },
    { display: translate('class.fields.room'), accessor: 'room', type: 'text' },
    { display: translate('class.fields.cancelDeadline'), accessor: 'cancelDeadline', type: 'date' },
  ];

  useEffect(() => {
    loadMetadata();
  }, [])

  const loadMetadata = async () => {
    try {
      const courseRes = await loadDataNoPaging('course');
      const course = courseRes.map((item) => ({
        id: item.courseCode,
        name: item.name
      }));
      tableFields[1].options = course;
      setCourses(course);

      const activeCourseRes = await loadDataNoPaging('course/active');
      const activeCourse = activeCourseRes.map((item) => ({
        id: item.courseCode,
        name: item.name
      }));
      setActiveCourses(activeCourse);


    } catch (error) {
      console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
    }

  };

  return (
    <PageLayout title={translate('class.title')}>
      <DataList formFields={formFields} tableFields={tableFields} dataName="class" pk="classId" label={translate('class.label')} />
    </PageLayout>
  );
};

export default ClassPage;
