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
                <Route path="/login" element={<Login />} />

              </Routes>
              </div>
            </Grid>
          </Grid>
        </Box>
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

const RouteStation = () => {
  const [Station, setStation] = React.useState(null);
  React.useEffect(() => {
    import('./pages/station/station').then((module) => {
      setStation(() => module.default);
    });
  }, []);

  if (!Station) {
    return null;
  }
  return <Station />
};

const RouteAccount = () => {
  const [Account, setAccount] = React.useState(null);
  React.useEffect(() => {
    import('./pages/Account/account').then((module) => {
      setAccount(() => module.default);
    });
  }, []);

  if (!Account) {
    return null;
  }
  return <Account />
};

const RouteLoanTransaction = () => {
  const [LoanTransaction, setLoanTransaction] = React.useState(null);
  React.useEffect(() => {
    import('./pages/LoanTransaction/loanTransaction').then((module) => {
      setLoanTransaction(() => module.default);
    });
  }, []);

  if (!LoanTransaction) {
    return null;
  }
  return <LoanTransaction />
};

const RouteLoanStatement = () => {
  const [LoanStatement, setLoanStatement] = React.useState(null);
  React.useEffect(() => {
    import('./pages/loanStatement/loanStatement').then((module) => {
      setLoanStatement(() => module.default);
    });
  }, []);

  if (!LoanStatement) {
    return null;
  }
  return <LoanStatement />
};

const RouteSavingTransaction = () => {
  const [SavingTransaction, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/savingTransaction/savingTransaction').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!SavingTransaction) {
    return null;
  }
  return <SavingTransaction />
};

const RouteSavingStatement = () => {
  const [SavingStatement, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/savingStatement/savingStatement').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!SavingStatement) {
    return null;
  }
  return <SavingStatement />
};

const RouteTransactionLogs = () => {
  const [TransactionLogs, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/transactionLogs/transactionLogs').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!TransactionLogs) {
    return null;
  }
  return <TransactionLogs />
};

const RouteActivityLogs = () => {
  const [TransactionLogs, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/activityLogs/activityLogs').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!TransactionLogs) {
    return null;
  }
  return <TransactionLogs />
};

const RouteNotificationLogs = () => {
  const [TransactionLogs, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/notificationLogs/notificationLogs').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!TransactionLogs) {
    return null;
  }
  return <TransactionLogs />
};

const RouteAudit = () => {
  const [TransactionLogs, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/audit/audit').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!TransactionLogs) {
    return null;
  }
  return <TransactionLogs />
};

const RouteKioskHealth = () => {
  const [KioskHealth, setModule] = React.useState(null);
  React.useEffect(() => {
    import('./pages/kioskHealth/kioskHealth').then((module) => {
      setModule(() => module.default);
    });
  }, []);

  if (!KioskHealth) {
    return null;
  }
  return <KioskHealth />
};

export default App;