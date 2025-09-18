<template>
  <div class="personal-center">
    <div class="center-header">
      <h1>个人中心</h1>
    </div>

    <div class="center-content">
      <!-- 用户信息卡片 -->
      <el-card class="user-info-card">
        <template #header>
          <div class="card-header">
            <span>个人信息</span>
          </div>
        </template>
        <div class="user-info">
          <div class="avatar">
            <el-avatar :size="80" :src="userInfo.avatar">
              {{ userInfo.nickName?.charAt(0) }}
            </el-avatar>
          </div>
          <div class="user-details">
            <h3>{{ userInfo.nickName || userInfo.userName }}</h3>
            <p>会员等级：{{ getLevelName(userInfo.currentLevel?.level) }}</p>
            <p>注册时间：{{ formatDate(userInfo.createdTime) }}</p>
          </div>
        </div>
      </el-card>

      <!-- 会员等级卡片 -->
      <el-card class="level-card">
        <template #header>
          <div class="card-header">
            <span>会员等级</span>
            <el-button v-if="userInfo.currentLevel?.level === 'Free'" type="primary" @click="goToUpgrade">
              立即升级
            </el-button>
          </div>
        </template>
        <div class="level-info">
          <div class="level-badge" :class="getLevelClass(userInfo.currentLevel?.level)">
            {{ getLevelName(userInfo.currentLevel?.level) }}
          </div>
          <div class="level-features">
            <div class="feature-item">
              <i class="el-icon-check"></i>
              <span>产品数据查看：{{ userInfo.currentLevel?.productDataLimit || 0 }}条</span>
            </div>
            <div class="feature-item">
              <i class="el-icon-check"></i>
              <span>供应商数据查看：{{ userInfo.currentLevel?.supplierDataLimit || 0 }}条</span>
            </div>
            <div class="feature-item">
              <i class="el-icon-check"></i>
              <span>完整联系人信息：{{ userInfo.currentLevel?.showFullContactInfo ? '是' : '否' }}</span>
            </div>
          </div>
        </div>
      </el-card>

      <!-- 供应商申请卡片 -->
      <el-card v-if="userInfo.currentLevel?.level !== 'Free'" class="supplier-card">
        <template #header>
          <div class="card-header">
            <span>供应商申请</span>
          </div>
        </template>
        <div class="supplier-info">
          <div v-if="!supplierApplication" class="no-application">
            <p>您还不是供应商，申请成为供应商后可以发布产品信息</p>
            <el-button type="primary" @click="openApplicationDialog">
              申请成为供应商
            </el-button>
          </div>
          <div v-else class="application-status">
            <div class="status-info">
              <el-tag :type="getStatusType(supplierApplication.status)">
                {{ getStatusText(supplierApplication.status) }}
              </el-tag>
              <p>申请时间：{{ formatDate(supplierApplication.createdTime) }}</p>
              <p v-if="supplierApplication.reviewComment">审核意见：{{ supplierApplication.reviewComment }}</p>
            </div>
          </div>
        </div>
      </el-card>

      <!-- 功能菜单 -->
      <el-card class="menu-card">
        <template #header>
          <div class="card-header">
            <span>功能菜单</span>
          </div>
        </template>
        <div class="menu-grid">
          <div class="menu-item" @click="goToUpgrade">
            <i class="el-icon-star-on"></i>
            <span>会员升级</span>
          </div>
          <div class="menu-item" @click="goToFavorites">
            <i class="el-icon-star"></i>
            <span>我的收藏</span>
          </div>
          <div class="menu-item" @click="goToInquiries">
            <i class="el-icon-chat-line-round"></i>
            <span>我的询价</span>
          </div>
          <div class="menu-item" @click="goToSettings">
            <i class="el-icon-setting"></i>
            <span>账户设置</span>
          </div>
        </div>
      </el-card>
    </div>

    <!-- 供应商申请对话框 -->
    <el-dialog v-model="applicationDialog.visible" title="申请成为供应商" width="800px" destroy-on-close>
      <el-form :model="applicationDialog.form" label-width="120px" :rules="applicationRules" ref="applicationForm">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="公司名称" prop="companyName">
              <el-input v-model="applicationDialog.form.companyName" placeholder="请输入公司名称" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="公司简称" prop="shortName">
              <el-input v-model="applicationDialog.form.shortName" placeholder="请输入公司简称" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="统一社会信用代码" prop="creditCode">
              <el-input v-model="applicationDialog.form.creditCode" placeholder="请输入统一社会信用代码" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="营业执照号" prop="businessLicense">
              <el-input v-model="applicationDialog.form.businessLicense" placeholder="请输入营业执照号" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="法定代表人" prop="legalPerson">
              <el-input v-model="applicationDialog.form.legalPerson" placeholder="请输入法定代表人" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="注册资本" prop="registeredCapital">
              <el-input v-model="applicationDialog.form.registeredCapital" placeholder="请输入注册资本" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="成立日期" prop="establishedDate">
          <el-date-picker
            v-model="applicationDialog.form.establishedDate"
            type="date"
            placeholder="请选择成立日期"
            value-format="YYYY-MM-DD"
          />
        </el-form-item>

        <el-form-item label="经营范围" prop="businessScope">
          <el-input
            v-model="applicationDialog.form.businessScope"
            type="textarea"
            :rows="3"
            placeholder="请输入经营范围"
          />
        </el-form-item>

        <el-form-item label="公司地址" prop="address">
          <el-input
            v-model="applicationDialog.form.address"
            type="textarea"
            :rows="2"
            placeholder="请输入公司地址"
          />
        </el-form-item>

        <el-row :gutter="20">
          <el-col :span="8">
            <el-form-item label="联系人姓名" prop="contactName">
              <el-input v-model="applicationDialog.form.contactName" placeholder="请输入联系人姓名" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="联系人电话" prop="contactPhone">
              <el-input v-model="applicationDialog.form.contactPhone" placeholder="请输入联系人电话" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="联系人邮箱" prop="contactEmail">
              <el-input v-model="applicationDialog.form.contactEmail" placeholder="请输入联系人邮箱" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="申请材料">
          <el-upload
            v-model:file-list="applicationDialog.fileList"
            action="/api/upload"
            multiple
            :limit="5"
            :on-success="handleUploadSuccess"
            :on-remove="handleUploadRemove"
          >
            <el-button type="primary">上传文件</el-button>
            <template #tip>
              <div class="el-upload__tip">
                支持上传营业执照、税务登记证等材料，最多5个文件
              </div>
            </template>
          </el-upload>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="applicationDialog.visible = false">取消</el-button>
        <el-button type="primary" @click="submitApplication" :loading="applicationDialog.loading">
          提交申请
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { useRouter } from 'vue-router'
import { SupplierApplicationApi } from '/@/api/client/SupplierApplication'
import { MemberLevelApi } from '/@/api/client/MemberLevel'

