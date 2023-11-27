// import './slidebar.css';
import React, { useState } from 'react';
import { AppstoreOutlined, HomeOutlined, ProfileOutlined, WifiOutlined, CreditCardOutlined, CopyOutlined, BellOutlined, AlignLeftOutlined, UnorderedListOutlined, AccountBookOutlined, MoneyCollectOutlined, FileTextOutlined, LockOutlined, MailOutlined, SettingOutlined, UserOutlined, UsergroupAddOutlined, AppstoreAddOutlined, LaptopOutlined, MenuUnfoldOutlined } from '@ant-design/icons';
import { Menu } from 'antd';
import './sidebar.css';
import { Outlet, Link } from "react-router-dom";
import { useLocation } from 'react-router-dom';
import Divider from '@mui/material/Divider';



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
  getItem('ADMIN', 'g1', null, [], 'group'),
  {
    type: 'divider',
  },
  getItem('Dashboard', '17', <HomeOutlined />),
  getItem('Admin', 'sub1', <SettingOutlined />, [
    getItem('Users', '1', <UserOutlined />),
    getItem('Users Group', '2', <UsergroupAddOutlined />),
    getItem('Kiosk Setup', '3', <AppstoreAddOutlined />),
    getItem('Kiosk Hardware', '4', <AppstoreAddOutlined />),
    getItem('Station', '5', <LaptopOutlined />),
    getItem('Video slideshow setup', '6', <MenuUnfoldOutlined />),
  ]),
  getItem('Transaction', 'sub2', <AppstoreOutlined />, [
    getItem('Account', '7', <LockOutlined />),
    getItem('Loan Transaction', '8', <FileTextOutlined />),
    getItem('Saving Transaction', '9', <MoneyCollectOutlined />),
    getItem('Loan Statement', '10', <AccountBookOutlined />),
    getItem('Saving Statement', '11', <AccountBookOutlined />),
  ]),
  getItem('Logs', 'sub3', <ProfileOutlined />, [
    getItem('Transaction Logs', '12', <UnorderedListOutlined />),
    getItem('Activity Logs', '13', <CopyOutlined />),
    getItem('Notification Logs', '14', <BellOutlined />),
    getItem('Audit', '15', <CreditCardOutlined />),
  ]),
  getItem('Report', 'sub4', <FileTextOutlined />, [
    getItem('Kiosk Health', '16', <WifiOutlined />),
    
  ]),
  getItem('Logout', '18', <HomeOutlined />),
];

