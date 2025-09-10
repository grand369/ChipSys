<template>
  <div v-if="searchItems.length > 0">
    <el-form ref="formRef" :model="formState" :inline="true" label-width="auto">
      <el-row :gutter="16">
        <!-- 动态渲染表单项 -->
        <el-col
          v-for="(item, index) in searchItems"
          v-show="index < displayCount || isExpanded"
          :key="index"
          :xs="col.xs"
          :sm="col.sm"
          :md="col.md"
          :lg="col.lg"
          :xl="col.xl"
          class="w100"
        >
          <el-form-item :label="item.label" :prop="item.field" :rules="item.rules" class="w100">
            <component :is="item.componentName" v-model="formState[item.field]" clearable v-bind="item.attrs" class="w100" />
          </el-form-item>
        </el-col>

        <!-- 操作按钮区域 -->
        <el-col :xs="col.xs" :sm="col.sm" :md="col.md" :lg="col.lg" :xl="col.xl">
          <el-form-item label-width="0px">
            <div class="my-flex">
              <el-button v-if="showToggle" type="primary" link @click="onToggleExpanded">
                {{ isExpanded ? '收起' : '展开' }}
                <SvgIcon :name="isExpanded ? 'ele-ArrowUp' : 'ele-ArrowDown'" />
              </el-button>
              <el-button type="primary" @click="onSearch">查询</el-button>
              <el-button icon="ele-RefreshLeft" text bg @click="onReset">重置</el-button>
            </div>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </div>
</template>

<script setup lang="ts" name="coms/my-search">
import { reactive, ref, computed, PropType } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { cloneDeep, isInteger, mergeWith } from 'lodash-es'

interface SearchItem {
  label: string
  field: string
  operator: OperatorEnum
  rules?: FormRules
  componentName: string
  attrs?: EmptyObjectType
  defaultValue?: any
}

type BreakpointKey = 'xs' | 'sm' | 'md' | 'lg' | 'xl'

type ColConfigType = {
  [key in BreakpointKey]?: number
}

// 默认�?colConfig
const defaultColConfig = {
  xs: 24,
  sm: 12,
  md: 12,
  lg: 8,
  xl: 6,
} as ColConfigType

// 默认显示数量
const DEFAULT_VISIBLE_COUNT = 3

// 定义父组件传过来的�?
const props = defineProps({
  // 查询条件列表
  searchItems: {
    type: Array<SearchItem>,
    default: () => [],
  },
  // 显示查询条件数量
  displayCount: {
    type: Number,
    default: () => 3,
  },
  // 是否过滤空�?
  isFilterEmptyValue: {
    type: Boolean,
    default: () => true,
  },
  // 是否重置查询
  isResetSearch: {
    type: Boolean,
    default: () => true,
  },
  colConfig: {
    type: Object as PropType<ColConfigType>,
    validator: (value: ColConfigType) => {
      const invalidEntries = Object.entries(value).filter(([k, v]) => {
        if (!['xs', 'sm', 'md', 'lg', 'xl'].includes(k)) {
          console.warn(`无效的断点配�? ${k}`)
          return true
        }
        if (!isInteger(v)) {
          console.warn(`非整数�? ${k}=${v} (类型: ${typeof v})`)
          return true
        }
        if (v < 1 || v > 24) {
          console.warn(`超出范围: ${k}=${v} (允许范围: 1-24)`)
          return true
        }
        return false
      })

      return invalidEntries.length === 0
    },
  },
})

// 定义子组件向父组件传�?事件
const emit = defineEmits(['search'])

const isExpanded = ref(false)
const formRef = ref<FormInstance>()

// 表单初始�?
const formState = reactive<EmptyObjectType>(
  props.searchItems.reduce((acc: EmptyObjectType, item) => {
    acc[item.field] = item.defaultValue ?? ''
    return acc
  }, {})
)

const col = computed(() => {
  return mergeWith({}, defaultColConfig, props.colConfig)
})

// 处理可见数量配置
const displayCount = computed(() => {
  // 验证配置有效�?
  const count = props.displayCount ?? DEFAULT_VISIBLE_COUNT
  return Math.max(1, Math.min(count, props.searchItems.length))
})

// 计算剩余项数�?
const remainingCount = computed(() => props.searchItems.length - displayCount.value)

// 是否需要显示展开按钮
const showToggle = computed(() => props.searchItems.length > displayCount.value && remainingCount.value > 0)

// 空值判断函�?
const isEmptyValue = (value: any) => {
  return value === '' || value === null || value === undefined
}

// 查询
const onSearch = async () => {
  try {
    await formRef.value?.validate()
    const filters = Object.entries(formState)
      // 根据配置过滤空�?
      .filter(([_, value]) => !props.isFilterEmptyValue || !isEmptyValue(value))
      .map(([field, value]) => ({
        field,
        operator: props.searchItems.find((item) => item.field === field)?.operator || 'Equal',
        value,
      }))

    emit('search', cloneDeep(formState), { logic: 'And', filters })
  } catch (error) {
    console.error('表单验证失败:', error)
  }
}

// 重置
const onReset = () => {
  formRef.value?.resetFields()
  props.searchItems.forEach((item) => {
    formState[item.field] = item.defaultValue ?? ''
  })
  if (props.isResetSearch) onSearch()
}

// 切换展开状�?
const onToggleExpanded = () => {
  isExpanded.value = !isExpanded.value
}
</script>
