<template>
  <div class="fun-tagsview layout-pd">
    <NoticeBar
      text="已删除非当前�?tagsView 演示，后续有时间可以再加回来！，tagsview 支持多标签（参数不同）、单标签共用（参数不同）"
      background="#ecf5ff"
      color="#409eff"
    />
    <el-card shadow="hover" header="tagsView 当前页演�? class="mt15">
      <div class="flex-warp">
        <div class="flex-warp-item">
          <div class="flex-warp-item-box">
            <el-button type="primary" @click="refreshCurrentTagsView">
              <el-icon>
                <ele-RefreshRight />
              </el-icon>
              刷新当前�?
            </el-button>
          </div>
        </div>
        <div class="flex-warp-item">
          <div class="flex-warp-item-box">
            <el-button type="info" @click="closeCurrentTagsView">
              <el-icon>
                <ele-Close />
              </el-icon>
              关闭当前�?
            </el-button>
          </div>
        </div>
        <div class="flex-warp-item">
          <div class="flex-warp-item-box">
            <el-button type="warning" @click="closeOtherTagsView">
              <el-icon>
                <ele-CircleClose />
              </el-icon>
              关闭其它
            </el-button>
          </div>
        </div>
        <div class="flex-warp-item">
          <div class="flex-warp-item-box">
            <el-button type="danger" @click="closeAllTagsView">
              <el-icon>
                <ele-FolderDelete />
              </el-icon>
              全部关闭
            </el-button>
          </div>
        </div>
        <div class="flex-warp-item">
          <div class="flex-warp-item-box">
            <el-button type="success" @click="openCurrenFullscreen">
              <el-icon>
                <ele-FullScreen />
              </el-icon>
              当前页全�?
            </el-button>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/funTagsView">
import { defineAsyncComponent } from 'vue'
import { useRoute } from 'vue-router'
import mittBus from '/@/utils/mitt'

// 引入组件
const NoticeBar = defineAsyncComponent(() => import('/@/components/noticeBar/index.vue'))

// 定义变量内容
const route = useRoute()

// 0 刷新当前�? 关闭当前�? 关闭其它�? 关闭全部 4 当前页全�?
// 1、刷新当�?tagsView
const refreshCurrentTagsView = () => {
  mittBus.emit('onCurrentContextmenuClick', Object.assign({}, { contextMenuClickId: 0, ...route }))
}
// 2、关闭当�?tagsView
const closeCurrentTagsView = () => {
  mittBus.emit('onCurrentContextmenuClick', Object.assign({}, { contextMenuClickId: 1, ...route }))
}
// 3、关闭其�?tagsView
const closeOtherTagsView = () => {
  mittBus.emit('onCurrentContextmenuClick', Object.assign({}, { contextMenuClickId: 2, ...route }))
}
// 4、关闭全�?tagsView
const closeAllTagsView = () => {
  mittBus.emit('onCurrentContextmenuClick', Object.assign({}, { contextMenuClickId: 3, ...route }))
}
// 5、开启当前页面全�?
const openCurrenFullscreen = () => {
  mittBus.emit('onCurrentContextmenuClick', Object.assign({}, { contextMenuClickId: 4, ...route }))
}
</script>

<style scoped lang="scss">
.fun-tagsview {
  .fun-tagsview-from-item {
    :deep(.el-form-item__content) {
      margin-left: 0 !important;
    }
  }
}
</style>
