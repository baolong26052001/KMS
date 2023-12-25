import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';

const AddInsurancePackage = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [newInsurancePackage, setnewInsurancePackage] = useState({
    packageName: '',
    insuranceType: '',
    duration: '',
    payType: '',
    annualFee: '',
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
      const response = await fetch(`${API_URL}api/InsurancePackage/AddInsurancePackage`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newInsurancePackage),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/insurancePackage`);
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
    navigate(`/insurancePackage`);
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
                id="packageName"
                label="Package Name"
                variant="outlined"
                value={newInsurancePackage.packageName}
                onChange={(e) => handleInputChange('packageName', e.target.value)}
              />
              <TextField
                id="insuranceType"
                label="Insurance Type"
                variant="outlined"
                value={newInsurancePackage.insuranceType}
                onChange={(e) => handleInputChange('insuranceType', e.target.value)}
              />
              <TextField
                id="duration"
                label="Duration"
                variant="outlined"
                value={newInsurancePackage.duration}
                onChange={(e) => handleInputChange('duration', e.target.value)}
              />
              <TextField
                id="payType"
                label="Payment Frequency"
                variant="outlined"
                value={newInsurancePackage.payType}
                onChange={(e) => handleInputChange('payType', e.target.value)}
              />
               <TextField
                id="annualFee"
                label="Annual Fee"
                variant="outlined"
                value={newInsurancePackage.annualFee}
                onChange={(e) => handleInputChange('annualFee', e.target.value)}
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

export default AddInsurancePackage;