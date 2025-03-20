import DataList from '../components/DataList';
const StatusPage = () => {
  const fields = [
    { display: 'Tình trạng', accessor: 'name', type: "text", required: true },
  ];
  
  return (
      <DataList fields={fields} dataName='student-statuses' pk='id' label='Tình trạng sinh viên'/>
  );
};

export default StatusPage;