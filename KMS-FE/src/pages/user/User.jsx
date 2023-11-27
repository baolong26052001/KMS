import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router

//import css
import './User.css';
import { Flex } from 'antd';



const ViewButton = ({ rowId, label, onClick }) => {
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0 }}>
      <Button size="small" variant="contained" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const EditButton = ({ rowId, label, onClick }) => {
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0 }}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const columns = [
  {
    field: 'viewButton',
    headerName: '',
    minWidth: 70,
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
    field: 'editButton',
    headerName: '',
    minWidth: 70, 
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <EditButton
        rowId={params.row.id}
        label="Edit"
        onClick={handleButtonClick}
      />
    ),
  },
  { field: 'id', headerName: 'User ID', 
    minWidth: 100,
},
  { field: 'userName', headerName: 'User Name', 
    minWidth: 150, 
  },
  { field: 'email', headerName: 'Email', 
    minWidth: 200,
  },
  {
    field: 'userGroup',
    headerName: 'User Group',
    sortable: false,
    minWidth: 150,
  },
  {
    field: 'active',
    headerName: 'Active',
    sortable: false,
    minWidth: 100, 
  },
  {
    field: 'lastLogin',
    headerName: 'Last Login',
    sortable: false,
    minWidth: 200,
  },
  {
    field: 'daysDormant',
    headerName: 'Total Days Dormant',
    sortable: false,
    minWidth: 200,
   },
];

const rows = [
  { id: 1, userName: 'Richard Nixon', email: 'richnix@gmail.com', userGroup: 'Administration', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 2, userName: 'Riadxon', email: 'ricx@gmail.com', userGroup: 'Support', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 3, userName: 'Nixn', email: 'nix@gmail.com', userGroup: 'Monitor', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 4, userName: 'Tayos', email: 'tayos@gmail.com', userGroup: 'Support', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 5, userName: 'Chad', email: 'Chadman@gmail.com', userGroup: 'Manager', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 6, userName: 'Stein', email: 'stein@gmail.com', userGroup: 'Support', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 7, userName: 'Lloyd', email: 'lloyd@gmail.com', userGroup: 'Administration', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 8, userName: 'Tessta', email: 'tessta@gmail.com', userGroup: 'Administration', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 9, userName: 'Carena', email: 'carena@gmail.com', userGroup: 'Administration', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
  { id: 10, userName: 'Wals', email: 'wals@gmail.com', userGroup: 'Administration', active: 'Yes', lastLogin: '24-12-2023 14:32:43', daysDormant: '10' },
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const User = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedRows, setSelectedRows] = useState([]);
    const [selectAllChecked, setSelectAllChecked] = useState(false);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });

    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };

    // const history = useHistory();

    // const handleButtonClick = (id) => {
    //   // Navigate to another page using React Router
    //   history.push(``);
    // };

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">User</h1>
        </div>
            <div className="bigcarddashboard">
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

export default User;