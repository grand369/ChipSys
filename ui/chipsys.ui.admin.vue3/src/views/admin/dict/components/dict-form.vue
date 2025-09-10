<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="769px"
    >
      <el-form ref="formRef" :model="form" label-width="80px">
        <el-row :gutter="35">
          <el-col v-if="state.isTree" :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="上级分类" prop="parentId">
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
            <el-form-item label="名称" prop="name" :rules="[{ required: true, message: '请输入名�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="编码" prop="code">
              <el-input v-model="form.code" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="字典�? prop="value">
              <el-input v-model="form.value" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="排序" prop="sort">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="启用" prop="enabled">
              <el-switch v-model="form.enabled" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="说明" prop="description">
              <el-input v-model="form.description" clearable type="textarea" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer my-flex my-flex-y-center my-flex-between">
          <div>
            <el-checkbox v-if="!(state.form?.id > 0)" v-model="state.contiAdd">连续新增</el-checkbox>
          </div>
          <div>
            <el-button @click="onCancel">�?�?/el-button>
            <el-button type="primary" @click="onSure" :loading="state.sureLoading">�?�?/el-button>
          </div>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/dict/form">
import { DictAddInput, DictUpdateInput, DictTypeGetListOutput } from '/@/api/admin/data-contracts'
import { DictApi } from '/@/api/admin/Dict'
import eventBus from '/@/utils/mitt'
import { FormInstance } from 'element-plus'
import { listToTree } from '/@/utils/tree'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = useTemplateRef<FormInstance>('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as DictAddInput & DictUpdateInput,
  contiAdd: false,
  data: [],
  isTree: false,
})
const { form } = toRefs(state)

const query = async (dictTypeId: number) => {
  const res = await new DictApi().getAll({ dictTypeId: dictTypeId }).catch(() => {})
  if (res && res.data && res.data.length > 0) {
    state.data = listToTree(res.data)
  } else {
    state.data = []
  }
}

// 打开对话�?
const open = async (row: any = {}, dictType: DictTypeGetListOutput) => {
  proxy.$modal.loading()
  state.isTree = dictType.isTree as boolean
  if (row.id > 0) {
    state.contiAdd = false
    const res = await new DictApi().get({ id: row.id }).catch(() => {})

    if (res?.success) {
      let formData = res.data as DictAddInput & DictUpdateInput
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
      state.form = formData
    }
  } else {
    state.form = { dictTypeId: row.dictTypeId, enabled: true } as DictAddInput & DictUpdateInput
  }

  if (state.isTree) {
    await query(state.form.dictTypeId as number)
  }

  proxy.$modal.closeLoading()

  state.showDialog = true
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value!.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new DictApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new DictApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      if (state.contiAdd) {
        formRef.value!.resetFields()
      }
      eventBus.emit('refreshDict')
      state.showDialog = state.contiAdd
    }
  })
}

defineExpose({
  open,
})
</script>
