
import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';


const EditSlideDetail = () => {
    const navigate = useNavigate();


    const location = useLocation();
    const { id, packageName } = useParams();

    //const { slideHeaderId } = location.state;

    const API_URL = "https://localhost:7017/";

    // State to store user information

    const [editedItem, seteditedItem] = useState({
        description: '',
        typeContent: '',
        contentUrl: '',
        slideHeaderId: id,
        isActive: '',
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
    const [selectedFile, setSelectedFile] = useState(null);
    const handleFileChange = (e) => {
        const file = e.target.files[0];
        setSelectedFile(file);

        if (file) {
            const typeContent = file.type.startsWith('image/') ? 'Image' : file.type.startsWith('video/') ? 'Video' : "";
            handleInputChange('typeContent', typeContent);
        } else {
            handleInputChange('typeContent', "");
        }
    };

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                const response = await fetch(`${API_URL}api/SlideDetail/ShowSlideDetailInEditPage/${id}`);

                if (response.ok) {
                    const groupData = await response.json();

                    // Populate the state with fetched group details
                    seteditedItem({
                        description: groupData[0].description,
                        typeContent: groupData[0].typeContent,
                        contentUrl: groupData[0].contentUrl,
                        slideHeaderId: groupData[0].slideHeaderId,
                        isActive: groupData[0].isActive,
                    });
                } else {
                    console.log('Failed to fetch group details');
                }
            } catch (error) {
                console.error('Error fetching group details:', error);
            }
        };

        fetchDetails();
    }, [API_URL, id]);

    const handleInputChange = (key, value) => {
        seteditedItem((prev) => ({
            ...prev,
            [key]: value,
        }));
    };

    const handleSave = async () => {
        try {
            const formData = new FormData();
            
            const headerId = editedItem.slideHeaderId;
            // Append other form data
            Object.entries(editedItem).forEach(([key, value]) => {
                formData.append(key, value);
                console.log(key+": "+value);
            });
            formData.append('File', selectedFile);
            const response = await axios.put(`${API_URL}api/SlideDetail/UpdateSlideDetail/${id}`, formData);
            
            console.log('Response Status:', response.ok);
            // console.log('Response Data:', response.data);
            // console.log('Response Content:', await response.text());
            
            if (response.status >= 200 && response.status < 300) {
                navigate(`/slideDetail/${headerId}/${packageName}`);
                console.log('Slide detail updated successfully');
            } else {
                console.log('Slide detail update failed');
            }
        } catch (error) {
            console.error('Error updating slide detail:', error);
        }
    };

    const handleCancel = () => {
        navigate(`/slideDetail/${editedItem.slideHeaderId}/${packageName}`);
    };

    return (
        <div className="content">
            <div className="admin-dashboard-text-div pt-5">
                <h1 className="h1-dashboard">Edit Slide Detail</h1>
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
                            autoComplete="on"
                        >
                            <TextField
                                id="description"
                                label="Slide Detail Description"
                                variant="outlined"
                                value={editedItem.description}
                                onChange={(e) => handleInputChange('description', e.target.value)}
                            />
                            <TextField
                                id="typeContent"
                                label="Type Content"
                                type="text"
                                variant="outlined"
                                value={editedItem.typeContent}
                                onChange={(e) => handleInputChange('typeContent', e.target.value)}
                            />

                            <input
                                id="contentUrl"
                                value={selectedFile}
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

                            <p
                                id="contentUrl"
                                label="Content URL"
                                type="text"
                                variant="outlined"
                                value={editedItem.contentUrl}
                                onChange={(e) => handleInputChange('contentUrl', e.target.value)}
                                style={{ textAlign: 'left', color: '#333' }}>File Name: <strong>{editedItem.contentUrl}</strong></p>


                            {editedItem.contentUrl ? (
                                <React.Fragment>
                                {(() => {
                                    try {
                                    const fileExtension = editedItem.contentUrl.split('.').pop().toLowerCase();
                                    const isImage = ['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension);
                                    const isVideo = ['mp4', 'webm', 'ogg'].includes(fileExtension);

                                    if (isImage) {
                                        return <img src={require(`../../../../KioskApp/Insurance/bin/Debug/net6.0-windows/images/${editedItem.contentUrl}`)} alt="Image" style={{ width: '100%' }} />;
                                    } else if (isVideo) {
                                        return (
                                        <video controls style={{ width: '100%' }}>
                                            <source src={require(`../../../../KioskApp/Insurance/bin/Debug/net6.0-windows/images/${editedItem.contentUrl}`)} type={`video/${fileExtension}`} />
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
                                <p>No Image</p>
                            )}
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
                                </div>
                            )}



                            <TextField
                                id="isActive"
                                label="Is Active"
                                variant="outlined"
                                select
                                value={editedItem.isActive}
                                onChange={(e) => handleInputChange('isActive', e.target.value)}
                            >
                                <MenuItem value={true}>True</MenuItem>
                                <MenuItem value={false}>False</MenuItem>
                            </TextField>

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

export default EditSlideDetail;