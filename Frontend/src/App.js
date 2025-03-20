import React from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import StudentList from './components/StudentList'; // Import the StudentList component
import './styles.css'; // Import the CSS file
import DepartmentPage from './pages/DepartmentPage';
import ProgramPage from './pages/ProgramPage';
import StatusPage from './pages/StatusPage';
import Sidebar from './components/Sidebar';

function App() {
  return (
    <div className="App">
      <Sidebar />
      <div className='mx-2'>
      <BrowserRouter>
      <Routes>
          <Route path="/" element={<StudentList />} />
          <Route path="/students" element={<StudentList />} />
          <Route path="/departments" element={<DepartmentPage />} />
          <Route path="/programs" element={<ProgramPage />} />
          <Route path="/statuses" element={<StatusPage />} />
      </Routes>
        </BrowserRouter>
        </div>
    </div>
  );
}

export default App;