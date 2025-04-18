import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';

const ClassPage = () => {
  const fields = [
    { display: 'Mã Lớp Học', accessor: 'classId', type: 'text', required: true },
    { display: 'Khóa Học', accessor: 'courseCode', type: 'select', optionsEndpoint: 'course', required: true },
    { display: 'Năm Học', accessor: 'academicYear', type: 'select', optionsEndpoint: 'schoolyears', required: true },
    { display: 'Học Kỳ', accessor: 'semester', type: 'text', required: true },
    { display: 'Giảng Viên', accessor: 'teacher', type: 'text', required: true },
    { display: 'Số Lượng Tối Đa', accessor: 'maxStudents', type: 'number', required: true },
    { display: 'Lịch Học', accessor: 'schedule', type: 'text' },
    { display: 'Phòng Học', accessor: 'room', type: 'text' },
    { display: 'Thời gian hủy đăng ký', accessor: 'cancelDeadline', type: 'date', required: true },
  ];

  return (
    <PageLayout title="Danh sách lớp học">
      <DataList fields={fields} dataName="class" pk="classId" label="Lớp Học" />
    </PageLayout>
  );
};

export default ClassPage;
