
import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box, Modal, Typography } from '@mui/material';
import DateFilter from '../../components/dateFilter/DateFilter';
import {useNavigate, useParams} from 'react-router-dom';
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

const ViewModal = ({ open, handleClose, imageUrl }) => {
  return (
    <Modal open={open} onClose={handleClose}>
      <Box
        sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          width: 700,
          bgcolor: 'background.paper',
          border: '2px solid #000',
          boxShadow: 24,
          p: 1,
        }}
      >
        {imageUrl ? (
          <React.Fragment>
            {(() => {
              try {
                const fileExtension = imageUrl.split('.').pop().toLowerCase();
                const isImage = ['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension);
                const isVideo = ['mp4', 'webm', 'ogg'].includes(fileExtension);

                if (isImage) {
                  return <img src={require(`../../../../KioskApp/Insurance/bin/Debug/net6.0-windows/images/${imageUrl}`)} alt="Image" style={{ width: '100%' }} />;
                } else if (isVideo) {
                  return (
                    <video controls style={{ width: '100%' }}>
                      <source src={require(`../../../../KioskApp/Insurance/bin/Debug/net6.0-windows/images/${imageUrl}`)} type={`video/${fileExtension}`} />
                      Your browser does not support the video tag.
                    </video>
                  );
                } else {
                  console.error("Unsupported file type:", fileExtension);
                  return <p>Unsupported File Type</p>;
                }
              } catch (error) {
                console.error("Error loading file:", error);
                return <p>Error Loading File</p>;
              }
            })()}
          </React.Fragment>
        ) : (
          <p>No File</p>
        )}
      </Box>
    </Modal>
  );
};

const CustomToolbar = ({ onButtonClick, selectedRows }) => {
  const navigate = useNavigate();
  const { handleDelete, handleClose, open, alertMessage, severity } = useDeleteHook('SlideDetail/DeleteSlideDetail'); 
  const { id, packageName } = useParams();

  const handleButtonClick = (buttonId) => {
    onButtonClick(buttonId);
    
    if (buttonId === 'Add') {
      navigate(`/addSlideDetail/${id}/${packageName}`);

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

const ViewImage = ({ imageUrl }) => {
  const [openModal, setOpenModal] = useState(false);

  const handleClick = (event) => {
    event.stopPropagation();
    if (imageUrl) {
      setOpenModal(true);
    }
  };

  const handleCloseModal = () => {
    setOpenModal(false);
  };

  return (
    <div>
      {imageUrl ? (
        <a href="#" onClick={handleClick}>
          {imageUrl}
        </a>
      ) : (
        <p>No image found</p>
      )}
      {imageUrl && <ViewModal open={openModal} handleClose={handleCloseModal} imageUrl={imageUrl} />}
    </div>
  );
};

const ViewButton = ({ rowId, label, onClick }) => {
  const navigate = useNavigate();
  const handleClick = (event) => {
    event.stopPropagation();
    onClick(rowId);
    navigate(`/viewSlideDetail/${rowId}`)
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
  const { packageName } = useParams();
  const handleClick = (event) => {
    event.stopPropagation(); // Stop the click event from propagating to the parent DataGrid row
    onClick(rowId);
    navigate(`/editSlideDetail/${rowId}/${packageName}`)
  };

  return (
    <Box sx={{alignItems: 'center'}}>
      <Button size="small"  variant="contained" color="warning" onClick={handleClick}>
        {label}
      </Button>
    </Box>
  );
};

function createData(id, description, typeContent, contentUrl, isActive, slideHeaderId, sequence, dateModified, dateCreated) {
  return {id, description, typeContent, contentUrl, isActive, slideHeaderId, sequence, dateModified, dateCreated};
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
  { field: 'id', headerName: 'Slide Detail ID', minWidth: 150, flex: 1,},
  { field: 'description', headerName: 'Description', minWidth: 200, flex: 1,},
  {
    field: 'typeContent',
    headerName: 'Type Content',
    minWidth: 120,
    flex: 1,
  },
  {
    field: 'contentUrl',
    headerName: 'Content Url',
    sortable: false,
    minWidth: 250,
    flex: 1,
    renderCell: (params) => (
      <ViewImage imageUrl={params.row.contentUrl} />
    ),
  },
  {
    field: 'sequence',
    headerName: 'Sequence',
    sortable: false,
    minWidth: 100,
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
    field: 'dateModified',
    headerName: 'Date Modified',
    sortable: false,
    minWidth: 250,
    flex: 1,
  },
  {
    field: 'dateCreated',
    headerName: 'Date Created',
    sortable: false,
    minWidth: 250,
    flex: 1,
  },
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const SlideDetail = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchTermButton, setSearchTermButton] = useState('');
    const [selectedRowIds, setSelectedRowIds] = useState([]);
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);
    const [rows, setRows] = useState([]);
    const { id, packageName } = useParams();
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
          let apiUrl = `${API_URL}api/SlideDetail/ShowSlideDetail/${id}`;
          let searchApi = ``;
    
          if (startDate || endDate) {
            apiUrl = `${API_URL}api/SlideDetail/FilterSlideDetail/${id}?startDate=${encodeURIComponent(dayjs(startDate).format('YYYY/MM/DD'))}&endDate=${encodeURIComponent(dayjs(endDate).format('YYYY/MM/DD'))}`;
            if (searchTerm) {
              searchApi = `${API_URL}api/SlideDetail/SearchSlideDetail?searchQuery=${encodeURIComponent(searchTerm)}`;
            }
          } else if (searchTerm) {
            apiUrl = `${API_URL}api/SlideDetail/SearchSlideDetail?searchQuery=${encodeURIComponent(searchTerm)}`;
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
              createData(row.id, row.description, row.typeContent, row.contentUrl, row.isActive, row.slideHeaderId, row.sequence, row.dateModified, row.dateCreated)
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
            <h1 className="h1-dashboard">Slide Show Detail</h1>
        </div>
            <div className="bigcarddashboard">

            <div className="selected-package-name">
                {packageName && (
                    <div style={{ color: '#2C3775' }}>
                        <strong>{packageName}</strong> 
                    </div>
                )}
            </div>

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

export default SlideDetail;