<template>
  <div class="layout-pd">
    <el-card shadow="hover" header="复制剪切演示">
      <el-alert
        title="感谢优秀�?`vue-clipboard3`，项目地址：https://github.com/JamieCurnow/vue-clipboard3`"
        type="success"
        :closable="false"
        class="mb15"
      ></el-alert>
      <el-input placeholder="请输入内�? v-model="state.copyVal">
        <template #append>
          <el-button @click="copyText(state.copyVal)">复制链接</el-button>
        </template>
      </el-input>
      <el-input placeholder="先点击上�?`复制链接` 按钮，然�?`Ctrl + V` 进行粘贴�?" v-model="state.shearVal" class="mt15"> </el-input>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/funClipboard">
import { reactive } from 'vue'
import commonFunction from '/@/utils/commonFunction'

// 定义变量内容
const { copyText } = commonFunction()
const state = reactive({
  copyVal: 'https://gitee.com/chipsys/admin.ui.plus',
  shearVal: '',
})
</script>
