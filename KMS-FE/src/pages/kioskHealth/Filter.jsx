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


const stations = [
    'INT - SaiGon',
    'INT - Hanoi',
    'AMD - SaiGon',
    'AMD - Hanoi',
  ];



function getStation(station, stationName, theme) {
    return {
      fontWeight:
      stationName.indexOf(station) === -1
          ? theme.typography.fontWeightRegular
          : theme.typography.fontWeightMedium,
    };
  }


export default function Filter() {
  const theme = useTheme();

  const [stationName, setStationName] = React.useState([]);

  const handleChangeStation = (event) => {
    const {
      target: { value },
    } = event;
    setStationName(
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
                    <InputLabel id="station-name-label">Station</InputLabel>
                    <Select
                    labelId="station-name-label"
                    id="station-name"
                    value={stationName}
                    onChange={handleChangeStation}
                    input={<OutlinedInput label=" Station" />}
                    MenuProps={MenuProps}
                    >
                    {stations.map((station) => (
                        <MenuItem
                        key={station}
                        value={station}
                        style={getStation(station, stationName, theme)}
                        >
                        {station}
                        </MenuItem>
                    ))}
                    </Select>
                </FormControl>
            </Grid>
        </Grid>

            

    </div>
  );
}
