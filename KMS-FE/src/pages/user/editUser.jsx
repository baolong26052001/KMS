import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import {useNavigate} from 'react-router-dom';
import { MenuItem } from '@mui/material';


export default function EditUser() {
  const { id } = useParams();
  const [Details, setDetails] = useState({});
  const [editedDetails, setEditedDetails] = useState({}); // State to track edited values
  const API_URL = "https://localhost:7017/";

  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/User/ShowUsersInEditPage/${id}`);
        const data = await response.json();
        setDetails(data[0]);
        setEditedDetails(data[0]);
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setIsLoading(false);
      }
    }
  
    fetchData();
  }, [id]);
  

  const handleInputChange = (key, value) => {
    setEditedDetails((prev) => ({
      ...prev,
      [key]: key === 'userGroupId' || key === 'isActive' ? value : value,
    }));
  };

  const handleSelectChange = (key, value) => {
    setEditedDetails((prev) => ({
      ...prev,
      [key]: value,
    }));
  };
  

  const handleSave = async () => {
    try {
      console.log('Sending PUT request to update user:', editedDetails);
      const response = await fetch(`${API_URL}api/User/UpdateUser/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(editedDetails),
      });
  
      console.log('Response Status:', response.status);
      console.log('Response Content:', await response.text());
  
      if (response.ok) {
        navigate(`/users`);
        // Provide user feedback on successful save
        console.log('User updated successfully');
        
      } else {
        console.log('User update failed');
      }
    } catch (error) {
      // Revert to the original state if the update fails
      console.error('Error updating data:', error);
      setDetails((prevDetails) => ({ ...prevDetails }));
    }
  };
  
  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Edit User</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          <div className='table-container'>
            <Box
              component="form"
              sx={{
                '& > :not(style)': {
                  m: 1,
                  width: 500,
                  maxWidth: '100%',
                },
              }}
              noValidate
              autoComplete="off"
            >
              {Details &&
                Object.entries(Details).map(([key, value]) => (
                  <div className='input-field' key={key}>
                    {key === 'id' ? (
                      <TextField
                        fullWidth
                        id={key}
                        label={key}
                        variant="outlined"
                        value={value}
                        disabled
                      />
                    ) : key === 'password' ? (
                      <TextField
                        fullWidth
                        id={key}
                        label={key}
                        variant="outlined"
                        type="password"
                        value={editedDetails[key] || ''}
                        onChange={(e) => handleInputChange(key, e.target.value)}
                      />
                    ) : key === 'userGroupId' || key === 'isActive' ? (
                      <TextField
                        fullWidth
                        id={key}
                        label={key}
                        variant="outlined"
                        select
                        value={editedDetails[key] || ''}
                        onChange={(e) => handleSelectChange(key, e.target.value)}
                      >
                        {key === 'userGroupId' ? (
                          <>
                            <MenuItem value={1}>Administraion</MenuItem>
                            <MenuItem value={2}>Support</MenuItem>
                          </>
                        ) : (
                          <>
                            <MenuItem value="yes">Yes</MenuItem>
                            <MenuItem value="no">No</MenuItem>
                          </>
                        )}
                      </TextField>
                    ) : (
                      <TextField
                        fullWidth
                        id={key}
                        label={key}
                        variant="outlined"
                        value={editedDetails[key] || ''}
                        onChange={(e) => handleInputChange(key, e.target.value)}
                      />
                    )}
                  </div>
                ))}
              <Button variant="contained" onClick={handleSave} style={{float: 'left'}}>
                Save
              </Button>
            </Box>
          </div>
        </div>
      </div>
    </div>
  );

}