import { useEffect, useState } from 'react';
import DataList from '../components/DataList';
import PageLayout from '../components/PageLayout';
import axios from 'axios';
import config from '../config';

const CoursePage = () => {
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    const [courses, setCourses] = useState([]);

    const formFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes, required: true },
        { display: 'Sinh Viên', accessor: 'studentId', type: 'select', options: students, required: true },
        { display: 'Thời gian đăng ký', accessor: 'registeredAt', type: 'date', required: false, disabled: true },
        { display: 'Hủy', accessor: 'isCancelled', type: 'checkbox', required: false },
        { display: 'Lí do hủy', accessor: 'cancelReason', type: 'text', required: false },
        { display: 'Thời gian hủy', accessor: 'cancelDate', type: 'date', required: false, disabled: true },
    ];

    const tableFields = [
        { display: 'Lớp Học', accessor: 'classId', type: 'select', options: classes },
        { display: 'Khóa Học', accessor: 'courseCode', type: 'text', options: courses },
        { display: 'Sinh Viên', accessor: 'studentId', type: 'select', options: students },
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
            const classRes = await axios.get(`${config.backendUrl}/api/class`);
            const studentRes = await axios.get(`${config.backendUrl}/api/students`);
            const courseRes = await axios.get(`${config.backendUrl}/api/course`);

            setClasses(classRes.data.map((item) => ({
                id: item.classId,
                name: item.classId
            })));

            setStudents(studentRes.data.students.map((item) => ({
                id: item.mssv,
                name: item.hoTen
            })));

            setCourses(courseRes.data.map((item) => ({
                id: item.courseCode,
                name: item.name
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
                        row[key] =
                            item.class?.courseCode || 'N/A';
                        break;

                    case 'studentId':
                        row[key] = item.student?.hoTen || 'N/A';
                        break;

                    case 'cancelDate':
                        row[key] = item.cancelDate ?
                            new Date(item.cancelDate).toLocaleDateString() : '';
                        break;

                    case 'registeredAt':
                        row[key] = item.registeredAt ?
                            new Date(item.registeredAt).toLocaleDateString() : 'N/A';
                        break;

                    case 'isCancelled':
                        row[key] = item.isCancelled ? 'Hủy' : 'Không';
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


    return (
        <PageLayout title="Danh sách đăng ký lớp học">
            <DataList formFields={formFields} tableFields={tableFields} dataName="enrollment" pk="enrollmentId" label="Đăng Ký Lớp Học" formatDataSet={formatDataSetForTable} />
        </PageLayout>
    );
};

export default CoursePage;
