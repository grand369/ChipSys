import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  SupplierApplicationAddInput,
  SupplierApplicationGetOutput,
  SupplierApplicationGetPageInput,
  PageInput,
  PageOutput,
  ResultOutputSupplierApplicationGetOutput,
  ResultOutputPageOutputSupplierApplicationGetOutput,
  ResultOutputInt64
} from './data-contracts'

/**
 * 供应商申请API客户端
 */
export class SupplierApplicationApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 提交供应商申请
   * @param input 申请信息
   * @returns 申请ID
   */
  async add(input: SupplierApplicationAddInput): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/client/supplier-application/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取申请详情
   * @param id 申请ID
   * @returns 申请详情
   */
  async get(id: number): Promise<ResultOutputSupplierApplicationGetOutput> {
    return this.request<ResultOutputSupplierApplicationGetOutput, any>({
      path: `/api/client/supplier-application/get`,
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
      path: `/api/client/supplier-application/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取我的申请
   * @returns 我的申请信息
   */
  async getMyApplication(): Promise<ResultOutputSupplierApplicationGetOutput> {
    return this.request<ResultOutputSupplierApplicationGetOutput, any>({
      path: `/api/client/supplier-application/my-application`,
      method: 'GET',
      secure: true,
      type: ContentType.Json
    })
  }
}