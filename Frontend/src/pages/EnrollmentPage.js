import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';

const EnrollmentPage = () => {
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    const [courses, setCourses] = useState([]);

    const formFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes, required: true },
        { display: 'Sinh Viên', accessor: 'StudentId', type: 'select', options: students, required: true },
        { display: 'Thời gian đăng ký', accessor: 'registeredAt', type: 'date', required: false, disabled: true },
        { display: 'Hủy', accessor: 'isCancelled', type: 'checkbox', required: false },
        { display: 'Lí do hủy', accessor: 'cancelReason', type: 'text', required: false, condition: (formData) => formData.isCancelled },
        { display: 'Thời gian hủy', accessor: 'cancelDate', type: 'date', required: false, disabled: true },
    ];

    const tableFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes },
        { display: 'Khóa Học', accessor: 'courseCode', type: 'text', options: courses },
        { display: 'Sinh Viên', accessor: 'StudentId', type: 'select', options: students },
        { display: 'Thời gian đăng ký', accessor: 'registeredAt', type: 'date' },
        { display: 'Hủy', accessor: 'isCancelled', type: 'checkbox' },
        { display: 'Lí do hủy', accessor: 'cancelReason', type: 'text' },
        { display: 'Thời gian hủy', accessor: 'cancelDate', type: 'date' },
    ];

    useEffect(() => {
        loadMetadata();
    }, []);

    const loadMetadata = async () => {
        try {
            const [classRes, studentRes, courseRes] = await Promise.all([
                axios.get(`${config.backendUrl}/api/class`),
                axios.get(`${config.backendUrl}/api/students`),
                axios.get(`${config.backendUrl}/api/course`)
            ]);

            setClasses(classRes.data.map((item) => ({
                id: item.classId,
                name: `${item.classId} - ${item.courseCode || 'N/A'}`
            })));

            setStudents(studentRes.data.students.map((item) => ({
                id: item.StudentId,
                name: `${item.StudentId} - ${item.FullName}`
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
                            `${item.class.courseCode} - ${item.class.courseName || 'N/A'}` : 'N/A';
                        break;

                    case 'StudentId':
                        row[key] = item.student ? 
                            `${item.student.StudentId} - ${item.student.FullName}` : 'N/A';
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
                        row[key] = item.isCancelled ? 'Đã hủy' : 'Đang học';
                        break;

                    default:
                        row[key] = item[key];
                }
            });

            row.__original = item;
            return row;
        });
    }

    if (classes.length === 0 || students.length === 0 || courses.length === 0) {
        return <PageLayout title="Danh sách đăng ký lớp học"><p>Đang tải dữ liệu...</p></PageLayout>;
    }

    return (
        <PageLayout title="Danh sách đăng ký lớp học">
            <DataList 
                formFields={formFields} 
                tableFields={tableFields} 
                dataName="enrollment" 
                pk="enrollmentId" 
                label="Đăng Ký Lớp Học" 
                formatDataSet={formatDataSetForTable} 
            />
        </PageLayout>
    );
};

export default EnrollmentPage;
