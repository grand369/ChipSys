<template>
  <el-dialog
    v-model="state.visible"
    title="会员等级详情"
    width="600px"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onClose"
  >
    <div v-loading="state.loading" class="detail-content">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="等级ID">
          {{ state.data.id }}
        </el-descriptions-item>
        <el-descriptions-item label="会员ID">
          {{ state.data.memberId }}
        </el-descriptions-item>
        <el-descriptions-item label="会员等级">
          <el-tag :type="getLevelType(state.data.level)">
            {{ getLevelText(state.data.level) }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="状态">
          <el-tag :type="state.data.enabled ? 'success' : 'danger'">
            {{ state.data.enabled ? '启用' : '禁用' }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="分类限制">
          {{ state.data.categoryLimit === 999999 ? '无限制' : `${state.data.categoryLimit} 个` }}
        </el-descriptions-item>
        <el-descriptions-item label="产品数据限制">
          {{ state.data.productDataLimit === 999999 ? '无限制' : `${state.data.productDataLimit} 条` }}
        </el-descriptions-item>
        <el-descriptions-item label="供应商数据限制">
          {{ state.data.supplierDataLimit === 999999 ? '无限制' : `${state.data.supplierDataLimit} 条` }}
        </el-descriptions-item>
        <el-descriptions-item label="显示完整联系信息">
          <el-tag :type="state.data.showFullContactInfo ? 'success' : 'info'">
            {{ state.data.showFullContactInfo ? '是' : '否' }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="生效时间">
          {{ formatDate(state.data.effectiveTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="过期时间">
          {{ formatDate(state.data.expireTime) }}
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
          v-if="state.data.enabled" 
          type="primary" 
          @click="onEdit"
        >
          编辑
        </el-button>
        <el-button 
          v-if="state.data.enabled" 
          type="warning" 
          @click="onDisable"
        >
          禁用
        </el-button>
        <el-button 
          v-if="!state.data.enabled" 
          type="success" 
          @click="onEnable"
        >
          启用
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { reactive, getCurrentInstance } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { memberLevelApi } from '/@/api/client'
import type { MemberLevelGetOutput } from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 响应式数据
const state = reactive({
  visible: false,
  loading: false,
  data: {} as MemberLevelGetOutput
})

// 打开对话框
const open = async (id: number) => {
  state.visible = true
  state.loading = true
  
  try {
    const res = await memberLevelApi.get(id)
    if (res?.success) {
      state.data = res.data
    } else {
      proxy.$modal.msgError(res?.msg || '获取详情失败')
    }
  } catch (error) {
    console.error('获取会员等级详情失败:', error)
    proxy.$modal.msgError('获取详情失败')
  } finally {
    state.loading = false
  }
}

// 关闭对话框
const onClose = () => {
  state.visible = false
  state.data = {} as MemberLevelGetOutput
}

// 编辑
const onEdit = () => {
  emit('edit', state.data)
  onClose()
}

// 启用
const onEnable = async () => {
  try {
    await ElMessageBox.confirm('确定要启用这个会员等级吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await memberLevelApi.update({
      id: state.data.id,
      memberId: state.data.memberId,
      level: state.data.level,
      categoryLimit: state.data.categoryLimit,
      productDataLimit: state.data.productDataLimit,
      supplierDataLimit: state.data.supplierDataLimit,
      showFullContactInfo: state.data.showFullContactInfo,
      enabled: true,
      effectiveTime: state.data.effectiveTime,
      expireTime: state.data.expireTime
    })
    
    if (res?.success) {
      proxy.$modal.msgSuccess('启用成功')
      emit('refresh')
      onClose()
    } else {
      proxy.$modal.msgError(res?.msg || '启用失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('启用会员等级失败:', error)
      proxy.$modal.msgError('启用失败')
    }
  }
}

// 禁用
const onDisable = async () => {
  try {
    await ElMessageBox.confirm('确定要禁用这个会员等级吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    
    const res = await memberLevelApi.update({
      id: state.data.id,
      memberId: state.data.memberId,
      level: state.data.level,
      categoryLimit: state.data.categoryLimit,
      productDataLimit: state.data.productDataLimit,
      supplierDataLimit: state.data.supplierDataLimit,
      showFullContactInfo: state.data.showFullContactInfo,
      enabled: false,
      effectiveTime: state.data.effectiveTime,
      expireTime: state.data.expireTime
    })
    
    if (res?.success) {
      proxy.$modal.msgSuccess('禁用成功')
      emit('refresh')
      onClose()
    } else {
      proxy.$modal.msgError(res?.msg || '禁用失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('禁用会员等级失败:', error)
      proxy.$modal.msgError('禁用失败')
    }
  }
}

// 获取等级类型
const getLevelType = (level: string) => {
  const typeMap = {
    'Free': 'info',
    'Basic': 'success',
    'Standard': 'primary',
    'Premium': 'warning',
    'Enterprise': 'danger'
  }
  return typeMap[level] || 'info'
}

// 获取等级文本
const getLevelText = (level: string) => {
  const textMap = {
    'Free': '免费会员',
    'Basic': '基础版',
    'Standard': '标准版',
    'Premium': '高级版',
    'Enterprise': '企业版'
  }
  return textMap[level] || '未知'
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return '-'
  return new Date(date).toLocaleString('zh-CN')
}

// 定义事件
const emit = defineEmits<{
  refresh: []
  edit: [data: MemberLevelGetOutput]
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

.dialog-footer {
  text-align: right;
}

.el-descriptions {
  margin-top: 20px;
}
</style>
