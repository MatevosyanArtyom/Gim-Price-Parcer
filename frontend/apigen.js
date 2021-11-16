const fs = require('fs')
const generateApi = require('swagger-typescript-api').generateApi

generateApi({
    name: 'api.ts',
    url: 'http://localhost:5000/swagger/v1/swagger.json',
    templates: 'templates',
    generateUnionEnums: true
})
    .then(sourceFile => {
        fs.writeFileSync('src/api.ts', sourceFile)
    })
    .catch(e => console.error(e))