import { AxiosResponse } from 'axios'
import {
  ContactAddInput,
  ContactUpdateInput,
  ContactGetOutput,
  ContactGetListOutput,
  ContactGetListInput,
  ResultOutputContactGetOutput,
  ResultOutputInt64,
  ResultOutputListContactGetListOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ContactApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
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
    this.request<ResultOutputContactGetOutput, any>({
      path: `/api/admin/contact/get`,
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
      filter?: ContactGetListInput
    },
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/contact/get-page`,
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
    data: ContactGetListInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListContactGetListOutput, any>({
      path: `/api/admin/contact/get-list`,
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
    data: ContactAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/contact/add`,
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
    data: ContactUpdateInput,
    params: RequestParams = {}
  ) =>
    this.request<any, any>({
      path: `/api/admin/contact/update`,
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
      path: `/api/admin/contact/soft-delete`,
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
      path: `/api/admin/contact/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
