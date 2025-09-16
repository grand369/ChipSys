import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  SupplierApplicationGetOutput,
  SupplierApplicationGetPageInput,
  SupplierApplicationGetPageOutput,
  SupplierApplicationAddInput,
  SupplierApplicationUpdateInput,
  PageInput,
  PageOutput,
  ResultOutputSupplierApplicationGetOutput,
  ResultOutputPageOutputSupplierApplicationGetPageOutput,
  ResultOutputInt64,
  ResultOutputBoolean
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
   * 获取供应商申请详情
   * @param id 申请ID
   * @returns 申请详情
   */
  async get(id: number): Promise<ResultOutputSupplierApplicationGetOutput> {
    return this.request<ResultOutputSupplierApplicationGetOutput, any>({
      path: `/api/client/supplier/get`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取供应商申请分页列表
   * @param input 分页查询参数
   * @returns 分页列表
   */
  async getPage(input: PageInput<SupplierApplicationGetPageInput>): Promise<ResultOutputPageOutputSupplierApplicationGetPageOutput> {
    return this.request<ResultOutputPageOutputSupplierApplicationGetPageOutput, any>({
      path: `/api/client/supplier/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 添加供应商申请
   * @param input 申请信息
   * @returns 申请ID
   */
  async add(input: SupplierApplicationAddInput): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/client/supplier/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新供应商申请
   * @param input 申请信息
   * @returns 操作结果
   */
  async update(input: SupplierApplicationUpdateInput): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supplier/update`,
      method: 'PUT',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除供应商申请
   * @param id 申请ID
   * @returns 操作结果
   */
  async delete(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supplier/delete`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 提交供应商申请
   * @param id 申请ID
   * @returns 操作结果
   */
  async submit(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supplier/submit`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 撤销供应商申请
   * @param id 申请ID
   * @returns 操作结果
   */
  async cancel(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supplier/cancel`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }
}
