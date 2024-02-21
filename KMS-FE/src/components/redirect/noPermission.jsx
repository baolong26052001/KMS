import React from 'react';
import { Box, Container, Typography, Button } from '@mui/material';
import Grid from '@mui/material/Grid';

export default function NoPermission() {
  const goBack = () => {
    window.history.back();
  };

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '90vh'
      }}
    >
      <Container maxWidth="md">
        <Grid container spacing={4} alignItems="center">
          <Grid item xs={12} md={6}>
            <Typography variant="h4" gutterBottom>
              Oops! Access Denied.
            </Typography>
            <Typography variant="body1" paragraph>
              Sorry, you don't have permission to view this page. 
              Please contact your administrator for assistance.
            </Typography>
            <Button variant="contained" color="primary" onClick={goBack}>
              Go Back
            </Button>
          </Grid>
          <Grid item xs={12} md={6}>
            <img
              src={require('../../images/no-permission.png')}
              alt="Access Denied"
              width="100%"
              height="auto"
            />
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
}
