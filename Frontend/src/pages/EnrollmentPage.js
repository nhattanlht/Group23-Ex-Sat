import { useEffect, useMemo, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';

const CoursePage = () => {
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    
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

    const fields = useMemo(() => [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes, required: true },
        { display: 'Sinh Viên', accessor: 'studentId', type: 'select', options: students, required: true },
    ], [classes, students]);

    if (classes.length === 0 || students.length === 0) {
        return <PageLayout title="Danh sách đăng ký lớp học"><p>Đang tải dữ liệu...</p></PageLayout>;
    }

    return (
        <PageLayout title="Danh sách đăng ký lớp học">
            <DataList fields={fields} dataName="enrollment" pk="enrollmentId" label="Đăng Ký Lớp Học" />
        </PageLayout>
    );
};

export default CoursePage;
