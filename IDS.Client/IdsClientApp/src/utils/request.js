import axios from 'axios'
// import store from '@/store'
// import storage from 'store'
import notification from 'ant-design-vue/es/notification'
import { VueAxios } from './axios'
// import { ACCESS_TOKEN } from '@/store/mutation-types'

// 创建 axios 实例
const request = axios.create({
  // API 请求的默认前缀
  baseURL: '',
  timeout: 6000 // 请求超时时间
})

// 异常拦截处理器
const errorHandler = (error) => {
  if (error.response) {
    const data = error.response.data
    // 从 localstorage 获取 token
    // const token = storage.get(ACCESS_TOKEN)
    if (error.response.status === 403) {
      notification.error({
        message: 'Forbidden',
        description: data.message
      })
    }
  }
  return Promise.reject(error)
}

// request interceptor
request.interceptors.request.use(config => {
  var global = JSON.parse(localStorage.getItem('GLOBAL'))
  config.baseURL = config.baseURL || global.BASEURL
  var token = localStorage.getItem('Access-Token')
  if (token) {
    // config.headers['Authorization'] = token.slice(1, token.length - 1)
  }
  return config
}, errorHandler)

// response interceptor
request.interceptors.response.use((response) => {
  return response.data
}, errorHandler)

const installer = {
  vm: {},
  install (Vue) {
    Vue.use(VueAxios, request)
  }
}

export default request

export {
  installer as VueAxios,
  request as axios
}
