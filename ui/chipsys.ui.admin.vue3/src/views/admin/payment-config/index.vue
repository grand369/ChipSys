<template>
  <div class="payment-config">
    <div class="page-header">
      <h1>支付配置管理</h1>
      <el-button type="primary" @click="openAddDialog">
        添加配置
      </el-button>
    </div>

    <div class="config-list">
      <el-card v-for="config in configList" :key="config.id" class="config-card">
        <template #header>
          <div class="card-header">
            <div class="config-info">
              <h3>{{ getPaymentMethodName(config.paymentMethod) }}</h3>
              <el-tag :type="config.enabled ? 'success' : 'danger'">
                {{ config.enabled ? '已启用' : '已禁用' }}
              </el-tag>
            </div>
            <div class="config-actions">
              <el-button size="small" @click="viewConfig(config)">查看</el-button>
              <el-button size="small" type="primary" @click="editConfig(config)">编辑</el-button>
              <el-button size="small" type="success" @click="testConfig(config)">测试</el-button>
              <el-button 
                size="small" 
                :type="config.enabled ? 'warning' : 'success'"
                @click="toggleConfig(config)"
              >
                {{ config.enabled ? '禁用' : '启用' }}
              </el-button>
              <el-button size="small" type="danger" @click="deleteConfig(config)">删除</el-button>
            </div>
          </div>
        </template>

        <div class="config-details">
          <div class="detail-row">
            <span class="label">配置名称：</span>
            <span>{{ config.configName }}</span>
          </div>
          <div class="detail-row">
            <span class="label">应用ID：</span>
            <span>{{ config.appId }}</span>
          </div>
          <div class="detail-row">
            <span class="label">商户号：</span>
            <span>{{ config.merchantId }}</span>
          </div>
          <div class="detail-row">
            <span class="label">环境：</span>
            <el-tag :type="config.environment === 'production' ? 'success' : 'warning'">
              {{ config.environment === 'production' ? '生产环境' : '沙箱环境' }}
            </el-tag>
          </div>
          <div class="detail-row">
            <span class="label">回调地址：</span>
            <span>{{ config.notifyUrl }}</span>
          </div>
          <div v-if="config.remark" class="detail-row">
            <span class="label">备注：</span>
            <span>{{ config.remark }}</span>
          </div>
        </div>
      </el-card>
    </div>

    <!-- 配置对话框 -->
    <el-dialog v-model="configDialog.visible" :title="configDialog.type === 'add' ? '添加支付配置' : '编辑支付配置'" width="800px" destroy-on-close>
      <el-form :model="configDialog.form" label-width="120px" :rules="configRules" ref="configForm">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="支付方式" prop="paymentMethod">
              <el-select v-model="configDialog.form.paymentMethod" placeholder="请选择支付方式">
                <el-option label="支付宝" value="alipay" />
                <el-option label="微信支付" value="wechat" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="配置名称" prop="configName">
              <el-input v-model="configDialog.form.configName" placeholder="请输入配置名称" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="应用ID" prop="appId">
              <el-input v-model="configDialog.form.appId" placeholder="请输入应用ID" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="商户号" prop="merchantId">
              <el-input v-model="configDialog.form.merchantId" placeholder="请输入商户号" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="私钥" prop="privateKey">
          <el-input
            v-model="configDialog.form.privateKey"
            type="textarea"
            :rows="4"
            placeholder="请输入私钥"
          />
        </el-form-item>

        <el-form-item label="公钥" prop="publicKey">
          <el-input
            v-model="configDialog.form.publicKey"
            type="textarea"
            :rows="4"
            placeholder="请输入公钥"
          />
        </el-form-item>

        <el-form-item v-if="configDialog.form.paymentMethod === 'alipay'" label="支付宝公钥" prop="alipayPublicKey">
          <el-input
            v-model="configDialog.form.alipayPublicKey"
            type="textarea"
            :rows="4"
            placeholder="请输入支付宝公钥"
          />
        </el-form-item>

        <el-form-item v-if="configDialog.form.paymentMethod === 'wechat'" label="微信支付密钥" prop="wechatSecret">
          <el-input v-model="configDialog.form.wechatSecret" placeholder="请输入微信支付密钥" />
        </el-form-item>

        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="回调地址" prop="notifyUrl">
              <el-input v-model="configDialog.form.notifyUrl" placeholder="请输入回调地址" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="返回地址" prop="returnUrl">
              <el-input v-model="configDialog.form.returnUrl" placeholder="请输入返回地址" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="环境" prop="environment">
              <el-select v-model="configDialog.form.environment" placeholder="请选择环境">
                <el-option label="沙箱环境" value="sandbox" />
                <el-option label="生产环境" value="production" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="是否启用">
              <el-switch v-model="configDialog.form.enabled" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="备注">
          <el-input
            v-model="configDialog.form.remark"
            type="textarea"
            :rows="2"
            placeholder="请输入备注"
          />
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="configDialog.visible = false">取消</el-button>
        <el-button type="primary" @click="submitConfig" :loading="configDialog.loading">
          确定
        </el-button>
      </template>
    </el-dialog>

    <!-- 查看配置对话框 -->
    <el-dialog v-model="viewDialog.visible" title="配置详情" width="600px">
      <div v-if="viewDialog.data" class="config-view">
        <el-descriptions :column="2" border>
          <el-descriptions-item label="支付方式">{{ getPaymentMethodName(viewDialog.data.paymentMethod) }}</el-descriptions-item>
          <el-descriptions-item label="配置名称">{{ viewDialog.data.configName }}</el-descriptions-item>
          <el-descriptions-item label="应用ID">{{ viewDialog.data.appId }}</el-descriptions-item>
          <el-descriptions-item label="商户号">{{ viewDialog.data.merchantId }}</el-descriptions-item>
          <el-descriptions-item label="环境">
            <el-tag :type="viewDialog.data.environment === 'production' ? 'success' : 'warning'">
              {{ viewDialog.data.environment === 'production' ? '生产环境' : '沙箱环境' }}
            </el-tag>
          </el-descriptions-item>
          <el-descriptions-item label="状态">
            <el-tag :type="viewDialog.data.enabled ? 'success' : 'danger'">
              {{ viewDialog.data.enabled ? '已启用' : '已禁用' }}
            </el-tag>
          </el-descriptions-item>
          <el-descriptions-item label="回调地址">{{ viewDialog.data.notifyUrl }}</el-descriptions-item>
          <el-descriptions-item label="返回地址">{{ viewDialog.data.returnUrl }}</el-descriptions-item>
          <el-descriptions-item v-if="viewDialog.data.remark" label="备注" :span="2">{{ viewDialog.data.remark }}</el-descriptions-item>
        </el-descriptions>
      </div>

      <template #footer>
        <el-button @click="viewDialog.visible = false">关闭</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { PaymentConfigApi } from '/@/api/admin/PaymentConfig'

