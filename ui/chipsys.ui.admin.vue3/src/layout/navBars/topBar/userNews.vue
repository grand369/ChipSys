<template>
  <div class="layout-navbars-breadcrumb-user-news">
    <div class="head-box">
      <div class="head-box-title">{{ $t('message.user.newTitle') }}</div>
      <div class="head-box-btn" v-if="state.newsList.length > 0" @click="onAllReadClick">{{ $t('message.user.newBtn') }}</div>
    </div>
    <div class="content-box">
      <template v-if="state.newsList.length > 0">
        <div class="content-box-item" v-for="(v, k) in state.newsList" :key="k">
          <div>{{ v.label }}</div>
          <div class="content-box-msg">
            {{ v.value }}
          </div>
          <div class="content-box-time">{{ v.time }}</div>
        </div>
      </template>
      <el-empty :description="$t('message.user.newDesc')" v-else></el-empty>
    </div>
    <div class="foot-box" @click="onGoToGiteeClick" v-if="state.newsList.length > 0">{{ $t('message.user.newGo') }}</div>
  </div>
</template>

<script setup lang="ts" name="layoutBreadcrumbUserNews">
import { reactive } from 'vue'

// 定义变量内容
const state = reactive({
  newsList: [
    {
      label: '关于版本发布的通知',
      value: 'admin，基�?vue3 + CompositionAPI + typescript + vite + element plus，正式发布时间：2022�?2�?8日！',
      time: '2022-12-08',
    },
  ],
})

// 全部已读点击
const onAllReadClick = () => {
  state.newsList = []
}
// 前往通知中心点击
const onGoToGiteeClick = () => {
  window.open('https://gitee.com/chipsys/admin.ui.plus')
}
</script>

<style scoped lang="scss">
.layout-navbars-breadcrumb-user-news {
  .head-box {
    display: flex;
    border-bottom: 1px solid var(--el-border-color-lighter);
    box-sizing: border-box;
    color: var(--el-text-color-primary);
    justify-content: space-between;
    height: 35px;
    align-items: center;
    .head-box-btn {
      color: var(--el-color-primary);
      font-size: 13px;
      cursor: pointer;
      opacity: 0.8;
      &:hover {
        opacity: 1;
      }
    }
  }
  .content-box {
    font-size: 13px;
    .content-box-item {
      padding-top: 12px;
      &:last-of-type {
        padding-bottom: 12px;
      }
      .content-box-msg {
        color: var(--el-text-color-secondary);
        margin-top: 5px;
        margin-bottom: 5px;
      }
      .content-box-time {
        color: var(--el-text-color-secondary);
      }
    }
  }
  .foot-box {
    height: 35px;
    color: var(--el-color-primary);
    font-size: 13px;
    cursor: pointer;
    opacity: 0.8;
    display: flex;
    align-items: center;
    justify-content: center;
    border-top: 1px solid var(--el-border-color-lighter);
    &:hover {
      opacity: 1;
    }
  }
  :deep(.el-empty__description p) {
    font-size: 13px;
  }
}
</style>
