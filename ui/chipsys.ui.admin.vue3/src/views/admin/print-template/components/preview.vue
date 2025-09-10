<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="state.title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="true"
      :width="width"
      :show-close="false"
      style="max-width: 90%"
      body-class="preview-box"
    >
      <template #header="{ titleId, titleClass }">
        <div class="my-flex my-flex-between my-flex-items-center mr10">
          <span :id="titleId" :class="titleClass">{{ state.title }}</span>
          <div>
            <el-button type="primary" @click="onExport">
              <template #icon>
                <el-icon>
                  <my-icon name="export" color="var(--color)"></my-icon>
                </el-icon>
              </template>
              导出PDF
            </el-button>
            <el-button type="primary" icon="ele-Printer" @click="onPrint">�?�?/el-button>
            <el-button @click="onCancel">�?�?/el-button>
          </div>
        </div>
      </template>

      <div ref="previewContainerRef"></div>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { hiprint } from 'vue-plugin-hiprint'

const state = reactive({
  showDialog: false,
  width: 0,
  title: '',
  printData: {},
  template: {} as any,
})

const width = computed(() => {
  return `${state.width + 20}mm`
})

const hiprintTemplate = ref()
const previewContainerRef = useTemplateRef<HTMLElement | null>('previewContainerRef') // 引用容器

// 打开对话�?
const open = async (template: any, printData: {}, title = '打印模板') => {
  state.title = title
  state.template = template
  state.printData = printData
  hiprintTemplate.value = new hiprint.PrintTemplate({
    template: state.template,
  })
  state.width = hiprintTemplate.value.printPanels?.length > 0 ? hiprintTemplate.value.printPanels[0].width : 210
  const htmlElements = hiprintTemplate.value.getHtml(printData)

  state.showDialog = true

  await nextTick()

  // 清空容器
  if (previewContainerRef.value) {
    previewContainerRef.value.innerHTML = ''
  }

  // �?HTML 元素对象插入到容器中
  if (htmlElements?.length > 0 && previewContainerRef.value) {
    htmlElements.each((index: number, element: HTMLElement) => {
      previewContainerRef.value?.appendChild(element)
    })
  }
}

// 导出PDF
const onExport = () => {
  hiprintTemplate.value.toPdf(state.printData, state.title)
}

//打印
const onPrint = async () => {
  if (hiprintTemplate.value) {
    hiprintTemplate.value.print(state.printData)
  }
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
:deep() {
  .el-dialog__body {
    overflow-x: auto;
  }
  .preview-box {
    background-color: var(--el-bg-color-overlay);
  }
  .hiprint-printPaper {
    background-color: var(--el-color-white);
  }
}
</style>
