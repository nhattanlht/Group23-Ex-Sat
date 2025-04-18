import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';

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
                name: item.course.name
            })));

            setStudents(studentRes.data.students.map((item) => ({
                id: item.mssv,
                name: item.hoTen
            })));

            console.log(classes, students);

        } catch (error) {
            console.error('Lỗi khi tải dữ liệu phụ trợ:', error);
        }
    };

    const fields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes, required: true },
        { display: 'Sinh viên', accessor: 'mssv', type: 'select', options: students, required: true },
        { display: 'Điểm', accessor: 'score', type: 'number', required: true },
        { display: 'Điểm chữ', accessor: 'gradeLetter', type: 'text', required: false },
        { display: 'GPA', accessor: 'gpa', type: 'number', required: true },
    ];

    if (classes.length === 0 || students.length === 0) {
        return <PageLayout title="Danh sách Điểm"><p>Đang tải dữ liệu...</p></PageLayout>;
    }

    const exportGrade = async (mssv) => {
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

    return (
        <PageLayout title="Danh sách Điểm">
            <div>
                <button className="btn btn-primary mb-2" onClick={() => exportGrade(mssv)}>Xuất điểm</button>
                <input
                    type="text"
                    value={mssv}
                    onChange={(e) => setMssv(e.target.value)}
                    placeholder="Nhập MSSV"
                    className="form-control mb-2" />
            </div>
            <DataList fields={fields} dataName="grade" pk="gradeId" label="điểm" />
        </PageLayout>
    );
};

export default CoursePage;
