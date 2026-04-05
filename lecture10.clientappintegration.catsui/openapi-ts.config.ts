export default {
    input: 'http://localhost:5185/swagger/v1/swagger.json',
    output: 'src/shared/api/generated',
    plugins: ['@hey-api/client-axios'],
}
