import React, { useState, useEffect } from 'react';
import { Menu } from 'antd';
import './sidebar.css';
import { Outlet, Link } from 'react-router-dom';
import { useNavigate, useLocation } from 'react-router-dom';
import Divider from '@mui/material/Divider';
import { AppstoreOutlined, HomeOutlined, ProfileOutlined, WifiOutlined, CreditCardOutlined, CopyOutlined, BellOutlined, UnorderedListOutlined, AccountBookOutlined, MoneyCollectOutlined, FileTextOutlined, LockOutlined, SettingOutlined, UserOutlined, UsergroupAddOutlined, AppstoreAddOutlined } from '@ant-design/icons';


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
    getItem('Users Group', 'usersgroup', <UsergroupAddOutlined />),
    getItem('Kiosk Setup', 'kiosksetup', <AppstoreAddOutlined />),
    getItem('Kiosk Hardware', 'kioskhardware', <HardwareIcon />),
    getItem('Station', 'station', <FmdGoodIcon />),
    getItem('Video slideshow setup', 'slideshow', <VideoSettingsIcon />),
  ]),
  getItem('Transaction', 'sub2', <AccountBalanceWalletIcon />, [
    getItem('Account', 'account', <LockOutlined />),
    getItem('Loan Transaction', 'loantransaction', <CreditScoreIcon />),
    getItem('Loan Statement', 'loanstatement', <AccountBookOutlined />),
    getItem('Saving Transaction', 'savingtransaction', <SavingsIcon />),
    getItem('Saving Statement', 'savingstatement', <MoneyCollectOutlined />),
    getItem('Insurance Transaction', 'insurancetransaction', <AddModeratorIcon />),
    getItem('Insurance Package', 'insurancepackage', <HealthAndSafetyIcon />),
  ]),
  getItem('Logs', 'sub3', <ProfileOutlined />, [
    getItem('Transaction Logs', 'transactionlogs', <ReceiptLongIcon />),
    getItem('Activity Logs', 'activitylogs', <CopyOutlined />),
    getItem('Notification Logs', 'notificationlogs', <BellOutlined />),
    getItem('Audit', 'audit', <CreditCardOutlined />),
  ]),
  getItem('Report', 'sub4', <FileTextOutlined />, [
    getItem('Kiosk Health', 'kioskhealth', <WifiOutlined />),
  ]),
  getItem('Logout', 'login', <LogoutIcon />),
];



const Sidebar = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const currentPath = location.pathname.split('/').filter(Boolean).pop();
  const parentName = items.find(item => item.children && item.children.some(subItem => subItem.key === currentPath))?.key;
  const subopen = parentName || '';

  // Load openKeys and selectedKey from localStorage on component mount
  const [openKeys, setOpenKeys] = useState(() => {
    const storedOpenKeys = localStorage.getItem('openKeys');
    return storedOpenKeys ? [storedOpenKeys] : [subopen];
  });
  const [selectedKey, setSelectedKey] = useState(() => {
    const storedSelectedKey = localStorage.getItem('selectedKey');
    return storedSelectedKey || currentPath;
  });
  const [defaultOpenKeys] = useState([]);

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
      defaultOpenKeys={defaultOpenKeys}
      defaultSelectedKeys={[selectedKey]}
    >
      {items.map((item) => {
        if (item.type === 'divider') {
          return <Divider style={{ background: 'white' }} key={item.key} />;
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