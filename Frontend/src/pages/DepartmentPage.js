import DataList from '../components/DataList';
const DepartmentPage = () => {
  const fields = [
    { display: 'Tên khoa', accessor: 'name', type: "text", required: true },
  ];
  
  return (
      <DataList fields={fields} dataName='departments' pk='id' label='Khoa'/>
  );
};

export default DepartmentPage;