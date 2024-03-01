import React from 'react';
import { Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const CustomButton = ({ rowId, label, onClick, destination, color = 'warning', variant = 'contained', size = 'small' }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation();
    onClick(rowId);
    navigate(destination);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
      <Button size={size} variant={variant} color={color} onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

export default CustomButton;