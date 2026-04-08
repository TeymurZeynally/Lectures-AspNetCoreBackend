import { LockOutlined, MailOutlined } from '@ant-design/icons'
import { Button, Card, Form, Input, Space, Typography } from 'antd'

const { Title } = Typography

export default function LoginPage() {
    return (
        <div
            style={{
                display: 'flex',
                justifyContent: 'center',
                paddingTop: 48,
            }}
        >
            <Card style={{ width: 440, borderRadius: 24 }}>
                <Space orientation="vertical" size={20} style={{ width: '100%' }}>
                    <div>
                        <Title level={2}>Вход</Title>
                    </div>

                    <Form layout="vertical" onFinish={(x) => console.log(x)}>
                        <Form.Item label="Email" name="email">
                            <Input prefix={<MailOutlined />} placeholder="demo@catspace.dev" />
                        </Form.Item>

                        <Form.Item label="Пароль" name="password">
                            <Input.Password prefix={<LockOutlined />} placeholder="Введите пароль" />
                        </Form.Item>

                        <Button type="primary" block size="large" htmlType="submit">
                            Войти
                        </Button>
                    </Form>
                </Space>
            </Card>
        </div>
    )
}
