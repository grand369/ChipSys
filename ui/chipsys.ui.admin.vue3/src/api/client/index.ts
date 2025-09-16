// Client模块API统一导出
// 使用方式与admin模块保持一致

import { MemberFavoriteApi } from './MemberFavorite'
import { MemberDashboardApi } from './MemberDashboard'
import { PublicQueryApi } from './PublicQuery'
import { SupplierApplicationApi } from './SupplierApplication'
import { SupplyDemandApi } from './SupplyDemand'
import { MemberLevelApi } from './MemberLevel'
import { InquiryApi } from './Inquiry'
import { QuoteApi } from './Quote'
import { MemberAuthApi } from './MemberAuth'

// 创建API实例
export const memberFavoriteApi = new MemberFavoriteApi()
export const memberDashboardApi = new MemberDashboardApi()
export const publicQueryApi = new PublicQueryApi()
export const supplierApplicationApi = new SupplierApplicationApi()
export const supplyDemandApi = new SupplyDemandApi()
export const memberLevelApi = new MemberLevelApi()
export const inquiryApi = new InquiryApi()
export const quoteApi = new QuoteApi()
export const memberAuthApi = new MemberAuthApi()

// 使用示例：
// import { 
//   memberFavoriteApi, 
//   supplierApplicationApi, 
//   supplyDemandApi,
//   memberLevelApi,
//   inquiryApi,
//   quoteApi 
// } from '/@/api/client'
// 
// // 查询会员收藏分页
// const favoriteResult = await memberFavoriteApi.getPage({
//   currentPage: 1,
//   pageSize: 10,
//   filter: { favoriteType: 'supplier' }
// })
// 
// // 查询供应商申请
// const applicationResult = await supplierApplicationApi.getPage({
//   currentPage: 1,
//   pageSize: 10,
//   filter: { status: 'Pending' }
// })
// 
// // 查询供求信息
// const supplyResult = await supplyDemandApi.getPage({
//   currentPage: 1,
//   pageSize: 10,
//   filter: { infoType: 'Supply' }
// })
// 
// // 查询会员等级
// const levelResult = await memberLevelApi.getCurrentLevel(userId)
// 
// // 发布询价
// const inquiryResult = await inquiryApi.add({
//   title: '采购芯片',
//   productName: 'CPU芯片',
//   quantity: 1000,
//   contactInfo: '联系人信息'
// })
// 
// // 提交报价
// const quoteResult = await quoteApi.add({
//   inquiryId: 123,
//   supplierId: 456,
//   supplierName: '供应商名称',
//   quotePrice: 1000,
//   currency: 'CNY',
//   contactName: '联系人'
// })
