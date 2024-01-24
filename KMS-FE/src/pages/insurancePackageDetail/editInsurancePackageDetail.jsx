import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';

const EditInsurancePackageDetail = () => {
  const navigate = useNavigate();
  const { id, packageHeaderId } = useParams();
  const API_URL = "https://localhost:7017/";
  const [insAge, setinsAge] = useState([]);
  const [insPackageHeader, setinsPackageHeader] = useState([]);
  // State to store user information
  const [editedPackageDetail, seteditedPackageDetail] = useState({
    packageHeaderId: '',
    ageRangeId: '',
    fee: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsurancePackageDetail/ShowInsurancePackageDetail/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          seteditedPackageDetail({
            packageHeaderId: packageHeaderId,
            ageRangeId: groupData[0].ageRangeId, 
            fee: groupData[0].fee,
          });
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };

    const fetchAgeRange = async () => {
      try {
        const response = await fetch(`${API_URL}api/AgeRange/ShowAgeRange`);
        if (response.ok) {
          const data = await response.json();
          setinsAge(data);
        } else {
          console.log('Failed to fetch');
        }
      } catch (error) {
        console.error('Error fetching:', error);
      }
    };

    const fetchPackageHeader = async () => {
      try {
        const response = await fetch(`${API_URL}api/InsurancePackageHeader/EditInsurancePackageHeader/${packageHeaderId}`);
        if (response.ok) {
          const data = await response.json();
          setinsPackageHeader({packageName: data[0].packageName});
        } else {
          console.log('Failed to fetch');
        }
      } catch (error) {
        console.error('Error fetching:', error);
      }
    };

    fetchAgeRange();
    fetchPackageHeader();
    fetchGroupDetails();
    
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    seteditedPackageDetail((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/InsurancePackageDetail/EditInsurancePackageDetail/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedPackageDetail),
      });

      console.log('Response Status:', response.status);

      if (response.ok) {
        navigate(`/insurancePackageDetail/${editedPackageDetail.packageHeaderId}`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insurancePackageDetail/${packageHeaderId}`);
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
                id="ageRangeId"
                label="Age Range"
                variant="outlined"
                value={editedPackageDetail.ageRangeId}
                onChange={(e) => handleInputChange('ageRangeId', e.target.value)}
                select
              >
              {insAge.map((age) => (
                  <MenuItem key={age.id} value={age.id}>
                  {age.description} 
                  </MenuItem>
              ))}
              </TextField>
              <TextField
              id="fee"
              label="Insurance Fee"
              variant="outlined"
              value={editedPackageDetail.fee}
              onChange={(e) => handleInputChange('fee', e.target.value)}
              >
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

export default EditInsurancePackageDetail;
