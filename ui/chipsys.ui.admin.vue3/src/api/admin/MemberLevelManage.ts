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
  ResultOutputBoolean,
  ResultOutputListMemberLevelGetOutput
} from '../client/data-contracts'

/**
 * 管理员端会员等级管理API客户端
 */
export class MemberLevelManageApi extends HttpClient {
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
      path: `/api/admin/member-level-manage/get`,
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
      path: `/api/admin/member-level-manage/get-page`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 添加会员等级模板
   * @param input 等级信息
   * @returns 等级ID
   */
  async add(input: MemberLevelAddInput): Promise<ResultOutputInt64> {
    return this.request<ResultOutputInt64, any>({
      path: `/api/admin/member-level-manage/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新会员等级模板
   * @param input 等级信息
   * @returns 操作结果
   */
  async update(input: MemberLevelUpdateInput): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/member-level-manage/update`,
      method: 'PUT',
      body: input,
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 删除会员等级模板
   * @param id 等级ID
   * @returns 操作结果
   */
  async delete(id: number): Promise<ResultOutputBoolean> {
    return this.request<ResultOutputBoolean, any>({
      path: `/api/admin/member-level-manage/delete`,
      method: 'DELETE',
      query: { id },
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 获取所有等级模板
   * @returns 等级模板列表
   */
  async getLevelTemplates(): Promise<ResultOutputListMemberLevelGetOutput> {
    return this.request<ResultOutputListMemberLevelGetOutput, any>({
      path: `/api/admin/member-level-manage/get-level-templates`,
      method: 'GET',
      secure: true,
      type: ContentType.Json
    })
  }
}
