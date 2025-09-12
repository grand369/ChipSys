import { AxiosResponse } from 'axios'
import {
  HitListAddInput,
  HitListUpdateInput,
  HitListGetOutput,
  HitListGetListOutput,
  HitListGetListInput,
  ResultOutputHitListGetOutput,
  ResultOutputInt64,
  ResultOutputListHitListGetListOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class HitListApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
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
    this.request<ResultOutputHitListGetOutput, any>({
      path: `/api/admin/hit-list/get`,
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
      filter?: HitListGetListInput
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/hit-list/get-page`,
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
    data: HitListGetListInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListHitListGetListOutput, any>({
      path: `/api/admin/hit-list/get-list`,
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
    data: HitListAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/hit-list/add`,
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
    data: HitListUpdateInput,
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/hit-list/update`,
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
      path: `/api/admin/hit-list/soft-delete`,
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
      path: `/api/admin/hit-list/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
