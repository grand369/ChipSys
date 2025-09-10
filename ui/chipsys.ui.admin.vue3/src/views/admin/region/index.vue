<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form ref="filterFormRef" :model="state.filter" :inline="true" label-width="auto" :label-position="'left'" @submit.stop.prevent>
        <el-form-item label="" prop="parentId">
          <RegionSelect ref="regionSelectRef" v-model:parentId="state.filter.parentId" placeholder="上级地区" />
        </el-form-item>
        <el-form-item label="" prop="name">
          <el-input v-model="state.filter.name" placeholder="地区�? @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="类型" prop="level">
          <el-select v-model="state.filter.level" empty-values="[null]" style="width: 100px" @change="onQuery">
            <el-option label="全部" :value="undefined" />
            <el-option v-for="item in state.regionLevelList" :key="item.label" :label="item.label" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="状�? prop="enabled">
          <el-select v-model="state.filter.enabled" :empty-values="[null]" style="width: 100px" @change="onQuery">
            <el-option v-for="item in state.statusList" :key="item.name" :label="item.name" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="热门" prop="hot">
          <el-select v-model="state.filter.hot" :empty-values="[null]" style="width: 100px" @change="onQuery">
            <el-option v-for="item in state.hotList" :key="item.name" :label="item.name" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button icon="ele-RefreshLeft" text bg @click="onReset"> 重置 </el-button>
          <el-button v-if="auth('api:admin:region:add')" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
          <el-button v-if="auth('api:admin:region:sync-data')" ref="syncRef" :loading="state.sync.loading" type="primary" icon="ele-Refresh">
            同步
          </el-button>
          <el-popover
            v-if="auth('api:admin:region:sync-data')"
            ref="popoverRef"
            :virtual-ref="syncRef"
            trigger="click"
            virtual-triggering
            :width="300"
          >
            <p class="my-flex my-flex-items-center">
              确定要同步数据？
              <!-- 确定要同步至
              <el-select v-model="state.sync.regionLevel"  :teleported="false" style="width: 75px; margin: 0px 5px">
                <el-option v-for="item in state.regionLevelList" :key="item.label" :label="item.label" :value="item.value" />
              </el-select>
              �?-->
            </p>
            <div class="mt10" style="text-align: right">
              <el-button text @click="onSyncCancel">取消</el-button>
              <el-button type="primary" @click="onSync"> 确定 </el-button>
            </div>
          </el-popover>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.dataList" default-expand-all highlight-current-row style="width: 100%" border>
        <el-table-column prop="name" label="地区�? min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="代码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="level" label="类型" min-width="140" show-overflow-tooltip :formatter="formatterEnum" />
        <el-table-column prop="pinyin" label="拼音" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="82" align="center" show-overflow-tooltip />
        <el-table-column label="状�? width="88" align="center" fixed="right">
          <template #default="{ row }">
            <el-switch
              v-if="auth('api:admin:region:set-enable')"
              v-model="row.enabled"
              :loading="row.loading"
              :active-value="true"
              :inactive-value="false"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
              :before-change="() => onSetEnable(row)"
            />
            <template v-else>
              <el-tag type="success" v-if="row.enabled">启用</el-tag>
              <el-tag type="danger" v-else>禁用</el-tag>
            </template>
          </template>
        </el-table-column>
        <el-table-column label="热门" width="88" align="center" fixed="right">
          <template #default="{ row }">
            <el-switch
              v-if="auth('api:admin:region:set-hot')"
              v-model="row.hot"
              :loading="row.hotLoading"
              :active-value="true"
              :inactive-value="false"
              inline-prompt
              active-text="�?
              inactive-text="�?
              :before-change="() => onSetHot(row)"
            />
            <template v-else>
              <el-tag type="success" v-if="row.enabled">�?/el-tag>
              <el-tag type="danger" v-else>�?/el-tag>
            </template>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="160" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:region:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:region:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
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

    <RegionForm ref="formRef" :title="state.formTitle"></RegionForm>
  </my-layout>
