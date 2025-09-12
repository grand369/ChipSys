<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.name" placeholder="供应商名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="contactPerson">
          <el-input v-model="state.filterModel.contactPerson" placeholder="联系人" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="phone">
          <el-input v-model="state.filterModel.phone" placeholder="电话" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="email">
          <el-input v-model="state.filterModel.email" placeholder="邮箱" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="rating">
          <el-select v-model="state.filterModel.rating" placeholder="评级" clearable class="w120">
            <el-option label="1星" :value="1" />
            <el-option label="2星" :value="2" />
            <el-option label="3星" :value="3" />
            <el-option label="4星" :value="4" />
            <el-option label="5星" :value="5" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:supplier:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.supplierListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="name" label="供应商名称" min-width="150" show-overflow-tooltip />
        <el-table-column prop="contactPerson" label="联系人" min-width="100" show-overflow-tooltip />
        <el-table-column prop="phone" label="电话" min-width="120" show-overflow-tooltip />
        <el-table-column prop="email" label="邮箱" min-width="150" show-overflow-tooltip />
        <el-table-column prop="address" label="地址" min-width="200" show-overflow-tooltip />
        <el-table-column prop="rating" label="评级" width="80" align="center">
          <template #default="{ row }">
            <el-rate v-model="row.rating" disabled show-score text-color="#ff9900" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:supplier:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:supplier:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
      
      <!-- 分页组件 -->
      <el-pagination
        v-model:current-page="state.currentPage"
        v-model:page-size="state.pageSize"
        :page-sizes="[10, 20, 50, 100]"
        :total="state.total"
        layout="total, sizes, prev, pager, next, jumper"
        @size-change="onSizeChange"
        @current-change="onCurrentChange"
        class="my-pagination"
      />
    </el-card>

    <supplier-form ref="supplierFormRef" :title="state.supplierFormTitle"></supplier-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/supplier">
import { SupplierGetPageOutput, SupplierGetPageInput } from '/@/api/admin/data-contracts'
import { SupplierApi } from '/@/api/admin/Supplier'
import eventBus from '/@/utils/mitt'

// 引入组件
const SupplierForm = defineAsyncComponent(() => import('./components/supplier-form.vue'))

const { proxy } = getCurrentInstance() as any

const supplierFormRef = useTemplateRef('supplierFormRef')

const state = reactive({
  loading: false,
  supplierFormTitle: '',
  currentPage: 1,
  pageSize: 20,
  total: 0,
  filterModel: {
    name: '',
    contactPerson: '',
    phone: '',
    email: '',
    rating: undefined as number | undefined,
  } as SupplierGetPageInput,
  supplierListData: [] as Array<SupplierGetPageOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshSupplier')
  eventBus.on('refreshSupplier', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshSupplier')
})

const onQuery = async () => {
  state.loading = true
  const res = await new SupplierApi().getPage({
    currentPage: state.currentPage,
    pageSize: state.pageSize,
    filter: state.filterModel
  }).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.supplierListData = res.data.list || []
    state.total = res.data.total || 0
  } else {
    state.supplierListData = []
    state.total = 0
  }
  state.loading = false
}

const onSizeChange = (val: number) => {
  state.pageSize = val
  state.currentPage = 1
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.currentPage = val
  onQuery()
}

const onAdd = () => {
  state.supplierFormTitle = '新增供应商'
  supplierFormRef.value?.open()
}

const onEdit = (row: SupplierGetPageOutput) => {
  state.supplierFormTitle = '编辑供应商'
  supplierFormRef.value?.open(row)
}

const onDelete = (row: SupplierGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new SupplierApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
