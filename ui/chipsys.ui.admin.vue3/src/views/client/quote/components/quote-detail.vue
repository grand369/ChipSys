<template>
  <el-dialog
    v-model="state.visible"
    title="报价详情"
    width="800px"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onClose"
  >
    <div v-loading="state.loading" class="detail-content">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="报价ID">
          {{ state.data.id }}
        </el-descriptions-item>
        <el-descriptions-item label="询价ID">
          {{ state.data.inquiryId }}
        </el-descriptions-item>
        <el-descriptions-item label="供应商ID">
          {{ state.data.supplierId }}
        </el-descriptions-item>
        <el-descriptions-item label="供应商名称">
          {{ state.data.supplierName }}
        </el-descriptions-item>
        <el-descriptions-item label="报价状态">
          <el-tag :type="getStatusType(state.data.status)">
            {{ getStatusText(state.data.status) }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="报价金额">
          <span class="price-text">
            {{ state.data.quotePrice }} {{ state.data.currency }}
          </span>
        </el-descriptions-item>
        <el-descriptions-item label="交期天数">
          {{ state.data.deliveryDays }} 天
        </el-descriptions-item>
        <el-descriptions-item label="最小起订量">
          {{ state.data.minOrderQuantity }} 件
        </el-descriptions-item>
        <el-descriptions-item label="有效期至">
          {{ formatDate(state.data.validUntil) }}
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
        <el-descriptions-item label="产品规格说明" :span="2">
          <div class="text-content">
            {{ state.data.productSpecification || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="质量保证" :span="2">
          <div class="text-content">
            {{ state.data.qualityAssurance || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="报价备注" :span="2">
          <div class="text-content">
            {{ state.data.quoteRemark || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="创建时间">
          {{ formatDate(state.data.createdTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="更新时间">
          {{ formatDate(state.data.updatedTime) }}
        </el-descriptions-item>
      </el-descriptions>
    </div>

    <template #footer>
      <div class="dialog-footer">
        <el-button @click="onClose">关闭</el-button>
        <el-button 
          v-if="state.data.status === 'Pending'" 
          type="primary" 
          @click="onEdit"
        >
          编辑
        </el-button>
        <el-button 
          v-if="state.data.status === 'Pending'" 
          type="success" 
          @click="onAccept"
        >
          接受报价
        </el-button>
        <el-button 
          v-if="state.data.status === 'Pending'" 
          type="warning" 
          @click="onReject"
        >
          拒绝报价
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { reactive, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { quoteApi } from '/@/api/client'
import type { QuoteGetOutput } from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const state = reactive({
  visible: false,
  loading: false,
  data: {} as QuoteGetOutput
})

// 打开对话框
const open = async (id: number) => {
  state.visible = true
  state.loading = true
  
  try {
    const res = await quoteApi.get(id)
    if (res?.success) {
      state.data = res.data
    } else {
      proxy.$modal.msgError(res?.msg || '获取详情失败')
    }
  } catch (error) {
    console.error('获取报价详情失败:', error)
    proxy.$modal.msgError('获取详情失败')
  } finally {
    state.loading = false
  }
}

// 关闭对话框
const onClose = () => {
  state.visible = false
  state.data = {} as QuoteGetOutput
}

// 编辑
const onEdit = () => {
  emit('edit', state.data)
  onClose()
}

// 接受报价
const onAccept = async () => {
  try {
    await ElMessageBox.confirm('确定要接受这个报价吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await quoteApi.accept(state.data.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('接受报价成功')
      emit('refresh')
      onClose()
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
const onReject = async () => {
  try {
    await ElMessageBox.confirm('确定要拒绝这个报价吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await quoteApi.reject(state.data.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('拒绝报价成功')
      emit('refresh')
      onClose()
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

// 定义事件
const emit = defineEmits<{
  refresh: []
  edit: [data: QuoteGetOutput]
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

.price-text {
  font-weight: bold;
  color: #e6a23c;
  font-size: 16px;
}

.dialog-footer {
  text-align: right;
}

.el-descriptions {
  margin-top: 20px;
}
</style>
