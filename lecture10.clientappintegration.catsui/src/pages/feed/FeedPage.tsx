import { HeartOutlined, MessageOutlined, PlusOutlined, UserOutlined } from '@ant-design/icons'
import { Avatar, Button, Card, Col, Form, Input, message, Row, Space, Spin, Tag, Typography } from 'antd'
import { getApiCats, getApiCatsByUid, getApiPosts, postApiPosts } from '../../shared/api/generated'
import { useQueries, useQuery } from '@tanstack/react-query'
import { data } from 'react-router-dom'
import { useMemo } from 'react'
import { CURRENT_USER_UID } from '../../app/constants'

const { Title, Paragraph, Text } = Typography

export default function FeedPage() {
    const [form] = Form.useForm()

    const [messageApi, messageContext] = message.useMessage()

    const {
        data: posts,
        isLoading,
        refetch,
    } = useQuery({
        queryKey: ['cat-posts'],
        queryFn: () => getApiPosts().then((x) => x.data),
        refetchInterval: 10000,
        refetchOnMount: false,
        refetchOnWindowFocus: false,
    })

    const catUids = useMemo(() => posts?.items?.flatMap((x) => x.catUids) ?? [], [posts?.items])

    const cats = useQueries({
        queries: catUids?.map((x) => ({
            enabled: !isLoading,
            queryKey: ['cat-uid', x],
            queryFn: () => getApiCatsByUid({ path: { uid: x! } }).then((x) => x.data),
        })),
    })

    const catsLoading = useMemo(() => cats.some((x) => x.isLoading), [cats])

    const catNameMap = useMemo(
        () => cats.map((x) => x.data).reduce((a, c) => ({ ...a, [c?.uid ?? '']: c?.name }) as never, {} as { [k: string]: string }),
        [cats]
    )

    const handlePostPublish = async (values: { title: string; description: string; imageUrl: string; cats: string }) => {
        try {
            await postApiPosts({
                body: {
                    userUid: CURRENT_USER_UID,
                    title: values.title,
                    catUids: ['20000000-0000-0000-0000-000000000006'],
                    photoUrl: values.imageUrl,
                    description: values.description,
                },
                throwOnError: true,
            })

            refetch()
            form.resetFields()
            messageApi.success('Публикация создана')
        } catch {
            messageApi.error('Ошибка создания поста')
        }
    }

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
                        <Form form={form} id="create-post-form" layout="vertical" onFinish={handlePostPublish}>
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

                    <Spin spinning={isLoading || catsLoading}>
                        {posts?.items?.map((post) => (
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
                                        <Paragraph style={{ marginBottom: 12 }}>{post.description}</Paragraph>
                                        <Space wrap>
                                            {post.catUids?.map((catUid) => (
                                                <Tag key={catUid} color="magenta">
                                                    {catNameMap[catUid]}
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
