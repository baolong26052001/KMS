import React, { useState, useEffect  } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import MenuItem from '@mui/material/MenuItem';
import { API_URL } from '../../components/config/apiUrl';

const AddUser = () => {
  const navigate = useNavigate();
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

  // State to store user information
  const [newUser, setNewUser] = useState({
    username: '',
    fullname: '',
    email: '',
    password: '',
    userGroupId: '',
    isActive: true, // Assuming isActive is a boolean
    userId: userIdCookie,
  });

  const [userGroups, setUserGroups] = useState([]);
  const [formSubmitted, setFormSubmitted] = useState(false);

  useEffect(() => {
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

    fetchUserGroups();
  }, [API_URL]);

  const handleInputChange = (key, value) => {
    setNewUser((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSave = async () => {
    setFormSubmitted(true);
    if (!isValidEmail(newUser.email) || newUser.password.length < 4 || !newUser.username || !newUser.userGroupId) {
      return;
    }
    
    try {
      // Assuming your API URL is correct
      const response = await fetch(`${API_URL}api/User/AddUser`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newUser),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());

      if (response.ok) {
        navigate(`/users`);
        // Provide user feedback on successful save
        console.log('User added successfully');
      } else {
        console.log('User add failed');
      }
    } catch (error) {
      console.error('Error adding user:', error);
    }
  };
  
  const handleCancel = () => {
    navigate(`/users`);
  };
  const isValidEmail = (email) => {
    // Regular expression for email format validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  };


  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add User</h1>
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
                value={newUser.username}
                onChange={(e) => handleInputChange('username', e.target.value)}
                error={!newUser.username && formSubmitted}
                helperText={formSubmitted && !newUser.username ? 'This field cannot be empty' : ''}
              />
              <TextField
                id="fullname"
                label="Fullname"
                variant="outlined"
                value={newUser.fullname}
                onChange={(e) => handleInputChange('fullname', e.target.value)}
                error={!newUser.fullname && formSubmitted}
                helperText={formSubmitted && !newUser.fullname ? 'This field cannot be empty' : ''}
              />
              <TextField
                id="email"
                label="Email"
                variant="outlined"
                value={newUser.email}
                onChange={(e) => handleInputChange('email', e.target.value)}
                error={!isValidEmail(newUser.email) && formSubmitted} // Show error only if form submitted
                helperText={formSubmitted && !isValidEmail(newUser.email) ? 'Invalid email format' : ''}
              />

              <TextField
                id="password"
                label="Password"
                variant="outlined"
                type="password"
                value={newUser.password}
                onChange={(e) => handleInputChange('password', e.target.value)}
                error={!newUser.password && formSubmitted}
                helperText={formSubmitted && !newUser.password ? 'Password must at least 4 characters' : ''}
              />
              <TextField
                id="userGroupId"
                label="User Group"
                variant="outlined"
                select
                value={newUser.userGroupId}
                onChange={(e) => handleInputChange('userGroupId', e.target.value)}
                error={!newUser.userGroupId && formSubmitted}
                helperText={formSubmitted && !newUser.userGroupId ? 'User group cannot be empty' : ''}
              >
                {userGroups.map((group) => (
                  <MenuItem key={group.id} value={group.id}>
                    {group.groupName}
                  </MenuItem>
                ))}
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

export default AddUser;
