import React, { useState, useEffect } from 'react';
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
  const { handleDelete, handleClose, open } = useDeleteHook('AgeRange/DeleteAgeRange'); 

  // const [open, setOpen] = React.useState(false);
  const handleButtonClick = (buttonId) => {
    onButtonClick(buttonId);
    
    if (buttonId === 'Add') {
      navigate('/addAgeRange');

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

const EditButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/editAgeRange/${rowId}`);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0 }}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

function createData(id, description, dateCreated, dateModified) {
  return {id, description, dateCreated, dateModified };
};

const columns = [ 
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
  { field: 'id', headerName: 'Age Range ID', minWidth: 100, flex: 1,},
  { field: 'description', headerName: 'Description', minWidth: 300, flex: 1,},
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
  {
    field: 'dateModified',
    headerName: 'Date Modified',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
  
};

const InsuranceAgeRange = () => {

    const [selectedRowIds, setSelectedRowIds] = useState([]);
    const [rows, setRows] = useState([]);

    // Get Back-end API URL to connect
    const API_URL = "https://localhost:7017/";
  
    useEffect(() => {
      async function fetchData() {
        try {
          let apiUrl = `${API_URL}api/AgeRange/ShowAgeRange`;
    
          const response = await fetch(apiUrl);
    
          if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
          }
    
          const responseData = await response.json();
    
          // Check if responseData is an array before calling map
          if (Array.isArray(responseData)) {
            const updatedRows = responseData.map((row) =>
              createData(
                row.id,
                row.description,
                row.dateCreated,
                row.dateModified
              )
            );
    
            setRows(updatedRows); // Update the component state with the combined data
          } else {
            console.error('Invalid data structure:', responseData);
          }
        } catch (error) {
          console.error('Error fetching data:', error);
        }
      }
      fetchData();
    }, []);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Insurance Age Range</h1>
        </div>
            <div className="bigcarddashboard">
                
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

export default InsuranceAgeRange;