import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';

const EditStation = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [editedGroup, setEditedGroup] = useState({
    stationName: '',
    companyName: '',
    city: '',
    address: '',
    isActive: ''
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/Station/ShowStation/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          // Populate the state with fetched group details
          setEditedGroup({
            stationName: groupData[0].stationName,
            companyName: groupData[0].companyName, 
            city: groupData[0].city, 
            address: groupData[0].address, 
            isActive: groupData[0].isActive,
          });
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };

    fetchGroupDetails();
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    setEditedGroup((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/Station/UpdateStation/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedGroup),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/station`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/station`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Station</h1>
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
                id="stationName"
                label="Station Name"
                variant="outlined"
                value={editedGroup.stationName}
                onChange={(e) => handleInputChange('stationName', e.target.value)}
              />
              <TextField
                id="companyName"
                label="Company Name"
                variant="outlined"
                value={editedGroup.companyName}
                onChange={(e) => handleInputChange('companyName', e.target.value)}
              />
            <TextField
                id="city"
                label="City"
                variant="outlined"
                value={editedGroup.city}
                onChange={(e) => handleInputChange('city', e.target.value)}
              />
            <TextField
                id="address"
                label="Address"
                variant="outlined"
                value={editedGroup.address}
                onChange={(e) => handleInputChange('address', e.target.value)}
              />

            <TextField
                id="isActive"
                label="Is Active"
                variant="outlined"
                select
                value={editedGroup.isActive}
                onChange={(e) => handleInputChange('isActive', e.target.value)}
                >
                <MenuItem value={true}>True</MenuItem>
                <MenuItem value={false}>False</MenuItem>
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

export default EditStation;
