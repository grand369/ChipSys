<template>
  <el-dialog class="my-captcha" title="请完成安全验�? draggable append-to-body width="380px" v-bind="$attrs">
    <MyCaptcha ref="myCaptchaRef" v-bind="$attrs" />
  </el-dialog>
</template>

<script lang="ts" setup name="my-captcha-dialog">
import { defineAsyncComponent, ref } from 'vue'

const MyCaptcha = defineAsyncComponent(() => import('./index.vue'))

const myCaptchaRef = ref()

//刷新滑块验证�?
const refresh = () => {
  myCaptchaRef.value?.refresh()
}

defineExpose({
  refresh,
})
</script>

<style lang="scss">
.my-captcha {
  .el-dialog__body {
    padding: 0px !important;
  }
}
</style>
