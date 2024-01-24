import React, { useState, useEffect } from 'react';
import Table from '@mui/material/Table';
import TableCell from '@mui/material/TableCell';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import { useParams } from 'react-router-dom';

const statusDes = {
  1: "Online", // Status: Online
  0: 'Offline', // Status: Offline
  2: 'NoPaper', // Status: NoPaper
  3: 'PaperLow', // Status: PaperLow
};

export default function ViewKioskDetails() {

  const { id } = useParams();
  const [Details, setDetails] = useState({});

  // Get Back-end API URL to connect
  const API_URL = "https://localhost:7017/";

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/Kiosk/ShowKioskSetup/${id}`);
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
      <h1 className="h1-dashboard">View Kiosk Details</h1>
    </div>
    <div className="bigcarddashboard">
      <div className="App">
        <div className='table-container'>
          <Table className='custom-table'>
            <TableBody>
              {Object.entries(Details).map(([key, value]) => (
                <TableRow className='row-style' key={key}>
                  <TableCell className='cell-head'>{key.toUpperCase()}</TableCell>
                  <TableCell className='cell-body'>
                  {key !== 'id' && statusDes.hasOwnProperty(value) ? statusDes[value] : (typeof value === 'boolean' ? value.toString() : value)}
                  </TableCell>
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
