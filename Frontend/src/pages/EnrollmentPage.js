import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import { useLanguage } from '../contexts/LanguageContext';
import { loadDataNoPaging } from '../util/callCRUDApi';

const EnrollmentPage = () => {
    const { translate } = useLanguage();
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    const [courses, setCourses] = useState([]);

    const formFields = [
        { display: translate('enrollment.fields.classId'), accessor: 'classId', type: 'select', options: classes, required: true },
        { display: translate('enrollment.fields.StudentId'), accessor: 'studentId', type: 'select', options: students, required: true },
        { display: translate('enrollment.fields.registeredAt'), accessor: 'registeredAt', type: 'date', required: false, disabled: true },
        { display: translate('enrollment.fields.isCancelled'), accessor: 'isCancelled', type: 'checkbox', required: false },
        { display: translate('enrollment.fields.cancelReason'), accessor: 'cancelReason', type: 'text', required: false, condition: (formData) => formData.isCancelled },
        { display: translate('enrollment.fields.cancelDate'), accessor: 'cancelDate', type: 'date', required: false, disabled: true },
    ];

    const tableFields = [
        { display: translate('enrollment.fields.classId'), accessor: 'classId', type: 'select', options: classes },
        { display: translate('enrollment.fields.courseCode'), accessor: 'courseCode', type: 'text', options: courses },
        { display: translate('enrollment.fields.StudentId'), accessor: 'studentId', type: 'select', options: students },
        { display: translate('enrollment.fields.registeredAt'), accessor: 'registeredAt', type: 'date' },
        { display: translate('enrollment.fields.isCancelled'), accessor: 'isCancelled', type: 'checkbox' },
        { display: translate('enrollment.fields.cancelReason'), accessor: 'cancelReason', type: 'text' },
        { display: translate('enrollment.fields.cancelDate'), accessor: 'cancelDate', type: 'date' },
    ];

    useEffect(() => {
        loadMetadata();
    }, []);

    const loadMetadata = async () => {
        try {
            const [classRes, studentRes, courseRes] = await Promise.all([
                loadDataNoPaging('class'),
                loadDataNoPaging('students'),
                loadDataNoPaging('course')
            ]);

            setClasses(classRes.data.map((item) => ({
                id: item.classId,
                name: `${item.classId} - ${item.courseCode || 'N/A'}`
            })));

            setStudents(studentRes.data.students.map((item) => ({
                id: item.studentId,
                name: `${item.studentId} - ${item.fullName}`
            })));

            setCourses(courseRes.data.map((item) => ({
                id: item.courseCode,
                name: `${item.courseCode} - ${item.name}`
            })));

        } catch (error) {
            console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
        }
    };

    function formatDataSetForTable(dataArray, fields, helpers = {}) {
        return dataArray.map((item) => {
            const row = {};

            fields.forEach((field) => {
                const key = field.accessor;
                switch (field.accessor) {
                    case 'courseCode':
                        row[key] = item.class?.courseCode ? 
                            item.class.courseCode : 'N/A';
                        break;

                    case 'studentId':
                        row[key] = item.student ? 
                            `${item.student.studentId} - ${item.student.fullName}` : 'N/A';
                        break;

                    case 'cancelDate':
                        row[key] = item.cancelDate ?
                            new Date(item.cancelDate).toLocaleString('vi-VN') : '';
                        break;

                    case 'registeredAt':
                        row[key] = item.registeredAt ?
                            new Date(item.registeredAt).toLocaleString('vi-VN') : 'N/A';
                        break;

                    case 'isCancelled':
                        row[key] = item.isCancelled ? translate('common.yes') : translate('common.no');
                        break;

                    default:
                        row[key] = item[key];
                }
            });

            row.__original = item;
            return row;
        });
    }

    return (
        <PageLayout title={translate('enrollment.title')}>
            <DataList 
                formFields={formFields} 
                tableFields={tableFields} 
                dataName="enrollment" 
                pk="enrollmentId" 
                label={translate('enrollment.label')} 
                formatDataSet={formatDataSetForTable} 
            />
        </PageLayout>
    );
};

export default EnrollmentPage;
