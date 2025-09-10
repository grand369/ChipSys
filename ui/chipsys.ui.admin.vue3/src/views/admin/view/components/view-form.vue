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
            <el-form-item label="上级视图">
              <el-tree-select
                v-model="form.parentId"
                :data="viewTreeData"
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
            <el-form-item label="视图名称" prop="label" :rules="[{ required: true, message: '请输入视图名�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.label" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="视图命名" prop="path">
              <el-input v-model="form.name" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="视图地址" prop="path">
              <el-input v-model="form.path" clearable>
                <template #prepend>{{ prependPath }}</template>
                <template #append>.vue</template>
              </el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="10" :md="10" :lg="10" :xl="10">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="7" :md="7" :lg="7" :xl="7">
            <el-form-item label="缓存">
              <el-switch v-model="form.cache" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="7" :md="7" :lg="7" :xl="7">
            <el-form-item label="启用">
              <el-switch v-model="form.enabled" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="视图描述" prop="description">
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

<script lang="ts" setup name="admin/view/form">
import { ViewGetListOutput, ViewUpdateInput, DictGetListOutput } from '/@/api/admin/data-contracts'
import { ViewApi } from '/@/api/admin/View'
import { DictApi } from '/@/api/admin/Dict'
import eventBus from '/@/utils/mitt'
import { cloneDeep } from 'lodash-es'
import { FormInstance } from 'element-plus'

defineProps({
  title: {
    type: String,
    default: '',
  },
  viewTreeData: {
    type: Array as PropType<ViewGetListOutput[]>,
    default: () => [],
  },
})

const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

const formRef = useTemplateRef<FormInstance>('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {
    enabled: true,
    cache: true,
  } as ViewUpdateInput,
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
})
const { form } = toRefs(state)

const prependPath = computed(() => {
  return form.value.path && form.value.path.indexOf('layout/') > -1 ? 'src/' : 'src/views/'
})

const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

// 打开对话�?
const open = async (row: ViewUpdateInput = { id: 0, enabled: true, cache: true }) => {
  await getDictList()

  let formData = cloneDeep(row)
  if (row.id > 0) {
    const res = await new ViewApi().get({ id: row.id }, { loading: true })
    if (res?.success) {
      formData = res.data as ViewUpdateInput
      formData.platform = row.platform
    }
  }
  formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
  state.form = formData

  state.showDialog = true
}
// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value?.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    state.form.parentId = state.form.parentId && state.form.parentId > 0 ? state.form.parentId : undefined
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new ViewApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new ViewApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshView')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
