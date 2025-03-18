import React from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import StudentPage from './pages/StudentPage'; // Import the StudentPage component
import './styles.css'; // Import the CSS file

function App() {
  return (
    <div className="App">
      <BrowserRouter>
      <Routes>
          <Route path="/" element={<StudentPage />} />
          <Route path="/department" element={<StudentPage />} />
      </Routes>
    </BrowserRouter>
    </div>
  );
}

export default App;