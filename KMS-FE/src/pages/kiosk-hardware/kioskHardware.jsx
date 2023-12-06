import React, { useState, useEffect } from 'react';
import {EyeOutlined, PlusOutlined, DeleteOutlined } from '@ant-design/icons';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';

//import css
import './kiosk-hardware.css';

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

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

function createData(id, memory, ipAddress, osName, osPlatform, osVersion) {
  return {id, memory, ipAddress, osName, osPlatform, osVersion};
};

const columns = [
  {
    field: 'permissionButton',
    headerName: '',
    width: 80,
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
  { field: 'id', headerName: 'Kiosk ID', minWidth: 70, flex: 1, },
  { field: 'memory', headerName: 'Availble Memory', minWidth: 140, flex: 1, },
  { field: 'ipAddress', headerName: 'Lan IP Address', minWidth: 140, flex: 1, },
  {
    field: 'osName',
    headerName: 'OS Name',
    sortable: false,
    minWidth: 120,
    flex: 1,
  },
  {
    field: 'osPlatform',
    headerName: 'OS Platform',
    sortable: false,
    minWidth: 120,
    flex: 1,
  },
  {
    field: 'osVersion',
    headerName: 'OS Version',
    sortable: false,
    minWidth: 300,
    flex: 1,
  },
];

const rows = [];

const KioskHardware = () => {
  const [searchTerm, setSearchTerm] = useState('');

  const [searchTermButton, setSearchTermButton] = useState('');

  const handleSearchButton = () => {
      setSearchTerm(searchTermButton);
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      handleSearchButton();
    }
  };

  const [rows, setRows] = useState([]);

  // Get Back-end API URL to connect
  const API_URL = "https://localhost:7017/";

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/Kiosk/ShowKioskHardware`);
        const data = await response.json();
  
        // Combine fetched data with createData function
        const updatedRows = data.map((row) =>
          createData(row.id, row.availableMemory + " GB", row.ipAddress, row.OSName, row.OSPlatform, row.OSVersion)
        );
  
        // If searchTerm is empty, display all rows, otherwise filter based on the search term
        const filteredRows = searchTerm
          ? updatedRows.filter((row) =>
              Object.values(row).some((value) =>
                value.toString().toLowerCase().includes(searchTerm.toLowerCase())
              )
            )
          : updatedRows;
  
        setRows(filteredRows); // Update the component state with the data
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }
  
    fetchData();
  }, [searchTerm]);

  return (
    
    <div class="content"> 

        <div class="admin-dashboard-text-div pt-5"> 
            <h1 class="h1-dashboard">Kiosk Hardware</h1>
        </div>
            <div class="bigcarddashboard">
                <div class="statusandimage">
                    
                </div>
                
                <div class="searchdivuser">

                
                    <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                    
                    <input onClick={handleSearchButton} type="button" value="Search" class="button button-search"></input>
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

export default KioskHardware;