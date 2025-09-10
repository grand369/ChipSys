<template>
  <el-dialog
    title="高级查询"
    v-model="state.showDialog"
    append-to-body
    :destroy-on-close="false"
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="660px"
    v-bind="$attrs"
  >
    <MyFilter ref="myFilterRef" v-bind="$attrs"></MyFilter>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onReset">�?�?/el-button>
        <el-button @click="onCancel">�?�?/el-button>
        <el-button type="primary" @click="onSure">�?�?/el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="my-filter-dialog">
import { ref, reactive, defineAsyncComponent, PropType } from 'vue'

defineProps({
  modelValue: Object as PropType<any | undefined | null>,
})

const MyFilter = defineAsyncComponent(() => import('./index.vue'))

const emits = defineEmits(['sure'])

const myFilterRef = ref()

const state = reactive({
  showDialog: false,
})

// 打开对话�?
const open = () => {
  state.showDialog = true
}

// 确定
const onSure = () => {
  const dynamicFilter = myFilterRef.value.getDynamicFilter()
  emits('sure', dynamicFilter)
  onCancel()
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

//重置
const onReset = () => {
  myFilterRef.value.reset()
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss"></style>
