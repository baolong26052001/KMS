import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import {useNavigate} from 'react-router-dom';
import DateFilter from '../../components/dateFilter/DateFilter';
import CustomButton from '../../components/CustomButton/customButton';
// Enable the customParseFormat plugin
dayjs.extend(customParseFormat);
dayjs.locale('en'); // Set the locale to English

const handleButtonClick = (id) => {
  console.log(`Button clicked for row with ID: ${id}`);
};

function createData(id, transactionDate, station, upTime, downTime, lastUpdate) {
  return {id, transactionDate, station, upTime, downTime, lastUpdate};
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
      <CustomButton
        rowId={params.row.id}
        label="View"
        onClick={handleButtonClick}
        //destination={`/viewStation/${params.row.id}`}
        color="primary"
        variant="contained"
        size="small"
      />
    ),
  },
  {
    field: 'detailButton',
    headerName: '',
    width: 90,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
      <CustomButton
        rowId={params.row.id}
        label="Details"
        onClick={handleButtonClick}
        destination={`/viewStation/${params.row.id}`}
        color="info"
        variant="contained"
        size="small"
      />
    ),
  },
  { field: 'transactionDate', headerName: 'Transaction Date', minWidth: 180, flex: 1,},
  { field: 'id', headerName: 'Kiosk ID', minWidth: 80, flex: 1,},
  { field: 'station', headerName: 'Station Code', minWidth: 150, flex: 1,},
  { field: 'upTime', headerName: 'Duration Uptime (Minutes)', minWidth: 200,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'downTime',
    headerName: 'Duration Downtime (Minutes)',
    minWidth: 230,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'lastUpdate',
    headerName: 'Last Update',
    sortable: false,
    disableColumnMenu: true,
    minWidth: 200,
    flex: 1 
  },
];
const rows = [];



const KioskHealth = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchTermButton, setSearchTermButton] = useState('');

  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [rows, setRows] = useState([]);

  const handleStartDateChange = (date) => {
    setStartDate(date);
  };

  const handleEndDateChange = (date) => {
    setEndDate(date);
  };

  const getRowId = (row) => row.id;
  const API_URL = "https://localhost:7017/";

  const handleSearchButton = () => {
    setSearchTerm(searchTermButton);
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      handleSearchButton();
    }
  };
  
  useEffect(() => {
    async function fetchData() {
      try {
        let apiUrl = `${API_URL}api/Kiosk/ShowKioskHealth`;
        let searchApi = ``;
  
        if (startDate || endDate) {
          apiUrl = `${API_URL}api/Kiosk/FilterKioskHealth?startDate=${encodeURIComponent(dayjs(startDate).format('YYYY/MM/DD'))}&endDate=${encodeURIComponent(dayjs(endDate).format('YYYY/MM/DD'))}`;
          if (searchTerm) {
            searchApi = `${API_URL}api/Kiosk/SearchKioskHealth?searchQuery=${encodeURIComponent(searchTerm)}`;
          }
        } else if (searchTerm) {
          apiUrl = `${API_URL}api/Kiosk/SearchKioskHealth?searchQuery=${encodeURIComponent(searchTerm)}`;
        }
  
        const [apiResponse, searchApiResponse] = await Promise.all([
          fetch(apiUrl),
          searchApi ? fetch(searchApi) : Promise.resolve(null),
        ]);
  
        if (!apiResponse.ok) {
          throw new Error(`HTTP error! Status: ${apiResponse.status}`);
        }
  
        const apiResponseData = await apiResponse.json();
        const searchApiResponseData = searchApiResponse ? await searchApiResponse.json() : null;
        
        console.log(searchApiResponseData);
  
        if (Array.isArray(apiResponseData)) {
          let filteredRows = apiResponseData;
        
          if (searchApiResponseData && Array.isArray(searchApiResponseData)) {
            filteredRows = apiResponseData.filter(row =>
              searchApiResponseData.some(searchRow => row.id === searchRow.id)
            );
          }
        
          const updatedRows = filteredRows.map(row =>
            createData(row.id, row.dateCreated, row.stationCode, row.durationUptime, row.durationDowntime, row.lastUpdated)
          );
        
          setRows(updatedRows); 
        } else {
          console.error('Invalid data structure:', apiResponseData);
        }
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }
  
    fetchData();
  }, [searchTerm, startDate, endDate]);
  
  
return (
  
  <div className="content"> 

      <div className="admin-dashboard-text-div pt-5"> 
          <h1 className="h1-dashboard">Kiosk Health</h1>
      </div>
          <div className="bigcarddashboard">

          <div className="Filter">
          <DateFilter
            startDate={startDate}
            endDate={endDate}
            handleStartDateChange={handleStartDateChange}
            handleEndDateChange={handleEndDateChange}
          />
          </div>
              <div className="searchdivuser">
                  <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                  <input onClick={() => {handleSearchButton()}} type="button" value="Search" className="button button-search"></input>
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

export default KioskHealth;