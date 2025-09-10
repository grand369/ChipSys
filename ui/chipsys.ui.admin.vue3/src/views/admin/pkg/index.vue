<template>
  <MySplitter>
    <el-splitter-panel size="45%" min="30%" max="60%">
      <div class="my-flex-column w100 h100">
        <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item label="套餐�?>
              <el-input v-model="state.filter.pkgName" placeholder="套餐�? @keyup.enter="onQuery" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
              <el-button v-auth="'api:admin:pkg:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table
            ref="pkgTableRef"
            v-loading="state.loading"
            :data="state.pkgData"
            default-expand-all
            highlight-current-row
            style="width: 100%"
            border
            @current-change="onTableCurrentChange"
          >
            <el-table-column prop="name" label="套餐�? min-width="120" show-overflow-tooltip />
            <el-table-column prop="sort" label="排序" width="82" align="center" show-overflow-tooltip />
            <el-table-column label="操作" width="100" fixed="right" header-align="center" align="center">
              <template #default="{ row }">
                <my-dropdown-more
                  v-auths="['api:admin:pkg:set-pkg-permissions', 'api:admin:pkg:update', 'api:admin:pkg:delete']"
                  style="margin-left: 0px"
                >
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item v-if="auth('api:admin:pkg:set-pkg-permissions')" @click="onSetPkgMenu(row)">菜单权限</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:pkg:update')" @click="onEdit(row)">编辑套餐</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:pkg:delete')" @click="onDelete(row)">删除套餐</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </my-dropdown-more>
              </template>
            </el-table-column>
          </el-table>
          <div class="my-flex my-flex-end" style="margin-top: 10px">
            <el-pagination
              v-model:currentPage="state.pageInput.currentPage"
              v-model:page-size="state.pageInput.pageSize"
              :total="state.total"
              :page-sizes="[10, 20, 50, 100]"
              background
              @size-change="onSizeChange"
              @current-change="onCurrentChange"
              layout="total, sizes, prev, pager, next, jumper"
            />
          </div>
        </el-card>
      </div>
    </el-splitter-panel>
    <el-splitter-panel>
      <div class="my-flex-column w100 h100">
        <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item label="企业�?>
              <el-input v-model="state.filter.name" placeholder="企业�? @keyup.enter="onGetPkgTenantList" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onGetPkgTenantList"> 查询 </el-button>
              <el-button v-auth="'api:admin:pkg:add-pkg-tenant'" type="primary" icon="ele-Plus" @click="onAddTenant"> 添加企业 </el-button>
              <el-button v-auth="'api:admin:pkg:remove-pkg-tenant'" type="danger" icon="ele-Delete" @click="onRemoveTenant"> 移除企业 </el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table
            ref="tenantTableRef"
            v-loading="state.tenantListLoading"
            :data="state.tenantData"
            row-key="id"
            style="width: 100%"
            border
            @row-click="onTenantRowClick"
          >
            <el-table-column type="selection" width="55" />
            <el-table-column prop="name" label="企业�? min-width="120" show-overflow-tooltip />
            <el-table-column prop="code" label="企业编码" min-width="120" show-overflow-tooltip />
          </el-table>
          <div class="my-flex my-flex-end" style="margin-top: 10px">
            <el-pagination
              v-model:currentPage="state.tenantPageInput.currentPage"
              v-model:page-size="state.tenantPageInput.pageSize"
              :total="state.tenantTotal"
              :page-sizes="[10, 20, 50, 100]"
              background
              @size-change="onTenantSizeChange"
              @current-change="onTenantCurrentChange"
              layout="total, sizes, prev, pager, next, jumper"
            />
          </div>
        </el-card>
      </div>
    </el-splitter-panel>

    <pkg-form ref="pkgFormRef" :title="state.pkgFormTitle"></pkg-form>
    <tenant-select
      ref="tenantSelectRef"
      :title="`添加�?{state.pkgName}】企业`"
      multiple
      :sure-loading="state.sureLoading"
      @sure="onSureTenant"
    ></tenant-select>
    <set-pkg-menu ref="setPkgMenuRef"></set-pkg-menu>
  </MySplitter>
</template>

<script lang="ts" setup name="admin/pkg">
import {
  PageInputPkgGetPageInput,
  PkgGetPkgTenantListOutput,
  PkgGetPageOutput,
  PkgAddPkgTenantListInput,
  PageInputPkgGetPkgTenantListInput,
  PkgUpdateInput,
} from '/@/api/admin/data-contracts'
import { PkgApi } from '/@/api/admin/Pkg'
import { TableInstance } from 'element-plus'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'

