// Client模块数据契约定义
// 这个文件定义了Client模块的所有数据传输对象(DTO)

// 基础分页输入
export interface PageInput<T = any> {
  currentPage: number
  pageSize: number
  filter?: T
  dynamicFilter?: any
}

// 基础分页输出
export interface PageOutput<T = any> {
  list: T[]
  total: number
}

// 基础响应类型
export interface ResultOutput<T = any> {
  success: boolean
  code: number
  msg: string
  data: T
}

// 具体响应类型 - 基础类型
export type ResultOutputInt64 = ResultOutput<number>
export type ResultOutputBoolean = ResultOutput<boolean>

// 会员收藏相关DTO
export interface MemberFavoriteGetPageInput {
  favoriteType?: string
  favoriteName?: string
}

export interface MemberFavoriteGetPageOutput {
  id: number
  memberId: number
  favoriteType: string
  favoriteId: number
  favoriteName: string
  remark?: string
  createdTime: string
}

export interface MemberFavoriteGetOutput {
  id: number
  memberId: number
  favoriteType: string
  favoriteId: number
  favoriteName: string
  remark?: string
  createdTime: string
}

export interface MemberFavoriteAddInput {
  favoriteType: string
  favoriteId: number
  remark?: string
}

export interface MemberFavoriteUpdateInput {
  id: number
  favoriteType: string
  favoriteId: number
  remark?: string
}

// 供求信息相关DTO
export interface SupplyDemandGetPageFilter {
  infoType?: string
  productName?: string
  status?: string
}

export interface SupplyDemandGetPageInput extends PageInput<SupplyDemandGetPageFilter> {
}

export interface SupplyDemandGetPageOutput {
  id: number
  infoType: string
  productName: string
  productModel?: string
  productBrand?: string
  quantity?: number
  priceRange?: string
  deliveryDays?: number
  status: string
  createdTime: string
}

export interface SupplyDemandGetOutput {
  id: number
  memberId: number
  title?: string
  infoType: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  productDescription?: string
  productSpecification?: string
  technicalRequirements?: string
  description?: string
  specification?: string
  unit?: string
  quantity?: number
  expectedPrice?: number
  currency?: string
  priceRange?: string
  deliveryDays?: number
  contactInfo?: string
  status: number
  createdTime: string
}

export interface SupplyDemandAddInput {
  title?: string
  infoType: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  productDescription?: string
  productSpecification?: string
  technicalRequirements?: string
  description?: string
  specification?: string
  unit?: string
  quantity?: number
  expectedPrice?: number
  currency?: string
  priceRange?: string
  deliveryDays?: number
  contactInfo?: string
}

export interface SupplyDemandUpdateInput {
  id: number
  title?: string
  infoType: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  productDescription?: string
  productSpecification?: string
  technicalRequirements?: string
  description?: string
  specification?: string
  unit?: string
  quantity?: number
  expectedPrice?: number
  currency?: string
  priceRange?: string
  deliveryDays?: number
  contactInfo?: string
}

// 供应商申请相关DTO
export interface SupplierApplicationGetPageFilter {
  companyName?: string
  contactName?: string
  status?: string
}

export interface SupplierApplicationGetPageInput extends PageInput<SupplierApplicationGetPageFilter> {
}

export interface SupplierApplicationGetPageOutput {
  id: number
  companyName: string
  companyAddress?: string
  contactName: string
  contactPhone: string
  contactEmail?: string
  mainProductCategories?: string
  status: string
  reviewComment?: string
  reviewTime?: string
  createdTime: string
}

export interface SupplierApplicationGetOutput {
  id: number
  memberId: number
  companyName: string
  contactName: string
  contactPhone: string
  contactEmail: string
  businessScope?: string
  address?: string
  status: number
  submittedTime?: string
  cancelledTime?: string
  createdTime: string
}

export interface SupplierApplicationAddInput {
  companyName: string
  contactName: string
  contactPhone: string
  contactEmail: string
  businessScope?: string
  address?: string
}

export interface SupplierApplicationUpdateInput {
  id: number
  companyName: string
  contactName: string
  contactPhone: string
  contactEmail: string
  businessScope?: string
  address?: string
}

// 会员仪表板相关DTO
export interface MemberDashboardStatsOutput {
  favoriteSupplierCount: number
  favoriteProductCount: number
  favoriteContactCount: number
  supplyInfoCount: number
  demandInfoCount: number
  supplierApplicationStatus?: number
  recentFavoriteSuppliers: RecentFavoriteOutput[]
  recentFavoriteProducts: RecentFavoriteOutput[]
  recentSupplyDemands: RecentSupplyDemandOutput[]
}

