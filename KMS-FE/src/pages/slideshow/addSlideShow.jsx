import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import InputAdornment from '@mui/material/InputAdornment';

const AddSlideShow = () => {
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
  // State to store user information
  const [newItem, setnewItem] = useState({
    description: '',
    startDate: new Date().toISOString().split('T')[0],
    endDate: new Date().toISOString().split('T')[0],
    timeNext: '',
    isActive: true,
    userId: userIdCookie,
  });

  const handleInputChange = (key, value) => {
    setnewItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/SlideHeader/AddSlideshow`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newItem),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/slideshow`);
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
    navigate(`/slideshow`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Slide Show</h1>
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
                id="description"
                label="Slide Description"
                variant="outlined"
                value={newItem.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
              />
              <TextField
                id="timeNext"
                label="Time Next"
                variant="outlined"
                value={newItem.timeNext}
                InputProps={{
                  endAdornment: <InputAdornment position="end">Seconds</InputAdornment>,
                }}
                onChange={(e) => handleInputChange('timeNext', e.target.value)}
              />
              <TextField
                id="startDate"
                label="Start Date"
                variant="outlined"
                type="date"
                value={newItem.startDate}
                onChange={(e) => handleInputChange('startDate', e.target.value)}
              />
              <TextField
                id="endDate"
                label="End Date"
                variant="outlined"
                type="date"
                value={newItem.endDate}
                onChange={(e) => handleInputChange('endDate', e.target.value)}
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

export default AddSlideShow;