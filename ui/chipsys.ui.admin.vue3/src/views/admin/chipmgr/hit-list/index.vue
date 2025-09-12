<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="itemId">
          <el-input v-model="state.filterModel.itemId" placeholder="清单条目ID" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="productSupplierId">
          <el-input v-model="state.filterModel.productSupplierId" placeholder="产品供应商ID" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="minConfidence">
          <el-input-number v-model="state.filterModel.minConfidence" placeholder="最小置信度" :min="0" :max="1" :precision="2" />
        </el-form-item>
        <el-form-item prop="maxConfidence">
          <el-input-number v-model="state.filterModel.maxConfidence" placeholder="最大置信度" :min="0" :max="1" :precision="2" />
        </el-form-item>
        <el-form-item prop="startDate">
          <el-date-picker v-model="state.filterModel.startDate" type="datetime" placeholder="开始时间" />
        </el-form-item>
        <el-form-item prop="endDate">
          <el-date-picker v-model="state.filterModel.endDate" type="datetime" placeholder="结束时间" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:hit-list:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.hitListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="itemId" label="清单条目ID" width="120" align="center" />
        <el-table-column prop="productSupplierId" label="产品供应商ID" width="140" align="center" />
        <el-table-column prop="confidence" label="置信度" width="100" align="center">
          <template #default="{ row }">
            <el-progress 
              v-if="row.confidence" 
              :percentage="Math.round(row.confidence * 100)" 
              :color="getConfidenceColor(row.confidence)"
              :show-text="true"
            />
            <span v-else>-</span>
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="创建时间" width="180" align="center">
          <template #default="{ row }">
            {{ formatDate(new Date(row.createdAt), 'YYYY-mm-dd HH:MM:SS') }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:hit-list:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:hit-list:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <hit-list-form ref="hitListFormRef" :title="state.hitListFormTitle"></hit-list-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/hit-list">
import { HitListGetListOutput, HitListGetListInput } from '/@/api/admin/data-contracts'
import { HitListApi } from '/@/api/admin/HitList'
import eventBus from '/@/utils/mitt'
import { formatDate } from '/@/utils/formatTime'

// 引入组件
const HitListForm = defineAsyncComponent(() => import('./components/hit-list-form.vue'))

const { proxy } = getCurrentInstance() as any

const hitListFormRef = useTemplateRef('hitListFormRef')

const state = reactive({
  loading: false,
  hitListFormTitle: '',
  filterModel: {
    itemId: undefined as number | undefined,
    productSupplierId: undefined as number | undefined,
    minConfidence: undefined as number | undefined,
    maxConfidence: undefined as number | undefined,
    startDate: undefined as Date | undefined,
    endDate: undefined as Date | undefined,
  } as HitListGetListInput,
  hitListData: [] as Array<HitListGetListOutput>,
})

const getConfidenceColor = (confidence: number) => {
  if (confidence >= 0.8) return '#67c23a'
  if (confidence >= 0.6) return '#e6a23c'
  return '#f56c6c'
}

onMounted(() => {
  onQuery()
  eventBus.off('refreshHitList')
  eventBus.on('refreshHitList', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshHitList')
})

const onQuery = async () => {
  state.loading = true
  const res = await new HitListApi().getList(state.filterModel).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.hitListData = res.data
  } else {
    state.hitListData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.hitListFormTitle = '新增命中清单'
  hitListFormRef.value?.open()
}

const onEdit = (row: HitListGetListOutput) => {
  state.hitListFormTitle = '编辑命中清单'
  hitListFormRef.value?.open(row)
}

const onDelete = (row: HitListGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除该命中记录?`)
    .then(async () => {
      await new HitListApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
