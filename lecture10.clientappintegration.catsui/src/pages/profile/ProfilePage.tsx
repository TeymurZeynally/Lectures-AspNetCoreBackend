import { MailOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Card, Col, Row, Space, Statistic, Typography } from 'antd'
import axios from 'axios'
import { CURRENT_USER_UID } from '../../app/constants'
import { useEffect, useState } from 'react'

const { Title, Text } = Typography

type User = {
    createdAt: string
    email: string
    uid: string
    username: string
}

export default function ProfilePage() {
    const [user, setUser] = useState<User>()
    const [postsCount, setPostsCount] = useState<number>()

    useEffect(() => {
        axios.get(`http://localhost:5185/api/users/${CURRENT_USER_UID}`).then((x) => setUser(x.data))
        axios.get(`http://localhost:5185/api/posts?UserUid=${CURRENT_USER_UID}&Page=1&PageSize=1`).then((x) => setPostsCount(x.data.totalCount))
    }, [])

    return (
        <Space orientation="vertical" size={24} style={{ width: '100%' }}>
            <Card style={{ borderRadius: 20 }}>
                <Space size={16} align="start">
                    <Avatar size={88} icon={<UserOutlined />} />
                    <div>
                        <Title level={2} style={{ marginBottom: 8 }}>
                            {user?.username}
                        </Title>

                        <Space orientation="vertical" size={4}>
                            <Text>
                                <MailOutlined /> {user?.email}
                            </Text>
                            <Text type="secondary">Uid: {user?.uid}</Text>
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
                        <Statistic title="Посты" value={postsCount} />
                    </Card>
                </Col>
            </Row>
        </Space>
    )
}
