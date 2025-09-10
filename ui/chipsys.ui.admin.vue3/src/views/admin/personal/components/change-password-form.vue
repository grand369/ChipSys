<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="475px"
    >
      <el-form ref="formRef" :model="form" label-width="80px" label-position="left">
        <el-row :gutter="35">
          <el-col :span="24">
            <el-form-item label="旧密�? prop="oldPassword" :rules="[{ required: true, message: '请输入旧密码', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.oldPassword" show-password autocomplete="off" clearable />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item
              label="新密�?
              prop="newPassword"
              :rules="[
                { required: true, message: '请输入新密码', trigger: ['blur', 'change'] },
                { validator: testNewPassword, trigger: ['blur', 'change'] },
                { validator: validatorPwd, trigger: ['blur', 'change'] },
              ]"
            >
              <el-input v-model="form.newPassword" show-password autocomplete="off" clearable @input="onInputNewPassword" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item
              label="确认密码"
              prop="confirmPassword"
              :rules="[
                { required: true, message: '请输入确认密�?, trigger: ['blur', 'change'] },
                { validator: testConfirmPassword, trigger: ['blur', 'change'] },
              ]"
            >
              <el-input v-model="form.confirmPassword" show-password autocomplete="off" clearable @input="onInputConfirmPassword" />
            </el-form-item>
          </el-col>
        </el-row>
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

<script lang="ts" setup name="admin/personal/change-password-form">
import { UserChangePasswordInput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import { verifyCnAndSpace } from '/@/utils/toolsValidate'
import { validatorPwd } from '/@/utils/validators'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const formRef = useTemplateRef('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as UserChangePasswordInput,
})
const { form } = toRefs(state)

// 新密码验证器
const testNewPassword = (rule: any, value: any, callback: any) => {
  if (value) {
    if (state.form.confirmPassword) {
      formRef.value.validateField('confirmPassword')
    }
    callback()
  }
}

// 确认密码验证�?
const testConfirmPassword = (rule: any, value: any, callback: any) => {
  if (value) {
    if (value !== state.form.newPassword) {
      callback(new Error('确认密码和新密码不一�?))
    } else {
      callback()
    }
  }
}

// 输入新密�?
const onInputNewPassword = (val: string) => {
  state.form.newPassword = verifyCnAndSpace(val)
}

// 输入确认密码
const onInputConfirmPassword = (val: string) => {
  state.form.confirmPassword = verifyCnAndSpace(val)
}

// 打开对话�?
const open = async () => {
  state.showDialog = true
  state.form = {} as UserChangePasswordInput
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    const res = await new UserApi().changePassword(state.form, { showSuccessMessage: true }).catch(() => {
      state.sureLoading = false
    })
    state.sureLoading = false

    if (res?.success) {
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
