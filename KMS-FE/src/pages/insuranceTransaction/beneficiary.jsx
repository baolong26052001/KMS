import React, { useState, useEffect } from 'react';
import dayjs from 'dayjs'; // Import dayjs
import customParseFormat from 'dayjs/plugin/customParseFormat'; // Import the customParseFormat plugin
import 'dayjs/locale/en'; // Import the English locale
import { DataGrid, GridToolbarExport } from '@mui/x-data-grid';
import { Button, Box } from '@mui/material';
import DateFilter from '../../components/dateFilter/DateFilter';
import {useNavigate, useParams} from 'react-router-dom';

// Enable the customParseFormat plugin
dayjs.extend(customParseFormat);
dayjs.locale('en'); // Set the locale to English


function createData(id, memberId, beneficiaryName, beneficiaryId, relationship, transactionId, birthday, address, email, phone) {
  return {id, memberId, beneficiaryName, beneficiaryId, relationship, transactionId, birthday, address, email, phone};
}

const columns = [ 
  { field: 'id', headerName: 'Beneficiary ID', minWidth: 150, flex: 1,},
  { field: 'memberId', headerName: 'Member Id', minWidth: 150, flex: 1,},
  { field: 'transactionId', headerName: 'Transaction ID', minWidth: 150, flex: 1,},
  { field: 'beneficiaryName', headerName: 'Beneficiary Name', minWidth: 200, flex: 1,},
  { field: 'beneficiaryId', headerName: 'ID Card Number', minWidth: 200, flex: 1,},
  { field: 'relationship', headerName: 'Relationship', minWidth: 170, flex: 1,},
  { field: 'phone', headerName: 'Phone', minWidth: 170, flex: 1,},
  { field: 'email', headerName: 'Email', minWidth: 200, flex: 1, },
  { field: 'address', headerName: 'Address', minWidth: 200, flex: 1,},
  {
    field: 'birthday',
    headerName: 'Birthday',
    minWidth: 200,
    flex: 1,
  },
];

const handleButtonClick = (id) => {
  // Handle button click, e.g., navigate to another page
  console.log(`Button clicked for row with ID: ${id}`);
};

const InsuranceTransaction = () => {
    const { id } = useParams()
    const [rows, setRows] = useState([]);
    const getRowId = (row) => row.id;
    const API_URL = "https://localhost:7017/";
  
    useEffect(() => {
        async function fetchData() {
          try {
            let apiUrl = `${API_URL}api/InsuranceTransaction/ShowBeneficiaryByInsuranceTransactionId/${id}`;      
            const response = await fetch(apiUrl);
      
            if (!response.ok) {
              throw new Error(`HTTP error! Status: ${response.status}`);
            }
      
            const responseData = await response.json();
      
            // Check if responseData is an array before calling map
            if (Array.isArray(responseData)) {
              const updatedRows = responseData.map((row) =>
                createData(
                    row.id, row.memberId, row.beneficiaryName, row.beneficiaryId, row.relationship, row.transactionId, row.birthday, row.address, row.email, row.phone
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
            <h1 className="h1-dashboard">Insurance Beneficiary</h1>
        </div>
            <div className="bigcarddashboard">
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
                      pageSizeOptions={[5, 10, 25, 50]}
                    />
                </div>
            </div>
    </div>
    
    
  )
}

export default InsuranceTransaction;