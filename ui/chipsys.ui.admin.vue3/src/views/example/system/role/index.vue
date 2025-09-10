<template>
  <div class="system-role-container layout-padding">
    <div class="system-role-padding layout-padding-auto layout-padding-view">
      <div class="system-user-search mb15">
        <el-input v-model="state.tableData.param.search" placeholder="请输入角色名�? style="max-width: 180px"> </el-input>
        <el-button type="primary" class="ml10">
          <el-icon>
            <ele-Search />
          </el-icon>
          查询
        </el-button>
        <el-button type="success" class="ml10" @click="onOpenAddRole('add')">
          <el-icon>
            <ele-FolderAdd />
          </el-icon>
          新增角色
        </el-button>
      </div>
      <el-table :data="state.tableData.data" v-loading="state.tableData.loading" style="width: 100%">
        <el-table-column type="index" label="序号" width="60" />
        <el-table-column prop="roleName" label="角色名称" show-overflow-tooltip></el-table-column>
        <el-table-column prop="roleSign" label="角色标识" show-overflow-tooltip></el-table-column>
        <el-table-column prop="sort" label="排序" show-overflow-tooltip></el-table-column>
        <el-table-column prop="status" label="角色状�? show-overflow-tooltip>
          <template #default="scope">
            <el-tag type="success" v-if="scope.row.status">启用</el-tag>
            <el-tag type="info" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="describe" label="角色描述" show-overflow-tooltip></el-table-column>
        <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>
        <el-table-column label="操作" width="100">
          <template #default="scope">
            <el-button :disabled="scope.row.roleName === '超级管理�?" text type="primary" @click="onOpenEditRole('edit', scope.row)">修改</el-button>
            <el-button :disabled="scope.row.roleName === '超级管理�?" text type="primary" @click="onRowDel(scope.row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        @size-change="onHandleSizeChange"
        @current-change="onHandleCurrentChange"
        class="mt15"
        :pager-count="5"
        :page-sizes="[10, 20, 30]"
        v-model:current-page="state.tableData.param.pageNum"
        background
        v-model:page-size="state.tableData.param.pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="state.tableData.total"
      >
      </el-pagination>
    </div>
    <RoleDialog ref="roleDialogRef" @refresh="getTableData()" />
  </div>
</template>

<script setup lang="ts" name="example/systemRole">
import { defineAsyncComponent, reactive, onMounted, ref } from 'vue'
import { ElMessageBox, ElMessage } from 'element-plus'

// 引入组件
const RoleDialog = defineAsyncComponent(() => import('./dialog.vue'))

// 定义变量内容
const roleDialogRef = ref()
const state = reactive<SysRoleState>({
  tableData: {
    data: [],
    total: 0,
    loading: false,
    param: {
      search: '',
      pageNum: 1,
      pageSize: 10,
    },
  },
})
// 初始化表格数�?
const getTableData = () => {
  state.tableData.loading = true
  const data = []
  for (let i = 0; i < 20; i++) {
    data.push({
      roleName: i === 0 ? '超级管理�? : '普通用�?,
      roleSign: i === 0 ? 'admin' : 'common',
      describe: `测试角色${i + 1}`,
      sort: i,
      status: true,
      createTime: new Date().toLocaleString(),
    })
  }
  state.tableData.data = data
  state.tableData.total = state.tableData.data.length
  setTimeout(() => {
    state.tableData.loading = false
  }, 500)
}
// 打开新增角色弹窗
const onOpenAddRole = (type: string) => {
  roleDialogRef.value.openDialog(type)
}
// 打开修改角色弹窗
const onOpenEditRole = (type: string, row: Object) => {
  roleDialogRef.value.openDialog(type, row)
}
// 删除角色
const onRowDel = (row: RowRoleType) => {
  ElMessageBox.confirm(`此操作将永久删除角色名称：�?{row.roleName}”，是否继续?`, '提示', {
    confirmButtonText: '确认',
    cancelButtonText: '取消',
    type: 'warning',
  })
    .then(() => {
      getTableData()
      ElMessage.success('删除成功')
    })
    .catch(() => {})
}
// 分页改变
const onHandleSizeChange = (val: number) => {
  state.tableData.param.pageSize = val
  getTableData()
}
// 分页改变
const onHandleCurrentChange = (val: number) => {
  state.tableData.param.pageNum = val
  getTableData()
}
// 页面加载�?
onMounted(() => {
  getTableData()
})
</script>

<style scoped lang="scss">
.system-role-container {
  .system-role-padding {
    padding: 15px;
    .el-table {
      flex: 1;
    }
  }
}
</style>
