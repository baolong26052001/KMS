import React, { useState } from 'react';
import { AppstoreOutlined, MailOutlined, SettingOutlined } from '@ant-design/icons';
import { Menu, Row, Switch, Col, Divider } from 'antd';
import DataTable from 'react-data-table-component';
import './App.css';
import Sidebar from './components/sidebar/Sidebar';
import Headerbar from './components/header/Header';
import Login from './pages/login/login';
import EmptyPage from './pages/empty';
//import Dashboard from './pages/dashboard/dashboard';
//import User from './pages/user/User';
//import Slideshow from './pages/slideshow/slideshow';

import Grid from '@mui/material/Unstable_Grid2'; // Grid version 2
import Box from '@mui/material/Box';

import KioskSetup from './pages/kiosk-setup/kiosk-setup';
import { render } from '@testing-library/react';
//import { Outlet, Link, Route } from "react-router-dom";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

const App = () => {
  return (
    <div>
      <Router>
        <div>
          <Box>
            <Grid container spacing={0}>
              <Grid xs={2}>
                
                <Routes>
                  <Route
                    path="/login"
                    element={<></>}
                  />
                  <Route
                    path="/*"
                    element={<Sidebar />}
                  />
                </Routes>

              </Grid>
              <Grid xs={10}>
                
                <Routes>
                  <Route
                    path="/login"
                    element={<></>}
                  />
                  <Route
                    path="/*"
                    element={<Headerbar />}
                  />
                </Routes>

              <div className='Dashboard-table'>
              <Routes> 
                <Route 
                  path="/" 
                  element={<RouteDashboard />} 
                  />

                <Route 
                  path="/dashboard" 
                  element={<RouteDashboard />} 
                  />

                <Route 
                  path="/users" 
                  element={<RouteUser />} 
                  />

                <Route 
                  path="/usersgroup" 
                  element={<RouteUsergroup />} 
                  />


                <Route path="/kiosksetup" element={<RouteKioskSetup />} />
                <Route path="/kioskhardware" element={<RoutekioskHardware />} />
                <Route path="/station" element={<EmptyPage />} />

                <Route
                    path="/slideshow"
                    element={<RouteSlideshow />}
                  />

                <Route path="/account" element={<EmptyPage />} />
                <Route path="/loantransaction" element={<EmptyPage />} />
                <Route path="/savingtransaction" element={<EmptyPage />} />
                <Route path="/loanstatement" element={<EmptyPage />} />
                <Route path="/savingstatement" element={<EmptyPage />} />
                <Route path="/transactionlogs" element={<EmptyPage />} />
                <Route path="/activitylogs" element={<EmptyPage />} />
                <Route path="/notificationlogs" element={<EmptyPage />} />
                <Route path="/audit" element={<EmptyPage />} />
                <Route path="/kioskhealth" element={<EmptyPage />} />
                <Route path="/login" element={<Login />} />

              </Routes>
              </div>
            </Grid>
          </Grid>
        </Box>
        
{/* 
        <Routes>

          <Route 
            path="/" 
            element={<RouteDashboard />} 
            />

          <Route 
            path="/users" 
            element={<RouteUser />} 
            />

          <Route 
            path="/usersgroup" 
            element={<RouteUsergroup />} 
            />


          <Route path="/kiosksetup" element={<RouteKioskSetup />} />
          <Route path="/kioskhardware" element={<RoutekioskHardware />} />
          <Route path="/station" element={<EmptyPage />} />

          <Route
              path="/slideshow"
              element={<RouteSlideshow />}
            />

          <Route path="/account" element={<EmptyPage />} />
          <Route path="/loantransaction" element={<EmptyPage />} />
          <Route path="/savingtransaction" element={<EmptyPage />} />
          <Route path="/loanstatement" element={<EmptyPage />} />
          <Route path="/savingstatement" element={<EmptyPage />} />
          <Route path="/transactionlogs" element={<EmptyPage />} />
          <Route path="/activitylogs" element={<EmptyPage />} />
          <Route path="/notificationlogs" element={<EmptyPage />} />
          <Route path="/audit" element={<EmptyPage />} />
          <Route path="/kioskhealth" element={<EmptyPage />} />
          
        </Routes> */}
      </div>
    </Router>
    </div>
  )
}


const RouteDashboard = () => {
  const [Dashboard, setDashboard] = React.useState(null);
  React.useEffect(() => {
    import('./pages/dashboard/dashboard').then((module) => {
      setDashboard(() => module.default);
    });
  }, []);
  if (!Dashboard) {
    return null;
  }
  return <Dashboard />;
};



const RouteUser = () => {
  const [User, setUser] = React.useState(null);
  React.useEffect(() => {
    import('./pages/user/User').then((module) => {
      setUser(() => module.default);
    });
  }, []);
  if (!User) {
    return null;
  }
  return <User />;
};

const RouteUsergroup = () => {
  const [Usergroup, setUsergroup] = React.useState(null);
  React.useEffect(() => {
    import('./pages/usergroup/Usergroup').then((module) => {
      setUsergroup(() => module.default);
    });
  }, []);
  if (!Usergroup) {
    return null;
  }
  return <Usergroup />;
};


const RouteSlideshow = () => {
  const [Slideshow, setSlideshow] = React.useState(null);
  React.useEffect(() => {
    import('./pages/slideshow/slideshow').then((module) => {
      setSlideshow(() => module.default);
    });
  }, []);
  if (!Slideshow) {
    return null;
  }
  return <Slideshow />;
};

const RoutekioskHardware = () => {
  const [KioskHardware, setKioskHardware] = React.useState(null);
  React.useEffect(() => {
    import('./pages/kiosk-hardware/kioskHardware').then((module) => {
      setKioskHardware(() => module.default);
    });
  }, []);

  if (!KioskHardware) {
    return null;
  }
  return <KioskHardware />
};

const RouteKioskSetup = () => {
  const [KioskSetup, setKioskSetup] = React.useState(null);
  React.useEffect(() => {
    import('./pages/kiosk-setup/kiosk-setup').then((module) => {
      setKioskSetup(() => module.default);
    });
  }, []);

  if (!KioskSetup) {
    return null;
  }
  return <KioskSetup />
};



export default App;