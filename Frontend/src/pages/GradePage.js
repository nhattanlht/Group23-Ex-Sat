import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';
import { Download } from 'lucide-react';

const CoursePage = () => {
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    const [mssv, setMssv] = useState('');

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
                id: item.mssv,
                name: item.mssv
            })));

        } catch (error) {
            console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
        }
    };

    const formFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes, required: true },
        { display: 'MSSV', accessor: 'mssv', type: 'select', options: students, required: true },
        { display: 'Điểm', accessor: 'score', type: 'number', required: true },
        { display: 'Điểm chữ', accessor: 'gradeLetter', type: 'text', required: false },
        { display: 'GPA', accessor: 'gpa', type: 'number', required: true },
    ];

    const tableFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes },
        { display: 'MSSV', accessor: 'mssv', type: 'select', options: students },
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
                        row[key] = item.student?.hoTen || 'N/A';
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

    const exportGrade = async (mssv) => {
        console.log('Exporting grade for:', mssv);
        try {
            const response = await axios.get(`${config.backendUrl}/api/grade/export/${mssv}`, {
                responseType: 'blob'
            });
            const blob = new Blob([response.data], { type: "text/csv" });
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = `BangDiem_${mssv}.csv`;
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
            onClick: (row) => exportGrade(row.__original.mssv),
            className: 'btn bg-green-600 text-white',
        }
    ];


    return (
        <PageLayout title="Danh sách Điểm">
            <div>
                <button className="btn btn-primary mb-2 disabled:opacity-50" disabled={!mssv} onClick={() => exportGrade(mssv)}>Xuất điểm</button>
                <input
                    type="text"
                    value={mssv}
                    onChange={(e) => setMssv(e.target.value)}
                    placeholder="Nhập MSSV để xuất bảng điểm"
                    className="form-control mb-2" />
            </div>
            <DataList formFields={formFields} tableFields={tableFields} dataName="grade" pk="gradeId" label="điểm" actions={actions} formatDataSet={formatDataSetForTable} />
        </PageLayout>
    );
};

export default CoursePage;
