<template>
  <div class="member-register">
    <div class="register-container">
      <div class="register-header">
        <h2>会员注册</h2>
        <p>欢迎加入我们的平台</p>
      </div>

      <div class="register-tabs">
        <el-tabs v-model="activeTab" @tab-change="onTabChange">
          <el-tab-pane label="手机号注册" name="mobile">
            <el-form
              ref="mobileFormRef"
              :model="mobileForm"
              :rules="mobileRules"
              label-width="0"
              class="register-form"
            >
              <el-form-item prop="mobile">
                <el-input
                  v-model="mobileForm.mobile"
                  placeholder="请输入手机号"
                  maxlength="11"
                  clearable
                >
                  <template #prefix>
                    <el-icon><Phone /></el-icon>
                  </template>
                </el-input>
              </el-form-item>

              <el-form-item prop="code">
                <div class="code-input-group">
                  <el-input
                    v-model="mobileForm.code"
                    placeholder="请输入验证码"
                    maxlength="6"
                    clearable
                  >
                    <template #prefix>
                      <el-icon><Message /></el-icon>
                    </template>
                  </el-input>
                  <el-button
                    :disabled="mobileCodeDisabled"
                    :loading="mobileCodeLoading"
                    @click="sendMobileCode"
                    class="code-btn"
                  >
                    {{ mobileCodeText }}
                  </el-button>
                </div>
              </el-form-item>

              <el-form-item>
                <el-button
                  type="primary"
                  :loading="mobileLoading"
                  @click="registerByMobile"
                  class="register-btn"
                >
                  注册
                </el-button>
              </el-form-item>
            </el-form>
          </el-tab-pane>

          <el-tab-pane label="邮箱注册" name="email">
            <el-form
              ref="emailFormRef"
              :model="emailForm"
              :rules="emailRules"
              label-width="0"
              class="register-form"
            >
              <el-form-item prop="email">
                <el-input
                  v-model="emailForm.email"
                  placeholder="请输入邮箱地址"
                  clearable
                >
                  <template #prefix>
                    <el-icon><Message /></el-icon>
                  </template>
                </el-input>
              </el-form-item>

              <el-form-item prop="code">
                <div class="code-input-group">
                  <el-input
                    v-model="emailForm.code"
                    placeholder="请输入验证码"
                    maxlength="6"
                    clearable
                  >
                    <template #prefix>
                      <el-icon><Key /></el-icon>
                    </template>
                  </el-input>
                  <el-button
                    :disabled="emailCodeDisabled"
                    :loading="emailCodeLoading"
                    @click="sendEmailCode"
                    class="code-btn"
                  >
                    {{ emailCodeText }}
                  </el-button>
                </div>
              </el-form-item>

              <el-form-item>
                <el-button
                  type="primary"
                  :loading="emailLoading"
                  @click="registerByEmail"
                  class="register-btn"
                >
                  注册
                </el-button>
              </el-form-item>
            </el-form>
          </el-tab-pane>

          <el-tab-pane label="微信注册" name="wechat">
            <div class="wechat-register">
              <div class="wechat-qr-container">
                <div class="qr-placeholder">
                  <el-icon size="80"><ChatDotRound /></el-icon>
                  <p>微信扫码注册</p>
                </div>
                <el-button
                  type="primary"
                  :loading="wechatLoading"
                  @click="registerByWechat"
                  class="wechat-btn"
                >
                  微信授权注册
                </el-button>
              </div>
            </div>
          </el-tab-pane>
        </el-tabs>
      </div>

      <div class="register-footer">
        <p>
          已有账号？
          <el-button type="text" @click="goToLogin">立即登录</el-button>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, computed } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Phone, Message, Key, ChatDotRound } from '@element-plus/icons-vue'
import { memberAuthApi } from '/@/api/client'
import { useUserInfo } from '/@/stores/userInfo'
import { Session } from '/@/utils/storage'
import type { FormInstance, FormRules } from 'element-plus'

// 响应式数据
const activeTab = ref('mobile')
const mobileFormRef = ref<FormInstance>()
const emailFormRef = ref<FormInstance>()

// 手机号注册表单
const mobileForm = reactive({
  mobile: '',
  code: '',
  codeId: ''
})

// 邮箱注册表单
const emailForm = reactive({
  email: '',
  code: '',
  codeId: ''
})

// 加载状态
const mobileLoading = ref(false)
const emailLoading = ref(false)
const wechatLoading = ref(false)
const mobileCodeLoading = ref(false)
const emailCodeLoading = ref(false)

// 验证码倒计时
const mobileCodeCountdown = ref(0)
const emailCodeCountdown = ref(0)

// 计算属性
const mobileCodeDisabled = computed(() => mobileCodeCountdown.value > 0)
const emailCodeDisabled = computed(() => emailCodeCountdown.value > 0)
const mobileCodeText = computed(() => 
  mobileCodeCountdown.value > 0 ? `${mobileCodeCountdown.value}s后重发` : '获取验证码'
)
const emailCodeText = computed(() => 
  emailCodeCountdown.value > 0 ? `${emailCodeCountdown.value}s后重发` : '获取验证码'
)

