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
import { render } from '@testing-library/react';
//import { Outlet, Link, Route } from "react-router-dom";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';



const App = () => {
  return (
    <div>
      <Router>
        <div>
        <Routes>
          <Route path="/login" element={<Login />} />
        </Routes>  
          <Box>
            <Grid container spacing={0}>
              <Grid xs={2}>
                <Sidebar />
              </Grid>
              <Grid xs={10}>
                <Headerbar />
              <div className='Dashboard-table'>
                <React.Suspense>      
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
                    <Route path="/station" element={<RouteStation />} />

                    <Route
                        path="/slideshow"
                        element={<RouteSlideshow />}
                      />

                    <Route path="/account" element={<RouteAccount />} />
                    <Route path="/loantransaction" element={<RouteLoanTransaction />} />
                    <Route path="/savingtransaction" element={<RouteSavingTransaction />} />
                    <Route path="/loanstatement" element={<RouteLoanStatement />} />
                    <Route path="/savingstatement" element={<RouteSavingStatement />} />
                    <Route path="/transactionlogs" element={<RouteTransactionLogs />} />
                    <Route path="/activitylogs" element={<RouteActivityLogs />} />
                    <Route path="/notificationlogs" element={<RouteNotificationLogs />} />
                    <Route path="/audit" element={<RouteAudit />} />
                    <Route path="/kioskhealth" element={<RouteKioskHealth />} />
                    {/* <Route path="/login" element={<Login />} /> */}
                  </Routes>
                </React.Suspense>
              </div>
            </Grid>
          </Grid>
        </Box>
  
      </div>
    </Router>
    </div>
  )
}

const RouteDashboard = React.lazy(() => import('./pages/dashboard/dashboard'));
const RouteUser = React.lazy(() => import('./pages/user/User'));
const RouteUsergroup = React.lazy(() => import('./pages/usergroup/Usergroup'));
const RouteSlideshow = React.lazy(() => import('./pages/slideshow/slideshow'));
const RoutekioskHardware = React.lazy(() => import('./pages/kiosk-hardware/kioskHardware'));
const RouteKioskSetup = React.lazy(() => import('./pages/kiosk-setup/kiosk-setup'));
const RouteStation = React.lazy(() => import('./pages/station/station'));
const RouteAccount = React.lazy(() => import('./pages/Account/account'));
const RouteLoanTransaction = React.lazy(() => import('./pages/LoanTransaction/loanTransaction'));
const RouteLoanStatement = React.lazy(() => import('./pages/loanStatement/loanStatement'));
const RouteSavingTransaction = React.lazy(() => import('./pages/savingTransaction/savingTransaction'));
const RouteSavingStatement = React.lazy(() => import('./pages/savingStatement/savingStatement'));
const RouteTransactionLogs = React.lazy(() => import('./pages/transactionLogs/transactionLogs'));
const RouteActivityLogs = React.lazy(() => import('./pages/activityLogs/activityLogs'));
const RouteNotificationLogs = React.lazy(() => import('./pages/notificationLogs/notificationLogs'));
const RouteAudit = React.lazy(() => import('./pages/audit/audit'));
const RouteKioskHealth = React.lazy(() => import('./pages/kioskHealth/kioskHealth'));

export default App;