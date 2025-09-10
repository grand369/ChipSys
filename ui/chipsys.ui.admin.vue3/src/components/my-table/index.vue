<template>
  <el-table ref="tableRef" v-loading="model.loading" :data="showPagination ? paginatedData : model.data" class="my-table-box" v-bind="model.attrs">
    <el-table-column v-for="(item, index) in columns" :key="item.attrs.prop" v-bind="item.attrs">
      <template v-if="item.slot" #default="scope">
        <slot :name="item.slot" v-bind="scope"></slot>
      </template>
    </el-table-column>
  </el-table>

  <div
    v-if="model.pagination.enabled"
    :class="[
      'my-flex',
      {
        'my-flex-start': model.pagination.position === 'left',
        'my-flex-center': model.pagination.position === 'center',
        'my-flex-end': !model.pagination.position || model.pagination.position === 'right',
      },
      'mt10',
      'my-pagination-box',
    ]"
  >
    <el-pagination
      v-model:current-page="model.pagination.currentPage"
      v-model:page-size="model.pagination.pageSize"
      :total="model.pagination.total"
      :page-sizes="model.pagination.pageSizes"
      background
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      layout="total, sizes, prev, pager, next, jumper"
      v-bind="model.pagination.attrs"
    />
  </div>
</template>

<script lang="ts" setup>
import type { TableInstance } from 'element-plus'
import { mergeWith, cloneDeep, isObject, isArray } from 'lodash-es'

// 使用 defineModel 定义整个表格模型
const propsModel = defineModel({
  type: Object,
  required: true,
})

// 默认表格配置
const defaultTableConfig = {
  columns: [] as Array<{
    attrs: Record<string, any>
    slot?: string
    isShow?: boolean
  }>,
  data: [] as any[],
  loading: false,
  attrs: {
    border: true,
    rowKey: 'id',
  },
  pagination: {
    enabled: true,
    position: 'right',
    currentPage: 1,
    pageSize: 10,
    total: 0,
    pageSizes: [10, 20, 50, 100],
    attrs: {},
  },
}

// 创建响应式模�?
const model = ref(cloneDeep(defaultTableConfig))

const emit = defineEmits(['size-change', 'current-change'])

const tableRef = useTemplateRef<TableInstance>('tableRef')

// 计算是否显示分页
const showPagination = computed(() => {
  return model.value.pagination.enabled
})

const columns = computed(() => {
  return model.value.columns.filter((v) => v.isShow)
})

// 计算分页后的数据
const paginatedData = computed(() => {
  if (!showPagination.value) return model.value.data

  const pagination = model.value.pagination
  const start = (pagination.currentPage - 1) * pagination.pageSize
  const end = start + pagination.pageSize
  return model.value.data.slice(start, end)
})

// 处理每页条数变化
const handleSizeChange = (size: number) => {
  model.value.pagination.pageSize = size
  model.value.pagination.currentPage = 1 // 重置到第一�?
  emit('size-change', size)
}

// 处理当前页码变化
const handleCurrentChange = (page: number) => {
  model.value.pagination.currentPage = page
  emit('current-change', page)
}

// 监听父组件传入的 model 变化，深度合�?
watchEffect(() => {
  model.value = mergeWith(cloneDeep(defaultTableConfig), propsModel.value, (objValue, srcValue) => {
    // 特殊处理数组：直接替换而非合并
    if (isArray(objValue)) return srcValue
    // 对象类型继续递归合并
    if (isObject(objValue)) return undefined // 触发默认合并行为
  })
})

// 暴露表格方法
defineExpose({
  tableRef,
  getSelectionRows: () => tableRef.value?.getSelectionRows() || [],
  clearSelection: () => tableRef.value?.clearSelection(),
  toggleRowSelection: (row: any, selected: boolean) => tableRef.value?.toggleRowSelection(row, selected),
  toggleAllSelection: () => tableRef.value?.toggleAllSelection(),
})
</script>

<style scoped lang="scss"></style>
