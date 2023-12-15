import React, { useState, useEffect } from 'react';

// import components from MUI
import { DataGrid } from '@mui/x-data-grid';
import { Button, Box, Tooltip } from '@mui/material';

// Import from React Router
import {useNavigate} from 'react-router-dom';
//import css
import './kiosk-setup.css';

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/viewKioskDetails/${rowId}`);
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
    <Tooltip title="Kiosk Status" placement="top">
        <img
          src={require('../../images/totalkiosk.png')}
          style={{ height: '30px', width: '30px' }}
        />
    </Tooltip>
  );
}

// render when hover on kiosk icon
const CamHover = () => {
  return (
    <Tooltip title="Camera Status" placement="top">
        <img
          src={require('../../images/Camera.png')}
          style={{ height: '30px', width: '30px' }}
        />
    </Tooltip>
  );
}

// render when hover on kiosk icon
const CashDekHover = () => {
  return (
    <Tooltip title="Cash Deposit Status" placement="top">
        <img
          src={require('../../images/Category.png')}
          style={{ height: '30px', width: '30px' }}
        />
    </Tooltip>
  );
}

// render when hover on kiosk icon
const ScanHover = () => {
  return (
    <Tooltip title="Scanner Status" placement="top">
        <img
          src={require('../../images/Scanner.png')}
          style={{ height: '30px', width: '30px' }}
        />
    </Tooltip>
  );
}

// render when hover on kiosk icon
const PrinterHover = () => {
  return (
    <Tooltip title="Printer Status" placement="top">
        <img
          src={require('../../images/Printer.png')}
          style={{ height: '30px', width: '30px' }}
        />
    </Tooltip>
  );
}

const statusImages = {
  1: require('../../images/online.png'), // Status: Online
  0: require('../../images/offline.png'), // Status: Offline
  2: require('../../images/nopaper.png'), // Status: NoPaper
  3: require('../../images/paperlow.png'), // Status: PaperLow
};


function createData(id, kioskName, country, station, slidePackage, kioskStatus, camStatus, cashDepositStatus, scannerStatus, printerStatus) {
  return {id, kioskName, country, station, slidePackage, kioskStatus: statusImages[kioskStatus], camStatus: statusImages[camStatus], cashDepositStatus: statusImages[cashDepositStatus], scannerStatus: statusImages[scannerStatus], printerStatus: statusImages[printerStatus] };
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
  { field: 'id', headerName: 'Kiosk ID', minWidth: 100, flex: 1,},
  { field: 'kioskName', headerName: 'Kiosk Name', minWidth: 150, flex: 1, },
  { field: 'country', headerName: 'Country', minWidth: 120, flex: 1,},
  {
    field: 'station',
    headerName: 'Station',
    minWidth: 130,
    flex: 1,
  },
  {
    field: 'slidePackage',
    headerName: 'Slide Show Package',
    sortable: false,
    minWidth: 170,
    flex: 1,
  },
  {
    field: 'kioskStatus',
    headerName: 'Kiosk Status',
    sortable: false,
    disableColumnMenu: true,
    width: 60,
    flex: 1,
    renderHeader: () => (
      <KioskHover/>
    ),
    renderCell: (params) => (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <img
          src={params.row.kioskStatus}
          alt={`Image for ${params.row.kioskStatus}`}
          style={{ height: '15px', width: '15px' }}
        />
      </div>
    ),
  },
  {
    field: 'camStatus',
    headerName: 'Camera Status',
    sortable: false,
    disableColumnMenu: true,
    width: 60,
    flex: 1,
    renderHeader: () => (
      <CamHover/>
    ),
    renderCell: (params) => (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <img
          src={params.row.camStatus}
          alt={`Image for ${params.row.camStatus}`}
          style={{ height: '15px', width: '15px' }}
        />
      </div>
    ),
  },
  {
    field: 'cashDepositStatus',
    headerName: 'Cash Deposit Status',
    sortable: false,
    disableColumnMenu: true,
    width: 60,
    flex: 1,
    renderHeader: () => (
      <CashDekHover/>
    ),
    renderCell: (params) => (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <img
          src={params.row.cashDepositStatus}
          alt={`Image for ${params.row.cashDepositStatus}`}
          style={{ height: '15px', width: '15px' }}
        />
      </div>
    ),
  },
  {
    field: 'scannerStatus',
    headerName: 'Scanner Status',
    sortable: false,
    disableColumnMenu: true,
    width: 60,
    flex: 1,
    renderHeader: () => (
      <ScanHover/>
    ),
    renderCell: (params) => (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <img
          src={params.row.scannerStatus}
          alt={`Image for ${params.row.scannerStatus}`}
          style={{ height: '15px', width: '15px' }}
        />
      </div>
    ),
  },
  {
    field: 'printerStatus',
    headerName: 'Printer Status',
    sortable: false,
    disableColumnMenu: true,
    width: 60,
    flex: 1,
    renderHeader: () => (
      <PrinterHover/>
    ),
    renderCell: (params) => (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <img
          src={params.row.printerStatus}
          alt={`Image for ${params.row.printerStatus}`}
          style={{ height: '15px', width: '15px' }}
        />
      </div>
    ),
  },
];

const rows = [];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const KioskSetup = () => {
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
        let apiUrl = `${API_URL}api/Kiosk/ShowKiosk`;
  
        // If searchTerm is not empty, use the search API endpoint
        if (searchTerm) {
          apiUrl = `${API_URL}api/Kiosk/SearchKioskSetup?searchQuery=${encodeURIComponent(searchTerm)}`;
        }
  
        const response = await fetch(apiUrl);
  
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
  
        const responseData = await response.json();
  
        // Check if responseData is an array before calling map
        if (Array.isArray(responseData)) {
          const updatedRows = responseData.map((row) =>
            createData(
              row.id, row.kioskName, row.location, row.stationName, row.packageName, row.kioskStatus, 
              row.cameraStatus, row.cashDepositStatus, row.scannerStatus, row.printerStatus
            )
          );
  
          setRows(updatedRows); // Update the component state with the combined data
        } else {
          console.error('Invalid data structure:', responseData);
        }
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }
  
    fetchData();
  }, [searchTerm]);
  

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Kiosk Setup</h1>
        </div>
            <div className="bigcarddashboard">
              
                <div className="searchdivuser">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder=" Search..." type="text"  name="Search" class="searchbar"></input>
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

export default KioskSetup;