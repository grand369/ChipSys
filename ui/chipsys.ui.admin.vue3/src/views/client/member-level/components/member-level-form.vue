<template>
  <el-dialog
    v-model="state.visible"
    :title="state.form.id > 0 ? '编辑会员等级' : '新增会员等级'"
    width="600px"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onCancel"
  >
    <el-form
      ref="formRef"
      :model="state.form"
      :rules="state.rules"
      label-width="120px"
      label-position="right"
    >
      <el-form-item label="会员等级" prop="level">
        <el-select
          v-model="state.form.level"
          placeholder="请选择会员等级"
          style="width: 100%"
          @change="onLevelChange"
        >
          <el-option label="免费会员" value="Free" />
          <el-option label="基础版" value="Basic" />
          <el-option label="标准版" value="Standard" />
          <el-option label="高级版" value="Premium" />
          <el-option label="企业版" value="Enterprise" />
        </el-select>
      </el-form-item>

      <el-form-item label="分类限制" prop="categoryLimit">
        <el-input-number
          v-model="state.form.categoryLimit"
          placeholder="请输入分类限制"
          :min="0"
          :max="999999"
          style="width: 100%"
        />
        <div class="form-tip">0表示无限制</div>
      </el-form-item>

      <el-form-item label="产品数据限制" prop="productDataLimit">
        <el-input-number
          v-model="state.form.productDataLimit"
          placeholder="请输入产品数据限制"
          :min="1"
          :max="999999"
          style="width: 100%"
        />
        <div class="form-tip">可查看的产品数据条数</div>
      </el-form-item>

      <el-form-item label="供应商数据限制" prop="supplierDataLimit">
        <el-input-number
          v-model="state.form.supplierDataLimit"
          placeholder="请输入供应商数据限制"
          :min="1"
          :max="999999"
          style="width: 100%"
        />
        <div class="form-tip">可查看的供应商数据条数</div>
      </el-form-item>

      <el-form-item label="显示完整联系信息" prop="showFullContactInfo">
        <el-switch
          v-model="state.form.showFullContactInfo"
          active-text="是"
          inactive-text="否"
        />
        <div class="form-tip">是否显示供应商的完整联系信息</div>
      </el-form-item>

      <el-form-item label="生效时间" prop="effectiveTime">
        <el-date-picker
          v-model="state.form.effectiveTime"
          type="datetime"
          placeholder="请选择生效时间"
          style="width: 100%"
          format="YYYY-MM-DD HH:mm:ss"
          value-format="YYYY-MM-DD HH:mm:ss"
        />
      </el-form-item>

      <el-form-item label="过期时间" prop="expireTime">
        <el-date-picker
          v-model="state.form.expireTime"
          type="datetime"
          placeholder="请选择过期时间"
          style="width: 100%"
          format="YYYY-MM-DD HH:mm:ss"
          value-format="YYYY-MM-DD HH:mm:ss"
        />
      </el-form-item>
    </el-form>

    <template #footer>
      <div class="dialog-footer">
        <el-button @click="onCancel">取消</el-button>
        <el-button type="primary" :loading="state.sureLoading" @click="onSure">
          {{ state.form.id > 0 ? '更新' : '保存' }}
        </el-button>
        <el-button
          v-if="!(state.form.id > 0)"
          type="success"
          :loading="state.sureLoading"
          @click="onSureAndContinue"
        >
          保存并继续新增
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { reactive, ref, getCurrentInstance } from 'vue'
import { ElMessage } from 'element-plus'
import { memberLevelApi } from '/@/api/client'
import type { 
  MemberLevelAddInput, 
  MemberLevelUpdateInput,
  MemberLevelGetOutput 
} from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 表单引用
const formRef = ref()

// 响应式数据
const state = reactive({
  visible: false,
  sureLoading: false,
  contiAdd: false,
  form: {} as MemberLevelAddInput & MemberLevelUpdateInput,
  rules: {
    level: [
      { required: true, message: '请选择会员等级', trigger: 'change' }
    ],
    categoryLimit: [
      { required: true, type: 'number', min: 0, message: '分类限制必须大于等于0', trigger: 'blur' }
    ],
    productDataLimit: [
      { required: true, type: 'number', min: 1, message: '产品数据限制必须大于0', trigger: 'blur' }
    ],
    supplierDataLimit: [
      { required: true, type: 'number', min: 1, message: '供应商数据限制必须大于0', trigger: 'blur' }
    ],
    effectiveTime: [
      { required: true, message: '请选择生效时间', trigger: 'change' }
    ],
    expireTime: [
      { required: true, message: '请选择过期时间', trigger: 'change' }
    ]
  }
})

