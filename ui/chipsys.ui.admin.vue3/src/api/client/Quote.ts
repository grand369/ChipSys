import { HttpClient, ContentType } from '../admin/http-client'
import type { 
  QuoteGetOutput,
  QuoteGetPageOutput,
  QuoteAddInput,
  QuoteUpdateInput,
  QuoteGetPageInput,
  InquiryGetOutput,
  ResultOutputQuoteGetOutput,
  ResultOutputPageOutputQuoteGetPageOutput,
  ResultOutputInquiryGetOutput
} from './data-contracts'

export class QuoteApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 获取报价详情
   */
  async get(id: number): Promise<ResultOutputQuoteGetOutput> {
    return this.request<ResultOutputQuoteGetOutput, any>({
      path: `/api/client/quote/get`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取报价分页列表
   */
  async getPage(input: PageInput<QuoteGetPageInput>): Promise<ResultOutputPageOutputQuoteGetPageOutput> {
    return this.request<ResultOutputPageOutputQuoteGetPageOutput, any>({
      path: `/api/client/quote/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 新增报价
   */
  async add(input: QuoteAddInput): Promise<ResultOutputQuoteGetOutput> {
    return this.request<ResultOutputQuoteGetOutput, any>({
      path: `/api/client/quote/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新报价
   */
  async update(input: QuoteUpdateInput): Promise<ResultOutputQuoteGetOutput> {
    return this.request<ResultOutputQuoteGetOutput, any>({
      path: `/api/client/quote/update`,
      method: 'PUT',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除报价
   */
  async delete(id: number): Promise<ResultOutputQuoteGetOutput> {
    return this.request<ResultOutputQuoteGetOutput, any>({
      path: `/api/client/quote/delete`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 接受报价
   */
  async accept(id: number): Promise<ResultOutputQuoteGetOutput> {
    return this.request<ResultOutputQuoteGetOutput, any>({
      path: `/api/client/quote/accept`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 拒绝报价
   */
  async reject(id: number): Promise<ResultOutputQuoteGetOutput> {
    return this.request<ResultOutputQuoteGetOutput, any>({
      path: `/api/client/quote/reject`,
      method: 'POST',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取询价的报价列表
   */
  async getInquiryQuotes(inquiryId: number, input: QuoteGetPageInput): Promise<ResultOutputPageOutputQuoteGetPageOutput> {
    return this.request<ResultOutputPageOutputQuoteGetPageOutput, any>({
      path: `/api/client/quote/get-inquiry-quotes`,
      method: 'POST',
      query: { inquiryId },
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取报价统计
   */
  async getStats(): Promise<ResultOutput<any>> {
    return this.request<ResultOutput<any>, any>({
      path: `/api/client/quote/get-stats`,
      method: 'GET',
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取询价信息（用于报价时显示询价详情）
   */
  async getInquiryInfo(inquiryId: number): Promise<ResultOutputInquiryGetOutput> {
    return this.request<ResultOutputInquiryGetOutput, any>({
      path: `/api/client/inquiry/get`,
      method: 'GET',
      query: { id: inquiryId },
      secure: true,
      type: ContentType.Json
    })
  }
}

export const quoteApi = new QuoteApi()
