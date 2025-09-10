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
            <el-form-item label="上级部门" prop="parentId" :rules="[{ required: true, message: '请选择上级部门', trigger: ['change'] }]">
              <el-tree-select
                v-model="form.parentId"
                :data="state.data"
                node-key="id"
                :props="{ label: 'name' }"
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
            <el-form-item label="部门名称" prop="name" :rules="[{ required: true, message: '请输入部门名�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="部门编码" prop="code">
              <el-input v-model="form.code" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="部门�? prop="value">
              <el-input v-model="form.value" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="启用">
              <el-switch v-model="form.enabled" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="说明">
              <el-input v-model="form.description" clearable type="textarea" />
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

<script lang="ts" setup name="admin/org/form">
import { OrgUpdateInput } from '/@/api/admin/data-contracts'
import { OrgApi } from '/@/api/admin/Org'
import eventBus from '/@/utils/mitt'
import { listToTree } from '/@/utils/tree'

const { proxy } = getCurrentInstance() as any

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const formRef = useTemplateRef('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {
    enabled: true,
  } as OrgUpdateInput,
  data: [],
})

const { form } = toRefs(state)

const query = async () => {
  const res = await new OrgApi().getList().catch(() => {})
  if (res && res.data && res.data.length > 0) {
    state.data = listToTree(res.data)
  } else {
    state.data = []
  }
}

// 打开对话�?
const open = async (row: any = {}) => {
  proxy.$modal.loading()
  await query()
  proxy.$modal.closeLoading()

  if (row.id > 0) {
    const res = await new OrgApi().get({ id: row.id }, { loading: true })

    if (res?.success) {
      let formData = res.data as OrgUpdateInput
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
      state.form = formData
    }
  } else {
    state.form = {
      enabled: true,
      parentId: row.parentId > 0 ? row.parentId : undefined,
    } as OrgUpdateInput
  }
  state.showDialog = true
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
      res = await new OrgApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new OrgApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshOrg')
      eventBus.emit('refreshOrgImg')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
