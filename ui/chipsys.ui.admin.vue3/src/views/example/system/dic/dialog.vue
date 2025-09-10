<template>
  <div class="system-dic-dialog-container">
    <el-dialog :title="state.dialog.title" v-model="state.dialog.isShowDialog" width="769px">
      <el-alert title="半成品，交互过于复杂，请自行扩展�? type="warning" :closable="false" class="mb20"> </el-alert>
      <el-form ref="dicDialogFormRef" :model="state.ruleForm" label-width="90px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="字典名称">
              <el-input v-model="state.ruleForm.dicName" placeholder="请输入字典名�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="字段�?>
              <el-input v-model="state.ruleForm.fieldName" placeholder="请输入字段名，拼�?ruleForm.list" clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="字典状�?>
              <el-switch v-model="state.ruleForm.status" inline-prompt active-text="�? inactive-text="�?></el-switch>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-row :gutter="35" v-for="(v, k) in state.ruleForm.list" :key="k">
              <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
                <el-form-item :prop="`list[${k}].label`">
                  <template #label>
                    <el-button type="primary" circle @click="onAddRow" v-if="k === 0">
                      <el-icon>
                        <ele-Plus />
                      </el-icon>
                    </el-button>
                    <el-button type="danger" circle @click="onDelRow(k)" v-else>
                      <el-icon>
                        <ele-Delete />
                      </el-icon>
                    </el-button>
                    <span class="ml10">字段</span>
                  </template>
                  <el-input v-model="v.label" style="width: 100%" placeholder="请输入字段名"> </el-input>
                </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
                <el-form-item label="属�? :prop="`list[${k}].value`">
                  <el-input v-model="v.value" style="width: 100%" placeholder="请输入属性�?> </el-input>
                </el-form-item>
              </el-col>
            </el-row>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="字典描述">
              <el-input v-model="state.ruleForm.describe" type="textarea" placeholder="请输入字典描�? maxlength="150"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel">�?�?/el-button>
          <el-button type="primary" @click="onSubmit">{{ state.dialog.submitTxt }}</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts" name="example/systemDicDialog">
import { reactive, ref } from 'vue'

// 定义子组件向父组件传�?事件
const emit = defineEmits(['refresh'])

// 定义变量内容
const dicDialogFormRef = ref()
const state = reactive({
  ruleForm: {
    dicName: '', // 字典名称
    fieldName: '', // 字段�?
    status: true, // 字典状�?
    list: [] as ListType[], // 子集字段 + 属性�?
    describe: '', // 字典描述
  },
  dialog: {
    isShowDialog: false,
    type: '',
    title: '',
    submitTxt: '',
  },
})

// 打开弹窗
const openDialog = (type: string, row: RowDicType) => {
  if (type === 'edit') {
    if (row.fieldName === 'SYS_UERINFO') {
      row.list = [
        { id: Math.random(), label: 'sex', value: '1' },
        { id: Math.random(), label: 'sex', value: '0' },
      ]
    } else {
      row.list = [
        { id: Math.random(), label: 'role', value: 'admin' },
        { id: Math.random(), label: 'role', value: 'common' },
        { id: Math.random(), label: 'roleName', value: '超级管理�? },
        { id: Math.random(), label: 'roleName', value: '普通用�? },
      ]
    }
    state.ruleForm = row
    state.dialog.title = '修改字典'
    state.dialog.submitTxt = '�?�?
  } else {
    state.dialog.title = '新增字典'
    state.dialog.submitTxt = '�?�?
    // 清空表单，此项需加表单验证才能使�?
    // nextTick(() => {
    // 	dicDialogFormRef.value.resetFields();
    // });
  }
  state.dialog.isShowDialog = true
}
// 关闭弹窗
const closeDialog = () => {
  state.dialog.isShowDialog = false
}
// 取消
const onCancel = () => {
  closeDialog()
}
// 提交
const onSubmit = () => {
  closeDialog()
  emit('refresh')
  // if (state.dialog.type === 'add') { }
}
// 新增�?
const onAddRow = () => {
  state.ruleForm.list.push({
    id: Math.random(),
    label: '',
    value: '',
  })
}
// 删除�?
const onDelRow = (k: number) => {
  state.ruleForm.list.splice(k, 1)
}

// 暴露变量
defineExpose({
  openDialog,
})
</script>
