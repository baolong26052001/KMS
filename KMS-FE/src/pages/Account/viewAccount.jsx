import React, { useState, useEffect } from 'react';
import Table from '@mui/material/Table';
import TableCell from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';
import TableBody from '@mui/material/TableBody';
import { useParams } from 'react-router-dom';
import './account';


export default function ViewAccount() {

  const { id } = useParams();
  const [accountDetails, setAccountDetails] = useState({});

  // Get Back-end API URL to connect
  const API_URL = "https://localhost:7017/";

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/Member/ShowMember/${id}`);
        const data = await response.json();
        setAccountDetails(data[0]); // Assuming the API returns an array with one element
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }

    fetchData();
  }, [id]);

  return (
  <div className="content">
    <div className="admin-dashboard-text-div pt-5">
      <h1 className="h1-dashboard">View Account Details</h1>
    </div>
    <div className="bigcarddashboard">
      <div className="App">
        <div className='table-container'>
          <Table className='custom-table'>
            <TableBody>
              {/* Render rows using accountDetails */}
              {Object.entries(accountDetails).map(([key, value]) => (
                <TableRow className='row-style' key={key}>
                  <TableCell className='cell-head'>{key.toUpperCase()}</TableCell>
                  <TableCell className='cell-body'>{typeof value === 'boolean' ? value.toString() : value}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>
      </div>
    </div>
  </div>
  );
}
