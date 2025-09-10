<template>
  <div class="form-rules-one-container">
    <el-form :model="state.form" :rules="state.rules" ref="formRulesOneRef" label-width="100px" class="mt35">
      <el-row :gutter="35">
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="姓名" prop="name">
            <el-input v-model="state.form.name" placeholder="请输入姓�? clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="邮箱" prop="email">
            <el-input v-model="state.form.email" placeholder="请输入用户邮�? clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="登陆账户�? prop="autograph">
            <el-input v-model="state.form.autograph" placeholder="请输入登陆账户名" clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="4" class="mb20">
          <el-form-item label="职务" prop="occupation">
            <el-select v-model="state.form.occupation" placeholder="请选择职务" clearable class="w100">
              <el-option label="计算�?/ 互联�?/ 通信" value="1"></el-option>
              <el-option label="生产 / 工艺 / 制�? value="2"></el-option>
              <el-option label="医疗 / 护理 / 制药" value="3"></el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </div>
</template>

<script setup lang="ts" name="example/pagesFormRulesOne">
import { reactive, onMounted } from 'vue'

// 定义父组件传过来的�?
const props = defineProps({
  data: {
    type: Object,
    default: () => {},
  },
})

// 定义变量内容
const state = reactive({
  form: { name: '', email: '', autograph: '', occupation: '' },
  rules: {
    name: { required: true, message: '请输入姓�?, trigger: 'blur' },
    email: { required: true, message: '请输入用户邮�?, trigger: 'blur' },
    autograph: { required: true, message: '请输入登陆账户名', trigger: 'blur' },
    occupation: { required: true, message: '请选择职务', trigger: 'change' },
  },
})

// 赋值回�?
const initForm = () => {
  state.form = props.data as TableRulesOneProps
}
// 页面加载�?
onMounted(() => {
  initForm()
})
</script>
