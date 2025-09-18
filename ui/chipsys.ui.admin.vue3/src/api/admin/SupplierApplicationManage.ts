import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  SupplierApplicationGetOutput,
  SupplierApplicationGetPageInput,
  SupplierApplicationReviewInput,
  PageInput,
  PageOutput,
  ResultOutputSupplierApplicationGetOutput,
  ResultOutputPageOutputSupplierApplicationGetOutput,
  ResultOutputBoolean
} from '../client/data-contracts'

/**
 * 管理员端供应商申请管理API客户端
 */
export class SupplierApplicationManageApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 获取申请详情
   * @param id 申请ID
   * @returns 申请详情
   */
  async get(id: number): Promise<ResultOutputSupplierApplicationGetOutput> {
    return this.request<ResultOutputSupplierApplicationGetOutput, any>({
      path: `/api/admin/supplier-application-manage/get`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取申请分页列表
   * @param input 分页查询参数
   * @returns 分页列表
   */
  async getPage(input: PageInput<SupplierApplicationGetPageInput>): Promise<ResultOutputPageOutputSupplierApplicationGetOutput> {
    return this.request<ResultOutputPageOutputSupplierApplicationGetOutput, any>({
      path: `/api/admin/supplier-application-manage/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 审核申请
   * @param input 审核信息
   * @returns 操作结果
   */
  async review(input: SupplierApplicationReviewInput): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/supplier-application-manage/review`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }
}
