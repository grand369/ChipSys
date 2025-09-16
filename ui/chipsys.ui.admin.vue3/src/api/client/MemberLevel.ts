import { AxiosResponse } from 'axios'
import { HttpClient, ContentType } from '../admin/http-client'
import {
  MemberLevelGetOutput,
  MemberLevelGetPageInput,
  MemberLevelGetPageOutput,
  MemberLevelAddInput,
  MemberLevelUpdateInput,
  PageInput,
  PageOutput,
  ResultOutputMemberLevelGetOutput,
  ResultOutputPageOutputMemberLevelGetPageOutput,
  ResultOutputInt64,
  ResultOutputBoolean
} from './data-contracts'

/**
 * 会员等级API客户端
 */
export class MemberLevelApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 获取会员等级详情
   * @param id 等级ID
   * @returns 等级详情
   */
  async get(id: number): Promise<ResultOutputMemberLevelGetOutput> {
    return this.request<ResultOutputMemberLevelGetOutput, any>({
      path: `/api/client/member-level/get`,
      method: 'GET',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取会员等级分页列表
   * @param input 分页查询参数
   * @returns 分页列表
   */
  async getPage(input: PageInput<MemberLevelGetPageInput>): Promise<ResultOutputPageOutputMemberLevelGetPageOutput> {
    return this.request<ResultOutputPageOutputMemberLevelGetPageOutput, any>({
      path: `/api/client/member-level/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取当前会员等级信息
   * @param memberId 会员ID
   * @returns 当前等级信息
   */
  async getCurrentLevel(memberId: number): Promise<ResultOutputMemberLevelGetOutput> {
    return this.request<ResultOutputMemberLevelGetOutput, any>({
      path: `/api/client/member-level/get-current-level`,
      method: 'GET',
      query: { memberId },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 添加会员等级
   * @param input 等级信息
   * @returns 等级ID
   */
  async add(input: MemberLevelAddInput): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/client/member-level/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新会员等级
   * @param input 等级信息
   * @returns 操作结果
   */
  async update(input: MemberLevelUpdateInput): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-level/update`,
      method: 'PUT',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除会员等级
   * @param id 等级ID
   * @returns 操作结果
   */
  async delete(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-level/delete`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 升级会员等级
   * @param input 升级信息
   * @returns 等级ID
   */
  async upgrade(input: { level: string }): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/client/member-level/upgrade`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 检查会员权限
   * @param memberId 会员ID
   * @param permissionType 权限类型
   * @returns 是否有权限
   */
  async checkPermission(memberId: number, permissionType: string): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-level/check-permission`,
      method: 'GET',
      query: { memberId, permissionType },
      secure: true,
      type: ContentType.Json
    })
  }
}
