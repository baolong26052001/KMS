import React, { useState, useEffect } from 'react';
import './App.css';
import Sidebar from './components/sidebar/Sidebar';
import Headerbar from './components/header/Header';
import Login from './pages/login/login';
import { Navigate, useNavigate } from "react-router-dom";
import Grid from '@mui/material/Unstable_Grid2'; // Grid version 2
import Box from '@mui/material/Box';
import { useAuth } from './components/AuthContext/AuthContext';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NoPermission from './components/redirect/noPermission';

import RouteDashboard from './pages/dashboard/dashboard';
import RouteUser from './pages/user/User';
import RouteViewUser from './pages/user/viewUser';
import RouteEditUser from './pages/user/editUser';
import RouteAddUser from './pages/user/addUser';

import RouteUsergroup from './pages/usergroup/Usergroup';
import RouteAddGroup from './pages/usergroup/addGroup';
import RouteEditGroup from './pages/usergroup/editGroup';
import RoutePermission from './pages/usergroup/permission';

import RouteKioskSetup from './pages/kiosk-setup/kiosk-setup';
import RouteViewKioskDetails from './pages/kiosk-setup/viewKioskDetails';
import RouteEditKiosk from './pages/kiosk-setup/editKiosk';
import RouteAddKiosk from './pages/kiosk-setup/addKiosk';

import RouteKioskHardware from './pages/kiosk-hardware/kioskHardware';
import RouteViewKioskHardware from './pages/kiosk-hardware/viewKioskHardware';

import RouteStation from './pages/station/station';
import RouteViewStation from './pages/station/viewStation';
import RouteEditStation from './pages/station/editStation';
import RouteAddStation from './pages/station/addStation';

import RouteSlideshow from './pages/slideshow/slideshow';
import RouteViewSlideShow from './pages/slideshow/viewSlideShow';
import RouteEditSlideShow from './pages/slideshow/editSlideShow';
import RouteAddSlideShow from './pages/slideshow/addSlideShow';

import RouteSlideDetail from './pages/slideDetail/slideDetail';
import RouteEditSlideDetail from './pages/slideDetail/editSlide';
import RouteAddSlideDetail from './pages/slideDetail/addSlide';

import RouteAccount from './pages/Account/account';
import RouteViewAccount from './pages/Account/viewAccount';

import RouteLoanTransaction from './pages/LoanTransaction/loanTransaction';
import RouteSavingTransaction from './pages/savingTransaction/savingTransaction';
import RouteLoanStatement from './pages/loanStatement/loanStatement';
import RouteSavingStatement from './pages/savingStatement/savingStatement';

import RouteTransactionLogs from './pages/transactionLogs/transactionLogs';
import RouteActivityLogs from './pages/activityLogs/activityLogs';
import RouteNotificationLogs from './pages/notificationLogs/notificationLogs';
import RouteAudit from './pages/audit/audit';
import RouteKioskHealth from './pages/kioskHealth/kioskHealth';

import RouteInsuranceTransaction from './pages/insuranceTransaction/insuranceTransaction';
import RouteInsurancePackage from './pages/InsurancePackage/insurancePackage';
import RouteViewInsurancePackage from './pages/InsurancePackage/insuranceBenefit';
import RouteAddInsurancePackage from './pages/InsurancePackage/addInsurancePackage';
import RouteEditInsurancePackage from './pages/InsurancePackage/editInsurancePackage';

import RouteInsuranceDetail from './pages/insurancePackageDetail/insurancePackageDetail';
import RouteAddInsurancePackageDetail from './pages/insurancePackageDetail/addInsurancePackageDetail';
import RouteEditInsurancePackageDetail from './pages/insurancePackageDetail/editInsurancePackageDetail';

import RouteBenefitDetail from './pages/InsurancePackage/benefitDetail';
import RouteAddBenefit from './pages/InsurancePackage/addBenefit';
import RouteEditBenefit from './pages/InsurancePackage/editBenefit';

import RouteAddBenefitDetail from './pages/InsurancePackage/addBenefitDetail';
import RouteEditBenefitDetail from './pages/InsurancePackage/editBenefitDetail';

import RouteInsuranceProvider from './pages/insuranceProvider/insuranceProvider';
import RouteAddInsuranceProvider from './pages/insuranceProvider/addInsuranceProvider';
import RouteEditInsuranceProvider from './pages/insuranceProvider/editInsuranceProvider';

import RouteInsuranceType from './pages/insuranceType/insuranceType';
import RouteAddInsuranceType from './pages/insuranceType/addInsuranceType';
import RouteEditInsuranceType from './pages/insuranceType/editInsuranceType';

import RouteInsuranceTerm from './pages/insuranceTerm/insuranceTerm';
import RouteAddInsuranceTerm from './pages/insuranceTerm/addInsuranceTerm';
import RouteEditInsuranceTerm from './pages/insuranceTerm/editInsuranceTerm';

import RouteInsuranceAgeRange from './pages/insuranceAgeRange/insuranceAgeRange';
import RouteAddInsuranceAgeRange from './pages/insuranceAgeRange/addAgeRange';
import RouteEditInsuranceAgeRange from './pages/insuranceAgeRange/editAgeRange';

