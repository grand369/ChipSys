<template>
  <div class="member-level-price-plan-container">
    <el-row :gutter="16">
      <!-- 左侧：筛选条件 -->
      <el-col :xs="24" :sm="6" :md="6" :lg="6" :xl="6">
        <el-card shadow="never" class="h100">
          <template #header>
            <span>筛选条件</span>
          </template>
          
          <el-form :model="searchForm" label-width="80px" size="small">
            <el-form-item label="会员等级">
              <el-select v-model="searchForm.memberLevelId" placeholder="全部等级" clearable @change="handleSearch">
                <el-option v-for="level in memberLevels" :key="level.id" :value="level.id" :label="level.levelName" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="方案类型">
              <el-select v-model="searchForm.planType" placeholder="全部类型" clearable @change="handleSearch">
                <el-option v-for="option in PricePlanTypeOptions" :key="option.value" :value="option.value" :label="option.label" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="状态">
              <el-select v-model="searchForm.enabled" placeholder="全部状态" clearable @change="handleSearch">
                <el-option :value="true" label="启用" />
                <el-option :value="false" label="禁用" />
              </el-select>
            </el-form-item>
            
            <el-form-item>
              <el-button type="primary" @click="handleSearch" class="w-full">搜索</el-button>
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>

      <!-- 右侧：数据表格 -->
      <el-col :xs="24" :sm="18" :md="18" :lg="18" :xl="18">
        <el-card shadow="never" class="h100">
          <template #header>
            <div class="card-header">
              <div>
                <span>价格方案管理</span>
                <el-tag class="ml8" size="small" type="info">共 {{ total }} 条记录</el-tag>
              </div>
              <div>
                <el-button type="success" size="small" @click="handleAdd">
                  <i class="fas fa-plus mr-1"></i>
                  新增方案
                </el-button>
                <el-button type="warning" size="small" @click="handleBatchSet">
                  <i class="fas fa-cogs mr-1"></i>
                  批量设置
                </el-button>
                <el-button size="small" @click="handleRefresh">
                  <i class="fas fa-sync-alt"></i>
                </el-button>
              </div>
            </div>
          </template>

          <el-table :data="list" border v-loading="loading">
            <el-table-column prop="planName" label="方案名称" width="120">
              <template #default="{ row }">
                <div>
                  <div class="font-medium">{{ row.planName }}</div>
                  <div class="text-xs text-gray-500">{{ getPlanTypeLabel(row.planType) }}</div>
                  <div v-if="row.description" class="text-xs text-gray-400 mt-1">{{ row.description }}</div>
                </div>
              </template>
            </el-table-column>
            
            <el-table-column label="价格信息" width="150">
              <template #default="{ row }">
                <div>
                  <div class="text-sm">原价: ¥{{ row.originalPrice }}</div>
                  <div class="text-sm text-green-600 font-medium">折扣价: ¥{{ row.discountedPrice }}</div>
                </div>
              </template>
            </el-table-column>
            
            <el-table-column label="折扣信息" width="120">
              <template #default="{ row }">
                <div class="flex items-center">
                  <span class="text-red-600 font-medium">{{ row.discountPercent }}%</span>
                  <el-tag v-if="row.isRecommended" size="small" type="warning" class="ml-2">推荐</el-tag>
                </div>
              </template>
            </el-table-column>
            
            <el-table-column prop="enabled" label="状态" width="80">
              <template #default="{ row }">
                <el-tag :type="row.enabled ? 'success' : 'danger'" size="small">
                  {{ row.enabled ? '启用' : '禁用' }}
                </el-tag>
              </template>
            </el-table-column>
            
            <el-table-column prop="sort" label="排序" width="80" />
            
            <el-table-column prop="createdTime" label="创建时间" width="160">
              <template #default="{ row }">
                {{ formatDate(row.createdTime) }}
              </template>
            </el-table-column>
            
            <el-table-column label="操作" width="150" fixed="right">
              <template #default="{ row }">
                <el-button text type="primary" size="small" @click="handleEdit(row)">编辑</el-button>
                <el-button 
                  text 
                  :type="row.enabled ? 'danger' : 'success'" 
                  size="small" 
                  @click="handleToggleEnabled(row)"
                >
                  {{ row.enabled ? '禁用' : '启用' }}
                </el-button>
                <el-button text type="danger" size="small" @click="handleDelete(row)">删除</el-button>
              </template>
            </el-table-column>
          </el-table>
          
          <!-- 分页 -->
          <div class="pagination-container">
            <el-pagination
              v-model:current-page="currentPage"
              v-model:page-size="pageSize"
              :page-sizes="[10, 20, 50, 100]"
              :total="total"
              layout="total, sizes, prev, pager, next, jumper"
              @size-change="handleSizeChange"
              @current-change="handleCurrentChange"
            />
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 新增/编辑对话框 -->
    <PricePlanDialog 
      v-if="dialogVisible"
      :visible="dialogVisible"
      :form-data="dialogFormData"
      :member-levels="memberLevels"
      @close="handleDialogClose"
      @submit="handleDialogSubmit"
    />

    <!-- 批量设置对话框 -->
    <BatchSetDialog 
      v-if="batchSetVisible"
      :visible="batchSetVisible"
      :member-levels="memberLevels"
      @close="handleBatchSetClose"
      @submit="handleBatchSetSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { MemberLevelPricePlanApi, PricePlanTypeOptions, type MemberLevelPricePlanGetPageOutput, type MemberLevelPricePlanAddInput, type MemberLevelPricePlanUpdateInput, type BatchSetPricePlansInput } from '/@/api/admin/MemberLevelPricePlan'