const router = useRouter()

const state = reactive({
  userInfo: {
    nickName: '',
    userName: '',
    avatar: '',
    createdTime: '',
    currentLevel: null as any,
    isSupplier: false
  },
  supplierApplication: null as any
})

const applicationDialog = reactive({
  visible: false,
  loading: false,
  form: {
    companyName: '',
    shortName: '',
    creditCode: '',
    businessLicense: '',
    legalPerson: '',
    registeredCapital: '',
    establishedDate: '',
    businessScope: '',
    address: '',
    contactName: '',
    contactPhone: '',
    contactEmail: '',
    materials: [] as string[]
  },
  fileList: [] as any[]
})

const applicationRules = {
  companyName: [
    { required: true, message: '请输入公司名称', trigger: 'blur' }
  ],
  contactName: [
    { required: true, message: '请输入联系人姓名', trigger: 'blur' }
  ],
  contactPhone: [
    { required: true, message: '请输入联系人电话', trigger: 'blur' }
  ]
}

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

// 获取等级样式类
const getLevelClass = (level: string) => {
  const classes: Record<string, string> = {
    'Free': 'level-free',
    'Basic': 'level-basic',
    'Standard': 'level-standard',
    'Premium': 'level-premium',
    'Enterprise': 'level-enterprise'
  }
  return classes[level] || 'level-free'
}

