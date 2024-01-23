import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

const AddAgeRange = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";

  const [newItem, setnewItem] = useState({
    startAge: '',
    endAge: ''
  });

  const handleInputChange = (key, value) => {
    setnewItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/AgeRange/AddAgeRange`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newItem),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/insuranceAgeRange`);
        console.log('added successfully');
      } else {
        console.log('add failed');
      }
    } catch (error) {
      console.error('Error adding:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceAgeRange`);
  };
  
  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Insurance Age Range</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          <div className="table-container">
          <Box
            component="form"
            sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: '16px',
                width: 400,
                margin: 'auto',
            }}
            noValidate
            autoComplete="on"
            >
                <Box sx={{ display: 'flex', gap: '16px' }}>
                    <TextField
                    id="startAge"
                    label="Min Age"
                    variant="outlined"
                    type="number"
                    value={newItem.startAge}
                    onChange={(e) => handleInputChange('startAge', e.target.value)}
                    />

                    <TextField
                    id="endAge"
                    label="Max Age"
                    variant="outlined"
                    type="number"
                    value={newItem.endAge}
                    onChange={(e) => handleInputChange('endAge', e.target.value)}
                    />
                </Box>

                <Box sx={{ display: 'flex', gap: '8px', width: 400, marginTop: 1 }}>
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

export default AddAgeRange;