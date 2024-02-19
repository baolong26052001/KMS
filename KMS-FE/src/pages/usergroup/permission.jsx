import React, { useState, useEffect } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import Box from '@mui/material/Box';
import Checkbox from '@mui/material/Checkbox';
import { useParams, useNavigate } from 'react-router-dom';
import Button from '@mui/material/Button';

const columns = [
  {
    field: 'site',
    headerName: 'Table',
    minWidth: 500,
    flex: 1,
    sortable: false,
  },
  {
    field: 'canView',
    headerName: 'View',
    minWidth: 100,
    flex: 1,
    sortable: false,
    filterable: false,
    disableColumnMenu: true,
    renderCell: (params) => (
      <Checkbox
        checked={params.value}
        color="primary"
        inputProps={{ 'aria-label': 'View checkbox' }}
      />
    ),
  },
  {
    field: 'canAdd',
    headerName: 'Add',
    minWidth: 100,
    flex: 1,
    sortable: false,
    filterable: false,
    disableColumnMenu: true,
    renderCell: (params) => (
      <Checkbox
        checked={params.value}
        color="primary"
        inputProps={{ 'aria-label': 'Add checkbox' }}
      />
    ),
  },
  {
    field: 'canDelete',
    headerName: 'Delete',
    minWidth: 100,
    flex: 1,
    sortable: false,
    filterable: false,
    disableColumnMenu: true,
    renderCell: (params) => (
      <Checkbox
        checked={params.value}
        color="primary"
        inputProps={{ 'aria-label': 'Delete checkbox' }}
      />
    ),
  },
  {
    field: 'canUpdate',
    headerName: 'Edit',
    minWidth: 100,
    flex: 1,
    sortable: false,
    filterable: false,
    disableColumnMenu: true,
    renderCell: (params) => (
      <Checkbox
        checked={params.value}
        color="primary"
        inputProps={{ 'aria-label': 'Edit checkbox' }}
      />
    ),
  },
];

const Permission = ({ routes }) => {
  const { id } = useParams();
  const [rows, setRows] = useState([]);
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";

  const [groupName, setGroupName] = useState({
    groupName: '',
  });

  useEffect(() => {
    const fetchGroupDetails = async () => {
      try {
        const response = await fetch(`${API_URL}api/Usergroup/ShowUsergroup/${id}`);

        if (response.ok) {
          const groupData = await response.json();

          setGroupName({
            groupName: groupData[0].groupName,
          });
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };

    const fetchRoles = async () => {
      try {
        const response = await fetch(`${API_URL}api/AccessRule/ShowPermissionInfoInEditPage/${id}`);
        if (response.ok) {
          const data = await response.json();
          setRows(data);
        } else {
          console.log('Failed to fetch group details');
        }
      } catch (error) {
        console.error('Error fetching group details:', error);
      }
    };
    
    fetchGroupDetails();
    fetchRoles();
  }, [routes, API_URL, id]);

  const handleCheckboxChange = (event, id, permissionType) => {
    const updatedRows = rows.map(row => {
      if (row.id === id) {
        const updatedRow = {
          ...row,
          [permissionType]: event.target.checked,
        };
        console.log(`Checkbox changed for row ${id}, site: ${row.site}, ${permissionType}: ${updatedRow[permissionType]}`);
        return updatedRow;
      }
      return row;
    });

    setRows(updatedRows); 
  };

  const handleSave = async () => {
    try {
      const updatePromises = rows.map(async (row) => {
        const response = await fetch(`${API_URL}api/AccessRule/UpdatePermission/${id}/${row.site}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            canView: row.canView,
            canAdd: row.canAdd,
            canUpdate: row.canUpdate,
            canDelete: row.canDelete,
          }),
        });

        if (!response.ok) {
          console.log(`Failed to update permission for site ${row.site}`);
        }
      });

      await Promise.all(updatePromises);
      navigate('/usersGroup');
      console.log('Permissions updated successfully');
    } catch (error) {
      console.error('Error updating permissions:', error);
    }
  };

  const handleCancel = async => {
    navigate('/usersGroup');
  }

  return (
    <div className="content">
      <div className="admin-dashboard-text-div">
        <h1 className="h1-dashboard">Permission</h1>
      </div>
      <div className="bigcarddashboard">
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h3 style={{ margin: 0 }}>{groupName.groupName}</h3>
          <Box sx={{ display: 'flex', gap: '8px' }}>    
            <Button variant="contained" onClick={handleSave} style={{ width: '100px' }}>Save</Button>
            <Button variant="contained" onClick={handleCancel} style={{ backgroundColor: '#848485', color: '#fff', width: '100px'  }}>
              Cancel
            </Button>
          </Box>
        </div>
        <div className='Table' style={{ height: 500, width: 'auto' }}>
          <DataGrid
            rows={rows}
            columns={[
              ...columns.slice(0, 1), // 'site' column
              ...columns.slice(1).map(column => ({
                ...column,
                renderCell: (params) => (
                  <Checkbox
                    checked={params.value}
                    onChange={(event) => handleCheckboxChange(event, params.id, column.field)}
                    color="primary"
                    inputProps={{ 'aria-label': `${column.headerName} checkbox` }}
                  />
                ),
              })),
            ]}
            pageSize={10}
            rowsPerPageOptions={[10, 25, 50, 100]}
            pagination
          />
        </div>
      </div>
    </div>
  );
};

export default Permission;