import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';
import InputAdornment from '@mui/material/InputAdornment';
import { API_URL } from '../../components/config/apiUrl';

const EditSlideShow = () => {
  const navigate = useNavigate();
  const { id } = useParams();
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
  const [editedItem, seteditedItem] = useState({
    description: '',
    startDate: '',
    endDate: '',
    timeNext: '',
    IsActive: '',
    userId: '',
  });

  useEffect(() => {
    const fetchDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/SlideHeader/ShowSlideHeader/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          // Populate the state with fetched group details
          seteditedItem({
            description: groupData[0].description,
            startDate: groupData[0].startDate.substring(0, 10), 
            endDate: groupData[0].endDate.substring(0, 10), 
            timeNext: groupData[0].timeNext, 
            IsActive: groupData[0].IsActive,
            userId: userIdCookie,
          });
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };

    fetchDetails();
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    seteditedItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/SlideHeader/UpdateSlideshow/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedItem),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/slideshow`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/slideshow`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Slide Show</h1>
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
              autoComplete="on"
            >
              <TextField
                id="description"
                label="Slide Show Description"
                variant="outlined"
                value={editedItem.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
              />
                <TextField
                id="startDate"
                label="Start Date"
                type="date"
                variant="outlined"
                value={editedItem.startDate}
                onChange={(e) => handleInputChange('startDate', e.target.value)}
                />

                <TextField
                id="endDate"
                label="End Date"
                type="date"
                variant="outlined"
                value={editedItem.endDate}
                onChange={(e) => handleInputChange('endDate', e.target.value)}
                />
              <TextField
                  id="timeNext"
                  label="Duration"
                  variant="outlined"
                  value={editedItem.timeNext}
                  onChange={(e) => handleInputChange('timeNext', e.target.value)}
                  InputProps={{
                      endAdornment: <InputAdornment position="end">Seconds</InputAdornment>,
                  }}
              />


            <TextField
                id="IsActive"
                label="Is Active"
                variant="outlined"
                select
                value={editedItem.IsActive}
                onChange={(e) => handleInputChange('IsActive', e.target.value)}
                >
                <MenuItem value={true}>Enable</MenuItem>
                <MenuItem value={false}>Disable</MenuItem>
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

export default EditSlideShow;
