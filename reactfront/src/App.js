import React, { useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './Components/HomePage';
import LoginPage from './Components/LoginPage';
import RegisterPage from './Components/RegisterPage';
import Address from './Components/Address';
import Navbar from './Components/Navbar';
import { useToken } from './TokenContext';
import AddAddress from './Components/AddAddress';
import ApproveAddresses from './Components/ApproveAddresses';
import EditAddress from './Components/EditAddress';

function App() {
  const { token, setToken } = useToken();

  useEffect(() => {
    setToken(localStorage.getItem("jwt"));
  },[])

    return (
      <Router>
        <div className="App">
          <Navbar />
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/addresses" element={<Address />} />
            <Route path="/address/add" element={<AddAddress />} />
            <Route path="/address/approve" element={<ApproveAddresses />} />
            <Route path="/address/edit/:id" element={<EditAddress />} />
          </Routes>
        </div>
      </Router>
    );
}

export default App;