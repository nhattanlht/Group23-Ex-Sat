import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
const DepartmentPage = () => {
  const fields = [
    { display: 'Tên khoa', accessor: 'name', type: "text", required: true },
  ];
  
  return (
    <PageLayout title="Danh sách khoa">
      <DataList fields={fields} dataName='departments' pk='id' label='Khoa' />
    </PageLayout>
  );
};

export default DepartmentPage;