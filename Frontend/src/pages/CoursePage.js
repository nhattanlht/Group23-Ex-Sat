import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';

const CoursePage = () => {
  const fields = [
    { display: 'Mã Khóa Học', accessor: 'courseCode', type: 'text', required: true },
    { display: 'Tên Khóa Học', accessor: 'name', type: 'text', required: true },
    { display: 'Số Tín Chỉ', accessor: 'credits', type: 'number', required: true },
    { display: 'Khoa', accessor: 'departmentId', type: 'select', optionsEndpoint: 'departments', required: true },
    { display: 'Mô Tả', accessor: 'description', type: 'textarea' },
    { display: 'Môn Tiên Quyết', accessor: 'prerequisiteCourseCode', type: 'select', optionsEndpoint: 'courses' },
    { display: 'Hoạt Động', accessor: 'isActive', type: 'checkbox' },
  ];

  return (
    <PageLayout title="Danh sách khóa học">
      <DataList fields={fields} dataName="courses" pk="courseCode" label="Khóa Học" />
    </PageLayout>
  );
};

export default CoursePage;
