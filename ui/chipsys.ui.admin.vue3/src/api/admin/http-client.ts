/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, RawAxiosRequestHeaders, ResponseType } from 'axios'
import { ElLoading, ElMessage, LoadingOptions } from 'element-plus'
import { useUserInfo } from '/@/stores/userInfo'

export type QueryParamsType = Record<string | number, any>

export interface FullRequestParams extends Omit<AxiosRequestConfig, 'data' | 'params' | 'url' | 'responseType'> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean
  /** request path */
  path: string
  /** content type of request body */
  type?: ContentType
  /** query params */
  query?: QueryParamsType
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType
  /** request body */
  body?: unknown
  /** 显示错误消息 */
  showErrorMessage?: boolean
  /** 显示成功消息 */
  showSuccessMessage?: boolean
  /** 登录访问 */
  login?: boolean
  /** 加载�?*/
  loading?: boolean
  /** 加载中选项 */
  loadingOptions?: LoadingOptions
  /** 取消重复请求 */
  cancelRepeatRequest?: boolean
  /** 返回整个响应对象 */
  returnResponse?: boolean
}

export type RequestParams = Omit<FullRequestParams, 'body' | 'method' | 'query' | 'path'>

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, 'data' | 'cancelToken'> {
  securityWorker?: (securityData: SecurityDataType | null) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void
  secure?: boolean
  format?: ResponseType
}

export enum ContentType {
  Json = 'application/json',
  FormData = 'multipart/form-data',
  UrlEncoded = 'application/x-www-form-urlencoded',
  Text = 'text/plain',
}

export interface LoadingInstance {
  target: any
  count: number
}

const pendingMap = new Map()

