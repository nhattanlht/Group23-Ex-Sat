import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';

const ClassPage = () => {
  const fields = [
    { display: 'Mã Lớp Học', accessor: 'classCode', type: 'text', required: true },
    { display: 'Khóa Học', accessor: 'courseCode', type: 'select', optionsEndpoint: 'courses', required: true },
    { display: 'Năm Học', accessor: 'schoolYearId', type: 'select', optionsEndpoint: 'schoolyears', required: true },
    { display: 'Học Kỳ', accessor: 'semester', type: 'text', required: true },
    { display: 'Giảng Viên', accessor: 'lecturer', type: 'text', required: true },
    { display: 'Số Lượng Tối Đa', accessor: 'maxStudents', type: 'number', required: true },
    { display: 'Lịch Học', accessor: 'schedule', type: 'text' },
    { display: 'Phòng Học', accessor: 'room', type: 'text' },
  ];

  return (
    <PageLayout title="Danh sách lớp học">
      <DataList fields={fields} dataName="classes" pk="classCode" label="Lớp Học" />
    </PageLayout>
  );
};

export default ClassPage;
