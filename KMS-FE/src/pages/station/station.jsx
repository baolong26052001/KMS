import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router

//import css
import './station.css';
import Filter from './Filter.jsx';



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

const EditButton = ({ rowId, label, onClick }) => {
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
  };

  return (
    <Box sx={{alignItems: 'center'}}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

function createData(id, stationName, companyName, city, address, isActive) {
  return {id, stationName, companyName, city, address, isActive};
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
    field: 'editButton',
    headerName: '',
    width: 80,
    disableColumnMenu: true,
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
  { field: 'id', headerName: 'Station ID', minWidth: 100, },
  { field: 'stationName', headerName: 'Station Name', minWidth: 150, },
  { field: 'companyName', headerName: 'Company Name', minWidth: 150, },
  { field: 'city', headerName: 'City', minWidth: 120,},
  {
    field: 'address',
    headerName: 'Address',
    minWidth: 350,
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    sortable: false,
    minWidth: 100,
    valueFormatter: (params) => (params.value ? 'Yes' : 'No'),
  },
];

const rows = [
  createData(1, 'INT - SaiGon', 'Intel', 'Sai Gon', 'Hi-Tech Park, Lô I2, Đ. D1, Phường Tân Phú, Quận 9, Thành phố Hồ Chí Minh, Vietnam', 'Yes'),
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const Station = () => {
    const [searchTerm, setSearchTerm] = useState('');
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
        const response = await fetch(`${API_URL}api/Station/ShowStation`);
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
            <h1 className="h1-dashboard">Station</h1>
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

export default Station;