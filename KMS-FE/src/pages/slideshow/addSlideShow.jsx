import React, { useState } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import axios from 'axios';

const AddSlideShow = () => {
  const navigate = useNavigate();
  const API_URL = 'https://localhost:7017/';

  // State to store user information
  const [newSlide, setnewSlide] = useState({
    packageName: '',
    imagevideo: '', 
    fileType: '',
    startDate: new Date().toISOString().split('T')[0],
    endDate: new Date().toISOString().split('T')[0],
  });

  // State to track selected file and its type
  const [selectedFile, setSelectedFile] = useState(null);

  const handleInputChange = (key, value) => {
    setnewSlide((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    setSelectedFile(file);
  
    if (file) {
      const fileType = file.type.startsWith('image/') ? 'Image' : file.type.startsWith('video/') ? 'Video' : "";
      handleInputChange('fileType', fileType);
    } else {
      handleInputChange('fileType', "");
    }
  };

  const handleSave = async () => {
    try {
      if (!selectedFile) {
        console.error('No file selected');
        return;
      }

      // Assuming your API URL is correct
      const formData = new FormData();
      formData.append('file', selectedFile);

      // Append other form data
      Object.entries(newSlide).forEach(([key, value]) => {
        formData.append(key, value);
      });

      const response = await axios.post(`${API_URL}api/Slideshow/AddSlideshow`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', response.data);

      if (response.status === 200) {
        navigate(`/slideshow`);
        // Provide user feedback on successful save
        console.log('Added successfully');
      } else {
        console.log('Add failed');
      }
    } catch (error) {
      console.error('Error adding:', error);
    }
  };

  const handleCancel = () => {
    navigate(`/slideshow`);
  };

  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Slide Show</h1>
      </div>
      <div className="bigcarddashboard">
        <div className="App">
          <div className="table-container">
            <Box
              component="form"
              sx={{
                display: 'flex',
                flexDirection: 'column', 
                gap: '16px', 
                width: 300, 
                margin: 'auto', 
              }}
              noValidate
              autoComplete="off"
            >
              <TextField
                id="packageName"
                label="Package Name"
                variant="outlined"
                value={newSlide.packageName}
                onChange={(e) => handleInputChange('packageName', e.target.value)}
              />
              <div>
                <input
                  id="imagevideo"
                  value={newSlide.imagevideo}
                  type="file"
                  accept="image/*, video/*"
                  onChange={handleFileChange}
                  style={{ display: 'none' }}
                />
                <label htmlFor="imagevideo">
                  <Button
                    variant="contained"
                    fullWidth
                    component="span"
                    sx={{ backgroundColor: '#685ed5', color: '#fff' , marginBottom: '10px' }}
                  >
                    Choose File
                  </Button>
                </label>
                {selectedFile && (
                  <div>
                    {selectedFile.type.startsWith('image/') ? (
                      <img
                        src={URL.createObjectURL(selectedFile)}
                        alt="Selected Image"
                        style={{ maxWidth: '100%', maxHeight: '200px' }}
                      />
                    ) : selectedFile.type.startsWith('video/') ? (
                      <video controls style={{ maxWidth: '100%', maxHeight: '200px' }}>
                        <source src={URL.createObjectURL(selectedFile)} type={selectedFile.type} />
                        Your browser does not support the video tag.
                      </video>
                    ) : null}
                    <p style={{ textAlign: 'left', color: '#333' }}>File Name: <strong>{selectedFile.name}</strong></p>
                  </div>
                )}
              </div>
              <TextField
                id="fileType"
                label="File Type"
                variant="outlined"
                value={newSlide.fileType}
                onChange={(e) => handleInputChange('fileType', e.target.value)}
              />
              <TextField
                id="startDate"
                label="Start Date"
                variant="outlined"
                type="date"
                value={newSlide.startDate}
                onChange={(e) => handleInputChange('startDate', e.target.value)}
              />
              <TextField
                id="endDate"
                label="End Date"
                variant="outlined"
                type="date"
                value={newSlide.endDate}
                onChange={(e) => handleInputChange('endDate', e.target.value)}
              />
              <Box sx={{ display: 'flex', gap: '8px' }}>
                <Button variant="contained" fullWidth onClick={handleSave}>
                  Save
                </Button>
                <Button
                  variant="contained"
                  fullWidth
                  onClick={handleCancel}
                  style={{ backgroundColor: '#848485', color: '#fff' }}
                >
                  Cancel
                </Button>
              </Box>
            </Box>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddSlideShow;
