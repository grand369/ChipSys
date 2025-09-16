<template>
  <el-dialog
    v-model="state.visible"
    title="供求信息详情"
    width="800px"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onClose"
  >
    <div v-loading="state.loading" class="detail-content">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="信息ID">
          {{ state.data.id }}
        </el-descriptions-item>
        <el-descriptions-item label="信息状态">
          <el-tag :type="getStatusType(state.data.status)">
            {{ getStatusText(state.data.status) }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="信息类型">
          <el-tag :type="getInfoTypeType(state.data.infoType)">
            {{ getInfoTypeText(state.data.infoType) }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="产品名称">
          {{ state.data.productName }}
        </el-descriptions-item>
        <el-descriptions-item label="产品型号">
          {{ state.data.productModel || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="产品品牌">
          {{ state.data.productBrand || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="产品制造商">
          {{ state.data.productManufacturer || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="数量要求">
          {{ state.data.quantity ? `${state.data.quantity} 件` : '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="价格范围">
          {{ state.data.priceRange || '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="供货周期">
          {{ state.data.deliveryDays ? `${state.data.deliveryDays} 天` : '-' }}
        </el-descriptions-item>
        <el-descriptions-item label="技术参数要求" :span="2">
          <div class="text-content">
            {{ state.data.technicalRequirements || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="联系信息" :span="2">
          <div class="text-content">
            {{ state.data.contactInfo || '-' }}
          </div>
        </el-descriptions-item>
        <el-descriptions-item label="详细描述" :span="2">
          <div class="text-content">
            {{ state.data.description || '-' }}
          </div>
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
          @click="onPublish"
        >
          发布
        </el-button>
        <el-button 
          v-if="state.data.status === 'Published'" 
          type="warning" 
          @click="onClose"
        >
          关闭
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { reactive, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { supplyDemandApi } from '/@/api/client'
import type { SupplyDemandGetOutput } from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const state = reactive({
  visible: false,
  loading: false,
  data: {} as SupplyDemandGetOutput
})

// 打开对话框
const open = async (id: number) => {
  state.visible = true
  state.loading = true
  
  try {
    const res = await supplyDemandApi.get(id)
    if (res?.success) {
      state.data = res.data
    } else {
      proxy.$modal.msgError(res?.msg || '获取详情失败')
    }
  } catch (error) {
    console.error('获取供求信息详情失败:', error)
    proxy.$modal.msgError('获取详情失败')
  } finally {
    state.loading = false
  }
}

// 关闭对话框
const onClose = () => {
  state.visible = false
  state.data = {} as SupplyDemandGetOutput
}

// 编辑
const onEdit = () => {
  emit('edit', state.data)
  onClose()
}

// 发布
const onPublish = async () => {
  try {
    await ElMessageBox.confirm('确定要发布这条供求信息吗？发布后将公开显示。', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await supplyDemandApi.publish(state.data.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('发布成功')
      emit('refresh')
      onClose()
    } else {
      proxy.$modal.msgError(res?.msg || '发布失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('发布供求信息失败:', error)
      proxy.$modal.msgError('发布失败')
    }
  }
}

// 关闭信息
const onCloseInfo = async () => {
  try {
    await ElMessageBox.confirm('确定要关闭这条供求信息吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await supplyDemandApi.close(state.data.id)
    if (res?.success) {
      proxy.$modal.msgSuccess('关闭成功')
      emit('refresh')
      onClose()
    } else {
      proxy.$modal.msgError(res?.msg || '关闭失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('关闭供求信息失败:', error)
      proxy.$modal.msgError('关闭失败')
    }
  }
}

// 获取状态类型
const getStatusType = (status: string) => {
  const statusMap = {
    'Draft': 'info',
    'Published': 'success',
    'Closed': 'warning',
    'Deleted': 'danger'
  }
  return statusMap[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string) => {
  const statusMap = {
    'Draft': '草稿',
    'Published': '已发布',
    'Closed': '已关闭',
    'Deleted': '已删除'
  }
  return statusMap[status] || '未知'
}

// 获取信息类型样式
const getInfoTypeType = (infoType: string) => {
  const typeMap = {
    'Supply': 'success',
    'Demand': 'warning',
    'Inquiry': 'primary'
  }
  return typeMap[infoType] || 'info'
}

// 获取信息类型文本
const getInfoTypeText = (infoType: string) => {
  const typeMap = {
    'Supply': '供应',
    'Demand': '需求',
    'Inquiry': '询价'
  }
  return typeMap[infoType] || '未知'
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return '-'
  return new Date(date).toLocaleString('zh-CN')
}

// 定义事件
const emit = defineEmits<{
  refresh: []
  edit: [data: SupplyDemandGetOutput]
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
