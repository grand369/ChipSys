<template>
  <slot v-if="getUserAuthBtnList" />
</template>

<script setup lang="ts" name="auths">
import { computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useUserInfo } from '/@/stores/userInfo'

// 定义父组件传过来的�?
const props = defineProps({
  value: {
    type: Array,
    default: () => [],
  },
})

// 定义变量内容
const stores = useUserInfo()
const { userInfos } = storeToRefs(stores)

// 获取 pinia 中的用户权限
const getUserAuthBtnList = computed(() => {
  let flag = false
  userInfos.value.authBtnList.map((val: string) => {
    props.value.map((v) => {
      if (val === v) flag = true
    })
  })
  return flag
})
</script>
