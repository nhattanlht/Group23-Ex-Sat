import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
const StatusPage = () => {
  const fields = [
    { display: 'Tình trạng', accessor: 'name', type: "text", required: true },
  ];
  
  return (
    <PageLayout title="Danh sách tình trạng sinh viên">
      <DataList fields={fields} dataName='student-statuses' pk='id' label='Tình trạng sinh viên' />
    </PageLayout>
  );
};

export default StatusPage;