const state = reactive({
  configList: [] as any[]
})

const configDialog = reactive({
  visible: false,
  type: 'add' as 'add' | 'edit',
  loading: false,
  form: {
    id: 0,
    paymentMethod: '',
    configName: '',
    appId: '',
    merchantId: '',
    privateKey: '',
    publicKey: '',
    alipayPublicKey: '',
    wechatSecret: '',
    notifyUrl: '',
    returnUrl: '',
    environment: 'sandbox',
    enabled: true,
    remark: ''
  }
})

const viewDialog = reactive({
  visible: false,
  data: null as any
})

const configRules = {
  paymentMethod: [
    { required: true, message: '请选择支付方式', trigger: 'change' }
  ],
  configName: [
    { required: true, message: '请输入配置名称', trigger: 'blur' }
  ],
  appId: [
    { required: true, message: '请输入应用ID', trigger: 'blur' }
  ],
  merchantId: [
    { required: true, message: '请输入商户号', trigger: 'blur' }
  ],
  privateKey: [
    { required: true, message: '请输入私钥', trigger: 'blur' }
  ],
  publicKey: [
    { required: true, message: '请输入公钥', trigger: 'blur' }
  ],
  notifyUrl: [
    { required: true, message: '请输入回调地址', trigger: 'blur' }
  ]
}

