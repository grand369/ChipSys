<template>
  <el-dialog
    v-model="state.visible"
    :title="state.form.id > 0 ? '编辑报价' : '新增报价'"
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
      <el-form-item label="询价信息" v-if="state.inquiryInfo">
        <div class="inquiry-info">
          <p><strong>询价标题：</strong>{{ state.inquiryInfo.title }}</p>
          <p><strong>产品名称：</strong>{{ state.inquiryInfo.productName }}</p>
          <p><strong>需求数量：</strong>{{ state.inquiryInfo.quantity }} 件</p>
          <p><strong>预算范围：</strong>{{ state.inquiryInfo.budgetRange || '未指定' }}</p>
        </div>
      </el-form-item>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="供应商名称" prop="supplierName">
            <el-input
              v-model="state.form.supplierName"
              placeholder="请输入供应商名称"
              maxlength="100"
              show-word-limit
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="供应商ID" prop="supplierId">
            <el-input-number
              v-model="state.form.supplierId"
              placeholder="请输入供应商ID"
              :min="1"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="报价金额" prop="quotePrice">
            <el-input-number
              v-model="state.form.quotePrice"
              placeholder="请输入报价金额"
              :min="0.01"
              :precision="2"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="货币" prop="currency">
            <el-select
              v-model="state.form.currency"
              placeholder="请选择货币"
              style="width: 100%"
            >
              <el-option label="人民币 (CNY)" value="CNY" />
              <el-option label="美元 (USD)" value="USD" />
              <el-option label="欧元 (EUR)" value="EUR" />
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="交期天数" prop="deliveryDays">
            <el-input-number
              v-model="state.form.deliveryDays"
              placeholder="请输入交期天数"
              :min="1"
              :max="365"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="最小起订量" prop="minOrderQuantity">
            <el-input-number
              v-model="state.form.minOrderQuantity"
              placeholder="请输入最小起订量"
              :min="1"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item label="报价有效期" prop="validDays">
        <el-input-number
          v-model="state.form.validDays"
          placeholder="请输入报价有效期（天）"
          :min="1"
          :max="90"
          style="width: 200px"
        />
        <div class="form-tip">报价的有效期，超过此天数后报价失效</div>
      </el-form-item>

      <el-form-item label="产品规格说明" prop="productSpecification">
        <el-input
          v-model="state.form.productSpecification"
          type="textarea"
          placeholder="请输入产品规格说明"
          :rows="3"
          maxlength="1000"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="质量保证" prop="qualityAssurance">
        <el-input
          v-model="state.form.qualityAssurance"
          type="textarea"
          placeholder="请输入质量保证说明"
          :rows="2"
          maxlength="500"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="报价备注" prop="quoteRemark">
        <el-input
          v-model="state.form.quoteRemark"
          type="textarea"
          placeholder="请输入报价备注"
          :rows="3"
          maxlength="1000"
          show-word-limit
        />
      </el-form-item>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="联系人姓名" prop="contactName">
            <el-input
              v-model="state.form.contactName"
              placeholder="请输入联系人姓名"
              maxlength="50"
              show-word-limit
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="联系电话" prop="contactPhone">
            <el-input
              v-model="state.form.contactPhone"
              placeholder="请输入联系电话"
              maxlength="20"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item label="联系邮箱" prop="contactEmail">
        <el-input
          v-model="state.form.contactEmail"
          placeholder="请输入联系邮箱"
          maxlength="100"
        />
      </el-form-item>
    </el-form>

    <template #footer>
      <div class="dialog-footer">
        <el-button @click="onCancel">取消</el-button>
        <el-button type="primary" :loading="state.sureLoading" @click="onSure">
          {{ state.form.id > 0 ? '更新' : '提交报价' }}
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { reactive, ref, getCurrentInstance } from 'vue'
import { ElMessage } from 'element-plus'
import { quoteApi } from '/@/api/client'
import type { 
  QuoteAddInput, 
  QuoteUpdateInput,
  QuoteGetOutput,
  InquiryGetOutput
} from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 表单引用
const formRef = ref()

