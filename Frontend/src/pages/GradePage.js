import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';
import { Download } from 'lucide-react';

const CoursePage = () => {
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

            setClasses(classRes.data.map((item) => ({
                id: item.classId,
                name: item.classId
            })));

            setStudents(studentRes.data.students.map((item) => ({
                id: item.StudentId,
                name: item.StudentId
            })));

        } catch (error) {
            console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
        }
    };

    const formFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes, required: true },
        { display: 'StudentId', accessor: 'StudentId', type: 'select', options: students, required: true },
        { display: 'Điểm', accessor: 'score', type: 'number', required: true },
        { display: 'Điểm chữ', accessor: 'gradeLetter', type: 'text', required: false },
        { display: 'GPA', accessor: 'gpa', type: 'number', required: true },
    ];

    const tableFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes },
        { display: 'StudentId', accessor: 'StudentId', type: 'select', options: students },
        { display: 'Họ tên Sinh viên', accessor: 'student', type: 'select', options: students },
        { display: 'Điểm', accessor: 'score', type: 'number' },
        { display: 'Điểm chữ', accessor: 'gradeLetter', type: 'text' },
        { display: 'GPA', accessor: 'gpa', type: 'number' },
    ];

    function formatDataSetForTable(dataArray, fields, helpers = {}) {
        return dataArray.map((item) => {
            const row = {};

            fields.forEach((field) => {
                const key = field.accessor;
                switch (field.accessor) {

                    case 'student':
                        row[key] = item.student?.FullName || 'N/A';
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

    const exportGrade = async (StudentId) => {
        console.log('Exporting grade for:', StudentId);
        try {
            const response = await axios.get(`${config.backendUrl}/api/grade/export/${StudentId}`, {
                responseType: 'blob'
            });
            const blob = new Blob([response.data], { type: "text/csv" });
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = `BangDiem_${StudentId}.csv`;
            a.click();

            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error("Error exporting transcript:", error);
            alert("Lỗi khi xuất bảng điểm.");
        }
    }

    const actions = [
        {
            label: 'Chi tiết',
            icon: <Download size={16} />,
            onClick: (row) => exportGrade(row.__original.StudentId),
            className: 'btn bg-green-600 text-white',
        }
    ];


    return (
        <PageLayout title="Danh sách Điểm">
            <div>
                <button className="btn btn-primary mb-2 disabled:opacity-50" disabled={!StudentId} onClick={() => exportGrade(StudentId)}>Xuất điểm</button>
                <input
                    type="text"
                    value={StudentId}
                    onChange={(e) => setStudentId(e.target.value)}
                    placeholder="Nhập StudentId để xuất bảng điểm"
                    className="form-control mb-2" />
            </div>
            <DataList formFields={formFields} tableFields={tableFields} dataName="grade" pk="gradeId" label="điểm" actions={actions} formatDataSet={formatDataSetForTable} />
        </PageLayout>
    );
};

export default CoursePage;
