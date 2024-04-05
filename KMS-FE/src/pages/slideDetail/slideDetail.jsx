import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box, Modal } from '@mui/material';
import DateFilter from '../../components/dateFilter/DateFilter';
import {useNavigate, useParams} from 'react-router-dom';
//import MUI Library
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

// import Delete Hook
import useDeleteHook from '../../components/deleteHook/deleteHook';
import { API_URL } from '../../components/config/apiUrl';
import CustomButton from '../../components/CustomButton/customButton';

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
          maxWidth: '700px', // Set maximum width
          maxHeight: '700px', // Set maximum height
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
                return <img src={`data:image/jpeg;base64,${imageUrl}`} style={{ width: '100%', height: '100%', objectFit: 'contain' }} />
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
          Image
        </a>
      ) : (
        <p>No image found</p>
      )}
      {imageUrl && <ViewModal open={openModal} handleClose={handleCloseModal} imageUrl={imageUrl} />}
    </div>
  );
};

function createData(id, description, typeContent, contentUrl, isActive, slideHeaderId, sequence, imageBase64, dateModified, dateCreated) {
  return {id, description, typeContent, contentUrl, isActive, slideHeaderId, sequence, imageBase64, dateModified, dateCreated};
}

const columns = [ 
  // {
  //   field: 'viewButton',
  //   headerName: '',
  //   width: 80,
  //   disableColumnMenu: true,
  //   sortable: false,
  //   filterable: false,
  //   renderCell: (params) => (
  //     <CustomButton
  //       rowId={params.row.id}
  //       label="View"
  //       onClick={handleButtonClick}
  //       destination={`/viewSlideDetail/${params.row.id}`}
  //       color="primary"
  //       variant="contained"
  //       size="small"
  //     />
  //   ),
  // },
  {
    field: 'editButton',
    headerName: '',
    width: 80,
    disableColumnMenu: true,
    sortable: false, // Disable sorting for this column
    filterable: false, // Disable filtering for this column
    renderCell: (params) => (
      <CustomButton
        label="Edit"
        onClick={handleButtonClick}
        destination={`/editSlideDetail/${params.row.id}/${localStorage.getItem("packageName")}`}
        color="warning"
        variant="contained"
        size="small"
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
    minWidth: 300,
    flex: 1,
  },
  {
    field: 'imageBase64',
    headerName: 'Image File',
    sortable: false,
    minWidth: 150,
    flex: 1,
    renderCell: (params) => (
      <ViewImage imageUrl={params.row.imageBase64} />
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
    localStorage.setItem('packageName', packageName);
    const handleStartDateChange = (date) => {
      setStartDate(date);
    };
  
    const handleEndDateChange = (date) => {
      setEndDate(date);
    };
  
    const getRowId = (row) => row.id;
  
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
    
          if (Array.isArray(apiResponseData)) {
            let filteredRows = apiResponseData;
          
            if (searchApiResponseData && Array.isArray(searchApiResponseData)) {
              filteredRows = apiResponseData.filter(row =>
                searchApiResponseData.some(searchRow => row.id === searchRow.id)
              );
            }
            
            const updatedRows = filteredRows.map(row =>
              createData(row.id, row.description, row.typeContent, row.contentUrl, row.isActive, row.slideHeaderId, row.sequence, row.imageBase64, row.dateModified, row.dateCreated)
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