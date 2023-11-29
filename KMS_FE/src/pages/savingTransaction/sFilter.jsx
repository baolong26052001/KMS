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


const packages = [
  'Yes',
  'No'
];


function getPackages(kpackage, packageName, theme) {
    return {
      fontWeight:
      packageName.indexOf(kpackage) === -1
          ? theme.typography.fontWeightRegular
          : theme.typography.fontWeightMedium,
    };
}

export default function SavingTransactionFilter() {
  const theme = useTheme();

  const [packageName, setPackage] = React.useState([]);


  const handleChangePackage = (event) => {
    const {
      target: { value },
    } = event;
    setPackage(
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
                    value={packageName}
                    onChange={handleChangePackage}
                    input={<OutlinedInput label="Is Active" />}
                    MenuProps={MenuProps}
                    >
                    {packages.map((kpackage) => (
                        <MenuItem
                        key={kpackage}
                        value={kpackage}
                        style={getPackages(kpackage, packageName, theme)}
                        >
                        {kpackage}
                        </MenuItem>
                    ))}
                    </Select>
                </FormControl>
            </Grid>
        </Grid>

            

    </div>
  );
}
