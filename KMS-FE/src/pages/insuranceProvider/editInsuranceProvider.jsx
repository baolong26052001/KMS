import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';
import { API_URL } from '../../components/config/apiUrl';

const EditInsuranceProvider = () => {
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
    provider: '',
    email: '',
    userId: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsuranceProvider/ShowInsuranceProvider/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();

          seteditedItem({
            provider: groupData[0].provider,
            email: groupData[0].email, 
            userId: userIdCookie,
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
    seteditedItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/InsuranceProvider/EditInsuranceProvider/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedItem),
      });

      console.log('Response Status:', response.status);

      if (response.ok) {
        navigate(`/insuranceProvider`);
        console.log(' updated successfully');
      } else {
        console.log(' update failed');
      }
    } catch (error) {
      console.error('Error updating:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceProvider`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Insurance Provider</h1>
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
                id="provider"
                label="Insurance Provider"
                variant="outlined"
                value={editedItem.provider}
                onChange={(e) => handleInputChange('provider', e.target.value)}
              />
              <TextField
                id="email"
                label="Email"
                variant="outlined"
                value={editedItem.email}
                onChange={(e) => handleInputChange('email', e.target.value)}
              />
              <Box sx={{ display: 'flex', gap: '8px' , marginTop: 1}}>
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

export default EditInsuranceProvider;
