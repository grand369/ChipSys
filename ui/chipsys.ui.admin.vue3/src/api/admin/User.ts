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
  PageInput,
  PageInputUserGetPageInput,
  ResultOutputInt64,
  ResultOutputPageOutputUserGetDeletedUserPageOutput,
  ResultOutputPageOutputUserGetPageOutput,
  ResultOutputString,
  ResultOutputTokenInfo,
  ResultOutputUserGetBasicOutput,
  ResultOutputUserGetOutput,
  ResultOutputUserGetPermissionOutput,
  UserAddInput,
  UserAddMemberInput,
  UserBatchSetOrgInput,
  UserChangePasswordInput,
  UserResetPasswordInput,
  UserRestoreInput,
  UserSetEnableInput,
  UserSetManagerInput,
  UserUpdateBasicInput,
  UserUpdateInput,
  UserUpdateMemberInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class UserApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags user
   * @name Get
   * @summary 查询用户
   * @request GET:/api/admin/user/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputUserGetOutput, any>({
      path: `/api/admin/user/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/user/get-page
   * @secure
   */
  getPage = (data: PageInputUserGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputUserGetPageOutput, any>({
      path: `/api/admin/user/get-page`,
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
   * @tags user
   * @name GetDeletedPage
   * @summary 查询已删除分页列�?
   * @request POST:/api/admin/user/get-deleted-page
   * @secure
   */
  getDeletedPage = (data: PageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputUserGetDeletedUserPageOutput, any>({
      path: `/api/admin/user/get-deleted-page`,
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
   * @tags user
   * @name GetBasic
   * @summary 查询用户基本信息
   * @request GET:/api/admin/user/get-basic
   * @secure
   */
  getBasic = (params: RequestParams = {}) =>
    this.request<ResultOutputUserGetBasicOutput, any>({
      path: `/api/admin/user/get-basic`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name GetPermission
   * @summary 查询用户权限信息
   * @request GET:/api/admin/user/get-permission
   * @secure
   */
  getPermission = (params: RequestParams = {}) =>
    this.request<ResultOutputUserGetPermissionOutput, any>({
      path: `/api/admin/user/get-permission`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name Add
   * @summary 新增用户
   * @request POST:/api/admin/user/add
   * @secure
   */
  add = (data: UserAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/user/add`,
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
   * @tags user
   * @name Update
   * @summary 修改用户
   * @request PUT:/api/admin/user/update
   * @secure
   */
  update = (data: UserUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name AddMember
   * @summary 新增会员
   * @request POST:/api/admin/user/add-member
   * @secure
   */
  addMember = (data: UserAddMemberInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/user/add-member`,
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
   * @tags user
   * @name UpdateMember
   * @summary 修改会员
   * @request PUT:/api/admin/user/update-member
   * @secure
   */
  updateMember = (data: UserUpdateMemberInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/update-member`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name UpdateBasic
   * @summary 更新用户基本信息
   * @request PUT:/api/admin/user/update-basic
   * @secure
   */
  updateBasic = (data: UserUpdateBasicInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/update-basic`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name ChangePassword
   * @summary 修改用户密码
   * @request PUT:/api/admin/user/change-password
   * @secure
   */
  changePassword = (data: UserChangePasswordInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/change-password`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name ResetPassword
   * @summary 重置密码
   * @request POST:/api/admin/user/reset-password
   * @secure
   */
  resetPassword = (data: UserResetPasswordInput, params: RequestParams = {}) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/user/reset-password`,
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
   * @tags user
   * @name SetManager
   * @summary 设置主管
   * @request POST:/api/admin/user/set-manager
   * @secure
   */
  setManager = (data: UserSetManagerInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/set-manager`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name SetEnable
   * @summary 设置启用
   * @request POST:/api/admin/user/set-enable
   * @secure
   */
  setEnable = (data: UserSetEnableInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/set-enable`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name Restore
   * @summary 恢复
   * @request POST:/api/admin/user/restore
   * @secure
   */
  restore = (data: UserRestoreInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/restore`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name BatchSetOrg
   * @summary 批量设置部门
   * @request PUT:/api/admin/user/batch-set-org
   * @secure
   */
  batchSetOrg = (data: UserBatchSetOrgInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/batch-set-org`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name Delete
   * @summary 彻底删除用户
   * @request DELETE:/api/admin/user/delete
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
      path: `/api/admin/user/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name BatchDelete
   * @summary 批量彻底删除用户
   * @request PUT:/api/admin/user/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name SoftDelete
   * @summary 删除用户
   * @request DELETE:/api/admin/user/soft-delete
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
      path: `/api/admin/user/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name BatchSoftDelete
   * @summary 批量删除用户
   * @request PUT:/api/admin/user/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name AvatarUpload
   * @summary 上传头像
   * @request POST:/api/admin/user/avatar-upload
   * @secure
   */
  avatarUpload = (
    data: {
      /** @format binary */
      file?: File
    },
    query?: {
      /** @default false */
      autoUpdate?: boolean
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/user/avatar-upload`,
      method: 'POST',
      query: query,
      body: data,
      secure: true,
      type: ContentType.FormData,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name OneClickLogin
   * @summary 一键登录用�?
   * @request GET:/api/admin/user/one-click-login
   * @secure
   */
  oneClickLogin = (
    query: {
      userName: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputTokenInfo, any>({
      path: `/api/admin/user/one-click-login`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags user
   * @name ForceOffline
   * @summary 强制用户下线
   * @request POST:/api/admin/user/force-offline
   * @secure
   */
  forceOffline = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/user/force-offline`,
      method: 'POST',
      query: query,
      secure: true,
      ...params,
    })
}
