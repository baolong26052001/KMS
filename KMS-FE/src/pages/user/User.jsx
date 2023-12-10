import React, { useState, useEffect } from 'react';

// import components from MUI
import { DataGrid} from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
// import { useHistory } from 'react-router-dom'; // Import useHistory from React Router
import {useNavigate} from 'react-router-dom';
//import css
import './User.css';

const ViewUser = React.lazy(() => import('./viewUser'));

const CustomToolbar = ({ onButtonClick }) => {
  return (
    <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
      <Button variant="outlined" onClick={() => onButtonClick('button1')}>
        Add
      </Button>
      <Button variant="outlined" onClick={() => onButtonClick('button2')}>
        Delete
      </Button>
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

  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/editUser/${rowId}`);
  };

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0 }}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
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
  
    // useEffect(() => {
    //   async function fetchData() {
    //     try {
    //       const response = await fetch(`${API_URL}api/User/ShowUsers`);
    //       const data = await response.json();
  
    //       // Combine fetched data with createData function
    //       const updatedRows = data.map((row) =>
    //       createData(row.id, row.username, row.email, row.groupName, row.isActive, row.lastLogin, row.TotalDaysDormant)
    //       );
  
    //       // If searchTerm is empty, display all rows, otherwise filter based on the search term
    //       const filteredRows = searchTerm
    //       ? updatedRows.filter((row) =>
    //           Object.values(row).some((value) =>
    //             value.toString().toLowerCase().includes(searchTerm.toLowerCase())
    //           )
    //         )
    //       : updatedRows;
  
    //       setRows(filteredRows); // Update the component state with the combined data
    //     } catch (error) {
    //       console.error('Error fetching data:', error);
    //     }
    //   }
  
    //   fetchData();
    // }, [searchTerm]);
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
            <h1 className="h1-dashboard">User</h1>
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
                          <div style={{  position: 'absolute', bottom: 8, alignItems: 'center', marginLeft: '16px' }}>
                            <CustomToolbar onButtonClick={(buttonId) => console.log(buttonId)} />
                            <div style={{ marginLeft: 'auto' }} /> 
                          </div>
                        ),
                      }}
                      pageSizeOptions={[5, 10, 25, 50]}
                      checkboxSelection
                    />

                </div>
            </div>

        
    </div>
    
    
  )
}

export default User;