const loadingInstance: LoadingInstance = {
  target: null,
  count: 0,
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance
  private securityData: SecurityDataType | null = null
  private securityWorker?: ApiConfig<SecurityDataType>['securityWorker']
  private secure?: boolean
  private format?: ResponseType

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, timeout: 60000, baseURL: axiosConfig.baseURL || window.__ENV_CONFIG__.VITE_API_URL })
    this.secure = secure
    this.format = format
    this.securityWorker = securityWorker
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data
  }

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method)

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      } as RawAxiosRequestHeaders,
    }
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === 'object' && formItem !== null) {
      return JSON.stringify(formItem)
    } else {
      return `${formItem}`
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key]
      const propertyContent: any[] = property instanceof Array ? property : [property]

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem))
      }

      return formData
    }, new FormData())
  }

  /**
   * 错误处理
   * @param {*} error
   */
  protected errorHandle(error: any) {
    if (!error) {
      return
    }
    if (axios.isCancel(error)) return console.error('请求重复已被自动取消�? + error.message)
    let message = ''
    if (error.response) {
      switch (error.response.status) {
        case 302:
          message = '接口重定�?
          break
        case 400:
          message = '参数不正�?
          break
        case 401:
          message = '您还没有登录'
          break
        case 403:
          message = '您没有权限操�?
          break
        case 404:
          message = '请求地址出错�? + error.response.config.url
          break
        case 408:
          message = '请求超时'
          break
        case 409:
          message = '系统已存在相同数�?
          break
        case 429:
          message = '访问过于频繁'
          break
        case 500:
          message = '服务器内部错�?
          break
        case 501:
          message = '服务未实�?
          break
        case 502:
          message = '网关错误'
          break
        case 503:
          message = '服务不可�?
          break
        case 504:
          message = '服务暂时无法访问，请稍后再试'
          break
        case 505:
          message = 'HTTP版本不受支持'
          break
        default:
          message = '异常问题，请联系网站管理�?
          break
      }
    }
    if (error.message.includes('timeout')) message = '请求超时'
    if (error.message.includes('Network')) message = window.navigator.onLine ? '服务端异�? : '您已断网'

    if (message) {
      ElMessage.error({ message, grouping: true })
    }
  }

  /**
   * 刷新token接口
   * @param string  refreshToken
   */
  protected async refreshApi(refreshToken: string) {
    return this.request<AxiosResponse, any>({
      path: `/api/admin/auth/refresh`,
      method: 'GET',
      secure: true,
      format: 'json',
      login: false,
      query: {
        token: refreshToken,
      },
    })
  }

  /**
   * 刷新token
   * @param {*} config
   */
  protected async refreshToken(config: any) {
    const storesUseUserInfo = useUserInfo()
    const token = storesUseUserInfo.getToken()
    if (!token) {
      storesUseUserInfo.clear()
      return Promise.reject(config)
    }

    if (window.tokenRefreshing) {
      window.requests = window.requests ? window.requests : []
      return new Promise((resolve) => {
        window.requests.push(() => {
          resolve(this.instance(config))
        })
      })
    }

    window.tokenRefreshing = true

    return this.refreshApi(token)
      .then((res) => {
        if (res?.success) {
          storesUseUserInfo.setTokenInfo(res.data)
          if (window.requests?.length > 0) {
            window.requests.forEach((apiRequest) => apiRequest())
            window.requests = []
          }
          return this.instance(config)
        } else {
          storesUseUserInfo.clear()
          return Promise.reject(res)
        }
      })
      .catch((error) => {
        storesUseUserInfo.clear()
        return Promise.reject(error)
      })
      .finally(() => {
        window.tokenRefreshing = false
      })
  }

  /**
   * 储存每个请求的唯一cancel回调, 以此为标�?
   */
  protected addPending(config: AxiosRequestConfig) {
    const pendingKey = this.getPendingKey(config)
    config.cancelToken =
      config.cancelToken ||
      new axios.CancelToken((cancel) => {
        if (!pendingMap.has(pendingKey)) {
          pendingMap.set(pendingKey, cancel)
        }
      })
  }

  /**
   * 删除重复的请�?
   */
  protected removePending(config: AxiosRequestConfig) {
    const pendingKey = this.getPendingKey(config)
    if (pendingMap.has(pendingKey)) {
      const cancelToken = pendingMap.get(pendingKey)
      cancelToken(pendingKey)
      pendingMap.delete(pendingKey)
    }
  }

  /**
   * 生成每个请求的唯一key
   */
  protected getPendingKey(config: AxiosRequestConfig) {
    let { data, headers } = config
    headers = headers as RawAxiosRequestHeaders
    const { url, method, params } = config
    if (typeof data === 'string') data = JSON.parse(data)
    return [url, method, headers && headers.Authorization ? headers.Authorization : '', JSON.stringify(params), JSON.stringify(data)].join('&')
  }

  /**
   * 关闭Loading层实�?
   */
  protected closeLoading(loading: boolean = false) {
    if (loading && loadingInstance.count > 0) loadingInstance.count--
    if (loadingInstance.count === 0) {
      loadingInstance.target.close()
      loadingInstance.target = null
    }
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    showErrorMessage = true,
    showSuccessMessage = false,
    login = true,
    loading = false,
    loadingOptions = {
      background: 'rgba(0,0,0,0.5)',
    },
    cancelRepeatRequest = false,
    returnResponse = false,
    ...params
  }: FullRequestParams): Promise<T> => {
    const secureParams =
      ((typeof secure === 'boolean' ? secure : this.secure) && this.securityWorker && (await this.securityWorker(this.securityData))) || {}
    const requestParams = this.mergeRequestParams(params, secureParams)
    const responseFormat = format || this.format || undefined

    if (type === ContentType.FormData && body && body !== null && typeof body === 'object') {
      body = this.createFormData(body as Record<string, unknown>)
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== 'string') {
      body = JSON.stringify(body)
    }

    // 请求拦截
    this.instance.interceptors.request.use(
      async (config) => {
        this.removePending(config)
        cancelRepeatRequest && this.addPending(config)

        if (loading) {
          loadingInstance.count++
          if (loadingInstance.count === 1) {
            loadingInstance.target = ElLoading.service(loadingOptions)
          }
        }

        const storesUseUserInfo = useUserInfo()
        const tokenInfo = storesUseUserInfo.getTokenInfo()

        if (tokenInfo && tokenInfo.accessToken) {
          // 判断 accessToken 是否快失�?
          const now = new Date().getTime()
          const expiresAt = new Date(tokenInfo.accessTokenExpiresAt).getTime()
          const maxThreshold = tokenInfo.accessTokenLifeTime * 0.5
          // 确保阈值不超过 5 分钟且不超过 accessTokenLifeTime 的一�?
          const threshold = Math.min(5 * 60 * 1000, maxThreshold)
          if (expiresAt - now < threshold) {
            //加锁
            if (!window.tokenRefreshing) {
              window.tokenRefreshing = true
              try {
                const res = await this.refreshApi(tokenInfo.accessToken)
                if (res?.success) {
                  storesUseUserInfo.setTokenInfo(res.data)
                  //处理等待队列中的请求
                  if (window.requests?.length > 0) {
                    window.requests.forEach((apiRequest) => apiRequest())
                    window.requests = []
                  }
                } else {
                  storesUseUserInfo.clear()
                  return Promise.reject(res)
                }
              } catch (error) {
                // 清空等待队列
                window.requests = []
                return Promise.reject(error)
              } finally {
                // 解锁
                window.tokenRefreshing = false
              }
            } else {
              // 如果正在刷新，则将当前请求加入等待队�?
              if (config.url !== '/api/admin/auth/refresh') {
                window.requests = window.requests ? window.requests : []
                return new Promise((resolve) => {
                  window.requests.push(() => {
                    resolve(this.instance(config))
                  })
                })
              }
            }
          }
        }

        const accessToken = storesUseUserInfo.getToken()
        config.headers!['Authorization'] = `Bearer ${accessToken}`
        return config
      },
      (error) => {
        return Promise.reject(error)
      }
    )
    // 响应拦截
    this.instance.interceptors.response.use(
      (res) => {
        this.removePending(res.config)
        loading && this.closeLoading(loading)

        if (res.config?.responseType == 'blob') {
          return res
        }

        const data = res.data
        if (data.success) {
          if (showSuccessMessage) {
            ElMessage.success({ message: data.msg ? data.msg : '操作成功', grouping: true })
          }
        } else {
          if (showErrorMessage) {
            ElMessage.error({ message: data.msg ? data.msg : '操作失败', grouping: true })
          }
          // return Promise.reject(res)
        }

        return res
      },
      async (error) => {
        error.config && this.removePending(error.config)
        loading && this.closeLoading(loading)

        //刷新token
        if (login && error?.response?.status === 401) {
          return this.refreshToken(error.config)
        }

        //错误处理
        if (showErrorMessage) {
          this.errorHandle(error)
        }

        return Promise.reject(error)
      }
    )

    return this.instance
      .request({
        ...requestParams,
        headers: {
          ...(requestParams.headers || {}),
          ...(type && type !== ContentType.FormData ? { 'Content-Type': type } : {}),
        } as RawAxiosRequestHeaders,
        params: query,
        responseType: responseFormat,
        data: body,
        url: path,
      })
      .then((response) => (returnResponse ? response : response.data))
  }
}
