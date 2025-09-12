import { AxiosResponse } from 'axios'
import {
  CategoryAddInput,
  CategoryUpdateInput,
  CategoryGetOutput,
  CategoryGetListOutput,
  CategoryGetListInput,
  ResultOutputCategoryGetOutput,
  ResultOutputInt64,
  ResultOutputListCategoryGetListOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class CategoryApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
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
    this.request<ResultOutputCategoryGetOutput, any>({
      path: `/api/admin/category/get`,
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
      filter?: CategoryGetListInput
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/category/get-page`,
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
    data: CategoryGetListInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListCategoryGetListOutput, any>({
      path: `/api/admin/category/get-list`,
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
    data: CategoryAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/category/add`,
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
    data: CategoryUpdateInput,
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/category/update`,
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
      path: `/api/admin/category/soft-delete`,
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
      path: `/api/admin/category/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
