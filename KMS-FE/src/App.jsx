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
import { API_URL } from './components/config/apiUrl';
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
import RouteViewSlideDetail from './pages/slideDetail/viewSlideDetail';
import RouteEditSlideDetail from './pages/slideDetail/editSlide';
import RouteAddSlideDetail from './pages/slideDetail/addSlide';

import RouteAccount from './pages/Account/account';
import RouteViewAccount from './pages/Account/viewAccount';
import RouteEditAccount from './pages/Account/editAccount';

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
import RouteBeneficiary from './pages/insuranceTransaction/beneficiary';
import RouteViewInsuranceTransaction from './pages/insuranceTransaction/viewInsuranceTransaction';
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

function hasPermission(permissionData, path, isEdit = false, isAdd = false) {
  if (!getCookie('groupId')) {
    return false;
  }
  
  // Check if groupId is 1 - Admin
  if (getCookie('groupId') === '1') {
    return true; // Allow permission to all routes
  } else if (path === "/usersGroup") {
    return false;
  }
  
  const formattedPath = path.startsWith("/") ? path.substring(1) : path;
  
  // Check if there is permission data for the given path
  if (Array.isArray(permissionData) && permissionData.length > 0) {
    const permission = permissionData.find(permission => permission.site === formattedPath);
    if (permission) {
      if (isEdit) {
        return permission.canUpdate;
      } else if (isAdd) {
        return permission.canAdd;
      } else {
        return permission.canView;
      }
    }
  }
  
  return false;
}

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
}

