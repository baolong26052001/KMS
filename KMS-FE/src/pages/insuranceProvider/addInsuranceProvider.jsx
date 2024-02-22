import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

const AddInsuranceProvider = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i].trim();
      if (cookie.startsWith(name + '=')) {
        return cookie.substring(name.length + 1);
      }
    }
    return null;
  }

  const userIdCookie = getCookie('userId');

  const [newInsuranceProvider, setnewInsuranceProvider] = useState({
    provider: '',
    email: '',
    userId: userIdCookie,
  });

  const handleInputChange = (key, value) => {
    setnewInsuranceProvider((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/InsuranceProvider/AddInsuranceProvider`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newInsuranceProvider),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/insuranceProvider`);
        console.log('added successfully');
      } else {
        console.log('add failed');
      }
    } catch (error) {
      console.error('Error adding:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceProvider`);
  };
  
  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Insurance Provider</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          <div className="table-container">
            <Box
              component="form"
              sx={{
                display: 'flex',
                flexDirection: 'column', 
                gap: '20px', // Add some spacing between form elements
                //width: 300, 
                margin: 'auto', 
              }}
              noValidate
              autoComplete="off"
            >
              <TextField
              id="provider"
              label="Insurance Provider"
              variant="outlined"
              value={newInsuranceProvider.provider}
              onChange={(e) => handleInputChange('provider', e.target.value)}
              >
              </TextField>
              <TextField
              id="email"
              label="Insurance Email"
              variant="outlined"
              value={newInsuranceProvider.email}
              onChange={(e) => handleInputChange('email', e.target.value)}
              >
              </TextField>
              <Box sx={{ display: 'flex', gap: '8px' }}>
                <Button variant="contained" fullWidth onClick={handleSave}>
                  Save
                </Button>
                <Button variant="contained" fullWidth onClick={handleCancel} style={{ backgroundColor: '#848485', color: '#fff' }}>
                  Cancel
                </Button>
              </Box>
            </Box>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddInsuranceProvider;