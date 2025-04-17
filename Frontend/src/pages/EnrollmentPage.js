import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';

const CoursePage = () => {
  const fields = [
    { display: 'Lớp Học', accessor: 'classId', type: 'select', optionsEndpoint: 'class', required: true },
    { display: 'Sinh Viên', accessor: 'studentId', type: 'select', optionsEndpoint: 'students', required: true },
  ];

  return (
    <PageLayout title="Danh sách đăng ký lớp học">
      <DataList fields={fields} dataName="enrollment" pk="enrollmentId" label="Đăng Ký Lớp Học" />
    </PageLayout>
  );
};

export default CoursePage;
