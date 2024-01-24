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
  const [insTerm, setinsTerm] = useState([]);
  const [insProvider, setinsProvider] = useState([]);
  // State to store user information
  const [editedPackage, seteditedPackage] = useState({
    packageName: '',
    insuranceTypeId: '',
    termId: '',
    insuranceProviderId: '',
    isActive: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsurancePackageHeader/ShowInsurancePackageHeader/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          seteditedPackage({
            packageName: groupData[0].packageName,
            insuranceTypeId: groupData[0].insuranceTypeId, 
            termId: groupData[0].termId,
            insuranceProviderId: groupData[0].insuranceProviderId,
            isActive: groupData[0].isActive,
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
      

    fetchInsuranceTerm();
    fetchInsuranceProvider();
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
      const response = await fetch(`${API_URL}api/InsurancePackageHeader/EditInsurancePackageHeader/${id}`, {
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
                gap: '20px', // Add some spacing between form elements
                //width: 300, 
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
              id="insuranceTypeId"
              label="Insurance Type"
              variant="outlined"
              value={editedPackage.insuranceTypeId}
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
              id="insuranceProviderId"
              label="Insurance Provider"
              variant="outlined"
              value={editedPackage.insuranceProviderId}
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
              id="termId"
              label="Insurance Term"
              variant="outlined"
              style={{textAlign: 'left'}}
              value={editedPackage.termId}
              onChange={(e) => handleInputChange('termId', e.target.value)}
              multiline
              select
              >
              <MenuItem value="">
                <em>No Term</em>
              </MenuItem>
              {insTerm.map((term) => (
                  <MenuItem key={term.id} value={term.id}>
                  {term.content}
                  </MenuItem>
              ))}
              </TextField>
              <TextField
                id="isActive"
                label="Is Active"
                variant="outlined"
                select
                value={editedPackage.isActive}
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

export default EditInsurancePackage;
