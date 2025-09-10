<template>
  <SlideCaptcha
    ref="slideCaptchaRef"
    :fail-tip="state.failTip"
    :success-tip="state.successTip"
    width="100%"
    height="auto"
    @refresh="onGenerate"
    @finish="onFinish"
    v-bind="$attrs"
  />
</template>

<script lang="ts" setup name="my-captcha">
import { defineAsyncComponent, ref, reactive } from 'vue'
import { CaptchaApi } from '/@/api/admin/Captcha'

const SlideCaptcha = defineAsyncComponent(() => import('./slide-captcha.vue'))

const slideCaptchaRef = ref()
const emits = defineEmits(['ok'])

const state = reactive({
  requestId: '',
  failTip: '',
  successTip: '',
})

//生成滑块验证�?
const onGenerate = async () => {
  slideCaptchaRef.value.startRequestGenerate()
  const res = await new CaptchaApi().generate({ captchaId: state.requestId }).catch(() => {
    slideCaptchaRef.value.endRequestGenerate(null, null)
  })
  if (res?.success && res.data) {
    state.requestId = res.data.id || ''
    slideCaptchaRef.value.endRequestGenerate(res.data.backgroundImage, res.data.sliderImage)
  }
}

//验证滑块验证�?
const onFinish = async (data: any) => {
  slideCaptchaRef.value.startRequestVerify()
  const res = await new CaptchaApi().check(data, { captchaId: state.requestId }).catch(() => {
    state.failTip = '服务异常，请稍后重试'
    slideCaptchaRef.value.endRequestVerify(false)
  })
  if (res?.success && res.data) {
    let success = res.data.result === 0
    state.failTip = res.data.result == 1 ? '验证未通过，拖动滑块将悬浮图像正确合并' : '验证超时, 请重新操�?
    state.successTip = '验证通过'
    slideCaptchaRef.value.endRequestVerify(success)
    if (success) {
      //验证成功
      emits('ok', { captchaId: state.requestId, track: data })
    } else {
      setTimeout(() => {
        onGenerate()
      }, 1000)
    }
  }
}

//刷新滑块验证�?
const refresh = () => {
  slideCaptchaRef.value?.handleRefresh()
}

defineExpose({
  refresh,
})
</script>

<style scoped lang="scss"></style>
