import React from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import DataManagement from './components/DataManagement'; // Import the StudentList component
import './styles.css'; // Import the CSS file
import DepartmentPage from './pages/DepartmentPage';
import ProgramPage from './pages/ProgramPage';
import StatusPage from './pages/StatusPage';
import StudentPage from './pages/StudentPage';

function App() {
  return (
    <div className="App">
      <div>
      <BrowserRouter>
      <Routes>
          <Route path="/" element={<StudentPage />} />
          <Route path="/students" element={<StudentPage />} />
          <Route path="/departments" element={<DepartmentPage />} />
          <Route path="/programs" element={<ProgramPage />} />
          <Route path="/statuses" element={<StatusPage />} />
          <Route path="/data" element={<DataManagement />} />
      </Routes>
        </BrowserRouter>
        </div>
    </div>
  );
}

export default App;