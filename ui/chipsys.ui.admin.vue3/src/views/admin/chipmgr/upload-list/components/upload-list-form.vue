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
      <el-form ref="formRef" :model="form" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="文件名" prop="fileName">
              <el-input v-model="form.fileName" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="状态" prop="status">
              <el-select v-model="form.status" placeholder="请选择状态" class="w100">
                <el-option label="待处理" value="Pending" />
                <el-option label="处理中" value="Processing" />
                <el-option label="已完成" value="Completed" />
                <el-option label="失败" value="Failed" />
              </el-select>
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

<script lang="ts" setup name="admin/chipmgr/upload-list/form">
import { UploadListAddInput, UploadListUpdateInput } from '/@/api/admin/data-contracts'
import { UploadListApi } from '/@/api/admin/UploadList'
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
  form: {} as UploadListAddInput & UploadListUpdateInput,
  contiAdd: false,
})

const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  proxy.$modal.loading()
  
  if (row.id > 0) {
    state.contiAdd = false
    const res = await new UploadListApi().get({ id: row.id }).catch(() => {})

    if (res?.success) {
      state.form = res.data as UploadListAddInput & UploadListUpdateInput
    }
  } else {
    state.form = { 
      status: 'Pending',
      createdAt: new Date(),
      userId: 1 // 这里应该从用户信息中获取
    } as UploadListAddInput & UploadListUpdateInput
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
      res = await new UploadListApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new UploadListApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      if (state.contiAdd) {
        formRef.value!.resetFields()
      }
      eventBus.emit('refreshUploadList')
      state.showDialog = state.contiAdd
    }
  })
}

defineExpose({
  open,
})
</script>
