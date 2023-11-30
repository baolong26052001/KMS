import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';
import styled from '@emotion/styled';
// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router

//import Filter
import LFilter from './LFilter';




const ViewButton = ({ rowId, label, onClick }) => {
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
  };

  return (
    <Box sx={{alignItems: 'center' }}>
      <Button size="small" variant="contained" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const DetailButton = ({ rowId, label, onClick }) => {
    const handleClick = (event) => {
      event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
      onClick(rowId);
    };
  
    return (
      <Box sx={{alignItems: 'center' }}>
        <Button size="small" variant="contained" onClick={handleClick} 
                sx={{
                    backgroundColor: 'gray', // Set the background color to gray
                    borderRadius: '10px',
                    '&:hover': {
                      backgroundColor: '#999', // Change the background color on hover
                    },
                  }}>
          {label}
        </Button>
      </Box>
    );
  };



function createData(id, kioskName, transactionId, memberID, station, transactionType, kioskRemainingMoney, transactionStatus, transactionDate) {
  return {id, kioskName, transactionId, memberID, station, transactionType, kioskRemainingMoney, transactionStatus, transactionDate};
}

const columns = [
  {
    field: 'viewButton',
    headerName: '',
    width: 80,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <ViewButton
        rowId={params.row.id}
        label="View"
        onClick={handleButtonClick}
      />
    ),
  },
  {
    field: 'detailButton',
    headerName: '',
    width: 170,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <DetailButton
        rowId={params.row.id}
        label="Transaction Details"
        onClick={handleButtonClick}
      />
    ),
  },
  {
    field: 'transactionDate',
    headerName: 'Transaction Date',
    sortable: false,
    minWidth: 200, 
    flex: 1,
  },
  { field: 'id', headerName: 'Kiosk ID', minWidth: 100, flex: 1,},
  { field: 'kioskName', headerName: 'Kiosk Name', minWidth: 150, flex: 1,},
  { field: 'memberID', headerName: 'Member ID', minWidth: 100, flex: 1,},
  { field: 'transactionId', headerName: 'Transaction ID', minWidth: 150, flex: 1,},
  {
    field: 'station',
    headerName: 'Station',
    minWidth: 150,
    flex: 1,
  },
  {
    field: 'transactionType',
    headerName: 'Transaction Type',
    sortable: false,
    minWidth: 170,
    flex: 1,
  },
  {
    field: 'kioskRemainingMoney',
    headerName: 'Kiosk Remaining Money',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
  {
    field: 'transactionStatus',
    headerName: 'Transaction Status',
    sortable: false,
    minWidth: 170,
    flex: 1,
  },
];

const rows = [
    createData(1, 'K001', 121, 15, 'INT - SaiGon', 'Loan', '50.000.000', 'Success', '1-12-2023 14:30:00')
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const TransactionLogs = () => {
    const [searchTerm, setSearchTerm] = useState('');

    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };

//   const [rows, setRows] = useState([]);
//   // Get id from Database  
//   const getRowId = (row) => row.id;
//   // Get Back-end API URL to connect
//   const API_URL = "https://localhost:7017/";

//   useEffect(() => {
//     async function fetchData() {
//       try {
//         const response = await fetch(`${API_URL}api/Kiosk/ShowKioskSetup`);
//         const data = await response.json();

//         // Combine fetched data with createData function
//         const updatedRows = data.map((row) =>
//           createData(row.id, row.kioskName, row.location, row.stationName, row.packageName, row.kioskRemainingMoney, 
//                     row.cameraStatus, row.cashDepositStatus, row.scannerStatus, row.printerStatus)
//         );

//         setRows(updatedRows); // Update the component state with the combined data
//       } catch (error) {
//         console.error('Error fetching data:', error);
//       }
//     }

//     fetchData();
//   }, []);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Transaction Logs</h1>
        </div>
            <div className="bigcarddashboard">

              <div className='Filter'>
                <LFilter />
              </div>
              
                <div className="searchdivuser">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} placeholder=" Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                    <input onClick={handleSearchButton} type="button" value="Search" className="button button-search"></input>
                </div>

                
                <div className='Table' style={{ height: 400, width: '100%'}}>
                    <DataGrid
                      rows={rows}
                      columns={columns}
                    //   getRowId={getRowId}
                      initialState={{
                      pagination: {
                          paginationModel: { page: 0, pageSize: 5 },
                      },
                      }}
                      pageSizeOptions={[5, 10, 25, 50]}
                      checkboxSelection
                    />
                </div>
            </div>

        
    </div>
    
    
  )
}

export default TransactionLogs;