<template>
  <el-drawer v-model="state.showDialog" direction="rtl" destroy-on-close :size="size">
    <template #header="{ titleId, titleClass }">
      <h4 :id="titleId" :class="titleClass">{{ title }}</h4>
      <el-icon v-if="state.isFull" class="el-drawer__btn" @click="state.isFull = !state.isFull" title="还原"><ele-CopyDocument /></el-icon>
      <el-icon v-else class="el-drawer__btn" @click="state.isFull = !state.isFull" title="最大化"><ele-FullScreen /></el-icon>
    </template>
    <div class="my-fill h100">
      <MyJsonEditor
        v-model="state.templateJson"
        :options="{
          mainMenuBar: false,
          statusBar: false,
          onEditable: () => false,
        }"
      ></MyJsonEditor>
    </div>
  </el-drawer>
</template>

<script lang="ts" setup>
import MyJsonEditor from '/@/components/my-json-editor/index.vue'

defineProps({
  title: {
    type: String,
    default: '查看JSON',
  },
})

const state = reactive({
  showDialog: false,
  isFull: false,
  isMobile: document.body.clientWidth < 1000,
  templateJson: '',
})

const size = computed(() => {
  return state.isMobile ? '100%' : state.isFull ? '100%' : '45%'
})

// 打开对话�?
const open = (templateJson: any) => {
  state.templateJson = templateJson
  state.showDialog = true
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.el-alert {
  border-width: 0px !important;
  margin-left: 110px;
  margin-top: 10px;
}
.el-drawer__btn {
  cursor: pointer;
  margin-right: 8px;
  &:hover {
    color: var(--el-color-primary);
  }
}
</style>
