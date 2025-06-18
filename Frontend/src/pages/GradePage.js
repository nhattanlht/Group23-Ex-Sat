import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';
import { Download } from 'lucide-react';
import { useLanguage } from '../contexts/LanguageContext';

const CoursePage = () => {
    const { translate } = useLanguage();
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    const [StudentId, setStudentId] = useState('');

    useEffect(() => {
        loadMetadata();
    }, []);

    const loadMetadata = async () => {
        try {
            const classRes = await axios.get(`${config.backendUrl}/api/class`);
            const studentRes = await axios.get(`${config.backendUrl}/api/students`);

            setClasses(classRes.data.data.map((item) => ({
                id: item.classId,
                name: item.classId
            })));

            setStudents(studentRes.data.data.students.map((item) => ({
                id: item.studentId,
                name: item.studentId
            })));

        } catch (error) {
            console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
        }
    };
    
    const dataName = 'grade';
    const formFields = [
        { display: translate(`${dataName}.fields.classId`), accessor: 'classId', type: 'select', options: classes, required: true },
        { display: translate(`${dataName}.fields.studentId`), accessor: 'studentId', type: 'select', options: students, required: true },
        { display: translate(`${dataName}.fields.score`), accessor: 'score', type: 'number', required: true },
        { display: translate(`${dataName}.fields.gradeLetter`), accessor: 'gradeLetter', type: 'text', required: false },
        { display: translate(`${dataName}.fields.gpa`), accessor: 'gpa', type: 'number', required: true },
    ];

    const tableFields = [
        { display: translate(`${dataName}.fields.classId`), accessor: 'classId', type: 'select', options: classes },
        { display: translate(`${dataName}.fields.studentId`), accessor: 'studentId', type: 'select', options: students },
        { display: translate(`${dataName}.fields.student`), accessor: 'student', type: 'select', options: students },
        { display: translate(`${dataName}.fields.score`), accessor: 'score', type: 'number' },
        { display: translate(`${dataName}.fields.gradeLetter`), accessor: 'gradeLetter', type: 'text' },
        { display: translate(`${dataName}.fields.gpa`), accessor: 'gpa', type: 'number' },
    ];


    function formatDataSetForTable(dataArray, fields, helpers = {}) {
        return dataArray.map((item) => {
            const row = {};

            fields.forEach((field) => {
                const key = field.accessor;
                switch (field.accessor) {

                    case 'student':
                        row[key] = item.student?.fullName || 'N/A';
                        break;

                    // Add more custom fields here

                    default:
                        row[key] = item[key];
                }
            });

            row.__original = item;


            return row;
        });
    }

    const exportGrade = async (studentId) => {
        console.log('Exporting grade for:', studentId);
        try {
            const response = await axios.get(`${config.backendUrl}/api/grade/export/${studentId}`, {
                responseType: 'blob'
            });
            const blob = new Blob([response.data], { type: "text/csv" });
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = `${translate(`${dataName}.export.file_name`)}_${studentId}.csv`;
            a.click();

            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error("Error exporting transcript:", error);
            alert(translate(`${dataName}.export.error`), error.message);
        }
    }

    const actions = [
        {
            label: translate(`${dataName}.export.button`),
            icon: <Download size={16} />,
            onClick: (row) => exportGrade(row.__original.studentId),
            className: 'btn bg-green-600 text-white',
        }
    ];


    return (
        <PageLayout title={translate(`${dataName}.title`)}>
            <div>
                <button className="btn btn-primary mb-2 disabled:opacity-50" disabled={!StudentId} onClick={() => exportGrade(StudentId)}>{translate(`${dataName}.export.button`)}</button>
                <input
                    type="text"
                    value={StudentId}
                    onChange={(e) => setStudentId(e.target.value)}
                    placeholder={translate(`${dataName}.export.guide`)}
                    className="form-control mb-2" />
            </div>
            <DataList formFields={formFields} tableFields={tableFields} dataName={dataName} pk="gradeId" label={translate(`${dataName}.label`)} actions={actions} formatDataSet={formatDataSetForTable} />
        </PageLayout>
    );
};

export default CoursePage;
