import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import {useNavigate} from 'react-router-dom';
import { useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import { API_URL } from '../../components/config/apiUrl';
// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';
import CustomButton from '../../components/CustomButton/customButton';
import DateFormatter from '../../components/DateFormat/dateFormat';
// Enable the customParseFormat plugin
dayjs.extend(customParseFormat);
dayjs.locale('en'); // Set the locale to English

const CustomToolbar = ({ onButtonClick, selectedRows }) => {
    const navigate = useNavigate();
    const { handleDelete, handleClose, open, alertMessage, severity } = useDeleteHook('InsurancePackage/DeleteBenefit'); 
    const { id } = useParams();
    const { packageName } = useParams();
    // const [open, setOpen] = React.useState(false);
    const handleButtonClick = (buttonId) => {
      
      onButtonClick(buttonId);
      
      if (buttonId === 'Add') {
        navigate(`/addBenefitDetail/${id}`);
  
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
          component={Link}
          to={`/addBenefit/${id}/${packageName}`}
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

const EditButton = ({ rowId, label, onClick}) => {
  const { id: packageId } = useParams();
  const { packageName } = useParams();
  const navigate = useNavigate();
    const handleClick = (event) => {
      event.stopPropagation(); 
      onClick(rowId);
      console.log(packageId, packageName)
      navigate(`/editBenefit/${rowId}`, { state: { packageId, packageName } });
    };
  
    return (
      <Box sx={{alignItems: 'center'}}>
        <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
          {label}
        </Button>
      </Box>
    );
  };

  const formatNumber = (value) => {
    return value.toLocaleString('vi-VN').replace(/,/g, '.');
  };

function createData(id, content, coverage, description, dateModified, dateCreated) {
  return {id, content, coverage, description, dateModified, dateCreated};
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
        packageId = {params.row.packageId}
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
      <CustomButton
        rowId={params.row.id}
        label="Details"
        onClick={handleButtonClick}
        destination={`/benefitDetail/${params.row.id}`}
        color="info"
        variant="contained"
        size="small"
      />
    ),
  },
  { field: 'id', headerName: 'Package Benefit ID', minWidth: 180, flex: 1,},
  { field: 'content', headerName: 'Content', minWidth: 400, flex: 1,},
  { field: 'coverage', headerName: 'Coverage', minWidth: 500, flex: 1, renderCell: (params) => formatNumber(params.value)},
  { 
    field: 'description', 
    headerName: 'Description', 
    minWidth: 400, 
    flex: 1,
    renderCell: (params) => (
      <div style={{ whiteSpace: 'pre-wrap'}} title={params.value}>
        {params.value}
      </div>
    ),
  },
  {
    field: 'dateModified',
    headerName: 'Date Modified',
    minWidth: 200,
    flex: 1,
    renderCell: (params) => <DateFormatter date={params.value} />,
  },
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 200,
    flex: 1,
    renderCell: (params) => <DateFormatter date={params.value} />,
  },
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const InsurancePackageDetail = () => {
    const [selectedRowIds, setSelectedRowIds] = useState([]);
    const [rows, setRows] = useState([]);
    const { id } = useParams();
    const { packageName } = useParams();
  
    useEffect(() => {
      async function fetchData() {
        try {
          let apiUrl = `${API_URL}api/InsurancePackage/ShowInsurancePackageDetail/${id}`;
    
          const response = await fetch(apiUrl);
    
          if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
          }
    
          const responseData = await response.json();
          
    
          if (Array.isArray(responseData)) {
          
            const updatedRows = responseData.map(row =>
              createData(row.id, row.content, row.coverage, row.description, row.dateModified, row.dateCreated)
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
    }, [id]);

  return (
    
    <div className="content"> 

        <div className="admin-dashboard-text-div pt-5"> 
            <h1 className="h1-dashboard">Insurance Benefits</h1>
        </div>
            <div className="bigcarddashboard">

            <div className="selected-package-name">
                {packageName && (
                    <div style={{ color: '#2C3775' }}>
                        <strong>{packageName}</strong> 
                    </div>
                )}
            </div>
                <div className='Table' style={{ height: 400, width: '100%'}}>
                
          <DataGrid
            rows={rows}
            columns={columns}
            
            initialState={{
              pagination: {
                paginationModel: { page: localStorage.getItem('insuranceBenefitPage') ? parseInt(localStorage.getItem('insuranceBenefitPage')) : 0, pageSize: 5 },
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
            onStateChange={(state) => {
              const currentPage = state.pagination.paginationModel.page;
              localStorage.setItem('insuranceBenefitPage', currentPage);
              console.log('Current Page:', currentPage + 1);
            }}
          />
                </div>
            </div>
    </div>
    
    
  )
}

export default InsurancePackageDetail;