<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="600px"
    >
      <el-form ref="formRef" :model="form" label-width="120px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="清单条目ID" prop="itemId" :rules="[{ required: true, message: '请输入清单条目ID', trigger: ['blur', 'change'] }]">
              <el-input-number v-model="form.itemId" :min="1" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="产品供应商ID" prop="productSupplierId" :rules="[{ required: true, message: '请输入产品供应商ID', trigger: ['blur', 'change'] }]">
              <el-input-number v-model="form.productSupplierId" :min="1" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="置信度" prop="confidence">
              <el-input-number v-model="form.confidence" :min="0" :max="1" :precision="2" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="创建时间" prop="createdAt">
              <el-date-picker v-model="form.createdAt" type="datetime" placeholder="选择日期时间" class="w100" />
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

<script lang="ts" setup name="admin/chipmgr/hit-list/form">
import { HitListAddInput, HitListUpdateInput } from '/@/api/admin/data-contracts'
import { HitListApi } from '/@/api/admin/HitList'
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
  form: {} as HitListAddInput & HitListUpdateInput,
  contiAdd: false,
})

const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  proxy.$modal.loading()
  
  if (row.id > 0) {
    state.contiAdd = false
    const res = await new HitListApi().get({ id: row.id }).catch(() => {})

    if (res?.success) {
      state.form = res.data as HitListAddInput & HitListUpdateInput
    }
  } else {
    state.form = { 
      createdAt: new Date(),
      userId: 1 // 这里应该从用户信息中获取
    } as HitListAddInput & HitListUpdateInput
  }

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
      res = await new HitListApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new HitListApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      if (state.contiAdd) {
        formRef.value!.resetFields()
      }
      eventBus.emit('refreshHitList')
      state.showDialog = state.contiAdd
    }
  })
}

defineExpose({
  open,
})
</script>
