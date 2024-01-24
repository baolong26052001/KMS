import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

const AddGroup = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [newGroup, setnewGroup] = useState({
    groupName: '',
    accessRuleId: '',
    isActive: true,
  });

  const handleInputChange = (key, value) => {
    setnewGroup((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/Usergroup/AddUsergroup`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newGroup),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/usersgroup`);
        // Provide user feedback on successful save
        console.log('Group added successfully');
      } else {
        console.log('Group add failed');
      }
    } catch (error) {
      console.error('Error adding Group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/usersgroup`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add User Group</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          <div className="table-container">
            <Box
              component="form"
              sx={{
                display: 'flex',
                flexDirection: 'column', // Set the form to vertical layout
                gap: '20px', // Add some spacing between form elements
                //width: 300, 
                margin: 'auto', // Center the form
              }}
              noValidate
              autoComplete="off"
            >
                
                <TextField
                id="userGroupName"
                label="User Group"
                variant="outlined"
                value={newGroup.groupName}
                onChange={(e) => handleInputChange('groupName', e.target.value)}
                >
                </TextField>

                <TextField
                id="accessRuleId"
                label="Access Rule"
                variant="outlined"
                value={newGroup.accessRuleId}
                onChange={(e) => handleInputChange('accessRuleId', e.target.value)}
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

export default AddGroup;
