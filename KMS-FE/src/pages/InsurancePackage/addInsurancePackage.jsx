import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';

const AddInsurancePackage = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  const { id } = useParams();
  const [insType, setinsType] = useState([]);
  // State to store user information
  const [newInsurancePackage, setnewInsurancePackage] = useState({
    packageName: '',
    insuranceType: '',
    provider: '',
    duration: '',
    payType: '',
    fee: '',
  });

  useEffect(() => {

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
    
    fetchInsuranceType();
    
  }, [API_URL, id]);

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
              select
              >
              {insType.map((type) => (
                  <MenuItem key={type.id} value={type.id}>
                  {type.typeName} 
                  </MenuItem>
              ))}
              </TextField>
              <TextField
                id="provider"
                label="Provider"
                variant="outlined"
                value={newInsurancePackage.provider}
                onChange={(e) => handleInputChange('provider', e.target.value)}
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
                id="fee"
                label="Fee"
                variant="outlined"
                value={newInsurancePackage.fee}
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

export default AddInsurancePackage;