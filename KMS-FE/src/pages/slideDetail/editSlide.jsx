import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { API_URL } from '../../components/config/apiUrl';
import ImageUpload from './imageUpload';

const EditSlideDetail = () => {
    const navigate = useNavigate();
    const { id, packageName } = useParams();
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

    const [editedItem, setEditedItem] = useState({
        description: '',
        typeContent: '',
        contentUrl: '',
        slideHeaderId: id,
        sequence: '',
        isActive: '',
        userId: '',
        imageBase64: '', // Add imageBase64 to state
    });

    const [selectedFile, setSelectedFile] = useState(null);

    const handleChange = (e) => {
        const { id, type, value } = e.target;
        if (type === 'file') {
            const file = e.target.files[0];
            setSelectedFile(file);
            if (file) {
                const reader = new FileReader();
                reader.onload = () => {
                    const base64Data = reader.result.split(',')[1];
                    setEditedItem(prevState => ({
                        ...prevState,
                        contentUrl: file.name,
                        imageBase64: base64Data // Update imageBase64 when new file is selected
                    }));
                };
                reader.readAsDataURL(file);
            }
        } else {
            handleInputChange(id, value);
        }
    };

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                const response = await fetch(`${API_URL}api/SlideDetail/ShowSlideDetailInEditPage/${id}`);

                if (response.ok) {
                    const groupData = await response.json();

                    setEditedItem({
                        description: groupData[0].description,
                        typeContent: groupData[0].typeContent,
                        contentUrl: groupData[0].contentUrl,
                        slideHeaderId: groupData[0].slideHeaderId,
                        sequence: groupData[0].sequence,
                        isActive: groupData[0].isActive,
                        userId: userIdCookie,
                        imageBase64: groupData[0].imageBase64, // Update imageBase64 from data
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
        setEditedItem(prevState => ({
            ...prevState,
            [key]: value,
        }));
    };

    const handleSave = async () => {
        try {
            const formData = new FormData();
            Object.entries(editedItem).forEach(([key, value]) => {
                formData.append(key, value);
            });
            formData.append('File', selectedFile);
            const response = await axios.put(`${API_URL}api/SlideDetail/UpdateSlideDetail/${id}`, formData);

            if (response.status >= 200 && response.status < 300) {
                const headerId = editedItem.slideHeaderId;
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
        const headerId = editedItem.slideHeaderId;
        navigate(`/slideDetail/${headerId}/${packageName}`);
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
                                gap: '20px',
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
                                type="file"
                                accept=".jpg, .jpeg, .png, .mp4, .avi, .mov"
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
                                    Choose File
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

                            {editedItem.imageBase64 && (
                               <img 
                                    src={`data:image/jpeg;base64,${editedItem.imageBase64}`} 
                                    alt="Uploaded File" 
                                    style={{ 
                                        maxWidth: '50%', 
                                        display: 'block', 
                                        margin: '0 auto' 
                                    }} 
                                />                           
                            )}

                            <TextField
                                id="sequence"
                                label="Sequence"
                                variant="outlined"
                                type='number'
                                value={editedItem.sequence}
                                onChange={(e) => handleInputChange('sequence', e.target.value)}
                            />
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
