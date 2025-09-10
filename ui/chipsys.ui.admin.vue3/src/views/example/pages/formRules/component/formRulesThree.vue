<template>
  <div class="form-rules-three-container">
    <el-form :model="state.form" :rules="state.rules" ref="formRulesThreeRef" label-width="100px" class="mt35">
      <el-row :gutter="35">
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="创建用户" prop="createUser">
            <el-input v-model="state.form.createUser" placeholder="请输入创建用�? clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="修改用户" prop="editUser">
            <el-input v-model="state.form.editUser" placeholder="请输入修改用�? clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="所属用�? prop="user">
            <el-input v-model="state.form.user" placeholder="请输入所属用�? clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="所属部�? prop="department">
            <el-input v-model="state.form.department" placeholder="请输入所属部�? clearable></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </div>
</template>

<script setup lang="ts" name="example/pagesFormRulesThree">
import { reactive } from 'vue'

// 定义变量内容
const state = reactive({
  form: { createUser: '', editUser: '', user: '', department: '' },
  rules: {
    createUser: { required: true, message: '请输入创建用�?, trigger: 'blur' },
    editUser: { required: true, message: '请输入修改用�?, trigger: 'blur' },
    user: { required: true, message: '请输入所属用�?, trigger: 'blur' },
    department: { required: true, message: '请输入所属部�?, trigger: 'blur' },
  },
})
</script>
