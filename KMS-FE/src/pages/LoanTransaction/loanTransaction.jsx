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
};

function createData(id, memberId, contractId, loanId, loanTerm, debt, totalDebtMustPay, loanRate, transactionDate, dueDate, status) {
  return {id, memberId, contractId, loanId, loanTerm, debt, totalDebtMustPay, loanRate, transactionDate, dueDate, status};
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
    field: 'transactionDate',
    headerName: 'Transaction Date',
    minWidth: 200,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  { field: 'id', headerName: 'ID', minWidth: 100, flex: 1,},
  { field: 'memberId', headerName: 'Member ID', minWidth: 100, flex: 1,},
  { field: 'contractId', headerName: 'Contract ID', minWidth: 100, flex: 1,},
  { field: 'loanId', headerName: 'Loan ID', minWidth: 150,  
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'loanTerm',
    headerName: 'Loan Term',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'debt',
    headerName: 'Debt',
    minWidth: 100,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'totalDebtMustPay',
    headerName: 'Total Debt Must Pay',
    minWidth: 180,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  {
    field: 'loanRate',
    headerName: 'Loan Rate',
    minWidth: 120,
    flex: 1,
    sortable: false,
    disableColumnMenu: true,
  },
  
  {
    field: 'dueDate',
    headerName: 'Due Date',
    sortable: false,
    minWidth: 200,
    flex: 1 
  },
  {
    field: 'status',
    headerName: 'Status',
    sortable: false,
    minWidth: 200,
    flex: 1
  },
];

const rows = [];



const LoanTransaction = () => {
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
        let apiUrl = `${API_URL}api/LoanTransaction/ShowLoanTransaction`;
        let searchApi = ``;
  
        if (startDate || endDate) {
          apiUrl = `${API_URL}api/LoanTransaction/FilterLoanTransaction?startDate=${encodeURIComponent(dayjs(startDate).format('YYYY/MM/DD'))}&endDate=${encodeURIComponent(dayjs(endDate).format('YYYY/MM/DD'))}`;
          if (searchTerm) {
            searchApi = `${API_URL}api/LoanTransaction/SearchLoanTransaction?searchQuery=${encodeURIComponent(searchTerm)}`;
          }
        } else if (searchTerm) {
          apiUrl = `${API_URL}api/LoanTransaction/SearchLoanTransaction?searchQuery=${encodeURIComponent(searchTerm)}`;
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
            createData(row.id, row.memberId, row.contractId, row.loanId, row.loanTerm, row.debt, row.totalDebtMustPay, row.loanRate, row.transactionDate, row.dueDate, row.status)
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
          <h1 className="h1-dashboard">Loan Transaction</h1>
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

export default LoanTransaction;