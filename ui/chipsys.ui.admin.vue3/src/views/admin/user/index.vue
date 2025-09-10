<template>
  <MySplitter>
    <el-splitter-panel size="20%" min="200" max="40%">
      <div class="my-flex-column w100 h100">
        <org-menu @node-click="onOrgNodeClick" select-first-node></org-menu>
      </div>
    </el-splitter-panel>
    <el-splitter-panel>
      <div class="my-flex-column w100 h100">
        <el-card v-show="state.showQuery" class="my-search-box mt8" shadow="never">
          <my-search
            :search-items="state.searchItems"
            :display-count="2"
            :col-config="{
              lg: 8,
            }"
            @search="onSearch"
          ></my-search>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <div class="my-tools-box mb8 my-flex my-flex-between">
            <div>
              <el-button v-auth="'api:admin:user:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
              <el-button
                v-auth="'api:admin:user:batch-set-org'"
                type="primary"
                :disabled="!isRowSelect"
                :loading="state.loadingBatchSetOrg"
                @click="onBatchSetOrg"
                >部门转移</el-button
              >
            </div>
            <div>
              <el-tooltip effect="dark" content="高级查询" placement="top">
                <el-button icon="ele-Filter" circle @click="onFilter"> </el-button>
              </el-tooltip>
              <el-tooltip effect="dark" content="回收�? placement="top">
                <el-button v-auth="'api:admin:user:restore'" circle @click="onRecycle">
                  <template #icon>
                    <el-icon>
                      <my-icon name="recycle" color="var(--color)"></my-icon>
                    </el-icon>
                  </template>
                </el-button>
              </el-tooltip>
              <MyColSet v-model="state.tableModel.columns" />
              <el-tooltip effect="dark" :content="state.showQuery ? '隐藏查询' : '显示查询'" placement="top">
                <el-button :icon="state.showQuery ? 'ele-ArrowUp' : 'ele-ArrowDown'" circle @click="state.showQuery = !state.showQuery" />
              </el-tooltip>
            </div>
          </div>
          <MyTable ref="tableRef" v-model="state.tableModel" @size-change="onQuery" @current-change="onQuery">
            <!-- 账号列自定义插槽 -->
            <template #userName="{ row }">
              <el-badge :type="row.online ? 'success' : 'info'" is-dot :offset="[0, 12]"></el-badge>
              {{ row.userName }}
            </template>

            <!-- 姓名列自定义插槽 -->
            <template #name="{ row }">
              <div class="my-flex my-flex-items-center">
                {{ row.name }}
                <el-icon v-if="row.sex === 1 || row.sex === 2" class="ml4">
                  <ele-Male v-if="row.sex === 1" color="#409EFF" />
                  <ele-Female v-else-if="row.sex === 2" color="#F34D37" />
                </el-icon>
                <el-tag v-if="row.isManager" type="success" class="ml4">主管</el-tag>
              </div>
            </template>

            <!-- 状态列自定义插�?-->
            <template #enabled="{ row }">
              <el-switch
                v-if="auth('api:admin:user:set-enable')"
                v-model="row.enabled"
                :loading="row.loading"
                :active-value="true"
                :inactive-value="false"
                inline-prompt
                active-text="启用"
                inactive-text="禁用"
                :before-change="() => onSetEnable(row)"
              />
              <template v-else>
                <el-tag type="success" v-if="row.enabled">启用</el-tag>
                <el-tag type="danger" v-else>禁用</el-tag>
              </template>
            </template>

            <!-- 操作列自定义插槽 -->
            <template #actions="{ row }">
              <el-button v-auth="'api:admin:user:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
              <my-dropdown-more
                v-auths="['api:admin:user:set-manager', 'api:admin:user:reset-password', 'api:admin:user:delete', 'api:admin:user:one-click-login']"
              >
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item v-if="auth('api:admin:user:set-manager')" @click="onSetManager(row)"
                      >{{ row.isManager ? '取消' : '设置' }}主管</el-dropdown-item
                    >
                    <el-dropdown-item v-if="auth('api:admin:user:reset-password')" @click="onResetPwd(row)">重置密码</el-dropdown-item>
                    <el-dropdown-item v-if="auth('api:admin:user:delete')" @click="onDelete(row)">删除用户</el-dropdown-item>
                    <el-dropdown-item v-if="auth('api:admin:user:one-click-login')" @click="onOneClickLogin(row)">一键登�?/el-dropdown-item>
                    <el-dropdown-item v-if="auth('api:admin:user:force-offline')" @click="onForceOffline(row)">强制下线</el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </my-dropdown-more>
            </template>
          </MyTable>
        </el-card>

        <user-form ref="userFormRef" :title="state.userFormTitle"></user-form>
        <UserRecycleDialog ref="userRecycleDialogRef" multiple></UserRecycleDialog>
        <user-update-form ref="userUpdateFormRef" :title="state.userFormTitle"></user-update-form>
        <user-set-org ref="userSetOrgRef" v-model:user-ids="selectionIds"></user-set-org>
        <user-reset-pwd ref="userRestPwdRef" title="提示"></user-reset-pwd>
        <MyHighSearchDialog ref="myHighSearchDialogRef" :fields="state.searchItems" @sure="onFilterSure"></MyHighSearchDialog>
      </div>
    </el-splitter-panel>
  </MySplitter>
