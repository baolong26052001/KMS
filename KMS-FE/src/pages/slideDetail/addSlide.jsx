import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import { API_URL } from '../../components/config/apiUrl';

const AddSlideDetail = () => {
  const navigate = useNavigate();
  function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i].trim();
      if (cookie.startsWith(name + '=')) {
        return cookie.substring(name.length + 1);
      }
    }
    return null;
  }

  const userIdCookie = getCookie('userId');
  const { id, packageName } = useParams();
  // State to store user information
  const [newItem, setnewItem] = useState({
    description: '',
    typeContent: '',
    contentUrl: '',
    sequence: '',
    slideHeaderId: id,
    isActive: true,
    userId: userIdCookie,
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

      const formData = new FormData();
      formData.append('File', selectedFile);

      // Append other form data
      Object.entries(newItem).forEach(([key, value]) => {
        formData.append(key, value);
      });

      const response = await axios.post(`${API_URL}api/SlideDetail/AddSlideDetail`, formData);

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
                gap: '20px', // Add some spacing between form elements
                //width: 300, 
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
                        style={{ maxWidth: '100%', maxHeight: '100%' }}
                      />
                    ) : selectedFile.type.startsWith('video/') ? (
                      <video controls style={{ maxWidth: '100%', maxHeight: '100%' }}>
                        <source src={URL.createObjectURL(selectedFile)} type={selectedFile.type} />
                        Your browser does not support the video tag.
                      </video>
                    ) : null}
                    <p style={{ textAlign: 'left', color: '#333' }}>File Name: <strong>{selectedFile.name}</strong></p>
                  </div>
                )}
              </div>
              <TextField
                id="sequence"
                label="Sequence"
                variant="outlined"
                value={newItem.sequence}
                onChange={(e) => handleInputChange('sequence', e.target.value)}
              />
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