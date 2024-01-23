import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import DateFilter from '../../components/dateFilter/DateFilter';
import {useNavigate} from 'react-router-dom';

//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';

// Enable the customParseFormat plugin
dayjs.extend(customParseFormat);
dayjs.locale('en'); // Set the locale to English

const CustomToolbar = ({ onButtonClick, selectedRows }) => {
    const navigate = useNavigate();
    const { handleDelete, handleClose, open } = useDeleteHook('InsurancePackageHeader/DeleteInsurancePackageHeader'); 
  
    // const [open, setOpen] = React.useState(false);
    const handleButtonClick = (buttonId) => {
      onButtonClick(buttonId);
      
      if (buttonId === 'Add') {
        navigate('/addInsurancePackage');
  
      } else if (buttonId === 'Delete') {
  
        const userIdsToDelete = selectedRows;
  
        handleDelete(userIdsToDelete);
      }
    };
  
    return (
      <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleButtonClick('Add')}
          style={{ backgroundColor: '#655BD3', color: '#fff' }}
        >
          Add
        </Button>
        <Button
          variant="contained"
          startIcon={<DeleteIcon />}
          onClick={() => handleButtonClick('Delete')}
          style={{ backgroundColor: '#FF3E1D', color: '#fff' }}
        >
          Delete
        </Button>
        <Snackbar open={open} autoHideDuration={1000} onClose={handleClose}>
          <Alert onClose={handleClose} variant="filled" severity="error">
            No rows selected for deletion!!!
          </Alert>
        </Snackbar>
        <GridToolbarExport />
      </div>
    );
};

const ViewButton = ({ rowId, label, onClick, packageName }) => {
  const navigate = useNavigate();
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId, packageName);
    navigate(`/viewPackageDetail/${rowId}/${packageName}`);
  };

  return (
    <Box sx={{alignItems: 'center' }}>
      <Button size="small" variant="contained" color="success" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const DetailButton = ({ rowId, label, onClick, packageName }) => {
  const navigate = useNavigate();
  const handleClick = (event) => {
    event.stopPropagation(); 
    onClick(rowId, packageName);
    navigate(`/insurancePackageDetail/${rowId}`);
  };

  return (
    <Box sx={{alignItems: 'center' }}>
      <Button size="small" variant="contained" color="info" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

const EditButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();
    const handleClick = (event) => {
      event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
      onClick(rowId);
      navigate(`/editInsurancePackage/${rowId}`);
    };
   
    return (
      <Box sx={{alignItems: 'center'}}>
        <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
          {label}
        </Button>
      </Box>
    );
  };

function createData(id, packageName, insuranceType, provider, term, dateModified, dateCreated) {
  return {id, packageName, insuranceType, provider, term, dateModified, dateCreated};
}

const columns = [ 
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
  {
    field: 'detailButton',
    headerName: '',
    width: 80,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <DetailButton
        rowId={params.row.id}
        packageName = {params.row.packageName}
        label="Details"
        onClick={handleButtonClick}
      />
    ),
  },
  {
    field: 'viewButton',
    headerName: '',
    width: 100,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
        <ViewButton
        rowId={params.row.id}
        packageName = {params.row.packageName}
        label="Benefits"
        onClick={handleButtonClick}
      />
    ),
  },
  { field: 'id', headerName: 'Insurance Package ID', minWidth: 180, flex: 1,},
  { field: 'packageName', headerName: 'Package Name', minWidth: 170, flex: 1,},
  { field: 'insuranceType', headerName: 'Insurance Type', minWidth: 170, flex: 1,},
  { field: 'provider', headerName: 'Provider', minWidth: 170, flex: 1,},
  { field: 'term', headerName: 'Term', minWidth: 170, flex: 1,},
  {
    field: 'dateModified',
    headerName: 'Date Modified',
    minWidth: 200,
    flex: 1,
  },
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
];

const rows = [];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const InsurancePackageHeader = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchTermButton, setSearchTermButton] = useState('');
    const [selectedRowIds, setSelectedRowIds] = useState([]);
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
          let apiUrl = `${API_URL}api/InsurancePackageHeader/ShowInsurancePackageHeader`;
          let searchApi = ``;
    
          if (startDate || endDate) {
            apiUrl = `${API_URL}api/InsurancePackageHeader/FilterInsurancePackageHeader?startDate=${encodeURIComponent(dayjs(startDate).format('YYYY/MM/DD'))}&endDate=${encodeURIComponent(dayjs(endDate).format('YYYY/MM/DD'))}`;
            if (searchTerm) {
              searchApi = `${API_URL}api/InsurancePackageHeader/SearchInsurancePackageHeader?searchQuery=${encodeURIComponent(searchTerm)}`;
            }
          } else if (searchTerm) {
            apiUrl = `${API_URL}api/InsurancePackageHeader/SearchInsurancePackageHeader?searchQuery=${encodeURIComponent(searchTerm)}`;
          }
          console.log(apiUrl);
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
              createData(row.id, row.packageName, row.typeName, row.provider, row.content, row.dateModified, row.dateCreated)
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
            <h1 className="h1-dashboard">Insurance Package</h1>
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
                      components={{
                        Toolbar: () => (
                          <div style={{ position: 'absolute', bottom: 8, alignItems: 'center', marginLeft: '16px' }}>
                            <CustomToolbar onButtonClick={(buttonId) => console.log(buttonId)} selectedRows={selectedRowIds} />
                            <div style={{ marginLeft: 'auto' }} />
                          </div>
                        ),
                      }}
                      pageSizeOptions={[5, 10, 25, 50]}
                      checkboxSelection
                      onRowSelectionModelChange={(rowSelectionModel) => {
                        setSelectedRowIds(rowSelectionModel.map((id) => parseInt(id, 10)));
                        console.log('Selected IDs:', rowSelectionModel);
                      }}  
                    />
                </div>
            </div>
    </div>
    
    
  )
}

export default InsurancePackageHeader;