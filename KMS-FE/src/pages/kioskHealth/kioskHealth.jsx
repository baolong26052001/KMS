import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {Routes, Route, useNavigate} from 'react-router-dom';

//import Filter
import Filter from './Filter';

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/accountView`);
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
                  '&:hover': {
                    backgroundColor: '#999', // Change the background color on hover
                  },
                }}>
        {label}
      </Button>
    </Box>
  );
};

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
 
};


function createData(id, transactionDate, station, upTime, downTime, lastUpdate) {
  return {id, transactionDate, station, upTime, downTime, lastUpdate};
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
    width: 100,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <DetailButton
        rowId={params.row.id}
        label="Details"
        onClick={handleButtonClick}
      />
    ),
  },
  { field: 'transactionDate', headerName: 'Transaction Date', minWidth: 180, flex: 1,},
  { field: 'id', headerName: 'Kiosk ID', minWidth: 80, flex: 1,},
  { field: 'station', headerName: 'Station ID', minWidth: 150, flex: 1,},
  { field: 'upTime', headerName: 'Duration Uptime (Minutes)', minWidth: 200,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'downTime',
    headerName: 'Duration Downtime (Minutes)',
    minWidth: 230,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'lastUpdate',
    headerName: 'Last Update',
    sortable: false,
    disableColumnMenu: true,
    minWidth: 200,
    flex: 1 
  },
];

const rows = [
  createData(1, '19-12-2023 14:00:00', 'INT - SaiGon', 140, 10, '30-11-2023 14:00:00'),
];


const KioskHealth = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };


  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Kiosk Health</h1>
        </div>
            <div className="bigcarddashboard">

              <div className='Filter'>
                <Filter />
              </div>
                <div className="searchdivuser">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                    <input onClick={handleSearchButton} type="button" value="Search" className="button button-search"></input>
                </div>

                
                <div className='Table' style={{ height: 400, width: '100%'}}>
                    <DataGrid
                      rows={rows}
                      columns={columns}
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

export default KioskHealth;