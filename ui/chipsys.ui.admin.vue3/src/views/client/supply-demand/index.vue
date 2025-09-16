<template>
  <div class="supply-demand-container">
    <div class="supply-demand-header">
      <h2>供求信息管理</h2>
      <el-button type="primary" @click="handleAdd">
        <el-icon><Plus /></el-icon>
        添加供求信息
      </el-button>
    </div>
    
    <div class="supply-demand-content">
      <!-- 查询条件 -->
      <el-card class="query-card">
        <el-form :model="queryForm" :inline="true" @submit.stop.prevent>
          <el-form-item label="信息类型">
            <el-select 
              v-model="queryForm.infoType" 
              placeholder="请选择信息类型" 
              clearable 
              style="width: 150px"
            >
              <el-option label="供应" value="Supply" />
              <el-option label="需求" value="Demand" />
              <el-option label="询价" value="Inquiry" />
            </el-select>
          </el-form-item>
          <el-form-item label="产品名称">
            <el-input 
              v-model="queryForm.productName" 
              placeholder="请输入产品名称" 
              clearable 
              style="width: 200px"
            />
          </el-form-item>
          <el-form-item label="状态">
            <el-select 
              v-model="queryForm.status" 
              placeholder="请选择状态" 
              clearable 
              style="width: 150px"
            >
              <el-option label="草稿" value="Draft" />
              <el-option label="已发布" value="Published" />
              <el-option label="已关闭" value="Closed" />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" icon="ele-Search" @click="handleQuery">查询</el-button>
            <el-button icon="ele-Refresh" @click="handleReset">重置</el-button>
          </el-form-item>
        </el-form>
      </el-card>

      <el-card class="table-card">
        <template #header>
          <div class="card-header">
            <span>供求信息列表</span>
          </div>
        </template>
        
        <el-table :data="tableData" v-loading="loading" border>
          <el-table-column prop="id" label="ID" width="80" />
          <el-table-column prop="title" label="标题" min-width="200" />
          <el-table-column prop="infoType" label="类型" width="100">
            <template #default="{ row }">
              <el-tag :type="row.infoType === 'Supply' ? 'success' : 'warning'">
                {{ row.infoType === 'Supply' ? '供应' : '需求' }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column prop="productName" label="产品名称" width="150" />
          <el-table-column prop="status" label="状态" width="100">
            <template #default="{ row }">
              <el-tag :type="getStatusType(row.status)">
                {{ getStatusText(row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column prop="createdTime" label="创建时间" width="180" />
          <el-table-column label="操作" width="200" fixed="right">
            <template #default="{ row }">
              <el-button size="small" @click="handleView(row)">查看</el-button>
              <el-button size="small" type="primary" @click="handleEdit(row)">编辑</el-button>
              <el-button size="small" type="danger" @click="handleDelete(row)">删除</el-button>
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
    <SupplyDemandForm ref="formRef" @refresh="handleRefresh" />
    
    <!-- 详情组件 -->
    <SupplyDemandDetail ref="detailRef" @refresh="handleRefresh" @edit="handleEdit" />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus } from '@element-plus/icons-vue'
import { supplyDemandApi } from '/@/api/client'
import type { SupplyDemandGetPageOutput } from '/@/api/client/data-contracts'
import SupplyDemandForm from './components/supply-demand-form.vue'
import SupplyDemandDetail from './components/supply-demand-detail.vue'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const loading = ref(false)
const tableData = ref<SupplyDemandGetPageOutput[]>([])
const pagination = reactive({
  currentPage: 1,
  pageSize: 10,
  total: 0
})

// 查询表单
const queryForm = reactive({
  infoType: '',
  productName: '',
  status: ''
})

// 组件引用
const formRef = ref()
const detailRef = ref()

// 获取状态类型
const getStatusType = (status: string | number) => {
  const statusMap = {
    'Draft': 'info',
    'Published': 'success',
    'Closed': 'warning',
    'Deleted': 'danger'
  }
  return statusMap[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string | number) => {
  const statusMap = {
    'Draft': '草稿',
    'Published': '已发布',
    'Closed': '已关闭',
    'Deleted': '已删除'
  }
  return statusMap[status] || '未知'
}

// 获取列表数据
const getList = async () => {
  loading.value = true
  try {
    const res = await supplyDemandApi.getPage({
      currentPage: pagination.currentPage,
      pageSize: pagination.pageSize,
      filter: {
        infoType: queryForm.infoType || undefined,
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
    console.error('获取供求信息列表失败:', error)
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
  queryForm.infoType = ''
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
const handleView = (row: SupplyDemandGetPageOutput) => {
  detailRef.value?.open(row.id)
}

// 编辑
const handleEdit = (row: SupplyDemandGetPageOutput) => {
  formRef.value?.open(row)
}

// 删除
const handleDelete = async (row: SupplyDemandGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要删除这条供求信息吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await supplyDemandApi.delete(row.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('删除成功')
      getList()
    } else {
      proxy.$modal.msgError(res?.msg || '删除失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('删除供求信息失败:', error)
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
.supply-demand-container {
  padding: 20px;
}

.supply-demand-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.supply-demand-header h2 {
  margin: 0;
  color: #303133;
}

.supply-demand-content {
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