export interface RecentFavoriteOutput {
  id: number
  favoriteId: number
  favoriteName: string
  name: string
  type: string
  createdTime: string
}

export interface RecentSupplyDemandOutput {
  id: number
  title?: string
  infoType: string
  productName: string
  status: number
  createdTime: string
}

// 会员等级相关DTO
export interface MemberLevelGetPageInput extends PageInput {
  memberId?: number
  level?: string
  enabled?: boolean
}

export interface MemberLevelGetPageOutput {
  id: number
  memberId: number
  level: string
  categoryLimit: number
  productDataLimit: number
  supplierDataLimit: number
  showFullContactInfo: boolean
  enabled: boolean
  effectiveTime?: string
  expireTime?: string
  createdTime: string
}

export interface MemberLevelGetOutput {
  id: number
  memberId: number
  level: number
  categoryLimit: number
  productDataLimit: number
  supplierDataLimit: number
  showFullContactInfo: boolean
  enabled: boolean
  createdTime: string
}

export interface MemberLevelAddInput {
  memberId?: number
  level: string
  categoryLimit: number
  productDataLimit: number
  supplierDataLimit: number
  showFullContactInfo: boolean
  enabled: boolean
  effectiveTime?: string
  expireTime?: string
}

export interface MemberLevelUpdateInput {
  id: number
  memberId?: number
  level: string
  categoryLimit: number
  productDataLimit: number
  supplierDataLimit: number
  showFullContactInfo: boolean
  enabled: boolean
  effectiveTime?: string
  expireTime?: string
}

// 询价相关DTO
export interface InquiryGetPageInput extends PageInput {
  title?: string
  productName?: string
  status?: string
}

export interface InquiryGetPageOutput {
  id: number
  title: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  quantity: number
  budgetRange?: string
  expectedDeliveryDays: number
  validDays: number
  technicalRequirements?: string
  qualityRequirements?: string
  otherRequirements?: string
  contactInfo: string
  quoteCount: number
  status: string
  memberId: number
  createdTime: string
  modifiedTime?: string
}

export interface InquiryGetOutput {
  id: number
  title: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  quantity: number
  budgetRange?: string
  expectedDeliveryDays: number
  validDays: number
  technicalRequirements?: string
  qualityRequirements?: string
  otherRequirements?: string
  contactInfo: string
  quoteCount: number
  status: string
  memberId: number
  createdTime: string
  modifiedTime?: string
}

export interface InquiryAddInput {
  title: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  quantity: number
  budgetRange?: string
  expectedDeliveryDays: number
  validDays: number
  technicalRequirements?: string
  qualityRequirements?: string
  otherRequirements?: string
  contactInfo: string
}

export interface InquiryUpdateInput {
  id: number
  title: string
  productName: string
  productModel?: string
  productBrand?: string
  productManufacturer?: string
  quantity: number
  budgetRange?: string
  expectedDeliveryDays: number
  validDays: number
  technicalRequirements?: string
  qualityRequirements?: string
  otherRequirements?: string
  contactInfo: string
}

// 报价相关DTO
export interface QuoteGetPageInput extends PageInput {
  inquiryId?: number
  supplierName?: string
  status?: string
}

export interface QuoteGetPageOutput {
  id: number
  inquiryId: number
  supplierId: number
  supplierName: string
  quotePrice: number
  currency: string
  deliveryDays: number
  minOrderQuantity: number
  validDays: number
  validUntil?: string
  productSpecification?: string
  qualityAssurance?: string
  quoteRemark?: string
  contactName: string
  contactPhone: string
  contactEmail?: string
  status: string
  acceptedTime?: string
  rejectedTime?: string
  createdTime: string
  modifiedTime?: string
}

export interface QuoteGetOutput {
  id: number
  inquiryId: number
  supplierId: number
  supplierName: string
  quotePrice: number
  currency: string
  deliveryDays: number
  minOrderQuantity: number
  validDays: number
  validUntil?: string
  productSpecification?: string
  qualityAssurance?: string
  quoteRemark?: string
  contactName: string
  contactPhone: string
  contactEmail?: string
  status: string
  acceptedTime?: string
  rejectedTime?: string
  createdTime: string
  modifiedTime?: string
}

export interface QuoteAddInput {
  inquiryId: number
  supplierId: number
  supplierName: string
  quotePrice: number
  currency: string
  deliveryDays: number
  minOrderQuantity: number
  validDays: number
  productSpecification?: string
  qualityAssurance?: string
  quoteRemark?: string
  contactName: string
  contactPhone: string
  contactEmail?: string
}

