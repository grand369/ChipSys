<template>
  <div class="supplier-application-container">
    <div class="supplier-application-header">
      <h2>供应商申请管理</h2>
      <el-button type="primary" @click="handleAdd">
        <el-icon><Plus /></el-icon>
        添加申请
      </el-button>
    </div>
    
    <div class="supplier-application-content">
      <!-- 查询条件 -->
      <el-card class="query-card">
        <el-form :model="queryForm" :inline="true" @submit.stop.prevent>
          <el-form-item label="公司名称">
            <el-input 
              v-model="queryForm.companyName" 
              placeholder="请输入公司名称" 
              clearable 
              style="width: 200px"
            />
          </el-form-item>
          <el-form-item label="联系人">
            <el-input 
              v-model="queryForm.contactName" 
              placeholder="请输入联系人姓名" 
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
              <el-option label="待审核" value="Pending" />
              <el-option label="已通过" value="Approved" />
              <el-option label="已拒绝" value="Rejected" />
              <el-option label="已取消" value="Cancelled" />
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
            <span>供应商申请列表</span>
          </div>
        </template>
        
        <el-table :data="tableData" v-loading="loading" border>
          <el-table-column prop="id" label="ID" width="80" />
          <el-table-column prop="companyName" label="公司名称" min-width="200" />
          <el-table-column prop="contactName" label="联系人" width="120" />
          <el-table-column prop="contactPhone" label="联系电话" width="140" />
          <el-table-column prop="contactEmail" label="邮箱" width="180" />
          <el-table-column prop="status" label="状态" width="100">
            <template #default="{ row }">
              <el-tag :type="getStatusType(row.status)">
                {{ getStatusText(row.status) }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column prop="createdTime" label="申请时间" width="180" />
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
    <SupplierApplicationForm ref="formRef" @refresh="handleRefresh" />
    
    <!-- 详情组件 -->
    <SupplierApplicationDetail ref="detailRef" @refresh="handleRefresh" @edit="handleEdit" />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus } from '@element-plus/icons-vue'
import { supplierApplicationApi } from '/@/api/client'
import type { SupplierApplicationGetPageOutput } from '/@/api/client/data-contracts'
import SupplierApplicationForm from './components/supplier-application-form.vue'
import SupplierApplicationDetail from './components/supplier-application-detail.vue'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const loading = ref(false)
const tableData = ref<SupplierApplicationGetPageOutput[]>([])
const pagination = reactive({
  currentPage: 1,
  pageSize: 10,
  total: 0
})

// 查询表单
const queryForm = reactive({
  companyName: '',
  contactName: '',
  status: ''
})

// 组件引用
const formRef = ref()
const detailRef = ref()

// 获取状态类型
const getStatusType = (status: string | number) => {
  const statusMap = {
    'Draft': 'info',
    'Pending': 'warning', 
    'Approved': 'success',
    'Rejected': 'danger',
    'Cancelled': 'info'
  }
  return statusMap[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string | number) => {
  const statusMap = {
    'Draft': '草稿',
    'Pending': '待审核',
    'Approved': '已通过',
    'Rejected': '已拒绝',
    'Cancelled': '已取消'
  }
  return statusMap[status] || '未知'
}

// 获取列表数据
const getList = async () => {
  loading.value = true
  try {
    const res = await supplierApplicationApi.getPage({
      currentPage: pagination.currentPage,
      pageSize: pagination.pageSize,
      filter: {
        companyName: queryForm.companyName || undefined,
        contactName: queryForm.contactName || undefined,
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
    console.error('获取供应商申请列表失败:', error)
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
  queryForm.companyName = ''
  queryForm.contactName = ''
  queryForm.status = ''
  pagination.currentPage = 1
  getList()
}

// 添加
const handleAdd = () => {
  formRef.value?.open()
}

// 查看
const handleView = (row: SupplierApplicationGetPageOutput) => {
  detailRef.value?.open(row.id)
}

// 编辑
const handleEdit = (row: SupplierApplicationGetPageOutput) => {
  formRef.value?.open(row)
}

// 删除
const handleDelete = async (row: SupplierApplicationGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要删除这条申请记录吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await supplierApplicationApi.delete(row.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('删除成功')
      getList()
    } else {
      proxy.$modal.msgError(res?.msg || '删除失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('删除供应商申请失败:', error)
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
.supplier-application-container {
  padding: 20px;
}

.supplier-application-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.supplier-application-header h2 {
  margin: 0;
  color: #303133;
}

.supplier-application-content {
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
