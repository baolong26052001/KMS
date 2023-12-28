import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';
import { useParams } from 'react-router-dom';

const AddBenefitDetail = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  const { id } = useParams();
  // State to store user information
  const [newInsurancePackage, setnewInsurancePackage] = useState({
    benefitId: id,
    content: '',
    coverage: '',
  });

  const handleInputChange = (key, value) => {
    setnewInsurancePackage((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/InsurancePackage/AddBenefitDetail`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newInsurancePackage),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/benefitDetail/${id}`);
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
    navigate(`/benefitDetail/${id}`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Benefit Detail</h1>
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
                id="content"
                label="Content"
                variant="outlined"
                value={newInsurancePackage.content}
                onChange={(e) => handleInputChange('content', e.target.value)}
              />
              <TextField
                id="coverage"
                label="Coverage"
                variant="outlined"
                value={newInsurancePackage.coverage}
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

export default AddBenefitDetail;