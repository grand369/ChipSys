import { AxiosResponse } from 'axios'
import {
  UserListAddInput,
  UserListUpdateInput,
  UserListGetOutput,
  UserListGetListOutput,
  UserListGetListInput,
  ResultOutputUserListGetOutput,
  ResultOutputInt64,
  ResultOutputListUserListGetListOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class UserListApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
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
    this.request<ResultOutputUserListGetOutput, any>({
      path: `/api/admin/user-list/get`,
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
      filter?: UserListGetListInput
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/user-list/get-page`,
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
    data: UserListGetListInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListUserListGetListOutput, any>({
      path: `/api/admin/user-list/get-list`,
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
    data: UserListAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/user-list/add`,
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
    data: UserListUpdateInput,
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/user-list/update`,
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
      path: `/api/admin/user-list/soft-delete`,
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
      path: `/api/admin/user-list/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
