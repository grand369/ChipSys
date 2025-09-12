<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="900px"
    >
      <el-form ref="formRef" :model="form" label-width="100px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="产品" prop="productId" :rules="[{ required: true, message: '请选择产品', trigger: ['blur', 'change'] }]">
              <el-select v-model="form.productId" placeholder="请选择产品" class="w100">
                <el-option
                  v-for="item in state.productOptions"
                  :key="item.id"
                  :label="`${item.code} - ${item.brand}`"
                  :value="item.id"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="供应商" prop="supplierId" :rules="[{ required: true, message: '请选择供应商', trigger: ['blur', 'change'] }]">
              <el-select v-model="form.supplierId" placeholder="请选择供应商" class="w100">
                <el-option
                  v-for="item in state.supplierOptions"
                  :key="item.id"
                  :label="item.name"
                  :value="item.id"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="之前价格" prop="previousPrice">
              <el-input-number v-model="form.previousPrice" :precision="2" :min="0" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="当前价格" prop="currentPrice" :rules="[{ required: true, message: '请输入当前价格', trigger: ['blur', 'change'] }]">
              <el-input-number v-model="form.currentPrice" :precision="2" :min="0" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="货币" prop="currency">
              <el-select v-model="form.currency" placeholder="请选择货币" class="w100">
                <el-option label="CNY" value="CNY" />
                <el-option label="USD" value="USD" />
                <el-option label="EUR" value="EUR" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="成色" prop="condition">
              <el-input v-model="form.condition" placeholder="如：全新、95新、翻新等" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="供应商型号" prop="supplierModel">
              <el-input v-model="form.supplierModel" placeholder="供应商提供的产品型号" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="最小起订量" prop="moq">
              <el-input-number v-model="form.moq" :min="0" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="交期天数" prop="leadTimeDays">
              <el-input-number v-model="form.leadTimeDays" :min="0" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="库存数量" prop="stockQty">
              <el-input-number v-model="form.stockQty" :min="0" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="有效开始" prop="validFrom">
              <el-date-picker v-model="form.validFrom" type="datetime" placeholder="选择日期时间" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="有效截止" prop="validTo">
              <el-date-picker v-model="form.validTo" type="datetime" placeholder="选择日期时间" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="使用描述" prop="usageDescription">
              <el-input v-model="form.usageDescription" type="textarea" placeholder="描述产品的使用情况" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer my-flex my-flex-y-center my-flex-between">
          <div>
            <el-checkbox v-if="!(state.form?.id > 0)" v-model="state.contiAdd">连续新增</el-checkbox>
          </div>
          <div>
            <el-button @click="onCancel">取 消</el-button>
            <el-button type="primary" @click="onSure" :loading="state.sureLoading">确 定</el-button>
          </div>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/product-supplier/form">
import { ProductSupplierAddInput, ProductSupplierUpdateInput, ProductGetListOutput, SupplierGetListOutput } from '/@/api/admin/data-contracts'
import { ProductSupplierApi } from '/@/api/admin/ProductSupplier'
import { ProductApi } from '/@/api/admin/Product'
import { SupplierApi } from '/@/api/admin/Supplier'
import eventBus from '/@/utils/mitt'
import { FormInstance } from 'element-plus'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = useTemplateRef<FormInstance>('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as ProductSupplierAddInput & ProductSupplierUpdateInput,
  contiAdd: false,
  productOptions: [] as Array<ProductGetListOutput>,
  supplierOptions: [] as Array<SupplierGetListOutput>,
})

const { form } = toRefs(state)

const loadProductOptions = async () => {
  const res = await new ProductApi().getList({}).catch(() => {})
  if (res && res.data) {
    state.productOptions = res.data
  }
}

const loadSupplierOptions = async () => {
  const res = await new SupplierApi().getList({}).catch(() => {})
  if (res && res.data) {
    state.supplierOptions = res.data
  }
}

// 打开对话框
const open = async (row: any = {}) => {
  proxy.$modal.loading()
  
  if (row.id > 0) {
    state.contiAdd = false
    const res = await new ProductSupplierApi().get({ id: row.id }).catch(() => {})

    if (res?.success) {
      state.form = res.data as ProductSupplierAddInput & ProductSupplierUpdateInput
    }
  } else {
    state.form = { currency: 'CNY', moq: 0, stockQty: 0 } as ProductSupplierAddInput & ProductSupplierUpdateInput
  }

  await loadProductOptions()
  await loadSupplierOptions()

  proxy.$modal.closeLoading()

  state.showDialog = true
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value!.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new ProductSupplierApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new ProductSupplierApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      if (state.contiAdd) {
        formRef.value!.resetFields()
      }
      eventBus.emit('refreshProductSupplier')
      state.showDialog = state.contiAdd
    }
  })
}

defineExpose({
  open,
})
</script>
