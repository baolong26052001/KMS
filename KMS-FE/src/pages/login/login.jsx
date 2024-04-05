import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '@mui/material/Button';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import { API_URL } from '../../components/config/apiUrl';
import '../login/login.css';

const Login = ({ onLogin }) => {
  const navigate = useNavigate();
  const [values, setValues] = useState({
    username: '',
    password: '',
    showPassword: false,
    loading: false,
  });
  const [errorMessage, setErrorMessage] = useState('');
  const handleCloseSnackbar = () => {
    setErrorMessage('');
  };

  const handleChange = (prop) => (event) => {
    setValues({ ...values, [prop]: event.target.value });
  };

  const handleShowPassword = () => {
    setValues({ ...values, showPassword: !values.showPassword });
  };

  const handleKeyDown = (event) => {
    if (event.key === 'Enter') {
      onFinish();
    }
  };

  const onFinish = async () => {
    try {
      setValues({ ...values, loading: true });

      // Check if the password field is empty
       if (!values.username.trim() && !values.password.trim()) {
        setErrorMessage('Username and password cannot be empty');
        return;
      }
      else if (!values.password.trim()) {
        setErrorMessage('Password cannot be empty');
        return;
      }
      else if (!values.username.trim()) {
        setErrorMessage('Username cannot be empty');
        return;
      }
      

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

      const data = await response.json();
      if (response.ok) {
        document.cookie = `role=${data.Role}; path=/`;
        document.cookie = `groupId=${data.GroupId}; path=/`;
        localStorage.setItem('userName', data.Username)
        localStorage.setItem('role', data.Role)
        document.cookie = `userId=${data.UserId}; path=/`;
        onLogin();
        navigate('/dashboard');
      } else {
        console.error('Login failed:', data.message);
        setErrorMessage('Username or password is wrong');
      }
    } catch (error) {
      console.error('Error during login:', error);
      setErrorMessage('An error occurred during login');
    } finally {
      setValues({ ...values, loading: false });
    }
  };


  return (
    <div className='login'>
      <div className='Login-field'>
        <h2>Login</h2>
        <div className="Login-Form">
          <FormControl fullWidth variant="outlined" margin="normal">
            <InputLabel htmlFor="outlined-username">Username</InputLabel>
            <OutlinedInput
              id="outlined-username"
              value={values.username}
              onChange={handleChange('username')}
              label="Username"
              onKeyDown={handleKeyDown}
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
              onKeyDown={handleKeyDown}
            />
          </FormControl>

          <Button
            variant="contained"
            color="primary"
            onClick={onFinish}
            disabled={values.loading} 
            style={{ marginTop: '10px' }}
          >
            {values.loading ? 'Signing In...' : 'Sign In'}
          </Button>
        </div>
      </div>

      <Snackbar
        open={errorMessage !== ''}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
      >
        <Alert onClose={handleCloseSnackbar} variant="filled" severity="error">
          {errorMessage}
        </Alert>
      </Snackbar>
    </div>
  );
};

export default Login;