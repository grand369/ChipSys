<template>
  <div class="quote-container">
    <div class="quote-header">
      <h2>报价管理</h2>
      <p>查看和管理供应商报价</p>
    </div>

    <div class="quote-content">
      <!-- 查询表单 -->
      <el-card shadow="never" class="search-card">
        <el-form :model="queryForm" inline>
          <el-form-item label="询价ID">
            <el-input-number
              v-model="queryForm.inquiryId"
              placeholder="请输入询价ID"
              :min="1"
              style="width: 150px"
            />
          </el-form-item>
          <el-form-item label="供应商名称">
            <el-input
              v-model="queryForm.supplierName"
              placeholder="请输入供应商名称"
              clearable
              style="width: 200px"
            />
          </el-form-item>
          <el-form-item label="报价状态">
            <el-select
              v-model="queryForm.status"
              placeholder="请选择状态"
              clearable
              style="width: 150px"
            >
              <el-option label="待审核" value="Pending" />
              <el-option label="已通过" value="Approved" />
              <el-option label="已接受" value="Accepted" />
              <el-option label="已拒绝" value="Rejected" />
              <el-option label="已取消" value="Cancelled" />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="handleQuery">
              <el-icon><Search /></el-icon>
              查询
            </el-button>
            <el-button @click="handleReset">
              <el-icon><Refresh /></el-icon>
              重置
            </el-button>
          </el-form-item>
        </el-form>
      </el-card>

      <!-- 数据表格 -->
      <el-card shadow="never" class="table-card">
        <template #header>
          <div class="card-header">
            <span>报价列表</span>
            <el-button 
              v-if="queryForm.inquiryId" 
              type="primary" 
              @click="handleAdd"
            >
              <el-icon><Plus /></el-icon>
              新增报价
            </el-button>
          </div>
        </template>

        <el-table
          v-loading="loading"
          :data="tableData"
          stripe
          style="width: 100%"
        >
          <el-table-column prop="id" label="ID" width="80" />
          <el-table-column prop="inquiryId" label="询价ID" width="100" align="center" />
          <el-table-column prop="supplierName" label="供应商名称" min-width="150" show-overflow-tooltip />
          <el-table-column prop="quotePrice" label="报价金额" width="120" align="center">
            <template #default="{ row }">
              <span class="price-text">{{ row.quotePrice }} {{ row.currency }}</span>
            </template>
          </el-table-column>
          <el-table-column prop="deliveryDays" label="交期" width="80" align="center">
            <template #default="{ row }">
              {{ row.deliveryDays }} 天
            </template>
          </el-table-column>
          <el-table-column prop="minOrderQuantity" label="最小起订量" width="100" align="center">
            <template #default="{ row }">
              {{ row.minOrderQuantity }} 件
            </template>
          </el-table-column>
          <el-table-column prop="validUntil" label="有效期" width="120">
            <template #default="{ row }">
              {{ formatDate(row.validUntil) }}
            </template>
          </el-table-column>
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }">
              <el-tag :type="getStatusType(row.status)">
                {{ getStatusText(row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column prop="createdTime" label="创建时间" width="160">
            <template #default="{ row }">
              {{ formatDate(row.createdTime) }}
            </template>
          </el-table-column>
          <el-table-column label="操作" width="200" fixed="right">
            <template #default="{ row }">
              <el-button type="primary" text @click="handleView(row)">
                查看
              </el-button>
              <el-button 
                v-if="row.status === 'Pending'" 
                type="warning" 
                text 
                @click="handleEdit(row)"
              >
                编辑
              </el-button>
              <el-button 
                v-if="row.status === 'Pending'" 
                type="success" 
                text 
                @click="handleAccept(row)"
              >
                接受
              </el-button>
              <el-button 
                v-if="row.status === 'Pending'" 
                type="danger" 
                text 
                @click="handleReject(row)"
              >
                拒绝
              </el-button>
            </template>
          </el-table-column>
        </el-table>

        <div class="pagination-container">
          <el-pagination
            v-model:current-page="pagination.currentPage"
            v-model:page-size="pagination.pageSize"
            :page-sizes="[10, 20, 50, 100]"
            :total="pagination.total"
            layout="total, sizes, prev, pager, next, jumper"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          />
        </div>
      </el-card>
    </div>

    <!-- 表单组件 -->
    <QuoteForm ref="formRef" @refresh="handleRefresh" />
    
    <!-- 详情组件 -->
    <QuoteDetail ref="detailRef" @refresh="handleRefresh" @edit="handleEdit" />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Search, Refresh } from '@element-plus/icons-vue'
import { quoteApi } from '/@/api/client'
import type { QuoteGetPageOutput } from '/@/api/client/data-contracts'
import QuoteForm from './components/quote-form.vue'
import QuoteDetail from './components/quote-detail.vue'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const loading = ref(false)
const tableData = ref<QuoteGetPageOutput[]>([])
const pagination = reactive({
  currentPage: 1,
  pageSize: 10,
  total: 0
})

// 查询表单
const queryForm = reactive({
  inquiryId: undefined as number | undefined,
  supplierName: '',
  status: ''
})

// 组件引用
const formRef = ref()
const detailRef = ref()

// 获取状态类型
const getStatusType = (status: string) => {
  const statusMap = {
    'Pending': 'warning',
    'Approved': 'success',
    'Accepted': 'success',
    'Rejected': 'danger',
    'Cancelled': 'info'
  }
  return statusMap[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string) => {
  const statusMap = {
    'Pending': '待审核',
    'Approved': '已通过',
    'Accepted': '已接受',
    'Rejected': '已拒绝',
    'Cancelled': '已取消'
  }
  return statusMap[status] || '未知'
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return '-'
  return new Date(date).toLocaleString('zh-CN')
}

// 获取列表数据
const getList = async () => {
  loading.value = true
  try {
    const res = await quoteApi.getPage({
      currentPage: pagination.currentPage,
      pageSize: pagination.pageSize,
      filter: {
        inquiryId: queryForm.inquiryId,
        supplierName: queryForm.supplierName || undefined,
        status: queryForm.status || undefined
      }
    })
    
    if (res?.success) {
      tableData.value = res.data.list
      pagination.total = res.data.total
    } else {
      proxy.$modal.msgError(res?.msg || '获取数据失败')
    }
  } catch (error) {
    console.error('获取报价列表失败:', error)
    proxy.$modal.msgError('获取数据失败')
  } finally {
    loading.value = false
  }
}

// 查询
const handleQuery = () => {
  pagination.currentPage = 1
  getList()
}

// 重置
const handleReset = () => {
  queryForm.inquiryId = undefined
  queryForm.supplierName = ''
  queryForm.status = ''
  pagination.currentPage = 1
  getList()
}

// 添加
const handleAdd = () => {
  if (queryForm.inquiryId) {
    formRef.value?.open(queryForm.inquiryId)
  } else {
    proxy.$modal.msgWarning('请先选择询价ID')
  }
}

// 查看
const handleView = (row: QuoteGetPageOutput) => {
  detailRef.value?.open(row.id)
}

// 编辑
const handleEdit = (row: QuoteGetPageOutput) => {
  formRef.value?.open(undefined, row)
}

// 接受报价
const handleAccept = async (row: QuoteGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要接受这个报价吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await quoteApi.accept(row.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('接受报价成功')
      getList()
    } else {
      proxy.$modal.msgError(res?.msg || '接受报价失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('接受报价失败:', error)
      proxy.$modal.msgError('接受报价失败')
    }
  }
}

// 拒绝报价
const handleReject = async (row: QuoteGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要拒绝这个报价吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await quoteApi.reject(row.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('拒绝报价成功')
      getList()
    } else {
      proxy.$modal.msgError(res?.msg || '拒绝报价失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('拒绝报价失败:', error)
      proxy.$modal.msgError('拒绝报价失败')
    }
  }
}

// 刷新列表
const handleRefresh = () => {
  getList()
}

// 分页大小改变
const handleSizeChange = (val: number) => {
  pagination.pageSize = val
  getList()
}

// 当前页改变
const handleCurrentChange = (val: number) => {
  pagination.currentPage = val
  getList()
}

// 组件挂载时获取数据
onMounted(() => {
  // 从路由参数获取询价ID
  const route = proxy.$route
  if (route.query.inquiryId) {
    queryForm.inquiryId = Number(route.query.inquiryId)
  }
  getList()
})
</script>

<style scoped>
.quote-container {
  padding: 20px;
}

.quote-header {
  margin-bottom: 20px;
}

.quote-header h2 {
  margin: 0 0 8px 0;
  color: #303133;
}

.quote-header p {
  margin: 0;
  color: #909399;
  font-size: 14px;
}

.quote-content {
  margin-top: 20px;
}

.search-card {
  margin-bottom: 20px;
}

.table-card {
  margin-top: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.pagination-container {
  margin-top: 20px;
  display: flex;
  justify-content: center;
}

.price-text {
  font-weight: bold;
  color: #e6a23c;
}
</style>
