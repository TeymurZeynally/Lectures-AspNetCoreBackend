import { MailOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Button, Card, Col, Row, Space, Statistic, Typography } from 'antd'
import { CURRENT_USER_UID } from '../../app/constants'
import axios from 'axios'
import { useState } from 'react'

const { Title, Text } = Typography

const getUserInfo = async () => {
    const user = await axios.get(`http://localhost:5185/api/users/${CURRENT_USER_UID}`)

    console.log(user)

    return user.data
}

export default function ProfilePage() {
    const [user, setUser] = useState()

    axios.get(`http://localhost:5185/api/users/${CURRENT_USER_UID}`).then((d) => setUser(d.data))

    return (
        <Space orientation="vertical" size={24} style={{ width: '100%' }}>
            <Button onClick={getUserInfo}>Хочу запрос на сервер</Button>

            <Card style={{ borderRadius: 20 }}>
                <Space size={16} align="start">
                    <Avatar size={88} icon={<UserOutlined />} />
                    <div>
                        <Title level={2} style={{ marginBottom: 8 }}>
                            Demo User
                        </Title>
                        <Space orientation="vertical" size={4}>
                            <Text>
                                <MailOutlined /> demo@catspace.dev
                            </Text>
                            <Text type="secondary">Current fixed user uid: {CURRENT_USER_UID}</Text>
                        </Space>
                    </div>
                </Space>
            </Card>

            <Row gutter={[24, 24]}>
                <Col xs={24} md={8}>
                    <Card style={{ borderRadius: 20 }}>
                        <Statistic title="Котики" value={3} />
                    </Card>
                </Col>
                <Col xs={24} md={8}>
                    <Card style={{ borderRadius: 20 }}>
                        <Statistic title="Посты" value={12} />
                    </Card>
                </Col>
            </Row>
        </Space>
    )
}
