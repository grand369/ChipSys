<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="420px"
    >
      <el-text class="my-flex my-flex-items-center mb20">
        <SvgIcon name="ele-WarningFilled" size="24" color="#e6a23c" class="mr5" />
        确定要给【{{ state.name }}】重置密�?
      </el-text>

      <el-form ref="formRef" :model="state.form" label-width="0px">
        <el-form-item prop="password">
          <el-input
            key="password"
            placeholder="选填，不填则使用系统默认密码"
            v-model="state.form.password"
            @input="onInputPwd"
            show-password
            autocomplete="off"
          />
        </el-form-item>
      </el-form>

      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel">�?�?/el-button>
          <el-button type="primary" @click="onSure" :loading="state.sureLoading">�?�?/el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/user/reset-pwd">
import { verifyCnAndSpace } from '/@/utils/toolsValidate'
import { UserApi } from '/@/api/admin/User'
import { UserGetPageOutput, UserResetPasswordInput } from '/@/api/admin/data-contracts'
import { FormInstance } from 'element-plus'
import eventBus from '/@/utils/mitt'

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
  name: '',
  form: {
    id: 0,
    password: '',
  } as UserResetPasswordInput,
})

// 输入密码
const onInputPwd = (val: string) => {
  state.form.password = verifyCnAndSpace(val)
}

// 打开对话�?
const open = async (row: UserGetPageOutput) => {
  state.form.password = ''
  state.showDialog = true
  if (row.name) state.name = row.name
  state.form.id = row.id
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = async () => {
  formRef.value!.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true

    const res = await new UserApi().resetPassword(state.form, { loading: true }).catch(() => {
      state.sureLoading = false
    })
    if (res?.success) {
      proxy.$modal.msgSuccess(`重置密码成功，密码为�?{res.data}`)
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshUser')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
