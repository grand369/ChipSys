<template>
  <div class="layout-pd">
    <el-input v-model="val" placeholder="menu13：请输入内容测试路由缓存"></el-input>
  </div>
</template>

<script setup lang="ts" name="example/menu13">
import { ref, onActivated, onMounted } from 'vue'

// 定义变量内容
const val = ref('')

// 页面加载�?
onMounted(() => {
  window.console.log(2222)
})
// keep-alive 钩子函数，页面进入时
onActivated(() => {
  window.console.log(1111)
})
</script>
