import React, { useState, useEffect } from 'react';
// import components from MUI
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import {useNavigate} from 'react-router-dom';
//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';


const CustomToolbar = ({ onButtonClick, selectedRows }) => {
  const navigate = useNavigate();
  const { handleDelete, handleClose, open, alertMessage } = useDeleteHook('Station/DeleteStation'); 

  // const [open, setOpen] = React.useState(false);
  const handleButtonClick = (buttonId) => {
    
    onButtonClick(buttonId);
    
    if (buttonId === 'Add') {
      navigate(`/addStation`);

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
      <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
        <Alert onClose={handleClose} variant="filled" severity="error">
            {alertMessage}
        </Alert>
      </Snackbar>
      <GridToolbarExport />
    </div>
  );
};

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/viewStation/${rowId}`);
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
  const navigate = useNavigate();
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/editStation/${rowId}`);
  };

  return (
    <Box sx={{alignItems: 'center'}}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

function createData(id, stationName, companyName, city, address, isActive) {
  return {id, stationName, companyName, city, address, isActive};
}

const columns = [
  {
    field: 'viewButton',
    headerName: '',
    width: 80,
    disableColumnMenu: true,
    sortable: false,
    filterable: false,
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
    sortable: false,
    filterable: false,
    renderCell: (params) => (
      <EditButton
        rowId={params.row.id}
        label="Edit"
        onClick={handleButtonClick}
      />
    ),
  },
  { field: 'id', headerName: 'Station ID', minWidth: 100, flex: 1 },
  { field: 'stationName', headerName: 'Station Name', minWidth: 150, flex: 1 },
  { field: 'companyName', headerName: 'Company Name', minWidth: 150, flex: 1 },
  { field: 'city', headerName: 'City', minWidth: 120, flex: 1 },
  {
    field: 'address',
    headerName: 'Address',
    minWidth: 250,
    flex: 1,
    renderCell: (params) => (
      <div style={{ whiteSpace: 'pre-wrap' }}>{params.value}</div>
    ),
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    sortable: false,
    minWidth: 100,
    flex: 1,
  },
];

const rows = [];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const Station = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchTermButton, setSearchTermButton] = useState('');
  const [selectedRowIds, setSelectedRowIds] = useState([]);

  const handleSearchButton = () => {
      setSearchTerm(searchTermButton);
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      handleSearchButton();
    }
  };

  const [rows, setRows] = useState([]);
  // Get Back-end API URL to connect
  const API_URL = "https://localhost:7017/";

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch(`${API_URL}api/Station/ShowStation`);
        const data = await response.json();
          // Combine fetched data with createData function
          const updatedRows = data.map((row) =>
            createData(row.id, row.stationName, row.companyName, row.city, row.address, row.isActive)
          );
  
          // If searchTerm is empty, display all rows, otherwise filter based on the search term
          const filteredRows = searchTerm
          ? updatedRows.filter((row) =>
              Object.values(row).some((value) =>
                value.toString().toLowerCase().includes(searchTerm.toLowerCase())
              )
            )
          : updatedRows;
  
          setRows(filteredRows); // Update the component state with the combined data

      } catch (error) {
        console.error('Error fetching data:', error);
      }
    }

    fetchData();
  }, [searchTerm]);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Station</h1>
        </div>
            <div className="bigcarddashboard">

                <div className="searchdivuser">
                    <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
                    <input onClick={handleSearchButton} type="button" value="Search" className="button button-search"></input>
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

export default Station;