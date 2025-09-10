<template>
  <div class="system-add-dept-container">
    <el-dialog title="新增部门" v-model="state.isShowDialog" width="769px">
      <el-form :model="state.ruleForm" label-width="90px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="上级部门">
              <el-cascader
                :options="state.deptData"
                :props="{ checkStrictly: true, value: 'deptName', label: 'deptName' }"
                placeholder="请选择部门"
                clearable
                class="w100"
                v-model="state.ruleForm.deptLevel"
              >
                <template #default="{ node, data }">
                  <span>{{ data.deptName }}</span>
                  <span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
                </template>
              </el-cascader>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="部门名称">
              <el-input v-model="state.ruleForm.deptName" placeholder="请输入部门名�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="负责�?>
              <el-input v-model="state.ruleForm.person" placeholder="请输入负责人" clearable></el-input>
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
            <el-form-item label="排序">
              <el-input-number v-model="state.ruleForm.sort" :min="0" :max="999" controls-position="right" placeholder="请输入排�? class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="部门状�?>
              <el-switch v-model="state.ruleForm.status" inline-prompt active-text="�? inactive-text="�?></el-switch>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="部门描述">
              <el-input v-model="state.ruleForm.describe" type="textarea" placeholder="请输入部门描�? maxlength="150"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel">�?�?/el-button>
          <el-button type="primary" @click="onSubmit">�?�?/el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts" name="example/systemAddDept">
import { reactive, onMounted } from 'vue'

// 定义变量内容
const state = reactive({
  isShowDialog: false,
  ruleForm: {
    deptLevel: [], // 上级部门
    deptName: '', // 部门名称
    person: '', // 负责�?
    phone: '', // 手机�?
    email: '', // 邮箱
    sort: 0, // 排序
    status: true, // 部门状�?
    describe: '', // 部门描述
  },
  deptData: [] as DeptTreeType[], // 部门数据
})

// 打开弹窗
const openDialog = () => {
  state.isShowDialog = true
}
// 关闭弹窗
const closeDialog = () => {
  state.isShowDialog = false
}
// 取消
const onCancel = () => {
  closeDialog()
}
// 新增
const onSubmit = () => {
  closeDialog()
}
// 初始化部门数�?
const initTableData = () => {
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
// 页面加载�?
onMounted(() => {
  initTableData()
})

// 暴露变量
defineExpose({
  openDialog,
})
</script>
