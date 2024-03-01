import React, { useState, useEffect } from 'react';
import Table from '@mui/material/Table';
import TableCell from '@mui/material/TableCell';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import { useParams } from 'react-router-dom';
import { Button } from '@mui/material';
import { API_URL } from '../../components/config/apiUrl';

export default function ViewSlideDetail() {
  const { id } = useParams();
  const [details, setDetails] = useState({});
  const [errorMessage, setErrorMessage] = useState('');

  const goBack = () => {
    window.history.back();
  };

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/SlideDetail/ShowSlideDetailInEditPage/${id}`);
        console.log(response.status);
        if (response.status === 401) {
          setErrorMessage("You don't have permission");
          return;
        }
        const data = await response.json();
        setDetails(data[0]);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }

    fetchData();
  }, [id]);

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Slide Details</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          {errorMessage ? (
            <p className="error-message">{errorMessage}</p>
          ) : (
            <div className="table-container">
              <Button variant="contained" color="primary" onClick={goBack} style={{ marginBottom: '10px', float: 'left' }}>
                Go Back
              </Button>
              <Table className='custom-table'>
                <TableBody>
                  {Object.entries(details).map(([key, value]) => (
                    <TableRow className='row-style' key={key}>
                      <TableCell className='cell-head'>{key.toUpperCase()}</TableCell>
                      <TableCell className='cell-body'>{typeof value === 'boolean' ? value.toString() : value}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
