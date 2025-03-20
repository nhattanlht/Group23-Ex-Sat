import React from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import StudentList from './components/StudentList'; // Import the StudentList component
import DataManagement from './components/DataManagement'; // Import the StudentList component
import './styles.css'; // Import the CSS file

function App() {
  return (
    <div className="App">
      <BrowserRouter>
      <Routes>
          <Route path="/" element={<StudentList />} />
          <Route path="/data" element={<DataManagement />} />
          <Route path="/department" element={<StudentList />} />
      </Routes>
    </BrowserRouter>
    </div>
  );
}

export default App;