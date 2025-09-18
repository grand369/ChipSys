<template>
  <div class="member-upgrade">
    <div class="upgrade-header">
      <h1>会员升级</h1>
      <p>选择适合您的会员等级，享受更多权益</p>
    </div>

    <div class="level-cards">
      <div 
        v-for="level in levelList" 
        :key="level.id"
        class="level-card"
        :class="{ 'selected': selectedLevel?.id === level.id, 'current': level.isCurrent }"
        @click="selectLevel(level)"
      >
        <div class="level-header">
          <h3>{{ getLevelName(level.level) }}</h3>
          <div v-if="level.isCurrent" class="current-badge">当前等级</div>
        </div>
        
        <div class="level-price">
          <span class="price">{{ getLevelPrice(level.level) }}</span>
          <span class="unit">元/年</span>
        </div>

        <div class="level-features">
          <div class="feature-item">
            <i class="el-icon-check"></i>
            <span>产品数据查看：{{ level.productDataLimit }}条</span>
          </div>
          <div class="feature-item">
            <i class="el-icon-check"></i>
            <span>供应商数据查看：{{ level.supplierDataLimit }}条</span>
          </div>
          <div class="feature-item">
            <i class="el-icon-check"></i>
            <span>分类限制：{{ level.categoryLimit }}个</span>
          </div>
          <div class="feature-item">
            <i class="el-icon-check"></i>
            <span>完整联系人信息：{{ level.showFullContactInfo ? '是' : '否' }}</span>
          </div>
          <div v-if="level.level !== 'Free'" class="feature-item">
            <i class="el-icon-check"></i>
            <span>询价功能</span>
          </div>
          <div v-if="level.level === 'Enterprise'" class="feature-item">
            <i class="el-icon-check"></i>
            <span>无限制数据查看</span>
          </div>
        </div>

        <div class="level-actions">
          <el-button 
            v-if="!level.isCurrent"
            type="primary" 
            size="large"
            :disabled="selectedLevel?.id !== level.id"
            @click="startPayment(level)"
          >
            立即升级
          </el-button>
          <el-button v-else type="success" size="large" disabled>
            当前等级
          </el-button>
        </div>
      </div>
    </div>

    <!-- 支付对话框 -->
    <el-dialog v-model="paymentDialog.visible" title="支付订单" width="500px" :close-on-click-modal="false">
      <div class="payment-info">
        <div class="order-info">
          <h3>订单信息</h3>
          <p>会员等级：{{ getLevelName(selectedLevel?.level) }}</p>
          <p>支付金额：<span class="amount">¥{{ getLevelPrice(selectedLevel?.level) }}</span></p>
          <p>有效期：1年</p>
        </div>

        <div class="payment-methods">
          <h3>选择支付方式</h3>
          <div class="payment-options">
            <div 
              class="payment-option"
              :class="{ 'selected': paymentDialog.method === 'alipay' }"
              @click="paymentDialog.method = 'alipay'"
            >
              <i class="alipay-icon"></i>
              <span>支付宝</span>
            </div>
            <div 
              class="payment-option"
              :class="{ 'selected': paymentDialog.method === 'wechat' }"
              @click="paymentDialog.method = 'wechat'"
            >
              <i class="wechat-icon"></i>
              <span>微信支付</span>
            </div>
          </div>
        </div>

        <div v-if="paymentDialog.qrCode" class="qr-code">
          <h3>请扫码支付</h3>
          <div class="qr-container">
            <img :src="paymentDialog.qrCode" alt="支付二维码" />
            <p>请使用{{ paymentDialog.method === 'alipay' ? '支付宝' : '微信' }}扫描二维码完成支付</p>
          </div>
        </div>
      </div>

      <template #footer>
        <el-button @click="paymentDialog.visible = false">取消</el-button>
        <el-button 
          type="primary" 
          @click="createPayment"
          :loading="paymentDialog.loading"
        >
          生成支付码
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted, computed } from 'vue'
import { ElMessage } from 'element-plus'
import { MemberLevelManageApi } from '/@/api/admin/MemberLevelManage'
import { PaymentApi } from '/@/api/client/Payment'

const state = reactive({
  levelList: [] as any[],
  selectedLevel: null as any,
  currentLevel: null as any
})

const paymentDialog = reactive({
  visible: false,
  method: 'alipay' as 'alipay' | 'wechat',
  qrCode: '',
  loading: false
})

// 获取等级名称
const getLevelName = (level: string) => {
  const levelNames: Record<string, string> = {
    'Free': '免费会员',
    'Basic': '基础版',
    'Standard': '标准版',
    'Premium': '高级版',
    'Enterprise': '企业版'
  }
  return levelNames[level] || level
}

// 获取等级价格
const getLevelPrice = (level: string) => {
  const prices: Record<string, number> = {
    'Free': 0,
    'Basic': 99,
    'Standard': 299,
    'Premium': 599,
    'Enterprise': 1299
  }
  return prices[level] || 0
}

// 选择等级
const selectLevel = (level: any) => {
  if (!level.isCurrent) {
    state.selectedLevel = level
  }
}

// 开始支付
const startPayment = (level: any) => {
  state.selectedLevel = level
  paymentDialog.visible = true
  paymentDialog.qrCode = ''
}

// 创建支付
const createPayment = async () => {
  if (!state.selectedLevel) return

  paymentDialog.loading = true
  try {
    const api = new PaymentApi()
    const res = await api.createPayment({
      level: state.selectedLevel.level,
      amount: getLevelPrice(state.selectedLevel.level),
      paymentMethod: paymentDialog.method
    })

    if (res?.success) {
      paymentDialog.qrCode = res.data?.qrCode
      // 开始轮询支付状态
      startPaymentStatusCheck(res.data?.orderId)
    } else {
      ElMessage.error(res?.message || '创建支付失败')
    }
  } catch (error) {
    ElMessage.error('创建支付失败')
  } finally {
    paymentDialog.loading = false
  }
}