// 获取状态类型
const getStatusType = (status: string) => {
  const types: Record<string, string> = {
    'pending': 'warning',
    'approved': 'success',
    'rejected': 'danger'
  }
  return types[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string) => {
  const texts: Record<string, string> = {
    'pending': '审核中',
    'approved': '已通过',
    'rejected': '已拒绝'
  }
  return texts[status] || status
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return ''
  return new Date(date).toLocaleDateString()
}

// 打开申请对话框
const openApplicationDialog = () => {
  applicationDialog.visible = true
  applicationDialog.form = {
    companyName: '',
    shortName: '',
    creditCode: '',
    businessLicense: '',
    legalPerson: '',
    registeredCapital: '',
    establishedDate: '',
    businessScope: '',
    address: '',
    contactName: '',
    contactPhone: '',
    contactEmail: '',
    materials: []
  }
  applicationDialog.fileList = []
}

// 提交申请
const submitApplication = async () => {
  try {
    applicationDialog.loading = true
    
    const api = new SupplierApplicationApi()
    const res = await api.add({
      ...applicationDialog.form,
      materials: applicationDialog.fileList.map(file => file.response?.url || file.url)
    })

    if (res?.success) {
      ElMessage.success('申请提交成功，请等待审核')
      applicationDialog.visible = false
      fetchSupplierApplication()
    } else {
      ElMessage.error(res?.message || '提交失败')
    }
  } catch (error) {
    ElMessage.error('提交失败')
  } finally {
    applicationDialog.loading = false
  }
}

// 文件上传成功
const handleUploadSuccess = (response: any, file: any) => {
  // 处理上传成功
}

// 文件移除
const handleUploadRemove = (file: any) => {
  // 处理文件移除
}

// 获取供应商申请
const fetchSupplierApplication = async () => {
  try {
    const api = new SupplierApplicationApi()
    const res = await api.getMyApplication()
    state.supplierApplication = res?.data
  } catch (error) {
    console.error('获取供应商申请失败:', error)
  }
}

// 获取用户信息
const fetchUserInfo = async () => {
  try {
    // 这里应该调用用户信息接口
    // 为了演示，使用模拟数据
    state.userInfo = {
      nickName: '测试用户',
      userName: 'test@example.com',
      avatar: '',
      createdTime: '2024-01-01',
      currentLevel: {
        level: 'Basic',
        productDataLimit: 100,
        supplierDataLimit: 100,
        showFullContactInfo: true
      },
      isSupplier: false
    }
  } catch (error) {
    console.error('获取用户信息失败:', error)
  }
}

// 跳转到升级页面
const goToUpgrade = () => {
  router.push('/client/member-upgrade')
}

// 跳转到收藏页面
const goToFavorites = () => {
  router.push('/client/favorites')
}

// 跳转到询价页面
const goToInquiries = () => {
  router.push('/client/inquiries')
}

// 跳转到设置页面
const goToSettings = () => {
  router.push('/client/settings')
}

onMounted(() => {
  fetchUserInfo()
  fetchSupplierApplication()
})
</script>

<style scoped>
.personal-center {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.center-header {
  margin-bottom: 30px;
}

.center-header h1 {
  font-size: 28px;
  color: #333;
}

.center-content {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 20px;
}

.user-info-card,
.level-card,
.supplier-card,
.menu-card {
  height: fit-content;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 20px;
}

.user-details h3 {
  margin-bottom: 8px;
  color: #333;
}

.user-details p {
  margin-bottom: 4px;
  color: #666;
  font-size: 14px;
}

.level-info {
  text-align: center;
}

.level-badge {
  display: inline-block;
  padding: 8px 16px;
  border-radius: 20px;
  color: white;
  font-weight: bold;
  margin-bottom: 16px;
}

.level-free {
  background: #909399;
}

.level-basic {
  background: #409eff;
}

.level-standard {
  background: #67c23a;
}

.level-premium {
  background: #e6a23c;
}

.level-enterprise {
  background: #f56c6c;
}

.level-features {
  text-align: left;
}

.feature-item {
  display: flex;
  align-items: center;
  margin-bottom: 8px;
  font-size: 14px;
  color: #666;
}

.feature-item i {
  color: #67c23a;
  margin-right: 8px;
}

.supplier-info {
  text-align: center;
}

.no-application p {
  margin-bottom: 16px;
  color: #666;
}

.application-status {
  text-align: left;
}

.status-info p {
  margin-bottom: 8px;
  color: #666;
  font-size: 14px;
}

.menu-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

.menu-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
  border: 1px solid #e4e7ed;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s;
}

.menu-item:hover {
  border-color: #409eff;
  background: #f0f9ff;
}

.menu-item i {
  font-size: 24px;
  color: #409eff;
  margin-bottom: 8px;
}

.menu-item span {
  font-size: 14px;
  color: #333;
}
</style>
