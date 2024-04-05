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

const formatNumber = (value) => {
  return value.toLocaleString('vi-VN').replace(/,/g, '.');
};

function createData(transactionDate, id, memberId, contractId, fullName, typeName, packageName, annualPay, provider, registrationDate, expireDate, status, term) {
  return {transactionDate, id, memberId, contractId, fullName, typeName, packageName, annualPay, provider, registrationDate, expireDate, status, term};
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
        destination={`/viewInsuranceTransaction/${params.row.id}`}
        color="primary"
        variant="contained"
        size="small"
      />
    ),
  },
  {
    field: 'detailButton',
    headerName: '',
    width: 120,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
      <CustomButton
        rowId={params.row.id}
        label="Beneficiary"
        onClick={handleButtonClick}
        destination={`/beneficiary/${params.row.id}`}
        color="info"
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
  },
  { field: 'id', headerName: 'Transaction ID', minWidth: 150, flex: 1,},
  { field: 'memberId', headerName: 'Member Id', minWidth: 150, flex: 1,},
  { field: 'contractId', headerName: 'Contract Id', minWidth: 150, flex: 1,},
  { field: 'fullName', headerName: 'Member', minWidth: 200, flex: 1,},
  { field: 'typeName', headerName: 'Insurance Type', minWidth: 170, flex: 1,},
  { field: 'term', headerName: 'Insurance Term', minWidth: 200, flex: 1,},
  { field: 'packageName', headerName: 'Insurance Package', minWidth: 170, flex: 1,},
  { field: 'annualPay', headerName: 'Paid', minWidth: 150, flex: 1, renderCell: (params) => formatNumber(params.value)},
  { field: 'provider', headerName: 'Insurance Provider', minWidth: 150, flex: 1,},
  {
    field: 'registrationDate',
    headerName: 'Registration Date',
    minWidth: 200,
    flex: 1,
  },
  {
    field: 'expireDate',
    headerName: 'Expired Date',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
  { field: 'status', headerName: 'Status', minWidth: 100, flex: 1,},
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const InsuranceTransaction = () => {
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
          let apiUrl = `${API_URL}api/InsuranceTransaction/ShowInsuranceTransaction`;
          let searchApi = ``;
    
          if (startDate || endDate) {
            apiUrl = `${API_URL}api/InsuranceTransaction/FilterInsuranceTransaction?startDate=${encodeURIComponent(dayjs(startDate).format('YYYY/MM/DD'))}&endDate=${encodeURIComponent(dayjs(endDate).format('YYYY/MM/DD'))}`;
            if (searchTerm) {
              searchApi = `${API_URL}api/InsuranceTransaction/SearchInsuranceTransaction?searchQuery=${encodeURIComponent(searchTerm)}`;
            }
          } else if (searchTerm) {
            apiUrl = `${API_URL}api/InsuranceTransaction/SearchInsuranceTransaction?searchQuery=${encodeURIComponent(searchTerm)}`;
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
              createData(row.transactionDate, row.id, row.memberId, row.contractId, row.fullName, row.typeName, row.packageName, row.annualPay, row.provider, row.registrationDate, row.expireDate, row.status, row.content)
            );
          
            setRows(updatedRows); // Update the component state with the combined data
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
            <h1 className="h1-dashboard">Insurance Transaction</h1>
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

export default InsuranceTransaction;