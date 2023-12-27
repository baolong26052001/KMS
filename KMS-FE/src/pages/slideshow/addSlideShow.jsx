import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

const AddSlideShow = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [newSlide, setnewSlide] = useState({
    packageName: '',
    imagevideo: '',
    fileType: '',
    startDate: '',
    endDate: '',
  });


  const handleInputChange = (key, value) => {
    setnewSlide((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/User/AddUser`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newSlide),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/slideshow`);
        // Provide user feedback on successful save
        console.log('Added successfully');
      } else {
        console.log('Add failed');
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
                flexDirection: 'column', // Set the form to vertical layout
                gap: '16px', // Add some spacing between form elements
                width: 300, // Adjust the width as needed
                margin: 'auto', // Center the form
              }}
              noValidate
              autoComplete="off"
            >
              <TextField
                id="packageName"
                label="Package Name"
                variant="outlined"
                value={newSlide.packageName}
                onChange={(e) => handleInputChange('packageName', e.target.value)}
              />
              <TextField
                id="imagevideo"
                label="Image/Video"
                variant="outlined"
                value={newSlide.imagevideo}
                onChange={(e) => handleInputChange('imagevideo', e.target.value)}
              />
              <TextField
                id="fileType"
                label="File Type"
                variant="outlined"
                value={newSlide.fileType}
                onChange={(e) => handleInputChange('fileType', e.target.value)}
              />
              <TextField
                id="startDate"
                label="Start Date"
                variant="outlined"
                type="date"
                value={newSlide.startDate}
                onChange={(e) => handleInputChange('startDate', e.target.value)}
              />
              <TextField
                id="endDate"
                label="End Date"
                variant="outlined"
                type="date"
                value={newSlide.endDate}
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