function fetchPermissionInfo(groupId) {
  const apiUrl = `${API_URL}api/AccessRule/ShowPermissionInfoInEditPage/${groupId}`;
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
  const editPermission = true;
  const addPermission = true;
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setShowHeaderbar(isAuthenticated);
    fetchPermissionInfo(groupId)
      .then(data => {
        setPermissionData(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching permission info:', error);
        setLoading(false);
      });
  
    let lastUserActionTime = Date.now();
    let inactivityTimeout;
  
    const resetInactivityTimeout = () => {
      clearTimeout(inactivityTimeout);
      const elapsedTime = Date.now() - lastUserActionTime;
      const remainingTime = 3600 * 1000 - elapsedTime;
      if (remainingTime > 0) {
        inactivityTimeout = setTimeout(() => {
          alert('Login session expired');
          if (isAuthenticated) {
            logout();
            localStorage.clear();
          }
        }, remainingTime);
      } else {
        if (isAuthenticated) {
          logout();
          localStorage.clear();
        }
      }
    };
  
    const userActionListener = () => {
      lastUserActionTime = Date.now();
      resetInactivityTimeout();
    };
  
    const handleVisibilityChange = () => {
      if (!document.hidden) {
        userActionListener();
      }
    };
  
    const unloadHandler = () => {
      localStorage.setItem('tabClosedTime', Date.now());
    };
  
    const checkTabClosedTime = () => {
      const tabClosedTime = localStorage.getItem('tabClosedTime');
      if (tabClosedTime) {
        const elapsedTime = Date.now() - parseInt(tabClosedTime);
        if (elapsedTime > 3600000 && isAuthenticated) {
          logout();
          localStorage.clear();
        }
      }
    };
  
    document.addEventListener('mousemove', userActionListener);
    document.addEventListener('keypress', userActionListener);
    document.addEventListener('visibilitychange', handleVisibilityChange);
    window.addEventListener('unload', unloadHandler);
  
    checkTabClosedTime();
  
    resetInactivityTimeout();
  
    return () => {
      clearTimeout(inactivityTimeout);
      document.removeEventListener('mousemove', userActionListener);
      document.removeEventListener('keypress', userActionListener);
      document.removeEventListener('visibilitychange', handleVisibilityChange);
      window.removeEventListener('unload', unloadHandler);
    };
  
  }, [isAuthenticated, groupId, logout]);

  if (loading) {
    return (
      <div>
        <div className="skeleton"></div>
      </div>
    );
  }

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
                          // Check if the user has permission to edit a user.
                          // If `editPermission` is true, allow access to the edit user page, otherwise redirect to NoPermission page.
                          element={hasPermission(permissionData, "/users", editPermission) ? <RouteEditUser /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addUser" 
                          // Check if the user has permission to add a user.
                          // editPermission should set to false to only check addPermission
                          // If `addPermission` is true, allow access to the edit user page, otherwise redirect to NoPermission page.
                          element={hasPermission(permissionData, "/users", !editPermission, addPermission) ? <RouteAddUser /> : <NoPermission />} 
                        />

                        {/* Routes for UserGroup */}
                        <Route 
                          path="/usersgroup" 
                          element={hasPermission(permissionData, "/usersGroup") ? <RouteUsergroup /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addGroup" 
                          element={hasPermission(permissionData, "/usersGroup", !editPermission, addPermission) ? <RouteAddGroup /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editGroup/:id" 
                          element={hasPermission(permissionData, "/usersGroup", editPermission) ? <RouteEditGroup /> : <NoPermission />} 
                        />
                        <Route 
                          path="/permission/:id" 
                          element={hasPermission(permissionData, "/usersGroup") ? <RoutePermission /> : <NoPermission />} 
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
                          element={hasPermission(permissionData, "/kioskSetup", editPermission) ? <RouteEditKiosk /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addKiosk" 
                          element={hasPermission(permissionData, "/kioskSetup", !editPermission, addPermission) ? <RouteAddKiosk /> : <NoPermission />} 
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
                          element={hasPermission(permissionData, "/station", editPermission) ? <RouteEditStation /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addStation" 
                          element={hasPermission(permissionData, "/station", !editPermission, addPermission) ? <RouteAddStation /> : <NoPermission />} 
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
                          element={hasPermission(permissionData, "/slideshow", editPermission) ? <RouteEditSlideShow /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addSlideShow" 
                          element={hasPermission(permissionData, "/slideshow", !editPermission, addPermission) ? <RouteAddSlideShow /> : <NoPermission />} 
                        />

                        {/* Routes for Slide Detail */}
                        <Route 
                          path="/slideDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteSlideDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewSlideDetail/:id" 
                          element={hasPermission(permissionData, "/slideshow") ? <RouteViewSlideDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addSlideDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/slideshow", editPermission) ? <RouteAddSlideDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/editSlideDetail/:id/:packageName" 
                          element={hasPermission(permissionData, "/slideshow", !editPermission, addPermission) ? <RouteEditSlideDetail /> : <NoPermission/>} 
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
                        <Route 
                          path="/editAccount/:id" 
                          element={hasPermission(permissionData, "/account", editPermission) ? <RouteEditAccount /> : <NoPermission />} 
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
                          element={hasPermission(permissionData, "/insurancetransaction") ? <RouteInsuranceTransaction /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/viewInsuranceTransaction/:id" 
                          element={hasPermission(permissionData, "/insurancetransaction") ? <RouteViewInsuranceTransaction /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/beneficiary/:id" 
                          element={hasPermission(permissionData, "/insurancetransaction") ? <RouteBeneficiary /> : <NoPermission/>} 
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
                          element={hasPermission(permissionData, "/insurancePackage", !editPermission, addPermission) ? <RouteAddInsurancePackage /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editInsurancePackage/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", editPermission) ? <RouteEditInsurancePackage /> : <NoPermission />} 
                        />

                        {/* Routes for Insurance Package Detail */}
                        <Route 
                          path="/insurancePackageDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteInsuranceDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsurancePackageDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", !editPermission, addPermission) ? <RouteAddInsurancePackageDetail /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editInsurancePackageDetail/:id/:packageHeaderId" 
                          element={hasPermission(permissionData, "/insurancePackage", editPermission) ? <RouteEditInsurancePackageDetail /> : <NoPermission />} 
                        />

                        {/* Routes for Insurance Benefit */}
                        <Route 
                          path="/benefitDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteBenefitDetail /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addBenefit/:id/:packageName" 
                          element={hasPermission(permissionData, "/insurancePackage", !editPermission, addPermission) ? <RouteAddBenefit /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editBenefit/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", editPermission) ? <RouteEditBenefit /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Benefit Detail */}
                        <Route 
                          path="/addBenefitDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage") ? <RouteAddBenefitDetail /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editBenefitDetail/:id" 
                          element={hasPermission(permissionData, "/insurancePackage", editPermission) ? <RouteEditBenefitDetail /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Provider */}
                        <Route 
                          path="/insuranceProvider" 
                          element={hasPermission(permissionData, "/insuranceProvider") ? <RouteInsuranceProvider /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsuranceProvider" 
                          element={hasPermission(permissionData, "/insuranceProvider", !editPermission, addPermission) ? <RouteAddInsuranceProvider /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editInsuranceProvider/:id" 
                          element={hasPermission(permissionData, "/insuranceProvider", editPermission) ? <RouteEditInsuranceProvider /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Type */}
                        <Route 
                          path="/insuranceType" 
                          element={hasPermission(permissionData, "/insuranceType") ? <RouteInsuranceType /> : <NoPermission />} 
                        />
                        <Route 
                          path="/addInsuranceType" 
                          element={hasPermission(permissionData, "/insuranceType", !editPermission, addPermission) ? <RouteAddInsuranceType /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editInsuranceType/:id" 
                          element={hasPermission(permissionData, "/insuranceType", editPermission) ? <RouteEditInsuranceType /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Term */}
                        <Route 
                          path="/insuranceTerm" 
                          element={hasPermission(permissionData, "/insuranceTerm") ? <RouteInsuranceTerm /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addInsuranceTerm" 
                          element={hasPermission(permissionData, "/insuranceTerm", !editPermission, addPermission) ? <RouteAddInsuranceTerm /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editInsuranceTerm/:id" 
                          element={hasPermission(permissionData, "/insuranceTerm", editPermission) ? <RouteEditInsuranceTerm /> : <NoPermission/>} 
                        />

                        {/* Routes for Insurance Age Range */}
                        <Route 
                          path="/insuranceAgeRange" 
                          element={hasPermission(permissionData, "/insuranceAgeRange") ? <RouteInsuranceAgeRange /> : <NoPermission/>} 
                        />
                        <Route 
                          path="/addAgeRange" 
                          element={hasPermission(permissionData, "/insuranceAgeRange", !editPermission, addPermission) ? <RouteAddInsuranceAgeRange /> : <NoPermission />} 
                        />
                        <Route 
                          path="/editAgeRange/:id" 
                          element={hasPermission(permissionData, "/insuranceAgeRange", editPermission) ? <RouteEditInsuranceAgeRange /> : <NoPermission/>} 
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
                          element={hasPermission(permissionData, "/kioskhealth") ? <RouteKioskHealth /> : <NoPermission/>} 
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

export default App;