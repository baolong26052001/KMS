import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';
import { API_URL } from '../../components/config/apiUrl';

const EditAccount = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [editedGroup, setEditedGroup] = useState({
    fullName: '',
    phone: '',
    address1: '',
    idenNumber: '',
    bankName: '',
    bankNumber: '',
    companyName: '',
    companyAddress: '',
    department: '',
    email: '',
    isActive: ''
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/Member/ShowMember/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          setEditedGroup({
            fullName: groupData[0].fullName,
            phone: groupData[0].phone,
            address1: groupData[0].address1,
            idenNumber: groupData[0].idenNumber,
            bankName: groupData[0].bankName,
            bankNumber: groupData[0].bankNumber,
            companyName: groupData[0].companyName,
            companyAddress: groupData[0].companyAddress,
            department: groupData[0].department,
            email: groupData[0].email,
            isActive: groupData[0].isActive,
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
      const response = await fetch(`${API_URL}api/Member/UpdateMember/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedGroup),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/account`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/account`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Account</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          <div className="table-container">
            <Box
              component="form"
              sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: '20px',
                margin: 'auto',
              }}
              noValidate
              autoComplete="on"
            >
              <TextField
                id="fullName"
                label="Full Name"
                variant="outlined"
                value={editedGroup.fullName}
                onChange={(e) => handleInputChange('fullName', e.target.value)}
              />
              <TextField
                id="phone"
                label="Phone Number"
                variant="outlined"
                value={editedGroup.phone}
                onChange={(e) => handleInputChange('phone', e.target.value)}
              />
            <TextField
                id="address1"
                label="Address"
                variant="outlined"
                value={editedGroup.address1}
                onChange={(e) => handleInputChange('address1', e.target.value)}
              />
            <TextField
                id="idenNumber"
                label="IdenNumber"
                variant="outlined"
                value={editedGroup.idenNumber}
                onChange={(e) => handleInputChange('idenNumber', e.target.value)}
              />
            <TextField
                id="bankName"
                label="Bank Name"
                variant="outlined"
                value={editedGroup.bankName}
                onChange={(e) => handleInputChange('bankName', e.target.value)}
              />
            <TextField
                id="bankNumber"
                label="Bank Number"
                variant="outlined"
                value={editedGroup.bankNumber}
                onChange={(e) => handleInputChange('bankNumber', e.target.value)}
              />
            <TextField
                id="companyName"
                label="Company Name"
                variant="outlined"
                value={editedGroup.companyName}
                onChange={(e) => handleInputChange('companyName', e.target.value)}
              />
            <TextField
                id="companyAddress"
                label="Company Address"
                variant="outlined"
                value={editedGroup.companyAddress}
                onChange={(e) => handleInputChange('companyAddress', e.target.value)}
              />
            <TextField
                id="department"
                label="Department"
                variant="outlined"
                value={editedGroup.department}
                onChange={(e) => handleInputChange('department', e.target.value)}
              />
            <TextField
                id="email"
                label="Email"
                variant="outlined"
                value={editedGroup.email}
                onChange={(e) => handleInputChange('email', e.target.value)}
              />
            <TextField
                id="isActive"
                label="Is Active"
                variant="outlined"
                select
                value={editedGroup.isActive}
                onChange={(e) => handleInputChange('isActive', e.target.value)}
                >
                <MenuItem value={true}>True</MenuItem>
                <MenuItem value={false}>False</MenuItem>
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

export default EditAccount;