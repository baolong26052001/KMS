import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';

const EditInsuranceType = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const API_URL = "https://localhost:7017/";

  const [editedItem, seteditedItem] = useState({
    typeName: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsuranceType/ShowInsuranceType/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();

          seteditedItem({
            typeName: groupData[0].typeName,
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
      const response = await fetch(`${API_URL}api/InsuranceType/EditInsuranceType/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedItem),
      });

      console.log('Response Status:', response.status);

      if (response.ok) {
        navigate(`/insuranceType`);
        console.log(' updated successfully');
      } else {
        console.log(' update failed');
      }
    } catch (error) {
      console.error('Error updating:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceType`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Insurance Type</h1>
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
                id="typeName"
                label="Insurance Type"
                variant="outlined"
                value={editedItem.typeName}
                onChange={(e) => handleInputChange('typeName', e.target.value)}
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

export default EditInsuranceType;
