<template>
  <el-dialog
    v-model="state.visible"
    :title="state.form.id > 0 ? '编辑供求信息' : '新增供求信息'"
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
          <el-form-item label="信息类型" prop="infoType">
            <el-select
              v-model="state.form.infoType"
              placeholder="请选择信息类型"
              style="width: 100%"
            >
              <el-option label="供应" value="Supply" />
              <el-option label="需求" value="Demand" />
              <el-option label="询价" value="Inquiry" />
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="产品名称" prop="productName">
            <el-select
              v-model="state.form.productName"
              placeholder="请选择产品"
              style="width: 100%"
              filterable
              remote
              :remote-method="searchProducts"
              :loading="state.productLoading"
              @change="onProductChange"
            >
              <el-option
                v-for="product in state.productOptions"
                :key="product.id"
                :label="product.name"
                :value="product.name"
                :data-product="product"
              >
                <span style="float: left">{{ product.name }}</span>
                <span style="float: right; color: #8492a6; font-size: 13px">{{ product.category.name }}</span>
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="产品型号" prop="productModel">
            <el-input
              v-model="state.form.productModel"
              placeholder="请输入产品型号"
              maxlength="50"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="产品品牌" prop="productBrand">
            <el-input
              v-model="state.form.productBrand"
              placeholder="请输入产品品牌"
              maxlength="50"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="产品描述" prop="productDescription">
            <el-input
              v-model="state.form.productDescription"
              placeholder="请输入产品描述"
              maxlength="500"
              show-word-limit
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="产品参数" prop="productSpecification">
            <el-input
              v-model="state.form.productSpecification"
              placeholder="请输入产品参数"
              maxlength="1000"
              show-word-limit
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="产品制造商" prop="productManufacturer">
            <el-input
              v-model="state.form.productManufacturer"
              placeholder="请输入产品制造商"
              maxlength="100"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="数量要求" prop="quantity">
            <el-input-number
              v-model="state.form.quantity"
              placeholder="请输入数量要求"
              :min="1"
              :max="999999"
              style="width: 100%"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="价格范围" prop="priceRange">
            <el-input
              v-model="state.form.priceRange"
              placeholder="请输入价格范围，如：100-200元"
              maxlength="100"
            />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="供货周期" prop="deliveryDays">
            <el-input-number
              v-model="state.form.deliveryDays"
              placeholder="请输入供货周期（天）"
              :min="1"
              :max="365"
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

      <el-form-item label="详细描述" prop="description">
        <el-input
          v-model="state.form.description"
          type="textarea"
          placeholder="请输入详细描述"
          :rows="4"
          maxlength="2000"
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
import { supplyDemandApi, publicQueryApi } from '/@/api/client'
import type { 
  SupplyDemandAddInput, 
  SupplyDemandUpdateInput,
  SupplyDemandGetOutput,
  PublicProductOutput
} from '/@/api/client/data-contracts'

const { proxy } = getCurrentInstance() as any

// 表单引用
const formRef = ref()

// 响应式数据
const state = reactive({
  visible: false,
  sureLoading: false,
  contiAdd: false,
  productLoading: false,
  productOptions: [] as PublicProductOutput[],
  selectedProduct: null as PublicProductOutput | null,
  form: {} as SupplyDemandAddInput & SupplyDemandUpdateInput,
  rules: {
    infoType: [
      { required: true, message: '请选择信息类型', trigger: 'change' }
    ],
    productName: [
      { required: true, message: '请选择产品', trigger: 'change' }
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
      { type: 'number', min: 1, message: '数量必须大于0', trigger: 'blur' }
    ],
    priceRange: [
      { max: 100, message: '价格范围长度不能超过 100 个字符', trigger: 'blur' }
    ],
    deliveryDays: [
      { type: 'number', min: 1, max: 365, message: '供货周期必须在1-365天之间', trigger: 'blur' }
    ],
    technicalRequirements: [
      { max: 1000, message: '技术参数要求长度不能超过 1000 个字符', trigger: 'blur' }
    ],
    contactInfo: [
      { max: 500, message: '联系信息长度不能超过 500 个字符', trigger: 'blur' }
    ],
    description: [
      { max: 2000, message: '详细描述长度不能超过 2000 个字符', trigger: 'blur' }
    ]
  }
})

