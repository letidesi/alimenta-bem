import React, { useEffect, useMemo, useState } from "react";
import { Button, Drawer, Layout, Menu, Typography, Grid, Space } from "antd";
import { MenuOutlined, LogoutOutlined, HeartFilled } from "@ant-design/icons";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import { getAuthHeaders } from "../Utils/auth";
import "../Css/Style.css";

const { Sider, Header, Content } = Layout;
const { Title, Text } = Typography;

export default function DashboardLayout({ menuItems, roleLabel }) {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [userName, setUserName] = useState("Usuário");
  const navigate = useNavigate();
  const location = useLocation();
  const screens = Grid.useBreakpoint();
  const isMobile = !screens.md;

  useEffect(() => {
    const token = localStorage.getItem("accessToken");

    if (!token) {
      navigate("/login", { replace: true });
      return;
    }

    const parseJwt = (jwt) => {
      try {
        return JSON.parse(atob(jwt.split(".")[1]));
      } catch {
        return null;
      }
    };

    const decodedToken = parseJwt(token);
    const userId = decodedToken?.sub || decodedToken?.userId || decodedToken?.id;

    if (!userId) {
      return;
    }

    const fetchUserName = async () => {
      try {
        const npResponse = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/natural-person/${userId}`,
          { headers: getAuthHeaders() }
        );
        const naturalPerson = npResponse.data;

        if (naturalPerson) {
          const { socialName, name } = naturalPerson;
          setUserName(socialName || name || "Usuário");
          return;
        }
      } catch {
        // Silently fallback to /user endpoint.
      }

      try {
        const userResponse = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/user/${userId}`,
          { headers: getAuthHeaders() }
        );
        const user = userResponse.data;
        setUserName(user?.name || "Usuário");
      } catch {
        setUserName("Usuário");
      }
    };

    fetchUserName();
  }, [navigate]);

  const selectedKey = useMemo(() => {
    const match = menuItems
      .filter((item) => location.pathname.startsWith(item.to))
      .sort((a, b) => b.to.length - a.to.length)[0];

    return match?.key ?? menuItems[0]?.key;
  }, [location.pathname, menuItems]);

  const onMenuClick = ({ key }) => {
    const item = menuItems.find((m) => m.key === key);
    if (!item) return;
    navigate(item.to);
    setMobileMenuOpen(false);
  };

  const handleSignOut = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    navigate("/login", { replace: true });
  };

  const menu = (
    <>
      <div className="brand-block">
        <Title level={4} className="brand-title">
          AlimentaBem
        </Title>
        <Text className="brand-subtitle">{roleLabel}</Text>
      </div>

      <div className="welcome-card" aria-live="polite">
        <Text className="welcome-label">Logado como</Text>
        <Text className="welcome-name">{userName}</Text>
      </div>

      <Menu
        mode="inline"
        selectedKeys={[selectedKey]}
        onClick={onMenuClick}
        items={menuItems.map((item) => ({ key: item.key, label: item.label }))}
        className="app-menu"
      />

      <Button
        icon={<LogoutOutlined />}
        onClick={handleSignOut}
        className="signout-button"
        type="default"
      >
        Sair
      </Button>
    </>
  );

  return (
    <Layout className="dashboard-shell">
      {isMobile ? (
        <Drawer
          title="Menu"
          placement="left"
          open={mobileMenuOpen}
          onClose={() => setMobileMenuOpen(false)}
          bodyStyle={{ padding: 16 }}
        >
          {menu}
        </Drawer>
      ) : (
        <Sider width={300} className="app-sider" theme="light">
          {menu}
        </Sider>
      )}

      <Layout>
        <Header className="app-header">
          <Space size={16} align="center">
            {isMobile && (
              <Button
                icon={<MenuOutlined />}
                onClick={() => setMobileMenuOpen(true)}
                aria-label="Abrir menu"
              />
            )}
            <Title level={3} className="page-title">
              <HeartFilled className="title-icon" /> Comunidade que alimenta
            </Title>
          </Space>
        </Header>

        <Content className="app-content" role="main">
          <div className="content-container">
            <Outlet />
          </div>
        </Content>
      </Layout>
    </Layout>
  );
}