// 等级配置
const levelConfigs = {
  'Free': { categoryLimit: 0, productDataLimit: 50, supplierDataLimit: 50, showFullContactInfo: false },
  'Basic': { categoryLimit: 2, productDataLimit: 100, supplierDataLimit: 100, showFullContactInfo: true },
  'Standard': { categoryLimit: 5, productDataLimit: 500, supplierDataLimit: 500, showFullContactInfo: true },
  'Premium': { categoryLimit: 10, productDataLimit: 1000, supplierDataLimit: 1000, showFullContactInfo: true },
  'Enterprise': { categoryLimit: 999999, productDataLimit: 999999, supplierDataLimit: 999999, showFullContactInfo: true }
}

// 打开对话框
const open = (row?: MemberLevelGetOutput) => {
  state.visible = true
  state.contiAdd = false
  
  if (row) {
    // 编辑模式
    state.form = {
      id: row.id,
      memberId: row.memberId,
      level: row.level,
      categoryLimit: row.categoryLimit,
      productDataLimit: row.productDataLimit,
      supplierDataLimit: row.supplierDataLimit,
      showFullContactInfo: row.showFullContactInfo,
      enabled: row.enabled,
      effectiveTime: row.effectiveTime || '',
      expireTime: row.expireTime || ''
    }
  } else {
    // 新增模式
    state.form = {
      id: 0,
      memberId: undefined, // 不指定会员ID，让后端自动使用当前用户ID
      level: 'Free',
      categoryLimit: 0,
      productDataLimit: 50,
      supplierDataLimit: 50,
      showFullContactInfo: false,
      enabled: true,
      effectiveTime: '',
      expireTime: ''
    }
  }
  
  // 重置表单验证
  setTimeout(() => {
    formRef.value?.clearValidate()
  }, 100)
}

// 关闭对话框
const close = () => {
  state.visible = false
  state.form = {} as MemberLevelAddInput & MemberLevelUpdateInput
  formRef.value?.resetFields()
}

// 取消
const onCancel = () => {
  close()
}

// 确定
const onSure = async () => {
  state.contiAdd = false
  await saveData()
}

// 保存并继续新增
const onSureAndContinue = async () => {
  state.contiAdd = true
  await saveData()
}

// 等级改变时自动填充配置
const onLevelChange = (level: string) => {
  const config = levelConfigs[level]
  if (config) {
    state.form.categoryLimit = config.categoryLimit
    state.form.productDataLimit = config.productDataLimit
    state.form.supplierDataLimit = config.supplierDataLimit
    state.form.showFullContactInfo = config.showFullContactInfo
  }
}

// 保存数据
const saveData = async () => {
  if (!formRef.value) return
  
  const valid = await formRef.value.validate().catch(() => false)
  if (!valid) return
  
  state.sureLoading = true
  
  try {
    let res: any
    
    if (state.form.id > 0) {
      // 更新
      res = await memberLevelApi.update({
        id: state.form.id,
        memberId: state.form.memberId,
        level: state.form.level,
        categoryLimit: state.form.categoryLimit,
        productDataLimit: state.form.productDataLimit,
        supplierDataLimit: state.form.supplierDataLimit,
        showFullContactInfo: state.form.showFullContactInfo,
        enabled: state.form.enabled,
        effectiveTime: state.form.effectiveTime,
        expireTime: state.form.expireTime
      } as MemberLevelUpdateInput)
    } else {
      // 新增
      res = await memberLevelApi.add({
        memberId: state.form.memberId,
        level: state.form.level,
        categoryLimit: state.form.categoryLimit,
        productDataLimit: state.form.productDataLimit,
        supplierDataLimit: state.form.supplierDataLimit,
        showFullContactInfo: state.form.showFullContactInfo,
        enabled: state.form.enabled,
        effectiveTime: state.form.effectiveTime,
        expireTime: state.form.expireTime
      } as MemberLevelAddInput)
    }
    
    if (res?.success) {
      proxy.$modal.msgSuccess(state.form.id > 0 ? '更新成功' : '新增成功')
      
      if (state.contiAdd && !(state.form.id > 0)) {
        // 连续新增
        state.form = {
          id: 0,
          memberId: 0,
          level: 'Free',
          categoryLimit: 0,
          productDataLimit: 50,
          supplierDataLimit: 50,
          showFullContactInfo: false,
          enabled: true,
          effectiveTime: '',
          expireTime: ''
        }
        formRef.value?.resetFields()
      } else {
        close()
        // 触发父组件刷新
        emit('refresh')
      }
    }
  } catch (error) {
    console.error('保存会员等级失败:', error)
  } finally {
    state.sureLoading = false
  }
}

// 定义事件
const emit = defineEmits<{
  refresh: []
}>()

// 暴露方法
defineExpose({
  open,
  close,
})
</script>

<style scoped>
.dialog-footer {
  text-align: right;
}

.el-form-item {
  margin-bottom: 20px;
}

.form-tip {
  font-size: 12px;
  color: #909399;
  margin-top: 4px;
}
</style>
