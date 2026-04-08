import { MailOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Card, Col, Row, Space, Statistic, Typography } from 'antd'

const { Title, Text } = Typography

export default function ProfilePage() {
    return (
        <Space orientation="vertical" size={24} style={{ width: '100%' }}>
            <Card style={{ borderRadius: 20 }}>
                <Space size={16} align="start">
                    <Avatar size={88} icon={<UserOutlined />} />
                    <div>
                        <Title level={2} style={{ marginBottom: 8 }}>
                            Демо-пользователь
                        </Title>
                        <Space orientation="vertical" size={4}>
                            <Text>
                                <MailOutlined /> email@for.test.ru
                            </Text>
                            <Text type="secondary">Uid: 00000000-0000-0000-0000-000000000000</Text>
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
                        <Statistic title="Посты" value={100} />
                    </Card>
                </Col>
            </Row>
        </Space>
    )
}
