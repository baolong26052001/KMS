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

const getRowUsername = (row) => row.userName;

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

function createData(id, userName, email, userGroup, isActive, lastLogin, totalDaysDormant) {
  return {id, userName, email, userGroup, isActive, lastLogin, totalDaysDormant };
};

const columns = [
  {
    field: 'viewButton',
    headerName: '',
    width: 80,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <ViewButton
        rowId={params.row.id}
        label="View"
        onClick={() => handleButtonClick(params.row.id)}
      />
    ),
  },
  {
    field: 'editButton',
    headerName: '',
    width: 80,
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
  { field: 'id', headerName: 'Group ID', minWidth: 100, },
  { field: 'fullname', headerName: 'Full Name', minWidth: 120, },
  { field: 'email', headerName: 'Email', minWidth: 200,},
  {
    field: 'groupName',
    headerName: 'Group Name',
    sortable: false,
    minWidth: 120,
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    sortable: false,
    minWidth: 100,
    valueFormatter: (params) => (params.value ? 'Yes' : 'No'),
  },
  {
    field: 'lastLogin',
    headerName: 'Last Login',
    sortable: false,
    minWidth: 180,
  },
  {
    field: 'TotalDaysDormant',
    headerName: 'Total Days Dormant',
    sortable: false,
    minWidth: 150,
  },
];

const rows = [
  createData(1, 'Richard Nixon', 'richnix@gmail.com', 'Administration', 'Yes', '24-12-2023 14:32:43', '10'),
  createData(2, 'Riadxon', 'ricx@gmail.com', 'Support', 'Yes', '24-12-2023 14:32:43', '10'),
  createData(3, 'Richard Nixn', 'nix@gmail.com', 'Monitor', 'Yes', '24-12-2023 14:32:43', '10'),
  createData(4, 'Tayos', 'tayos@gmail.com', 'Manager', 'Yes', '24-12-2023 14:32:43', '10'), 
  createData(5, 'Chad', 'Chadman@gmail.com', 'Support', 'Yes', '24-12-2023 14:32:43', '10'), 
  createData(6, 'Stein', 'stein@gmail.com', 'Administration', 'Yes', '24-12-2023 14:32:43', '10'), 
  createData(7, 'Lloyd', 'lloyd@gmail.com', 'Administration', 'Yes', '24-12-2023 14:32:43', '10'), 
  createData(8, 'Tessta', 'tessta@gmail.com', 'Administration', 'Yes', '24-12-2023 14:32:43', '10'), 
  createData(9, 'Carena', 'carena@gmail.com', 'Administration', 'Yes', '24-12-2023 14:32:43', '10'), 
  createData(10, 'Wals', 'wals@gmail.com', 'Administration', 'Yes', '24-12-2023 14:32:43', '10'),  
];


const handleButtonClick = (id) => {
   console.log(`Button clicked for row with ID: ${id}`);
};

const User = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedRows, setSelectedRows] = useState([]);
    const [selectAllChecked, setSelectAllChecked] = useState(false);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });

    const [searchTermButton, setSearchTermButton] = useState('');
    const [rows, setRows] = useState([]);

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };

    // const history = useHistory();

    // const handleButtonClick = (id) => {
    //   // Navigate to another page using React Router
    //   history.push(``);
    // };

  const getRowId = (row) => row.id;

  const API_URL = "http://localhost:5000/";
  console.log(`${getRowId}`);
  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/User/ShowUsers`);
        const data = await response.json();
        setRows(data); // Update the component state with the fetched data
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }

    fetchData();
  }, []); // Empty dependency array to run the effect once when the component mounts


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

export default User;