// 引入组件
const PkgForm = defineAsyncComponent(() => import('./components/pkg-form.vue'))
const SetPkgMenu = defineAsyncComponent(() => import('./components/set-pkg-menu.vue'))
const TenantSelect = defineAsyncComponent(() => import('/@/views/admin/tenant/components/tenant-select.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))
const MySplitter = defineAsyncComponent(() => import('/@/components/my-layout/splitter.vue'))

const { proxy } = getCurrentInstance() as any

const pkgTableRef = useTemplateRef('pkgTableRef')
const pkgFormRef = useTemplateRef('pkgFormRef')
const tenantTableRef = useTemplateRef<TableInstance>('tenantTableRef')
const tenantSelectRef = useTemplateRef('tenantSelectRef')
const setPkgMenuRef = useTemplateRef('setPkgMenuRef')

const state = reactive({
  loading: false,
  tenantListLoading: false,
  sureLoading: false,
  pkgFormTitle: '',
  filter: {
    name: '',
    pkgName: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      name: '',
    },
  } as PageInputPkgGetPageInput,
  pkgData: [] as any,
  tenantPageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      pkgId: null,
      tenantName: '',
    },
  } as PageInputPkgGetPkgTenantListInput,
  tenantData: [] as PkgGetPkgTenantListOutput[],
  tenantTotal: 0,
  pkgId: undefined as number | undefined,
  pkgName: '' as string | null | undefined,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshPkg')
  eventBus.on('refreshPkg', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshPkg')
})

const onQuery = async () => {
  state.loading = true
  if (state.pageInput.filter) state.pageInput.filter.name = state.filter.pkgName
  const res = await new PkgApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.pkgData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0

  if (state.pkgData?.length > 0) {
    nextTick(() => {
      pkgTableRef.value!.setCurrentRow(state.pkgData[0])
    })
  }

  state.loading = false
}

const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}

const onTableCurrentChange = (currentRow: PkgGetPageOutput) => {
  if (!currentRow) {
    return
  }

  state.pkgId = currentRow.id
  state.pkgName = currentRow.name
  onGetPkgTenantList()
}

const onAdd = () => {
  state.pkgFormTitle = '新增套餐'
  pkgFormRef.value?.open({ id: 0, enabled: true })
}

const onEdit = (row: PkgGetPageOutput) => {
  state.pkgFormTitle = '编辑套餐'
  pkgFormRef.value?.open(row as PkgUpdateInput)
}

const onDelete = (row: PkgGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除套餐�?{row.name}�?`)
    .then(async () => {
      await new PkgApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

const onGetPkgTenantList = async () => {
  state.tenantListLoading = true
  state.tenantPageInput.filter = { pkgId: state.pkgId, tenantName: state.filter.name }
  const res = await new PkgApi().getPkgTenantPage(state.tenantPageInput).catch(() => {
    state.tenantListLoading = false
  })
  state.tenantListLoading = false
  if (res?.success) {
    state.tenantData = res?.data?.list ?? []
    state.tenantTotal = res?.data?.total ?? 0
  }
}

const onTenantSizeChange = (val: number) => {
  state.tenantPageInput.pageSize = val
  onGetPkgTenantList()
}

const onTenantCurrentChange = (val: number) => {
  state.tenantPageInput.currentPage = val
  onGetPkgTenantList()
}

const onTenantRowClick = (row: PkgGetPkgTenantListOutput) => {
  tenantTableRef.value!.toggleRowSelection(row, undefined)
}

const onAddTenant = () => {
  if (!((state.pkgId as number) > 0)) {
    proxy.$modal.msgWarning('请选择套餐')
    return
  }

  tenantSelectRef.value?.open()
}

const onRemoveTenant = () => {
  if (!((state.pkgId as number) > 0)) {
    proxy.$modal.msgWarning('请选择套餐')
    return
  }

  const selectionRows = tenantTableRef.value!.getSelectionRows() as PkgGetPageOutput[]

  if (!((selectionRows.length as number) > 0)) {
    proxy.$modal.msgWarning('请选择租户')
    return
  }

  proxy.$modal
    .confirm(`确定要移除吗?`)
    .then(async () => {
      const tenantIds = selectionRows?.map((a) => a.id)
      const input = { pkgId: state.pkgId, tenantIds } as PkgAddPkgTenantListInput
      await new PkgApi().removePkgTenant(input, { loading: true })
      onGetPkgTenantList()
    })
    .catch(() => {})
}

const onSureTenant = async (tenants: PkgGetPageOutput[]) => {
  if (!(tenants?.length > 0)) {
    tenantSelectRef.value?.close()
    return
  }

  state.sureLoading = true
  const tenantIds = tenants?.map((a) => a.id)
  const input = { pkgId: state.pkgId, tenantIds } as PkgAddPkgTenantListInput
  await new PkgApi().addPkgTenant(input, { showSuccessMessage: true }).catch(() => {
    state.sureLoading = false
  })
  state.sureLoading = false
  tenantSelectRef.value?.close()
  onGetPkgTenantList()
}

const onSetPkgMenu = (pkg: PkgGetPageOutput) => {
  if (!((pkg?.id as number) > 0)) {
    proxy.$modal.msgWarning('请选择套餐')
    return
  }
  setPkgMenuRef.value?.open(pkg)
}
</script>

<style scoped lang="scss"></style>
