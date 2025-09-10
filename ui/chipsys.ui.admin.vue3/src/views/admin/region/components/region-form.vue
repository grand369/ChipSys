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
            <el-form-item label="上级地区">
              <RegionSelect v-model="form.parentIdList" v-model:parentId="form.parentId" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="类型" prop="level" :rules="[{ required: true, message: '请选择类型', trigger: ['change'] }]">
              <el-select v-model="form.level" placeholder="请选择类型" class="w100">
                <el-option v-for="item in state.regionLevelList" :key="item.label" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="地区名称" prop="name" :rules="[{ required: true, message: '请输入地区名�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="地区简�?>
              <el-input v-model="form.shortName" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="地区代码" prop="code" :rules="[{ required: true, message: '请输入地区代�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.code" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="驻地">
              <el-input v-model="form.capital" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="人口">
              <el-input v-model="form.population" clearable>
                <template #append>万人</template>
              </el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="面积">
              <el-input v-model="form.area" clearable>
                <template #append>km²</template>
              </el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="区号">
              <el-input v-model="form.areaCode" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="邮编">
              <el-input v-model="form.zipCode" clearable />
            </el-form-item>
          </el-col>

          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="热门">
              <el-switch v-model="form.hot" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="启用">
              <el-switch v-model="form.enabled" />
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

<script lang="ts" setup>
import { RegionUpdateInput, RegionGetOutput } from '/@/api/admin/data-contracts'
import { RegionApi } from '/@/api/admin/Region'
import eventBus from '/@/utils/mitt'
import { RegionLevel as RegionLevelEnum } from '/@/api/admin/enum-contracts'
import { toOptionsByValue } from '/@/utils/enum'

const RegionSelect = defineAsyncComponent(() => import('./region-select.vue'))

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
  regionLevelList: toOptionsByValue(RegionLevelEnum),
  form: {
    enabled: true,
    hot: false,
  } as RegionUpdateInput & RegionGetOutput,
})

const { form } = toRefs(state)

// 打开对话�?
const open = async (row: any = {}) => {
  if (row.id > 0) {
    const res = await new RegionApi().get({ id: row.id }, { loading: true })

    if (res?.success) {
      let formData = res.data as RegionUpdateInput & RegionGetOutput
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
      state.form = formData
    }
  } else {
    state.form = {
      enabled: true,
      hot: false,
    } as RegionUpdateInput & RegionGetOutput
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
      res = await new RegionApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new RegionApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshRegion')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