// 响应式数据
const state = reactive({
  visible: false,
  sureLoading: false,
  inquiryInfo: null as InquiryGetOutput | null,
  form: {} as QuoteAddInput & QuoteUpdateInput,
  rules: {
    supplierName: [
      { required: true, message: '请输入供应商名称', trigger: 'blur' },
      { min: 2, max: 100, message: '供应商名称长度在 2 到 100 个字符', trigger: 'blur' }
    ],
    supplierId: [
      { required: true, type: 'number', min: 1, message: '请输入供应商ID', trigger: 'blur' }
    ],
    quotePrice: [
      { required: true, type: 'number', min: 0.01, message: '报价必须大于0', trigger: 'blur' }
    ],
    currency: [
      { required: true, message: '请选择货币', trigger: 'change' }
    ],
    deliveryDays: [
      { required: true, type: 'number', min: 1, max: 365, message: '交期天数必须在1-365天之间', trigger: 'blur' }
    ],
    minOrderQuantity: [
      { required: true, type: 'number', min: 1, message: '最小起订量必须大于0', trigger: 'blur' }
    ],
    validDays: [
      { required: true, type: 'number', min: 1, max: 90, message: '有效期天数必须在1-90天之间', trigger: 'blur' }
    ],
    contactName: [
      { required: true, message: '请输入联系人姓名', trigger: 'blur' },
      { min: 2, max: 50, message: '联系人姓名长度在 2 到 50 个字符', trigger: 'blur' }
    ],
    contactPhone: [
      { required: true, message: '请输入联系电话', trigger: 'blur' },
      { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码', trigger: 'blur' }
    ],
    contactEmail: [
      { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur' }
    ],
    productSpecification: [
      { max: 1000, message: '产品规格说明长度不能超过 1000 个字符', trigger: 'blur' }
    ],
    qualityAssurance: [
      { max: 500, message: '质量保证说明长度不能超过 500 个字符', trigger: 'blur' }
    ],
    quoteRemark: [
      { max: 1000, message: '报价备注长度不能超过 1000 个字符', trigger: 'blur' }
    ]
  }
})

// 打开对话框
const open = (inquiryId?: number, row?: QuoteGetOutput) => {
  state.visible = true
  
  if (row) {
    // 编辑模式
    state.form = {
      id: row.id,
      inquiryId: row.inquiryId,
      supplierId: row.supplierId,
      supplierName: row.supplierName,
      quotePrice: row.quotePrice,
      currency: row.currency,
      deliveryDays: row.deliveryDays,
      minOrderQuantity: row.minOrderQuantity,
      validDays: row.validDays,
      productSpecification: row.productSpecification || '',
      qualityAssurance: row.qualityAssurance || '',
      quoteRemark: row.quoteRemark || '',
      contactName: row.contactName,
      contactPhone: row.contactPhone,
      contactEmail: row.contactEmail || ''
    }
    state.inquiryInfo = null
  } else if (inquiryId) {
    // 新增模式 - 针对特定询价
    state.form = {
      id: 0,
      inquiryId: inquiryId,
      supplierId: 0,
      supplierName: '',
      quotePrice: 0,
      currency: 'CNY',
      deliveryDays: 30,
      minOrderQuantity: 1,
      validDays: 30,
      productSpecification: '',
      qualityAssurance: '',
      quoteRemark: '',
      contactName: '',
      contactPhone: '',
      contactEmail: ''
    }
    // 获取询价信息
    getInquiryInfo(inquiryId)
  } else {
    // 新增模式 - 通用
    state.form = {
      id: 0,
      inquiryId: 0,
      supplierId: 0,
      supplierName: '',
      quotePrice: 0,
      currency: 'CNY',
      deliveryDays: 30,
      minOrderQuantity: 1,
      validDays: 30,
      productSpecification: '',
      qualityAssurance: '',
      quoteRemark: '',
      contactName: '',
      contactPhone: '',
      contactEmail: ''
    }
    state.inquiryInfo = null
  }
  
  // 重置表单验证
  setTimeout(() => {
    formRef.value?.clearValidate()
  }, 100)
}

// 获取询价信息
const getInquiryInfo = async (inquiryId: number) => {
  try {
    const res = await quoteApi.getInquiryInfo(inquiryId)
    if (res?.success) {
      state.inquiryInfo = res.data
    }
  } catch (error) {
    console.error('获取询价信息失败:', error)
  }
}

// 关闭对话框
const close = () => {
  state.visible = false
  state.form = {} as QuoteAddInput & QuoteUpdateInput
  state.inquiryInfo = null
  formRef.value?.resetFields()
}

// 取消
const onCancel = () => {
  close()
}

// 确定
const onSure = async () => {
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
      res = await quoteApi.update({
        id: state.form.id,
        inquiryId: state.form.inquiryId,
        supplierId: state.form.supplierId,
        supplierName: state.form.supplierName,
        quotePrice: state.form.quotePrice,
        currency: state.form.currency,
        deliveryDays: state.form.deliveryDays,
        minOrderQuantity: state.form.minOrderQuantity,
        validDays: state.form.validDays,
        productSpecification: state.form.productSpecification,
        qualityAssurance: state.form.qualityAssurance,
        quoteRemark: state.form.quoteRemark,
        contactName: state.form.contactName,
        contactPhone: state.form.contactPhone,
        contactEmail: state.form.contactEmail,
        status: 'Pending'
      } as QuoteUpdateInput)
    } else {
      // 新增
      res = await quoteApi.add({
        inquiryId: state.form.inquiryId,
        supplierId: state.form.supplierId,
        supplierName: state.form.supplierName,
        quotePrice: state.form.quotePrice,
        currency: state.form.currency,
        deliveryDays: state.form.deliveryDays,
        minOrderQuantity: state.form.minOrderQuantity,
        validDays: state.form.validDays,
        productSpecification: state.form.productSpecification,
        qualityAssurance: state.form.qualityAssurance,
        quoteRemark: state.form.quoteRemark,
        contactName: state.form.contactName,
        contactPhone: state.form.contactPhone,
        contactEmail: state.form.contactEmail
      } as QuoteAddInput)
    }
    
    if (res?.success) {
      proxy.$modal.msgSuccess(state.form.id > 0 ? '更新成功' : '报价提交成功')
      close()
      // 触发父组件刷新
      emit('refresh')
    }
  } catch (error) {
    console.error('保存报价失败:', error)
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

.inquiry-info {
  background-color: #f5f7fa;
  padding: 12px;
  border-radius: 4px;
  border-left: 4px solid #409eff;
}

.inquiry-info p {
  margin: 4px 0;
  font-size: 14px;
  color: #606266;
}
</style>
