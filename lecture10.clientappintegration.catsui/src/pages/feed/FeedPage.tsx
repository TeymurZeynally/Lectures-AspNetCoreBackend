import { HeartOutlined, MessageOutlined, PlusOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Button, Card, Col, Form, Input, message, Row, Space, Spin, Tag, Typography } from 'antd'

const { Title, Paragraph, Text } = Typography

const mockPosts = [
    {
        uid: 'post-1',
        title: 'Милo нашёл лучшее солнечное место',
        description: 'Сегодня Мило провёл три часа у окна и выглядел чрезвычайно важным.',
        photoUrl:
            'data:image/jpeg;base64,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',
        catNames: ['Мило'],
        createdAt: '2 часа назад',
    },
    {
        uid: 'post-2',
        title: 'Луна снова всех осуждает',
        description: 'Очень элегантная поза. Очень твёрдое мнение. Объяснений не поступало.',
        photoUrl:
            'data:image/jpeg;base64,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',
        catNames: ['Луна', 'Нори'],
        createdAt: '5 часов назад',
    },
]

export default function FeedPage() {
    const [form] = Form.useForm()

    const [, messageContext] = message.useMessage()

    return (
        <Row gutter={[24, 24]} justify="center">
            <Col xs={24} sm={24} md={20} lg={16} xl={14} xxl={12}>
                {messageContext}

                <Space orientation="vertical" size={24} style={{ width: '100%' }}>
                    <Card
                        style={{ borderRadius: 20 }}
                        title="Создать публикацию"
                        extra={
                            <Button type="primary" htmlType="submit" form="create-post-form" icon={<PlusOutlined />}>
                                Опубликовать
                            </Button>
                        }
                    >
                        <Form form={form} id="create-post-form" layout="vertical" onFinish={(x) => console.log(x)}>
                            <Form.Item name="title" rules={[{ required: true, message: 'Введите заголовок' }]}>
                                <Input placeholder="Заголовок публикации" />
                            </Form.Item>

                            <Form.Item name="description" rules={[{ required: true, message: 'Введите описание' }]}>
                                <Input.TextArea rows={4} placeholder="Напишите что-нибудь о вашем коте..." />
                            </Form.Item>

                            <Form.Item name="imageUrl" rules={[{ required: true, message: 'Введите URL фотографии' }]}>
                                <Input placeholder="URL фотографии" />
                            </Form.Item>

                            <Form.Item name="cats" rules={[{ required: true, message: 'Введите котов' }]}>
                                <Input placeholder="Выбранные коты" />
                            </Form.Item>
                        </Form>
                    </Card>

                    <Spin spinning={false}>
                        {mockPosts.map((post) => (
                            <Card
                                key={post.uid}
                                style={{ borderRadius: 20, overflow: 'hidden' }}
                                cover={<img src={post.photoUrl!} alt={post.title!} style={{ height: 400, objectFit: 'cover' }} />}
                            >
                                <Space orientation="vertical" size={16} style={{ width: '100%' }}>
                                    <Space align="center" style={{ justifyContent: 'space-between', width: '100%' }}>
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
                                        <Paragraph style={{ marginBottom: 12 }}>
                                            <div dangerouslySetInnerHTML={{ __html: post.description } as never} />
                                        </Paragraph>
                                        <Space wrap>
                                            {post.catNames?.map((catName) => (
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
                    </Spin>
                </Space>
            </Col>
        </Row>
    )
}
