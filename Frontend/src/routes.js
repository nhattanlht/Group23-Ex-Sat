import React from 'react';
import { Routes, Route } from "react-router-dom";
import DepartmentPage from './pages/DepartmentPage';
import ProgramPage from './pages/ProgramPage';
import StatusPage from './pages/StatusPage';
import StudentPage from './pages/StudentPage';
import CoursePage from './pages/CoursePage';
import ClassPage from './pages/ClassPage';
import EnrollmentPage from './pages/EnrollmentPage';
import GradePage from './pages/GradePage';
import DataManagement from './pages/DataManagement';

const AppRoutes = () => {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<StudentPage />} />
        <Route path="/students" element={<StudentPage />} />
        <Route path="/departments" element={<DepartmentPage />} />
        <Route path="/programs" element={<ProgramPage />} />
        <Route path="/statuses" element={<StatusPage />} />
        <Route path="/data" element={<DataManagement />} />
        <Route path="/courses" element={<CoursePage />} />
        <Route path="/classes" element={<ClassPage />} />
        <Route path="/enrollment" element={<EnrollmentPage />} />
        <Route path="/grade" element={<GradePage />} />
      </Routes>
    </div>
  );
};

export default AppRoutes; 