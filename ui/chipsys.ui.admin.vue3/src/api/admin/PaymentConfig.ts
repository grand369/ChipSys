import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  ResultOutputListPaymentConfigEntity,
  ResultOutputPaymentConfigEntity,
  ResultOutputInt64,
  ResultOutputBoolean
} from './data-contracts'

/**
 * 支付配置API客户端
 */
export class PaymentConfigApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 获取支付配置列表
   * @returns 配置列表
   */
  async getConfigs(): Promise<ResultOutputListPaymentConfigEntity> {
    return this.request<ResultOutputListPaymentConfigEntity, any>({
      path: `/api/admin/payment-config/get-configs`,
      method: 'GET',
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取支付配置详情
   * @param id 配置ID
   * @returns 配置详情
   */
  async getConfig(id: number): Promise<ResultOutputPaymentConfigEntity> {
    return this.request<ResultOutputPaymentConfigEntity, any>({
      path: `/api/admin/payment-config/get-config`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 添加支付配置
   * @param config 配置信息
   * @returns 配置ID
   */
  async addConfig(config: any): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/admin/payment-config/add-config`,
      method: 'POST',
      body: config,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新支付配置
   * @param config 配置信息
   * @returns 操作结果
   */
  async updateConfig(config: any): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/payment-config/update-config`,
      method: 'PUT',
      body: config,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除支付配置
   * @param id 配置ID
   * @returns 操作结果
   */
  async deleteConfig(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/payment-config/delete-config`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 切换配置状态
   * @param id 配置ID
   * @param enabled 是否启用
   * @returns 操作结果
   */
  async toggleConfig(id: number, enabled: boolean): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/payment-config/toggle-config`,
      method: 'POST',
      body: { id, enabled },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 测试支付配置
   * @param id 配置ID
   * @returns 测试结果
   */
  async testConfig(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/payment-config/test-config`,
      method: 'POST',
      body: { id },
      secure: true,
      type: ContentType.Json
    })
  }
}
