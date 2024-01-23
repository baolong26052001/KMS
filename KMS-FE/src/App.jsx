import React, { useState } from 'react';
import './App.css';
import Sidebar from './components/sidebar/Sidebar';
import Headerbar from './components/header/Header';
import Login from './pages/login/login';
import { Navigate } from "react-router-dom";
import Grid from '@mui/material/Unstable_Grid2'; // Grid version 2
import Box from '@mui/material/Box';
import { useAuth } from './components/AuthContext/AuthContext';
//import { Outlet, Link, Route } from "react-router-dom";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';


const App = () => {

  const { isAuthenticated, login  } = useAuth();
  return (
    <div>
      <Router>
        <div>
        <Routes>
          <Route
            path="/login"
            element={isAuthenticated ? <Navigate to="/dashboard" /> : <Login  onLogin={login}/>}
          />
        </Routes> 
        {/* {isAuthenticated ? (  */}
          <React.Fragment>
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

                    {/* Route for Kiosk*/}
                    <Route path="/kiosksetup" element={<RouteKioskSetup />} />
                    <Route path="/viewKioskDetails/:id" element={<RouteKioskDetails />} />
                    <Route path="/kioskhardware" element={<RoutekioskHardware />} />
                    <Route path="/viewKioskHardware/:id" element={<RouteKioskHardwareDetails />} />
                    
                    <Route path="/addKiosk" element={<RouteAddKiosk />} />
                    <Route path="/editKiosk/:id" element={<RouteEditKioskSetup />} />
                    

                    {/* Route for Station*/}
                    <Route path="/station" element={<RouteStation />} />
                    <Route path="/viewStation/:id" element={<RouteStationDetails />} />
                    <Route path="/addStation" element={<RouteAddStation />} />
                    <Route path="/editStation/:id" element={<RouteEditStation />} />
                    
                    {/* Route for Slide Show*/}
                    <Route
                        path="/slideshow"
                        element={<RouteSlideshow />}
                      />
                    <Route path="/addSlideShow" element={<RouteAddSlide />} />
                    <Route path="/editSlideShow/:id" element={<RouteEditSlideShow />} />
                    <Route path="/viewSlideShow/:id" element={<RouteViewSlideShow />} />
                    {/* Route for Slide Detail*/}
                    <Route path="/slideDetail/:id/:packageName" element={<RouteSlideDetail />} />
                    <Route path="/addSlideDetail/:id/:packageName" element={<RouteAddSlideDetail />} />
                    <Route path="/editSlideDetail/:id/:packageName" element={<RouteEditSlideDetail />} />
                    
                    
                    {/* Route for Account*/}
                    <Route path="/account" element={<RouteAccount />} />
                    <Route path="/viewAccount/:id" element={<RouteViewAccount />} />

                    {/* Route for Loan*/}
                    <Route path="/loantransaction" element={<RouteLoanTransaction />} />
                    <Route path="/savingtransaction" element={<RouteSavingTransaction />} />
                    <Route path="/loanstatement" element={<RouteLoanStatement />} />
                    <Route path="/savingstatement" element={<RouteSavingStatement />} />

                    {/* Route for Insurance Transaction*/}
                    <Route path="/insuranceTransaction" element={<RouteInsuranceTransaction />} />

                    {/* Route for Insurance Package*/}
                    <Route path="/insurancePackage" element={<RouteInsurancePackage />} />
                    <Route path="/viewPackageDetail/:id/:packageName" element={<RouteViewInsurancePackage />} />
                    <Route path="/addInsurancePackage" element={<RouteAddInsurancePackage />} />
                    <Route path="/editInsurancePackage/:id" element={<RouteEditInsurancePackage />} />
                    <Route path="/editBenefit/:id" element={<RouteEditBenefit />} />

                    {/* Route for Insurance Package*/}
                    <Route path="/insurancePackageDetail/:id" element={<RouteInsuranceDetail />} />
                    <Route path="/addInsurancePackageDetail/:id" element={<RouteAddInsurancePackageDetail />} />
                    <Route path="/editInsurancePackageDetail/:id/:packageHeaderId" element={<RouteEditInsurancePackageDetail />} />
                    
                    {/* Route for Insurance Benefit*/}
                    <Route path="/addBenefit/:id/:packageName" element={<RouteAddBenefit />} />
                    <Route path="/benefitDetail/:id" element={<RouteBenefitDetail />} />
                    <Route path="/addBenefitDetail/:id" element={<RouteAddBenefitDetail />} />
                    <Route path="/editBenefitDetail/:id" element={<RouteEditBenefitDetail />} />
                    
                    {/* Route for Insurance Provider*/}
                    <Route path="/insuranceProvider" element={<RouteInsuranceProvider />} />
                    <Route path="/addInsuranceProvider" element={<RouteAddInsuranceProvider />} />
                    <Route path="/editInsuranceProvider/:id" element={<RouteEditInsuranceProvider />} />                    
                    
                    {/* Route for Insurance Type*/}
                    <Route path="/insuranceType" element={<RouteInsuranceType />} />
                    <Route path="/addInsuranceType" element={<RouteAddInsuranceType />} />
                    <Route path="/editInsuranceType/:id" element={<RouteEditInsuranceType />} />   

                    {/* Route for Insurance Term*/}
                    <Route path="/insuranceTerm" element={<RouteInsuranceTerm />} />
                    <Route path="/addInsuranceTerm" element={<RouteAddInsuranceTerm />} />
                    <Route path="/editInsuranceTerm/:id" element={<RouteEditInsuranceTerm />} />   

                    {/* Route for Insurance Age Range*/}
                    <Route path="/insuranceAgeRange" element={<RouteInsuranceAgeRange />} />
                    <Route path="/addAgeRange" element={<RouteAddInsuranceAgeRange />} />
                    <Route path="/editAgeRange/:id" element={<RouteEditInsuranceAgeRange />} />       

                    <Route path="/transactionlogs" element={<RouteTransactionLogs />} />
                    <Route path="/activitylogs" element={<RouteActivityLogs />} />
                    <Route path="/notificationlogs" element={<RouteNotificationLogs />} />
                    <Route path="/audit" element={<RouteAudit />} />
                    <Route path="/kioskhealth" element={<RouteKioskHealth />} />
                    <Route
                      path="/login"
                    />
                  </Routes>
                </React.Suspense>
              </div>
            </Grid>
          </Grid>
        </Box>
        </React.Fragment>
         {/* ) : (
           // Redirect to login if not authenticated
          <Navigate to="/login" />
         )}  */}
      </div>
    </Router>
    </div>
  )
}

