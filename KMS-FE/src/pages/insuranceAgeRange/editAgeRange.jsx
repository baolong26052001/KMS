import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';
import { API_URL } from '../../components/config/apiUrl';

const EditAgeRange = () => {
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

  const [editedItem, seteditedItem] = useState({
    startAge: '',
    endAge: '',
    userId: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/AgeRange/ShowAgeRange/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();

          seteditedItem({
            startAge: groupData[0].startAge,
            endAge: groupData[0].endAge,
            userId: userIdCookie,
          });
          console.log(editedItem.endAge);
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
    seteditedItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/AgeRange/EditAgeRange/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedItem),
      });

      console.log('Response Status:', response.status);

      if (response.ok) {
        navigate(`/insuranceAgeRange`);
        console.log(' updated successfully');
      } else {
        console.log(' update failed');
      }
    } catch (error) {
      console.error('Error updating:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceAgeRange`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Insurance Age Range</h1>
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
                    value={editedItem.startAge}
                    onChange={(e) => handleInputChange('startAge', e.target.value)}
                    />

                    <TextField
                    id="endAge"
                    label="Max Age"
                    variant="outlined"
                    type="number"
                    value={editedItem.endAge}
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

export default EditAgeRange;