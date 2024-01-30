import React, { useState, useEffect } from 'react';
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {useNavigate} from 'react-router-dom';

//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';

const API_URL = "https://localhost:7017/";

const CustomToolbar = ({ onButtonClick, selectedRows }) => {
  const navigate = useNavigate();
  const { handleDelete, handleClose, open } = useDeleteHook('User/DeleteUsers'); 

  // const [open, setOpen] = React.useState(false);
  const handleButtonClick = (buttonId) => {
    onButtonClick(buttonId);
    
    if (buttonId === 'Add') {
      navigate('/addUser');

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

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/viewUser/${rowId}`);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0 }}>
      <Button size="small" variant="contained" onClick={handleClick}>
        {label}
      </Button>
    </Box>

  );
};

const EditButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();
  const [permissions, setPermissions] = useState([]);
  useEffect(() => {
    async function fetchPermissions() {
      try {
        const permissionResponse = await fetch(`${API_URL}api/AccessRule/ShowPermission`);
        if (!permissionResponse.ok) {
          throw new Error(`HTTP error! Status: ${permissionResponse.status}`);
        }

        const permissionData = await permissionResponse.json();
        setPermissions(permissionData);
      } catch (error) {
        console.error('Error fetching permissions:', error);
      }
    }

    fetchPermissions();
  }, []);
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/editUser/${rowId}`);
  };

  // Check if "canUpdate" is false and "site" is "users"
  const currentSite = window.location.pathname.split('/')[1]; // Extract the first part of the path after the domain
  const canUpdate = permissions.find(permission => permission.site === currentSite)?.canUpdate;
  const userRoleId = permissions.find(permission => permission.site === currentSite)?.groupId;
  const userRoleInCookie = parseInt(document.cookie.split('; ').find(row => row.startsWith('groupId')).split('=')[1]);

  if (!canUpdate && userRoleId == userRoleInCookie) {
    return null; // Return null to hide the Edit button
  }

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0 }}>
      <Button size="small" variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

function createData(id, userName, email, userGroup, isActive, lastLogin, totalDaysDormant) {
  return {id, userName, email, userGroup, isActive, lastLogin, totalDaysDormant };
};

const columns = [ 
  {
    field: 'permissionButton',
    headerName: '',
    width: 80,
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
  {
    field: 'editButton',
    headerName: '',
    width: 80,
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
  { field: 'id', headerName: 'User ID', minWidth: 100, flex: 1,},
  { field: 'userName', headerName: 'User Name', minWidth: 120, flex: 1,},
  { field: 'email', headerName: 'Email', minWidth: 200, flex: 1,},
  {
    field: 'userGroup',
    headerName: 'Group Name',
    sortable: false,
    minWidth: 120,
    flex: 1,
  },
  {
    field: 'isActive',
    headerName: 'Is Active',
    sortable: false,
    minWidth: 100,
    flex: 1,
  },
  {
    field: 'lastLogin',
    headerName: 'Last Login',
    sortable: false,
    minWidth: 180,
    flex: 1,
  },
  {
    field: 'totalDaysDormant',
    headerName: 'Total Days Dormant',
    sortable: false,
    minWidth: 160,
    flex: 1,
  },
];

const rows = [];


const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
  
};


const User = () => {
    const [searchTerm, setSearchTerm] = useState('');

    const [searchTermButton, setSearchTermButton] = useState('');

    const [selectedRowIds, setSelectedRowIds] = useState([]);
    
    const handleSearchButton = () => {
        setSearchTerm(searchTermButton);
    };

    const handleKeyPress = (event) => {
      // Check if the pressed key is Enter (key code 13)
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
          let apiUrl = `${API_URL}api/User/ShowUsers`;
    
          // If searchTerm is not empty, use the search API endpoint
          if (searchTerm) {
            apiUrl = `${API_URL}api/User/SearchUsers?searchQuery=${encodeURIComponent(searchTerm)}`;
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
                row.id,
                row.username,
                row.email,
                row.groupName,
                row.isActive,
                row.lastLogin,
                row.TotalDaysDormant
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

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Users</h1>
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

export default User;