<template>
  <div class="w100">
    <el-input text :maxlength="props.maxlength" placeholder="请输入验证码" autocomplete="off" v-bind="$attrs">
      <template #prefix>
        <el-icon class="el-input__icon"><ele-Message /></el-icon>
      </template>
      <template #suffix>
        <el-button
          v-show="state.status !== 'countdown'"
          :loading="state.loading.getCode"
          type="primary"
          link
          :disabled="state.status === 'countdown'"
          @click.prevent.stop="onGetCode"
          >{{ text }}</el-button
        >
        <el-countdown
          v-show="state.status === 'countdown'"
          :format="state.changeText"
          :value="state.countdown"
          value-style="font-size:var(--el-font-size-base);color:var(--el-color-primary)"
          @change="onChange"
        />
      </template>
    </el-input>
    <MyCaptchaDialog ref="myCaptchaDialogRef" v-model="state.showDialog" @ok="onOk" />
  </div>
</template>

<script lang="ts" setup name="my-input-code">
import { reactive, defineAsyncComponent, ref, computed } from 'vue'
import { isMobile } from '/@/utils/test'
import { verifyEmail } from '/@/utils/toolsValidate'
import { ElMessage } from 'element-plus'
import { CaptchaApi } from '/@/api/admin/Captcha'

const MyCaptchaDialog = defineAsyncComponent(() => import('/@/components/my-captcha/dialog.vue'))

const emits = defineEmits(['send'])

const props = defineProps({
  maxlength: {
    type: Number,
    default: 6,
  },
  seconds: {
    type: Number,
    default: 60,
  },
  startText: {
    type: String,
    default: '获取验证�?,
  },
  changeText: {
    type: String,
    default: 's秒后重发',
  },
  endText: {
    type: String,
    default: '重新发送验证码',
  },
  mobile: {
    type: String,
    default: '',
  },
  email: {
    type: String,
    default: '',
  },
  validate: {
    type: Function,
    default: null,
  },
})

const myCaptchaDialogRef = ref()
const countdown = Date.now()

const state = reactive({
  status: 'ready',
  startText: props.startText,
  changeText: props.changeText,
  endText: props.endText,
  countdown: countdown,

  showDialog: false,
  codeId: '',
  loading: {
    getCode: false,
  },
})

//获取验证码文�?
const text = computed(() => {
  return state.status === 'ready' ? state.startText : state.endText
})

//开始倒计�?
const startCountdown = () => {
  state.status = 'countdown'
  state.countdown = Date.now() + (props.seconds + 1) * 1000
}

//点击获取验证�?
const onGetCode = () => {
  if (state.status !== 'countdown') {
    if (props.validate) {
      props.validate(getCode)
    } else {
      getCode()
    }
  }
}

//监听倒计�?
const onChange = (value: number) => {
  if (state.countdown != countdown && value < 1000) state.status = 'finish'
}

//验证通过
const onOk = async (data: any) => {
  state.showDialog = false

  //发送短信验证码
  state.loading.getCode = true
  const api = props.mobile
    ? new CaptchaApi().sendSmsCode({
        mobile: props.mobile,
        captchaId: data.captchaId,
        track: data.track,
        codeId: state.codeId,
      })
    : new CaptchaApi().sendEmailCode({
        email: props.email,
        captchaId: data.captchaId,
        track: data.track,
        codeId: state.codeId,
      })

  const res = await api
    .catch(() => {})
    .finally(() => {
      state.loading.getCode = false
    })

  if (res?.success && res.data) {
    state.codeId = res.data
    emits('send', res.data)
    startCountdown()
  }
}

//获得验证�?
const getCode = () => {
  if (props.mobile) {
    //验证手机�?
    if (!isMobile(props.mobile)) {
      ElMessage.warning({ message: '请输入正确的手机号码', grouping: true })
      return
    }
  } else {
    //验证邮箱
    if (!verifyEmail(props.email)) {
      ElMessage.warning({ message: '请输入正确的邮件地址', grouping: true })
      return
    }
  }

  state.showDialog = true
  //刷新滑块拼图
  myCaptchaDialogRef.value?.refresh()
}
</script>

<style scoped lang="scss">
:deep(.el-statistic__content) {
  font-size: var(--el-font-size-base);
}
</style>
<style lang="scss">
.my-captcha .el-dialog__body {
  padding-top: 10px;
}
.my-captcha .captcha__bar {
  border-color: var(--el-border-color) !important;
}
</style>
