import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
const DepartmentPage = () => {
  const formFields = [
    { display: 'Tên khoa', accessor: 'name', type: "text", required: true },
  ];
  
  return (
    <PageLayout title="Danh sách khoa">
      <DataList formFields={formFields} dataName='departments' pk='id' label='Khoa' />
    </PageLayout>
  );
};

export default DepartmentPage;