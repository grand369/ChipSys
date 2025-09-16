import { HttpClient, ContentType } from '../admin/http-client'
import type { 
  InquiryGetOutput,
  InquiryGetPageOutput,
  InquiryAddInput,
  InquiryUpdateInput,
  InquiryGetPageInput,
  ResultOutputInquiryGetOutput,
  ResultOutputPageOutputInquiryGetPageOutput
} from './data-contracts'

export class InquiryApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 获取询价详情
   */
  async get(id: number): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/get`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取询价分页列表
   */
  async getPage(input: PageInput<InquiryGetPageInput>): Promise<ResultOutputPageOutputInquiryGetPageOutput> {
    return this.request<ResultOutputPageOutputInquiryGetPageOutput, any>({
      path: `/api/client/inquiry/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 新增询价
   */
  async add(input: InquiryAddInput): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新询价
   */
  async update(input: InquiryUpdateInput): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/update`,
      method: 'PUT',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除询价
   */
  async delete(id: number): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/delete`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 发布询价
   */
  async publish(id: number): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/publish`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 关闭询价
   */
  async close(id: number): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/close`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取询价统计
   */
  async getStats(): Promise<ResultOutput<any>> {
    return this.request<ResultOutput<any>, any>({
      path: `/api/client/inquiry/get-stats`,
      method: 'GET',
      secure: true,
      type: ContentType.Json
    })
  }
}

export const inquiryApi = new InquiryApi()
