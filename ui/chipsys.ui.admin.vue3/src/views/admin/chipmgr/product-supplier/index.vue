<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="productId">
          <el-select v-model="state.filterModel.productId" placeholder="产品" clearable class="w200">
            <el-option
              v-for="item in state.productOptions"
              :key="item.id"
              :label="`${item.code} - ${item.brand}`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item prop="supplierId">
          <el-select v-model="state.filterModel.supplierId" placeholder="供应商" clearable class="w200">
            <el-option
              v-for="item in state.supplierOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item prop="condition">
          <el-input v-model="state.filterModel.condition" placeholder="成色" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="supplierModel">
          <el-input v-model="state.filterModel.supplierModel" placeholder="供应商型号" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="currency">
          <el-select v-model="state.filterModel.currency" placeholder="货币" clearable class="w120">
            <el-option label="CNY" value="CNY" />
            <el-option label="USD" value="USD" />
            <el-option label="EUR" value="EUR" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:product-supplier:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.productSupplierListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="productCode" label="产品编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="productBrand" label="产品品牌" min-width="100" show-overflow-tooltip />
        <el-table-column prop="productModel" label="产品型号" min-width="120" show-overflow-tooltip />
        <el-table-column prop="supplierName" label="供应商" min-width="150" show-overflow-tooltip />
        <el-table-column prop="supplierModel" label="供应商型号" min-width="120" show-overflow-tooltip />
        <el-table-column prop="previousPrice" label="之前价格" width="100" align="right">
          <template #default="{ row }">
            <span v-if="row.previousPrice">{{ row.previousPrice }} {{ row.currency }}</span>
            <span v-else>-</span>
          </template>
        </el-table-column>
        <el-table-column prop="currentPrice" label="当前价格" width="100" align="right">
          <template #default="{ row }">
            <span>{{ row.currentPrice }} {{ row.currency }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="condition" label="成色" width="80" show-overflow-tooltip />
        <el-table-column prop="stockQty" label="库存数量" width="100" align="center" />
        <el-table-column prop="leadTimeDays" label="交期(天)" width="100" align="center">
          <template #default="{ row }">
            <span v-if="row.leadTimeDays">{{ row.leadTimeDays }}</span>
            <span v-else>-</span>
          </template>
        </el-table-column>
        <el-table-column prop="moq" label="最小起订量" width="120" align="center" />
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:product-supplier:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:product-supplier:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
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

    <product-supplier-form ref="productSupplierFormRef" :title="state.productSupplierFormTitle"></product-supplier-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/product-supplier">
import { ProductSupplierGetPageOutput, ProductSupplierGetPageInput, ProductGetListOutput, SupplierGetListOutput } from '/@/api/admin/data-contracts'
import { ProductSupplierApi } from '/@/api/admin/ProductSupplier'
import { ProductApi } from '/@/api/admin/Product'
import { SupplierApi } from '/@/api/admin/Supplier'
import eventBus from '/@/utils/mitt'

// 引入组件
const ProductSupplierForm = defineAsyncComponent(() => import('./components/product-supplier-form.vue'))

const { proxy } = getCurrentInstance() as any

const productSupplierFormRef = useTemplateRef('productSupplierFormRef')

const state = reactive({
  loading: false,
  productSupplierFormTitle: '',
  currentPage: 1,
  pageSize: 20,
  total: 0,
  filterModel: {
    productId: undefined as number | undefined,
    supplierId: undefined as number | undefined,
    condition: '',
    supplierModel: '',
    currency: '',
  } as ProductSupplierGetPageInput,
  productSupplierListData: [] as Array<ProductSupplierGetPageOutput>,
  productOptions: [] as Array<ProductGetListOutput>,
  supplierOptions: [] as Array<SupplierGetListOutput>,
})

onMounted(() => {
  onQuery()
  loadProductOptions()
  loadSupplierOptions()
  eventBus.off('refreshProductSupplier')
  eventBus.on('refreshProductSupplier', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshProductSupplier')
})

const loadProductOptions = async () => {
  const res = await new ProductApi().getList({}).catch(() => {})
  if (res && res.data) {
    state.productOptions = res.data
  }
}

const loadSupplierOptions = async () => {
  const res = await new SupplierApi().getList({}).catch(() => {})
  if (res && res.data) {
    state.supplierOptions = res.data
  }
}

const onQuery = async () => {
  state.loading = true
  const res = await new ProductSupplierApi().getPage({
    currentPage: state.currentPage,
    pageSize: state.pageSize,
    filter: state.filterModel
  }).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.productSupplierListData = res.data.list || []
    state.total = res.data.total || 0
  } else {
    state.productSupplierListData = []
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
  state.productSupplierFormTitle = '新增产品供应商关系'
  productSupplierFormRef.value?.open()
}

const onEdit = (row: ProductSupplierGetPageOutput) => {
  state.productSupplierFormTitle = '编辑产品供应商关系'
  productSupplierFormRef.value?.open(row)
}

const onDelete = (row: ProductSupplierGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除该产品供应商关系?`)
    .then(async () => {
      await new ProductSupplierApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
