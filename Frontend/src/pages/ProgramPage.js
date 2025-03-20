import DataList from '../components/DataList';
const ProgramPage = () => {
  const fields = [
    { display: 'Tên chương trình', accessor: 'name', type: "text", required: true },
  ];
  
  return (
      <DataList fields={fields} dataName='programs' pk='id' label='Chương trình đào tạo'/>
  );
};

export default ProgramPage;