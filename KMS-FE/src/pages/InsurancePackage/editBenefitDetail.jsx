import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';
import { useLocation } from 'react-router-dom';

const EditBenefitDetail = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams();
  const { benefitId } = location.state;
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [editedGroup, setEditedGroup] = useState({
    benefitId: benefitId,
    content: '',
    coverage: '',
  });


  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsurancePackage/ShowBenefitDetailById/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          // Populate the state with fetched group details
          setEditedGroup({
            content: groupData[0].content,
            coverage: groupData[0].coverage, 
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
      const response = await fetch(`${API_URL}api/InsurancePackage/EditBenefitDetail/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedGroup),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/benefitDetail/${benefitId}`);
        console.log(' updated successfully');
      } else {
        console.log(' update failed');
      }
    } catch (error) {
      console.error('Error updating:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/benefitDetail/${benefitId}`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Benefit Detail</h1>
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
              autoComplete="on"
            >
              <TextField
                id="content"
                label="Benefit Content"
                variant="outlined"
                value={editedGroup.content}
                onChange={(e) => handleInputChange('content', e.target.value)}
              />
              <TextField
                id="coverage"
                label="Coverage"
                variant="outlined"
                value={editedGroup.coverage}
                onChange={(e) => handleInputChange('coverage', e.target.value)}
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

export default EditBenefitDetail;
