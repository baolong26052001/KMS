import React, { useState, useEffect } from 'react';
import Grid from '@mui/material/Grid';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import Select from '@mui/material/Select';
import { useTheme } from '@mui/material/styles';

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4 + ITEM_PADDING_TOP,
      width: 'auto',
    },
  },
};

export default function UserFilter({ onFilterChange }) {
  const theme = useTheme();
  const [groupName, setGroupName] = useState('');
  const [isActive, setIsActive] = useState('');

  useEffect(() => {
    // Fetch user group names and active status from the backend
    const fetchOptions = async () => {
      try {
        const response = await fetch('https://localhost:7017/api/Usergroup/FetchFilterOptions');
        if (response.ok) {
          const data = await response.json();
        } else {
          console.error('Failed to fetch filter options:', response.statusText);
        }
      } catch (error) {
        console.error('Error fetching filter options:', error);
      }
    };

    fetchOptions();
  }, []);


  return (
    <div>
      <Grid container spacing={5}>
        <Grid item xs={4}>
          <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350 }}>
            <InputLabel id="group-name-label">Group Name</InputLabel>
            <Select
              labelId="group-name-label"
              id="group-name"
              value={groupName}
              onChange={(e) => setGroupName(e.target.value)}
              input={<OutlinedInput label="Group Name" />}
              MenuProps={MenuProps}
            >
              {/* Fetch API from const API_URL = "https://localhost:7017/"; fetch(`${API_URL}api/Usergroup/ShowUsergroup`) */}
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={4}>
          <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350, placeItems: 'right' }}>
            <InputLabel id="active-label">Active</InputLabel>
            <Select
              labelId="active-label"
              id="active"
              value={isActive}
              onChange={(e) => setIsActive(e.target.value)}
              input={<OutlinedInput label="Active" />}
              MenuProps={MenuProps}
            >
              {/* MenuItem have two value: True and False */}
            </Select>
          </FormControl>
        </Grid>
      </Grid>
    </div>
  );
}
