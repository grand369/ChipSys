<template>
  <div class="member-auth-page">
    <!-- 登录页面 -->
    <member-login
      v-if="currentView === 'login'"
      @login-success="onLoginSuccess"
      @complete-profile="onCompleteProfile"
      @go-to-register="goToRegister"
    />

    <!-- 注册页面 -->
    <member-register
      v-if="currentView === 'register'"
      @register-success="onRegisterSuccess"
      @complete-profile="onCompleteProfile"
      @go-to-login="goToLogin"
    />

    <!-- 信息完善弹窗 -->
    <member-profile-complete
      v-model="showProfileComplete"
      :member-info="currentMemberInfo"
      @complete="onProfileComplete"
      @skip="onProfileSkip"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { useUserInfo } from '/@/stores/userInfo'
import { Session } from '/@/utils/storage'
import MemberLogin from './components/member-login.vue'
import MemberRegister from './components/member-register.vue'
import MemberProfileComplete from './components/member-profile-complete.vue'

// 路由
const router = useRouter()

// 响应式数据
const currentView = ref<'login' | 'register'>('login')
const showProfileComplete = ref(false)
const currentMemberInfo = ref<any>(null)

// 登录成功
const onLoginSuccess = (memberInfo: any) => {
  ElMessage.success('登录成功，欢迎回来！')
  // 检查是否有重定向参数
  const redirect = router.currentRoute.value.query.redirect as string
  if (redirect) {
    router.push(redirect)
  } else {
    router.push('/client/dashboard')
  }
}

// 注册成功
const onRegisterSuccess = (memberInfo: any) => {
  ElMessage.success('注册成功，欢迎加入我们！')
  // 检查是否有重定向参数
  const redirect = router.currentRoute.value.query.redirect as string
  if (redirect) {
    router.push(redirect)
  } else {
    router.push('/client/dashboard')
  }
}

// 需要完善信息
const onCompleteProfile = (memberInfo: any) => {
  currentMemberInfo.value = memberInfo
  showProfileComplete.value = true
}

// 信息完善完成
const onProfileComplete = (memberInfo: any) => {
  ElMessage.success('信息完善成功！')
  // 更新sessionStorage中的会员信息
  Session.set('member_info', memberInfo)
  // 检查是否有重定向参数
  const redirect = router.currentRoute.value.query.redirect as string
  if (redirect) {
    router.push(redirect)
  } else {
    router.push('/client/dashboard')
  }
}

// 跳过信息完善
const onProfileSkip = () => {
  ElMessage.info('您可以稍后在个人中心完善信息')
  
  // 调试信息
  const storesUseUserInfo = useUserInfo()
  const token = storesUseUserInfo.getToken()
  const memberInfo = Session.get('member_info')
  console.log('跳过信息完善时的状态:', {
    token: !!token,
    memberInfo: !!memberInfo,
    userRoles: storesUseUserInfo.userInfos.roles
  })
  
  // 检查是否有重定向参数
  const redirect = router.currentRoute.value.query.redirect as string
  if (redirect) {
    console.log('跳转到重定向页面:', redirect)
    router.push(redirect)
  } else {
    console.log('跳转到会员Dashboard')
    router.push('/client/dashboard')
  }
}

// 跳转到注册页面
const goToRegister = () => {
  currentView.value = 'register'
}

// 跳转到登录页面
const goToLogin = () => {
  currentView.value = 'login'
}

// 检查是否已登录
onMounted(() => {
  const storesUseUserInfo = useUserInfo()
  const token = storesUseUserInfo.getToken()
  const memberInfo = Session.get('member_info')
  
  if (token && memberInfo) {
    // 已登录，让路由守卫处理跳转，避免与路由守卫冲突
    // 路由守卫会自动将已登录的会员从登录页面重定向到Dashboard
  }
})
</script>

<style scoped>
.member-auth-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}
</style>
