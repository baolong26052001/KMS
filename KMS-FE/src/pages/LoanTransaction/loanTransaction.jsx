import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {Routes, Route, useNavigate} from 'react-router-dom';

//import css
import LoanTransactionFilter from './loanTransactionFilter';



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


function createData(id, memberId, loanId, loanTerm, debt, balance, transactionType, interestRate, isActive, loanDate) {
  return {id, memberId, loanId, loanTerm, debt, balance, transactionType, interestRate, isActive, loanDate};
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
  { field: 'loanId', headerName: 'Loan ID', minWidth: 100, flex: 1,},
  { field: 'loanTerm', headerName: 'Loan Term', minWidth: 150,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'debt',
    headerName: 'Debt',
    minWidth: 100,
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
    field: 'transactionType',
    headerName: 'Transaction Type',
    minWidth: 150,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'interestRate',
    headerName: 'Interest Rate',
    minWidth: 120,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'loanDate',
    headerName: 'Loan Date',
    sortable: false,
    minWidth: 200,
    flex: 1 
  },
];

const rows = [
  createData(1, 15, 9232, '12 Months', '500,000', '2,060,000', 'Loan', '1%', 'Yes' , '19-12-2023 14:00:00'),
  createData(2, 16, 1222, '6 Months', '500,000', '3,000,000', 'Loan', '1%', 'Yes' , '1-4-2023 12:00:00'),
];


const LoanTransaction = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };


  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Loan Transaction</h1>
        </div>
            <div className="bigcarddashboard">

              <div className='Filter'>
                <LoanTransactionFilter />
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

export default LoanTransaction;