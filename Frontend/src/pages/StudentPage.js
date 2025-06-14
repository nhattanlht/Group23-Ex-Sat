import React from 'react';
import StudentList from '../components/StudentList';
import PageLayout from '../components/PageLayout';
import { useLanguage } from '../contexts/LanguageContext';

const StudentPage = () => {
  const { translate } = useLanguage();
  
  return (
    <PageLayout title={translate('student.title')}>
      <StudentList />
    </PageLayout>
  );
};

export default StudentPage;