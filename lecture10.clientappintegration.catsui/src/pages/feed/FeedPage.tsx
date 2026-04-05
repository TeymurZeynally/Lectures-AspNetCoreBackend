import { HeartOutlined, MessageOutlined, PlusOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Button, Card, Col, Input, Row, Space, Tag, Typography } from 'antd'

const { Title, Paragraph, Text } = Typography
const { TextArea } = Input

const mockPosts = [
    {
        uid: 'post-1',
        title: 'Милo нашёл лучшее солнечное место',
        description: 'Сегодня Мило провёл три часа у окна и выглядел чрезвычайно важным.',
        photoUrl: 'https://images.unsplash.com/photo-1519052537078-e6302a4968d4?auto=format&fit=crop&w=1200&q=80',
        catNames: ['Мило'],
        createdAt: '2 часа назад',
    },
    {
        uid: 'post-2',
        title: 'Луна снова всех осуждает',
        description: 'Очень элегантная поза. Очень твёрдое мнение. Объяснений не поступало.',
        photoUrl: 'https://images.unsplash.com/photo-1495360010541-f48722b34f7d?auto=format&fit=crop&w=1200&q=80',
        catNames: ['Луна', 'Нори'],
        createdAt: '5 часов назад',
    },
]

export default function FeedPage() {
    return (
        <Row gutter={[24, 24]} justify="center">
            <Col xs={24} sm={24} md={20} lg={16} xl={14} xxl={12}>
                <Space orientation="vertical" size={24} style={{ width: '100%' }}>
                    <Card
                        style={{ borderRadius: 20 }}
                        title="Создать публикацию"
                        extra={
                            <Button type="primary" icon={<PlusOutlined />}>
                                Опубликовать
                            </Button>
                        }
                    >
                        <Space orientation="vertical" size={12} style={{ width: '100%' }}>
                            <Input placeholder="Заголовок публикации" />
                            <TextArea rows={4} placeholder="Напишите что-нибудь о вашем коте..." />
                            <Input placeholder="URL фотографии" />
                            <Input placeholder="UID котов или выбранные коты" />
                        </Space>
                    </Card>

                    {mockPosts.map((post) => (
                        <Card
                            key={post.uid}
                            style={{ borderRadius: 20, overflow: 'hidden' }}
                            cover={<img src={post.photoUrl} alt={post.title} style={{ height: 400, objectFit: 'cover' }} />}
                        >
                            <Space orientation="vertical" size={16} style={{ width: '100%' }}>
                                <Space
                                    align="center"
                                    style={{
                                        justifyContent: 'space-between',
                                        width: '100%',
                                    }}
                                >
                                    <Space>
                                        <Avatar icon={<UserOutlined />} />
                                        <div>
                                            <Text strong>Демо-пользователь</Text>
                                            <br />
                                            <Text type="secondary">{post.createdAt}</Text>
                                        </div>
                                    </Space>
                                </Space>

                                <div>
                                    <Title level={4} style={{ marginBottom: 8 }}>
                                        {post.title}
                                    </Title>
                                    <Paragraph style={{ marginBottom: 12 }}>{post.description}</Paragraph>
                                    <Space wrap>
                                        {post.catNames.map((catName) => (
                                            <Tag key={catName} color="magenta">
                                                {catName}
                                            </Tag>
                                        ))}
                                    </Space>
                                </div>

                                <Space size="large">
                                    <Button icon={<HeartOutlined />}>Нравится</Button>
                                    <Button icon={<MessageOutlined />}>Комментировать</Button>
                                </Space>
                            </Space>
                        </Card>
                    ))}
                </Space>
            </Col>
        </Row>
    )
}
