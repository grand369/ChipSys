<template>
  <el-dialog
    v-model="state.visible"
    :title="state.form.id > 0 ? '编辑询价' : '新增询价'"
    width="800px"
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
      <el-form-item label="询价标题" prop="title">
        <el-input
          v-model="state.form.title"
          placeholder="请输入询价标题"
          maxlength="100"
          show-word-limit
        />
      </el-form-item>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="产品名称" prop="productName">
            <el-input
              v-model="state.form.productName"
              placeholder="请输入产品名称"
              maxlength="100"
              show-word-limit
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="产品型号" prop="productModel">
            <el-input
              v-model="state.form.productModel"
              placeholder="请输入产品型号"
              maxlength="50"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="产品品牌" prop="productBrand">
            <el-input
              v-model="state.form.productBrand"
              placeholder="请输入产品品牌"
              maxlength="50"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="产品制造商" prop="productManufacturer">
            <el-input
              v-model="state.form.productManufacturer"
              placeholder="请输入产品制造商"
              maxlength="100"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="需求数量" prop="quantity">
            <el-input-number
              v-model="state.form.quantity"
              placeholder="请输入需求数量"
              :min="1"
              :max="999999"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="预算范围" prop="budgetRange">
            <el-input
              v-model="state.form.budgetRange"
              placeholder="请输入预算范围，如：1000-5000元"
              maxlength="100"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="期望交期" prop="expectedDeliveryDays">
            <el-input-number
              v-model="state.form.expectedDeliveryDays"
              placeholder="请输入期望交期（天）"
              :min="1"
              :max="365"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="询价有效期" prop="validDays">
            <el-input-number
              v-model="state.form.validDays"
              placeholder="请输入询价有效期（天）"
              :min="1"
              :max="90"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item label="技术参数要求" prop="technicalRequirements">
        <el-input
          v-model="state.form.technicalRequirements"
          type="textarea"
          placeholder="请输入技术参数要求"
          :rows="3"
          maxlength="1000"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="质量要求" prop="qualityRequirements">
        <el-input
          v-model="state.form.qualityRequirements"
          type="textarea"
          placeholder="请输入质量要求"
          :rows="2"
          maxlength="500"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="其他要求" prop="otherRequirements">
        <el-input
          v-model="state.form.otherRequirements"
          type="textarea"
          placeholder="请输入其他要求"
          :rows="2"
          maxlength="500"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="联系信息" prop="contactInfo">
        <el-input
          v-model="state.form.contactInfo"
          type="textarea"
          placeholder="请输入联系信息"
          :rows="2"
          maxlength="500"
          show-word-limit
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
import { inquiryApi } from '/@/api/client'
import type { 
  InquiryAddInput, 
  InquiryUpdateInput,
  InquiryGetOutput 
} from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 表单引用
const formRef = ref()

// 响应式数据
const state = reactive({
  visible: false,
  sureLoading: false,
  contiAdd: false,
  form: {} as InquiryAddInput & InquiryUpdateInput,
  rules: {
    title: [
      { required: true, message: '请输入询价标题', trigger: 'blur' },
      { min: 2, max: 100, message: '询价标题长度在 2 到 100 个字符', trigger: 'blur' }
    ],
    productName: [
      { required: true, message: '请输入产品名称', trigger: 'blur' },
      { min: 2, max: 100, message: '产品名称长度在 2 到 100 个字符', trigger: 'blur' }
    ],
    productModel: [
      { max: 50, message: '产品型号长度不能超过 50 个字符', trigger: 'blur' }
    ],
    productBrand: [
      { max: 50, message: '产品品牌长度不能超过 50 个字符', trigger: 'blur' }
    ],
    productManufacturer: [
      { max: 100, message: '产品制造商长度不能超过 100 个字符', trigger: 'blur' }
    ],
    quantity: [
      { required: true, type: 'number', min: 1, message: '需求数量必须大于0', trigger: 'blur' }
    ],
    budgetRange: [
      { max: 100, message: '预算范围长度不能超过 100 个字符', trigger: 'blur' }
    ],
    expectedDeliveryDays: [
      { required: true, type: 'number', min: 1, max: 365, message: '期望交期必须在1-365天之间', trigger: 'blur' }
    ],
    validDays: [
      { required: true, type: 'number', min: 1, max: 90, message: '询价有效期必须在1-90天之间', trigger: 'blur' }
    ],
    technicalRequirements: [
      { max: 1000, message: '技术参数要求长度不能超过 1000 个字符', trigger: 'blur' }
    ],
    qualityRequirements: [
      { max: 500, message: '质量要求长度不能超过 500 个字符', trigger: 'blur' }
    ],
    otherRequirements: [
      { max: 500, message: '其他要求长度不能超过 500 个字符', trigger: 'blur' }
    ],
    contactInfo: [
      { required: true, message: '请输入联系信息', trigger: 'blur' },
      { max: 500, message: '联系信息长度不能超过 500 个字符', trigger: 'blur' }
    ]
  }
})

