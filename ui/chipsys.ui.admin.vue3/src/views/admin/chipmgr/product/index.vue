<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="code">
          <el-input v-model="state.filterModel.code" placeholder="产品编码" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="brand">
          <el-input v-model="state.filterModel.brand" placeholder="品牌" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="manufacturer">
          <el-input v-model="state.filterModel.manufacturer" placeholder="制造商" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="model">
          <el-input v-model="state.filterModel.model" placeholder="产品型号" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="categoryId">
          <el-select v-model="state.filterModel.categoryId" placeholder="产品分类" clearable class="w200">
            <el-option
              v-for="item in state.categoryOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:product:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.productListData"
        row-key="id"
        :default-sort="state.defalutSort"
        border
        style="width: 100%"
        @sort-change="onSortChange"
      >
        <el-table-column prop="code" label="产品编码" min-width="120" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="brand" label="品牌" min-width="100" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="description" label="描述" min-width="200" show-overflow-tooltip />
        <el-table-column prop="manufacturer" label="制造商" min-width="120" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="model" label="产品型号" min-width="120" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="categoryName" label="分类" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="90" align="center" sortable="custom" show-overflow-tooltip />
        <el-table-column label="状态" width="80" align="center">
          <template #default="{ row }">
            <el-tag v-if="row.enabled" type="success">启用</el-tag>
            <el-tag v-else type="info">禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:product:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:product:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <product-form ref="productFormRef" :title="state.productFormTitle"></product-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/product">
import { ProductGetPageOutput, ProductGetPageInput, SortInput, CategoryGetListOutput } from '/@/api/admin/data-contracts'
import { ProductApi } from '/@/api/admin/Product'
import { CategoryApi } from '/@/api/admin/Category'
import eventBus from '/@/utils/mitt'

// 引入组件
const ProductForm = defineAsyncComponent(() => import('./components/product-form.vue'))

const { proxy } = getCurrentInstance() as any

const productFormRef = useTemplateRef('productFormRef')

const defalutSort = { prop: 'sort', order: 'ascending' }

const getSortList = (data: { prop: string; order: any }) => {
  return [
    {
      propName: data.prop,
      order: data.order === 'ascending' ? 0 : data.order === 'descending' ? 1 : undefined,
    },
  ] as [SortInput]
}

const state = reactive({
  loading: false,
  productFormTitle: '',
  defalutSort: defalutSort,
  total: 0,
  currentPage: 1,
  pageSize: 20,
  filterModel: {
    code: '',
    brand: '',
    manufacturer: '',
    model: '',
    categoryId: undefined as number | undefined,
    sortList: getSortList(defalutSort),
  } as ProductGetPageInput,
  productListData: [] as Array<ProductGetPageOutput>,
  categoryOptions: [] as Array<CategoryGetListOutput>,
})

onMounted(() => {
  onQuery()
  loadCategoryOptions()
  eventBus.off('refreshProduct')
  eventBus.on('refreshProduct', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshProduct')
})

const loadCategoryOptions = async () => {
  const res = await new CategoryApi().getList({}).catch(() => {})
  if (res && res.data) {
    state.categoryOptions = res.data
  }
}

const onSortChange = (data: { column: any; prop: string; order: any }) => {
  state.filterModel.sortList = getSortList(data)
  onQuery()
}

const onQuery = async () => {
  state.loading = true
  const input = {
    ...state.filterModel,
    currentPage: state.currentPage,
    pageSize: state.pageSize,
  }
  const res = await new ProductApi().getPage(input).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.productListData = res.data.list || []
    state.total = res.data.total || 0
  } else {
    state.productListData = []
    state.total = 0
  }
  state.loading = false
}

const onAdd = () => {
  state.productFormTitle = '新增产品'
  productFormRef.value?.open()
}

const onEdit = (row: ProductGetPageOutput) => {
  state.productFormTitle = '编辑产品'
  productFormRef.value?.open(row)
}

const onDelete = (row: ProductGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.code}】?`)
    .then(async () => {
      await new ProductApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