const RouteDashboard = React.lazy(() => import('./pages/dashboard/dashboard'));
const RouteUser = React.lazy(() => import('./pages/user/User'));
const RouteUsergroup = React.lazy(() => import('./pages/usergroup/Usergroup'));
const RouteSlideshow = React.lazy(() => import('./pages/slideshow/slideshow'));
const RouteSlideDetail = React.lazy(() => import('./pages/slideDetail/slideDetail'));
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
const RouteInsuranceTransaction = React.lazy(() => import('./pages/insuranceTransaction/insuranceTransaction'));
const RouteInsurancePackage = React.lazy(() => import('./pages/InsurancePackage/insurancePackage'));
const RouteBenefitDetail = React.lazy(() => import('./pages/InsurancePackage/benefitDetail'));
const RouteInsuranceDetail = React.lazy(() => import('./pages/insurancePackageDetail/insurancePackageDetail'));
const RouteInsuranceProvider = React.lazy(() => import('./pages/insuranceProvider/insuranceProvider'));
const RouteInsuranceType = React.lazy(() => import('./pages/insuranceType/insuranceType'));
const RouteInsuranceTerm = React.lazy(() => import('./pages/insuranceTerm/insuranceTerm'));
const RouteInsuranceAgeRange = React.lazy(() => import('./pages/insuranceAgeRange/insuranceAgeRange'));

