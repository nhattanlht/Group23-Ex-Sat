import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import { useLanguage } from '../contexts/LanguageContext';

const ProgramPage = () => {
  const { translate } = useLanguage();
  
  const formFields = [
    { display: translate('program.fields.name'), accessor: 'name', type: "text", required: true },
  ];

  return (
    <PageLayout title={translate('program.title')}>
      <DataList 
        formFields={formFields} 
        dataName='programs' 
        pk='id' 
        label={translate('program.label')} 
      />
    </PageLayout>
  );
};

export default ProgramPage;