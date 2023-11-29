import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {Routes, Route, useNavigate} from 'react-router-dom';

//import css
import Filter from './sFilter';



const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    // navigate(`/accountView`);
  };

  return (
    <Box sx={{alignItems: 'center' }}>
      <Button size="small" variant="contained" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
 
};


function createData(id, memberId, savingId, savingTerm, balance, annualRate, status, dueDate, dateCreated) {
  return {id, memberId, savingId, savingTerm, balance, annualRate, status, dueDate, dateCreated};
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
  { field: 'id', headerName: 'Account ID', minWidth: 100, flex: 1,},
  { field: 'memberId', headerName: 'Member ID', minWidth: 100, flex: 1,},
  { field: 'savingId', headerName: 'Saving ID', minWidth: 100, flex: 1,},
  { field: 'savingTerm', headerName: 'Saving Term', minWidth: 150,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'balance',
    headerName: 'Balance',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'annualRate',
    headerName: 'Annual Rate',
    minWidth: 120,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'status',
    headerName: 'Is Active',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'dueDate',
    headerName: 'Due Created',
    sortable: false,
    minWidth: 180,
    flex: 1 
  },
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 180,
    flex: 1 
  },
];

const rows = [
  createData(1, 15, 9232, '12 Months', '2,060,000', '1%', 'Yes' , '19-12-2024 14:00:00', '19-12-2023 14:00:00'),
];


const SavingStatement = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };


  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Saving Statement</h1>
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

export default SavingStatement;