import { useState } from 'react';

const useDeleteHook = (deleteEndpoint, fetchDataCallback) => {
  const [open, setOpen] = useState(false);

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
    if (selectedItems.length > 0) {
      await deleteItems(selectedItems);
    } else {
      console.warn(`No ${deleteEndpoint}s selected for deletion.`);
      setOpen(true);
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
  };
};

export default useDeleteHook;
