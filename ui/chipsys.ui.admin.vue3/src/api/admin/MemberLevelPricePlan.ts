import { HttpClient, ContentType } from './http-client'
import { 
  PageInput, 
  PageOutput, 
  ResultOutputInt64,
  ResultOutputBoolean
} from './data-contracts'

// 价格方案类型枚举
export enum PricePlanType {
  Monthly = 'Monthly',
  Quarterly = 'Quarterly', 
  Yearly = 'Yearly'
}

// 价格方案类型选项
export const PricePlanTypeOptions = [
  { label: '包月', value: PricePlanType.Monthly },
  { label: '包季', value: PricePlanType.Quarterly },
  { label: '包年', value: PricePlanType.Yearly }
]

// 价格方案输出
export interface MemberLevelPricePlanGetOutput {
  id: number
  memberLevelId: number
  planType: PricePlanType
  planName: string
  originalPrice: number
  discountedPrice: number
  discountPercent: number
  description?: string
  isRecommended: boolean
  enabled: boolean
  sort: number
  createdTime: string
}

// 价格方案分页输出
export interface MemberLevelPricePlanGetPageOutput extends MemberLevelPricePlanGetOutput {
  // 可以添加更多用于列表展示的字段
}

// 价格方案添加输入
export interface MemberLevelPricePlanAddInput {
  memberLevelId: number
  planType: PricePlanType
  planName: string
  originalPrice: number
  discountedPrice: number
  discountPercent: number
  description?: string
  isRecommended: boolean
  enabled: boolean
  sort: number
}

// 价格方案更新输入
export interface MemberLevelPricePlanUpdateInput extends MemberLevelPricePlanAddInput {
  id: number
}

// 价格方案分页输入
export interface MemberLevelPricePlanGetPageInput {
  memberLevelId?: number
  planType?: string
  enabled?: boolean
}

// 批量设置价格方案输入
export interface BatchSetPricePlansInput {
  memberLevelId: number
  pricePlans: MemberLevelPricePlanAddInput[]
}

// API接口
export class MemberLevelPricePlanApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * 获取价格方案信息
   */
  get(id: number): Promise<any> {
    return this.request({
      path: `/api/client/member-level-price-plan/get`,
      method: 'GET',
      query: { id },
      format: 'json',
    })
  }

  /**
   * 获取价格方案分页列表
   */
  getPage(input: PageInput<MemberLevelPricePlanGetPageInput>): Promise<any> {
    return this.request({
      path: `/api/client/member-level-price-plan/get-page`,
      method: 'POST',
      body: input,
      type: ContentType.Json,
      format: 'json',
    })
  }

  /**
   * 添加价格方案
   */
  add(input: MemberLevelPricePlanAddInput): Promise<ResultOutputInt64> {
    return this.request({
      path: `/api/client/member-level-price-plan/add`,
      method: 'POST',
      body: input,
      type: ContentType.Json,
      format: 'json',
    })
  }

  /**
   * 更新价格方案
   */
  update(input: MemberLevelPricePlanUpdateInput): Promise<ResultOutputBoolean> {
    return this.request({
      path: `/api/client/member-level-price-plan/update`,
      method: 'PUT',
      body: input,
      type: ContentType.Json,
      format: 'json',
    })
  }

  /**
   * 删除价格方案
   */
  delete(id: number): Promise<ResultOutputBoolean> {
    return this.request({
      path: `/api/client/member-level-price-plan/delete`,
      method: 'DELETE',
      query: { id },
      format: 'json',
    })
  }

  /**
   * 获取指定等级的价格方案列表
   */
  getByMemberLevelId(memberLevelId: number): Promise<any> {
    return this.request({
      path: `/api/client/member-level-price-plan/get-by-member-level-id`,
      method: 'GET',
      query: { memberLevelId },
      format: 'json',
    })
  }

  /**
   * 批量设置价格方案
   */
  batchSetPricePlans(input: BatchSetPricePlansInput): Promise<ResultOutputBoolean> {
    return this.request({
      path: `/api/client/member-level-price-plan/batch-set-price-plans`,
      method: 'PUT',
      body: input,
      type: ContentType.Json,
      format: 'json',
    })
  }

  /**
   * 切换启用状态（如 swagger 未提供，可保留为管理员端专用）
   */
  toggleEnabled(id: number, enabled: boolean): Promise<ResultOutputBoolean> {
    return this.request({
      path: `/api/admin/member-level-price-plan-manage/toggle-enabled`,
      method: 'POST',
      query: { id, enabled },
      format: 'json',
    })
  }
}
