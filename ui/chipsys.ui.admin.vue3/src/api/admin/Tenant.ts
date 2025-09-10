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
import {
  PageInputTenantGetPageInput,
  ResultOutputInt64,
  ResultOutputPageOutputTenantGetPageOutput,
  ResultOutputTenantGetOutput,
  ResultOutputTokenInfo,
  TenantAddInput,
  TenantSetEnableInput,
  TenantUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class TenantApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags tenant
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/tenant/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputTenantGetOutput, any>({
      path: `/api/admin/tenant/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/tenant/get-page
   * @secure
   */
  getPage = (data: PageInputTenantGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputTenantGetPageOutput, any>({
      path: `/api/admin/tenant/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/tenant/add
   * @secure
   */
  add = (data: TenantAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/tenant/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/tenant/update
   * @secure
   */
  update = (data: TenantUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/tenant/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/tenant/delete
   * @secure
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/tenant/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/tenant/soft-delete
   * @secure
   */
  softDelete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/tenant/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/tenant/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/tenant/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name SetEnable
   * @summary 设置启用
   * @request POST:/api/admin/tenant/set-enable
   * @secure
   */
  setEnable = (data: TenantSetEnableInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/tenant/set-enable`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags tenant
   * @name OneClickLogin
   * @summary 一键登�?
   * @request POST:/api/admin/tenant/one-click-login
   * @secure
   */
  oneClickLogin = (
    query?: {
      /** @format int64 */
      tenantId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputTokenInfo, any>({
      path: `/api/admin/tenant/one-click-login`,
      method: 'POST',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
}
