<template>
  <div class="pt15 pr15 pb15 pl15">
    <el-form :model="state.line" label-width="50px">
      <el-form-item label="来往">
        <el-input v-model="state.line.contact" placeholder="来往" clearable disabled></el-input>
      </el-form-item>
      <el-form-item label="类型">
        <el-input v-model="state.line.type" placeholder="类型" clearable disabled></el-input>
      </el-form-item>
      <el-form-item label="label">
        <el-input v-model="state.line.label" placeholder="请输入label内容" clearable></el-input>
      </el-form-item>
      <el-form-item>
        <el-button @click="onLineTextReset">
          <SvgIcon name="ele-RefreshRight" />
          重置
        </el-button>
        <el-button @click="onLineTextChange" type="primary">
          <SvgIcon name="ele-Check" />
          保存
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup lang="ts" name="example/pagesWorkflowDrawerLine">
import { reactive } from 'vue'

// 定义子组件向父组件传�?事件
const emit = defineEmits(['change', 'close'])

// 定义变量内容
const state = reactive<EmptyObjectType>({
  line: {},
})

// 获取父组件数�?
const getParentData = (data: object) => {
  state.line = data
}
// 重置
const onLineTextReset = () => {
  state.line.label = ''
}
// 保存
const onLineTextChange = () => {
  emit('change', state.line.label)
  emit('close')
}

// 暴露变量
defineExpose({
  getParentData,
})
</script>
