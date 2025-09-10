import axios, { AxiosInstance, AxiosRequestConfig } from 'axios'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Session, Local } from '/@/utils/storage'
import qs from 'qs'
import { adminTokenKey } from '/@/stores/userInfo'

// 配置新建一�?axios 实例
const service: AxiosInstance = axios.create({
  baseURL: window.__ENV_CONFIG__.VITE_API_URL,
  timeout: 50000,
  headers: { 'Content-Type': 'application/json' },
  paramsSerializer: {
    serialize(params) {
      return qs.stringify(params, { allowDots: true })
    },
  },
})

// 添加请求拦截�?
service.interceptors.request.use(
  (config: AxiosRequestConfig) => {
    // 在发送请求之前做些什�?token
    if (Local.get(adminTokenKey)) {
      config.headers!['Authorization'] = `${Local.get(adminTokenKey)}`
    }
    return config
  },
  (error) => {
    // 对请求错误做些什�?
    return Promise.reject(error)
  }
)

// 添加响应拦截�?
service.interceptors.response.use(
  (response) => {
    // 对响应数据做点什�?
    const res = response.data
    if (res.code && res.code !== 0) {
      // `token` 过期或者账号已在别处登�?
      if (res.code === 401 || res.code === 4001) {
        Local.remove(adminTokenKey)
        Session.clear() // 清除浏览器全部临时缓�?
        window.location.href = '/' // 去登录页
        ElMessageBox.alert('你已被登出，请重新登�?, '提示', {})
          .then(() => {})
          .catch(() => {})
      }
      return Promise.reject(service.interceptors.response)
    } else {
      return res
    }
  },
  (error) => {
    // 对响应错误做点什�?
    if (error.message.indexOf('timeout') != -1) {
      ElMessage.error('网络超时')
    } else if (error.message == 'Network Error') {
      ElMessage.error('网络连接错误')
    } else {
      if (error.response.data) ElMessage.error(error.response.statusText)
      else ElMessage.error('接口路径找不�?)
    }
    return Promise.reject(error)
  }
)

// 导出 axios 实例
export default service
