<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="fileName">
          <el-input v-model="state.filterModel.fileName" placeholder="文件名" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="status">
          <el-select v-model="state.filterModel.status" placeholder="状态" clearable class="w120">
            <el-option label="待处理" value="Pending" />
            <el-option label="处理中" value="Processing" />
            <el-option label="已完成" value="Completed" />
            <el-option label="失败" value="Failed" />
          </el-select>
        </el-form-item>
        <el-form-item prop="startDate">
          <el-date-picker v-model="state.filterModel.startDate" type="datetime" placeholder="开始时间" />
        </el-form-item>
        <el-form-item prop="endDate">
          <el-date-picker v-model="state.filterModel.endDate" type="datetime" placeholder="结束时间" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:upload-list:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.uploadListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="fileName" label="文件名" min-width="200" show-overflow-tooltip />
        <el-table-column prop="status" label="状态" width="100" align="center">
          <template #default="{ row }">
            <el-tag v-if="row.status === 'Pending'" type="warning">待处理</el-tag>
            <el-tag v-else-if="row.status === 'Processing'" type="primary">处理中</el-tag>
            <el-tag v-else-if="row.status === 'Completed'" type="success">已完成</el-tag>
            <el-tag v-else-if="row.status === 'Failed'" type="danger">失败</el-tag>
            <el-tag v-else type="info">{{ row.status }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="创建时间" width="180" align="center">
          <template #default="{ row }">
            {{ formatDate(new Date(row.createdAt), 'YYYY-mm-dd HH:MM:SS') }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:upload-list:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button icon="ele-View" text type="info" @click="onViewItems(row)">查看条目</el-button>
            <el-button v-auth="'api:admin:upload-list:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
      
      <!-- 分页组件 -->
      <el-pagination
        v-model:current-page="state.currentPage"
        v-model:page-size="state.pageSize"
        :page-sizes="[10, 20, 50, 100]"
        :total="state.total"
        layout="total, sizes, prev, pager, next, jumper"
        @size-change="onSizeChange"
        @current-change="onCurrentChange"
        class="my-pagination"
      />
    </el-card>

    <upload-list-form ref="uploadListFormRef" :title="state.uploadListFormTitle"></upload-list-form>
    <upload-list-items ref="uploadListItemsRef" :title="state.uploadListItemsTitle"></upload-list-items>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/upload-list">
import { UploadListGetPageOutput, UploadListGetPageInput } from '/@/api/admin/data-contracts'
import { UploadListApi } from '/@/api/admin/UploadList'
import eventBus from '/@/utils/mitt'
import { formatDate } from '/@/utils/formatTime'

// 引入组件
const UploadListForm = defineAsyncComponent(() => import('./components/upload-list-form.vue'))
const UploadListItems = defineAsyncComponent(() => import('./components/upload-list-items.vue'))

const { proxy } = getCurrentInstance() as any

const uploadListFormRef = useTemplateRef('uploadListFormRef')
const uploadListItemsRef = useTemplateRef('uploadListItemsRef')

const state = reactive({
  loading: false,
  uploadListFormTitle: '',
  uploadListItemsTitle: '',
  currentPage: 1,
  pageSize: 20,
  total: 0,
  filterModel: {
    fileName: '',
    status: '',
    startDate: undefined as Date | undefined,
    endDate: undefined as Date | undefined,
  } as UploadListGetPageInput,
  uploadListData: [] as Array<UploadListGetPageOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshUploadList')
  eventBus.on('refreshUploadList', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshUploadList')
})

const onQuery = async () => {
  state.loading = true
  const res = await new UploadListApi().getPage({
    currentPage: state.currentPage,
    pageSize: state.pageSize,
    filter: state.filterModel
  }).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.uploadListData = res.data.list || []
    state.total = res.data.total || 0
  } else {
    state.uploadListData = []
    state.total = 0
  }
  state.loading = false
}

const onSizeChange = (val: number) => {
  state.pageSize = val
  state.currentPage = 1
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.currentPage = val
  onQuery()
}

const onAdd = () => {
  state.uploadListFormTitle = '新增上传清单'
  uploadListFormRef.value?.open()
}

const onEdit = (row: UploadListGetPageOutput) => {
  state.uploadListFormTitle = '编辑上传清单'
  uploadListFormRef.value?.open(row)
}

const onViewItems = (row: UploadListGetPageOutput) => {
  state.uploadListItemsTitle = `查看清单条目 - ${row.fileName}`
  uploadListItemsRef.value?.open(row.id)
}

const onDelete = (row: UploadListGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.fileName}】?`)
    .then(async () => {
      await new UploadListApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
