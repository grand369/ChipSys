<template>
  <el-dialog
    v-model="state.showDialog"
    destroy-on-close
    :title="innerTitle"
    append-to-body
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="480px"
  >
    <el-form :model="form" ref="formRef" label-width="80px" label-position="top">
      <el-row :gutter="35">
        <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
          <el-form-item label="数据范围">
            <el-select key="dataScope" v-model="form.dataScope" placeholder="请选择" class="w100">
              <el-option v-for="item in state.dataScopeList" :key="item.value" :label="item.label" :value="item.value" />
            </el-select>

            <org-menu
              ref="orgRef"
              show-checkbox
              check-on-click-node
              v-model="form.orgIds"
              :default-checked-keys="form.orgIds"
              class="w100"
              v-show="form.dataScope === 5"
            />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel">�?�?/el-button>
        <el-button type="primary" @click="onSure" :loading="state.sureLoading">�?�?/el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/role/components/set-role-data-scope">
import { RoleGetListOutput, RoleSetDataScopeInput } from '/@/api/admin/data-contracts'
import { RoleApi } from '/@/api/admin/Role'

const OrgMenu = defineAsyncComponent(() => import('/@/views/admin/org/components/org-menu.vue'))

const orgRef = useTemplateRef('orgRef')

const props = defineProps({
  title: {
    type: String,
    default: '',
  },
})

const innerTitle = computed(() => {
  return props.title ? props.title : state.roleName ? `设置�?{state.roleName}】数据权限` : '设置数据权限'
})

const state = reactive({
  showDialog: false,
  loading: false,
  sureLoading: false,
  permissionTreeData: [],
  roleId: 0 as number | undefined,
  roleName: '' as string | undefined | null,
  checkedKeys: [] as number[] | undefined | null,
  form: {} as RoleSetDataScopeInput,
  dataScopeList: [
    { label: '全部', value: 1 },
    { label: '本部门和下级部门', value: 2 },
    { label: '本部�?, value: 3 },
    { label: '本人数据', value: 4 },
    { label: '指定部门', value: 5 },
  ],
})

const { form } = toRefs(state)

// 打开对话�?
const open = async (role: RoleGetListOutput) => {
  state.roleId = role.id
  state.roleName = role.name

  if ((role.id as number) > 0) {
    const res = await new RoleApi().get({ id: role.id }, { loading: true })

    if (res?.success) {
      const data = res.data
      state.form = { roleId: data?.id, dataScope: data?.dataScope, orgIds: data?.orgIds } as RoleSetDataScopeInput

      state.showDialog = true
    }
  }
}

// 关闭对话�?
const close = () => {
  state.showDialog = false
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = async () => {
  state.sureLoading = true
  const res = await new RoleApi().setDataScope(state.form, { showSuccessMessage: true }).catch(() => {
    state.sureLoading = false
  })
  state.sureLoading = false

  if (res?.success) {
    state.showDialog = false
  }
}

defineExpose({
  open,
  close,
})
</script>

<style scoped lang="scss">
:deep(.el-dialog__body) {
  padding: 5px 10px;
}
</style>