// view Route
const RouteViewAccount = React.lazy(() => import('./pages/Account/viewAccount'));
const RouteViewUser = React.lazy(() => import('./pages/user/viewUser'));
const RouteKioskDetails = React.lazy(() => import('./pages/kiosk-setup/viewKioskDetails'));
const RouteKioskHardwareDetails = React.lazy(() => import('./pages/kiosk-hardware/viewKioskHardware'));
const RouteStationDetails = React.lazy(() => import('./pages/station/viewStation'));
const RouteViewSlideShow = React.lazy(() => import('./pages/slideshow/viewSlideShow'));
const RouteViewInsurancePackage = React.lazy(() => import('./pages/InsurancePackage/insuranceBenefit'));
const RouteViewLoanTransaction = React.lazy(() => import('./pages/LoanTransaction/viewLoanTransaction'));

// Edit Route
const RouteEditUser = React.lazy(() => import('./pages/user/editUser'));
const RouteEditGroup = React.lazy(() => import('./pages/usergroup/editGroup'));
const RouteEditStation = React.lazy(() => import('./pages/station/editStation'));
const RouteEditSlideShow = React.lazy(() => import('./pages/slideshow/editSlideShow'));
const RouteEditSlideDetail = React.lazy(() => import('./pages/slideDetail/editSlide'));
const RouteEditKioskSetup = React.lazy(() => import('./pages/kiosk-setup/editKiosk'));
const RouteEditBenefit = React.lazy(() => import('./pages/InsurancePackage/editBenefit'));
const RouteEditInsurancePackage = React.lazy(() => import('./pages/InsurancePackage/editInsurancePackage'));
const RouteEditBenefitDetail = React.lazy(() => import('./pages/InsurancePackage/editBenefitDetail'));
const RouteEditInsurancePackageDetail = React.lazy(() => import('./pages/insurancePackageDetail/editInsurancePackageDetail'));
const RouteEditInsuranceProvider = React.lazy(() => import('./pages/insuranceProvider/editInsuranceProvider'));
const RouteEditInsuranceType = React.lazy(() => import('./pages/insuranceType/editInsuranceType'));
const RouteEditInsuranceTerm = React.lazy(() => import('./pages/insuranceTerm/editInsuranceTerm'));
const RouteEditInsuranceAgeRange = React.lazy(() => import('./pages/insuranceAgeRange/editAgeRange'));

// Add Route
const RouteAddUser = React.lazy(() => import('./pages/user/addUser'));
const RouteAddGroup = React.lazy(() => import('./pages/usergroup/addGroup'));
const RouteAddSlide = React.lazy(() => import('./pages/slideshow/addSlideShow'));
const RouteAddSlideDetail = React.lazy(() => import('./pages/slideDetail/addSlide'));
const RouteAddInsurancePackage = React.lazy(() => import('./pages/InsurancePackage/addInsurancePackage'));
const RouteAddInsurancePackageDetail = React.lazy(() => import('./pages/insurancePackageDetail/addInsurancePackageDetail'));
const RouteAddBenefit = React.lazy(() => import('./pages/InsurancePackage/addBenefit'));
const RouteAddBenefitDetail = React.lazy(() => import('./pages/InsurancePackage/addBenefitDetail'));
const RouteAddStation = React.lazy(() => import('./pages/station/addStation'));
const RouteAddKiosk = React.lazy(() => import('./pages/kiosk-setup/addKiosk'));
const RouteAddInsuranceProvider = React.lazy(() => import('./pages/insuranceProvider/addInsuranceProvider'));
const RouteAddInsuranceType = React.lazy(() => import('./pages/insuranceType/addInsuranceType'));
const RouteAddInsuranceTerm = React.lazy(() => import('./pages/insuranceTerm/addInsuranceTerm'));
const RouteAddInsuranceAgeRange = React.lazy(() => import('./pages/insuranceAgeRange/addAgeRange'));

export default App;