import React, { useState, useEffect } from 'react';
import { Menu } from 'antd';
import './sidebar.css';
import { Link } from 'react-router-dom';
import { useLocation } from 'react-router-dom';
import Divider from '@mui/material/Divider';
import { HomeOutlined, ProfileOutlined, WifiOutlined, CreditCardOutlined, CopyOutlined, BellOutlined, AccountBookOutlined, MoneyCollectOutlined, FileTextOutlined, LockOutlined, SettingOutlined, UserOutlined, UsergroupAddOutlined, AppstoreAddOutlined } from '@ant-design/icons';


// Import Icon from MUI
import HealthAndSafetyIcon from '@mui/icons-material/HealthAndSafety';
import AddModeratorIcon from '@mui/icons-material/AddModerator';
import SavingsIcon from '@mui/icons-material/Savings';
import CreditScoreIcon from '@mui/icons-material/CreditScore';
import VideoSettingsIcon from '@mui/icons-material/VideoSettings';
import FmdGoodIcon from '@mui/icons-material/FmdGood';
import HardwareIcon from '@mui/icons-material/Hardware';
import LogoutIcon from '@mui/icons-material/Logout';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import ReceiptLongIcon from '@mui/icons-material/ReceiptLong';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import HandshakeIcon from '@mui/icons-material/Handshake';
import LabelIcon from '@mui/icons-material/Label';
import IndeterminateCheckBoxIcon from '@mui/icons-material/IndeterminateCheckBox';
import PermContactCalendarIcon from '@mui/icons-material/PermContactCalendar';

function getItem(label, key, icon, children, type) {
  return {
    key,
    icon,
    children,
    label: type === 'group' ? <strong className="admin-text">{label}</strong> : label,
    type,
  };
}

const items = [
  getItem('ADMIN', '#', null, null, 'group'),
  { type: 'divider' },
  getItem('Dashboard', 'dashboard', <HomeOutlined />),
  getItem('Admin', 'sub1', <SettingOutlined />, [
    getItem('Users', 'users', <UserOutlined />),
    getItem('Users Group', 'usersGroup', <UsergroupAddOutlined />),
    getItem('Kiosk Setup', 'kioskSetup', <AppstoreAddOutlined />),
    getItem('Kiosk Hardware', 'kioskHardware', <HardwareIcon />),
    getItem('Station', 'station', <FmdGoodIcon />),
    getItem('Video slideshow setup', 'slideshow', <VideoSettingsIcon />),
  ]),
  getItem('Insurance Config', 'sub2', <AdminPanelSettingsIcon />, [
    getItem('Insurance Package', 'insurancePackage', <HealthAndSafetyIcon />),
    getItem('Insurance Provider', 'insuranceProvider', <HandshakeIcon />),
    getItem('Insurance Type', 'insuranceType', <LabelIcon />),
    getItem('Insurance Term', 'insuranceTerm', <IndeterminateCheckBoxIcon />),
    getItem('Insurance Age Range', 'insuranceAgeRange', <PermContactCalendarIcon />),
  ]),
  getItem('Transaction', 'sub3', <AccountBalanceWalletIcon />, [
    getItem('Account', 'account', <LockOutlined />),
    getItem('Loan Transaction', 'loantransaction', <CreditScoreIcon />),
    getItem('Loan Statement', 'loanstatement', <AccountBookOutlined />),
    getItem('Saving Transaction', 'savingtransaction', <SavingsIcon />),
    getItem('Saving Statement', 'savingstatement', <MoneyCollectOutlined />),
    getItem('Insurance Transaction', 'insurancetransaction', <AddModeratorIcon />),
  ]),
  getItem('Logs', 'sub4', <ProfileOutlined />, [
    getItem('Transaction Logs', 'transactionlogs', <ReceiptLongIcon />),
    getItem('Activity Logs', 'activitylogs', <CopyOutlined />),
    getItem('Notification Logs', 'notificationlogs', <BellOutlined />),
    getItem('Audit', 'audit', <CreditCardOutlined />),
  ]),
  getItem('Report', 'sub5', <FileTextOutlined />, [
    getItem('Kiosk Health', 'kioskhealth', <WifiOutlined />),
  ]),
  getItem('Logout', 'login', <LogoutIcon />),
];



