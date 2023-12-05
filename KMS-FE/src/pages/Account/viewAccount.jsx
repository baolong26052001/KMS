import React, { useState, useEffect } from 'react';
import Table from '@mui/material/Table';
import TableCell, { tableCellClasses } from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';
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
        <h1 className="h1-dashboard">View Account</h1>
      </div>
      <div className="bigcarddashboard">
          <div className="App">
            <div className='table-container'>
            {/* <Table>      
                <TableRow className='row-style'>
                    <TableCell variant="head">Account ID</TableCell>
                    <TableCell>1</TableCell>
                </TableRow>     
                <TableRow  className='row-style'>
                    <TableCell variant="head">Member ID</TableCell>
                    <TableCell>15</TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Contract ID</TableCell>
                    <TableCell>3462</TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Phone Number</TableCell>
                    <TableCell></TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Department</TableCell>
                    <TableCell>HR</TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Company</TableCell>
                    <TableCell>AHQ</TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Bank</TableCell>
                    <TableCell>VCB</TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Member Address</TableCell>
                    <TableCell>1 Đ. Tôn Đức Thắng, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh</TableCell>
                </TableRow>  
                <TableRow  className='row-style'>
                    <TableCell variant="head">Is Active</TableCell>
                    <TableCell>Yes</TableCell>
                </TableRow>    
                <TableRow  className='row-style'>
                    <TableCell variant="head">Date Create</TableCell>
                    <TableCell>19-12-2023 14:00:00</TableCell>
                </TableRow>       
            </Table> */}
            <Table>
              {/* Render rows using accountDetails */}
              {Object.entries(accountDetails).map(([key, value]) => (
                <TableRow className='row-style' key={key}>
                  <TableCell variant="head">{key}</TableCell>
                  <TableCell>{typeof value === 'boolean' ? value.toString() : value}</TableCell>
                </TableRow>
              ))}
            </Table>
            </div>
        </div>
      </div>
    </div>
  );
}
