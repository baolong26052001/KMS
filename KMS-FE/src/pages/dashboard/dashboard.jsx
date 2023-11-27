import React, { useState, useEffect } from 'react';
import { AppstoreOutlined, MailOutlined, SettingOutlined } from '@ant-design/icons';
//import { Col, Menu, Switch } from 'antd';
import DataTable from 'react-data-table-component';

import Accordion from 'react-bootstrap/Accordion';
//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

// import css
import './dashboard.css';

// import react bootstrap
import { Container } from 'react-bootstrap';

// import components MUI
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import Typography from '@mui/material/Typography';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import ButtonBase from '@mui/material/ButtonBase';
import { styled, withStyles } from '@mui/material/styles';
import Stack from '@mui/material/Stack';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { auto } from '@popperjs/core';


const columns = [
    { field: 'id', headerName: 'Kiosk ID', flex: 1 },
    { field: 'kioskName', headerName: 'Kiosk Name', flex: 1 },
    { field: 'stationCode', headerName: 'Station', flex: 1 },
    {
      field: 'kioskStatus',
      headerName: 'Kiosk Heart Beat Update',
      sortable: false,
    //   type: 'number',
        flex: 2,
    },
    {
      field: 'camStatus',
      headerName: 'Camera Last Update',
      sortable: false,
      flex: 2,
    },
    {
        field: 'scannerStatus',
        headerName: 'Scanner Last Update',
        sortable: false,
        flex: 2,
    },
    {
        field: 'cashDeStatus',
        headerName: 'Cash Deposit Last Update',
        sortable: false,
        flex: 2,
     },
  ];

  const rows = [
    { id: 1, kioskName: 'K001', stationCode: 'SaiGon', kioskStatus: '24-12-2023', camStatus: '24-12-2023', scannerStatus: '24-12-2023', cashDeStatus: '24-12-2023' },
    { id: 2, kioskName: 'K002', stationCode: 'SaiGon', kioskStatus: '24-12-2023', camStatus: '24-12-2023', scannerStatus: '24-12-2023', cashDeStatus: '24-12-2023' },
    { id: 3, kioskName: 'K003', stationCode: 'SaiGon', kioskStatus: '24-12-2023', camStatus: '24-12-2023', scannerStatus: '24-12-2023', cashDeStatus: '24-12-2023' },
    { id: 4, kioskName: 'K004', stationCode: 'SaiGon', kioskStatus: '24-12-2023', camStatus: '24-12-2023', scannerStatus: '24-12-2023', cashDeStatus: '24-12-2023' },
    { id: 5, kioskName: 'K005', stationCode: 'Ha Noi', kioskStatus: '24-12-2023', camStatus: '24-12-2023', scannerStatus: '24-12-2023', cashDeStatus: '24-12-2023' },
    { id: 6, kioskName: 'K006', stationCode: 'Binh Duong', kioskStatus: '24-12-2023', camStatus: '24-12-2023', scannerStatus: '24-12-2023', cashDeStatus: '24-12-2023' },

  ];

