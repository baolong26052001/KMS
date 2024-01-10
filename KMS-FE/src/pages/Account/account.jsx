import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import {useNavigate} from 'react-router-dom';
import DateFilter from '../../components/dateFilter/DateFilter';

// Enable the customParseFormat plugin
dayjs.extend(customParseFormat);
dayjs.locale('en'); // Set the locale to English

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/viewAccount/${rowId}`);
  };

  return (
    <Box sx={{alignItems: 'center' }}>
      <Button size="small" variant="contained" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
  //navigate(`/viewAccount/${id}`);
};


function createData(id, memberId, contractId, phoneNumber, department, company, bankName, memberAddress, status, dateCreated) {
  return {id, memberId, contractId, phoneNumber, department, company, bankName, memberAddress, status, dateCreated};
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
  { field: 'id', headerName: 'Account ID', minWidth: 100, flex: 1,},
  { field: 'memberId', headerName: 'Member ID', minWidth: 100, flex: 1,},
  { field: 'contractId', headerName: 'Contract ID', minWidth: 100, flex: 1,},
  { field: 'phoneNumber', headerName: 'Phone Number', minWidth: 150,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'department',
    headerName: 'Department',
    minWidth: 130,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'company',
    headerName: 'Company',
    minWidth: 150,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'bankName',
    headerName: 'Bank',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'memberAddress',
    headerName: 'Member Address',
    minWidth: 250,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
    renderCell: (params) => (
        <div style={{ whiteSpace: 'pre-wrap' }}>{params.value}</div>
    ),
  },
  {
    field: 'status',
    headerName: 'Is Active',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 200,
    flex: 1 
  },
];

const rows = [];



const Account = () => {
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
        let apiUrl = `${API_URL}api/Account/ShowAccount`;
        let searchApi = ``;
  
        if (startDate || endDate) {
          apiUrl = `${API_URL}api/Account/FilterAccount?startDate=${encodeURIComponent(dayjs(startDate).format('YYYY/MM/DD'))}&endDate=${encodeURIComponent(dayjs(endDate).format('YYYY/MM/DD'))}`;
          if (searchTerm) {
            searchApi = `${API_URL}api/Account/FilterAccount?searchQuery=${encodeURIComponent(searchTerm)}`;
          }
        } else if (searchTerm) {
          apiUrl = `${API_URL}api/Account/SearchAccount?searchQuery=${encodeURIComponent(searchTerm)}`;
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
            createData(row.id, row.memberId, row.contractId, row.phone, row.department, row.companyName, row.bankName, row.address1, row.isActive, row.dateCreated)
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
          <h1 className="h1-dashboard">Account</h1>
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

export default Account;