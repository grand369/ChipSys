<template>
  <div class="my-flex-column w100 h100">
    <!-- 统计卡片 -->
    <el-row :gutter="20" class="mb20">
      <el-col :xs="24" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-content">
            <div class="stat-icon favorite-supplier">
              <el-icon><User /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-number">{{ dashboardStats.favoriteSupplierCount || 0 }}</div>
              <div class="stat-label">收藏供应商</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-content">
            <div class="stat-icon favorite-product">
              <el-icon><Box /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-number">{{ dashboardStats.favoriteProductCount || 0 }}</div>
              <div class="stat-label">收藏产品</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-content">
            <div class="stat-icon favorite-contact">
              <el-icon><Phone /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-number">{{ dashboardStats.favoriteContactCount || 0 }}</div>
              <div class="stat-label">收藏联系人</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card class="stat-card" shadow="hover">
          <div class="stat-content">
            <div class="stat-icon supply-demand">
              <el-icon><Document /></el-icon>
            </div>
            <div class="stat-info">
              <div class="stat-number">{{ (dashboardStats.supplyInfoCount || 0) + (dashboardStats.demandInfoCount || 0) }}</div>
              <div class="stat-label">供求信息</div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 内容区域 -->
    <el-row :gutter="20">
      <!-- 最近收藏 -->
      <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
        <el-card class="mb20" shadow="never">
          <template #header>
            <div class="card-header">
              <span>最近收藏</span>
              <el-button type="primary" text @click="goToFavorites">查看全部</el-button>
            </div>
          </template>
          <div class="recent-list">
            <div v-if="recentFavorites.length === 0" class="empty-state">
              <el-empty description="暂无收藏记录" />
            </div>
            <div v-else>
              <div v-for="item in recentFavorites" :key="item.id" class="recent-item">
                <div class="item-icon">
                  <el-icon v-if="item.type === 'Supplier'"><User /></el-icon>
                  <el-icon v-else-if="item.type === 'Product'"><Box /></el-icon>
                  <el-icon v-else><Phone /></el-icon>
                </div>
                <div class="item-content">
                  <div class="item-name">{{ item.name }}</div>
                  <div class="item-time">{{ formatDate(item.createdTime) }}</div>
                </div>
                <div class="item-type">
                  <el-tag :type="getFavoriteTypeTagType(item.type)" size="small">
                    {{ getFavoriteTypeText(item.type) }}
                  </el-tag>
                </div>
              </div>
            </div>
          </div>
        </el-card>
      </el-col>

      <!-- 最近供求信息 -->
      <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
        <el-card class="mb20" shadow="never">
          <template #header>
            <div class="card-header">
              <span>最近供求信息</span>
              <el-button type="primary" text @click="goToSupplyDemand">查看全部</el-button>
            </div>
          </template>
          <div class="recent-list">
            <div v-if="recentSupplyDemands.length === 0" class="empty-state">
              <el-empty description="暂无供求信息" />
            </div>
            <div v-else>
              <div v-for="item in recentSupplyDemands" :key="item.id" class="recent-item">
                <div class="item-icon">
                  <el-icon><Document /></el-icon>
                </div>
                <div class="item-content">
                  <div class="item-name">{{ item.title || item.productName }}</div>
                  <div class="item-time">{{ formatDate(item.createdTime) }}</div>
                </div>
                <div class="item-type">
                  <el-tag :type="item.infoType === 'Supply' ? 'success' : 'warning'" size="small">
                    {{ item.infoType === 'Supply' ? '供应' : '需求' }}
                  </el-tag>
                </div>
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 供应商申请状态 -->
    <el-card v-if="dashboardStats.supplierApplicationStatus" shadow="never">
      <template #header>
        <div class="card-header">
          <span>供应商申请状态</span>
        </div>
      </template>
      <div class="application-status">
        <el-alert
          :title="getApplicationStatusText(dashboardStats.supplierApplicationStatus)"
          :type="getApplicationStatusType(dashboardStats.supplierApplicationStatus)"
          :closable="false"
          show-icon
        />
      </div>
    </el-card>
  </div>
</template>

