import React, { useState } from 'react';
import Alert from '@mui/material/Alert';

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
}

function fetchPermissionInfo(groupId) {
  const apiUrl = `https://localhost:7017/api/AccessRule/ShowPermissionInfoInEditPage/${groupId}`;
  return fetch(apiUrl)
    .then(response => response.json())
    .catch(error => {
      console.error('Error fetching permission info:', error);
    });
}

function checkDeletePermission(permissionData, path) {
  if (getCookie('groupId') === '1') {
    return true;
  }

  let modifiedPath = path.startsWith("/") ? path.substring(1) : path;
  modifiedPath = modifiedPath.replace("slideDetail", "slideshow");
  modifiedPath = modifiedPath.replace("insurancePackageDetail", "insurancePackage");
  modifiedPath = modifiedPath.replace("viewPackageDetail", "insurancePackage");
  modifiedPath = modifiedPath.replace("benefitDetail", "insurancePackage");

  for (const entry of permissionData) {
    const normalizedSite = entry.site.startsWith("/") ? entry.site.substring(1) : entry.site;
    if (modifiedPath === normalizedSite) {
      if (entry.canDelete) {
        return true;
      } else {
        return false;
      }
    }
  }

  return false;
}


const useDeleteHook = (deleteEndpoint, fetchDataCallback) => {
  const [open, setOpen] = useState(false);
  const [alertMessage, setAlertMessage] = useState('');

  const deleteItems = async (itemIds) => {
    try {
      const API_URL = "https://localhost:7017/";

      const response = await fetch(`${API_URL}api/${deleteEndpoint}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(itemIds),
      });

      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(`HTTP error! Status: ${response.status}. ${errorMessage}`);
      }

      const responseData = await response.json();
      console.log('Response from the backend:', responseData);

      window.location.reload()

      setOpen(false);
    } catch (error) {
      console.error(`Error deleting ${deleteEndpoint}:`, error);
    }
  };

  const handleDelete = async (selectedItems) => {
    try {
      const groupId = getCookie("groupId");
      const permissionData = await fetchPermissionInfo(groupId);
      const currentPath = window.location.pathname;
      const formattedPath = currentPath.startsWith("/") ? currentPath.substring(1) : currentPath;
      const canDelete = checkDeletePermission(permissionData, formattedPath);
  
      // Check if there are selected items to delete
      if (selectedItems.length === 0) {
        setAlertMessage(`No Items selected for deletion.`);
        setOpen(true);
        return;
      }
  
      if (!canDelete) {
        setAlertMessage("You don't have permission to delete.");
        setOpen(true);
        return;
      }
  
      await deleteItems(selectedItems);
    } catch (error) {
      console.error('Error checking permission and deleting:', error);
    }
  };
  

  const handleClose = (event, reason) => {
    if (reason === 'clickaway') {
      return;
    }
    setOpen(false);
  };

  return {
    handleDelete,
    handleClose,
    open,
    alertMessage
  };
};

export default useDeleteHook;