// 获取支付方式名称
const getPaymentMethodName = (method: string) => {
  const names: Record<string, string> = {
    'alipay': '支付宝',
    'wechat': '微信支付'
  }
  return names[method] || method
}

// 获取配置列表
const fetchConfigs = async () => {
  try {
    const api = new PaymentConfigApi()
    const res = await api.getConfigs()
    
    if (res?.success) {
      state.configList = res.data || []
    }
  } catch (error) {
    ElMessage.error('获取配置列表失败')
  }
}

// 打开添加对话框
const openAddDialog = () => {
  configDialog.type = 'add'
  configDialog.form = {
    id: 0,
    paymentMethod: '',
    configName: '',
    appId: '',
    merchantId: '',
    privateKey: '',
    publicKey: '',
    alipayPublicKey: '',
    wechatSecret: '',
    notifyUrl: '',
    returnUrl: '',
    environment: 'sandbox',
    enabled: true,
    remark: ''
  }
  configDialog.visible = true
}

// 编辑配置
const editConfig = (config: any) => {
  configDialog.type = 'edit'
  configDialog.form = { ...config }
  configDialog.visible = true
}

// 查看配置
const viewConfig = (config: any) => {
  viewDialog.data = config
  viewDialog.visible = true
}

// 提交配置
const submitConfig = async () => {
  try {
    configDialog.loading = true
    
    const api = new PaymentConfigApi()
    let res
    
    if (configDialog.type === 'add') {
      res = await api.addConfig(configDialog.form)
    } else {
      res = await api.updateConfig(configDialog.form)
    }

    if (res?.success) {
      ElMessage.success(configDialog.type === 'add' ? '添加成功' : '更新成功')
      configDialog.visible = false
      fetchConfigs()
    } else {
      ElMessage.error(res?.message || '操作失败')
    }
  } catch (error) {
    ElMessage.error('操作失败')
  } finally {
    configDialog.loading = false
  }
}

// 测试配置
const testConfig = async (config: any) => {
  try {
    const api = new PaymentConfigApi()
    const res = await api.testConfig(config.id)
    
    if (res?.success && res.data) {
      ElMessage.success('配置测试通过')
    } else {
      ElMessage.error('配置测试失败')
    }
  } catch (error) {
    ElMessage.error('配置测试失败')
  }
}

// 切换配置状态
const toggleConfig = async (config: any) => {
  try {
    const api = new PaymentConfigApi()
    const res = await api.toggleConfig(config.id, !config.enabled)
    
    if (res?.success) {
      ElMessage.success(config.enabled ? '已禁用' : '已启用')
      fetchConfigs()
    } else {
      ElMessage.error(res?.message || '操作失败')
    }
  } catch (error) {
    ElMessage.error('操作失败')
  }
}

// 删除配置
const deleteConfig = async (config: any) => {
  try {
    await ElMessageBox.confirm('确定要删除这个配置吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })

    const api = new PaymentConfigApi()
    const res = await api.deleteConfig(config.id)
    
    if (res?.success) {
      ElMessage.success('删除成功')
      fetchConfigs()
    } else {
      ElMessage.error(res?.message || '删除失败')
    }
  } catch (error) {
    if (error !== 'cancel') {
      ElMessage.error('删除失败')
    }
  }
}

onMounted(() => {
  fetchConfigs()
})
</script>

<style scoped>
.payment-config {
  padding: 20px;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.page-header h1 {
  font-size: 24px;
  color: #333;
}

.config-list {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(600px, 1fr));
  gap: 20px;
}

.config-card {
  height: fit-content;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.config-info {
  display: flex;
  align-items: center;
  gap: 12px;
}

.config-info h3 {
  margin: 0;
  font-size: 16px;
  color: #333;
}

.config-actions {
  display: flex;
  gap: 8px;
}

.config-details {
  padding: 16px 0;
}

.detail-row {
  display: flex;
  margin-bottom: 8px;
  font-size: 14px;
}

.detail-row .label {
  font-weight: bold;
  color: #666;
  min-width: 80px;
}

.config-view {
  max-height: 500px;
  overflow-y: auto;
}
</style>