<script lang="ts" setup name="client/member-dashboard">
import { MemberDashboardStatsOutput } from '/@/api/client/data-contracts'
import { MemberDashboardApi } from '/@/api/client/MemberDashboard'
import { useRouter } from 'vue-router'
import { Session } from '/@/utils/storage'

const router = useRouter()

const state = reactive({
  loading: false,
  dashboardStats: {} as MemberDashboardStatsOutput,
})

const { dashboardStats } = toRefs(state)

// 计算属性
const recentFavorites = computed(() => {
  const favorites = [
    ...(dashboardStats.value.recentFavoriteSuppliers || []),
    ...(dashboardStats.value.recentFavoriteProducts || []),
  ]
  return favorites.slice(0, 5) // 只显示最近5条
})

const recentSupplyDemands = computed(() => {
  return (dashboardStats.value.recentSupplyDemands || []).slice(0, 5) // 只显示最近5条
})

// 获取收藏类型标签类型
const getFavoriteTypeTagType = (type: string) => {
  switch (type) {
    case 'Supplier':
      return 'success'
    case 'Product':
      return 'primary'
    case 'Contact':
      return 'warning'
    default:
      return 'info'
  }
}

// 获取收藏类型文本
const getFavoriteTypeText = (type: string) => {
  switch (type) {
    case 'Supplier':
      return '供应商'
    case 'Product':
      return '产品'
    case 'Contact':
      return '联系人'
    default:
      return type
  }
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return ''
  return new Date(date).toLocaleString('zh-CN')
}

// 获取申请状态文本
const getApplicationStatusText = (status: number) => {
  switch (status) {
    case 0:
      return '草稿状态'
    case 1:
      return '已提交，等待审核'
    case 2:
      return '审核通过'
    case 3:
      return '审核拒绝'
    case 4:
      return '已撤销'
    default:
      return '未知状态'
  }
}

// 获取申请状态类型
const getApplicationStatusType = (status: number) => {
  switch (status) {
    case 0:
      return 'info'
    case 1:
      return 'warning'
    case 2:
      return 'success'
    case 3:
      return 'error'
    case 4:
      return 'info'
    default:
      return 'info'
  }
}

// 跳转到收藏页面
const goToFavorites = () => {
  router.push('/client/member-favorite')
}

// 跳转到供求信息页面
const goToSupplyDemand = () => {
  router.push('/client/supply-demand')
}

// 获取仪表板统计数据
const getDashboardStats = async () => {
  state.loading = true
  try {
    const res = await new MemberDashboardApi().getDashboardStats()
    if (res?.success) {
      state.dashboardStats = res.data
    }
  } catch (error) {
    console.error('获取仪表板统计数据失败:', error)
  } finally {
    state.loading = false
  }
}

// 页面加载时获取数据
onMounted(() => {
  getDashboardStats()
})
</script>

<style scoped>
.stat-card {
  margin-bottom: 20px;
}

.stat-content {
  display: flex;
  align-items: center;
}

.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 16px;
  font-size: 24px;
  color: white;
}

.stat-icon.favorite-supplier {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.stat-icon.favorite-product {
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
}

.stat-icon.favorite-contact {
  background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
}

.stat-icon.supply-demand {
  background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
}

.stat-info {
  flex: 1;
}

.stat-number {
  font-size: 28px;
  font-weight: bold;
  color: #303133;
  line-height: 1;
  margin-bottom: 4px;
}

.stat-label {
  font-size: 14px;
  color: #909399;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.recent-list {
  max-height: 400px;
  overflow-y: auto;
}

.recent-item {
  display: flex;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
}

.recent-item:last-child {
  border-bottom: none;
}

.item-icon {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #f5f7fa;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 12px;
  color: #606266;
}

.item-content {
  flex: 1;
}

.item-name {
  font-size: 14px;
  color: #303133;
  margin-bottom: 4px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.item-time {
  font-size: 12px;
  color: #909399;
}

.item-type {
  margin-left: 12px;
}

.empty-state {
  text-align: center;
  padding: 40px 0;
}

.application-status {
  padding: 20px 0;
}
</style>
