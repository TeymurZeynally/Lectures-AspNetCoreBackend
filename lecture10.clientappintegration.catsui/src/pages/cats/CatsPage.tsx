import { EditOutlined, PlusOutlined } from '@ant-design/icons'
import { Avatar, Button, Card, Col, Input, Row, Space, Tag, Typography } from 'antd'

const { Title, Text } = Typography

const mockCats = [
    {
        uid: 'cat-1',
        name: 'Milo',
        breed: 'British Shorthair',
        age: 3,
    },
    {
        uid: 'cat-2',
        name: 'Luna',
        breed: 'Maine Coon',
        age: 2,
    },
    {
        uid: 'cat-3',
        name: 'Nori',
        breed: 'Scottish Fold',
        age: 1,
    },
]

export default function CatsPage() {
    return (
        <Space orientation="vertical" size={24} style={{ width: '100%' }}>
            <Card style={{ borderRadius: 20 }}>
                <Row gutter={[16, 16]} align="middle" justify="space-between">
                    <Col xs={24} md={16}>
                        <Space orientation="vertical" size={4}>
                            <Title level={3} style={{ margin: 0 }}>
                                Мои котики
                            </Title>
                            <Text type="secondary">First step layout: simple cards and inline inputs, no dialogs.</Text>
                        </Space>
                    </Col>
                    <Col xs={24} md={8}>
                        <Space
                            style={{
                                width: '100%',
                                justifyContent: 'flex-end',
                            }}
                        >
                            <Input placeholder="Search cats" style={{ width: 220 }} />
                            <Button type="primary" icon={<PlusOutlined />}>
                                Поиск котиков
                            </Button>
                        </Space>
                    </Col>
                </Row>
            </Card>

            <Row gutter={[24, 24]}>
                {mockCats.map((cat) => (
                    <Col xs={24} md={12} xl={8} key={cat.uid}>
                        <Card
                            style={{ borderRadius: 20 }}
                            actions={[
                                <Button type="link" icon={<EditOutlined />} key="edit">
                                    Edit inline
                                </Button>,
                            ]}
                        >
                            <Space orientation="vertical" size={16} style={{ width: '100%' }}>
                                <Space>
                                    <Avatar
                                        size={56}
                                        style={{
                                            backgroundColor: '#ffd666',
                                            color: '#593815',
                                        }}
                                    >
                                        {cat.name[0]}
                                    </Avatar>
                                    <div>
                                        <Title level={4} style={{ margin: 0 }}>
                                            {cat.name}
                                        </Title>
                                        <Text type="secondary">UID: {cat.uid}</Text>
                                    </div>
                                </Space>

                                <Space wrap>
                                    <Tag color="gold">{cat.breed}</Tag>
                                    <Tag color="blue">{cat.age} years old</Tag>
                                </Space>
                            </Space>
                        </Card>
                    </Col>
                ))}
            </Row>
        </Space>
    )
}
