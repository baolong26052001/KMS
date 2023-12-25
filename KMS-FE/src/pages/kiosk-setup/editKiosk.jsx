import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';

const EditKiosk = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [editedKiosk, seteditedKiosk] = useState({
    kioskName: '',
    location: '',
    stationCode: '',
    slidePackage: '',
    webServices: ''
  });

  const [stations, setStations] = useState([]);
  const [slidePackage, setslidePackage] = useState([]);

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/Kiosk/ShowKioskSetup/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          // Populate the state with fetched group details
          seteditedKiosk({
            kioskName: groupData[0].kioskName,
            location: groupData[0].location, 
            stationCode: String(groupData[0].stationCode), 
            slidePackage: String(groupData[0].slidePackage), 
            webServices: groupData[0].webServices,
          });
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };
    const fetchStation = async () => {
        try {
          const response = await fetch(`${API_URL}api/Station/ShowStation`);
          if (response.ok) {
            const data = await response.json();
            setStations(data);
          } else {
            console.log('Failed to fetch user groups');
          }
        } catch (error) {
          console.error('Error fetching user groups:', error);
        }
      };
  
      const fetchSlide = async () => {
          try {
            const response = await fetch(`${API_URL}api/Slideshow/ShowSlideshow`);
            if (response.ok) {
              const data = await response.json();
              setslidePackage(data);
            } else {
              console.log('Failed to fetch user groups');
            }
          } catch (error) {
            console.error('Error fetching user groups:', error);
          }
        };
    
    fetchGroupDetails();
    fetchStation();
    fetchSlide();
   
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    seteditedKiosk((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/Kiosk/UpdateKiosk/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedKiosk),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/kiosksetup`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/kiosksetup`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Kiosk</h1>
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
                id="kioskName"
                label="Kiosk Name"
                variant="outlined"
                value={editedKiosk.kioskName}
                onChange={(e) => handleInputChange('kioskName', e.target.value)}
              />
              <TextField
                id="location"
                label="Country"
                variant="outlined"
                value={editedKiosk.location}
                onChange={(e) => handleInputChange('location', e.target.value)}
              />
                <TextField
                id="stationCode"
                label="Station Code"
                variant="outlined"
                value={editedKiosk.stationCode}
                onChange={(e) => handleInputChange('stationCode', e.target.value)}
                select
                >
                {stations.map((station) => (
                    <MenuItem key={station.id} value={station.id}>
                    {station.stationName}
                    </MenuItem>
                ))}
                </TextField>

                <TextField
                id="slidePackage"
                label="Slide Package"
                variant="outlined"
                value={editedKiosk.slidePackage}
                onChange={(e) => handleInputChange('slidePackage', e.target.value)}
                select 
                >
                {slidePackage.map((slideshow) => (
                    <MenuItem key={slideshow.id} value={slideshow.id}>
                    {slideshow.packageName}
                    </MenuItem>
                ))}
                </TextField>
                <TextField
                id="webServices"
                label="Web Services"
                variant="outlined"
                value={editedKiosk.webServices}
                onChange={(e) => handleInputChange('webServices', e.target.value)}
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

export default EditKiosk;
