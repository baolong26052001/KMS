import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';

const EditInsurancePackage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const API_URL = "https://localhost:7017/";
  const [insType, setinsType] = useState([]);
  // State to store user information
  const [editedPackage, seteditedPackage] = useState({
    packageName: '',
    insuranceType: '',
    duration: '',
    payType: '',
    fee: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsurancePackage/ShowInsurancePackage/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          seteditedPackage({
            packageName: groupData[0].packageName,
            insuranceType: groupData[0].insuranceType, 
            duration: groupData[0].duration,
            payType: groupData[0].payType,
            fee: groupData[0].fee,
          });
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };

    const fetchInsuranceType = async () => {
        try {
          const response = await fetch(`${API_URL}api/InsuranceType/ShowInsuranceType`);
          if (response.ok) {
            const data = await response.json();
            setinsType(data);
          } else {
            console.log('Failed to fetch');
          }
        } catch (error) {
          console.error('Error fetching:', error);
        }
      };
    
    fetchGroupDetails();
    fetchInsuranceType();
    
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    seteditedPackage((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/InsurancePackage/EditInsurancePackage/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedPackage),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/insurancePackage`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insurancePackage`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Insurance Package</h1>
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
                id="packageName"
                label="Package Name"
                variant="outlined"
                value={editedPackage.packageName}
                onChange={(e) => handleInputChange('packageName', e.target.value)}
              />
            <TextField
            id="insuranceType"
            label="Insurance Type"
            variant="outlined"
            value={editedPackage.insuranceType}
            onChange={(e) => handleInputChange('insuranceType', e.target.value)}
            select
            >
            {insType.map((type) => (
                <MenuItem key={type.id} value={type.id}>
                {type.typeName} 
                </MenuItem>
            ))}
            </TextField>
            <TextField
                id="duration"
                label="Duration"
                variant="outlined"
                value={editedPackage.duration}
                onChange={(e) => handleInputChange('duration', e.target.value)}
              />
            <TextField
                id="payType"
                label="Pay Type"
                variant="outlined"
                value={editedPackage.payType}
                onChange={(e) => handleInputChange('payType', e.target.value)}
              />
            <TextField
                id="fee"
                label="Fee"
                variant="outlined"
                value={editedPackage.fee}
                onChange={(e) => handleInputChange('fee', e.target.value)}
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

export default EditInsurancePackage;
