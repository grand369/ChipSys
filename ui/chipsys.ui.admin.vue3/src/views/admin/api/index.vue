<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="接口名称">
          <el-input v-model="state.filter.name" placeholder="接口名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:api:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
          <el-popconfirm title="确定要同步接�? hide-icon width="180" hide-after="0" @confirm="onSync">
            <template #reference>
              <el-button v-auth="'api:admin:api:sync'" :loading="state.syncLoading" type="primary" icon="ele-Refresh"> 同步 </el-button>
            </template>
          </el-popconfirm>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        :data="state.apiTreeData"
        style="width: 100%"
        v-loading="state.loading"
        row-key="id"
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        :expand-row-keys="state.expandRowKeys"
        border
      >
        <el-table-column prop="label" label="接口名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="path" label="接口地址" min-width="120" show-overflow-tooltip>
          <template #default="{ row }">
            <el-tag v-if="row.httpMethods" :type="getTagTypeByHttpMethod(row.httpMethods)" style="width: 54px">{{ row.httpMethods }}</el-tag>
            {{ row.path }}
          </template>
        </el-table-column>
        <el-table-column label="请求日志" width="90" align="center">
          <template #default="{ row }">
            <el-switch
              v-if="row.httpMethods"
              v-model="row.enabledLog"
              :loading="row.loadingEnabledLog"
              :active-value="true"
              :inactive-value="false"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
              :before-change="() => onSetEnableLog(row)"
            />
          </template>
        </el-table-column>
        <el-table-column label="请求参数" width="90" align="center">
          <template #default="{ row }">
            <el-switch
              v-if="row.httpMethods"
              v-model="row.enabledParams"
              :loading="row.loadingEnabledParams"
              :active-value="true"
              :inactive-value="false"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
              :before-change="() => onSetEnableParams(row)"
            />
          </template>
        </el-table-column>
        <el-table-column label="响应结果" width="90" align="center">
          <template #default="{ row }">
            <el-switch
              v-if="row.httpMethods"
              v-model="row.enabledResult"
              :loading="row.loadingEnabledResult"
              :active-value="true"
              :inactive-value="false"
              inline-prompt
              active-text="启用"
              inactive-text="禁用"
              :before-change="() => onSetEnableResult(row)"
            />
          </template>
        </el-table-column>
        <el-table-column prop="sort" label="排序" width="82" align="center" show-overflow-tooltip />
        <el-table-column label="状�? width="82" align="center">
          <template #default="{ row }">
            <el-tag type="success" v-if="row.enabled">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="160" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:api:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:api:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <api-form ref="apiFormRef" :title="state.apiFormTitle" :api-tree-data="state.formApiTreeData"></api-form>
  </my-layout>
</template>

<script lang="ts" setup name="admin/api">
import { ApiGetListOutput } from '/@/api/admin/data-contracts'
import { ApiApi } from '/@/api/admin/Api'
import { ApiApi as ApiExtApi } from '/@/api/admin.extend/Api'
import { listToTree, treeToList, filterTree, filterList } from '/@/utils/tree'
import { cloneDeep, isArray } from 'lodash-es'
import eventBus from '/@/utils/mitt'

// 引入组件
const ApiForm = defineAsyncComponent(() => import('./components/api-form.vue'))

const { proxy } = getCurrentInstance() as any

const apiFormRef = useTemplateRef('apiFormRef')

const state = reactive({
  loading: false,
  syncLoading: false,
  apiFormTitle: '',
  filter: {
    name: '',
  },
  apiTreeData: [] as Array<ApiGetListOutput>,
  formApiTreeData: [] as Array<ApiGetListOutput>,
  expandRowKeys: [] as string[],
})

