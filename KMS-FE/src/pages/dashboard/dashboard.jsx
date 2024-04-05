import React, { useState, useEffect } from 'react';

// import css
import './dashboard.css';

// import components MUI
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import ButtonBase from '@mui/material/ButtonBase';
import Stack from '@mui/material/Stack';
import { DataGrid } from '@mui/x-data-grid';
import { API_URL } from '../../components/config/apiUrl';

const statusImages = {
    1: require('../../images/online.png'), // Status: Online
    0: require('../../images/offline.png'), // Status: Offline
    2: require('../../images/nopaper.png'), // Status: NoPaper
    3: require('../../images/paperlow.png'), // Status: PaperLow
  };

  function createData(id, kioskName, stationCode, kioskStatus, camStatus, scannerStatus, cashDeStatus, printerStatus) {
    return {id, kioskName, stationCode, kioskStatus: statusImages[kioskStatus], camStatus: statusImages[camStatus], scannerStatus: statusImages[scannerStatus], cashDeStatus: statusImages[cashDeStatus], printerStatus: statusImages[printerStatus]};
  }
  
  const columns = [
    { field: 'id', headerName: 'Kiosk ID', minWidth: 100, flex: 1 },
    { field: 'kioskName', headerName: 'Kiosk Name', minWidth: 120, flex: 1 },
    { field: 'stationCode', headerName: 'Station', minWidth: 100, flex: 1 },
    {
      field: 'kioskStatus',
      headerName: 'Kiosk Heart Beat Update',
      sortable: false,
      minWidth: 180, 
      flex: 1,
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
      headerName: 'Camera Last Update',
      sortable: false,
      minWidth: 180, 
      flex: 1,
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
        field: 'scannerStatus',
        headerName: 'Scanner Last Update',
        sortable: false,
        minWidth: 180, 
        flex: 1,
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
        field: 'cashDeStatus',
        headerName: 'Cash Deposit Last Update',
        sortable: false,
        minWidth: 190, 
        flex: 1,
        renderCell: (params) => (
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
              <img
                src={params.row.cashDeStatus}
                alt={`Image for ${params.row.cashDeStatus}`}
                style={{ height: '15px', width: '15px' }}
              />
            </div>
          ),
     },
     {
        field: 'printerStatus',
        headerName: 'Printer Last Update',
        sortable: false,
        minWidth: 180, 
        flex: 1,
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

const Dashboard = () => {
  const [totalNumbers, setTotalNumbers] = useState({
    TotalKiosk: 0,
    TotalKioskOnline: 0,
    TotalKioskOffline: 0,
    TotalTransaction: 0
  });

  const fetchTotalNumbers = async () => {
    try {
      const response = await fetch(`${API_URL}api/Dashboard/ShowTotalNumbers`);
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      const data = await response.json();
      // Update the state with total numbers
      setTotalNumbers(data[0]);
    } catch (error) {
      console.error('Error fetching total numbers:', error);
    }
  };

  useEffect(() => {
    fetchTotalNumbers();
  }, []);

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
    
      useEffect(() => {
        async function fetchData() {
          try {
            let apiUrl = `${API_URL}api/Kiosk/ShowKiosk`;
            if (searchTerm) {
              apiUrl = `${API_URL}api/Kiosk/SearchKioskSetup?searchQuery=${encodeURIComponent(searchTerm)}`;
            }
      
            const response = await fetch(apiUrl);
      
            if (!response.ok) {
              throw new Error(`HTTP error! Status: ${response.status}`);
            }
      
            const responseData = await response.json();
      
            if (Array.isArray(responseData)) {
              const updatedRows = responseData.map((row) =>
                createData(
                    row.id, row.kioskName, row.stationName, row.kioskStatus, row.cameraStatus, 
                    row.scannerStatus, row.cashDepositStatus, row.printerStatus
                )
              );
      
              setRows(updatedRows);
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
    
    <div class="content"> 

        <div class="admin-dashboard-text-div">
            <h1 class="h1-dashboard">Dashboard</h1>
        </div>

    <div className='Card'>
        <Grid className='Card-field' container spacing={2}>
            <Grid item xs={3}>
                <Paper
                    sx={{
                        p: 2,
                        margin: 'auto',
                        maxWidth: 300,
                        flexGrow: 1,
                        backgroundColor: (theme) =>
                        theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
                    }}
                    >
                    <Grid container spacing={0}>
                        <Grid className='Card-body' item xs={10} sm container>
                            <Grid item>
                            <Typography gutterBottom variant="subtitle1" component="h5">
                                Total Kiosk
                            </Typography>
                            <Typography variant="body2" gutterBottom>
                                <Stack direction="row" spacing={2}> 
                                    <div className='kiosk-num' style={{color: '#1976d2'}}>{totalNumbers.TotalKiosk}</div>
                                </Stack>
                            </Typography>
                            </Grid>
                        </Grid>
                        <Grid item>
                        <ButtonBase sx={{ width: 128, height: 128 }}>
                            <img className="icon"  src={require('../../images/totalkiosk.png')}></img>
                        </ButtonBase>
                        </Grid>
                    </Grid>
                </Paper>
            </Grid>
            <Grid item xs={3}>
                <Paper
                    sx={{
                        p: 2,
                        margin: 'auto',
                        maxWidth: 300,
                        flexGrow: 1,
                        backgroundColor: (theme) =>
                        theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
                    }}
                    >
                    <Grid container spacing={0}>
                        <Grid className='Card-body' item xs={10} sm container>
                            <Grid item>
                            <Typography gutterBottom variant="subtitle1" component="h5">
                                Total Kiosk Online
                            </Typography>
                            <Typography variant="body2" gutterBottom>
                                <Stack direction="row" spacing={2}> 
                                    <div className='kiosk-num' style={{color: '#12E95B'}}>{totalNumbers.TotalKioskOnline}</div>
                                </Stack>
                            </Typography>
                            </Grid>
                        </Grid>
                        <Grid item>
                        <ButtonBase sx={{ width: 128, height: 128 }}>
                            <img className="icon"  src={require('../../images/kioskonline.png')}></img>
                        </ButtonBase>
                        </Grid>
                    </Grid>
                </Paper>
            </Grid>
            <Grid item xs={3}>
                <Paper
                    sx={{
                        p: 2,
                        margin: 'auto',
                        maxWidth: 300,
                        flexGrow: 1,
                        backgroundColor: (theme) =>
                        theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
                    }}
                    >
                    <Grid container spacing={0}>
                        <Grid className='Card-body' item xs={10} sm container>
                            <Grid item>
                            <Typography gutterBottom variant="subtitle1" component="h5">
                                Total Kiosk Offline
                            </Typography>
                            <Typography variant="body2" gutterBottom>
                                <Stack direction="row" spacing={2}> 
                                    <div className='kiosk-num' style={{color: '#E92323'}}>{totalNumbers.TotalKioskOffline}</div>
                                </Stack>
                            </Typography>
                            </Grid>
                        </Grid>
                        <Grid item>
                        <ButtonBase sx={{ width: 128, height: 128 }}>
                            <img className="icon"  src={require('../../images/kioskoffline.png')}></img>
                        </ButtonBase>
                        </Grid>
                    </Grid>
                </Paper>
            </Grid>
            <Grid item xs={3}>
                <Paper
                    sx={{
                        p: 2,
                        margin: 'auto',
                        maxWidth: 300,
                        flexGrow: 1,
                        backgroundColor: (theme) =>
                        theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
                    }}
                    >
                    <Grid container spacing={0}>
                        <Grid className='Card-body' item xs={10} sm container>
                            <Grid item>
                            <Typography gutterBottom variant="subtitle1" component="h5">
                                Total Transaction
                            </Typography>
                            <Typography variant="body2" gutterBottom>
                                <Stack direction="row" spacing={2}> 
                                    <div className='kiosk-num' style={{color: '#655BD3'}}>{totalNumbers.TotalTransaction}</div>
                                </Stack>
                            </Typography>
                            </Grid>
                        </Grid>
                        <Grid item>
                        <ButtonBase sx={{ width: 128, height: 128 }}>
                            <img className="icon"  src={require('../../images/transaction.png')}></img>
                        </ButtonBase>
                        </Grid>
                    </Grid>
                </Paper>
            </Grid>
        </Grid>
    </div>

        
        
            <div class="bigcarddashboard">
                <div class="statusandimage">
                    <h3 class="kioskstatus">Kiosk Status</h3>
                    <img class="icononline" width="10px" height="10px" src={require('../../images/online.png')}></img>
                    <h5 class="onlinetext">Online</h5>
                    <img class="iconoffline" width="10px" height="10px" src={require('../../images/offline.png')}></img>
                    <h5 class="offlinetext">Offline</h5>
                    <img class="iconpaperlow" width="10px" height="10px" src={require('../../images/paperlow.png')}></img>
                    <h5 class="paperlowtext">Paper Low</h5>
                    <img class="iconnopaper" width="10px" height="10px" src={require('../../images/nopaper.png')}></img>
                    <h5 class="nopapertext">No Paper</h5>
                </div>
                
                <div class="searchdiv">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder="  Search kiosk ID..." type="text" id="kioskid myInput" name="kioskid" class="searchbar"></input> 
                    <input onClick={handleSearchButton} type="button" value="Search" class="button button-search"></input>
                </div>

                <div className='Table' style={{ height: 400, width: '100%'}}>
          <DataGrid
            rows={rows}
            columns={columns}
            
            initialState={{
              pagination: {
                paginationModel: { page: localStorage.getItem('dashboardPage') ? parseInt(localStorage.getItem('dashboardPage')) : 0, pageSize: 5 },
              },
            }}
            components={{
              Toolbar: () => (
                <div style={{ position: 'absolute', bottom: 8, alignItems: 'center', marginLeft: '16px' }}>
                  
                  <div style={{ marginLeft: 'auto' }} />
                </div>
              ),
            }}
            pageSizeOptions={[5, 10, 25, 50]}
            
            
            onStateChange={(state) => {
              const currentPage = state.pagination.paginationModel.page;
              localStorage.setItem('dashboardPage', currentPage);
              console.log('Current Page:', currentPage + 1);
            }}
          />
                </div>
                
            
            </div>

        
    </div>
    
  )
}

export default Dashboard;