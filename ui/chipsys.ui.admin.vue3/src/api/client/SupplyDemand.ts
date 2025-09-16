import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  SupplyDemandGetOutput,
  SupplyDemandGetPageInput,
  SupplyDemandGetPageOutput,
  SupplyDemandAddInput,
  SupplyDemandUpdateInput,
  PageInput,
  PageOutput,
  ResultOutputSupplyDemandGetOutput,
  ResultOutputPageOutputSupplyDemandGetPageOutput,
  ResultOutputInt64,
  ResultOutputBoolean
} from './data-contracts'

/**
 * 供求信息API客户端
 */
export class SupplyDemandApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 获取供求信息详情
   * @param id 信息ID
   * @returns 信息详情
   */
  async get(id: number): Promise<ResultOutputSupplyDemandGetOutput> {
    return this.request<ResultOutputSupplyDemandGetOutput, any>({
      path: `/api/client/supply-demand/get`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取我的供求信息分页列表
   * @param input 分页查询参数
   * @returns 分页列表
   */
  async getPage(input: PageInput<SupplyDemandGetPageInput>): Promise<ResultOutputPageOutputSupplyDemandGetPageOutput> {
    return this.request<ResultOutputPageOutputSupplyDemandGetPageOutput, any>({
      path: `/api/client/supply-demand/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取公开的供求信息分页列表
   * @param input 分页查询参数
   * @returns 分页列表
   */
  async getPublicPage(input: PageInput<SupplyDemandGetPageInput>): Promise<ResultOutputPageOutputSupplyDemandGetPageOutput> {
    return this.request<ResultOutputPageOutputSupplyDemandGetPageOutput, any>({
      path: `/api/client/supply-demand/get-public-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 添加供求信息
   * @param input 信息内容
   * @returns 信息ID
   */
  async add(input: SupplyDemandAddInput): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/client/supply-demand/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新供求信息
   * @param input 信息内容
   * @returns 操作结果
   */
  async update(input: SupplyDemandUpdateInput): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supply-demand/update`,
      method: 'PUT',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除供求信息
   * @param id 信息ID
   * @returns 操作结果
   */
  async delete(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supply-demand/delete`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 批量删除供求信息
   * @param ids 信息ID数组
   * @returns 操作结果
   */
  async batchDelete(ids: number[]): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supply-demand/batch-delete`,
      method: 'POST',
      body: ids,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 发布供求信息
   * @param id 信息ID
   * @returns 操作结果
   */
  async publish(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supply-demand/publish`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 关闭供求信息
   * @param id 信息ID
   * @returns 操作结果
   */
  async close(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/supply-demand/close`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }
}
