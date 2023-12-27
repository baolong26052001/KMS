import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';
import { useParams } from 'react-router-dom';

const AddBenefit = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  const { id } = useParams();
  const { packageName } = useParams();
  // State to store user information
  const [newInsurancePackage, setnewInsurancePackage] = useState({
    packageId: id,
    content: '',
    coverage: '',
    description: '',
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
      const response = await fetch(`${API_URL}api/InsurancePackage/AddBenefit`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newInsurancePackage),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/viewPackageDetail/${id}/${packageName}`);
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
    navigate(`/viewPackageDetail/${id}/${packageName}`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Insurance Package</h1>
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
              <TextField
                id="description"
                label="Description"
                variant="outlined"
                value={newInsurancePackage.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
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

export default AddBenefit;