import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../AuthContext/AuthContext';

const Logout = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  useEffect(() => {
    const handleLogout = () => {
      sessionStorage.clear();
      localStorage.setItem('selectedKey', 'dashboard');
      logout();
      navigate('/login');
    };

    handleLogout(); 
    return () => {
    };
  }, [navigate, logout]);

  return null;
};

export default Logout;