// 表单验证规则
const mobileRules: FormRules = {
  mobile: [
    { required: true, message: '请输入手机号', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '手机号格式不正确', trigger: 'blur' }
  ],
  code: [
    { required: true, message: '请输入验证码', trigger: 'blur' },
    { pattern: /^\d{6}$/, message: '验证码格式不正确', trigger: 'blur' }
  ]
}

const emailRules: FormRules = {
  email: [
    { required: true, message: '请输入邮箱地址', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: 'blur' }
  ],
  code: [
    { required: true, message: '请输入验证码', trigger: 'blur' },
    { pattern: /^\d{6}$/, message: '验证码格式不正确', trigger: 'blur' }
  ]
}

// 发送手机验证码
const sendMobileCode = async () => {
  if (!mobileForm.mobile) {
    ElMessage.warning('请先输入手机号')
    return
  }

  if (!/^1[3-9]\d{9}$/.test(mobileForm.mobile)) {
    ElMessage.warning('手机号格式不正确')
    return
  }

  try {
    mobileCodeLoading.value = true
    
    // 检查手机号是否已注册
    const exists = await memberAuthApi.checkMobileExists(mobileForm.mobile)
    if (exists) {
      ElMessage.warning('该手机号已注册，请直接登录')
      return
    }

    // 发送验证码
    const codeId = await memberAuthApi.sendMobileCode(mobileForm.mobile)
    mobileForm.codeId = codeId
    
    ElMessage.success('验证码已发送')
    startCountdown('mobile')
  } catch (error: any) {
    ElMessage.error(error.message || '发送验证码失败')
  } finally {
    mobileCodeLoading.value = false
  }
}

// 发送邮箱验证码
const sendEmailCode = async () => {
  if (!emailForm.email) {
    ElMessage.warning('请先输入邮箱地址')
    return
  }

  try {
    emailCodeLoading.value = true
    
    // 检查邮箱是否已注册
    const exists = await memberAuthApi.checkEmailExists(emailForm.email)
    if (exists) {
      ElMessage.warning('该邮箱已注册，请直接登录')
      return
    }

    // 发送验证码
    const codeId = await memberAuthApi.sendEmailCode(emailForm.email)
    emailForm.codeId = codeId
    
    ElMessage.success('验证码已发送')
    startCountdown('email')
  } catch (error: any) {
    ElMessage.error(error.message || '发送验证码失败')
  } finally {
    emailCodeLoading.value = false
  }
}

// 手机号注册
const registerByMobile = async () => {
  if (!mobileFormRef.value) return

  try {
    await mobileFormRef.value.validate()
    mobileLoading.value = true

    const result = await memberAuthApi.registerByMobile({
      mobile: mobileForm.mobile,
      code: mobileForm.code,
      codeId: mobileForm.codeId
    })

    // 检查返回结果是否有效
    if (!result || !result.accessToken || !result.memberInfo) {
      throw new Error('注册失败：返回数据无效')
    }

    ElMessage.success('注册成功')
    
    // 使用统一的token管理机制
    const storesUseUserInfo = useUserInfo()
    storesUseUserInfo.setToken(result.accessToken)
    
    // 设置用户基本信息到store
    storesUseUserInfo.setUserName(result.memberInfo.nickName || result.memberInfo.realName || '会员用户')
    
    // 存储会员信息到sessionStorage，避免与admin用户信息冲突
    Session.set('member_info', result.memberInfo)
    
    // 检查是否需要完善信息
    if (!result.memberInfo.isProfileComplete) {
      // 跳转到信息完善页面
      emit('complete-profile', result.memberInfo)
    } else {
      // 跳转到首页
      emit('register-success', result.memberInfo)
    }
  } catch (error: any) {
    // 后端已注册等业务错误时，仅提示，不再访问 result.memberInfo
    ElMessage.error(error?.message || '注册失败')
  } finally {
    mobileLoading.value = false
  }
}

// 邮箱注册
const registerByEmail = async () => {
  if (!emailFormRef.value) return

  try {
    await emailFormRef.value.validate()
    emailLoading.value = true

    const result = await memberAuthApi.registerByEmail({
      email: emailForm.email,
      code: emailForm.code,
      codeId: emailForm.codeId
    })

    // 检查返回结果是否有效
    if (!result || !result.accessToken || !result.memberInfo) {
      throw new Error('注册失败：返回数据无效')
    }

    ElMessage.success('注册成功')
    
    // 使用统一的token管理机制
    const storesUseUserInfo = useUserInfo()
    storesUseUserInfo.setToken(result.accessToken)
    
    // 设置用户基本信息到store
    storesUseUserInfo.setUserName(result.memberInfo.nickName || result.memberInfo.realName || '会员用户')
    
    // 存储会员信息到sessionStorage，避免与admin用户信息冲突
    Session.set('member_info', result.memberInfo)
    
    // 检查是否需要完善信息
    if (!result.memberInfo.isProfileComplete) {
      // 跳转到信息完善页面
      emit('complete-profile', result.memberInfo)
    } else {
      // 跳转到首页
      emit('register-success', result.memberInfo)
    }
  } catch (error: any) {
    ElMessage.error(error?.message || '注册失败')
  } finally {
    emailLoading.value = false
  }
}

