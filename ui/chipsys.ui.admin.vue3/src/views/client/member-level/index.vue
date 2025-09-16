<template>
  <div class="member-level-container">
    <div class="member-level-header">
      <h2>会员等级管理</h2>
      <el-button type="primary" @click="handleAdd">
        <el-icon><Plus /></el-icon>
        添加等级
      </el-button>
    </div>
    
    <div class="member-level-content">
      <!-- 查询条件 -->
      <el-card class="query-card">
        <el-form :model="queryForm" :inline="true" @submit.stop.prevent>
          <el-form-item label="等级">
            <el-select 
              v-model="queryForm.level" 
              placeholder="请选择等级" 
              clearable 
              style="width: 150px"
            >
              <el-option label="免费会员" value="Free" />
              <el-option label="基础版" value="Basic" />
              <el-option label="标准版" value="Standard" />
              <el-option label="高级版" value="Premium" />
              <el-option label="企业版" value="Enterprise" />
            </el-select>
          </el-form-item>
          <el-form-item label="状态">
            <el-select 
              v-model="queryForm.enabled" 
              placeholder="请选择状态" 
              clearable 
              style="width: 150px"
            >
              <el-option label="启用" :value="true" />
              <el-option label="禁用" :value="false" />
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
            <span>会员等级列表</span>
          </div>
        </template>
        
        <el-table :data="tableData" v-loading="loading" border>
          <el-table-column prop="id" label="ID" width="80" />
          <el-table-column prop="level" label="等级" width="100" />
          <el-table-column prop="categoryLimit" label="分类限制" width="120" />
          <el-table-column prop="productDataLimit" label="产品数据限制" width="140" />
          <el-table-column prop="supplierDataLimit" label="供应商数据限制" width="140" />
          <el-table-column prop="showFullContactInfo" label="显示完整联系信息" width="160">
            <template #default="{ row }">
              <el-tag :type="row.showFullContactInfo ? 'success' : 'info'">
                {{ row.showFullContactInfo ? '是' : '否' }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column prop="enabled" label="状态" width="100">
            <template #default="{ row }">
              <el-tag :type="row.enabled ? 'success' : 'danger'">
                {{ row.enabled ? '启用' : '禁用' }}
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
    <MemberLevelForm ref="formRef" @refresh="handleRefresh" />
    
    <!-- 详情组件 -->
    <MemberLevelDetail ref="detailRef" @refresh="handleRefresh" @edit="handleEdit" />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus } from '@element-plus/icons-vue'
import { memberLevelApi } from '/@/api/client'
import type { MemberLevelGetPageOutput } from '/@/api/client/data-contracts'
import MemberLevelForm from './components/member-level-form.vue'
import MemberLevelDetail from './components/member-level-detail.vue'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const loading = ref(false)
const tableData = ref<MemberLevelGetPageOutput[]>([])
const pagination = reactive({
  currentPage: 1,
  pageSize: 10,
  total: 0
})

// 查询表单
const queryForm = reactive({
  level: '',
  enabled: undefined as boolean | undefined
})

// 组件引用
const formRef = ref()
const detailRef = ref()

// 获取列表数据
const getList = async () => {
  loading.value = true
  try {
    const res = await memberLevelApi.getPage({
      currentPage: pagination.currentPage,
      pageSize: pagination.pageSize,
      level: queryForm.level || undefined,
      enabled: queryForm.enabled
    })
    
    if (res?.success) {
      tableData.value = res.data.list
      pagination.total = res.data.total
    } else {
      proxy.$modal.msgError(res?.msg || '获取数据失败')
    }
  } catch (error) {
    console.error('获取会员等级列表失败:', error)
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
  queryForm.level = ''
  queryForm.enabled = undefined
  pagination.currentPage = 1
  getList()
}

// 添加
const handleAdd = () => {
  formRef.value?.open()
}

// 查看
const handleView = (row: MemberLevelGetPageOutput) => {
  detailRef.value?.open(row.id)
}

// 编辑
const handleEdit = (row: MemberLevelGetPageOutput) => {
  formRef.value?.open(row)
}

// 删除
const handleDelete = async (row: MemberLevelGetPageOutput) => {
  try {
    await ElMessageBox.confirm('确定要删除这个会员等级吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await memberLevelApi.delete(row.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('删除成功')
      getList()
    } else {
      proxy.$modal.msgError(res?.msg || '删除失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('删除会员等级失败:', error)
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
.member-level-container {
  padding: 20px;
}

.member-level-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.member-level-header h2 {
  margin: 0;
  color: #303133;
}

.member-level-content {
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
