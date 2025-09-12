import { AxiosResponse } from 'axios'
import {
  ProductAddInput,
  ProductUpdateInput,
  ProductGetOutput,
  ProductGetPageOutput,
  ProductGetPageInput,
  ProductGetListOutput,
  ProductGetListInput,
  ResultOutputProductGetOutput,
  ResultOutputInt64,
  ResultOutputListProductGetListOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ProductApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * 查询
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputProductGetOutput, any>({
      path: `/api/admin/product/get`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 查询分页
   */
  getPage = (
    data: {
      /** @format int32 */
      currentPage?: number
      /** @format int32 */
      pageSize?: number
      filter?: ProductGetListInput
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/product/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 查询列表
   */
  getList = (
    data: ProductGetListInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListProductGetListOutput, any>({
      path: `/api/admin/product/get-list`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 新增
   */
  add = (
    data: ProductAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/product/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 修改
   */
  update = (
    data: ProductUpdateInput,
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/product/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 删除
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/product/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 批量删除
   */
  batchDelete = (
    data: {
      /** @format int64 */
      ids?: number[]
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/product/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
