<template>
  <div class="layout-pd">
    <el-alert
      title="温馨提示：此权限页面代码及效果只作为演示使用，若出现不可逆转的bug，请尝试 `F5` 刷新页面。若实际项目中非要实现此用户权限切换功能�?
      请在切换方法 `onRadioChange` 最后面添加刷新代码 `window.location.reload()`�?请注意：按钮权限页面中的演示2（指令模式）、演�?（函数模式）
      切换用户时无法动态演示，想要动态演示，请按 `F5` 或者添�?`window.location.reload()`�?
      type="warning"
      :closable="false"
    ></el-alert>
    <el-alert
      :title="`当前用户页面权限：[${userInfos.roles}]，当前用户按钮权限：[${userInfos.authBtnList}]`"
      type="success"
      :closable="false"
      class="mt15"
    ></el-alert>
    <el-card shadow="hover" header="切换用户演示，前端控制不同用户显示不同页面、按钮权�? class="mt15">
      <el-radio-group v-model="userAuth" @change="onRadioChange">
        <el-radio-button label="admin"></el-radio-button>
        <el-radio-button label="common"></el-radio-button>
      </el-radio-group>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/limitsFrontEndPage">
import { onMounted, ref } from 'vue'
import Cookies from 'js-cookie'
import { storeToRefs } from 'pinia'
import { useUserInfo } from '/@/stores/userInfo'
import { Session } from '/@/utils/storage'
import { frontEndsResetRoute, setAddRoute, setFilterMenuAndCacheTagsViewRoutes } from '/@/router/frontEnd'

// 定义变量内容
const storesUserInfo = useUserInfo()
const { userInfos } = storeToRefs(storesUserInfo)
const userAuth = ref('')

// 初始化用户权�?
const initUserAuth = () => {
  userAuth.value = userInfos.value.roles[0]
}
// 用户权限改变�?
const onRadioChange = async () => {
  // 清空之前缓存�?userInfo，防止不请求接口�?
  // stores/userInfo.ts
  Session.remove('userInfo')
  // 模拟数据
  frontEndsResetRoute()
  Cookies.set('userName', userAuth.value)
  // 模拟切换不同权限用户
  await storesUserInfo.setUserInfos()
  await setAddRoute()
  setFilterMenuAndCacheTagsViewRoutes()
}
// 页面加载�?
onMounted(() => {
  initUserAuth()
})
</script>
