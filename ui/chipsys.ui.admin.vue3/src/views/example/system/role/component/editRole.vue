<template>
  <div class="system-edit-role-container">
    <el-dialog title="修改角色" v-model="state.isShowDialog" width="769px">
      <el-form :model="state.ruleForm" label-width="90px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="角色名称">
              <el-input v-model="state.ruleForm.roleName" placeholder="请输入角色名�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="角色标识">
              <template #label>
                <el-tooltip effect="dark" content="用于 `router/route.ts` meta.roles" placement="top-start">
                  <span>角色标识</span>
                </el-tooltip>
              </template>
              <el-input v-model="state.ruleForm.roleSign" placeholder="请输入角色标�? clearable></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="排序">
              <el-input-number v-model="state.ruleForm.sort" :min="0" :max="999" controls-position="right" placeholder="请输入排�? class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="角色状�?>
              <el-switch v-model="state.ruleForm.status" inline-prompt active-text="�? inactive-text="�?></el-switch>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="角色描述">
              <el-input v-model="state.ruleForm.describe" type="textarea" placeholder="请输入角色描�? maxlength="150"></el-input>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="菜单权限">
              <el-tree
                :data="state.menuData"
                :props="state.menuProps"
                :default-checked-keys="[112, 113]"
                node-key="id"
                show-checkbox
                class="menu-data-tree"
              />
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

<script setup lang="ts" name="example/systemEditRole">
import { reactive } from 'vue'

// 定义变量内容
const state = reactive({
  isShowDialog: false,
  ruleForm: {
    roleName: '', // 角色名称
    roleSign: '', // 角色标识
    sort: 0, // 排序
    status: true, // 角色状�?
    describe: '', // 角色描述
  },
  menuData: [] as TreeType[],
  menuProps: {
    children: 'children',
    label: 'label',
  },
})

// 打开弹窗
const openDialog = (row: RowRoleType) => {
  state.ruleForm = row
  state.isShowDialog = true
  getMenuData()
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
// 获取菜单结构数据
const getMenuData = () => {
  state.menuData = [
    {
      id: 1,
      label: '系统管理',
      children: [
        {
          id: 11,
          label: '菜单管理',
          children: [
            {
              id: 111,
              label: '菜单新增',
            },
            {
              id: 112,
              label: '菜单修改',
            },
            {
              id: 113,
              label: '菜单删除',
            },
            {
              id: 114,
              label: '菜单查询',
            },
          ],
        },
        {
          id: 12,
          label: '角色管理',
          children: [
            {
              id: 121,
              label: '角色新增',
            },
            {
              id: 122,
              label: '角色修改',
            },
            {
              id: 123,
              label: '角色删除',
            },
            {
              id: 124,
              label: '角色查询',
            },
          ],
        },
        {
          id: 13,
          label: '用户管理',
          children: [
            {
              id: 131,
              label: '用户新增',
            },
            {
              id: 132,
              label: '用户修改',
            },
            {
              id: 133,
              label: '用户删除',
            },
            {
              id: 134,
              label: '用户查询',
            },
          ],
        },
      ],
    },
    {
      id: 2,
      label: '权限管理',
      children: [
        {
          id: 21,
          label: '前端控制',
          children: [
            {
              id: 211,
              label: '页面权限',
            },
            {
              id: 212,
              label: '页面权限',
            },
          ],
        },
        {
          id: 22,
          label: '后端控制',
          children: [
            {
              id: 221,
              label: '页面权限',
            },
          ],
        },
      ],
    },
  ]
}

// 暴露变量
defineExpose({
  openDialog,
})
</script>

<style scoped lang="scss">
.system-edit-role-container {
  .menu-data-tree {
    width: 100%;
    border: 1px solid var(--el-border-color);
    border-radius: var(--el-input-border-radius, var(--el-border-radius-base));
    padding: 5px;
  }
}
</style>
