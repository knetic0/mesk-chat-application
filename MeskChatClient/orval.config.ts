// orval.config.ts
export default {
  api: {
    input: {
      target: 'http://localhost:5254/openapi/v1.json',
    },
    output: {
      mode: 'single',
      target: './src/api/service.ts',
      schemas: './src/types',
      client: 'axios',
      override: {
        mutator: {
          path: './src/axios.ts',
          name: 'axiosInstance',
        },
      },
      prettier: '.prettierrc',
    },
  },
}