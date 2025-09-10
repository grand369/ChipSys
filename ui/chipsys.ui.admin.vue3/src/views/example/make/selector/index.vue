<template>
  <div class="selector-container layout-pd">
    <el-card shadow="hover" header="图标选择�?宽度自动)�?>
      <IconSelector @get="onGetIcon" @clear="onClearIcon" v-model="state.modelIcon" />
    </el-card>

    <el-card shadow="hover" header="图标选择�?宽度自动)：参�? class="mt15">
      <el-table :data="state.tableData" style="width: 100%">
        <el-table-column prop="a1" label="参数"> </el-table-column>
        <el-table-column prop="a2" label="说明"> </el-table-column>
        <el-table-column prop="a3" label="类型"> </el-table-column>
        <el-table-column prop="a4" label="可选�?> </el-table-column>
        <el-table-column prop="a5" label="默认�?> </el-table-column>
      </el-table>
    </el-card>

    <el-card shadow="hover" header="图标选择�?宽度自动)：事�? class="mt15">
      <el-table :data="state.tableData1" style="width: 100%">
        <el-table-column prop="a1" label="事件名称"> </el-table-column>
        <el-table-column prop="a2" label="说明"> </el-table-column>
        <el-table-column prop="a3" label="类型"> </el-table-column>
        <el-table-column prop="a4" label="回调参数"> </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/makeSelector">
import { defineAsyncComponent, reactive } from 'vue'

// 引入组件
const IconSelector = defineAsyncComponent(() => import('/@/components/iconSelector/index.vue'))

// 定义变量内容
const state = reactive({
  modelIcon: '',
  tableData: [
    {
      a1: 'v-model',
      a2: '双向绑定�?,
      a3: 'string',
      a4: '',
      a5: '',
    },
    {
      a1: 'prepend',
      a2: '输入框前置内容，只能字体图标',
      a3: 'string',
      a4: '',
      a5: 'ele-Pointer',
    },
    {
      a1: 'placeholder',
      a2: '输入框占位文�?,
      a3: 'string',
      a4: '',
      a5: '请输入内容搜索图标或者选择图标',
    },
    {
      a1: 'size',
      a2: '尺寸',
      a3: 'string',
      a4: 'large / default / small',
      a5: 'default',
    },
    {
      a1: 'title',
      a2: '弹窗标题',
      a3: 'string',
      a4: '',
      a5: '请选择图标',
    },
    {
      a1: 'disabled',
      a2: '禁用',
      a3: 'boolean',
      a4: 'true',
      a5: 'false',
    },
    {
      a1: 'clearable',
      a2: '是否可清�?,
      a3: 'boolean',
      a4: 'false',
      a5: 'true',
    },
    {
      a1: 'emptyDescription',
      a2: '自定义空状态描述文�?,
      a3: 'String',
      a4: '',
      a5: '无相关图�?,
    },
  ],
  tableData1: [
    {
      a1: 'get',
      a2: '获取当前点击�?icon 图标',
      a3: 'function',
      a4: '(icon: string)',
    },
    {
      a1: 'clear',
      a2: '清空当前点击�?icon 图标',
      a3: 'function',
      a4: '(icon: string)',
    },
  ],
})
// 获取当前点击�?icon 图标
const onGetIcon = (icon: string) => {
  window.console.log(icon)
}
// 清空当前点击�?icon 图标
const onClearIcon = (icon: string) => {
  window.console.log(icon)
}
</script>
