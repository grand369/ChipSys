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
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="企业名称" prop="name" :rules="[{ required: true, message: '请输入企业名�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="企业编码" prop="code" :rules="[{ required: true, message: '请输入企业编�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.code" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="套餐" prop="pkgIds">
              <el-select
                v-model="form.pkgIds"
                placeholder="请选择套餐"
                clearable
                multiple
                collapse-tags
                collapse-tags-tooltip
                filterable
                class="w100"
              >
                <el-option v-for="item in state.pkgData" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="姓名" prop="realName" :rules="[{ required: true, message: '请输入姓�?, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.realName" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item
              label="手机�?
              prop="phone"
              :rules="[
                { required: true, message: '请输入手机号', trigger: ['blur', 'change'] },
                { validator: testMobile, trigger: ['blur', 'change'] },
              ]"
            >
              <el-input v-model="form.phone" autocomplete="off" maxlength="11" @blur="onBlurMobile" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="账号" prop="userName" :rules="[{ required: true, message: '请输入管理员账号', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.userName" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col v-if="!isUpdate" :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item prop="password" :rules="[{ validator: validatorPwd, trigger: ['blur', 'change'] }]">
              <template #label>
                <div class="my-flex-y-center">
                  密码<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>选填，不填则使用系统默认密码<br />字母+数字+可选特殊字符，长度�?-16之间</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input key="password" v-model="form.password" @input="onInputPwd" show-password autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="邮箱" prop="email" :rules="[{ validator: testEmail, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.email" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="colCount" :md="colCount" :lg="colCount" :xl="colCount">
            <el-form-item label="域名" prop="domain">
              <el-input v-model="form.domain" autocomplete="off" />
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

<script lang="ts" setup name="admin/tenant/form">
import { TenantAddInput, TenantUpdateInput, PkgGetListOutput } from '/@/api/admin/data-contracts'
import { TenantApi } from '/@/api/admin/Tenant'
import { PkgApi } from '/@/api/admin/Pkg'
import { isMobile, testMobile, testEmail } from '/@/utils/test'
import eventBus from '/@/utils/mitt'
import { validatorPwd } from '/@/utils/validators'
import { verifyCnAndSpace } from '/@/utils/toolsValidate'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = useTemplateRef('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  pkgData: [] as PkgGetListOutput[],
  form: {} as TenantAddInput & TenantUpdateInput,
})
const { form } = toRefs(state)

const isUpdate = computed(() => {
  return state.form.id > 0
})

const colCount = computed(() => {
  return isUpdate.value ? 24 : 12
})

const getPkgs = async () => {
  const res = await new PkgApi().getList().catch(() => {
    state.pkgData = []
  })

  state.pkgData = res?.data ?? []
}

// 打开对话�?
const open = async (row: any = {}) => {
  await getPkgs()

  if (row.id > 0) {
    const res = await new TenantApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as TenantAddInput & TenantUpdateInput
    }
  } else {
    state.form = { pkgIds: [] as number[], enabled: true } as TenantAddInput & TenantUpdateInput
  }
  state.showDialog = true
}

// 输入密码
const onInputPwd = (val: string) => {
  state.form.password = verifyCnAndSpace(val)
}

//手机号失去焦�?
const onBlurMobile = () => {
  if (!state.form.userName && state.form.phone && isMobile(state.form.phone)) {
    state.form.userName = state.form.phone
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
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new TenantApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new TenantApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshTenant')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
