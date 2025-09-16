import { AxiosResponse } from 'axios'
import {
  MemberDashboardStatsOutput,
  ResultOutputMemberDashboardStatsOutput,
  ResultOutputBoolean,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from '../admin/http-client'

export class MemberDashboardApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * 获取会员仪表板统计数据
   */
  getDashboardStats = (params: RequestParams = {}) =>
    this.request<ResultOutputMemberDashboardStatsOutput, any>({
      path: `/api/client/member-dashboard/get-dashboard-stats`,
      method: 'GET',
      secure: true,
      ...params,
    })

  /**
   * 获取会员收藏统计
   */
  getFavoriteStats = (params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-dashboard/get-favorite-stats`,
      method: 'GET',
      secure: true,
      ...params,
    })

  /**
   * 获取会员供求信息统计
   */
  getSupplyDemandStats = (params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/client/member-dashboard/get-supply-demand-stats`,
      method: 'GET',
      secure: true,
      ...params,
    })
}
