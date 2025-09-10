<template>
  <div>
    <el-form ref="formRef" :model="form" size="large" class="login-content-form">
      <div class="login-title">更改密码</div>
      <el-form-item
        class="login-animation1"
        prop="email"
        :rules="[
          { required: true, message: '请输入邮箱地址', trigger: ['blur', 'change'] },
          { validator: testEmail, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input ref="emailRef" text :placeholder="$t('message.email.placeholder1')" v-model="form.email" clearable autocomplete="off">
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Promotion /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation2" prop="code" :rules="[{ required: true, message: '请输入邮箱验证码', trigger: ['blur', 'change'] }]">
        <MyInputCode v-model="form.code" :email="form.email" :validate="validateEmail" @send="onSend" />
      </el-form-item>
      <el-form-item
        class="login-animation3"
        prop="newPassword"
        :rules="[
          { required: true, message: '请输入新密码', trigger: ['blur', 'change'] },
          { validator: testNewPassword, trigger: ['blur', 'change'] },
          { validator: validatorPwd, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input v-model="form.newPassword" :placeholder="'输入新密�?" show-password autocomplete="off" clearable @input="onInputNewPassword">
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Unlock /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item
        v-if="hasConfirmPassword"
        class="login-animation4"
        prop="confirmPassword"
        :rules="[
          { required: true, message: '请输入确认密�?, trigger: ['blur', 'change'] },
          { validator: testConfirmPassword, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input
          v-model="form.confirmPassword"
          :placeholder="'输入确认密码'"
          show-password
          autocomplete="off"
          clearable
          @input="onInputConfirmPassword"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Unlock /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation6 mb12">
        <el-button round type="primary" v-waves class="login-content-submit" :loading="state.loading" @click="onChangePwd">
          <span>{{ $t('更改密码') }}</span>
        </el-button>
      </el-form-item>
      <div class="login-animation6 my-flex my-flex-center f12 mt10">
        <span class="login-remind">想起密码?</span>
        <el-link underline="never" type="primary" class="f12" @click="isChangePassword = false">去登�?/el-link>
      </div>
    </el-form>
  </div>
</template>

<script lang="ts" setup name="admin/change-password-email">
import { AuthChangePasswordByEmailInput } from '/@/api/admin/data-contracts'
import { AuthApi } from '/@/api/admin/Auth'
import { verifyCnAndSpace } from '/@/utils/toolsValidate'
import { validatorPwd } from '/@/utils/validators'
import { testEmail } from '/@/utils/test'
import { ElMessage } from 'element-plus'

const MyInputCode = defineAsyncComponent(() => import('/@/components/my-input-code/index.vue'))
const isChangePassword = defineModel('isChangePassword', { type: Boolean, default: false })
const hasConfirmPassword = defineModel('hasConfirmPassword', { type: Boolean, default: false })

const formRef = useTemplateRef('formRef')
const emailRef = useTemplateRef('emailRef')

const state = reactive({
  showDialog: false,
  loading: false,
  form: {
    email: '',
    code: '',
    codeId: '',
    newPassword: '',
    confirmPassword: '',
  } as AuthChangePasswordByEmailInput,
})
const { form } = toRefs(state)

//验证邮箱
const validateEmail = (callback: Function) => {
  formRef.value.validateField('email', (isValid: boolean) => {
    if (!isValid) {
      emailRef.value?.focus()
      return
    }
    callback?.()
  })
}

//发送验证码
const onSend = (codeId: string) => {
  state.form.codeId = codeId
}

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
  state.form = {} as AuthChangePasswordByEmailInput
}

// 更改密码
const onChangePwd = () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.loading = true
    const res = await new AuthApi().changePasswordByEmail(state.form, { showSuccessMessage: false }).catch(() => {
      state.loading = false
    })
    state.loading = false

    if (res?.success) {
      ElMessage.success('更新成功')
      isChangePassword.value = false
    }
  })
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.login-content-form {
  .login-title {
    margin-bottom: 50px;
    font-size: 27px;
    text-align: center;
    letter-spacing: 3px;
    color: var(--el-text-color-primary);
    position: relative;
  }
  .login-remind {
    color: #7f8792;
    margin-right: 5px;
  }
  @for $i from 1 through 6 {
    .login-animation#{$i} {
      opacity: 0;
      animation-name: error-num;
      animation-duration: 0.5s;
      animation-fill-mode: forwards;
      animation-delay: calc($i/10) + s;
    }
  }
  .login-content-code {
    width: 100%;
    padding: 0;
  }
  .login-content-submit {
    width: 100%;
    letter-spacing: 2px;
    font-weight: 300;
    margin-top: 15px;
  }
  .login-msg {
    color: var(--el-text-color-placeholder);
  }
  .f12 {
    font-size: 12px;
  }
}
</style>
