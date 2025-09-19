<template>
  <el-dialog 
    :model-value="visible" 
    @update:model-value="handleVisibleChange"
    title="批量设置价格方案" 
    width="800px" 
    destroy-on-close
    @close="handleClose"
  >
    <el-form :model="form" label-width="100px" size="default">
      <el-form-item label="会员等级" required>
        <el-select v-model="form.memberLevelId" placeholder="请选择会员等级" class="w-full">
          <el-option v-for="level in props.memberLevels" :key="level.id" :value="level.id" :label="level.levelName" />
        </el-select>
      </el-form-item>

      <!-- 价格方案列表 -->
      <el-divider content-position="left">
        <span>价格方案配置</span>
        <el-button type="primary" size="small" @click="addPricePlan" class="ml-4">
          <i class="fas fa-plus mr-1"></i>
          添加方案
        </el-button>
      </el-divider>

      <div v-if="form.pricePlans.length > 0" class="space-y-4">
        <el-card 
          v-for="(plan, index) in form.pricePlans" 
          :key="index"
          class="price-plan-card"
        >
          <template #header>
            <div class="flex justify-between items-center">
              <span>方案 {{ index + 1 }}</span>
              <el-button type="danger" size="small" text @click="removePricePlan(index)">
                <i class="fas fa-trash"></i>
              </el-button>
            </div>
          </template>

          <el-row :gutter="16">
            <el-col :span="6">
              <el-form-item label="方案类型" required>
                <el-select v-model="plan.planType" placeholder="请选择" @change="handlePlanTypeChange(plan)">
                  <el-option v-for="option in PricePlanTypeOptions" :key="option.value" :value="option.value" :label="option.label" />
                </el-select>
              </el-form-item>
            </el-col>
            
            <el-col :span="6">
              <el-form-item label="方案名称" required>
                <el-input v-model="plan.planName" placeholder="方案名称" />
              </el-form-item>
            </el-col>
            
            <el-col :span="6">
              <el-form-item label="原价（元）" required>
                <el-input-number 
                  v-model="plan.originalPrice" 
                  :min="0" 
                  :precision="2" 
                  placeholder="原价"
                  @input="calculateDiscount(plan)"
                />
              </el-form-item>
            </el-col>
            
            <el-col :span="6">
              <el-form-item label="折扣价（元）" required>
                <el-input-number 
                  v-model="plan.discountedPrice" 
                  :min="0" 
                  :precision="2" 
                  placeholder="折扣价"
                  @input="calculateDiscountFromPrice(plan)"
                />
              </el-form-item>
            </el-col>
          </el-row>
          
          <el-row :gutter="16">
            <el-col :span="6">
              <el-form-item label="折扣百分比">
                <el-input-number 
                  v-model="plan.discountPercent" 
                  :min="0" 
                  :max="100" 
                  :precision="2" 
                  placeholder="自动计算"
                  readonly
                />
              </el-form-item>
            </el-col>
            
            <el-col :span="6">
              <el-form-item label="排序">
                <el-input-number v-model="plan.sort" :min="0" placeholder="排序" />
              </el-form-item>
            </el-col>
            
            <el-col :span="6">
              <el-form-item label="推荐方案">
                <el-switch v-model="plan.isRecommended" />
              </el-form-item>
            </el-col>
            
            <el-col :span="6">
              <el-form-item label="启用">
                <el-switch v-model="plan.enabled" />
              </el-form-item>
            </el-col>
          </el-row>
          
          <el-form-item label="方案描述">
            <el-input 
              v-model="plan.description" 
              type="textarea" 
              :rows="2"
              placeholder="请输入方案描述（可选）"
            />
          </el-form-item>
        </el-card>
      </div>

      <!-- 空状态 -->
      <el-empty v-if="form.pricePlans.length === 0" description="暂无价格方案，点击'添加方案'开始配置" />
    </el-form>
    
    <template #footer>
      <el-button @click="handleClose">取消</el-button>
      <el-button type="primary" @click="handleSubmit">批量设置</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { ElMessage } from 'element-plus'
