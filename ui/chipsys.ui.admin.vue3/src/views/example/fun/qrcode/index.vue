<template>
  <div class="qrcode-container layout-pd">
    <el-card shadow="hover" header="qrcodejs2 二维码生�?>
      <el-alert
        title="感谢优秀�?`qrcodejs2`，项目地址：https://github.com/davidshimjs/qrcodejs"
        type="success"
        :closable="false"
        class="mb15"
      ></el-alert>
      <div class="qrcode-img-warp">
        <div class="mb30 mt30 qrcode-img">
          <div class="qrcode" ref="qrcodeRef"></div>
        </div>
        <el-button type="primary" @click="onInitQrcode">
          <el-icon>
            <ele-Refresh />
          </el-icon>
          重新生成
        </el-button>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/funQrcode">
import { onMounted, ref } from 'vue'
import QRCode from 'qrcodejs2-fixes'

// 定义变量内容
const qrcodeRef = ref()

// 初始化生成二维码
const initQrcode = () => {
  new QRCode(qrcodeRef.value, {
    text: `https://admin.chipsys.net/login?t=${new Date().getTime()}`,
    width: 125,
    height: 125,
    colorDark: '#000000',
    colorLight: '#ffffff',
  })
}
// 重新生成
const onInitQrcode = () => {
  qrcodeRef.value.innerHTML = ''
  initQrcode()
}
// 页面加载�?
onMounted(() => {
  initQrcode()
})
</script>

<style scoped lang="scss">
.qrcode-container {
  .qrcode-img-warp {
    text-align: center;
    .qrcode-img {
      display: flex;
      width: 100%;
      height: 125px;
      .qrcode {
        margin: auto;
        width: 125px;
        height: 125px;
      }
    }
  }
}
</style>
