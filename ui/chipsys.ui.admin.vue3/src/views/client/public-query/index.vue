<template>
  <div class="my-flex-column w100 h100">
    <!-- 搜索区域 -->
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.searchModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="keyword">
          <el-input 
            v-model="state.searchModel.keyword" 
            placeholder="搜索供应商、产品名称" 
            @keyup.enter="onSearch"
            class="w300"
          >
            <template #append>
              <el-button icon="ele-Search" @click="onSearch" />
            </template>
          </el-input>
        </el-form-item>
        <el-form-item prop="type">
          <el-select v-model="state.searchModel.type" placeholder="搜索类型" class="w120">
            <el-option label="全部" value="all" />
            <el-option label="供应商" value="supplier" />
            <el-option label="产品" value="product" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="onSearch">搜索</el-button>
          <el-button @click="onReset">重置</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <!-- 搜索结果 -->
    <el-card v-if="state.searchResults" class="my-fill mt8" shadow="never">
      <template #header>
        <div class="search-header">
          <span>搜索结果</span>
          <el-tag type="info">共找到 {{ getTotalCount() }} 条结果</el-tag>
        </div>
      </template>

      <!-- 供应商结果 -->
      <div v-if="state.searchResults.suppliers && state.searchResults.suppliers.length > 0" class="search-section">
        <h3 class="section-title">
          <el-icon><User /></el-icon>
          供应商 ({{ state.searchResults.suppliers.length }})
        </h3>
        <el-row :gutter="20">
          <el-col v-for="supplier in state.searchResults.suppliers" :key="supplier.id" :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
            <el-card class="supplier-card" shadow="hover" @click="viewSupplier(supplier)">
              <div class="supplier-info">
                <div class="supplier-name">{{ supplier.companyName }}</div>
                <div class="supplier-scope">{{ supplier.businessScope }}</div>
                <div class="supplier-address">{{ supplier.address }}</div>
                <div class="supplier-contact">
                  <div class="contact-item">
                    <el-icon><User /></el-icon>
                    {{ supplier.contactName }}
                  </div>
                  <div class="contact-item">
                    <el-icon><Phone /></el-icon>
                    {{ supplier.contactPhone }}
                  </div>
                  <div class="contact-item">
                    <el-icon><Message /></el-icon>
                    {{ supplier.contactEmail }}
                  </div>
                </div>
              </div>
            </el-card>
          </el-col>
        </el-row>
      </div>

      <!-- 产品结果 -->
      <div v-if="state.searchResults.products && state.searchResults.products.length > 0" class="search-section">
        <h3 class="section-title">
          <el-icon><Box /></el-icon>
          产品 ({{ state.searchResults.products.length }})
        </h3>
        <el-table :data="state.searchResults.products" border>
          <el-table-column prop="name" label="产品名称" min-width="200" show-overflow-tooltip />
          <el-table-column prop="category" label="分类" width="120">
            <template #default="{ row }">
              {{ row.category?.name || '-' }}
            </template>
          </el-table-column>
          <el-table-column prop="description" label="描述" min-width="200" show-overflow-tooltip />
          <el-table-column prop="specification" label="规格" width="120" show-overflow-tooltip />
          <el-table-column prop="unit" label="单位" width="80" />
          <el-table-column prop="price" label="价格" width="100">
            <template #default="{ row }">
              <span v-if="row.price">¥{{ row.price }}</span>
              <span v-else>-</span>
            </template>
          </el-table-column>
          <el-table-column label="操作" width="100" fixed="right">
            <template #default="{ row }">
              <el-button type="primary" text @click="viewProduct(row)">查看</el-button>
            </template>
          </el-table-column>
        </el-table>
      </div>

      <!-- 无结果 -->
      <div v-if="!hasResults()" class="empty-state">
        <el-empty description="暂无搜索结果" />
      </div>
    </el-card>

    <!-- 默认展示区域 -->
    <div v-else class="my-fill mt8">
      <el-row :gutter="20">
        <!-- 供应商列表 -->
        <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
          <el-card shadow="never">
            <template #header>
              <div class="card-header">
                <span>热门供应商</span>
                <el-button type="primary" text @click="viewAllSuppliers">查看全部</el-button>
              </div>
            </template>
            <div class="supplier-list">
              <div v-if="state.suppliers.length === 0" class="empty-state">
                <el-empty description="暂无供应商数据" />
              </div>
              <div v-else>
                <div v-for="supplier in state.suppliers" :key="supplier.id" class="supplier-item" @click="viewSupplier(supplier)">
                  <div class="supplier-name">{{ supplier.companyName }}</div>
                  <div class="supplier-scope">{{ supplier.businessScope }}</div>
                  <div class="supplier-address">{{ supplier.address }}</div>
                </div>
              </div>
            </div>
          </el-card>
        </el-col>

        <!-- 产品列表 -->
        <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
          <el-card shadow="never">
            <template #header>
              <div class="card-header">
                <span>热门产品</span>
                <el-button type="primary" text @click="viewAllProducts">查看全部</el-button>
              </div>
            </template>
            <div class="product-list">
              <div v-if="state.products.length === 0" class="empty-state">
                <el-empty description="暂无产品数据" />
              </div>
              <div v-else>
                <div v-for="product in state.products" :key="product.id" class="product-item" @click="viewProduct(product)">
                  <div class="product-name">{{ product.name }}</div>
                  <div class="product-category">{{ product.category?.name || '-' }}</div>
                  <div class="product-description">{{ product.description }}</div>
                  <div class="product-price" v-if="product.price">¥{{ product.price }}</div>
                </div>
              </div>
            </div>
          </el-card>
        </el-col>
      </el-row>
    </div>
  </div>
