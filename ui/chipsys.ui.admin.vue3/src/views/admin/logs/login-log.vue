<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form ref="filterFormRef" :model="state.filter" :inline="true" label-width="auto" :label-position="'left'" @submit.stop.prevent>
        <el-form-item label="登录账号" prop="createdUserName">
          <el-input v-model="state.filter.createdUserName" placeholder="登录账号" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="登录状�? prop="status">
          <el-select v-model="state.filter.status" :empty-values="[null]" style="width: 120px" @change="onQuery">
            <el-option v-for="status in state.statusList" :key="status.name" :label="status.name" :value="status.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="登录IP" prop="ip">
          <el-input v-model="state.filter.ip" placeholder="登录IP" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="登录时间">
          <MyDateRange
            v-model:startDate="state.filter.addStartTime as string"
            v-model:endDate="state.filter.addEndTime as string"
            :shortcuts="[]"
            style="width: 230px"
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button icon="ele-RefreshLeft" text bg @click="onReset(filterFormRef!)"> 重置 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.loginLogListData" row-key="id" style="width: 100%" border>
        <el-table-column prop="createdUserName" label="登录账号" min-width="150" show-overflow-tooltip>
          <template #default="{ row }">
            <el-badge :type="row.status ? 'success' : 'danger'" is-dot :offset="[0, 12]"></el-badge>
            {{ row.createdUserName }}<br />{{ row.nickName }}
          </template>
        </el-table-column>
        <el-table-column prop="ip" label="登录IP" min-width="150">
          <template #default="{ row }"> {{ row.ip }} {{ row.isp }} </template>
        </el-table-column>
        <el-table-column prop="country" label="登录地区" min-width="150" show-overflow-tooltip>
          <template #default="{ row }"> {{ row.country }} {{ row.province }} {{ row.city }} </template>
        </el-table-column>
        <el-table-column prop="os" label="操作系统" min-width="120" show-overflow-tooltip />
        <el-table-column prop="browser" label="浏览�? min-width="120" show-overflow-tooltip />
        <el-table-column prop="elapsedMilliseconds" label="耗时 ms" min-width="120" />
        <el-table-column prop="msg" label="登录信息" min-width="150" show-overflow-tooltip />
        <el-table-column prop="createdTime" label="登录时间" :formatter="formatterTime" min-width="160" />
      </el-table>
      <div class="my-flex my-flex-end" style="margin-top: 10px">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          background
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>
  </my-layout>
</template>

<script lang="ts" setup name="admin/loginLog">
import { PageInputLoginLogGetPageInput, LoginLogGetPageInput, LoginLogGetPageOutput } from '/@/api/admin/data-contracts'
import { LoginLogApi } from '/@/api/admin/LoginLog'
import dayjs from 'dayjs'
import type { FormInstance } from 'element-plus'

const MyDateRange = defineAsyncComponent(() => import('/@/components/my-date-range/index.vue'))

const filterFormRef = useTemplateRef<FormInstance>('filterFormRef')

const state = reactive({
  loading: false,
  loginLogFormTitle: '',
  filter: {} as LoginLogGetPageInput,
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputLoginLogGetPageInput,
  loginLogListData: [] as Array<LoginLogGetPageOutput>,
  loginLogLogsTitle: '',
  statusList: [
    { name: '全部', value: undefined },
    { name: '成功', value: true },
    { name: '失败', value: false },
  ],
})

onMounted(() => {
  onQuery()
})

const formatterTime = (row: any, column: any, cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new LoginLogApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.loginLogListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onReset = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  state.filter.addStartTime = undefined
  state.filter.addEndTime = undefined
  formEl.resetFields()

  onQuery()
}

const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}
</script>

<style scoped lang="scss"></style>