import RouteLogout from './components/logout/logout';

function hasPermission(permissionData, path, isEdit = false) {
  if (!getCookie('groupId')) {
    return false;
  }
  
  // Check if groupId is 1 - Admin
  if (getCookie('groupId') === '1') {
    return true; // Allow permission to all routes
  }
  
  const formattedPath = path.startsWith("/") ? path.substring(1) : path;
  
  // Check if there is permission data for the given path
  if (Array.isArray(permissionData) && permissionData.length > 0) {
    const permission = permissionData.find(permission => permission.site === formattedPath);
    if (permission) {
      if (isEdit) {
        return permission.canUpdate;
      } else {
        return permission.canView;
      }
    }
  }
  
  return false;
}

function fetchPermissionInfo(groupId) {
  const apiUrl = `https://localhost:7017/api/AccessRule/ShowPermissionInfoInEditPage/${groupId}`;
  return fetch(apiUrl)
    .then(response => response.json())
    .catch(error => {
      console.error('Error fetching permission info:', error);
    });
}

const App = () => {
  const { isAuthenticated, login, logout  } = useAuth();
  const [showHeaderbar, setShowHeaderbar] = useState(isAuthenticated);
  const groupId = getCookie('groupId');
  const [permissionData, setPermissionData] = useState([]);

  // Fetch permission info on component mount
  useEffect(() => {
    setShowHeaderbar(isAuthenticated);
    fetchPermissionInfo(groupId)
      .then(data => {
        setPermissionData(data);
      });
  }, [isAuthenticated, groupId]);
  
  return (
    <div>
      <Router>
        <div>
        <Routes>
          <Route path="/login" element={<Login onLogin={login} />} />
        </Routes>
        {isAuthenticated ? ( 
          <React.Fragment>
            <Box>
              <Grid container spacing={0}>
                <Grid xs={2}>
                  <Sidebar />
                </Grid>
                <Grid xs={10}>
                  {showHeaderbar && <Headerbar onLogout={logout} />}
                  <div className='Dashboard-table'>
                    <React.Suspense>      
                      <Routes> 
                        <Route path="/" element={<RouteDashboard />} />
                        <Route path="/dashboard" element={<RouteDashboard />} />

                        {/* Routes for User */}
                        <Route 
                          path="/users" 
                          element={hasPermission(permissionData, "/users") ? <RouteUser /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewUser/:id" 
                          element={hasPermission(permissionData, "/users") ? <RouteViewUser /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editUser/:id" 
                          element={hasPermission(permissionData, "/users", true) ? <RouteEditUser /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addUser" 
                          element={hasPermission(permissionData, "/users") ? <RouteAddUser /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for UserGroup */}
                        <Route 
                          path="/usersgroup" 
                          element={hasPermission(permissionData, "/usersGroup") ? <RouteUsergroup /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addGroup" 
                          element={hasPermission(permissionData, "/usersGroup") ? <RouteAddGroup /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editGroup/:id" 
                          element={hasPermission(permissionData, "/usersGroup", true) ? <RouteEditGroup /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/permission/:id" 
                          element={hasPermission(permissionData, "/usersGroup") ? <RoutePermission /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for Kiosk Setup */}
                        <Route 
                          path="/kiosksetup" 
                          element={hasPermission(permissionData, "/kioskSetup") ? <RouteKioskSetup /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewKioskDetails/:id" 
                          element={hasPermission(permissionData, "/kioskSetup") ? <RouteViewKioskDetails /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editKiosk/:id" 
                          element={hasPermission(permissionData, "/kioskSetup", true) ? <RouteEditKiosk /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addKiosk" 
                          element={hasPermission(permissionData, "/kioskSetup") ? <RouteAddKiosk /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for Kiosk Hardware */}
                        <Route 
                          path="/kioskhardware" 
                          element={hasPermission(permissionData, "/kioskHardware") ? <RouteKioskHardware /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewKioskHardware/:id" 
                          element={hasPermission(permissionData, "/kioskHardware") ? <RouteViewKioskHardware /> : <NoPermission/>} 
                        />

                        {/* Routes for Station */}
                        <Route 
                          path="/station" 
                          element={hasPermission(permissionData, "/station") ? <RouteStation /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewStation/:id" 
                          element={hasPermission(permissionData, "/station") ? <RouteViewStation /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editStation/:id" 
                          element={hasPermission(permissionData, "/station", true) ? <RouteEditStation /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addStation" 
                          element={hasPermission(permissionData, "/station") ? <RouteAddStation /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for Slideshow */}
                        <Route 
                          path="/slideshow" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteSlideshow /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewSlideShow/:id" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteViewSlideShow /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editSlideShow/:id" 
                          element={hasPermission(permissionData, "/slideshow", true) ? <RouteEditSlideShow /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addSlideShow" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteAddSlideShow /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for Slide Detail */}
                        <Route 
                          path="/slideDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteSlideDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addSlideDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteAddSlideDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editSlideDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteEditSlideDetail /> : <NoPermission/>} 
                        />

                        {/* Routes for Account */}
                        <Route 
                          path="/account" 
                          element={hasPermission(permissionData, "/account") ? <RouteAccount /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewAccount/:id" 
                          element={hasPermission(permissionData, "/account") ? <RouteViewAccount /> : <NoPermission/>} 
                        />

                        {/* Routes for Loan */}
                        <Route 
                          path="/loantransaction" 
                          element={hasPermission(permissionData, "/loantransaction") ? <RouteLoanTransaction /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/savingtransaction" 
                          element={hasPermission(permissionData, "/savingtransaction") ? <RouteSavingTransaction /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/loanstatement" 
                          element={hasPermission(permissionData, "/loanstatement") ? <RouteLoanStatement /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/savingstatement" 
                          element={hasPermission(permissionData, "/savingstatement") ? <RouteSavingStatement /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Transaction */}
                        <Route 
                          path="/insuranceTransaction" 
                          element={hasPermission(permissionData, "/insuranceTransaction") ? <RouteInsuranceTransaction /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Package */}
                        <Route 
                          path="/insurancePackage" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteInsurancePackage /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewPackageDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteViewInsurancePackage /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsurancePackage" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteAddInsurancePackage /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editInsurancePackage/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", true) ? <RouteEditInsurancePackage /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for Insurance Package Detail */}
                        <Route 
                          path="/insurancePackageDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteInsuranceDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsurancePackageDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteAddInsurancePackageDetail /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editInsurancePackageDetail/:id/:packageHeaderId" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteEditInsurancePackageDetail /> : <Navigate to="/dashboard" />} 
                        />

                        {/* Routes for Insurance Benefit */}
                        <Route 
                          path="/benefitDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteBenefitDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addBenefit/:id/:packageName" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteAddBenefit /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editBenefit/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", true) ? <RouteEditBenefit /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Benefit Detail */}
                        <Route 
                          path="/addBenefitDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteAddBenefitDetail /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editBenefitDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", true) ? <RouteEditBenefitDetail /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Provider */}
                        <Route 
                          path="/insuranceProvider" 
                          element={hasPermission(permissionData, "/insuranceProvider") ? <RouteInsuranceProvider /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsuranceProvider" 
                          element={hasPermission(permissionData, "/insuranceProvider") ? <RouteAddInsuranceProvider /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editInsuranceProvider/:id" 
                          element={hasPermission(permissionData, "/insuranceProvider", true) ? <RouteEditInsuranceProvider /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Type */}
                        <Route 
                          path="/insuranceType" 
                          element={hasPermission(permissionData, "/insuranceType") ? <RouteInsuranceType /> : <NoPermission />} 
                        />
                        <Route 
                          path="/addInsuranceType" 
                          element={hasPermission(permissionData, "/insuranceType") ? <RouteAddInsuranceType /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editInsuranceType/:id" 
                          element={hasPermission(permissionData, "/insuranceType", true) ? <RouteEditInsuranceType /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Term */}
                        <Route 
                          path="/insuranceTerm" 
                          element={hasPermission(permissionData, "/insuranceTerm") ? <RouteInsuranceTerm /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsuranceTerm" 
                          element={hasPermission(permissionData, "/insuranceTerm") ? <RouteAddInsuranceTerm /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editInsuranceTerm/:id" 
                          element={hasPermission(permissionData, "/insuranceTerm", true) ? <RouteEditInsuranceTerm /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Age Range */}
                        <Route 
                          path="/insuranceAgeRange" 
                          element={hasPermission(permissionData, "/insuranceAgeRange") ? <RouteInsuranceAgeRange /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addAgeRange" 
                          element={hasPermission(permissionData, "/insuranceAgeRange") ? <RouteAddInsuranceAgeRange /> : <Navigate to="/dashboard" />} 
                        />
                        <Route 
                          path="/editAgeRange/:id" 
                          element={hasPermission(permissionData, "/insuranceAgeRange", true) ? <RouteEditInsuranceAgeRange /> : <NoPermission/>} 
                        />

                        {/* Routes for Logs */}
                        <Route 
                          path="/transactionlogs" 
                          element={hasPermission(permissionData, "/transactionlogs") ? <RouteTransactionLogs /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/activitylogs" 
                          element={hasPermission(permissionData, "/activitylogs") ? <RouteActivityLogs /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/notificationlogs" 
                          element={hasPermission(permissionData, "/notificationlogs") ? <RouteNotificationLogs /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/audit" 
                          element={hasPermission(permissionData, "/audit") ? <RouteAudit /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/kioskHealth" 
                          element={hasPermission(permissionData, "/kioskHealth") ? <RouteKioskHealth /> : <NoPermission/>} 
                        />

                        <Route path="/logout" element={<RouteLogout onLogout={logout} />} />
                      </Routes>
                    </React.Suspense>
                  </div>
                </Grid>
              </Grid>
            </Box>
          </React.Fragment>
        ) : (
          <Navigate to="/login" />
        )}
        </div>
      </Router>
    </div>
  );
}

// Function to get cookie value by name
function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
}

export default App;