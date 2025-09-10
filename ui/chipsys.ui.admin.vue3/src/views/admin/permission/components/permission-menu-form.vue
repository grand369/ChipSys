<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="600px"
    >
      <el-form :model="form" ref="formRef" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="所属平�?>
              <el-select v-model="form.platform" disabled placeholder="请选择所属平�? class="w100">
                <el-option v-for="item in state.dictData[DictType.PlatForm.name]" :key="item.code" :label="item.name" :value="item.code" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="上级分组">
              <el-tree-select
                v-model="form.parentId"
                :data="permissionTreeData"
                node-key="id"
                check-strictly
                default-expand-all
                render-after-expand
                fit-input-width
                clearable
                class="w100"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="视图">
              <el-tree-select
                v-model="form.viewId"
                :data="state.viewTreeData"
                node-key="id"
                :props="{ label: 'path' }"
                default-expand-all
                render-after-expand
                fit-input-width
                clearable
                filterable
                :filter-node-method="onViewFilterNode"
                class="w100"
                @current-change="onViewCurrentChange"
              >
                <template #default="{ data }">
                  <span class="my-flex my-flex-between">
                    <span>{{ data.label }}</span>
                    <span class="my-line-1 my-ml-12" :title="data.path">
                      {{ data.path }}
                    </span>
                  </span>
                </template>
              </el-tree-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="名称" prop="label" :rules="[{ required: true, message: '请输入名�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.label" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="路由地址" prop="path" :rules="[{ required: true, message: '请输入路由地址', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.path" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="路由命名">
              <el-input v-model="form.name" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="链接地址">
              <el-input v-model="form.link" clearable placeholder="内嵌/外链链接地址" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="图标" prop="icon">
              <my-select-icon v-model="form.icon" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="24" :lg="24" :xl="24">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="系统">
              <el-switch v-model="form.isSystem" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="启用">
              <el-switch v-model="form.enabled" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="内嵌窗口">
              <el-switch v-model="form.isIframe" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="缓存">
              <el-switch v-model="form.isKeepAlive" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="固定">
              <el-switch v-model="form.isAffix" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="隐藏">
              <el-switch v-model="form.hidden" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="新窗�?>
              <el-switch v-model="form.newWindow" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="链接外显">
              <el-switch v-model="form.external" />
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
  </div>
</template>

<script lang="ts" setup name="admin/permission/permission-menu-form">
import { PermissionGetListOutput, PermissionUpdateMenuInput, ViewGetListOutput, DictGetListOutput } from '/@/api/admin/data-contracts'
import { PermissionApi } from '/@/api/admin/Permission'
import { ViewApi } from '/@/api/admin/View'
import { listToTree } from '/@/utils/tree'
import eventBus from '/@/utils/mitt'
import { DictApi } from '/@/api/admin/Dict'

// 引入组件
const MySelectIcon = defineAsyncComponent(() => import('/@/components/my-select-icon/index.vue'))

/** 字典分类 */
const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

defineProps({
  title: {
    type: String,
    default: '',
  },
  permissionTreeData: {
    type: Array as PropType<PermissionGetListOutput[]>,
    default: () => [],
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = useTemplateRef('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: { enabled: true, isKeepAlive: true } as PermissionUpdateMenuInput,
  viewTreeData: [] as ViewGetListOutput[],
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
})
const { form } = toRefs(state)

const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

const getViews = async (platform: string) => {
  const res = await new ViewApi().getList({ platform: platform })
  if (res?.success && res.data && res.data.length > 0) {
    state.viewTreeData = listToTree(res.data) as ViewGetListOutput[]
  } else {
    state.viewTreeData = []
  }
}

// 打开对话�?
const open = async (
  row: PermissionUpdateMenuInput = {
    id: 0,
    enabled: true,
    isKeepAlive: true,
    icon: 'ele-Memo',
    parentId: undefined,
  },
  isCopy = false
) => {
  proxy.$modal.loading()

  await getDictList()
  await getViews(row.platform as string)

  if (row.id > 0) {
    const res = await new PermissionApi().getMenu({ id: row.id }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      let formData = res.data as PermissionUpdateMenuInput
      formData.platform = row.platform
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
      if (isCopy) formData.id = 0
      state.form = formData
    }
  } else {
    state.form = row
  }

  proxy.$modal.closeLoading()
  state.showDialog = true
}

const onViewFilterNode = (value: string, data: ViewGetListOutput) => {
  if (!value) return true
  return data.label?.indexOf(value) !== -1 || data.path?.indexOf(value) !== -1
}

const onViewCurrentChange = (data: ViewGetListOutput) => {
  if (data) {
    if (!state.form.label) {
      state.form.label = data.label
    }
    if (!state.form.path) {
      state.form.path = '/' + data.path
    }
  }
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    state.form.parentId = state.form.parentId && state.form.parentId > 0 ? state.form.parentId : undefined
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new PermissionApi().updateMenu(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new PermissionApi().addMenu(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshPermission')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.my-ml-12 {
  margin-left: 12px;
}
</style>
