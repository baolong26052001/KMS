import React, { useState, useEffect } from 'react';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import components from MUI
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router

//import css
import './kiosk-setup.css';
import KioskFilter from './kioskFilter';
import styled from '@emotion/styled';



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

// render when hover on kiosk icon
const KioskHover = () => {
  return (
    <Tooltip title="Kiosk Status">
        <img
          src={require('../../images/totalkiosk.png')}
          style={{ height: '30px', width: '30px' }}
        />
    </Tooltip>
  );
}



function createData(id, kioskName, country, station, slidePackage, kioskStatus, camStatus, cashDepositStatus, scannerStatus) {
  return {id, kioskName, country, station, slidePackage, kioskStatus };
}

const columns = [
  {
    field: 'permissionButton',
    headerName: '',
    minWidth: 100,
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
    minWidth: 100,
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
  { field: 'id', headerName: 'Kiosk ID', minWidth: 100, },
  { field: 'kioskName', headerName: 'Kiosk Name', minWidth: 100, },
  { field: 'country', headerName: 'Country', minWidth: 100,},
  {
    field: 'station',
    headerName: 'Station',
    minWidth: 120,
  },
  {
    field: 'slidePackage',
    headerName: 'Slide Show Package',
    sortable: false,
    minWidth: 200,
  },
  {
    field: 'kioskStatus',
    headerName: 'Kiosk Status',
    sortable: false,
    minWidth: 100,
    disableColumnFilter: false,
    renderHeader: () => (
      <KioskHover/>
    ),
  },
];

const rows = [
  createData(1, 'Kiosk 1', 'VietNam', 'Sai Gon', 'Ads Promotion'),
  createData(2, 'Kiosk 2', 'VietNam', 'Sai Gon', 'Ads Promotion'),
  createData(3, 'Kiosk 3', 'VietNam', 'Ha Noi', 'Ads Promotion'),
  createData(4, 'Kiosk 4', 'VietNam', 'Da Nang', 'Ads Promotion'),
  createData(5, 'Kiosk 5', 'VietNam', 'Nha Trang', 'Ads Promotion'),
  createData(6, 'Kiosk 6', 'VietNam', 'Nha Trang', 'Ads Promotion'),
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const KioskSetup = () => {
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
            <h1 className="h1-dashboard">Kiosk Setup</h1>
        </div>
            <div className="bigcarddashboard">

              <div className='Filter'>
                <KioskFilter />
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

export default KioskSetup;