<template>
  <div class="inquiry-container">
    <div class="inquiry-header">
      <h2>询价管理</h2>
      <p>发布询价需求，获取供应商报价</p>
    </div>

    <div class="inquiry-content">
      <!-- 查询表单 -->
      <el-card shadow="never" class="search-card">
        <el-form :model="queryForm" inline>
          <el-form-item label="询价标题">
            <el-input
              v-model="queryForm.title"
              placeholder="请输入询价标题"
              clearable
              style="width: 200px"
            />
          </el-form-item>
          <el-form-item label="产品名称">
            <el-input
              v-model="queryForm.productName"
              placeholder="请输入产品名称"
              clearable
              style="width: 200px"
            />
          </el-form-item>
          <el-form-item label="询价状态">
            <el-select
              v-model="queryForm.status"
              placeholder="请选择状态"
              clearable
              style="width: 150px"
            >
              <el-option label="草稿" value="Draft" />
              <el-option label="已发布" value="Published" />
              <el-option label="已关闭" value="Closed" />
              <el-option label="已完成" value="Completed" />
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
            <span>询价列表</span>
            <el-button type="primary" @click="handleAdd">
              <el-icon><Plus /></el-icon>
              新增询价
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
          <el-table-column prop="title" label="询价标题" min-width="150" show-overflow-tooltip />
          <el-table-column prop="productName" label="产品名称" min-width="120" show-overflow-tooltip />
          <el-table-column prop="quantity" label="需求数量" width="100" align="center">
            <template #default="{ row }">
              {{ row.quantity }} 件
            </template>
          </el-table-column>
          <el-table-column prop="budgetRange" label="预算范围" width="120" show-overflow-tooltip />
          <el-table-column prop="expectedDeliveryDays" label="期望交期" width="100" align="center">
            <template #default="{ row }">
              {{ row.expectedDeliveryDays }} 天
            </template>
          </el-table-column>
          <el-table-column prop="quoteCount" label="报价数量" width="100" align="center">
            <template #default="{ row }">
              <el-tag v-if="row.quoteCount > 0" type="success">{{ row.quoteCount }}</el-tag>
              <span v-else>0</span>
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
                v-if="row.status === 'Draft'" 
                type="warning" 
                text 
                @click="handleEdit(row)"
              >
                编辑
              </el-button>
              <el-button 
                v-if="row.status === 'Published'" 
                type="info" 
                text 
                @click="handleViewQuotes(row)"
              >
                报价
              </el-button>
              <el-button 
                v-if="row.status === 'Draft'" 
                type="danger" 
                text 
                @click="handleDelete(row)"
              >
                删除
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
    <InquiryForm ref="formRef" @refresh="handleRefresh" />
    
    <!-- 详情组件 -->
    <InquiryDetail ref="detailRef" @refresh="handleRefresh" @edit="handleEdit" @viewQuotes="handleViewQuotes" />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Search, Refresh } from '@element-plus/icons-vue'
import { inquiryApi } from '/@/api/client'
import type { InquiryGetPageOutput } from '/@/api/client/data-contracts'
import InquiryForm from './components/inquiry-form.vue'
import InquiryDetail from './components/inquiry-detail.vue'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const loading = ref(false)
const tableData = ref<InquiryGetPageOutput[]>([])
const pagination = reactive({
  currentPage: 1,
  pageSize: 10,
  total: 0
})

// 查询表单
const queryForm = reactive({
  title: '',
  productName: '',
  status: ''
})

// 组件引用
const formRef = ref()
const detailRef = ref()

// 获取状态类型
const getStatusType = (status: string) => {
  const statusMap = {
    'Draft': 'info',
    'Published': 'success',
    'Closed': 'warning',
    'Completed': 'primary',
    'Cancelled': 'danger'
  }
  return statusMap[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string) => {
  const statusMap = {
    'Draft': '草稿',
    'Published': '已发布',
    'Closed': '已关闭',
    'Completed': '已完成',
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
    const res = await inquiryApi.getPage({
      currentPage: pagination.currentPage,
      pageSize: pagination.pageSize,
      filter: {
        title: queryForm.title || undefined,
        productName: queryForm.productName || undefined,
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
    console.error('获取询价列表失败:', error)
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
  queryForm.title = ''
  queryForm.productName = ''
  queryForm.status = ''
  pagination.currentPage = 1
  getList()
}

// 添加
const handleAdd = () => {
  formRef.value?.open()
}

// 查看
const handleView = (row: InquiryGetPageOutput) => {
  detailRef.value?.open(row.id)
}

// 编辑
const handleEdit = (row: InquiryGetPageOutput) => {
  formRef.value?.open(row)
}

// 查看报价
const handleViewQuotes = (row: InquiryGetPageOutput) => {
  // 跳转到报价页面，并传递询价ID
  proxy.$router.push({
    path: '/client/quote',
    query: { inquiryId: row.id }
  })
}

// 删除
const handleDelete = async (row: InquiryGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要删除这个询价吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await inquiryApi.delete(row.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('删除成功')
      getList()
    } else {
      proxy.$modal.msgError(res?.msg || '删除失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('删除询价失败:', error)
      proxy.$modal.msgError('删除失败')
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
  getList()
})
</script>

<style scoped>
.inquiry-container {
  padding: 20px;
}

.inquiry-header {
  margin-bottom: 20px;
}

.inquiry-header h2 {
  margin: 0 0 8px 0;
  color: #303133;
}

.inquiry-header p {
  margin: 0;
  color: #909399;
  font-size: 14px;
}

.inquiry-content {
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
</style>
