import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

const AddSlideDetail = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  // State to store user information
  const [newItem, setnewItem] = useState({
    description: '',
    typeContent:'',
    contentUrl:'',
    slideHeaderId: '',
    isActive: true,
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
        //navigate(`/slideDetail/${rowId}/${packageName}`);
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
    //navigate(`/slideDetail/${rowId}/${packageName}`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Slide Detail</h1>
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
                width: 300, 
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
                id="typeContent"
                label="typeContent"
                variant="outlined"
                value={newItem.typeContent}
                onChange={(e) => handleInputChange('typeContent', e.target.value)}
              />
              <TextField
                id="contentUrl"
                label="Content Url"
                variant="outlined"
                value={newItem.contentUrl}
                onChange={(e) => handleInputChange('contentUrl', e.target.value)}
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

export default AddSlideDetail;