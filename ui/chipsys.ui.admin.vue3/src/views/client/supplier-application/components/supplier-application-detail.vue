<template>
  <el-dialog
    v-model="state.visible"
    title="供应商申请详情"
    width="800px"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onClose"
  >
    <div v-loading="state.loading" class="detail-content">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="申请ID">
          {{ state.data.id }}
        </el-descriptions-item>
        <el-descriptions-item label="申请状态">
          <el-tag :type="getStatusType(state.data.status)">
            {{ getStatusText(state.data.status) }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="公司名称" :span="2">
          {{ state.data.companyName }}
        </el-descriptions-item>
        <el-descriptions-item label="公司地址" :span="2">
          {{ state.data.companyAddress || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="联系人姓名">
          {{ state.data.contactName }}
        </el-descriptions-item>
        <el-descriptions-item label="联系电话">
          {{ state.data.contactPhone }}
        </el-descriptions-item>
        <el-descriptions-item label="联系邮箱">
          {{ state.data.contactEmail || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="营业执照号">
          {{ state.data.businessLicense || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="主要产品类别" :span="2">
          {{ state.data.mainProductCategories || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="公司简介" :span="2">
          <div class="text-content">
            {{ state.data.companyDescription || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="申请理由" :span="2">
          <div class="text-content">
            {{ state.data.applicationReason || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="审核意见" :span="2" v-if="state.data.reviewComment">
          <div class="text-content">
            {{ state.data.reviewComment }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="审核时间" v-if="state.data.reviewTime">
          {{ formatDate(state.data.reviewTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="创建时间">
          {{ formatDate(state.data.createdTime) }}
        </el-descriptions-item>
      </el-descriptions>
    </div>

    <template #footer>
      <div class="dialog-footer">
        <el-button @click="onClose">关闭</el-button>
        <el-button 
          v-if="state.data.status === 'Draft'" 
          type="primary" 
          @click="onEdit"
        >
          编辑
        </el-button>
        <el-button 
          v-if="state.data.status === 'Draft'" 
          type="success" 
          @click="onSubmit"
        >
          提交申请
        </el-button>
        <el-button 
          v-if="state.data.status === 'Pending'" 
          type="warning" 
          @click="onCancel"
        >
          撤销申请
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { reactive, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { supplierApplicationApi } from '/@/api/client'
import type { SupplierApplicationGetOutput } from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const state = reactive({
  visible: false,
  loading: false,
  data: {} as SupplierApplicationGetOutput
})

// 打开对话框
const open = async (id: number) => {
  state.visible = true
  state.loading = true
  
  try {
    const res = await supplierApplicationApi.get(id)
    if (res?.success) {
      state.data = res.data
    } else {
      proxy.$modal.msgError(res?.msg || '获取详情失败')
    }
  } catch (error) {
    console.error('获取供应商申请详情失败:', error)
    proxy.$modal.msgError('获取详情失败')
  } finally {
    state.loading = false
  }
}

// 关闭对话框
const onClose = () => {
  state.visible = false
  state.data = {} as SupplierApplicationGetOutput
}

// 编辑
const onEdit = () => {
  emit('edit', state.data)
  onClose()
}

// 提交申请
const onSubmit = async () => {
  try {
    await ElMessageBox.confirm('确定要提交申请吗？提交后将无法修改。', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await supplierApplicationApi.submit(state.data.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('提交成功')
      emit('refresh')
      onClose()
    } else {
      proxy.$modal.msgError(res?.msg || '提交失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('提交申请失败:', error)
      proxy.$modal.msgError('提交失败')
    }
  }
}

// 撤销申请
const onCancel = async () => {
  try {
    await ElMessageBox.confirm('确定要撤销申请吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await supplierApplicationApi.cancel(state.data.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('撤销成功')
      emit('refresh')
      onClose()
    } else {
      proxy.$modal.msgError(res?.msg || '撤销失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('撤销申请失败:', error)
      proxy.$modal.msgError('撤销失败')
    }
  }
}

// 获取状态类型
const getStatusType = (status: string) => {
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
const getStatusText = (status: string) => {
  const statusMap = {
    'Draft': '草稿',
    'Pending': '待审核',
    'Approved': '已通过',
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

// 定义事件
const emit = defineEmits<{
  refresh: []
  edit: [data: SupplierApplicationGetOutput]
}>()

// 暴露方法
defineExpose({
  open,
  close: onClose,
})
</script>

<style scoped>
.detail-content {
  padding: 20px 0;
}

.text-content {
  white-space: pre-wrap;
  word-break: break-word;
  line-height: 1.6;
}

.dialog-footer {
  text-align: right;
}

.el-descriptions {
  margin-top: 20px;
}
</style>
