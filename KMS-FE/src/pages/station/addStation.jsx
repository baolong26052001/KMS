import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import { API_URL } from '../../components/config/apiUrl';

const AddStation = () => {
  const navigate = useNavigate();
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
  // State to store user information
  const [newStation, setnewStation] = useState({
    stationName: '',
    companyName: '',
    city: '',
    address: '',
    isActive: true,
    userId: userIdCookie,
  });

  const handleInputChange = (key, value) => {
    setnewStation((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/Station/AddStation`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newStation),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/station`);
        // Provide user feedback on successful save
        console.log('added successfully');
      } else {
        console.log('add failed');
      }
    } catch (error) {
      console.error('Error adding:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/station`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Station</h1>
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
                id="stationName"
                label="Station Name"
                variant="outlined"
                value={newStation.stationName}
                onChange={(e) => handleInputChange('stationName', e.target.value)}
              />
              <TextField
                id="companyName"
                label="Company Name"
                variant="outlined"
                value={newStation.companyName}
                onChange={(e) => handleInputChange('companyName', e.target.value)}
              />
              <TextField
                id="city"
                label="City"
                variant="outlined"
                value={newStation.city}
                onChange={(e) => handleInputChange('city', e.target.value)}
              />
              <TextField
                id="address"
                label="Address"
                variant="outlined"
                value={newStation.address}
                onChange={(e) => handleInputChange('address', e.target.value)}
              />
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

export default AddStation;