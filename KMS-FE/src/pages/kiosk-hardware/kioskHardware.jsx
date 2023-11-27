import React, { useState, useEffect } from 'react';
import {EyeOutlined, PlusOutlined, DeleteOutlined } from '@ant-design/icons';

//import Sidebar from '../components/sidebar/Sidebar';
import { render } from '@testing-library/react';

import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';

//import css
import './kiosk-hardware.css';

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

const KioskHardware = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedRows, setSelectedRows] = useState([]);
    const [selectAllChecked, setSelectAllChecked] = useState(false);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: 'ascending' });

    const [searchTermButton, setSearchTermButton] = useState('');

    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };





  return (
    
    <div class="content"> 

        <div class="admin-dashboard-text-div pt-5"> 
            <h1 class="h1-dashboard">Kiosk Hardware</h1>
        </div>
            <div class="bigcarddashboard">
                <div class="statusandimage">
                    
                </div>
                
                <div class="searchdivuser">

                
                    <input onChange={(event) => setSearchTermButton(event.target.value)} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                    
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

export default KioskHardware;