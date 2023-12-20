import React from 'react';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import FormControl from '@mui/material/FormControl';
import Grid from '@mui/material/Grid';

const DateFilter = ({ startDate, endDate, handleStartDateChange, handleEndDateChange }) => {
  return (
    <div className="Filter">
      <Grid container spacing={6}>
        <Grid item xs={4}>
          <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350 }}>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DatePicker
                label="From Date"
                value={startDate}
                onChange={handleStartDateChange}
              />
            </LocalizationProvider>
          </FormControl>
        </Grid>
        <Grid item xs={4}>
          <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350 }}>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DatePicker
                label="To Date"
                value={endDate}
                onChange={handleEndDateChange}
              />
            </LocalizationProvider>
          </FormControl>
        </Grid>
      </Grid>
    </div>
  );
};

export default DateFilter;
