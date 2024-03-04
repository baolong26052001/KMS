import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid } from '@mui/x-data-grid';
import DateFilter from '../../components/dateFilter/DateFilter';
import CustomButton from '../../components/CustomButton/customButton';
import { API_URL } from '../../components/config/apiUrl';
// Enable the customParseFormat plugin
dayjs.extend(customParseFormat);
dayjs.locale('en'); // Set the locale to English

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
  //navigate(`/viewAccount/${id}`);
};

function createData(id, memberId, memberName, contractId, phoneNumber, email, department, company, bankName, memberAddress, status, dateCreated) {
  return {id, memberId, memberName, contractId, phoneNumber, email, department, company, bankName, memberAddress, status, dateCreated};
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
        destination={`/viewAccount/${params.row.id}`}
        color="primary"
        variant="contained"
        size="small"
      />
    ),
  },
  { field: 'id', headerName: 'Account ID', minWidth: 100, flex: 1,},
  { field: 'memberId', headerName: 'Member ID', minWidth: 100, flex: 1,},
  { field: 'memberName', headerName: 'Member', minWidth: 300, flex: 1,},
  { field: 'contractId', headerName: 'Contract ID', minWidth: 100, flex: 1,},
  { field: 'phoneNumber', headerName: 'Phone Number', minWidth: 150,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  { field: 'email', headerName: 'Email', minWidth: 150,  
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
    minWidth: 150,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
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

const Account = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchTermButton, setSearchTermButton] = useState('');

  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [rows, setRows] = useState([]);

  const [errorMessage, setErrorMessage] = useState('');

  const handleStartDateChange = (date) => {
    setStartDate(date);
  };

  const handleEndDateChange = (date) => {
    setEndDate(date);
  };

  const getRowId = (row) => row.id;

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
            searchApi = `${API_URL}api/Account/SearchAccount?searchQuery=${encodeURIComponent(searchTerm)}`;
          }
        } else if (searchTerm) {
          apiUrl = `${API_URL}api/Account/SearchAccount?searchQuery=${encodeURIComponent(searchTerm)}`;
        }
  
        const [apiResponse, searchApiResponse] = await Promise.all([
          fetch(apiUrl, {
            headers: {
              Authorization: `Bearer ${localStorage.token}`,
            },
          }),
          searchApi ? fetch(searchApi) : Promise.resolve(null),
        ]);

        if (apiResponse.status === 401) {
          setErrorMessage("You don't have permission");
          return;
        }
  
        if (!apiResponse.ok) {
          throw new Error(`HTTP error! Status: ${apiResponse.status}`);
        }
  
        const apiResponseData = await apiResponse.json();
        const searchApiResponseData = searchApiResponse ? await searchApiResponse.json() : null;
        
  
        if (Array.isArray(apiResponseData)) {
          let filteredRows = apiResponseData;
        
          if (searchApiResponseData && Array.isArray(searchApiResponseData)) {
            filteredRows = apiResponseData.filter(row =>
              searchApiResponseData.some(searchRow => row.id === searchRow.id)
            );
          }
        
          const updatedRows = filteredRows.map(row =>
            createData(row.id, row.memberId, row.fullname, row.contractId, row.phone, row.email, row.department, row.companyName, row.bankName, row.address1, row.isActive, row.dateCreated)
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
          <input
            onChange={(event) => setSearchTermButton(event.target.value)}
            onKeyDown={handleKeyPress}
            placeholder="  Search..."
            type="text"
            id="kioskID myInput"
            name="kioskID"
            className="searchbar"
          ></input>
          <input
            onClick={() => {
              handleSearchButton();
            }}
            type="button"
            value="Search"
            className="button button-search"
          ></input>
        </div>

        {errorMessage ? (
          <h3 className="error-message">{errorMessage}</h3>
        ) : (
          <div className='Table' style={{ height: 400, width: '100%' }}>
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
        )}
      </div>
    </div>
  )
}

export default Account;