import { PricePlanTypeOptions, type MemberLevelPricePlanAddInput, type MemberLevelGetOutput, type BatchSetPricePlansInput } from '/@/api/admin/MemberLevelPricePlan'

// Props
interface Props {
  visible: boolean
  memberLevels: MemberLevelGetOutput[]
}

const props = defineProps<Props>()

// Emits
const emit = defineEmits<{
  close: []
  submit: [data: BatchSetPricePlansInput]
}>()

// 响应式数据
const form = reactive<BatchSetPricePlansInput>({
  memberLevelId: 0,
  pricePlans: []
})

// 方法
const addPricePlan = () => {
  form.pricePlans.push({
    memberLevelId: form.memberLevelId,
    planType: 'Monthly',
    planName: '',
    originalPrice: 0,
    discountedPrice: 0,
    discountPercent: 0,
    description: '',
    isRecommended: false,
    enabled: true,
    sort: form.pricePlans.length
  })
}

const removePricePlan = (index: number) => {
  form.pricePlans.splice(index, 1)
  // 重新计算排序
  form.pricePlans.forEach((plan, idx) => {
    plan.sort = idx
  })
}

const handlePlanTypeChange = (plan: MemberLevelPricePlanAddInput) => {
  // 根据方案类型自动设置方案名称
  const option = PricePlanTypeOptions.find(opt => opt.value === plan.planType)
  if (option) {
    plan.planName = option.label
  }
}

const calculateDiscount = (plan: MemberLevelPricePlanAddInput) => {
  if (plan.originalPrice > 0 && plan.discountedPrice > 0) {
    const discount = ((plan.originalPrice - plan.discountedPrice) / plan.originalPrice) * 100
    plan.discountPercent = Math.round(discount * 100) / 100
  }
}

const calculateDiscountFromPrice = (plan: MemberLevelPricePlanAddInput) => {
  if (plan.originalPrice > 0 && plan.discountedPrice > 0) {
    const discount = ((plan.originalPrice - plan.discountedPrice) / plan.originalPrice) * 100
    plan.discountPercent = Math.round(discount * 100) / 100
  }
}

const handleVisibleChange = (value: boolean) => {
  if (!value) {
    handleClose()
  }
}

const handleClose = () => {
  // 重置表单
  form.memberLevelId = 0
  form.pricePlans = []
  emit('close')
}

const handleSubmit = () => {
  // 验证表单
  if (!form.memberLevelId) {
    ElMessage.error('请选择会员等级')
    return
  }
  
  if (form.pricePlans.length === 0) {
    ElMessage.error('请至少添加一个价格方案')
    return
  }

  // 验证每个价格方案
  for (let i = 0; i < form.pricePlans.length; i++) {
    const plan = form.pricePlans[i]
    
    if (!plan.planType) {
      ElMessage.error(`方案 ${i + 1}：请选择方案类型`)
      return
    }
    
    if (!plan.planName.trim()) {
      ElMessage.error(`方案 ${i + 1}：请输入方案名称`)
      return
    }
    
    if (plan.originalPrice < 0) {
      ElMessage.error(`方案 ${i + 1}：原价不能小于0`)
      return
    }
    
    if (plan.discountedPrice < 0) {
      ElMessage.error(`方案 ${i + 1}：折扣后价格不能小于0`)
      return
    }
    
    if (plan.discountedPrice > plan.originalPrice) {
      ElMessage.error(`方案 ${i + 1}：折扣后价格不能大于原价`)
      return
    }
  }

  // 提交表单
  emit('submit', { ...form })
}
</script>

<style scoped>
.price-plan-card {
  margin-bottom: 16px;
}

.space-y-4 > * + * {
  margin-top: 16px;
}
</style>