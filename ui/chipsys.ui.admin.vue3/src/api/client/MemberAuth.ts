import { HttpClient, ContentType } from '../admin/http-client'
import type {
  MemberRegByMobileInput,
  MemberRegByEmailInput,
  MemberRegByWechatInput,
  MemberLoginByMobileInput,
  MemberLoginByWechatInput,
  MemberLoginOutput,
  MemberInfoOutput,
  MemberProfileUpdateInput,
  ResultOutput
} from './data-contracts'

/**
 * 会员认证API
 */
export class MemberAuthApi extends HttpClient {
  constructor() {
    super({
      baseURL: window.__ENV_CONFIG__.VITE_API_URL
    })
  }

  /**
   * 发送手机验证码
   */
  async sendMobileCode(mobile: string): Promise<string> {
    const res = await this.request<ResultOutput<string>, any>({
      path: `/api/client/member-auth/send-mobile-code`,
      method: 'POST',
      query: { mobile },
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as string)
  }

  /**
   * 发送邮箱验证码
   */
  async sendEmailCode(email: string): Promise<string> {
    const res = await this.request<ResultOutput<string>, any>({
      path: `/api/client/member-auth/send-email-code`,
      method: 'POST',
      query: { email },
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as string)
  }

  /**
   * 检查手机号是否已注册
   */
  async checkMobileExists(mobile: string): Promise<boolean> {
    const res = await this.request<ResultOutput<boolean>, any>({
      path: `/api/client/member-auth/check-mobile-exists`,
      method: 'POST',
      query: { mobile },
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as boolean)
  }

  /**
   * 检查邮箱是否已注册
   */
  async checkEmailExists(email: string): Promise<boolean> {
    const res = await this.request<ResultOutput<boolean>, any>({
      path: `/api/client/member-auth/check-email-exists`,
      method: 'POST',
      query: { email },
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as boolean)
  }

  /**
   * 手机号注册
   */
  async registerByMobile(input: MemberRegByMobileInput): Promise<MemberLoginOutput> {
    const res = await this.request<ResultOutput<MemberLoginOutput>, any>({
      path: `/api/client/member-auth/register-by-mobile`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as MemberLoginOutput)
  }

  /**
   * 邮箱注册
   */
  async registerByEmail(input: MemberRegByEmailInput): Promise<MemberLoginOutput> {
    const res = await this.request<ResultOutput<MemberLoginOutput>, any>({
      path: `/api/client/member-auth/register-by-email`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as MemberLoginOutput)
  }

  /**
   * 微信注册
   */
  async registerByWechat(input: MemberRegByWechatInput): Promise<MemberLoginOutput> {
    const res = await this.request<ResultOutput<MemberLoginOutput>, any>({
      path: `/api/client/member-auth/register-by-wechat`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as MemberLoginOutput)
  }

  /**
   * 手机号登录
   */
  async loginByMobile(input: MemberLoginByMobileInput): Promise<MemberLoginOutput> {
    const res = await this.request<ResultOutput<MemberLoginOutput>, any>({
      path: `/api/client/member-auth/login-by-mobile`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as MemberLoginOutput)
  }

  /**
   * 微信登录
   */
  async loginByWechat(input: MemberLoginByWechatInput): Promise<MemberLoginOutput> {
    const res = await this.request<ResultOutput<MemberLoginOutput>, any>({
      path: `/api/client/member-auth/login-by-wechat`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
    return (res as any)?.data ?? (res as unknown as MemberLoginOutput)
  }

  /**
   * 获取会员信息
   */
  async getMemberInfo(): Promise<ResultOutput<MemberInfoOutput>> {
    return this.request<ResultOutput<MemberInfoOutput>, any>({
      path: `/api/client/member-auth/get-member-info`,
      method: 'GET',
      secure: true,
      type: ContentType.Json
    })
  }

  /**
   * 更新会员信息
   */
  async updateProfile(input: MemberProfileUpdateInput): Promise<void> {
    await this.request<ResultOutput<void>, any>({
      path: `/api/client/member-auth/update-member-profile`,
      method: 'POST',
      body: input,
      secure: true,
      type: ContentType.Json,
      showErrorMessage: false // 禁用自动错误消息，让组件自己处理
    })
  }

  /**
   * 刷新令牌
   */
  async refreshToken(refreshToken: string): Promise<ResultOutput<MemberLoginOutput>> {
    return this.request<ResultOutput<MemberLoginOutput>, any>({
      path: `/api/client/member-auth/refresh-token`,
      method: 'POST',
      body: refreshToken,
      secure: true,
      type: ContentType.Json
    })
  }
}

export const memberAuthApi = new MemberAuthApi()