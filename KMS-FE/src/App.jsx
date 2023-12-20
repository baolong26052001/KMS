import React, { useState } from 'react';
import './App.css';
import Sidebar from './components/sidebar/Sidebar';
import Headerbar from './components/header/Header';
import Login from './pages/login/login';

import Grid from '@mui/material/Unstable_Grid2'; // Grid version 2
import Box from '@mui/material/Box';
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
                    {/* Route for User */}
                    <Route 
                      path="/users" 
                      element={<RouteUser />} 
                      />
                    <Route path="/viewUser/:id" element={<RouteViewUser />} />
                    <Route path="/editUser/:id" element={<RouteEditUser />} />
                    <Route path="/addUser" element={<RouteAddUser />} />

                    {/* Route for UserGroup */}
                    <Route 
                      path="/usersgroup" 
                      element={<RouteUsergroup />} 
                      />
                    <Route path="/addGroup" element={<RouteAddGroup />} />
                    <Route path="/editGroup/:id" element={<RouteEditGroup />} />

                    <Route path="/kiosksetup" element={<RouteKioskSetup />} />
                    <Route path="/viewKioskDetails/:id" element={<RouteKioskDetails />} />
                    <Route path="/kioskhardware" element={<RoutekioskHardware />} />
                    <Route path="/station" element={<RouteStation />} />
                    <Route path="/viewStation/:id" element={<RouteStationDetails />} />

                    <Route
                        path="/slideshow"
                        element={<RouteSlideshow />}
                      />
                    <Route path="/addSlideShow" element={<RouteAddSlide />} />

                    <Route path="/account" element={<RouteAccount />} />
                    <Route path="/viewAccount/:id" element={<RouteViewAccount />} />
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

// view Route
const RouteViewAccount = React.lazy(() => import('./pages/Account/viewAccount'));
const RouteViewUser = React.lazy(() => import('./pages/user/viewUser'));
const RouteKioskDetails = React.lazy(() => import('./pages/kiosk-setup/viewKioskDetails'));
const RouteStationDetails = React.lazy(() => import('./pages/station/viewStation'));

// Edit Route
const RouteEditUser = React.lazy(() => import('./pages/user/editUser'));
const RouteEditGroup = React.lazy(() => import('./pages/usergroup/editGroup'));

// Add Route
const RouteAddUser = React.lazy(() => import('./pages/user/addUser'));
const RouteAddGroup = React.lazy(() => import('./pages/usergroup/addGroup'));
const RouteAddSlide = React.lazy(() => import('./pages/slideshow/addSlideShow'));

export default App;