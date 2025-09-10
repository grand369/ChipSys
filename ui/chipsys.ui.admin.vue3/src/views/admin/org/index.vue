<template>
  <MyLayout>
    <el-card v-show="state.showQuery" class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" label-width="auto" @submit.stop.prevent>
        <el-form-item label="部门名称">
          <el-input v-model="state.filter.name" placeholder="部门名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <div class="my-tools-box mb8 my-flex my-flex-between">
        <div>
          <el-button v-show="state.showOrgList" v-auth="'api:admin:org:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </div>
        <div>
          <el-tooltip effect="dark" :content="state.showQuery ? '隐藏查询' : '显示查询'" placement="top">
            <el-button :icon="state.showQuery ? 'ele-ArrowUp' : 'ele-ArrowDown'" circle @click="state.showQuery = !state.showQuery" />
          </el-tooltip>

          <el-tooltip effect="dark" :content="state.showOrgList ? '部门图形' : '部门列表'" placement="top">
            <el-button :icon="state.showOrgList ? 'ele-Share' : 'ele-Grid'" circle @click="onChangeOrgList" />
          </el-tooltip>
        </div>
      </div>
      <el-table
        v-if="state.showOrgList"
        :data="state.data"
        style="width: 100%"
        v-loading="state.loading"
        row-key="id"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        border
      >
        <el-table-column prop="name" label="部门名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="部门编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="value" label="部门�? min-width="82" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="82" align="center" show-overflow-tooltip />
        <el-table-column label="状�? width="82" align="center">
          <template #default="{ row }">
            <el-tag type="success" v-if="row.enabled">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="210" fixed="right" header-align="center" align="left">
          <template #default="{ row }">
            <el-button v-if="auth('api:admin:org:add')" icon="ele-Plus" text type="primary" @click="onAdd(row)"> 新增 </el-button>
            <el-button v-if="auth('api:admin:org:update') && row.parentId > 0" icon="ele-EditPen" text type="primary" @click="onEdit(row)"
              >编辑</el-button
            >
            <el-button v-if="auth('api:admin:org:delete') && row.parentId > 0" icon="ele-Delete" text type="danger" @click="onDelete(row)"
              >删除</el-button
            >
          </template>
        </el-table-column>
      </el-table>
      <OrgImg ref="orgImgRef" v-else></OrgImg>
    </el-card>

    <org-form ref="orgFormRef" :title="state.orgFormTitle"></org-form>
  </MyLayout>
</template>

<script lang="ts" setup name="admin/org">
import { OrgGetListOutput } from '/@/api/admin/data-contracts'
import { OrgApi } from '/@/api/admin/Org'
import { listToTree, filterList } from '/@/utils/tree'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'

// 引入组件
const OrgForm = defineAsyncComponent(() => import('./components/org-form.vue'))
const OrgImg = defineAsyncComponent(() => import('./components/org-img.vue'))

const { proxy } = getCurrentInstance() as any

const orgFormRef = useTemplateRef('orgFormRef')
const orgImgRef = useTemplateRef('orgImgRef')

const state = reactive({
  loading: false,
  orgFormTitle: '',
  filter: {
    name: '',
  },
  data: [] as Array<OrgGetListOutput>,
  showQuery: true,
  showOrgList: true,
})

onMounted(() => {
  Query()
  eventBus.off('refreshOrg')
  eventBus.on('refreshOrg', () => {
    Query()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshOrg')
})

const onChangeOrgList = () => {
  state.showOrgList = !state.showOrgList
  if (state.showOrgList) {
    Query()
  }
}

const onQuery = () => {
  if (state.showOrgList) {
    Query()
  } else {
    orgImgRef.value?.filter(state.filter.name)
  }
}

const Query = async () => {
  state.loading = true
  const res = await new OrgApi().getList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.data = listToTree(
      state.filter.name
        ? filterList(res.data, state.filter.name, {
            filterWhere: (item: any, filterword: string) => {
              return item.name?.toLocaleLowerCase().indexOf(filterword) > -1
            },
          })
        : res.data
    )
  } else {
    state.data = []
  }
  state.loading = false
}

const onAdd = (row: OrgGetListOutput) => {
  state.orgFormTitle = '新增部门'
  orgFormRef.value?.open({ parentId: row.id })
}

const onEdit = (row: OrgGetListOutput) => {
  state.orgFormTitle = '编辑部门'
  orgFormRef.value?.open(row)
}

const onDelete = (row: OrgGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除部门�?{row.name}�?`)
    .then(async () => {
      await new OrgApi().delete({ id: row.id }, { loading: true })
      Query()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
