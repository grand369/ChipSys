<template>
  <div class="system-user-dialog-container">
    <el-dialog :title="state.dialog.title" v-model="state.dialog.isShowDialog" width="769px">
      <el-form ref="userDialogFormRef" :model="state.ruleForm" label-width="90px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="账户名称">
              <el-input v-model="state.ruleForm.userName" placeholder="请输入账户名�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="用户昵称">
              <el-input v-model="state.ruleForm.userNickname" placeholder="请输入用户昵�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="关联角色">
              <el-select v-model="state.ruleForm.roleSign" placeholder="请选择" clearable class="w100">
                <el-option label="超级管理�? value="admin"></el-option>
                <el-option label="普通用�? value="common"></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="部门">
              <el-cascader
                :options="state.deptData"
                :props="{ checkStrictly: true, value: 'deptName', label: 'deptName' }"
                placeholder="请选择部门"
                clearable
                class="w100"
                v-model="state.ruleForm.department"
              >
                <template #default="{ node, data }">
                  <span>{{ data.deptName }}</span>
                  <span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
                </template>
              </el-cascader>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="手机�?>
              <el-input v-model="state.ruleForm.phone" placeholder="请输入手机号" clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="邮箱">
              <el-input v-model="state.ruleForm.email" placeholder="请输�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="性别">
              <el-select v-model="state.ruleForm.sex" placeholder="请选择" clearable class="w100">
                <el-option label="�? value="�?></el-option>
                <el-option label="�? value="�?></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="账户密码">
              <el-input v-model="state.ruleForm.password" placeholder="请输�? type="password" clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="账户过期">
              <el-date-picker v-model="state.ruleForm.overdueTime" type="date" placeholder="请选择" class="w100"> </el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="用户状�?>
              <el-switch v-model="state.ruleForm.status" inline-prompt active-text="�? inactive-text="�?></el-switch>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="用户描述">
              <el-input v-model="state.ruleForm.describe" type="textarea" placeholder="请输入用户描�? maxlength="150"></el-input>
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

<script setup lang="ts" name="example/systemUserDialog">
import { reactive, ref } from 'vue'

// 定义子组件向父组件传�?事件
const emit = defineEmits(['refresh'])

// 定义变量内容
const userDialogFormRef = ref()
const state = reactive({
  ruleForm: {
    userName: '', // 账户名称
    userNickname: '', // 用户昵称
    roleSign: '', // 关联角色
    department: [] as string[], // 部门
    phone: '', // 手机�?
    email: '', // 邮箱
    sex: '', // 性别
    password: '', // 账户密码
    overdueTime: '', // 账户过期
    status: true, // 用户状�?
    describe: '', // 用户描述
  },
  deptData: [] as DeptTreeType[], // 部门数据
  dialog: {
    isShowDialog: false,
    type: '',
    title: '',
    submitTxt: '',
  },
})

// 打开弹窗
const openDialog = (type: string, row: RowUserType) => {
  if (type === 'edit') {
    state.ruleForm = row
    state.dialog.title = '修改用户'
    state.dialog.submitTxt = '�?�?
  } else {
    state.dialog.title = '新增用户'
    state.dialog.submitTxt = '�?�?
    // 清空表单，此项需加表单验证才能使�?
    // nextTick(() => {
    // 	userDialogFormRef.value.resetFields();
    // });
  }
  state.dialog.isShowDialog = true
  getMenuData()
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
// 初始化部门数�?
const getMenuData = () => {
  state.deptData.push({
    deptName: 'vueNextAdmin',
    createTime: new Date().toLocaleString(),
    status: true,
    sort: Math.random(),
    describe: '顶级部门',
    id: Math.random(),
    children: [
      {
        deptName: 'IT外包服务',
        createTime: new Date().toLocaleString(),
        status: true,
        sort: Math.random(),
        describe: '总部',
        id: Math.random(),
      },
      {
        deptName: '资本控股',
        createTime: new Date().toLocaleString(),
        status: true,
        sort: Math.random(),
        describe: '分部',
        id: Math.random(),
      },
    ],
  })
}

// 暴露变量
defineExpose({
  openDialog,
})
</script>
