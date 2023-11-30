import * as React from 'react';
import { useTheme } from '@mui/material/styles';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import Grid from '@mui/material/Grid';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

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


const mstatus = [
  'Yes',
  'No'
];


function getPackages(status, statusName, theme) {
    return {
      fontWeight:
      statusName.indexOf(status) === -1
          ? theme.typography.fontWeightRegular
          : theme.typography.fontWeightMedium,
    };
}

export default function AccountFilter() {
  const theme = useTheme();

  const [statusName, setStatus] = React.useState([]);


  const handleChangeStatus = (event) => {
    const {
      target: { value },
    } = event;
    setStatus(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };

  return (
    <div>

        <Grid container spacing={6}>
            <Grid item xs={4}>
                <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350}}>
                  <LocalizationProvider dateAdapter={AdapterDayjs}>
                    <DatePicker label="From Date" />
                  </LocalizationProvider>
                </FormControl>
            </Grid>
            <Grid item xs={4}>
            <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350}}>
                  <LocalizationProvider dateAdapter={AdapterDayjs}>
                    <DatePicker label="To Date" />
                  </LocalizationProvider>
                </FormControl>
            </Grid>
            <Grid item xs={4}>
                <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350}}>
                    <InputLabel id="group-name-label">Is Active</InputLabel>
                    <Select
                    labelId="group-name-label"
                    id="group-name"
                    value={statusName}
                    onChange={handleChangeStatus}
                    input={<OutlinedInput label="Is Active" />}
                    MenuProps={MenuProps}
                    >
                    {mstatus.map((status) => (
                        <MenuItem
                        key={status}
                        value={status}
                        style={getPackages(status, statusName, theme)}
                        >
                        {status}
                        </MenuItem>
                    ))}
                    </Select>
                </FormControl>
            </Grid>
        </Grid>

            

    </div>
  );
}