// submenu keys of first level
const rootSubmenuKeys = ['sub1', 'sub2', 'sub3', 'sub4'];
const Sidebar = () => {
  let defaultSelectedKey = '0';
  let subMenuSelect = '0';
  const currentPath = window.location.pathname;
  console.log('Current Path:', currentPath);
  
  if (currentPath === '/users') {
    defaultSelectedKey = '1';
    subMenuSelect = 'sub1';
  } else if (currentPath === '/usersgroup') {
    defaultSelectedKey = '2';
    subMenuSelect = 'sub1';
  } else if (currentPath === '/kiosksetup') {
    defaultSelectedKey = '3';
    subMenuSelect = 'sub1';
  } else if (currentPath === '/kioskhardware') {
    defaultSelectedKey = '4';
    subMenuSelect = 'sub1';
  } else if (currentPath === '/station') {
    defaultSelectedKey = '5';
    subMenuSelect = 'sub1';
  } else if (currentPath === '/slideshow') {
    defaultSelectedKey = '6';
    subMenuSelect = 'sub1';
  } else if (currentPath === '/account') {
    defaultSelectedKey = '7';
    subMenuSelect = 'sub2';
  } else if (currentPath === '/loantransaction') {
    defaultSelectedKey = '8';
    subMenuSelect = 'sub2';
  } else if (currentPath === '/savingtransaction') {
    defaultSelectedKey = '9';
    subMenuSelect = 'sub2';
  } else if (currentPath === '/loanstatement') {
    defaultSelectedKey = '10';
    subMenuSelect = 'sub2';
  } else if (currentPath === '/savingstatement') {
    defaultSelectedKey = '11';
    subMenuSelect = 'sub2';
  } else if (currentPath === '/transactionlogs') {
    defaultSelectedKey = '12';
    subMenuSelect = 'sub3';
  } else if (currentPath === '/activitylogs') {
    defaultSelectedKey = '13';
    subMenuSelect = 'sub3';
  } else if (currentPath === '/notificationlogs') {
    defaultSelectedKey = '14';
    subMenuSelect = 'sub3';
  } else if (currentPath === '/audit') {
    defaultSelectedKey = '15';
    subMenuSelect = 'sub3';
  } else if (currentPath === '/kioskhealth') {
    defaultSelectedKey = '16';
    subMenuSelect = 'sub4';
  } else {
    defaultSelectedKey = '17';
  }
  

  const [openKeys, setOpenKeys] = useState([subMenuSelect]);
  const [defaultOpenKeys, setDefaultOpenKeys] = useState([]);

  //const [openKeys, setOpenKeys] = useState(['sub1']);
  //const [defaultOpenKeys, setDefaultOpenKeys] = useState(['sub1']);

  const onOpenChange = (keys) => {
    const latestOpenKey = keys.find((key) => openKeys.indexOf(key) === -1);
    setOpenKeys(latestOpenKey ? [latestOpenKey] : []);
  };


  const handleMenuClick = (e) => {
    const clickedKey = e.key;
    
    // Your logic to handle the clicked menu item
    switch (clickedKey) {
      case '17':
        window.location.href = '/';
        break;
      case '1':
        window.location.href = '/users';
        break;
      case '2':
        window.location.href = '/usersgroup';
        break;
      case '3':
        window.location.href = '/kiosksetup';
        break;
      case '4':
        window.location.href = '/kioskhardware';
        break;
      case '5':
        window.location.href = '/station';
        break;
      case '6':
        window.location.href = '/slideshow';
        break;
      case '7':
        window.location.href = '/account';
        break;
      case '8':
        window.location.href = '/loantransaction';
        break;
      case '9':
        window.location.href = '/savingtransaction';
        break;
      case '10':
        window.location.href = '/loanstatement';
        break;
      case '11':
        window.location.href = '/savingstatement';
        break;
      case '12':
        window.location.href = '/transactionlogs';
        break;
      case '13':
        window.location.href = '/activitylogs';
        break;
      case '14':
        window.location.href = '/notificationlogs';
        break;
      case '15':
        window.location.href = '/audit';
        break;
      case '16':
        window.location.href = '/kioskhealth';
        break;
      case '18':
        window.location.href = '/login';
        break;
      default:
        console.log('Key not handled:', clickedKey);
        break;
    }
  };

  return (
    
      <Menu
        theme="dark"
        mode="inline"
        openKeys={openKeys}
        onOpenChange={onOpenChange}
        defaultOpenKeys={defaultOpenKeys}
        defaultSelectedKeys={[defaultSelectedKey]}
        items={items}
        onClick={handleMenuClick}
      >
        
        {items.map((item) => {
          if (item.children) {
            return (
              <Menu.SubMenu key={item.key} title={item.label} icon={item.icon}>
                {item.children.map((subItem) => (
                  <Menu.Item key={subItem.key} icon={subItem.icon}>
                    <Link to={`/route/${subItem.key}`}>{subItem.label}</Link>
                  </Menu.Item>
                ))}
              </Menu.SubMenu>
            );
          } else {
            return (
              <Menu.Item key={item.key} icon={item.icon}>
                <Link to={`/route/${item.key}`}>{item.label}</Link>
              </Menu.Item>
            );
          }
        })}
      </Menu>
    
  );

};

export default Sidebar;