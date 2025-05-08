import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
const ProgramPage = () => {
  const formFields = [
    { display: 'Tên chương trình', accessor: 'name', type: "text", required: true },
  ];

  return (
    <PageLayout title="Danh sách chương trình">
      <DataList formFields={formFields} dataName='programs' pk='id' label='Chương trình đào tạo' />
    </PageLayout>
  );
};

export default ProgramPage;