</template>

<script lang="ts" setup name="client/public-query">
import { PublicQueryApi } from '/@/api/client/PublicQuery'
import { useRouter } from 'vue-router'
import { getCurrentInstance, reactive, onMounted } from 'vue'

const router = useRouter()
const { proxy } = getCurrentInstance() as any

const state = reactive({
  loading: false,
  searchModel: {
    keyword: '',
    type: 'all',
  },
  searchResults: null as any,
  suppliers: [] as any[],
  products: [] as any[],
})

// 搜索
const onSearch = async () => {
  if (!state.searchModel.keyword.trim()) {
    proxy.$modal.msgWarning('请输入搜索关键词')
    return
  }

  state.loading = true
  try {
    const res = await new PublicQueryApi().search({
      keyword: state.searchModel.keyword,
      type: state.searchModel.type,
    })
    
    if (res?.success) {
      state.searchResults = res.data
    }
  } catch (error) {
    console.error('搜索失败:', error)
  } finally {
    state.loading = false
  }
}

// 重置
const onReset = () => {
  state.searchModel.keyword = ''
  state.searchModel.type = 'all'
  state.searchResults = null
}

// 获取总结果数
const getTotalCount = () => {
  if (!state.searchResults) return 0
  const supplierCount = state.searchResults.suppliers?.length || 0
  const productCount = state.searchResults.products?.length || 0
  return supplierCount + productCount
}

// 是否有结果
const hasResults = () => {
  if (!state.searchResults) return false
  const supplierCount = state.searchResults.suppliers?.length || 0
  const productCount = state.searchResults.products?.length || 0
  return supplierCount > 0 || productCount > 0
}

// 查看供应商
const viewSupplier = (supplier: any) => {
  // 这里可以跳转到供应商详情页面
  console.log('查看供应商:', supplier)
}

// 查看产品
const viewProduct = (product: any) => {
  // 这里可以跳转到产品详情页面
  console.log('查看产品:', product)
}

// 查看所有供应商
const viewAllSuppliers = async () => {
  state.loading = true
  try {
    const res = await new PublicQueryApi().getPublicSuppliers({
      currentPage: 1,
      pageSize: 20,
    })
    
    if (res?.success) {
      state.suppliers = res.data?.list || []
    }
  } catch (error) {
    console.error('获取供应商列表失败:', error)
  } finally {
    state.loading = false
  }
}

// 查看所有产品
const viewAllProducts = async () => {
  state.loading = true
  try {
    const res = await new PublicQueryApi().getPublicProducts({
      currentPage: 1,
      pageSize: 20,
    })
    
    if (res?.success) {
      state.products = res.data?.list || []
    }
  } catch (error) {
    console.error('获取产品列表失败:', error)
  } finally {
    state.loading = false
  }
}

// 页面加载时获取默认数据
onMounted(() => {
  viewAllSuppliers()
  viewAllProducts()
})
</script>

<style scoped>
.search-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.search-section {
  margin-bottom: 30px;
}

.section-title {
  display: flex;
  align-items: center;
  margin-bottom: 16px;
  font-size: 16px;
  font-weight: 600;
  color: #303133;
}

.section-title .el-icon {
  margin-right: 8px;
  color: #409eff;
}

.supplier-card {
  cursor: pointer;
  transition: all 0.3s;
  margin-bottom: 16px;
}

.supplier-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.supplier-info {
  padding: 8px 0;
}

.supplier-name {
  font-size: 16px;
  font-weight: 600;
  color: #303133;
  margin-bottom: 8px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.supplier-scope {
  font-size: 14px;
  color: #606266;
  margin-bottom: 8px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.supplier-address {
  font-size: 12px;
  color: #909399;
  margin-bottom: 12px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.supplier-contact {
  border-top: 1px solid #f0f0f0;
  padding-top: 8px;
}

.contact-item {
  display: flex;
  align-items: center;
  font-size: 12px;
  color: #606266;
  margin-bottom: 4px;
}

.contact-item .el-icon {
  margin-right: 4px;
  color: #909399;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.supplier-list, .product-list {
  max-height: 400px;
  overflow-y: auto;
}

.supplier-item, .product-item {
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
  cursor: pointer;
  transition: background-color 0.3s;
}

.supplier-item:last-child, .product-item:last-child {
  border-bottom: none;
}

.supplier-item:hover, .product-item:hover {
  background-color: #f5f7fa;
}

.product-name {
  font-size: 14px;
  font-weight: 600;
  color: #303133;
  margin-bottom: 4px;
}

.product-category {
  font-size: 12px;
  color: #909399;
  margin-bottom: 4px;
}

.product-description {
  font-size: 12px;
  color: #606266;
  margin-bottom: 4px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.product-price {
  font-size: 14px;
  font-weight: 600;
  color: #e6a23c;
}

.empty-state {
  text-align: center;
  padding: 40px 0;
}
</style>
