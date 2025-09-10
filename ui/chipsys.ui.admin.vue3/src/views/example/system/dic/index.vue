<template>
  <div class="system-dic-container layout-padding">
    <el-card shadow="hover" class="layout-padding-auto">
      <div class="system-user-search mb15">
        <el-input placeholder="请输入字典名�? style="max-width: 180px"> </el-input>
        <el-button type="primary" class="ml10">
          <el-icon>
            <ele-Search />
          </el-icon>
          查询
        </el-button>
        <el-button type="success" class="ml10" @click="onOpenAddDic('add')">
          <el-icon>
            <ele-FolderAdd />
          </el-icon>
          新增字典
        </el-button>
      </div>
      <el-table :data="state.tableData.data" v-loading="state.tableData.loading" style="width: 100%">
        <el-table-column type="index" label="序号" width="50" />
        <el-table-column prop="dicName" label="字典名称" show-overflow-tooltip></el-table-column>
        <el-table-column prop="fieldName" label="字段�? show-overflow-tooltip></el-table-column>
        <el-table-column prop="status" label="字典状�? show-overflow-tooltip>
          <template #default="scope">
            <el-tag type="success" v-if="scope.row.status">启用</el-tag>
            <el-tag type="info" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="describe" label="字典描述" show-overflow-tooltip></el-table-column>
        <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>
        <el-table-column label="操作" width="100">
          <template #default="scope">
            <el-button text type="primary" @click="onOpenEditDic('edit', scope.row)">修改</el-button>
            <el-button text type="primary" @click="onRowDel(scope.row)">删除</el-button>
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
    </el-card>
    <DicDialog ref="dicDialogRef" @refresh="getTableData()" />
  </div>
</template>

<script setup lang="ts" name="example/systemDic">
import { defineAsyncComponent, reactive, onMounted, ref } from 'vue'
import { ElMessageBox, ElMessage } from 'element-plus'

// 引入组件
const DicDialog = defineAsyncComponent(() => import('./dialog.vue'))

// 定义变量内容
const dicDialogRef = ref()
const state = reactive<SysDicState>({
  tableData: {
    data: [],
    total: 0,
    loading: false,
    param: {
      pageNum: 1,
      pageSize: 10,
    },
  },
})

// 初始化表格数�?
const getTableData = () => {
  state.tableData.loading = true
  const data = []
  for (let i = 0; i < 2; i++) {
    data.push({
      dicName: i === 0 ? '角色标识' : '用户性别',
      fieldName: i === 0 ? 'SYS_ROLE' : 'SYS_UERINFO',
      describe: i === 0 ? '这是角色字典' : '这是用户性别字典',
      status: true,
      createTime: new Date().toLocaleString(),
      list: [],
    })
  }
  state.tableData.data = data
  state.tableData.total = state.tableData.data.length
  setTimeout(() => {
    state.tableData.loading = false
  }, 500)
}
// 打开新增字典弹窗
const onOpenAddDic = (type: string) => {
  dicDialogRef.value.openDialog(type)
}
// 打开修改字典弹窗
const onOpenEditDic = (type: string, row: RowDicType) => {
  dicDialogRef.value.openDialog(type, row)
}
// 删除字典
const onRowDel = (row: RowDicType) => {
  ElMessageBox.confirm(`此操作将永久删除字典名称：�?{row.dicName}”，是否继续?`, '提示', {
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
