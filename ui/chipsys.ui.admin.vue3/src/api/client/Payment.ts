import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  PaymentCreateInput,
  PaymentStatusOutput,
  ResultOutputPaymentCreateOutput,
  ResultOutputPaymentStatusOutput
} from './data-contracts'

/**
 * 支付API客户端
 */
export class PaymentApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 创建支付订单
   * @param input 支付信息
   * @returns 支付订单信息
   */
  async createPayment(input: PaymentCreateInput): Promise<ResultOutputPaymentCreateOutput> {
    return this.request<ResultOutputPaymentCreateOutput, any>({
      path: `/api/client/payment/create`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 检查支付状态
   * @param orderId 订单ID
   * @returns 支付状态
   */
  async checkPaymentStatus(orderId: string): Promise<ResultOutputPaymentStatusOutput> {
    return this.request<ResultOutputPaymentStatusOutput, any>({
      path: `/api/client/payment/status`,
      method: 'GET',
      query: { orderId },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 支付回调（内部使用）
   * @param input 回调数据
   * @returns 处理结果
   */
  async paymentCallback(input: any): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/payment/callback`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }
}
