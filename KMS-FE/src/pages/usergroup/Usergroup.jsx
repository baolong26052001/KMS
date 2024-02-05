import React, { useState, useEffect } from 'react';

// Import from React Router
import {useNavigate} from 'react-router-dom';

// import components from MUI
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';

//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';

const CustomToolbar = ({ onButtonClick, selectedRows }) => {
  const navigate = useNavigate();
  const { handleDelete, handleClose, open } = useDeleteHook('Usergroup/DeleteUsergroup'); 

  // const [open, setOpen] = React.useState(false);
  const handleButtonClick = (buttonId) => {
    onButtonClick(buttonId);
    
    if (buttonId === 'Add') {
      navigate('/addGroup');

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

function createData(id, groupName, dateModified, dateCreated, isActive) {
  return {id, groupName, dateModified, dateCreated, isActive };
}

const PermissionButton = ({ rowId, groupName, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation();
    onClick(rowId);
    navigate(`/permission/${rowId}`);
  };

  const isPermissionButtonVisible = groupName !== 'Admin';

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
      {isPermissionButtonVisible && (
        <Button size="small" variant="contained" color="error" onClick={handleClick}>
          {label}
        </Button>
      )}
    </Box>
  );
};


const EditButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/editGroup/${rowId}`);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
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
  {
    field: 'permissionButton',
    headerName: '',
    width: 120,
    disableColumnMenu: true,
    sortable: false,
    filterable: false, 
    renderCell: (params) => (
      <PermissionButton
        rowId={params.row.id}
        groupName={params.row.groupName}
        label="Permission"
        onClick={handleButtonClick}
      />
    ),
  },
  { field: 'id', headerName: 'Group ID', minWidth: 100, flex: 1, },
  { field: 'groupName', headerName: 'Group Name', minWidth: 200, flex: 1, },
  { field: 'dateModified', headerName: 'Date Modified', minWidth: 200, flex: 1,},
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 200,
    flex: 1,
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    sortable: false,
    minWidth: 100,
    flex: 1,
  },
];


const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};


const Usergroup = () => {
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

  const API_URL = "https://localhost:7017/";

  useEffect(() => {
    async function fetchData() {
      try {
        let apiUrl = `${API_URL}api/Usergroup/ShowUsergroup`;
  
        // If searchTerm is not empty, use the search API endpoint
        if (searchTerm) {
          apiUrl = `${API_URL}api/Usergroup/SearchUsergroup?searchQuery=${encodeURIComponent(searchTerm)}`;
        }
  
        const response = await fetch(apiUrl);
  
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
  
        const responseData = await response.json();
  
        // Check if responseData is an array before calling map
        if (Array.isArray(responseData)) {
          const updatedRows = responseData.map((row) =>
            createData(
              row.id, row.groupName, row.dateModified, row.dateCreated, row.isActive
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
  }, [searchTerm]);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div"> 
            <h1 className="h1-dashboard">Users Group</h1>
        </div>
        <div className="bigcarddashboard">

          <div className="searchdivuser">
              <input onChange={(event) => setSearchTermButton(event.target.value)} onKeyDown={handleKeyPress} placeholder="  Search..." type="text" id="kioskID myInput" name="kioskID" class="searchbar"></input>
              <input onClick={handleSearchButton} type="button" value="Search" className="button button-search"></input>
          </div>

          <div className='Table' style={{ height: 400, width: 'auto'}}>
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

export default Usergroup;