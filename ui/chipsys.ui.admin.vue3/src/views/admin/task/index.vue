<template>
  <my-layout>
    <div class="my-query-box mt8" style="position: relative">
      <el-card shadow="never" :body-style="{ paddingBottom: '0' }">
        <el-form :inline="true" label-width="auto" :label-position="'left'" @submit.stop.prevent>
          <el-form-item label="任务分组">
            <el-select v-model="state.filter.groupName" :empty-values="[null, undefined]" style="width: 120px" @change="onQuery">
              <el-option v-for="group in state.groupList" :key="group.name" :label="group.name" :value="group.value" />
            </el-select>
          </el-form-item>
          <el-form-item label="任务名称">
            <el-input v-model="state.filter.taskName" placeholder="任务名称" @keyup.enter="onQuery" />
          </el-form-item>
          <el-form-item label="任务状�?>
            <el-select v-model="state.filter.taskStatus" :empty-values="[null]" style="width: 120px" @change="onQuery">
              <el-option v-for="status in state.statusList" :key="status.name" :label="status.name" :value="status.value" />
            </el-select>
          </el-form-item>
          <el-form-item label="创建日期">
            <MyDateRange v-model:startDate="state.filter.startAddTime" v-model:endDate="state.filter.endAddTime" style="width: 230px" />
          </el-form-item>
          <el-form-item>
            <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
            <el-button v-auth="'api:admin:task:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
          </el-form-item>
        </el-form>
        <div
          v-show="rowSelectCount > 0"
          class="my-flex my-flex-items-center pl10"
          style="position: absolute; top: 0; bottom: 0; left: 0; right: 0; background-color: var(--el-bg-color)"
        >
          <el-text class="mx-1"
            >已选中 <el-text class="mx-1" type="primary">{{ rowSelectCount }}</el-text> �?/el-text
          >
          <el-divider direction="vertical" />
          <el-button v-auth="'api:admin:task:run'" icon="ele-Promotion" text type="primary" @click="onBatchRun">执行</el-button>
          <el-divider direction="vertical" />
          <el-button v-auth="'api:admin:task:pause'" icon="ele-CaretRight" text type="primary" @click="onBatchStart">启动</el-button>
          <el-divider direction="vertical" />
          <el-button v-auth="'api:admin:task:resume'" icon="ele-VideoPause" text type="primary" @click="onBatchPause">停止</el-button>
          <el-divider direction="vertical" />
          <el-button v-auth="'api:admin:task:delete'" icon="ele-Delete" text type="danger" @click="onBatchDelete">删除</el-button>

          <el-button size="large" link @click="onClear" style="position: absolute; right: 6px; top: 6px">
            <svgIcon name="ele-Close" size="18"></svgIcon>
          </el-button>
        </div>
      </el-card>
    </div>

    <el-card class="my-fill mt8 el-card-table" shadow="never">
      <el-table ref="tableRef" v-loading="state.loading" :data="state.taskListData" row-key="id" style="width: 100%" border>
        <el-table-column type="selection" width="40" />
        <el-table-column type="index" label="序号" width="62" :index="indexMethod" />
        <el-table-column prop="topic" label="任务名称" min-width="260">
          <template #default="{ row }">
            <div>{{ row.id }}</div>
            <div>{{ row.topic }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="任务状�? width="95">
          <template #default="{ row }">
            <el-tag v-if="row.status === 0 || row.status === 'Running'" disable-transitions>运行�?/el-tag>
            <el-tag v-if="row.status === 1 || row.status === 'Paused'" type="info" disable-transitions>停止</el-tag>
            <el-tag v-if="row.status === 2 || row.status === 'Completed'" type="success" disable-transitions>完成</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="round" label="运行次数" width="90" />
        <el-table-column prop="currentRound" label="当前次数" width="90" />
        <el-table-column prop="errorTimes" label="失败次数" width="90">
          <template #default="{ row }">
            <el-text class="mx-1" type="danger">{{ row.errorTimes }}</el-text>
          </template>
        </el-table-column>
        <el-table-column prop="body" label="任务数据" min-width="260" />
        <el-table-column prop="intervalArgument" label="定时参数" min-width="120">
          <template #default="{ row }">
            <div>{{ formatterInterval(row.interval) }}</div>
            <div>{{ row.intervalArgument }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="创建时间" :formatter="formatterTime" width="110" />
        <el-table-column prop="lastRunTime" label="最后运行时�? :formatter="formatterTime" width="120" />
        <el-table-column label="操作" width="210" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <div class="my-flex">
              <el-button v-auth="'api:admin:task-log:get-page'" icon="ele-Tickets" text type="primary" @click="onShowLogs(row)">日志</el-button>
              <el-button v-auth="'api:admin:task:update'" icon="ele-Edit" text type="primary" @click="onUpdate(row)">修改</el-button>
              <el-button v-auth="'api:admin:task:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
            </div>

            <div class="my-flex">
              <el-button v-auth="'api:admin:task:run'" icon="ele-Promotion" text type="primary" @click="onRun(row)">执行</el-button>
              <el-button v-auth="'api:admin:task:update'" icon="ele-CopyDocument" text type="primary" @click="onCopy(row)"> 复制 </el-button>
              <el-button
                v-if="row.status === 1 || row.status === 'Paused'"
                v-auth="'api:admin:task:pause'"
                icon="ele-CaretRight"
                text
                type="primary"
                @click="onStart(row)"
              >
                启动
              </el-button>
              <el-button
                v-if="row.status === 0 || row.status === 'Running'"
                v-auth="'api:admin:task:resume'"
                icon="ele-VideoPause"
                text
                type="primary"
                @click="onPause(row)"
              >
                停止
              </el-button>
            </div>
          </template>
        </el-table-column>
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

    <task-logs ref="taskLogsRef" :title="state.taskLogsTitle"></task-logs>
    <task-form ref="taskFormRef" :title="state.taskFormTitle"></task-form>
  </my-layout>
</template>

<script lang="ts" setup name="admin/task">
import { TableInstance, ElMessage } from 'element-plus'
import { TaskGetPageOutput, PageInputTaskGetPageInput, TaskStatus, TaskUpdateInput } from '/@/api/admin/data-contracts'
import { TaskApi } from '/@/api/admin/Task'
import dayjs from 'dayjs'
import eventBus from '/@/utils/mitt'
import { cloneDeep } from 'lodash-es'

// 引入组件
const TaskLogs = defineAsyncComponent(() => import('./components/task-logs.vue'))
const TaskForm = defineAsyncComponent(() => import('./components/task-form.vue'))
const MyDateRange = defineAsyncComponent(() => import('/@/components/my-date-range/index.vue'))

const { proxy } = getCurrentInstance() as any

const taskLogsRef = useTemplateRef('taskLogsRef')
const taskFormRef = useTemplateRef('taskFormRef')
const tableRef = useTemplateRef<TableInstance>('tableRef')

const state = reactive({
  loading: false,
  taskFormTitle: '',
  filter: {
    taskName: '',
    groupName: '',
    taskStatus: undefined as TaskStatus | undefined,
    startAddTime: undefined,
    endAddTime: undefined,
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputTaskGetPageInput,
  taskListData: [] as Array<TaskGetPageOutput>,
  taskLogsTitle: '',
  groupList: [{ name: '全部', value: '' }],
  statusList: [
    { name: '全部', value: undefined },
    { name: '运行�?, value: 0 },
    { name: '停止', value: 1 },
    { name: '已完�?, value: 2 },
  ],
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshTask')
  eventBus.on('refreshTask', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshTask')
})

const rowSelectCount = computed(() => {
  return tableRef.value?.getSelectionRows().length as number
})

const taskIds = computed(() => {
  return tableRef.value?.getSelectionRows().map((a: any) => a.id) as string[]
})

const indexMethod = (index: number) => {
  if (state.pageInput.currentPage && state.pageInput.pageSize) return index + 1 + (state.pageInput.currentPage - 1) * state.pageInput.pageSize
  else return index
}

const formatterInterval = (cellValue: any) => {
  let label = ''
  switch (cellValue) {
    case 1:
    case 'SEC':
      label = '按秒触发'
      break
    case 11:
    case 'RunOnDay':
      label = '每天'
      break
    case 12:
    case 'RunOnWeek':
      label = '每周�?
      break
    case 13:
    case 'RunOnMonth':
      label = '每月第几�?
      break
    case 21:
    case 'Custom':
      label = 'Cron表达�?
      break
  }
  return label
}

const formatterTime = (row: any, column: any, cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const onClear = () => {
  tableRef.value?.clearSelection()
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new TaskApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.taskListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.taskFormTitle = '新增任务'
  taskFormRef.value?.open()
}

const onUpdate = (row: TaskGetPageOutput) => {
  state.taskFormTitle = '修改任务'
  taskFormRef.value?.open(row as TaskUpdateInput)
}

const onCopy = (row: TaskGetPageOutput) => {
  state.taskFormTitle = '新增任务'
  var task = cloneDeep(row)
  task.id = null
  taskFormRef.value?.open(task as TaskUpdateInput)
}

// 查看日志
const onShowLogs = (row: TaskGetPageOutput) => {
  state.taskLogsTitle = `${row.topic}${row.id}运行日志`
  taskLogsRef.value?.open(row)
}

const onRun = (row: TaskGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要运行�?{row.topic}】任�?`)
    .then(async () => {
      await new TaskApi().run({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onPause = (row: TaskGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要停止�?{row.topic}】任�?`)
    .then(async () => {
      await new TaskApi().pause({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onStart = (row: TaskGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要启动�?{row.topic}】任�?`)
    .then(async () => {
      await new TaskApi().resume({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onDelete = (row: TaskGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除�?{row.topic}】任�?`)
    .then(async () => {
      await new TaskApi().delete({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const checkRowSelect = () => {
  if (rowSelectCount.value > 0) {
    return true
  } else {
    ElMessage({
      message: '请选择任务再操�?,
      type: 'warning',
    })
    return false
  }
}

const onBatchRun = () => {
  if (!checkRowSelect()) {
    return
  }

  proxy.$modal
    .confirm(`确定要运�?${rowSelectCount.value} 项任�?`)
    .then(async () => {
      await new TaskApi().batchRun(taskIds.value, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onBatchPause = () => {
  if (!checkRowSelect()) {
    return
  }

  proxy.$modal
    .confirm(`确定要停�?${rowSelectCount.value} 项任�?`)
    .then(async () => {
      await new TaskApi().batchPause(taskIds.value, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onBatchStart = () => {
  if (!checkRowSelect()) {
    return
  }

  proxy.$modal
    .confirm(`确定要启�?${rowSelectCount.value} 项任�?`)
    .then(async () => {
      await new TaskApi().batchResume(taskIds.value, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onBatchDelete = () => {
  if (!checkRowSelect()) {
    return
  }

  proxy.$modal
    .confirm(`确定要删�?${rowSelectCount.value} 项任�?`)
    .then(async () => {
      await new TaskApi().batchDelete(taskIds.value, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
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
