<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="productSupplierId">
          <el-input v-model="state.filterModel.productSupplierId" placeholder="产品供应商ID" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="listType">
          <el-select v-model="state.filterModel.listType" placeholder="清单类型" clearable class="w120">
            <el-option label="关注" value="Favorite" />
            <el-option label="询价" value="Inquiry" />
            <el-option label="采购" value="Purchase" />
          </el-select>
        </el-form-item>
        <el-form-item prop="status">
          <el-select v-model="state.filterModel.status" placeholder="状态" clearable class="w120">
            <el-option label="待处理" value="Pending" />
            <el-option label="处理中" value="Processing" />
            <el-option label="已完成" value="Completed" />
            <el-option label="已取消" value="Cancelled" />
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
          <el-button v-auth="'api:admin:user-list:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.userListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="productSupplierId" label="产品供应商ID" width="140" align="center" />
        <el-table-column prop="listType" label="清单类型" width="100" align="center">
          <template #default="{ row }">
            <el-tag v-if="row.listType === 'Favorite'" type="success">关注</el-tag>
            <el-tag v-else-if="row.listType === 'Inquiry'" type="primary">询价</el-tag>
            <el-tag v-else-if="row.listType === 'Purchase'" type="warning">采购</el-tag>
            <el-tag v-else type="info">{{ row.listType }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="100" align="center">
          <template #default="{ row }">
            <el-tag v-if="row.status === 'Pending'" type="warning">待处理</el-tag>
            <el-tag v-else-if="row.status === 'Processing'" type="primary">处理中</el-tag>
            <el-tag v-else-if="row.status === 'Completed'" type="success">已完成</el-tag>
            <el-tag v-else-if="row.status === 'Cancelled'" type="danger">已取消</el-tag>
            <el-tag v-else type="info">{{ row.status }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="创建时间" width="180" align="center">
          <template #default="{ row }">
            {{ formatDate(new Date(row.createdAt), 'YYYY-mm-dd HH:MM:SS') }}
          </template>
        </el-table-column>
        <el-table-column prop="updatedAt" label="更新时间" width="180" align="center">
          <template #default="{ row }">
            {{ formatDate(new Date(row.updatedAt), 'YYYY-mm-dd HH:MM:SS') }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:user-list:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:user-list:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <user-list-form ref="userListFormRef" :title="state.userListFormTitle"></user-list-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/user-list">
import { UserListGetListOutput, UserListGetListInput } from '/@/api/admin/data-contracts'
import { UserListApi } from '/@/api/admin/UserList'
import eventBus from '/@/utils/mitt'
import { formatDate } from '/@/utils/formatTime'

// 引入组件
const UserListForm = defineAsyncComponent(() => import('./components/user-list-form.vue'))

const { proxy } = getCurrentInstance() as any

const userListFormRef = useTemplateRef('userListFormRef')

const state = reactive({
  loading: false,
  userListFormTitle: '',
  filterModel: {
    productSupplierId: undefined as number | undefined,
    listType: '',
    status: '',
    startDate: undefined as Date | undefined,
    endDate: undefined as Date | undefined,
  } as UserListGetListInput,
  userListData: [] as Array<UserListGetListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshUserList')
  eventBus.on('refreshUserList', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshUserList')
})

const onQuery = async () => {
  state.loading = true
  const res = await new UserListApi().getList(state.filterModel).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.userListData = res.data
  } else {
    state.userListData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.userListFormTitle = '新增用户操作清单'
  userListFormRef.value?.open()
}

const onEdit = (row: UserListGetListOutput) => {
  state.userListFormTitle = '编辑用户操作清单'
  userListFormRef.value?.open(row)
}

const onDelete = (row: UserListGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除该用户操作记录?`)
    .then(async () => {
      await new UserListApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