import { MemberLevelManageApi, type MemberLevelGetOutput } from '/@/api/admin/MemberLevelManage'
import PricePlanDialog from './components/PricePlanDialog.vue'
import BatchSetDialog from './components/BatchSetDialog.vue'

// 响应式数据
const list = ref<MemberLevelPricePlanGetPageOutput[]>([])
const total = ref(0)
const currentPage = ref(1)
const pageSize = ref(20)
const loading = ref(false)

// 搜索表单
const searchForm = reactive({
  memberLevelId: '',
  planType: '',
  enabled: ''
})

// 会员等级列表
const memberLevels = ref<MemberLevelGetOutput[]>([])

// 对话框状态
const dialogVisible = ref(false)
const dialogFormData = ref<MemberLevelPricePlanAddInput | MemberLevelPricePlanUpdateInput | null>(null)
const batchSetVisible = ref(false)

// 计算属性
const totalPages = computed(() => Math.ceil(total.value / pageSize.value))
const visiblePages = computed(() => {
  const pages = []
  const start = Math.max(1, currentPage.value - 2)
  const end = Math.min(totalPages.value, start + 4)
  for (let i = start; i <= end; i++) {
    pages.push(i)
  }
  return pages
})

// 方法
const getPlanTypeLabel = (planType: string) => {
  const option = PricePlanTypeOptions.find(opt => opt.value === planType)
  return option ? option.label : planType
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleString('zh-CN')
}

const loadData = async () => {
  try {
    loading.value = true
    const api = new MemberLevelPricePlanApi()
    const response = await api.getPage({
      currentPage: currentPage.value,
      pageSize: pageSize.value,
      filter: {
        memberLevelId: searchForm.memberLevelId ? Number(searchForm.memberLevelId) : undefined,
        planType: searchForm.planType || undefined,
        enabled: searchForm.enabled !== '' ? searchForm.enabled === 'true' : undefined
      }
    })
    list.value = response.data?.list || []
    total.value = response.data?.total || 0
  } catch (error) {
    ElMessage.error('加载数据失败')
    console.error(error)
  } finally {
    loading.value = false
  }
}

const loadMemberLevels = async () => {
  try {
    const api = new MemberLevelManageApi()
    const response = await api.getPage({
      currentPage: 1,
      pageSize: 1000,
      filter: {}
    })
    memberLevels.value = response.data?.list || []
  } catch (error) {
    console.error('加载会员等级失败:', error)
  }
}

const handleSearch = () => {
  currentPage.value = 1
  loadData()
}

const handleRefresh = () => {
  loadData()
}

const handlePageChange = (page: number) => {
  currentPage.value = page
  loadData()
}

const handleSizeChange = (size: number) => {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}

const handleCurrentChange = (page: number) => {
  currentPage.value = page
  loadData()
}

const handleAdd = () => {
  dialogFormData.value = {
    memberLevelId: 0,
    planType: 'Monthly',
    planName: '',
    originalPrice: 0,
    discountedPrice: 0,
    discountPercent: 0,
    description: '',
    isRecommended: false,
    enabled: true,
    sort: 0
  }
  dialogVisible.value = true
}

const handleEdit = (item: MemberLevelPricePlanGetPageOutput) => {
  dialogFormData.value = { ...item }
  dialogVisible.value = true
}

const handleDelete = async (item: MemberLevelPricePlanGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要删除这个价格方案吗？', '确认删除', {
      type: 'warning'
    })
    const api = new MemberLevelPricePlanApi()
    await api.delete(item.id)
    ElMessage.success('删除成功')
    loadData()
  } catch (error) {
    if (error !== 'cancel') {
      ElMessage.error('删除失败')
      console.error(error)
    }
  }
}

const handleToggleEnabled = async (item: MemberLevelPricePlanGetPageOutput) => {
  try {
    const api = new MemberLevelPricePlanApi()
    await api.toggleEnabled(item.id, !item.enabled)
    ElMessage.success(item.enabled ? '已禁用' : '已启用')
    loadData()
  } catch (error) {
    ElMessage.error('操作失败')
    console.error(error)
  }
}

const handleBatchSet = () => {
  batchSetVisible.value = true
}

const handleDialogClose = () => {
  dialogVisible.value = false
  dialogFormData.value = null
}

const handleDialogSubmit = async (formData: MemberLevelPricePlanAddInput | MemberLevelPricePlanUpdateInput) => {
  try {
    const api = new MemberLevelPricePlanApi()
    if ('id' in formData) {
      await api.update(formData as MemberLevelPricePlanUpdateInput)
      ElMessage.success('更新成功')
    } else {
      await api.add(formData as MemberLevelPricePlanAddInput)
      ElMessage.success('添加成功')
    }
    handleDialogClose()
    loadData()
  } catch (error) {
    ElMessage.error('操作失败')
    console.error(error)
  }
}

const handleBatchSetClose = () => {
  batchSetVisible.value = false
}

const handleBatchSetSubmit = async (data: BatchSetPricePlansInput) => {
  try {
    const api = new MemberLevelPricePlanApi()
    await api.batchSetPricePlans(data)
    ElMessage.success('批量设置成功')
    handleBatchSetClose()
    loadData()
  } catch (error) {
    ElMessage.error('批量设置失败')
    console.error(error)
  }
}

// 生命周期
onMounted(() => {
  loadMemberLevels()
  loadData()
})
</script>

<style scoped>
.member-level-price-plan-container { height: 100%; }
.h100 { height: 100%; }
.card-header { display: flex; justify-content: space-between; align-items: center; }
.w-full { width: 100%; }
.ml8 { margin-left: 8px; }
.pagination-container { margin-top: 12px; display: flex; justify-content: flex-end; }
</style>