const Dashboard = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedRows, setSelectedRows] = useState([]);
    const [selectAllChecked, setSelectAllChecked] = useState(false);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });

    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };

    const renderSortIcon = (columnKey) => {
        if (sortConfig && sortConfig.key === columnKey) {
          return sortConfig.direction === 'ascending' ? <span class="arrow">&#9660;</span> : <span class="arrow">&#9650;</span>;
        }
        return sortConfig.direction === 'ascending' ? <span class="arrow">&#9660;</span> : <span class="arrow">&#9650;</span>;;
    };
      



    // const data = [

    //     { 
    //         kioskName: 'Kiosk 123', 
    //         kioskID: 123, 
    //         stationCode: 258, 
    //         heartbeatUpdate: '24-01-2023', 
    //         cameraUpdate: '24-01-2023', 
    //         scannerUpdate: '24-01-2023', 
    //         cashDepositUpdate: '24-01-2023'
    //     },

    //     { 
    //         kioskName: 'Kiosk 124', 
    //         kioskID: 124, 
    //         stationCode: 258, 
    //         heartbeatUpdate: '24-01-2023', 
    //         cameraUpdate: '24-01-2023', 
    //         scannerUpdate: '24-01-2023', 
    //         cashDepositUpdate: '24-01-2023' 
    //     },

    //     { 
    //         kioskName: 'Kiosk 125', 
    //         kioskID: 125, 
    //         stationCode: 258, 
    //         heartbeatUpdate: '24-01-2023', 
    //         cameraUpdate: '24-01-2023', 
    //         scannerUpdate: '24-01-2023', 
    //         cashDepositUpdate: '24-01-2023' 
    //     },
        
    //     { 
    //         kioskName: 'Kiosk 126', 
    //         kioskID: 126, 
    //         stationCode: 3213, 
    //         heartbeatUpdate: '24-01-2023', 
    //         cameraUpdate: '24-01-2023', 
    //         scannerUpdate: '24-01-2023', 
    //         cashDepositUpdate: '24-01-2023' 
    //     },

    //     { 
    //         kioskName: 'Kiosk 127', 
    //         kioskID: 127, 
    //         stationCode: 3213, 
    //         heartbeatUpdate: '24-01-2023', 
    //         cameraUpdate: '24-01-2023', 
    //         scannerUpdate: '24-01-2023', 
    //         cashDepositUpdate: '24-01-2023' 
    //     },

    //     { 
    //         kioskName: 'Kiosk 128', 
    //         kioskID: 128, 
    //         stationCode: 3213, 
    //         heartbeatUpdate: '24-01-2023', 
    //         cameraUpdate: '24-01-2023', 
    //         scannerUpdate: '24-01-2023', 
    //         cashDepositUpdate: '24-01-2023' 
    //     },

        
    //   ];
      
    //   const handleSearch = (event) => {
    //     setSearchTerm(event.target.value);
    //   };
      
    //   const handleSort = (key) => {
    //     let direction = 'ascending';
    //     if (sortConfig && sortConfig.key === key && sortConfig.direction === 'ascending') {
    //       direction = 'descending';
    //     }
    //     setSortConfig({ key, direction });
    //   };
    
    //   const sortData = (data) => {
    //     if (sortConfig.key) {
    //       const sortedData = [...data];
    //       sortedData.sort((a, b) => {
    //         const keyA = a[sortConfig.key];
    //         const keyB = b[sortConfig.key];
    //         if (keyA < keyB) return sortConfig.direction === 'ascending' ? -1 : 1;
    //         if (keyA > keyB) return sortConfig.direction === 'ascending' ? 1 : -1;
    //         return 0;
    //       });
    //       return sortedData;
    //     }
    //     return data;
    //   };

    //   const filteredData = sortData(
    //     data.filter((item) =>
    //       Object.values(item).some(
    //         (value) =>
    //           value &&
    //           value.toString().toLowerCase().includes(searchTerm.toLowerCase())
    //       )
    //     )
    //   );

  return (
    
    <div class="content"> 

        <div class="admin-dashboard-text-div"> 
            <p>Admin/Dashboard</p>
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
                                    <div className='kiosk-num'>100</div>
                                    <div className='percent' style={{color: '#12E95B'}}>(+18%)</div>
                                </Stack>
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Last Week Analytics
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
                                    <div className='kiosk-num'>99</div>
                                    <div className='percent' style={{color: '#E92323'}}>(-1%)</div>
                                </Stack>
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Last Week Analytics
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
                                    <div className='kiosk-num'>1</div>
                                    <div className='percent' style={{color: '#12E95B'}}>(+1%)</div>
                                </Stack>
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Last Week Analytics
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
                                    <div className='kiosk-num'>100</div>
                                    <div className='percent' style={{color: '#12E95B'}}>(+29%)</div>
                                </Stack>
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Last Week Analytics
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
                    <input onChange={(event) => setSearchTermButton(event.target.value)} placeholder="  Search kiosk ID..." type="text" id="kioskid myInput" name="kioskid" class="searchbar"></input> 
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
                        pageSizeOptions={[5, 10]}
                        checkboxSelection
                    />
                </div>
                
            
            </div>

        
    </div>
    
  )
}

export default Dashboard;