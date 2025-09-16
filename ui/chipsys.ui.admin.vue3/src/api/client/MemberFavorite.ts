import { AxiosResponse } from 'axios'
import {
  MemberFavoriteGetOutput,
  MemberFavoriteGetPageInput,
  PageOutput,
  MemberFavoriteGetPageOutput,
  MemberFavoriteAddInput,
  MemberFavoriteUpdateInput,
  ResultOutputMemberFavoriteGetOutput,
  ResultOutputPageOutputMemberFavoriteGetPageOutput,
  ResultOutputInt64,
  ResultOutputBoolean,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from '../admin/http-client'

export class MemberFavoriteApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * 查询会员收藏
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputMemberFavoriteGetOutput, any>({
      path: `/api/client/member-favorite/get`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 查询会员收藏分页
   */
  getPage = (
    data: MemberFavoriteGetPageInput & {
      /** @format int32 */
      currentPage?: number
      /** @format int32 */
      pageSize?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPageOutputMemberFavoriteGetPageOutput, any>({
      path: `/api/client/member-favorite/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 添加会员收藏
   */
  add = (
    data: MemberFavoriteAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/client/member-favorite/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 更新会员收藏
   */
  update = (
    data: MemberFavoriteUpdateInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-favorite/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 删除会员收藏
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-favorite/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 批量删除会员收藏
   */
  batchDelete = (
    data: {
      ids: number[]
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-favorite/batch-delete`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })

  /**
   * 检查是否已收藏
   */
  isFavorited = (
    query?: {
      favoriteType?: string
      /** @format int64 */
      favoriteId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-favorite/is-favorited`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })

  /**
   * 一键收藏/取消收藏
   */
  toggleFavorite = (
    data: MemberFavoriteAddInput,
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-favorite/toggle-favorite`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
