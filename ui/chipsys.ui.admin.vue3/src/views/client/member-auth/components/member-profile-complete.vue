<template>
  <div class="member-profile-complete">
    <el-dialog
      v-model="visible"
      title="完善会员信息"
      width="600px"
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      :show-close="false"
    >
      <div class="profile-content">
        <div class="profile-header">
          <el-icon size="24" color="#409EFF"><User /></el-icon>
          <span>请完善您的个人信息，以便我们为您提供更好的服务</span>
        </div>

        <el-form
          ref="formRef"
          :model="form"
          :rules="rules"
          label-width="100px"
          class="profile-form"
        >
          <el-row :gutter="20">
            <el-col :span="12">
              <el-form-item label="昵称" prop="nickName">
                <el-input
                  v-model="form.nickName"
                  placeholder="请输入昵称"
                  maxlength="50"
                  clearable
                />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item label="真实姓名" prop="realName">
                <el-input
                  v-model="form.realName"
                  placeholder="请输入真实姓名"
                  maxlength="50"
                  clearable
                />
              </el-form-item>
            </el-col>
          </el-row>

          <el-row :gutter="20">
            <el-col :span="12">
              <el-form-item label="密码" prop="password">
                <el-input
                  v-model="form.password"
                  type="password"
                  placeholder="请设置登录密码"
                  maxlength="50"
                  show-password
                  clearable
                />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item label="确认密码" prop="confirmPassword">
                <el-input
                  v-model="form.confirmPassword"
                  type="password"
                  placeholder="请再次输入密码"
                  maxlength="50"
                  show-password
                  clearable
                />
              </el-form-item>
            </el-col>
          </el-row>

          <el-row :gutter="20">
            <el-col :span="12">
              <el-form-item label="身份证号" prop="idCard">
                <el-input
                  v-model="form.idCard"
                  placeholder="请输入身份证号"
                  maxlength="18"
                  clearable
                />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item label="性别" prop="gender">
                <el-select
                  v-model="form.gender"
                  placeholder="请选择性别"
                  style="width: 100%"
                  clearable
                >
                  <el-option label="男" value="Male" />
                  <el-option label="女" value="Female" />
                  <el-option label="其他" value="Other" />
                </el-select>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row :gutter="20">
            <el-col :span="8">
              <el-form-item label="省份" prop="province">
                <el-select
                  v-model="form.province"
                  placeholder="请选择省份"
                  style="width: 100%"
                  clearable
                  @change="onProvinceChange"
                >
                  <el-option
                    v-for="province in provinces"
                    :key="province.value"
                    :label="province.label"
                    :value="province.value"
                  />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="城市" prop="city">
                <el-select
                  v-model="form.city"
                  placeholder="请选择城市"
                  style="width: 100%"
                  clearable
                  :disabled="!form.province"
                  @change="onCityChange"
                >
                  <el-option
                    v-for="city in cities"
                    :key="city.value"
                    :label="city.label"
                    :value="city.value"
                  />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="区县" prop="district">
                <el-select
                  v-model="form.district"
                  placeholder="请选择区县"
                  style="width: 100%"
                  clearable
                  :disabled="!form.city"
                >
                  <el-option
                    v-for="district in districts"
                    :key="district.value"
                    :label="district.label"
                    :value="district.value"
                  />
                </el-select>
              </el-form-item>
            </el-col>
          </el-row>

          <el-form-item label="详细地址" prop="address">
            <el-input
              v-model="form.address"
              placeholder="请输入详细地址"
              maxlength="200"
              clearable
            />
          </el-form-item>
        </el-form>
      </div>

      <template #footer>
        <div class="dialog-footer">
          <el-button @click="skipComplete">跳过</el-button>
          <el-button
            type="primary"
            :loading="loading"
            @click="completeProfile"
          >
            完成
          </el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, computed, watch } from 'vue'
import { ElMessage } from 'element-plus'
import { User } from '@element-plus/icons-vue'
import { memberAuthApi } from '/@/api/client'
import type { FormInstance, FormRules } from 'element-plus'

// Props
interface Props {
  modelValue: boolean
  memberInfo?: any
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: false,
  memberInfo: null
})

// Emits
const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  'complete': [memberInfo: any]
  'skip': []
}>()

// 响应式数据
const formRef = ref<FormInstance>()
const loading = ref(false)
const visible = computed({
  get: () => props.modelValue,
  set: (value) => emit('update:modelValue', value)
})

// 表单数据
const form = reactive({
  nickName: '',
  password: '',
  confirmPassword: '',
  realName: '',
  idCard: '',
  gender: '',
  province: '',
  city: '',
  district: '',
  address: ''
})