// 轮询支付状态
const startPaymentStatusCheck = (orderId: string) => {
  let pollCount = 0
  const maxPolls = 60 // 最多轮询60次（3分钟）
  
  const checkStatus = async () => {
    try {
      pollCount++
      
      const api = new PaymentApi()
      const res = await api.checkPaymentStatus(orderId)
      
      if (res?.success && res.data?.status === 'paid') {
        ElMessage.success('支付成功！会员等级已升级')
        paymentDialog.visible = false
        fetchLevels()
        return
      }
      
      if (res?.success && res.data?.status === 'failed') {
        ElMessage.error('支付失败')
        paymentDialog.qrCode = ''
        return
      }
      
      if (res?.success && res.data?.status === 'expired') {
        ElMessage.warning('支付已过期，请重新支付')
        paymentDialog.qrCode = ''
        return
      }
      
      // 检查是否超过最大轮询次数
      if (pollCount >= maxPolls) {
        ElMessage.warning('支付超时，请重新支付')
        paymentDialog.qrCode = ''
        return
      }
      
      // 继续轮询
      setTimeout(checkStatus, 3000)
    } catch (error) {
      console.error('检查支付状态失败:', error)
      
      // 网络错误时也继续轮询，但增加计数
      if (pollCount < maxPolls) {
        setTimeout(checkStatus, 5000) // 网络错误时延长间隔
      }
    }
  }
  
  checkStatus()
}

// 获取等级列表
const fetchLevels = async () => {
  try {
    const api = new MemberLevelManageApi()
    const res = await api.getLevelTemplates()
    
    if (res?.success) {
      // 获取当前用户等级
      const currentLevelRes = await api.getCurrentLevel()
      const currentLevel = currentLevelRes?.data
      
      state.levelList = res.data?.map((level: any) => ({
        ...level,
        isCurrent: level.level === currentLevel?.level
      }))
      
      state.currentLevel = currentLevel
    }
  } catch (error) {
    ElMessage.error('获取等级列表失败')
  }
}

onMounted(() => {
  fetchLevels()
})
</script>

<style scoped>
.member-upgrade {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.upgrade-header {
  text-align: center;
  margin-bottom: 40px;
}

.upgrade-header h1 {
  font-size: 32px;
  color: #333;
  margin-bottom: 10px;
}

.upgrade-header p {
  font-size: 16px;
  color: #666;
}

.level-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
  margin-bottom: 40px;
}

.level-card {
  border: 2px solid #e4e7ed;
  border-radius: 12px;
  padding: 24px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s;
  position: relative;
}

.level-card:hover {
  border-color: #409eff;
  box-shadow: 0 4px 12px rgba(64, 158, 255, 0.15);
}

.level-card.selected {
  border-color: #409eff;
  background: #f0f9ff;
}

.level-card.current {
  border-color: #67c23a;
  background: #f0f9ff;
}

.current-badge {
  position: absolute;
  top: -10px;
  right: 20px;
  background: #67c23a;
  color: white;
  padding: 4px 12px;
  border-radius: 12px;
  font-size: 12px;
}

.level-header h3 {
  font-size: 24px;
  color: #333;
  margin-bottom: 16px;
}

.level-price {
  margin-bottom: 24px;
}

.price {
  font-size: 36px;
  font-weight: bold;
  color: #409eff;
}

.unit {
  font-size: 16px;
  color: #666;
  margin-left: 4px;
}

.level-features {
  text-align: left;
  margin-bottom: 24px;
}

.feature-item {
  display: flex;
  align-items: center;
  margin-bottom: 12px;
  font-size: 14px;
  color: #666;
}

.feature-item i {
  color: #67c23a;
  margin-right: 8px;
  font-size: 16px;
}

.level-actions {
  margin-top: 24px;
}

.payment-info {
  padding: 20px 0;
}

.order-info {
  margin-bottom: 24px;
  padding: 16px;
  background: #f5f7fa;
  border-radius: 8px;
}

.order-info h3 {
  margin-bottom: 12px;
  color: #333;
}

.order-info p {
  margin-bottom: 8px;
  color: #666;
}

.amount {
  font-size: 20px;
  font-weight: bold;
  color: #e6a23c;
}

.payment-methods h3 {
  margin-bottom: 16px;
  color: #333;
}

.payment-options {
  display: flex;
  gap: 16px;
}

.payment-option {
  flex: 1;
  padding: 16px;
  border: 2px solid #e4e7ed;
  border-radius: 8px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s;
}

.payment-option:hover {
  border-color: #409eff;
}

.payment-option.selected {
  border-color: #409eff;
  background: #f0f9ff;
}

.payment-option i {
  display: block;
  width: 40px;
  height: 40px;
  margin: 0 auto 8px;
  background-size: contain;
}

.alipay-icon {
  background: url('/icons/alipay.png') no-repeat center;
}

.wechat-icon {
  background: url('/icons/wechat.png') no-repeat center;
}

.qr-code {
  text-align: center;
  margin-top: 24px;
}

.qr-code h3 {
  margin-bottom: 16px;
  color: #333;
}

.qr-container {
  padding: 20px;
  background: #f5f7fa;
  border-radius: 8px;
}

.qr-container img {
  max-width: 200px;
  margin-bottom: 12px;
}

.qr-container p {
  color: #666;
  font-size: 14px;
}
</style>