// 打开对话框
const open = (row?: InquiryGetOutput) => {
  state.visible = true
  state.contiAdd = false
  
  if (row) {
    // 编辑模式
    state.form = {
      id: row.id,
      title: row.title,
      productName: row.productName,
      productModel: row.productModel || '',
      productBrand: row.productBrand || '',
      productManufacturer: row.productManufacturer || '',
      quantity: row.quantity,
      budgetRange: row.budgetRange || '',
      expectedDeliveryDays: row.expectedDeliveryDays,
      validDays: row.validDays,
      technicalRequirements: row.technicalRequirements || '',
      qualityRequirements: row.qualityRequirements || '',
      otherRequirements: row.otherRequirements || '',
      contactInfo: row.contactInfo
    }
  } else {
    // 新增模式
    state.form = {
      id: 0,
      title: '',
      productName: '',
      productModel: '',
      productBrand: '',
      productManufacturer: '',
      quantity: 1,
      budgetRange: '',
      expectedDeliveryDays: 30,
      validDays: 30,
      technicalRequirements: '',
      qualityRequirements: '',
      otherRequirements: '',
      contactInfo: ''
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
  state.form = {} as InquiryAddInput & InquiryUpdateInput
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
      res = await inquiryApi.update({
        id: state.form.id,
        title: state.form.title,
        productName: state.form.productName,
        productModel: state.form.productModel,
        productBrand: state.form.productBrand,
        productManufacturer: state.form.productManufacturer,
        quantity: state.form.quantity,
        budgetRange: state.form.budgetRange,
        expectedDeliveryDays: state.form.expectedDeliveryDays,
        validDays: state.form.validDays,
        technicalRequirements: state.form.technicalRequirements,
        qualityRequirements: state.form.qualityRequirements,
        otherRequirements: state.form.otherRequirements,
        contactInfo: state.form.contactInfo
      } as InquiryUpdateInput)
    } else {
      // 新增
      res = await inquiryApi.add({
        title: state.form.title,
        productName: state.form.productName,
        productModel: state.form.productModel,
        productBrand: state.form.productBrand,
        productManufacturer: state.form.productManufacturer,
        quantity: state.form.quantity,
        budgetRange: state.form.budgetRange,
        expectedDeliveryDays: state.form.expectedDeliveryDays,
        validDays: state.form.validDays,
        technicalRequirements: state.form.technicalRequirements,
        qualityRequirements: state.form.qualityRequirements,
        otherRequirements: state.form.otherRequirements,
        contactInfo: state.form.contactInfo
      } as InquiryAddInput)
    }
    
    if (res?.success) {
      proxy.$modal.msgSuccess(state.form.id > 0 ? '更新成功' : '新增成功')
      
      if (state.contiAdd && !(state.form.id > 0)) {
        // 连续新增
        state.form = {
          id: 0,
          title: '',
          productName: '',
          productModel: '',
          productBrand: '',
          productManufacturer: '',
          quantity: 1,
          budgetRange: '',
          expectedDeliveryDays: 30,
          validDays: 30,
          technicalRequirements: '',
          qualityRequirements: '',
          otherRequirements: '',
          contactInfo: ''
        }
        formRef.value?.resetFields()
      } else {
        close()
        // 触发父组件刷新
        emit('refresh')
      }
    }
  } catch (error) {
    console.error('保存询价失败:', error)
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
</style>
