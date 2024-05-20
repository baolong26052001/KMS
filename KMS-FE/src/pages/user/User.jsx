import React, { useState, useEffect } from 'react';
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button } from '@mui/material';
// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {useNavigate} from 'react-router-dom';

//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';

import CustomButton from '../../components/CustomButton/customButton';
import { API_URL } from '../../components/config/apiUrl';
import DateFormat from '../../components/DateFormat/dateFormat';

const CustomToolbar = ({ onButtonClick, selectedRows }) => {
  const navigate = useNavigate();
  const { handleDelete, handleClose, open, alertMessage, severity  } = useDeleteHook('User/DeleteUsers'); 

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
      <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
        <Alert onClose={handleClose} variant="filled" severity={severity}>
            {alertMessage}
        </Alert>
      </Snackbar>
      <GridToolbarExport />
    </div>
  );
};

function createData(id, userName, email, userGroup, isActive, lastLogin, totalDaysDormant) {
  // if (lastLogin) {
  //   const options = {
  //     day: '2-digit',
  //     month: '2-digit',
  //     year: 'numeric',
  //     hour: '2-digit',
  //     minute: '2-digit',
  //     second: '2-digit',
  //     hour12: true,
  //   };
  //   const formatter = new Intl.DateTimeFormat('en-GB', options);
  //   lastLogin = formatter.format(new Date(lastLogin)); // Assuming 'lastLogin' is a string
  // }
  console.log(<DateFormat dateString={lastLogin} />);
  return {
    id,
    userName,
    email,
    userGroup,
    isActive,
    lastLogin: <DateFormat dateString={lastLogin} />, // Using DateFormat component for formatting
    totalDaysDormant,
  };
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
      <CustomButton
        rowId={params.row.id}
        label="View"
        onClick={handleButtonClick}
        destination={`/viewUser/${params.row.id}`}
        color="primary"
        variant="contained"
        size="small"
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
      <CustomButton
        rowId={params.row.id}
        label="Edit"
        onClick={handleButtonClick}
        destination={`/editUser/${params.row.id}`}
        color="warning"
        variant="contained"
        size="small"
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
    minWidth: 190,
    flex: 1,
  },
  {
    field: 'totalDaysDormant',
    headerName: 'Total Days Dormant',
    sortable: false,
    minWidth: 150,
    flex: 1,
  },
];


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
      if (event.key === 'Enter') {
        handleSearchButton();
      }
    };

    const [rows, setRows] = useState([]);
  
    useEffect(() => {
      async function fetchData() {
        try {
          let apiUrl = `${API_URL}api/User/ShowUsers`;

          if (searchTerm) {
            apiUrl = `${API_URL}api/User/SearchUsers?searchQuery=${encodeURIComponent(searchTerm)}`;
          }
    
          const response = await fetch(apiUrl);
    
          if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
          }
    
          const responseData = await response.json();

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
    
            setRows(updatedRows);
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