<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="769px"
    >
      <el-form ref="formRef" :model="form" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
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
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="姓名" prop="name" :rules="[{ required: true, message: '请输入联系人姓名', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="电话" prop="phone">
              <el-input v-model="form.phone" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="邮箱" prop="email">
              <el-input v-model="form.email" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="职位" prop="position">
              <el-input v-model="form.position" autocomplete="off" />
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

<script lang="ts" setup name="admin/chipmgr/contact/form">
import { ContactAddInput, ContactUpdateInput, SupplierGetListOutput } from '/@/api/admin/data-contracts'
import { ContactApi } from '/@/api/admin/Contact'
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
  form: {} as ContactAddInput & ContactUpdateInput,
  contiAdd: false,
  supplierOptions: [] as Array<SupplierGetListOutput>,
})

const { form } = toRefs(state)

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
    const res = await new ContactApi().get({ id: row.id }).catch(() => {})

    if (res?.success) {
      state.form = res.data as ContactAddInput & ContactUpdateInput
    }
  } else {
    state.form = {} as ContactAddInput & ContactUpdateInput
  }

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
      res = await new ContactApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new ContactApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      if (state.contiAdd) {
        formRef.value!.resetFields()
      }
      eventBus.emit('refreshContact')
      state.showDialog = state.contiAdd
    }
  })
}

defineExpose({
  open,
})
</script>
