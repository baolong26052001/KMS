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

const names = [
  'Oliver Hansen',
  'Van Henry',
  'April Tucker',
  'Ralph Hubbard',
  'Omar Alexander',
  'Carlos Abbott',
  'Miriam Wagner',
  'Bradley Wilkerson',
  'Virginia Andrews',
  'Kelly Snyder',
];

const statuss = [
    'Yes',
    'No',
];

function getStyles(name, personName, theme) {
  return {
    fontWeight:
      personName.indexOf(name) === -1
        ? theme.typography.fontWeightRegular
        : theme.typography.fontWeightMedium,
  };
}

function getStatus(status, isActive, theme) {
    return {
      fontWeight:
      isActive.indexOf(status) === -1
          ? theme.typography.fontWeightRegular
          : theme.typography.fontWeightMedium,
    };
  }

export default function UserFilter() {
  const theme = useTheme();
  const [personName, setPersonName] = React.useState([]);

  const [isActive, setActive] = React.useState([]);

  const handleChange = (event) => {
    const {
      target: { value },
    } = event;
    setPersonName(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };

  const handleChangeStatus = (event) => {
    const {
      target: { value },
    } = event;
    setActive(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };

  return (
    <div>

        <Grid container spacing={5}>
            <Grid item xs={4}>
                <FormControl fullWidth sx={{ mb: 4, mt: 2, minWidth: 350}}>
                    <InputLabel id="group-name-label">Group Name</InputLabel>
                    <Select
                    labelId="group-name-label"
                    id="group-name"
                    multiple
                    value={personName}
                    onChange={handleChange}
                    input={<OutlinedInput label="Group Name" />}
                    MenuProps={MenuProps}
                    >
                    {names.map((name) => (
                        <MenuItem
                        key={name}
                        value={name}
                        style={getStyles(name, personName, theme)}
                        >
                        {name}
                        </MenuItem>
                    ))}
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
                    onChange={handleChangeStatus}
                    input={<OutlinedInput label="Active" />}
                    MenuProps={MenuProps}
                    >
                    {statuss.map((status) => (
                        <MenuItem
                        key={status}
                        value={status}
                        style={getStyles(status, isActive, theme)}
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
