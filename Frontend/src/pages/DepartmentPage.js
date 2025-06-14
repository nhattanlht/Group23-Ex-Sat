import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import { useLanguage } from '../contexts/LanguageContext';

const DepartmentPage = () => {
  const { translate } = useLanguage();
  
  const formFields = [
    { display: translate('department.fields.name'), accessor: 'name', type: "text", required: true },
  ];
  
  return (
    <PageLayout title={translate('department.title')}>
      <DataList 
        formFields={formFields} 
        dataName='departments' 
        pk='id' 
        label={translate('department.label')} 
      />
    </PageLayout>
  );
};

export default DepartmentPage;