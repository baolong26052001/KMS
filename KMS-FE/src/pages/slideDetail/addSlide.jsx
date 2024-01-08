import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const AddSlideDetail = () => {
  const navigate = useNavigate();
  const API_URL = "https://localhost:7017/";
  const { id, packageName } = useParams();
  // State to store user information
  const [newItem, setnewItem] = useState({
    description: '',
    typeContent: '',
    contentUrl: '',
    slideHeaderId: id,
    isActive: true,
  });

  const handleChange = (e) => {
    const { id, type, value } = e.target;

    if (type === 'file') {
      handleFileChange(e);
      handleInputChange('contentUrl', value.replace(/^.*[\\\/]/, ''));
    } else {
      handleInputChange(id, value);
    }
  };

  const handleInputChange = (key, value) => {
    setnewItem((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const [selectedFile, setSelectedFile] = useState(null);

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
      formData.append('File', selectedFile);



      // Append other form data
      Object.entries(newItem).forEach(([key, value]) => {
        formData.append(key, value);
        console.log(value);
      });

      const response = await fetch(`${API_URL}api/SlideDetail/AddSlideDetail`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newItem),
      });

      console.log('Response Status:', response.status);
      console.log('Response Content:', response.data);

      if (response.status === 200) {
        navigate(`/slideDetail/${id}/${packageName}`);
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
    navigate(`/slideDetail/${id}/${packageName}`);
  };


  return (
    <div className="content">
      <div className="admin-dashboard-text-div pt-5">
        <h1 className="h1-dashboard">Add Slide Detail</h1>
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
                id="description"
                label="Slide Description"
                variant="outlined"
                value={newItem.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
              />
              <TextField
                id="typeContent"
                label="Type Content"
                variant="outlined"
                value={newItem.typeContent}
                onChange={(e) => handleInputChange('typeContent', e.target.value)}
              />
              <div>
                <input
                  id="contentUrl"
                  value={newItem.contentUrl}
                  type={selectedFile ? 'text' : 'file'}
                  accept="image/*, video/*"
                  onChange={handleChange}
                  style={{ display: 'none' }}
                />
                <label htmlFor="contentUrl">
                  <Button
                    variant="contained"
                    fullWidth
                    component="span"
                    sx={{ backgroundColor: '#685ed5', color: '#fff', marginBottom: '10px' }}
                  >
                    Choose Image/Video
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
              <Box sx={{ display: 'flex', gap: '8px' }}>
                <Button variant="contained" fullWidth onClick={handleSave}>
                  Save
                </Button>
                <Button variant="contained" fullWidth onClick={handleCancel} style={{ backgroundColor: '#848485', color: '#fff' }}>
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

export default AddSlideDetail;