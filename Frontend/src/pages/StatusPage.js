import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import { useLanguage } from '../contexts/LanguageContext';

const StatusPage = () => {
  const { translate } = useLanguage();
  
  const formFields = [
    { display: translate('status.fields.name'), accessor: 'name', type: "text", required: true },
  ];
  
  return (
    <PageLayout title={translate('status.title')}>
      <DataList 
        formFields={formFields} 
        dataName='student-statuses' 
        pk='id' 
        label={translate('status.label')} 
      />
    </PageLayout>
  );
};

export default StatusPage;