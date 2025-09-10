<template>
  <el-dialog
    v-model="state.showDialog"
    destroy-on-close
    :title="innerTitle"
    append-to-body
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="780px"
  >
    <template #header="{ close, titleId, titleClass }">
      <div class="my-header">
        <div :id="titleId" :class="titleClass">
          设置{{ innerTitle }}
          <el-select v-model="state.platform" placeholder="请选择所属平�? style="width: 100px" @change="onQuery">
            <el-option v-for="item in state.dictData[DictType.PlatForm.name]" :key="item.code" :label="item.name" :value="item.code" />
          </el-select>
          菜单权限
        </div>
      </div>
    </template>
    <div>
      <el-tree
        ref="permissionTreeRef"
        :data="state.permissionTreeData"
        node-key="id"
        show-checkbox
        highlight-current
        default-expand-all
        check-on-click-node
        :expand-on-click-node="false"
        :props="{ class: customNodeClass }"
        :default-checked-keys="state.checkedKeys"
      />
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel">�?�?/el-button>
        <el-button type="primary" @click="onSure" :loading="state.sureLoading">�?�?/el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/pkg/components/set-pkg-menu">
import { PkgGetListOutput, PkgSetPkgPermissionsInput, DictGetListOutput, PermissionGetPermissionListOutput } from '/@/api/admin/data-contracts'
import { PkgApi } from '/@/api/admin/Pkg'
import { PermissionApi } from '/@/api/admin/Permission'
import { TreeInstance } from 'element-plus'
import { DictApi } from '/@/api/admin/Dict'
import { PlatformType } from '/@/api/admin.extend/enum-contracts'

/** 字典分类 */
const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

const props = defineProps({
  title: {
    type: String,
    default: '',
  },
})

const innerTitle = computed(() => {
  return props.title ? props.title : state.pkgName ? `设置�?{state.pkgName}】菜单权限` : '设置菜单权限'
})

const state = reactive({
  showDialog: false,
  loading: false,
  sureLoading: false,
  permissionTreeData: [] as PermissionGetPermissionListOutput[],
  pkgId: 0 as number | undefined,
  pkgName: '' as string | undefined | null,
  checkedKeys: [],
  platform: PlatformType.Web.name,
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
})

const { proxy } = getCurrentInstance() as any
const permissionTreeRef = useTemplateRef<TreeInstance>('permissionTreeRef')

const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

const getPkgPermissionList = async () => {
  const res = await new PkgApi().getPkgPermissionList({ pkgId: state.pkgId })
  state.checkedKeys = res?.success ? (res.data as never[]) : []
}

// 打开对话�?
const open = async (pkg: PkgGetListOutput) => {
  await getDictList()
  state.pkgId = pkg.id
  state.pkgName = pkg.name
  proxy.$modal.loading()
  await onQuery()
  await getPkgPermissionList()
  proxy.$modal.closeLoading()
  state.showDialog = true
}

// 关闭对话�?
const close = () => {
  state.showDialog = false
}

const onQuery = async () => {
  state.loading = true

  const res = await new PermissionApi().getPermissionList({ platform: state.platform }).catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.permissionTreeData = res.data
  } else {
    state.permissionTreeData = []
  }

  state.loading = false
}

const customNodeClass = (data: any, node: any) => {
  let isPenultimate = true
  for (const key in data.children) {
    if (data.children[key]?.children?.length ?? 0 > 0) {
      isPenultimate = false
      break
    }
  }
  return data.children?.length > 0 && isPenultimate ? `is-penultimate level${node.level}` : ''
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = async () => {
  state.sureLoading = true
  const permissionIds = permissionTreeRef.value?.getCheckedKeys(true)
  const input = {
    platform: state.platform,
    pkgId: state.pkgId,
    permissionIds: permissionIds,
  } as PkgSetPkgPermissionsInput
  const res = await new PkgApi().setPkgPermissions(input, { showSuccessMessage: true }).catch(() => {
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
:deep(.is-penultimate) {
  .el-tree-node__children {
    padding-left: 65px;
    white-space: pre-wrap;
    line-height: 100%;

    .el-tree-node {
      display: inline-block;
    }

    .el-tree-node__content {
      padding-left: 12px !important;
      padding-right: 12px;

      .el-tree-node__expand-icon.is-leaf {
        display: none;
      }
    }
  }
  &.level1 {
    .el-tree-node__children {
      padding-left: 36px;
    }
  }
  &.level2 {
    .el-tree-node__children {
      padding-left: 54x;
    }
  }
  &.level3 {
    .el-tree-node__children {
      padding-left: 72px;
    }
  }
  &.level4 {
    .el-tree-node__children {
      padding-left: 90px;
    }
  }
}
</style>
