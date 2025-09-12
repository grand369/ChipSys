<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="1200px"
    >
      <el-table
        v-loading="state.loading"
        :data="state.uploadListItemData"
        row-key="id"
        border
        style="width: 100%"
        max-height="500"
      >
        <el-table-column prop="rawCode" label="原始编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="rawBrand" label="原始品牌" min-width="100" show-overflow-tooltip />
        <el-table-column prop="rawDescription" label="原始描述" min-width="200" show-overflow-tooltip />
        <el-table-column prop="matchStatus" label="匹配状态" width="100" align="center">
          <template #default="{ row }">
            <el-tag v-if="row.matchStatus === 'Matched'" type="success">已匹配</el-tag>
            <el-tag v-else-if="row.matchStatus === 'Unmatched'" type="warning">未匹配</el-tag>
            <el-tag v-else-if="row.matchStatus === 'Partial'" type="info">部分匹配</el-tag>
            <el-tag v-else type="info">{{ row.matchStatus }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="matchedProductCode" label="匹配产品编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="matchedProductBrand" label="匹配产品品牌" min-width="100" show-overflow-tooltip />
        <el-table-column prop="createdAt" label="创建时间" width="180" align="center">
          <template #default="{ row }">
            {{ formatDate(new Date(row.createdAt), 'YYYY-mm-dd HH:MM:SS') }}
          </template>
        </el-table-column>
      </el-table>
      <template #footer>
        <span class="dialog-footer my-flex my-flex-y-center my-flex-between">
          <div>
            <span>共 {{ state.total }} 条记录</span>
          </div>
          <div>
            <el-button @click="onCancel">关 闭</el-button>
          </div>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/upload-list/items">
import { UploadListItemGetListOutput } from '/@/api/admin/data-contracts'
import { UploadListItemApi } from '/@/api/admin/UploadListItem'
import { formatDate } from '/@/utils/formatTime'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const state = reactive({
  showDialog: false,
  loading: false,
  uploadListItemData: [] as Array<UploadListItemGetListOutput>,
  total: 0,
  listId: 0,
})

// 打开对话框
const open = async (listId: number) => {
  state.listId = listId
  state.showDialog = true
  await loadData()
}

const loadData = async () => {
  state.loading = true
  const res = await new UploadListItemApi().getList({ listId: state.listId }).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.uploadListItemData = res.data
    state.total = res.data.length
  } else {
    state.uploadListItemData = []
    state.total = 0
  }
  state.loading = false
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

defineExpose({
  open,
})
</script>