const Sidebar = () => {
  const location = useLocation();
  const currentPath = location.pathname.split('/').filter(Boolean).pop();
  const [permissions, setPermissions] = useState([]);

  const parentName = items.find(item => item.children && item.children.some(subItem => subItem.key === currentPath))?.key;
  const subopen = parentName || '';

  const [openKeys, setOpenKeys] = useState(() => {
    const storedOpenKeys = localStorage.getItem('openKeys');
    return storedOpenKeys ? [storedOpenKeys] : [subopen];
  });
  const [selectedKey, setSelectedKey] = useState(() => {
    const storedSelectedKey = localStorage.getItem('selectedKey');
    return storedSelectedKey || currentPath;
  });

  const handleLoginClick = () => {
    setSelectedKey('dashboard');
    try {
      localStorage.setItem('selectedKey', 'dashboard');
    } catch (error) {
      console.log(error);
    }
  };

  const userRole = localStorage.getItem('role');

  useEffect(() => {
    // Fetch permissions from the API
    fetch('https://localhost:7017/api/AccessRule/ShowPermission')
      .then((response) => response.json())
      .then((data) => setPermissions(data))
      .catch((error) => console.error('Error fetching permissions:', error));
  }, []);

  const filteredItems = items.map((item) => {
    if (item.type === 'divider') {
      return item;
    }

    if (item.children) {
      const filteredChildren = item.children.map((subItem) => {
        const hasPermission =
          permissions.some((permission) =>
            permission.site === subItem.key &&
            permission.groupName === userRole &&
            permission.canView
          );

        return hasPermission ? subItem : null;
      }).filter(Boolean);

      return filteredChildren.length > 0 ? { ...item, children: filteredChildren } : null;
    }

    return item;
  }).filter(Boolean);

  useEffect(() => {
    localStorage.setItem('openKeys', openKeys[0]);
    localStorage.setItem('selectedKey', selectedKey);
  }, [openKeys, selectedKey]);

  const onOpenChange = (keys) => {
    const latestOpenKey = keys.find((key) => openKeys.indexOf(key) === -1);
    setOpenKeys(latestOpenKey ? [latestOpenKey] : []);
  };

  return (
    <Menu
      theme="dark"
      mode="inline"
      openKeys={openKeys}
      onOpenChange={onOpenChange}
      selectedKeys={[selectedKey]} 
    >
      {filteredItems.map((item) => {
        if (item.type === 'divider') {
          return <Divider style={{ background: 'white' }} key={item.key} />;
        }

        if (item.key === 'login') {
          return (
            <Menu.Item key={item.key} icon={item.icon} onClick={handleLoginClick}>
              <Link to={`/${item.key}`}>
                {item.label}
              </Link>
            </Menu.Item>
          );
        }

        if (item.children) {
          return (
            <Menu.SubMenu key={item.key} title={item.label} icon={item.icon}>
              {item.children.map((subItem) => (
                <Menu.Item key={subItem.key} icon={subItem.icon}>
                  <Link to={`/${subItem.key}`} onClick={() => setSelectedKey(subItem.key)}>
                    {subItem.label}
                  </Link>
                </Menu.Item>
              ))}
            </Menu.SubMenu>
          );
        } else {
          return (
            <Menu.Item key={item.key} icon={item.icon}>
              <Link to={`/${item.key}`} onClick={() => setSelectedKey(item.key)}>
                {item.label}
              </Link>
            </Menu.Item>
          );
        }
      })}
    </Menu>
  );
};

export default Sidebar;