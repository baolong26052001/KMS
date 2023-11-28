import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';


//import css
import './Usergroup.css';
import UserFilter from './userFilter';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';

function createData(id, groupName, dateModified, dateCreated, isActive) {
  return {id, groupName, dateModified, dateCreated, isActive };
}

const ViewButton = ({ rowId, label, onClick }) => {
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
      <Button size="small" variant="contained" color="error" onClick={handleClick}>
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
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};



const columns = [
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
  {
    field: 'permissionButton',
    headerName: '',
    width: 120,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <ViewButton
        rowId={params.row.id}
        label="Permission"
        onClick={handleButtonClick}
      />
    ),
  },
  { field: 'id', headerName: 'Group ID', flex: 1, },
  { field: 'groupName', headerName: 'Group Name', flex: 2, },
  { field: 'dateModified', headerName: 'Date Modified', flex: 2,},
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    flex: 2,
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    sortable: false,
    flex: 1,
    valueFormatter: (params) => (params.value ? 'Yes' : 'No'),
  },
];

const rows = [
  createData(1, 'Administration', '24-12-2023 12:00:00', '19-11-2023 10:23:11', 'Yes'),
  createData(2, 'Support', '24-12-2023 12:00:00', '19-11-2023 10:23:11', 'Yes'),
  createData(3, 'Mornitor', '24-12-2023 12:00:00', '19-11-2023 10:23:11', 'Yes'),
];


const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};


const Usergroup = () => {
    const [searchTerm, setSearchTerm] = useState('');
    // const [selectedRows, setSelectedRows] = useState([]);
    // const [selectAllChecked, setSelectAllChecked] = useState(false);
    // const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });

    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };
  const [rows, setRows] = useState([]);

  const getRowId = (row) => row.id;
  const API_URL = "http://localhost:5000/";






  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/Usergroup/ShowUsergroup`);
        const data = await response.json();
        setRows(data); // Update the component state with the fetched data
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }

    fetchData();
  }, []);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div"> 
            <h1 className="h1-dashboard">User Group</h1>
        </div>
        <div className="bigcarddashboard">

          <div className='Filter'>
            <UserFilter />
          </div>

          <div className="searchdivuser">
              <input onChange={(event) => setSearchTermButton(event.target.value)} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
              <input onClick={handleSearchButton} type="button" value="Search" className="button button-search"></input>
          </div>

          <div className='Table' style={{ height: 400, width: 'auto'}}>
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
                >
              </DataGrid>
          </div>
        </div>
    </div>
    
  )
}

export default Usergroup;