// 打开对话框
const open = (row?: SupplyDemandGetOutput) => {
  state.visible = true
  state.contiAdd = false
  
  if (row) {
    // 编辑模式
    state.form = {
      id: row.id,
      infoType: row.infoType,
      productName: row.productName,
      productModel: row.productModel || '',
      productBrand: row.productBrand || '',
      productManufacturer: row.productManufacturer || '',
      productDescription: row.productDescription || '',
      productSpecification: row.productSpecification || '',
      technicalRequirements: row.technicalRequirements || '',
      quantity: row.quantity || undefined,
      priceRange: row.priceRange || '',
      deliveryDays: row.deliveryDays || undefined,
      contactInfo: row.contactInfo || '',
      description: row.description || ''
    }
  } else {
    // 新增模式
    state.form = {
      id: 0,
      infoType: '',
      productName: '',
      productModel: '',
      productBrand: '',
      productManufacturer: '',
      productDescription: '',
      productSpecification: '',
      technicalRequirements: '',
      quantity: undefined,
      priceRange: '',
      deliveryDays: undefined,
      contactInfo: '',
      description: ''
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
  state.form = {} as SupplyDemandAddInput & SupplyDemandUpdateInput
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
      res = await supplyDemandApi.update({
        id: state.form.id,
        infoType: state.form.infoType,
        productName: state.form.productName,
        productModel: state.form.productModel,
        productBrand: state.form.productBrand,
        productManufacturer: state.form.productManufacturer,
        technicalRequirements: state.form.technicalRequirements,
        quantity: state.form.quantity,
        priceRange: state.form.priceRange,
        deliveryDays: state.form.deliveryDays,
        contactInfo: state.form.contactInfo,
        description: state.form.description
      } as SupplyDemandUpdateInput)
    } else {
      // 新增
      res = await supplyDemandApi.add({
        infoType: state.form.infoType,
        productName: state.form.productName,
        productModel: state.form.productModel,
        productBrand: state.form.productBrand,
        productManufacturer: state.form.productManufacturer,
        technicalRequirements: state.form.technicalRequirements,
        quantity: state.form.quantity,
        priceRange: state.form.priceRange,
        deliveryDays: state.form.deliveryDays,
        contactInfo: state.form.contactInfo,
        description: state.form.description
      } as SupplyDemandAddInput)
    }
    
    if (res?.success) {
      proxy.$modal.msgSuccess(state.form.id > 0 ? '更新成功' : '新增成功')
      
      if (state.contiAdd && !(state.form.id > 0)) {
        // 连续新增
        state.form = {
          id: 0,
          infoType: '',
          productName: '',
          productModel: '',
          productBrand: '',
          productManufacturer: '',
          productDescription: '',
          productSpecification: '',
          technicalRequirements: '',
          quantity: undefined,
          priceRange: '',
          deliveryDays: undefined,
          contactInfo: '',
          description: ''
        }
        formRef.value?.resetFields()
      } else {
        close()
        // 触发父组件刷新
        emit('refresh')
      }
    }
  } catch (error) {
    console.error('保存供求信息失败:', error)
  } finally {
    state.sureLoading = false
  }
}

// 定义事件
const emit = defineEmits<{
  refresh: []
}>()

// 搜索产品
const searchProducts = async (query: string) => {
  if (!query) {
    state.productOptions = []
    return
  }
  
  try {
    state.productLoading = true
    const result = await publicQueryApi.getPublicProducts({
      currentPage: 1,
      pageSize: 20,
      filter: {
        name: query
      }
    })
    
    if (result?.success && result.data?.list) {
      state.productOptions = result.data.list
    }
  } catch (error) {
    console.error('搜索产品失败:', error)
    ElMessage.error('搜索产品失败')
  } finally {
    state.productLoading = false
  }
}

// 产品选择变化
const onProductChange = (productName: string) => {
  const selectedProduct = state.productOptions.find(p => p.name === productName)
  if (selectedProduct) {
    state.selectedProduct = selectedProduct
    // 自动填充产品信息
    state.form.productModel = selectedProduct.specification || ''
    state.form.productDescription = selectedProduct.description || ''
    state.form.productSpecification = selectedProduct.specification || ''
    // 注意：品牌和制造商信息在产品数据中可能没有，需要根据实际情况调整
  } else {
    state.selectedProduct = null
  }
}

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
