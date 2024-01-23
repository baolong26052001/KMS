import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';

const EditInsuranceTerm = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const API_URL = "https://localhost:7017/";

  const [editedItem, seteditedItem] = useState({
    content: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/Term/ShowTerm/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();

          seteditedItem({
            content: groupData[0].content,
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
      const response = await fetch(`${API_URL}api/Term/EditTerm/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedItem),
      });

      console.log('Response Status:', response.status);

      if (response.ok) {
        navigate(`/insuranceTerm`);
        console.log(' updated successfully');
      } else {
        console.log(' update failed');
      }
    } catch (error) {
      console.error('Error updating:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insuranceTerm`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Insurance Term</h1>
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
                width: '100%' ,
                margin: 'auto',
              }}
              noValidate
              autoComplete="on"
            >
              <TextField
                id="content"
                label="Insurance Term"
                variant="outlined"
                multiline
                value={editedItem.content}
                onChange={(e) => handleInputChange('content', e.target.value)}
              />
              <Box sx={{ display: 'flex', gap: '8px' , width: 300, marginTop: 1}}>
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

export default EditInsuranceTerm;