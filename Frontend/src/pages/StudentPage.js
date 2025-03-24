import StudentList from '../components/StudentList';
import PageLayout from '../components/PageLayout';
const StudentPage = () => {
  
  return (
    <PageLayout title="Danh sách sinh viên">
      <StudentList />
    </PageLayout>
  );
};

export default StudentPage;