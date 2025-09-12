<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.name" placeholder="产品分类名称或编码" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:category:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.categoryListData"
        row-key="id"
        highlight-current-row
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        border
        style="width: 100%"
      >
        <el-table-column prop="name" label="名称" min-width="140" show-overflow-tooltip>
          <template #default="{ row }">
            <el-badge :type="row.enabled ? 'success' : 'info'" is-dot :offset="[0, 12]"></el-badge>
            {{ row.name }}
          </template>
        </el-table-column>
        <el-table-column prop="code" label="编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="description" label="描述" min-width="200" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="70" align="center" show-overflow-tooltip />
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:category:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:category:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
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

    <category-form ref="categoryFormRef" :title="state.categoryFormTitle"></category-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/category">
import { CategoryGetPageOutput, CategoryGetPageInput } from '/@/api/admin/data-contracts'
import { CategoryApi } from '/@/api/admin/Category'
import eventBus from '/@/utils/mitt'
import { listToTree, filterList } from '/@/utils/tree'

// 引入组件
const CategoryForm = defineAsyncComponent(() => import('./components/category-form.vue'))

const { proxy } = getCurrentInstance() as any

const categoryFormRef = useTemplateRef('categoryFormRef')

const state = reactive({
  loading: false,
  categoryFormTitle: '',
  currentPage: 1,
  pageSize: 20,
  total: 0,
  filterModel: {
    name: '',
  } as CategoryGetPageInput,
  categoryListData: [] as Array<CategoryGetPageOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshCategory')
  eventBus.on('refreshCategory', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshCategory')
})

const onQuery = async () => {
  state.loading = true
  const res = await new CategoryApi().getPage({
    currentPage: state.currentPage,
    pageSize: state.pageSize,
    filter: state.filterModel
  }).catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.list && res.data.list.length > 0) {
    state.categoryListData = listToTree(
      state.filterModel.name
        ? filterList(res.data.list, state.filterModel.name, {
            filterWhere: (item: any, filterword: string) => {
              return item.name?.toLocaleLowerCase().indexOf(filterword) > -1
            },
          })
        : res.data.list
    )
    state.total = res.data.total
  } else {
    state.categoryListData = []
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
  state.categoryFormTitle = '新增产品分类'
  categoryFormRef.value?.open()
}

const onEdit = (row: CategoryGetPageOutput) => {
  state.categoryFormTitle = '编辑产品分类'
  categoryFormRef.value?.open(row)
}

const onDelete = (row: CategoryGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new CategoryApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
