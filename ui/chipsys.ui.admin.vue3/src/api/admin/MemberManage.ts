import { HttpClient, ContentType } from './http-client'

export interface MemberGetPageInput {
  currentPage?: number
  pageSize?: number
  keyword?: string
  level?: string
  enabled?: boolean
}

export interface MemberSimpleOutput {
  id: number
  userId: number
  nickName?: string
  realName?: string
  mobile?: string
  email?: string
  enabled: boolean
  level?: string
  createdTime: string
}

export interface PageOutput<T> {
  list: T[]
  total: number
}

export interface ResultOutput<T> {
  success: boolean
  msg?: string
  data: T
}

export class MemberManageApi extends HttpClient {
  constructor() {
    super({ baseURL: window.__ENV_CONFIG__.VITE_API_URL })
  }

  getLevels() {
    return this.request<ResultOutput<{ level: string; count: number }[]>, any>({
      path: `/api/admin/member-manage/get-levels`,
      method: 'GET',
      secure: true,
    })
  }

  getMembers(input: MemberGetPageInput) {
    return this.request<ResultOutput<PageOutput<MemberSimpleOutput>>, any>({
      path: `/api/admin/member-manage/get-members`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
    })
  }

  add(input: { nickName?: string; realName?: string; mobile?: string; email?: string }) {
    return this.request<ResultOutput<number>, any>({
      path: `/api/admin/member-manage/add`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
    })
  }

  update(input: { id: number; nickName?: string; realName?: string; mobile?: string; email?: string; enabled?: boolean }) {
    return this.request<ResultOutput<void>, any>({
      path: `/api/admin/member-manage/update`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
    })
  }

  toggleEnabled(input: { id: number; enabled: boolean }) {
    return this.request<ResultOutput<void>, any>({
      path: `/api/admin/member-manage/toggle-enabled`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
    })
  }

  adjustLevel(input: { memberId: number; level: string }) {
    return this.request<ResultOutput<void>, any>({
      path: `/api/admin/member-manage/adjust-level`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
    })
  }
}

export const memberManageApi = new MemberManageApi()



