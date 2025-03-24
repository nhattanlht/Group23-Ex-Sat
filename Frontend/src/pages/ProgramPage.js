import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
const ProgramPage = () => {
  const fields = [
    { display: 'Tên chương trình', accessor: 'name', type: "text", required: true },
  ];

  return (
    <PageLayout title="Danh sách chương trình">
      <DataList fields={fields} dataName='programs' pk='id' label='Chương trình đào tạo' />
    </PageLayout>
  );
};

export default ProgramPage;