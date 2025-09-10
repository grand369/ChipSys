<template>
  <my-layout>
    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.cacheListData" row-key="id" style="width: 100%" border>
        <el-table-column type="index" width="82" label="#" />
        <el-table-column prop="description" label="缓存�? />
        <el-table-column prop="name" label="键名" />
        <el-table-column prop="value" label="键�? />
        <el-table-column label="操作" width="180" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:cache:clear'" icon="ele-Brush" text type="danger" @click="onClear(row)">清除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </my-layout>
</template>

<script lang="ts" setup name="admin/cache">
import { CacheApi } from '/@/api/admin/Cache'

const { proxy } = getCurrentInstance() as any

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const state = reactive({
  loading: false,
  cacheListData: [] as any,
})

onMounted(() => {
  onQuery()
})

const onQuery = async () => {
  state.loading = true
  const res = await new CacheApi().getList().catch(() => {
    state.loading = false
  })
  state.cacheListData = res?.data ?? []
  state.loading = false
}

const onClear = (row: any) => {
  proxy.$modal
    .confirmDelete(`确定要清除�?{row.description}】缓�?`, { icon: 'ele-Brush' })
    .then(async () => {
      await new CacheApi().clear({ cacheKey: row.value }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
