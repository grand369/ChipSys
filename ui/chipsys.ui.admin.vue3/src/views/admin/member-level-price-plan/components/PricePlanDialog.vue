<template>
  <el-dialog 
    :model-value="visible" 
    @update:model-value="handleVisibleChange"
    :title="isEdit ? '编辑价格方案' : '新增价格方案'" 
    width="600px" 
    destroy-on-close
    @close="handleClose"
  >
    <el-form :model="form" label-width="100px" size="default">
      <el-row :gutter="16">
        <el-col :span="12">
          <el-form-item label="会员等级" required>
            <el-select v-model="form.memberLevelId" placeholder="请选择会员等级" class="w-full">
              <el-option v-for="level in memberLevels" :key="level.id" :value="level.id" :label="level.levelName" />
            </el-select>
          </el-form-item>
        </el-col>
        
        <el-col :span="12">
          <el-form-item label="方案类型" required>
            <el-select v-model="form.planType" placeholder="请选择方案类型" class="w-full" @change="handlePlanTypeChange">
              <el-option v-for="option in PricePlanTypeOptions" :key="option.value" :value="option.value" :label="option.label" />
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      
      <el-row :gutter="16">
        <el-col :span="12">
          <el-form-item label="方案名称" required>
            <el-input v-model="form.planName" placeholder="请输入方案名称" />
          </el-form-item>
        </el-col>
        
        <el-col :span="12">
          <el-form-item label="排序">
            <el-input-number v-model="form.sort" :min="0" placeholder="数字越小排序越靠前" class="w-full" />
          </el-form-item>
        </el-col>
      </el-row>

      <!-- 价格信息 -->
      <el-divider content-position="left">价格信息</el-divider>
      
      <el-row :gutter="16">
        <el-col :span="8">
          <el-form-item label="原价（元）" required>
            <el-input-number 
              v-model="form.originalPrice" 
              :min="0" 
              :precision="2" 
              placeholder="请输入原价" 
              class="w-full"
              @input="calculateDiscount"
            />
          </el-form-item>
        </el-col>
        
        <el-col :span="8">
          <el-form-item label="折扣价（元）" required>
            <el-input-number 
              v-model="form.discountedPrice" 
              :min="0" 
              :precision="2" 
              placeholder="请输入折扣后价格" 
              class="w-full"
              @input="calculateDiscountFromPrice"
            />
          </el-form-item>
        </el-col>
        
        <el-col :span="8">
          <el-form-item label="折扣百分比">
            <el-input-number 
              v-model="form.discountPercent" 
              :min="0" 
              :max="100" 
              :precision="2" 
              placeholder="自动计算" 
              readonly
              class="w-full"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <!-- 其他设置 -->
      <el-divider content-position="left">其他设置</el-divider>
      
      <el-form-item label="方案描述">
        <el-input 
          v-model="form.description" 
          type="textarea" 
          :rows="3"
          placeholder="请输入方案描述（可选）"
        />
      </el-form-item>
      
      <el-row :gutter="16">
        <el-col :span="12">
          <el-form-item label="推荐方案">
            <el-switch v-model="form.isRecommended" />
          </el-form-item>
        </el-col>
        
        <el-col :span="12">
          <el-form-item label="启用">
            <el-switch v-model="form.enabled" />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSubmit">
        {{ isEdit ? '更新' : '添加' }}
      </el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, reactive, computed, watch } from 'vue'
import { ElMessage } from 'element-plus'
import { PricePlanTypeOptions, type MemberLevelPricePlanAddInput, type MemberLevelPricePlanUpdateInput, type MemberLevelGetOutput } from '/@/api/admin/MemberLevelPricePlan'

// Props
interface Props {
  visible: boolean
  formData: MemberLevelPricePlanAddInput | MemberLevelPricePlanUpdateInput | null
  memberLevels: MemberLevelGetOutput[]
}

const props = defineProps<Props>()

// Emits
const emit = defineEmits<{
  close: []
  submit: [data: MemberLevelPricePlanAddInput | MemberLevelPricePlanUpdateInput]
}>()

// 响应式数据
const form = reactive<MemberLevelPricePlanAddInput>({
  memberLevelId: 0,
  planType: 'Monthly',
  planName: '',
  originalPrice: 0,
  discountedPrice: 0,
  discountPercent: 0,
  description: '',
  isRecommended: false,
  enabled: true,
  sort: 0
})

// 计算属性
const isEdit = computed(() => {
  return props.formData && 'id' in props.formData
})

// 监听表单数据变化
watch(() => props.formData, (newData) => {
  if (newData) {
    Object.assign(form, newData)
  } else {
    resetForm()
  }
}, { immediate: true })

// 方法
const resetForm = () => {
  Object.assign(form, {
    memberLevelId: 0,
    planType: 'Monthly',
    planName: '',
    originalPrice: 0,
    discountedPrice: 0,
    discountPercent: 0,
    description: '',
    isRecommended: false,
    enabled: true,
    sort: 0
  })
}

const handlePlanTypeChange = () => {
  // 根据方案类型自动设置方案名称
  const option = PricePlanTypeOptions.find(opt => opt.value === form.planType)
  if (option) {
    form.planName = option.label
  }
}

const calculateDiscount = () => {
  if (form.originalPrice > 0 && form.discountedPrice > 0) {
    const discount = ((form.originalPrice - form.discountedPrice) / form.originalPrice) * 100
    form.discountPercent = Math.round(discount * 100) / 100
  }
}

const calculateDiscountFromPrice = () => {
  if (form.originalPrice > 0 && form.discountedPrice > 0) {
    const discount = ((form.originalPrice - form.discountedPrice) / form.originalPrice) * 100
    form.discountPercent = Math.round(discount * 100) / 100
  }
}

const handleVisibleChange = (value: boolean) => {
  if (!value) {
    handleClose()
  }
}

const handleClose = () => {
  emit('close')
}

const handleSubmit = () => {
  // 验证表单
  if (!form.memberLevelId) {
    ElMessage.error('请选择会员等级')
    return
  }
  
  if (!form.planType) {
    ElMessage.error('请选择方案类型')
    return
  }
  
  if (!form.planName.trim()) {
    ElMessage.error('请输入方案名称')
    return
  }
  
  if (form.originalPrice < 0) {
    ElMessage.error('原价不能小于0')
    return
  }
  
  if (form.discountedPrice < 0) {
    ElMessage.error('折扣后价格不能小于0')
    return
  }
  
  if (form.discountedPrice > form.originalPrice) {
    ElMessage.error('折扣后价格不能大于原价')
    return
  }

  // 提交表单
  const submitData = isEdit.value 
    ? { ...form, id: (props.formData as MemberLevelPricePlanUpdateInput).id }
    : form

  emit('submit', submitData)
}
</script>
