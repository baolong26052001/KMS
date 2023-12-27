import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';


const AddKiosk = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  // State to store user information
  const [newKiosk, setnewKiosk] = useState({
    kioskName: '',
    location: '',
    stationCode: '',
    slidePackage: '',
    webServices: '',
  });

  const [stations, setStations] = useState([]);
  const [slidePackage, setslidePackage] = useState([]);


  useEffect(() => {
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

    fetchStation();
    fetchSlide();
  }, [API_URL]);

  const handleInputChange = (key, value) => {
    setnewKiosk((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/Kiosk/AddKiosk`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newKiosk),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/kiosksetup`);
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
    navigate(`/kiosksetup`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Kiosk</h1>
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
                value={newKiosk.kioskName}
                onChange={(e) => handleInputChange('kioskName', e.target.value)}
              />
              <TextField
                id="location"
                label="Country"
                variant="outlined"
                value={newKiosk.location}
                onChange={(e) => handleInputChange('location', e.target.value)}
              />
                <TextField
                id="stationCode"
                label="Station Code"
                variant="outlined"
                value={newKiosk.stationCode}
                onChange={(e) => handleInputChange('stationCode', e.target.value)}
                select // Add the select prop for a dropdown
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
                value={newKiosk.slidePackage}
                onChange={(e) => handleInputChange('slidePackage', e.target.value)}
                select // Add the select prop for a dropdown
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
                value={newKiosk.webServices}
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

export default AddKiosk;