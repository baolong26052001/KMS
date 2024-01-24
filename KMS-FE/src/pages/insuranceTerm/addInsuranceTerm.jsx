import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

const AddInsuranceTerm = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";

  const [newItem, setnewItem] = useState({
    content: '',
  });

  const handleInputChange = (key, value) => {
    setnewItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/Term/AddTerm`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newItem),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/insuranceTerm`);
        console.log('added successfully');
      } else {
        console.log('add failed');
      }
    } catch (error) {
      console.error('Error adding:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceTerm`);
  };
  
  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Insurance Term</h1>
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
              id="content"
              label="Insurance Term"
              variant="outlined"
              value={newItem.content}
              onChange={(e) => handleInputChange('content', e.target.value)}
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

export default AddInsuranceTerm;