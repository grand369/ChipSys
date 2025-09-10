<template>
  <div class="croppers-container layout-pd">
    <el-card shadow="hover" header="cropper 图片裁剪">
      <el-alert
        title="感谢优秀�?`cropperjs`，项目地址：https://github.com/fengyuanchen/cropperjs"
        type="success"
        :closable="false"
        class="mb15"
      ></el-alert>
      <div class="cropper-img-warp">
        <div class="mb15 mt15">
          <img class="cropper-img" :src="state.cropperImg" />
        </div>
        <el-button type="primary" @click="onCropperDialogOpen">
          <el-icon>
            <ele-Crop />
          </el-icon>
          更换头像
        </el-button>
      </div>
    </el-card>
    <CropperDialog ref="cropperDialogRef" v-model:cropperImg="state.cropperImg" />
  </div>
</template>

<script setup lang="ts" name="example/funCropper">
import { defineAsyncComponent, ref, reactive } from 'vue'

// 引入组件
const CropperDialog = defineAsyncComponent(() => import('/@/components/cropper/index.vue'))

// 定义变量内容
const cropperDialogRef = ref()
const defaultImg = 'https://img2.baidu.com/it/u=1978192862,2048448374&fm=253&fmt=auto&app=138&f=JPEG?w=504&h=500'
const state = reactive({
  cropperImg: defaultImg,
})

// 打开裁剪弹窗
const onCropperDialogOpen = () => {
  cropperDialogRef.value.openDialog(defaultImg)
}
</script>

<style scoped lang="scss">
.croppers-container {
  .cropper-img-warp {
    text-align: center;
    .cropper-img {
      margin: auto;
      width: 150px;
      height: 150px;
      border-radius: 100%;
    }
  }
}
</style>