</template>

<script lang="ts" setup name="admin/region">
import { PageInputRegionGetPageInput, RegionGetPageOutput, RegionLevel } from '/@/api/admin/data-contracts'
import { RegionLevel as RegionLevelEnum } from '/@/api/admin/enum-contracts'
import { RegionApi } from '/@/api/admin/Region'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { toOptionsByValue, getDescByValue } from '/@/utils/enum'
import type { FormInstance } from 'element-plus'

// 引入组件
const RegionForm = defineAsyncComponent(() => import('./components/region-form.vue'))
const RegionSelect = defineAsyncComponent(() => import('./components/region-select.vue'))

const { proxy } = getCurrentInstance() as any

const regionSelectRef = useTemplateRef('regionSelectRef')
const filterFormRef = useTemplateRef<FormInstance>('filterFormRef')
const formRef = useTemplateRef('formRef')
const syncRef = useTemplateRef('syncRef')
const popoverRef = useTemplateRef('popoverRef')

const state = reactive({
  loading: false,
  sync: {
    loading: false,
    regionLevel: 2 as RegionLevel,
  },
  formTitle: '',
  total: 0,
  statusList: [
    { name: '全部', value: undefined },
    { name: '启用', value: true },
    { name: '禁用', value: false },
  ],
  hotList: [
    { name: '全部', value: undefined },
    { name: '�?, value: true },
    { name: '�?, value: false },
  ],
  regionLevelList: toOptionsByValue(RegionLevelEnum),
  filter: {
    parentId: undefined as number | undefined,
    name: '',
    enabled: undefined,
    hot: undefined,
    level: undefined,
  },
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputRegionGetPageInput,
  dataList: [] as Array<RegionGetPageOutput>,
})

onMounted(async () => {
  await onQuery()

  eventBus.off('refreshRegion')
  eventBus.on('refreshRegion', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshRegion')
})

const formatterEnum = (row: any, column: any, cellValue: any) => {
  return getDescByValue(RegionLevelEnum, cellValue)
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

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new RegionApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.dataList = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0

  state.loading = false
}

const onReset = () => {
  regionSelectRef.value?.reset()
  filterFormRef.value!.resetFields()

  onQuery()
}

const onAdd = () => {
  state.formTitle = '新增地区'
  formRef.value?.open()
}

const onEdit = (row: RegionGetPageOutput) => {
  state.formTitle = '编辑地区'
  formRef.value?.open(row)
}

const onDelete = (row: RegionGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除地区�?{row.name}�?`)
    .then(async () => {
      await new RegionApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

//启用或禁�?
const onSetEnable = (row: RegionGetPageOutput & { loading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定�?{row.enabled ? '禁用' : '启用'}�?{row.name}�?`)
      .then(async () => {
        row.loading = true
        const res = await new RegionApi()
          .setEnable({ regionId: row.id, enabled: !row.enabled }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loading = false
          })
        if (res && res.success) {
          resolve(true)
          onQuery()
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}

//设置热门
const onSetHot = (row: RegionGetPageOutput & { loading: boolean; hotLoading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定�?{row.hot ? '关闭' : '开�?}�?{row.name}】热�?`)
      .then(async () => {
        row.hotLoading = true
        const res = await new RegionApi()
          .setHot({ regionId: row.id, hot: !row.hot }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.hotLoading = false
          })
        if (res && res.success) {
          resolve(true)
          onQuery()
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}

const onSyncCancel = () => {
  popoverRef.value?.hide?.()
}

const onSync = async () => {
  onSyncCancel()
  state.sync.loading = true
  await new RegionApi()
    .syncData(state.sync.regionLevel, { showErrorMessage: false })
    .then(() => {
      proxy.$modal.msgSuccess(`同步完成`)
      onQuery()
    })
    .catch(() => {
      proxy.$modal.msgError(`同步失败`)
    })
    .finally(() => {
      state.sync.loading = false
    })
}
</script>

<style scoped lang="scss"></style>
