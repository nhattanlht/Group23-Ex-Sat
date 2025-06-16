import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import { useLanguage } from '../contexts/LanguageContext';

const DepartmentPage = () => {
  const { translate } = useLanguage();
  const dataName = 'departments';
  
  const formFields = [
    { display: translate(`${dataName}.fields.name`), accessor: 'name', type: "text", required: true },
  ];
  
  return (
    <PageLayout title={translate(`${dataName}.title`)}>
      <DataList 
        formFields={formFields} 
        dataName={dataName}
        pk='id' 
        label={translate(`${dataName}.label`)} 
      />
    </PageLayout>
  );
};

export default DepartmentPage;