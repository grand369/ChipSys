<template>
  <div class="svg-demo-container layout-pd">
    <el-card shadow="hover" header="svgIcon：演示（支持本地svg�?>
      <SvgIcon name="iconfont icon-shuju1" color="red" :size="30" />
      <SvgIcon name="ele-Trophy" color="var(--el-color-primary)" :size="30" />
      <SvgIcon name="fa fa-flag-checkered" color="#09f" :size="30" />
      <SvgIcon class="ml5" :name="logoMini" :size="30" color="" />
    </el-card>
    <el-card shadow="hover" header="svgIcon：参�? class="mt15">
      <el-table :data="state.tableData" style="width: 100%">
        <el-table-column prop="a1" label="参数"> </el-table-column>
        <el-table-column prop="a2" label="说明"> </el-table-column>
        <el-table-column prop="a3" label="类型"> </el-table-column>
        <el-table-column prop="a4" label="可选�?> </el-table-column>
        <el-table-column prop="a5" label="默认�?> </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/makeSvgDemo">
import { reactive } from 'vue'
import logoMini from '/@/assets/logo-mini.svg'

// 定义变量内容
const state = reactive({
  tableData: [
    {
      a1: 'name',
      a2: 'svg 图标组件名字 / svg 路径 url',
      a3: 'string',
      a4: '',
      a5: '',
    },
    {
      a1: 'size',
      a2: 'svg 大小',
      a3: 'number',
      a4: '',
      a5: 14,
    },
    {
      a1: 'color',
      a2: 'svg 颜色',
      a3: 'string',
      a4: '',
      a5: 'currentColor',
    },
  ],
})
</script>
