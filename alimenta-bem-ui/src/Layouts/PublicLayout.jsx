import React from "react";
import { Outlet, Link } from "react-router-dom";
import { Button, Layout, Space, Typography } from "antd";
import { HeartFilled } from "@ant-design/icons";
import "../Css/Style.css";

const { Header, Content } = Layout;
const { Title, Paragraph } = Typography;

export default function PublicLayout() {
  return (
    <Layout className="public-shell">
      <Header className="public-header">
        <div className="public-brand">
          <HeartFilled className="title-icon" />
          <span>AlimentaBem</span>
        </div>
        <Space>
          <Link to="/login">
            <Button type="default">Entrar</Button>
          </Link>
          <Link to="/create-user">
            <Button type="primary">Criar conta</Button>
          </Link>
        </Space>
      </Header>

      <Content className="public-content">
        <section className="hero-card" aria-label="Mensagem principal">
          <Title level={1}>Doar fica mais simples quando a tecnologia aproxima.</Title>
          <Paragraph>
            Uma plataforma acessível para conectar pessoas, doações e organizações
            com impacto real.
          </Paragraph>
        </section>

        <section className="form-container" aria-label="Área de autenticação">
          <Outlet />
        </section>
      </Content>
    </Layout>
  );
}
