import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';

const AddInsurancePackageDetail = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  const { id } = useParams();
  const [insAge, setinsAge] = useState([]);
  const [insPackageHeader, setinsPackageHeader] = useState([]);
  
  // State to store user information
  const [newInsurancePackage, setnewInsurancePackage] = useState({
    packageHeaderId: id,
    ageRangeId: '',
    fee: '',
  });

  useEffect(() => {

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
        const response = await fetch(`${API_URL}api/InsurancePackageHeader/ShowInsurancePackageHeader/${newInsurancePackage.packageHeaderId}`);
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
      const response = await fetch(`${API_URL}api/InsurancePackageDetail/AddInsurancePackageDetail`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newInsurancePackage),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/insurancePackageDetail/${id}`);
        console.log('added successfully');
      } else {
        console.log('add failed');
      }
    } catch (error) {
      console.error('Error adding:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/insurancePackageDetail/${id}`);
  };
  
  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Insurance Package Details</h1>
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
              //label="Insurance Package"
              variant="outlined"
              defaultValue = {insPackageHeader.packageName}
              value={insPackageHeader.packageName}
              disabled
              onChange={(e) => handleInputChange('fee', e.target.value)}
              >
              </TextField>
              <TextField
              id="ageRangeId"
              label="Age Range"
              variant="outlined"
              value={newInsurancePackage.ageRangeId}
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
              value={newInsurancePackage.fee}
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

export default AddInsurancePackageDetail;