// 微信注册
const registerByWechat = async () => {
  try {
    wechatLoading.value = true
    
    // TODO: 实现微信授权逻辑
    // 这里应该调用微信授权接口
    const mockWechatData = {
      code: 'mock_wechat_code',
      openId: 'mock_openid',
      unionId: 'mock_unionid',
      nickName: '微信用户',
      avatar: ''
    }

    const result = await memberAuthApi.registerByWechat(mockWechatData)

    // 检查返回结果是否有效
    if (!result || !result.accessToken || !result.memberInfo) {
      throw new Error('注册失败：返回数据无效')
    }

    ElMessage.success('注册成功')
    
    // 使用统一的token管理机制
    const storesUseUserInfo = useUserInfo()
    storesUseUserInfo.setToken(result.accessToken)
    
    // 设置用户基本信息到store
    storesUseUserInfo.setUserName(result.memberInfo.nickName || result.memberInfo.realName || '会员用户')
    
    // 存储会员信息到sessionStorage，避免与admin用户信息冲突
    Session.set('member_info', result.memberInfo)
    
    // 检查是否需要完善信息
    if (!result.memberInfo.isProfileComplete) {
      // 跳转到信息完善页面
      emit('complete-profile', result.memberInfo)
    } else {
      // 跳转到首页
      emit('register-success', result.memberInfo)
    }
  } catch (error: any) {
    ElMessage.error(error.message || '微信注册失败')
  } finally {
    wechatLoading.value = false
  }
}

// 开始倒计时
const startCountdown = (type: 'mobile' | 'email') => {
  const countdown = type === 'mobile' ? mobileCodeCountdown : emailCodeCountdown
  countdown.value = 60
  
  const timer = setInterval(() => {
    countdown.value--
    if (countdown.value <= 0) {
      clearInterval(timer)
    }
  }, 1000)
}

// 切换标签页
const onTabChange = (tabName: string) => {
  // 清空表单
  if (tabName === 'mobile') {
    Object.assign(mobileForm, { mobile: '', code: '', codeId: '' })
  } else if (tabName === 'email') {
    Object.assign(emailForm, { email: '', code: '', codeId: '' })
  }
}

// 跳转到登录页面
const goToLogin = () => {
  emit('go-to-login')
}

// 定义事件
const emit = defineEmits<{
  'register-success': [memberInfo: any]
  'complete-profile': [memberInfo: any]
  'go-to-login': []
}>()
</script>

<style scoped>
.member-register {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
}

.register-container {
  background: white;
  border-radius: 12px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
  padding: 40px;
  width: 100%;
  max-width: 450px;
}

.register-header {
  text-align: center;
  margin-bottom: 30px;
}

.register-header h2 {
  color: #333;
  margin: 0 0 10px 0;
  font-size: 28px;
  font-weight: 600;
}

.register-header p {
  color: #666;
  margin: 0;
  font-size: 14px;
}

.register-tabs {
  margin-bottom: 20px;
}

.register-form {
  margin-top: 20px;
}

.code-input-group {
  display: flex;
  gap: 10px;
}

.code-input-group .el-input {
  flex: 1;
}

.code-btn {
  min-width: 120px;
  white-space: nowrap;
}

.register-btn {
  width: 100%;
  height: 45px;
  font-size: 16px;
  font-weight: 500;
}

.wechat-register {
  text-align: center;
  padding: 40px 20px;
}

.wechat-qr-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
}

.qr-placeholder {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
  color: #999;
}

.qr-placeholder p {
  margin: 0;
  font-size: 14px;
}

.wechat-btn {
  width: 200px;
  height: 45px;
  font-size: 16px;
  background: #07c160;
  border-color: #07c160;
}

.wechat-btn:hover {
  background: #06ad56;
  border-color: #06ad56;
}

.register-footer {
  text-align: center;
  margin-top: 20px;
}

.register-footer p {
  color: #666;
  margin: 0;
  font-size: 14px;
}

:deep(.el-tabs__item) {
  font-size: 16px;
  font-weight: 500;
}

:deep(.el-tabs__content) {
  padding-top: 20px;
}

:deep(.el-form-item) {
  margin-bottom: 20px;
}

:deep(.el-input__wrapper) {
  height: 45px;
  border-radius: 8px;
}

:deep(.el-button) {
  border-radius: 8px;
}
</style>
