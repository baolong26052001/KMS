import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router

//import css
import './slideshow.css';
import Filter from './Filter';



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

function createData(id, packageName, imgsrc, fileType, startDate, endDate) {
  return {id, packageName, imgsrc, fileType, startDate, endDate};
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
  { field: 'id', headerName: 'Package ID', minWidth: 100, flex: 1,},
  { field: 'packageName', headerName: 'Package Name', minWidth: 250, flex: 1,},
  { field: 'imgsrc', headerName: 'Image/Video', minWidth: 100, flex: 1,},
  { field: 'fileType', headerName: 'File Type', minWidth: 120, flex: 1,},
  {
    field: 'startDate',
    headerName: 'Start Date',
    minWidth: 200,
    flex: 1,
  },
  {
    field: 'endDate',
    headerName: 'End Date',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
];

const rows = [
  // createData(1, 'Ads Promotion', 'image.png', 'IMAGE', '19-10-2023 14:00:00', '19-12-2023 14:00:00'),
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const Slideshow = () => {
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
    // Get id from Database  
    const getRowId = (row) => row.id;
    // Get Back-end API URL to connect
    const API_URL = "https://localhost:7017/";
  
  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/Slideshow/ShowSlideshow`);
        const data = await response.json();
  
        // Combine fetched data with createData function
        const updatedRows = data.map((row) =>
          createData(row.id, row.packageName, row.imagevideo, row.fileType, row.startDate, row.endDate)
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
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Slide Show Setup</h1>
        </div>
            <div className="bigcarddashboard">

              <div className='Filter'>
                <Filter />
              </div>
                <div className="searchdivuser">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
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

export default Slideshow;