export interface QuoteUpdateInput {
  id: number
  inquiryId: number
  supplierId: number
  supplierName: string
  quotePrice: number
  currency: string
  deliveryDays: number
  minOrderQuantity: number
  validDays: number
  productSpecification?: string
  qualityAssurance?: string
  quoteRemark?: string
  contactName: string
  contactPhone: string
  contactEmail?: string
  status: string
}

// 公开查询相关DTO
export interface PublicSupplierOutput {
  id: number
  companyName: string
  businessScope: string
  address: string
  contactName: string
  contactPhone: string
  contactEmail: string
  createdTime: string
}

export interface PublicProductOutput {
  id: number
  name: string
  category: {
    id: number
    name: string
  }
  description: string
  specification: string
  unit: string
  price?: number
  supplierId: number
  createdTime: string
}

export interface SearchResultOutput {
  suppliers: PublicSupplierOutput[]
  products: PublicProductOutput[]
}

// 具体响应类型 - 复合类型
export type ResultOutputMemberFavoriteGetOutput = ResultOutput<MemberFavoriteGetOutput>
export type ResultOutputPageOutputMemberFavoriteGetPageOutput = ResultOutput<PageOutput<MemberFavoriteGetPageOutput>>

// 供求信息相关响应类型
export type ResultOutputSupplyDemandGetOutput = ResultOutput<SupplyDemandGetOutput>
export type ResultOutputPageOutputSupplyDemandGetPageOutput = ResultOutput<PageOutput<SupplyDemandGetPageOutput>>

// 供应商申请相关响应类型
export type ResultOutputSupplierApplicationGetOutput = ResultOutput<SupplierApplicationGetOutput>
export type ResultOutputPageOutputSupplierApplicationGetPageOutput = ResultOutput<PageOutput<SupplierApplicationGetPageOutput>>

// 会员等级相关响应类型
export type ResultOutputMemberLevelGetOutput = ResultOutput<MemberLevelGetOutput>
export type ResultOutputPageOutputMemberLevelGetPageOutput = ResultOutput<PageOutput<MemberLevelGetPageOutput>>

// 询价报价相关响应类型
export type ResultOutputInquiryGetOutput = ResultOutput<InquiryGetOutput>
export type ResultOutputPageOutputInquiryGetPageOutput = ResultOutput<PageOutput<InquiryGetPageOutput>>
export type ResultOutputQuoteGetOutput = ResultOutput<QuoteGetOutput>
export type ResultOutputPageOutputQuoteGetPageOutput = ResultOutput<PageOutput<QuoteGetPageOutput>>

// 会员仪表板相关响应类型
export type ResultOutputMemberDashboardStatsOutput = ResultOutput<MemberDashboardStatsOutput>

// 公开查询相关响应类型
export type ResultOutputPageOutputPublicSupplierOutput = ResultOutput<PageOutput<PublicSupplierOutput>>
export type ResultOutputPublicSupplierOutput = ResultOutput<PublicSupplierOutput>
export type ResultOutputPageOutputPublicProductOutput = ResultOutput<PageOutput<PublicProductOutput>>
export type ResultOutputPublicProductOutput = ResultOutput<PublicProductOutput>
export type ResultOutputSearchResultOutput = ResultOutput<SearchResultOutput>

// 会员认证相关接口
export interface MemberRegByMobileInput {
  mobile: string
  code: string
  codeId: string
}

export interface MemberRegByEmailInput {
  email: string
  code: string
  codeId: string
}

export interface MemberRegByWechatInput {
  code: string
  openId?: string
  unionId?: string
  nickName?: string
  avatar?: string
}

export interface MemberLoginByMobileInput {
  mobile: string
  code: string
  codeId: string
}

export interface MemberLoginByWechatInput {
  code: string
}

export interface MemberLoginOutput {
  accessToken: string
  refreshToken: string
  expires: string
  memberInfo: MemberInfoOutput
}

export interface MemberInfoOutput {
  id: number
  userId: number
  nickName?: string
  realName?: string
  mobile?: string
  email?: string
  gender?: string
  region?: string
  isProfileComplete: boolean
  wechatInfo?: WechatInfoOutput
}

export interface WechatInfoOutput {
  nickName?: string
  avatar?: string
}

export interface MemberProfileUpdateInput {
  nickName: string
  password?: string
  realName?: string
  idCard?: string
  gender?: string
  province?: string
  city?: string
  district?: string
  address?: string
}

export type ResultOutputMemberLoginOutput = ResultOutput<MemberLoginOutput>
export type ResultOutputMemberInfoOutput = ResultOutput<MemberInfoOutput>