</template>

<script lang="ts" setup>
import { UserGetPageOutput, PageInputUserGetPageInput, OrgGetListOutput, UserSetManagerInput, UserUpdateInput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { useUserInfo } from '/@/stores/userInfo'
import { Session } from '/@/utils/storage'
import { TableInstance } from 'element-plus'
import { Sex } from '/@/api/admin/enum-contracts'
import { toOptionsByValue } from '/@/utils/enum'
import { Operator } from '/@/api/admin.extend/enum-contracts'

defineOptions({
  name: 'admin/user',
})

// 引入组件
const UserForm = defineAsyncComponent(() => import('./components/user-form.vue'))
const UserRecycleDialog = defineAsyncComponent(() => import('./components/user-recycle-dialog.vue'))
const UserUpdateForm = defineAsyncComponent(() => import('./components/user-update-form.vue'))
const UserSetOrg = defineAsyncComponent(() => import('./components/user-set-org.vue'))
const UserResetPwd = defineAsyncComponent(() => import('./components/user-reset-pwd.vue'))
const OrgMenu = defineAsyncComponent(() => import('/@/views/admin/org/components/org-menu.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))
const MySplitter = defineAsyncComponent(() => import('/@/components/my-layout/splitter.vue'))
const MyHighSearchDialog = defineAsyncComponent(() => import('/@/components/my-high-search/dialog.vue'))

const { proxy } = getCurrentInstance() as any

const tableRef = useTemplateRef<TableInstance>('tableRef')
const userFormRef = useTemplateRef('userFormRef')
const userRecycleDialogRef = useTemplateRef('userRecycleDialogRef')
const userUpdateFormRef = useTemplateRef('userUpdateFormRef')
const userSetOrgRef = useTemplateRef('userSetOrgRef')
const userRestPwdRef = useTemplateRef('userRestPwdRef')
const myHighSearchDialogRef = useTemplateRef('myHighSearchDialogRef')

const storesUseUserInfo = useUserInfo()

const state = reactive({
  loading: false,
  loadingBatchSetOrg: false,
  showQuery: true,
  userFormTitle: '',
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      orgId: null,
    },
  } as PageInputUserGetPageInput,
  userListData: [] as Array<UserGetPageOutput>,
  checkAll: false,
  checkIndeterminate: false,
  // 表格模型
  tableModel: {
    columns: [
      { attrs: { type: 'selection', prop: '_multiCheck', label: '多�? }, isShow: true },
      {
        attrs: {
          prop: 'userName',
          label: '账号',
          minWidth: 180,
          showOverflowTooltip: true,
        },
        slot: 'userName',
        isShow: true,
      },
      {
        attrs: {
          prop: 'name',
          label: '姓名',
          width: 130,
          showOverflowTooltip: true,
        },
        slot: 'name',
        isShow: true,
      },
      { attrs: { prop: 'mobile', label: '手机�?, width: 120, showOverflowTooltip: true }, isShow: true },
      { attrs: { prop: 'orgPaths', label: '部门', minWidth: 200, showOverflowTooltip: true }, isShow: true },
      { attrs: { prop: 'orgPath', label: '主属部门', minWidth: 180, showOverflowTooltip: true }, isShow: true },
      { attrs: { prop: 'roleNames', label: '角色', minWidth: 180, showOverflowTooltip: true }, isShow: true },
      { attrs: { prop: 'email', label: '邮箱', minWidth: 180, showOverflowTooltip: true }, isShow: true },
      {
        attrs: {
          prop: 'enabled',
          label: '状�?,
          width: 88,
          align: 'center',
          fixed: 'right',
        },
        slot: 'enabled',
        isShow: true,
      },
      {
        attrs: {
          prop: '_actions',
          label: '操作',
          width: 140,
          headerAlign: 'center',
          align: 'center',
          fixed: 'right',
        },
        slot: 'actions',
        isShow: true,
      },
    ] as Array<{ attrs: Record<string, any>; slot?: string; isShow?: boolean }>,
    data: [] as Array<UserGetPageOutput>,
    loading: false,
    pagination: {
      currentPage: 1,
      pageSize: 20,
      total: 0,
    },
  },
  searchItems: [
    {
      label: '姓名',
      field: 'name',
      operator: Operator.contains.value,
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入姓�?,
      },
    },
    {
      label: '状�?,
      field: 'enabled',
      operator: Operator.equal.value,
      componentName: 'el-select',
      type: 'select',
      attrs: {
        placeholder: '请选择',
        options: [
          {
            label: '启用',
            value: 1,
          },
          {
            label: '禁用',
            value: 0,
          },
        ],
      },
    },
    {
      label: '手机�?,
      field: 'mobile',
      operator: Operator.contains.value,
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入手机号',
      },
    },
    {
      label: '邮箱',
      field: 'email',
      operator: Operator.contains.value,
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入邮�?,
      },
    },
    {
      label: '性别',
      field: 'staff.sex',
      operator: Operator.equal.value,
      componentName: 'el-select',
      type: 'select',
      attrs: {
        placeholder: '请选择',
        options: toOptionsByValue(Sex),
      },
    },
    {
      label: '创建时间',
      field: 'createdTime',
      operator: Operator.dateRange.value,
      componentName: 'el-date-picker',
      type: 'date',
      attrs: {
        type: 'daterange',
        format: 'YYYY-MM-DD',
        valueFormat: 'YYYY-MM-DD',
        unlinkPanels: true,
        startPlaceholder: '开始时�?,
        endPlaceholder: '结束时间',
        disabledDate: (time: any) => {
          return time.getTime() > Date.now()
        },
      },
    },
    {
      label: '账号',
      field: 'userName',
      operator: Operator.contains.value,
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入账�?,
      },
    },
  ],
})

const selectionRows = computed(() => {
  return tableRef.value?.getSelectionRows()
})

const rowSelectCount = computed(() => {
  return selectionRows.value?.length || 0
})

const isRowSelect = computed(() => {
  return rowSelectCount.value > 0
})

const selectionIds = computed(() => {
  return selectionRows.value?.map((a: any) => a.id)
})

onMounted(() => {
  eventBus.off('refreshUser')
  eventBus.on('refreshUser', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshUser')
})

//查询分页
const onQuery = async () => {
  state.tableModel.loading = true

  // 直接从表格模型中获取分页参数
  state.pageInput.currentPage = state.tableModel.pagination.currentPage
  state.pageInput.pageSize = state.tableModel.pagination.pageSize

  const res = await new UserApi().getPage(state.pageInput).catch(() => {
    state.tableModel.loading = false
  })

  state.tableModel.data = res?.data?.list ?? []
  state.tableModel.pagination.total = res?.data?.total ?? 0
  state.tableModel.loading = false
}

//查询
const onSearch = (filter: any, dynamicFilter: any) => {
  state.pageInput.dynamicFilter = dynamicFilter
  onQuery()
}

//高级查询
const onFilter = () => {
  myHighSearchDialogRef.value?.open()
}
//高级查询确定
const onFilterSure = (dynamicFilter: any) => {
  state.pageInput.dynamicFilter = dynamicFilter
  onQuery()
}

//新增
const onAdd = () => {
  state.userFormTitle = '新增用户'
  userFormRef.value?.open({} as any)
}

//回收�?
const onRecycle = () => {
  userRecycleDialogRef.value?.open()
}

//修改
const onEdit = (row: UserGetPageOutput) => {
  state.userFormTitle = '编辑用户'
  userUpdateFormRef.value?.open(row as UserUpdateInput)
}

//删除
const onDelete = (row: UserGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除�?{row.name}�?`)
    .then(async () => {
      await new UserApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

//重置密码
const onResetPwd = (row: UserGetPageOutput) => {
  userRestPwdRef.value?.open(row)
}

//设置或取消主�?
const onSetManager = (row: UserGetPageOutput) => {
  if (!((state.pageInput.filter?.orgId as number) > 0)) {
    proxy.$modal.msgWarning('请选择部门')
    return
  }

  const title = row.isManager ? `确定要取消�?{row.name}】的主管?` : `确定要设置�?{row.name}】为主管?`
  proxy.$modal
    .confirm(title)
    .then(async () => {
      const input = { userId: row.id, orgId: state.pageInput.filter?.orgId, isManager: !row.isManager } as UserSetManagerInput
      await new UserApi().setManager(input, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

//启用或禁�?
const onSetEnable = (row: UserGetPageOutput & { loading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定�?{row.enabled ? '禁用' : '启用'}�?{row.name}�?`)
      .then(async () => {
        row.loading = true
        const res = await new UserApi()
          .setEnable({ userId: row.id, enabled: !row.enabled }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loading = false
          })
        if (res && res.success) {
          resolve(true)
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}

//一键登�?
const onOneClickLogin = (row: UserGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要一键登录�?{row.name}�?`)
    .then(async () => {
      const res = await new UserApi().oneClickLogin({ userName: row.userName || '' }, { loading: true })
      if (res?.success) {
        proxy.$modal.msgSuccess('一键登录成�?)
        window.requests = []
        Session.remove('tagsViewList')
        storesUseUserInfo.setTokenInfo(res.data)
        window.location.href = '/'
      }
    })
    .catch(() => {})
}

//强制下线
const onForceOffline = (row: UserGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要强制下线�?{row.name}�?`)
    .then(async () => {
      const res = await new UserApi().forceOffline({ id: row.id }, { loading: true })
      if (res?.success) {
        proxy.$modal.msgSuccess('强制下线成功')
        onQuery()
      }
    })
    .catch(() => {})
}

//部门转移
const onBatchSetOrg = () => {
  userSetOrgRef.value?.open()
}

//选择部门
const onOrgNodeClick = (node: OrgGetListOutput | null) => {
  if (state.pageInput.filter) {
    state.pageInput.filter.orgId = node?.id
  }
  onQuery()
}
</script>

<style scoped lang="scss"></style>
