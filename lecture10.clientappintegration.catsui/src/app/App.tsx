import { CameraOutlined, HomeOutlined, LoginOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Button, Layout, Menu, Space, Typography, theme } from 'antd'
import { Link, Navigate, Route, Routes, useLocation, useNavigate } from 'react-router-dom'
import FeedPage from '../pages/feed/FeedPage'
import CatsPage from '../pages/cats/CatsPage'
import ProfilePage from '../pages/profile/ProfilePage'
import LoginPage from '../pages/auth/LoginPage'

import logo from '../assets/logo-192x192.png'

const { Header, Content, Sider } = Layout
const { Title, Text } = Typography

const menuItems = [
    {
        key: '/feed',
        icon: <HomeOutlined />,
        label: <Link to="/feed">Лента</Link>,
    },
    {
        key: '/cats',
        icon: <CameraOutlined />,
        label: <Link to="/cats">Котики</Link>,
    },
    {
        key: '/profile',
        icon: <UserOutlined />,
        label: <Link to="/profile">Профиль</Link>,
    },
    {
        key: '/login',
        icon: <LoginOutlined />,
        label: <Link to="/login">Вход</Link>,
    },
]

export default function App() {
    const location = useLocation()
    const navigate = useNavigate()
    const { token } = theme.useToken()

    return (
        <Layout style={{ minHeight: '100vh' }}>
            <Sider breakpoint="lg" collapsedWidth="0" width={260} style={{ background: 'linear-gradient(180deg, #111827 0%, #1f2937 100%)' }}>
                <div style={{ padding: 20 }}>
                    <Space align="center">
                        <Avatar size={48} style={{ backgroundColor: '#ff85c0' }} src={logo}></Avatar>
                        <div>
                            <Title level={4} style={{ color: 'white', margin: 0 }}>
                                Cat Space
                            </Title>
                            <Text style={{ color: 'rgba(255,255,255,0.7)' }}>учебное приложение</Text>
                        </div>
                    </Space>
                </div>

                <Menu
                    theme="dark"
                    mode="inline"
                    selectedKeys={[location.pathname]}
                    items={menuItems}
                    style={{ background: 'transparent', borderInlineEnd: 0 }}
                />
            </Sider>

            <Layout>
                <Header
                    style={{
                        background: token.colorBgContainer,
                        paddingInline: 24,
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'space-between',
                        borderBottom: `1px solid ${token.colorBorderSecondary}`,
                    }}
                >
                    <div style={{ minWidth: 220 }}>
                        <Title level={3} style={{ margin: 0, fontSize: 28, lineHeight: 1.2 }}>
                            Милые котики
                        </Title>

                        <Text
                            type="secondary"
                            style={{
                                display: 'block',
                                marginTop: 4,
                                maxWidth: 620,
                                overflow: 'hidden',
                                whiteSpace: 'nowrap',
                                textOverflow: 'ellipsis',
                            }}
                        >
                            Простое клиентское приложение для объяснения React + API integration
                        </Text>
                    </div>

                    <Space>
                        <Avatar size={40}>T</Avatar>
                        <Button type="primary" onClick={() => navigate('/profile')}>
                            Мой профиль
                        </Button>
                    </Space>
                </Header>

                <Content style={{ padding: 24 }}>
                    <Routes>
                        <Route path="/" element={<Navigate to="/feed" replace />} />
                        <Route path="/feed" element={<FeedPage />} />
                        <Route path="/cats" element={<CatsPage />} />
                        <Route path="/profile" element={<ProfilePage />} />
                        <Route path="/login" element={<LoginPage />} />
                    </Routes>
                </Content>
            </Layout>
        </Layout>
    )
}
