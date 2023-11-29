import * as React from 'react';
import { useTheme } from '@mui/material/styles';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import Grid from '@mui/material/Grid';

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
    'Intel',
    'AMD',
];

const countries = [
  'INT - SaiGon',
  'INT - Hanoi',
  'AMD - SaiGon',
  'AMD - Hanoi',
];



const packages = [
  'Sai Gon',
  'Ha Noi',
  'Nha Trang',
  'Vung Tau',
  'Binh Duong',
  'Da Nang',
];



function getStation(station, stationName, theme) {
  return {
    fontWeight:
    stationName.indexOf(station) === -1
        ? theme.typography.fontWeightRegular
        : theme.typography.fontWeightMedium,
  };
}

function getCountries(country, countryName, theme) {
    return {
      fontWeight:
      countryName.indexOf(country) === -1
          ? theme.typography.fontWeightRegular
          : theme.typography.fontWeightMedium,
    };
  }

function getPackages(kpackage, packageName, theme) {
    return {
      fontWeight:
      packageName.indexOf(kpackage) === -1
          ? theme.typography.fontWeightRegular
          : theme.typography.fontWeightMedium,
    };
}

export default function Filter() {
  const theme = useTheme();
  const [stationName, setStationName] = React.useState([]);

  const [countryName, setCountry] = React.useState([]);

  const [packageName, setPackage] = React.useState([]);

  const handleChangeStation = (event) => {
    const {
      target: { value },
    } = event;
    setStationName(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };

  const handleChangeCountry = (event) => {
    const {
      target: { value },
    } = event;
    setCountry(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };

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
                    <InputLabel id="group-name-label">Station Name</InputLabel>
                    <Select
                    labelId="country-name-label"
                    id="country-name"
                    multiple
                    value={countryName}
                    onChange={handleChangeCountry}
                    input={<OutlinedInput label="Station Name" />}
                    MenuProps={MenuProps}
                    >
                    {countries.map((country) => (
                        <MenuItem
                        key={country}
                        value={country}
                        style={getCountries(country, countryName, theme)}
                        >
                        {country}
                        </MenuItem>
                    ))}
                    </Select>
                </FormControl>
            </Grid>
            <Grid item xs={4}>
                <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350}}>
                    <InputLabel id="group-name-label">Company Name</InputLabel>
                    <Select
                    labelId="group-name-label"
                    id="group-name"
                    value={stationName}
                    onChange={handleChangeStation}
                    input={<OutlinedInput label=" Company Name" />}
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
            <Grid item xs={4}>
                <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350}}>
                    <InputLabel id="group-name-label">City</InputLabel>
                    <Select
                    labelId="group-name-label"
                    id="group-name"
                    value={packageName}
                    onChange={handleChangePackage}
                    input={<OutlinedInput label="City" />}
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
