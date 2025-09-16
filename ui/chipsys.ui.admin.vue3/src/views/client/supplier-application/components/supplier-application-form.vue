<template>
  <el-dialog
    v-model="state.visible"
    :title="state.form.id > 0 ? '编辑供应商申请' : '新增供应商申请'"
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
      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="公司名称" prop="companyName">
            <el-input
              v-model="state.form.companyName"
              placeholder="请输入公司名称"
              maxlength="100"
              show-word-limit
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="公司地址" prop="companyAddress">
            <el-input
              v-model="state.form.companyAddress"
              placeholder="请输入公司地址"
              maxlength="200"
              show-word-limit
            />
          </el-form-item>
        </el-col>
      </el-row>

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

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="联系邮箱" prop="contactEmail">
            <el-input
              v-model="state.form.contactEmail"
              placeholder="请输入联系邮箱"
              maxlength="100"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="营业执照号" prop="businessLicense">
            <el-input
              v-model="state.form.businessLicense"
              placeholder="请输入营业执照号"
              maxlength="50"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item label="主要产品类别" prop="mainProductCategories">
        <el-input
          v-model="state.form.mainProductCategories"
          type="textarea"
          placeholder="请输入主要产品类别，多个类别用逗号分隔"
          :rows="3"
          maxlength="500"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="公司简介" prop="companyDescription">
        <el-input
          v-model="state.form.companyDescription"
          type="textarea"
          placeholder="请输入公司简介"
          :rows="4"
          maxlength="1000"
          show-word-limit
        />
      </el-form-item>

      <el-form-item label="申请理由" prop="applicationReason">
        <el-input
          v-model="state.form.applicationReason"
          type="textarea"
          placeholder="请输入申请理由"
          :rows="4"
          maxlength="1000"
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
import { supplierApplicationApi } from '/@/api/client'
import type { 
  SupplierApplicationAddInput, 
  SupplierApplicationUpdateInput,
  SupplierApplicationGetOutput 
} from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 表单引用
const formRef = ref()

// 响应式数据
const state = reactive({
  visible: false,
  sureLoading: false,
  contiAdd: false,
  form: {} as SupplierApplicationAddInput & SupplierApplicationUpdateInput,
  rules: {
    companyName: [
      { required: true, message: '请输入公司名称', trigger: 'blur' },
      { min: 2, max: 100, message: '公司名称长度在 2 到 100 个字符', trigger: 'blur' }
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
    companyAddress: [
      { max: 200, message: '公司地址长度不能超过 200 个字符', trigger: 'blur' }
    ],
    businessLicense: [
      { max: 50, message: '营业执照号长度不能超过 50 个字符', trigger: 'blur' }
    ],
    mainProductCategories: [
      { max: 500, message: '主要产品类别长度不能超过 500 个字符', trigger: 'blur' }
    ],
    companyDescription: [
      { max: 1000, message: '公司简介长度不能超过 1000 个字符', trigger: 'blur' }
    ],
    applicationReason: [
      { max: 1000, message: '申请理由长度不能超过 1000 个字符', trigger: 'blur' }
    ]
  }
})

// 打开对话框
const open = (row?: SupplierApplicationGetOutput) => {
  state.visible = true
  state.contiAdd = false
  
  if (row) {
    // 编辑模式
    state.form = {
      id: row.id,
      companyName: row.companyName,
      companyAddress: row.companyAddress || '',
      contactName: row.contactName,
      contactPhone: row.contactPhone,
      contactEmail: row.contactEmail || '',
      businessLicense: row.businessLicense || '',
      mainProductCategories: row.mainProductCategories || '',
      companyDescription: row.companyDescription || '',
      applicationReason: row.applicationReason || ''
    }
  } else {
    // 新增模式
    state.form = {
      id: 0,
      companyName: '',
      companyAddress: '',
      contactName: '',
      contactPhone: '',
      contactEmail: '',
      businessLicense: '',
      mainProductCategories: '',
      companyDescription: '',
      applicationReason: ''
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
  state.form = {} as SupplierApplicationAddInput & SupplierApplicationUpdateInput
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
      res = await supplierApplicationApi.update({
        id: state.form.id,
        companyName: state.form.companyName,
        companyAddress: state.form.companyAddress,
        contactName: state.form.contactName,
        contactPhone: state.form.contactPhone,
        contactEmail: state.form.contactEmail,
        businessLicense: state.form.businessLicense,
        mainProductCategories: state.form.mainProductCategories,
        companyDescription: state.form.companyDescription,
        applicationReason: state.form.applicationReason
      } as SupplierApplicationUpdateInput)
    } else {
      // 新增
      res = await supplierApplicationApi.add({
        companyName: state.form.companyName,
        companyAddress: state.form.companyAddress,
        contactName: state.form.contactName,
        contactPhone: state.form.contactPhone,
        contactEmail: state.form.contactEmail,
        businessLicense: state.form.businessLicense,
        mainProductCategories: state.form.mainProductCategories,
        companyDescription: state.form.companyDescription,
        applicationReason: state.form.applicationReason
      } as SupplierApplicationAddInput)
    }
    
    if (res?.success) {
      proxy.$modal.msgSuccess(state.form.id > 0 ? '更新成功' : '新增成功')
      
      if (state.contiAdd && !(state.form.id > 0)) {
        // 连续新增
        state.form = {
          id: 0,
          companyName: '',
          companyAddress: '',
          contactName: '',
          contactPhone: '',
          contactEmail: '',
          businessLicense: '',
          mainProductCategories: '',
          companyDescription: '',
          applicationReason: ''
        }
        formRef.value?.resetFields()
      } else {
        close()
        // 触发父组件刷新
        emit('refresh')
      }
    }
  } catch (error) {
    console.error('保存供应商申请失败:', error)
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
