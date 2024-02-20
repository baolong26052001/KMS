import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useParams, useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';

const EditUser = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const API_URL = "https://localhost:7017/";

  // State to store user information
  const [editedUser, setEditedUser] = useState({
    username: '',
    fullname: '',
    email: '',
    password: '',
    userGroupId: '',
    isActive: '',
  });

  const [userGroups, setUserGroups] = useState([]);

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        // Fetch user details based on the provided id
        const response = await fetch(`${API_URL}api/User/ShowUsersInEditPage/${id}`);
        
        if (response.ok) {
          const userData = await response.json();
          
          setEditedUser({
            username: userData[0].username,
            fullname: userData[0].fullname,
            email: userData[0].email,
            password: userData[0].password,
            userGroupId: userData[0].userGroupId,
            isActive: userData[0].isActive,
          });
        } else {
          console.log('Failed to fetch user details');
        }
      } catch (error) {
        console.error('Error fetching user details:', error);
      }
    };

    const fetchUserGroups = async () => {
      try {
        const response = await fetch(`${API_URL}api/Usergroup/ShowUsergroup`);
        
        if (response.ok) {
          const data = await response.json();
          setUserGroups(data);
        } else {
          console.log('Failed to fetch user groups');
        }
      } catch (error) {
        console.error('Error fetching user groups:', error);
      }
    };

    fetchUserDetails();
    fetchUserGroups();
  }, [API_URL, id]);

  const handleInputChange = (key, value) => {
    setEditedUser((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    try {
      const response = await fetch(`${API_URL}api/User/UpdateUser/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedUser),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/users`);
        console.log('User updated successfully');
      } else {
        console.log('User update failed');
      }
    } catch (error) {
      console.error('Error updating user:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/users`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit User</h1>
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
                id="username"
                label="Username"
                variant="outlined"
                fullWidth
                value={editedUser.username}
                onChange={(e) => handleInputChange('username', e.target.value)}
              />
              <TextField
                id="fullname"
                label="Fullname"
                variant="outlined"
                value={editedUser.fullname}
                onChange={(e) => handleInputChange('fullname', e.target.value)}
              />
              <TextField
                id="email"
                label="Email"
                variant="outlined"
                value={editedUser.email}
                onChange={(e) => handleInputChange('email', e.target.value)}
              />
              <TextField
                id="password"
                label="Password"
                variant="outlined"
                type="password"
                value={editedUser.password}
                onChange={(e) => handleInputChange('password', e.target.value)}
              />
              <TextField
                id="userGroupId"
                label="User Group"
                variant="outlined"
                select
                value={editedUser.userGroupId}
                onChange={(e) => handleInputChange('userGroupId', e.target.value)}
              >
                {userGroups.map((group) => (
                  <MenuItem key={group.id} value={group.id}>
                    {group.groupName}
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                id="isActive"
                label="Is Active"
                variant="outlined"
                select
                value={editedUser.isActive}
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

export default EditUser;
