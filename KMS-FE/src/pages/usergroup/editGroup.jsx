import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';
import { API_URL } from '../../components/config/apiUrl';
const EditGroup = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i].trim();
      if (cookie.startsWith(name + '=')) {
        return cookie.substring(name.length + 1);
      }
    }
    return null;
  }

  const userIdCookie = getCookie('userId');

  // State to store group information
  const [editedGroup, setEditedGroup] = useState({
    groupName: '',
    isActive: '', 
    userId: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/Usergroup/ShowUsergroup/${id}`);
        
        if (response.ok) {
          const groupData = await response.json();
          
          setEditedGroup({
            groupName: groupData[0].groupName,
            isActive: groupData[0].isActive,
            userId: userIdCookie,
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
      const response = await fetch(`${API_URL}api/Usergroup/UpdateUsergroup/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedGroup),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/usersgroup`);
        console.log('Group updated successfully');
      } else {
        console.log('Group update failed');
      }
    } catch (error) {
      console.error('Error updating group:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/usersgroup`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit Group</h1>
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
                id="groupName"
                label="Group Name"
                variant="outlined"
                value={editedGroup.groupName}
                onChange={(e) => handleInputChange('groupName', e.target.value)}
                disabled={editedGroup.groupName === 'Admin'}
              />
              <TextField
                id="isActive"
                label="Is Active"
                variant="outlined"
                value={editedGroup.isActive}
                select
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

export default EditGroup;