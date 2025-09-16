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
      <el-form ref="formRef" :model="form" label-width="100px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="收藏类型" prop="favoriteType" :rules="[{ required: true, message: '请选择收藏类型', trigger: ['blur', 'change'] }]">
              <el-select v-model="form.favoriteType" placeholder="请选择收藏类型" class="w100">
                <el-option label="供应商" value="Supplier" />
                <el-option label="产品" value="Product" />
                <el-option label="联系人" value="Contact" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="收藏对象ID" prop="favoriteId" :rules="[{ required: true, message: '请输入收藏对象ID', trigger: ['blur', 'change'] }]">
              <el-input-number v-model="form.favoriteId" :min="1" placeholder="请输入收藏对象ID" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="备注" prop="remark">
              <el-input v-model="form.remark" type="textarea" :rows="3" placeholder="请输入备注信息" />
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
            <el-button @click="onCancel">取 消</el-button>
            <el-button type="primary" @click="onSure" :loading="state.sureLoading">确 定</el-button>
          </div>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="client/member-favorite/form">
import { MemberFavoriteAddInput, MemberFavoriteUpdateInput } from '/@/api/client/data-contracts'
import { MemberFavoriteApi } from '/@/api/client/MemberFavorite'
import eventBus from '/@/utils/mitt'
import { FormInstance } from 'element-plus'
import { getCurrentInstance, reactive, toRefs, useTemplateRef } from 'vue'

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
  form: {} as MemberFavoriteAddInput & MemberFavoriteUpdateInput,
  contiAdd: false,
})

const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  proxy.$modal.loading()
  
  if (row.id > 0) {
    state.contiAdd = false
    const res = await new MemberFavoriteApi().get({ id: row.id }).catch(() => {})

    if (res?.success) {
      state.form = res.data as MemberFavoriteAddInput & MemberFavoriteUpdateInput
    }
  } else {
    state.form = {} as MemberFavoriteAddInput & MemberFavoriteUpdateInput
  }
  
  proxy.$modal.closeLoading()
  state.showDialog = true
}

// 关闭对话框
const close = () => {
  state.showDialog = false
  state.form = {} as MemberFavoriteAddInput & MemberFavoriteUpdateInput
  formRef.value?.resetFields()
}

// 取消
const onCancel = () => {
  close()
}

// 确定
const onSure = async () => {
  if (!formRef.value) return
  
  const valid = await formRef.value.validate().catch(() => false)
  if (!valid) return
  
  state.sureLoading = true
  
  try {
    let res: any
    
    if (state.form.id > 0) {
      // 更新
      res = await new MemberFavoriteApi().update({
        id: state.form.id,
        favoriteType: state.form.favoriteType,
        favoriteId: state.form.favoriteId,
        remark: state.form.remark,
      } as MemberFavoriteUpdateInput)
    } else {
      // 新增
      res = await new MemberFavoriteApi().add({
        favoriteType: state.form.favoriteType,
        favoriteId: state.form.favoriteId,
        remark: state.form.remark,
      } as MemberFavoriteAddInput)
    }
    
    if (res?.success) {
      proxy.$modal.msgSuccess(state.form.id > 0 ? '更新成功' : '新增成功')
      
      if (state.contiAdd && !(state.form.id > 0)) {
        // 连续新增
        state.form = {} as MemberFavoriteAddInput & MemberFavoriteUpdateInput
        formRef.value?.resetFields()
      } else {
        close()
        eventBus.emit('refreshMemberFavoriteList')
      }
    }
  } catch (error) {
    console.error('保存会员收藏失败:', error)
  } finally {
    state.sureLoading = false
  }
}

// 暴露方法
defineExpose({
  open,
  close,
})
</script>
