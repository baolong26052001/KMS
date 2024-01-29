import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Navigate } from "react-router-dom";
import Button from '@mui/material/Button';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';

import '../login/login.css';

const Login = ({onLogin }) => {
  const navigate = useNavigate();
  const [values, setValues] = useState({
    username: '',
    password: '',
    showPassword: false,
    loading: false,
  });

  const handleChange = (prop) => (event) => {
    setValues({ ...values, [prop]: event.target.value });
  };

  const handleShowPassword = () => {
    setValues({ ...values, showPassword: !values.showPassword });
  };

  const onFinish = async () => {
    const API_URL = 'https://localhost:7017/';

    try {
      setValues({ ...values, loading: true }); 

      const response = await fetch(`${API_URL}api/User/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: values.username,
          password: values.password,
        }),
      });

      // The response is in JSON format
      const data = await response.json();
      console.log(data);
      if (response.ok) {
        localStorage.setItem('token', data.Token);
        localStorage.setItem('role', data.Role);
        onLogin();
        navigate('/dashboard'); 
      } else {
        console.error('Login failed:', data.message);
      }
    } catch (error) {
      console.error('Error during login:', error);
    } finally {
      setValues({ ...values, loading: false });
    }
  };

  return (
    <div className='login'>
      <div className='Login-field'>
        <h3>Login</h3>
        <div className="Login-Form">
          <FormControl fullWidth variant="outlined" margin="normal">
            <InputLabel htmlFor="outlined-username">Username</InputLabel>
            <OutlinedInput
              id="outlined-username"
              value={values.username}
              onChange={handleChange('username')}
              label="Username"
            />
          </FormControl>

          <FormControl fullWidth variant="outlined" margin="normal">
            <InputLabel htmlFor="outlined-adornment-password">Password</InputLabel>
            <OutlinedInput
              id="outlined-adornment-password"
              type={values.showPassword ? 'text' : 'password'}
              value={values.password}
              onChange={handleChange('password')}
              endAdornment={
                <InputAdornment position="end">
                  <IconButton
                    onClick={handleShowPassword}
                    edge="end"
                    aria-label="toggle password visibility"
                  >
                    {values.showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              }
              label="Password"
            />
          </FormControl>

          <Button
            variant="contained"
            color="primary"
            onClick={onFinish}
            disabled={values.loading} // Disable the button during loading
          >
            {values.loading ? 'Signing In...' : 'Sign In'}
          </Button>
        </div>
      </div>
    </div>
  );
};

export default Login;