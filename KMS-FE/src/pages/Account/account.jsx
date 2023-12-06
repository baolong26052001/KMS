import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {Routes, Route, useNavigate} from 'react-router-dom';

//import css
import './account.css';
import AccountFilter from './accountFilter';

const ViewAccount = React.lazy(() => import('./viewAccount'));

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/viewAccount/${rowId}`);
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
  //navigate(`/viewAccount/${id}`);
};


function createData(id, memberId, contractId, phoneNumber, department, company, bankName, memberAddress, status, dateCreate) {
  return {id, memberId, contractId, phoneNumber, department, company, bankName, memberAddress, status, dateCreate};
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
  { field: 'contractId', headerName: 'Contract ID', minWidth: 100, flex: 1,},
  { field: 'phoneNumber', headerName: 'Phone Number', minWidth: 150,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'department',
    headerName: 'Department',
    minWidth: 130,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'company',
    headerName: 'Company',
    minWidth: 150,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'bankName',
    headerName: 'Bank',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'memberAddress',
    headerName: 'Member Address',
    minWidth: 250,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
    renderCell: (params) => (
        <div style={{ whiteSpace: 'pre-wrap' }}>{params.value}</div>
    ),
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
    field: 'dateCreate',
    headerName: 'Date Create',
    sortable: false,
    minWidth: 200,
    flex: 1 
  },
];

const rows = [];



const Account = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };

    const [rows, setRows] = useState([]);
    // Get id from Database  
    const getRowId = (row) => row.id;
    // Get Back-end API URL to connect
    const API_URL = "https://localhost:7017/";
  
    useEffect(() => {
      async function fetchData() {
        try {
          const response = await fetch(`${API_URL}api/Member/ShowMember`);
          const data = await response.json();
  
          // Combine fetched data with createData function
          const updatedRows = data.map((row) =>
            createData(row.id, row.memberId, row.contractId, row.phone, row.department, row.companyName, row.bankName, row.address1, row.isActive, row.dateCreated)
          );

          // If searchTerm is empty, display all rows, otherwise filter based on the search term
          const filteredRows = searchTerm
          ? updatedRows.filter((row) =>
              Object.values(row).some((value) =>
                value.toString().toLowerCase().includes(searchTerm.toLowerCase())
              )
            )
          : updatedRows;
  
          setRows(filteredRows); // Update the component state with the combined data
        } catch (error) {
          console.error('Error fetching data:', error);
        }
      }
  
      fetchData();
    }, [searchTerm]);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Account</h1>
        </div>
            <div className="bigcarddashboard">

              <div className='Filter'>
                <AccountFilter />
              </div>
                <div className="searchdivuser">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                    <input onClick={handleSearchButton} type="button" value="Search" className="button button-search"></input>
                </div>

                
                <div className='Table' style={{ height: 400, width: '100%'}}>
                    <DataGrid
                      rows={rows}
                      columns={columns}
                      getRowId={getRowId}
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

export default Account;