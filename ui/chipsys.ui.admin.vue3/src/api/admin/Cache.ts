/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { AxiosResponse } from 'axios'
import { ResultOutputListObject } from './data-contracts'
import { HttpClient, RequestParams } from './http-client'

export class CacheApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags cache
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/cache/get-list
   * @secure
   */
  getList = (params: RequestParams = {}) =>
    this.request<ResultOutputListObject, any>({
      path: `/api/admin/cache/get-list`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags cache
   * @name Clear
   * @summary 清除缓存
   * @request DELETE:/api/admin/cache/clear
   * @secure
   */
  clear = (
    query?: {
      /** 缓存�?*/
      cacheKey?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/cache/clear`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
}
