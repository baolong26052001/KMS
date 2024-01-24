import React, { useState, useEffect } from 'react';
import Table from '@mui/material/Table';
import TableCell from '@mui/material/TableCell';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import { useParams } from 'react-router-dom';

export default function ViewUser() {
  const { id } = useParams();
  const [details, setDetails] = useState({});
  const [errorMessage, setErrorMessage] = useState('');

  // Get Back-end API URL to connect
  const API_URL = "https://localhost:7017/";

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/User/ShowUsers/${id}`);
        console.log(response.status);
        if (response.status === 401) {
          setErrorMessage("You don't have permission");
          return;
        }
        const data = await response.json();
        setDetails(data[0]); // Assuming the API returns an array with one element
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }

    fetchData();
  }, [id]);

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">View User Details</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          {errorMessage ? (
            <p className="error-message">{errorMessage}</p>
          ) : (
            <div className='table-container'>
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