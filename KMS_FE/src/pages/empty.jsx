import React from 'react';
import { Breadcrumb, Layout, Menu, theme } from 'antd';
import { useLocation } from 'react-router-dom';

function EmptyPage() {
    const location = useLocation();
    const path = location.pathname;
    const wordAfterSlash = path.split('/')[1]; // Get the word after the first slash

    return (
      <div className="App">
        
        <p>Page /{wordAfterSlash}</p>
      </div>
    );
}
  
export default EmptyPage;