import DeleteIcon from '@mui/icons-material/Delete';
import React from 'react';

export default function DeleteButton() {
  return (
    <Button variant="contained" startIcon={<DeleteIcon />}>
    Delete
    </Button>
  );
}
