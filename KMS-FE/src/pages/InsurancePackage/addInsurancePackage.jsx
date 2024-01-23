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
  const [insTerm, setinsTerm] = useState([]);
  const [insProvider, setinsProvider] = useState([]);
  
  // State to store user information
  const [newInsurancePackage, setnewInsurancePackage] = useState({
    packageName: '',
    insuranceTypeId: '',
    termId: '',
    insuranceProviderId: '',
    isActive: true,
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

    const fetchInsuranceTerm = async () => {
      try {
        const response = await fetch(`${API_URL}api/Term/ShowTerm`);
        if (response.ok) {
          const data = await response.json();
          setinsTerm(data);
        } else {
          console.log('Failed to fetch');
        }
      } catch (error) {
        console.error('Error fetching:', error);
      }
    };

    const fetchInsuranceProvider = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsuranceProvider/ShowInsuranceProvider`);
        if (response.ok) {
          const data = await response.json();
          setinsProvider(data);
        } else {
          console.log('Failed to fetch');
        }
      } catch (error) {
        console.error('Error fetching:', error);
      }
    };
    
    fetchInsuranceType();
    fetchInsuranceTerm();
    fetchInsuranceProvider();
    
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    setnewInsurancePackage((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/InsurancePackageHeader/AddInsurancePackageHeader`, {
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
              id="insuranceTypeId"
              label="Insurance Type"
              variant="outlined"
              value={newInsurancePackage.insuranceTypeId}
              onChange={(e) => handleInputChange('insuranceTypeId', e.target.value)}
              select
              >
              {insType.map((type) => (
                  <MenuItem key={type.id} value={type.id}>
                  {type.typeName} 
                  </MenuItem>
              ))}
              </TextField>
              <TextField
                id="termId"
                label="Term"
                variant="outlined"
                multiline
                value={newInsurancePackage.termId}
                onChange={(e) => handleInputChange('termId', e.target.value)}
                select
              >
              {insTerm.map((term) => (
                  <MenuItem key={term.id} value={term.id}>
                  {term.content} 
                  </MenuItem>
              ))}
              </TextField>
              <TextField
                id="insuranceProviderId"
                label="Insurance Provider"
                variant="outlined"
                value={newInsurancePackage.insuranceProviderId}
                onChange={(e) => handleInputChange('insuranceProviderId', e.target.value)}
                select
              >
              {insProvider.map((provider) => (
                  <MenuItem key={provider.id} value={provider.id}>
                  {provider.provider} 
                  </MenuItem>
              ))}
              </TextField>
              <TextField
                id="isActive"
                label="Is Active"
                variant="outlined"
                select
                value={newInsurancePackage.isActive}
                onChange={(e) => handleInputChange('isActive', e.target.value)}
                >
                <MenuItem value={true}>Enable</MenuItem>
                <MenuItem value={false}>Disable</MenuItem>
            </TextField>
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