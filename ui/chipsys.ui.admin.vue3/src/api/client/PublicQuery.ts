import { AxiosResponse } from 'axios'
import {
  PageOutput,
  PublicSupplierOutput,
  PublicProductOutput,
  SearchResultOutput,
  ResultOutputPageOutputPublicSupplierOutput,
  ResultOutputPublicSupplierOutput,
  ResultOutputPageOutputPublicProductOutput,
  ResultOutputPublicProductOutput,
  ResultOutputSearchResultOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from '../admin/http-client'

export class PublicQueryApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * 查询公开的供应商列表
   */
  getPublicSuppliers = (
    data: {
      /** @format int32 */
      currentPage?: number
      /** @format int32 */
      pageSize?: number
      filter?: any
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPageOutputPublicSupplierOutput, any>({
      path: `/api/client/public-query/get-public-suppliers`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 查询公开的供应商详情
   */
  getPublicSupplier = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPublicSupplierOutput, any>({
      path: `/api/client/public-query/get-public-supplier`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 查询公开的产品列表
   */
  getPublicProducts = (
    data: {
      /** @format int32 */
      currentPage?: number
      /** @format int32 */
      pageSize?: number
      filter?: any
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPageOutputPublicProductOutput, any>({
      path: `/api/client/public-query/get-public-products`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 查询公开的产品详情
   */
  getPublicProduct = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPublicProductOutput, any>({
      path: `/api/client/public-query/get-public-product`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 查询供应商的产品列表
   */
  getSupplierProducts = (
    supplierId: number,
    data: {
      /** @format int32 */
      currentPage?: number
      /** @format int32 */
      pageSize?: number
      filter?: any
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPageOutputPublicProductOutput, any>({
      path: `/api/client/public-query/get-supplier-products/${supplierId}`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 搜索供应商和产品
   */
  search = (
    body?: {
      keyword?: string
      type?: string
    },
    params: RequestParams = {}
  ) =>
    this.request({
      path: `/api/client/public-query/search`,
      method: 'GET',
      query: { keyword: body?.keyword, type: body?.type },
      secure: true,
    })

  /**
   * 获取供应商详细信息（包含联系人）
   */
  getSupplierDetail = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request({
      path: `/api/client/public-query/get-supplier-detail`,
      method: 'GET',
      query: query,
      secure: true,
    })
}
