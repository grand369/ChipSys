<template>
  <div ref="printRef" class="layout-pd">
    <el-card shadow="hover" header="打印演示">
      <el-alert
        title="感谢优秀�?`print-js`，项目地址：https://github.com/crabbly/Print.js。请在打印弹�?`更多设置` 中开�?`背景图形。`"
        type="success"
        :closable="false"
        class="mb15"
      ></el-alert>
      <el-button @click="onPrintJs" type="primary">
        <SvgIcon name="iconfont icon-dayin" />
        点击打印演示
      </el-button>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/funPrintJs">
import { ref } from 'vue'
import printJs from 'print-js'

// 定义变量内容
const printRef = ref()

// 打印点击
const onPrintJs = () => {
  printJs({
    printable: printRef.value,
    type: 'html',
    css: ['//at.alicdn.com/t/c/font_2298093_rnp72ifj3ba.css', '//unpkg.com/element-plus/dist/index.css'],
    scanStyles: false,
    style: `@media print{.mb15{margin-bottom:15px;}.el-button--small i.iconfont{font-size: 12px !important;margin-right: 5px;}}`,
  })
}
</script>
