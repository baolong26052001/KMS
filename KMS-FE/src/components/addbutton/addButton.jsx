import AddIcon from '@mui/icons-material/Add';
import React from 'react';

export default function AddButton() {
  return (
    <Button variant="contained" startIcon={<AddIcon />}>
    Add
    </Button>
  );
}