onMounted(async () => {
  await onQuery()
  state.expandRowKeys = treeToList(cloneDeep(state.apiTreeData))
    .filter((a: ApiGetListOutput) => a.parentId === 0)
    .map((a: ApiGetListOutput) => a.id + '') as string[]
  eventBus.off('refreshApi')
  eventBus.on('refreshApi', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshApi')
})

const getTagTypeByHttpMethod = (httpMethods: string) => {
  const methods = httpMethods.toLowerCase().split(/\s+/)
  if (methods.some((method) => method === 'get')) {
    return 'success'
  }
  if (methods.some((method) => method === 'delete')) {
    return 'danger'
  }
  if (methods.some((method) => method === 'patch')) {
    return 'info'
  }

  return 'primary'
}
//启用或禁用请求日�?
const onSetEnableLog = (row: ApiGetListOutput & { loadingEnabledLog: boolean; loadingEnabledParams: boolean; loadingEnabledResult: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定�?{row.enabledLog ? '禁用' : '启用'}�?{row.label}】请求参�?`)
      .then(async () => {
        row.loadingEnabledLog = true
        const res = await new ApiApi()
          .setEnableLog({ apiId: row.id, enabledLog: !row.enabledLog }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loadingEnabledLog = false
          })
        if (res && res.success) {
          resolve(true)
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}
//启用或禁用请求参�?
const onSetEnableParams = (row: ApiGetListOutput & { loadingEnabledLog: boolean; loadingEnabledParams: boolean; loadingEnabledResult: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定�?{row.enabledParams ? '禁用' : '启用'}�?{row.label}】请求参�?`)
      .then(async () => {
        row.loadingEnabledParams = true
        const res = await new ApiApi()
          .setEnableParams({ apiId: row.id, enabledParams: !row.enabledParams }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loadingEnabledParams = false
          })
        if (res && res.success) {
          resolve(true)
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}

//启用或禁用响应结�?
const onSetEnableResult = (row: ApiGetListOutput & { loadingEnabledLog: boolean; loadingEnabledParams: boolean; loadingEnabledResult: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定�?{row.enabledResult ? '禁用' : '启用'}�?{row.label}】响应结�?`)
      .then(async () => {
        row.loadingEnabledResult = true
        const res = await new ApiApi()
          .setEnableResult({ apiId: row.id, enabledResult: !row.enabledResult }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loadingEnabledResult = false
          })
        if (res && res.success) {
          resolve(true)
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}

const onQuery = async () => {
  state.loading = true
  const res = await new ApiApi().getList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.apiTreeData = filterTree(listToTree(cloneDeep(res.data)), state.filter.name, {
      filterWhere: (item: any, keyword: string) => {
        return item.label?.toLocaleLowerCase().indexOf(keyword) > -1 || item.path?.toLocaleLowerCase().indexOf(keyword) > -1
      },
    })

    //修复#57
    state.formApiTreeData = listToTree(
      filterList(cloneDeep(res.data), '', {
        filterWhere: (item: any, word: string) => {
          return !item.httpMethods
        },
      })
    )
  } else {
    state.apiTreeData = []
    state.formApiTreeData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.apiFormTitle = '新增接口'
  apiFormRef.value?.open()
}

const onEdit = (row: ApiGetListOutput) => {
  state.apiFormTitle = '编辑接口'
  apiFormRef.value?.open(row)
}

const onDelete = (row: ApiGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除接口�?{row.label}�?`)
    .then(async () => {
      await new ApiApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

const syncApi = async (swaggerResource: any) => {
  const res = await new ApiExtApi().getSwaggerJson(swaggerResource.url, { showErrorMessage: false })
  if (!res) {
    return
  }

  const tags = res.tags
  const paths = res.paths
  const apis = []
  const urls = swaggerResource.url.split('/')
  const code = urls.length >= 2 ? urls[urls.length - 2] : ''
  if (code === '') {
    return
  }
  apis[apis.length] = {
    label: swaggerResource.name,
    path: code,
  }
  // tags
  if (tags && tags.length > 0) {
    tags.forEach((t: any) => {
      apis[apis.length] = {
        label: t.description,
        path: t.name,
        parentPath: code,
      }
    })
  }
  // paths
  if (paths) {
    for (const [key, value] of Object.entries(paths)) {
      const keys = Object.keys(value as any)
      const values = Object.values(value as any)
      const v = values && values.length > 0 ? values[0] : ({} as any)
      const parentPath = v.tags && v.tags.length > 0 ? v.tags[0] : ''
      apis[apis.length] = {
        label: v.summary,
        path: key,
        parentPath,
        httpMethods: keys.join(','),
      }
    }
  }

  return await new ApiApi().sync({ apis })
}

const onSync = async () => {
  state.syncLoading = true
  const swaggerResourcePaths = ['doc/admin']
  //const swaggerResourcePaths = ['doc/admin']
  // const swaggerResourcePaths = ['doc/app']
  const swaggerResourceUrls = swaggerResourcePaths?.map((path) => `/${path}/swagger-resources`) as string[]
  const lastSwaggerResourcesIndex = swaggerResourceUrls.length - 1
  swaggerResourceUrls.forEach(async (swaggerResourceUrl, swaggerResourcesIndex) => {
    const resSwaggerResources = await new ApiExtApi().getSwaggerResources(swaggerResourceUrl, { showErrorMessage: false }).catch(() => {
      state.syncLoading = false
    })
    if (isArray(resSwaggerResources) && (resSwaggerResources?.length as number) > 0) {
      for (let index = 0, len = resSwaggerResources.length; index < len; index++) {
        const swaggerResource = resSwaggerResources[index]
        await syncApi(swaggerResource).catch(() => {
          proxy.$modal.msgSuccess(`同步${swaggerResource.name}失败`)
        })
      }
    }

    if (swaggerResourcesIndex === lastSwaggerResourcesIndex) {
      state.syncLoading = false
      proxy.$modal.msgSuccess(`同步完成`)
      onQuery()
    }
  })
}
</script>

<style scoped lang="scss"></style>