// 地区数据
const provinces = ref([
  { label: '北京市', value: '北京市' },
  { label: '上海市', value: '上海市' },
  { label: '广东省', value: '广东省' },
  { label: '江苏省', value: '江苏省' },
  { label: '浙江省', value: '浙江省' },
  { label: '山东省', value: '山东省' },
  { label: '河南省', value: '河南省' },
  { label: '四川省', value: '四川省' },
  { label: '湖北省', value: '湖北省' },
  { label: '湖南省', value: '湖南省' }
])

const cities = ref<Array<{ label: string; value: string }>>([])
const districts = ref<Array<{ label: string; value: string }>>([])

// 表单验证规则
const rules: FormRules = {
  nickName: [
    { required: true, message: '请输入昵称', trigger: 'blur' },
    { min: 2, max: 50, message: '昵称长度在2到50个字符', trigger: 'blur' }
  ],
  password: [
    { min: 6, max: 50, message: '密码长度在6到50个字符', trigger: 'blur' }
  ],
  confirmPassword: [
    {
      validator: (rule, value, callback) => {
        if (form.password && value !== form.password) {
          callback(new Error('两次输入密码不一致'))
        } else {
          callback()
        }
      },
      trigger: 'blur'
    }
  ],
  idCard: [
    {
      pattern: /^[1-9]\d{5}(18|19|20)\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/,
      message: '身份证号格式不正确',
      trigger: 'blur'
    }
  ]
}

// 省份变化
const onProvinceChange = (province: string) => {
  form.city = ''
  form.district = ''
  
  // 模拟城市数据
  const cityMap: Record<string, Array<{ label: string; value: string }>> = {
    '北京市': [
      { label: '东城区', value: '东城区' },
      { label: '西城区', value: '西城区' },
      { label: '朝阳区', value: '朝阳区' },
      { label: '海淀区', value: '海淀区' }
    ],
    '上海市': [
      { label: '黄浦区', value: '黄浦区' },
      { label: '徐汇区', value: '徐汇区' },
      { label: '长宁区', value: '长宁区' },
      { label: '静安区', value: '静安区' }
    ],
    '广东省': [
      { label: '广州市', value: '广州市' },
      { label: '深圳市', value: '深圳市' },
      { label: '珠海市', value: '珠海市' },
      { label: '汕头市', value: '汕头市' }
    ]
  }
  
  cities.value = cityMap[province] || []
  districts.value = []
}

// 城市变化
const onCityChange = (city: string) => {
  form.district = ''
  
  // 模拟区县数据
  const districtMap: Record<string, Array<{ label: string; value: string }>> = {
    '广州市': [
      { label: '天河区', value: '天河区' },
      { label: '越秀区', value: '越秀区' },
      { label: '荔湾区', value: '荔湾区' },
      { label: '海珠区', value: '海珠区' }
    ],
    '深圳市': [
      { label: '福田区', value: '福田区' },
      { label: '罗湖区', value: '罗湖区' },
      { label: '南山区', value: '南山区' },
      { label: '宝安区', value: '宝安区' }
    ]
  }
  
  districts.value = districtMap[city] || []
}

// 完善信息
const completeProfile = async () => {
  if (!formRef.value) return

  try {
    await formRef.value.validate()
    loading.value = true

    await memberAuthApi.updateProfile({
      nickName: form.nickName,
      password: form.password || undefined,
      realName: form.realName,
      idCard: form.idCard,
      gender: form.gender,
      province: form.province,
      city: form.city,
      district: form.district,
      address: form.address
    })

    ElMessage.success('信息完善成功')
    emit('complete', { ...props.memberInfo, isProfileComplete: true })
    visible.value = false
  } catch (error: any) {
    ElMessage.error(error.message || '信息完善失败')
  } finally {
    loading.value = false
  }
}

// 跳过完善
const skipComplete = () => {
  emit('skip')
  visible.value = false
}

// 监听会员信息变化，初始化表单
watch(() => props.memberInfo, (memberInfo) => {
  if (memberInfo) {
    form.nickName = memberInfo.nickName || ''
    form.realName = memberInfo.realName || ''
    form.gender = memberInfo.gender || ''
    form.province = memberInfo.province || ''
    form.city = memberInfo.city || ''
    form.district = memberInfo.district || ''
    form.address = memberInfo.address || ''
  }
}, { immediate: true })
</script>

<style scoped>
.profile-content {
  padding: 20px 0;
}

.profile-header {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 30px;
  padding: 15px;
  background: #f0f9ff;
  border-radius: 8px;
  color: #409EFF;
  font-size: 14px;
}

.profile-form {
  max-height: 400px;
  overflow-y: auto;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

:deep(.el-form-item) {
  margin-bottom: 20px;
}

:deep(.el-input__wrapper) {
  border-radius: 6px;
}

:deep(.el-select) {
  width: 100%;
}

:deep(.el-dialog__body) {
  padding: 20px;
}

:deep(.el-dialog__footer) {
  padding: 20px;
  border-top: 1px solid #ebeef5;
}
</style>
